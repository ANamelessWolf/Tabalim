using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define las acciones auxiliares para administrar una transacción
    /// </summary>
    public static class TransactionUtils
    {
        /// <summary>
        /// Crea un nuevo tablero y lo guarda en la aplicación
        /// </summary>
        /// <param name="app">La aplicación</param>
        /// <param name="tablero">El tablero a insertar</param>
        public static void CreateTableroTr(this TabalimApp app, Tablero tablero)
        {
            //1: Se agregá el tablero a la aplicación
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Tablero tab = (Tablero)input;
                    Boolean flag = false;
                    flag = tab.Create(conn, null);
                    if (flag)
                        tab.Id = (int)conn.SelectValue<long>(TABLE_TABLERO.SelectLastId("tab_id"));
                    return new Object[] { flag, tab.Id > 0 ? tab : null };
                },
                TaskCompleted = (Object qResult) =>
                {
                    var result = (Object[])qResult;
                    Tablero tab = (Tablero)result[1];
                    if ((Boolean)result[0] && tab != null)
                    {
                        app.Tableros.Add(tablero);
                        TabalimApp.CurrentTablero = tab;
                    }
                }
            };
            tr.Run(tablero);
        }
        /// <summary>
        /// Define una transacción que agregá un componente a un tablero
        /// </summary>
        /// <param name="tablero">El tablero</param>
        /// <param name="component">El componente agregar</param>
        public static void AddComponentTr(this Tablero tablero, Componente component)
        {
            //Primero se guarda el circuito
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath);
            Circuito circuito = component.Circuito;
            //Se revisa si el ciruito ya existe
            if (tablero.Circuitos.ContainsKey(circuito.ToString()))
                tr.TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = (Object[])input;
                    Circuito cto = data[0] as Circuito;
                    Componente cmp = conn.InsertComponentTr(data[1] as Componente, cto);
                    return new Object[] { cmp.Id > 0, cto, cmp };
                };
            else
                tr.TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = (Object[])input;
                    Tablero tab = data[2] as Tablero;
                    Circuito cto = conn.InsertCircuitTr(data[0] as Circuito, tab);
                    Componente cmp = conn.InsertComponentTr(data[1] as Componente, cto);
                    return new Object[] { cto.Id > 0 && cmp.Id > 0, cto, cmp };
                };
            tr.TaskCompleted = (Object qResult) =>
            {
                Object[] result = qResult as Object[];
                bool succed = (bool)result[0];
                if (succed)
                {
                    Circuito cto = (Circuito)result[1];
                    Componente cmp = (Componente)result[2];
                    if (!tablero.Circuitos.ContainsKey(cto.ToString()))
                        tablero.Circuitos.Add(cto.ToString(), cto);
                    if (!tablero.Componentes.ContainsKey(cmp.Id))
                        tablero.Componentes.Add(cmp.Id, cmp);
                }
                else
                    throw new Exception("Error al anexar el componente.");
            };
            tr.Run(new Object[] { circuito, component, TabalimApp.CurrentTablero });
        }
        /// <summary>
        /// Crea una transacción que inserta un componente en la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="cmp">El componente a insertar.</param>
        /// <param name="cto">El circuito al que se conecta el componente.</param>
        /// <returns>El componente insertado</returns>
        public static Componente InsertComponentTr(this SQLite_Connector conn, Componente cmp, Circuito cto)
        {
            if (cmp.Create(conn, cto))
                cmp.Id = (int)conn.SelectValue<long>(TABLE_COMPONENT.SelectLastId("comp_id"));
            return cmp;
        }
        /// <summary>
        /// Crea una transacción que inserta un componente en la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="cmp">El componente a insertar.</param>
        /// <param name="cto">El circuito al que se conecta el componente.</param>
        /// <returns>El componente insertado</returns>
        public static Circuito InsertCircuitTr(this SQLite_Connector conn, Circuito cto, Tablero tab)
        {
            if (cto.Create(conn, tab))
                cto.Id = (int)conn.SelectValue<long>(TABLE_CIRCUIT.SelectLastId("cir_id"));
            return cto;
        }
    }
}
