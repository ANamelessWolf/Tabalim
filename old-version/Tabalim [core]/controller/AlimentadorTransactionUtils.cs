using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.controller
{
    public static partial class TransactionUtils
    {
        public static void CreateAlimentadorTr(this TabalimApp app, AlimInput input, Destination destination, Action<Object,int> alimoAddedTask = null)
        {
            input.CreateAlimentadorTr(
                (Object qResult) =>
                {
                    var result = (Object[])qResult;
                    AlimInput alim = (AlimInput)result[1];
                    //Si se agregá el tablero alimentador
                    if ((Boolean)result[0] && alim != null)
                    {
                        switch (alim.End.Id)
                        {
                            case 0://Tablero solo insertar la conexión
                                Tablero tab = destination.Cargas.FirstOrDefault();
                                if (tab != null)
                                    CreateAlimentadorDestinationTr(alim.Id, tab.Id, 1, alimoAddedTask);
                                break;
                            case 1://Un solo motor
                            case 3://Varios motores
                                CreateMotorAndDestinations(alim.Id, destination.Motors, alimoAddedTask);
                                break;
                            case 2://Un solo motor y otras cargas
                            case 4://Varios motores y otras cargas
                                CreateMotorAndCargasAndDestinations(alim.Id, destination.Cargas, destination.Motors, alimoAddedTask);
                                break;
                            default://Capacitor Transformador o sub-alimentador
                                CreateExtraDataAndDestinations(alim.Id, destination.ExtraData, alimoAddedTask);
                                break;
                        }
                    }
                });
        }
        /// <summary>
        /// Creates the extra data and destinations.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="extraData">The extra data.</param>
        /// <param name="alimoAddedTask">The alimo added task.</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void CreateExtraDataAndDestinations(int alimId, ExtraData extraData, Action<object,int> connectionCreated)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = CreateExtraDataConnections,
                TaskCompleted = (Object result) => { connectionCreated(result,alimId); }
            };
            tr.Run(new Object[] { alimId, extraData });
        }

        private static object CreateExtraDataConnections(SQLite_Connector conn, object input)
        {
            try
            {
                Object[] data = input as Object[];
                int alimId = (int)data[0];
                ExtraData extra = (ExtraData)data[1];
                Boolean flag = extra.Create(conn, null);
                if (flag)
                {
                    extra.Id = (int)conn.SelectValue<long>("extras".SelectLastId(extra.PrimaryKey));
                    DestinationRow row = new DestinationRow()
                    {
                        AlimId = alimId,
                        ConnId = extra.Id,
                        TypeId = 2,
                    };
                    return row.Create(conn, null);
                }
                return false;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        /// <summary>
        /// Creates the motor and cargas and destinations.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cargas">The cargas.</param>
        /// <param name="motors">The motors.</param>
        /// <param name="alimoAddedTask">The alimo added task.</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void CreateMotorAndCargasAndDestinations(int alimId, IEnumerable<Tablero> cargas, IEnumerable<BigMotor> motors, Action<object,int> connectionCreated)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = CreateMotorsCargasConnections,
                TaskCompleted = (Object result) => { connectionCreated(result,alimId); }
            };
            tr.Run(new Object[] { alimId, cargas, motors });
        }
        /// <summary>
        /// Creates the motors cargas connections.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static object CreateMotorsCargasConnections(SQLite_Connector conn, object input)
        {
            Object[] data = input as Object[];
            int alimId = (int)data[0];
            IEnumerable<Tablero> cargas = (IEnumerable<Tablero>)data[1];
            IEnumerable<BigMotor> motors = (IEnumerable<BigMotor>)data[2];
            Object motorsResult = CreateMotorsConnections(conn, new Object[] { alimId, motors });
            Object cargasResult = CreateTableroConnections(conn, new Object[] { alimId, cargas });
            if (motorsResult is Boolean && cargasResult is Boolean)
                return (Boolean)motorsResult && (Boolean)cargasResult;
            else if (!(motorsResult is Boolean) && cargasResult is Boolean)
                return motorsResult;
            else
                return cargasResult;
        }

        private static object CreateTableroConnections(SQLite_Connector conn, object[] input)
        {
            try
            {
                Object[] data = input as Object[];
                int alimId = (int)data[0];
                IEnumerable<Tablero> cargas = (IEnumerable<Tablero>)data[1];
                List<Object> result = new List<object>();
                cargas.ToList().ForEach(x =>
                {
                    result.Add(CreateAlimentadorConnection(conn, new Object[] { alimId, x.Id, 1 }));
                });
                return result.Count(x => x is Exception) == 0 ? true : result.FirstOrDefault(y => y is Exception);
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        /// <summary>
        /// Creates the motor and destinations.
        /// </summary>
        /// <param name="alimId">The alim identifier.</param>
        /// <param name="motors">The motors.</param>
        /// <param name="connectionCreated">The connection created.</param>
        private static void CreateMotorAndDestinations(int alimId, IEnumerable<BigMotor> motors, Action<object,int> connectionCreated)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = CreateMotorsConnections,
                TaskCompleted = (Object result) => { connectionCreated(result,alimId); }
            };
            tr.Run(new Object[] { alimId, motors });
        }
        /// <summary>
        /// Creates the motors connections.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static object CreateMotorsConnections(SQLite_Connector conn, object input)
        {
            try
            {
                Object[] data = input as Object[];
                int alimId = (int)data[0];
                IEnumerable<BigMotor> motors = (IEnumerable<BigMotor>)data[1];
                Boolean flag;
                DestinationRow row;
                motors.ToList().ForEach(x =>
                {
                    flag = x.Create(conn, null);
                    if (flag)
                    {
                        x.Id = (int)conn.SelectValue<long>("motores".SelectLastId(x.PrimaryKey));
                        row = new DestinationRow()
                        {
                            AlimId = alimId,
                            ConnId = x.Id,
                            TypeId = 0,
                        };
                        row.Create(conn, null);
                    }
                });

                return motors.Count(x => x.Id > 0) > 0;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        /// <summary>
        /// Crea la conexión del alimentador
        /// </summary>
        /// <param name="alimId">El id del alimentador.</param>
        /// <param name="conn_id">El id del elemento conectado.</param>
        /// <param name="typeId">El tipo de elemento conectado al alimentador.</param>
        /// <param name="connectionCreated">La tarea a ejecutarse una vez que la conexión fue creada.</param>
        private static void CreateAlimentadorDestinationTr(int alimId, int conn_id, int typeId, Action<Object,int> connectionCreated)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = CreateAlimentadorConnection,
                TaskCompleted = (Object result) => { connectionCreated(result,alimId); }
            };
            tr.Run(new Object[] { alimId, conn_id, typeId });

        }
        /// <summary>
        /// Crea la inserción del alimentador
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada para realizar la inserción.</param>
        /// <returns>Una bandera que indica si se realizó la conexión</returns>
        private static object CreateAlimentadorConnection(SQLite_Connector conn, object input)
        {
            try
            {
                Object[] data = input as Object[];
                DestinationRow row = new DestinationRow()
                {
                    AlimId = (int)data[0],
                    ConnId = (int)data[1],
                    TypeId = (int)data[2],
                };
                return row.Create(conn, null);
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        /// <summary>
        /// Crea un nuevo alimentador y lo guarda en la aplicación
        /// </summary>
        /// <param name="alimentador">El tablero a insertar</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void CreateAlimentadorTr(this AlimInput alimentador, Action<Object> task_completed)
        {
            //1: Se agregá el tablero a la aplicación
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = InsertAlimTask,
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(alimentador);
        }
        /// <summary>
        /// Define la tarea que inserta un alimentador 
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada de la conexión debe ser un alimentador.</param>
        /// <returns>El resultado de la transacción</returns>
        private static object InsertAlimTask(SQLite_Connector conn, object input)
        {
            AlimInput alim = (AlimInput)input;
            Boolean flag = false;
            flag = alim.Create(conn, null);
            if (flag)
                alim.Id = (int)conn.SelectValue<long>("alimentador".SelectLastId(alim.PrimaryKey));
            return new Object[] { flag, alim.Id > 0 ? alim : null };
        }
    }
}
