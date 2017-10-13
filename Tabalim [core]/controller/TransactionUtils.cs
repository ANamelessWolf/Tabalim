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
        public static void AddComponentTr(this Tablero tablero, Componente component, Action<Object> componenteAddedTask = null)
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

                    if (cmp is Motor)
                    {
                        cto.Corriente = cmp.GetCorriente()
                    }
                    if (!tablero.Circuitos.ContainsKey(cto.ToString()))
                        tablero.Circuitos.Add(cto.ToString(), cto);
                    if (!tablero.Componentes.ContainsKey(cmp.Id))
                        tablero.Componentes.Add(cmp.Id, cmp);
                    if (!cto.Componentes.ContainsKey(cmp.Id))
                        cto.Componentes.Add(cmp.Id, cmp);
                    if (componenteAddedTask != null)
                        componenteAddedTask(cto);
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
            return cmp.GetLastId<Componente>(conn, cto);
        }
        /// <summary>
        /// Define la transacción que actualiza un componente
        /// </summary>
        /// <param name="component">El componente actualizar.</param>
        /// <param name="tablero">El tablero actualizar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        /// <param name="cto">El nuevo circuito.</param>
        /// <param name="cmp_count">El número total de componentes.</param>
        /// <param name="potencia">La potencia del circuito.</param>
        public static void UpdateComponentTr(this Componente component, Tablero tablero, Action<Object> task_completed, Circuito circuit = null, int cmp_count = 0, Potencia potencia = null)
        {
            KeyValuePair<string, object>[] updateData = new KeyValuePair<string, object>[]
            {
                circuit != null ? new KeyValuePair<string, object>("cir_id", circuit) : new KeyValuePair<string, object>(String.Empty, null),
                cmp_count > 0 ? new KeyValuePair<string, object>("comp_count", cmp_count) : new KeyValuePair<string, object>(String.Empty, null),
                potencia !=null ? new KeyValuePair<string, object>("potencia", potencia) : new KeyValuePair<string, object>(String.Empty, null)
            }.Where(x => x.Key != String.Empty).ToArray();
            if (updateData.Length > 0)
            {
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = (SQLite_Connector conn, Object input) =>
                    {
                        Object[] data = input as Object[];
                        Componente cmp = data[0] as Componente;
                        var uData = data[1] as KeyValuePair<string, object>[];
                        Circuito cto = data[2] as Circuito;
                        Tablero tab = data[3] as Tablero;
                        //Crear el circuito
                        if (cto != null && cto.Id < 1)
                        {
                            cto = conn.InsertCircuitTr(cto, tab);
                            tab.Circuitos.Add(cto.ToString(), cto);
                            int index = uData.Select(x => x.Key).ToList().IndexOf("cir_id");
                            uData[index] = new KeyValuePair<string, object>("cir_id", cto);
                            if (cmp.Circuito.Componentes.Count == 1)
                                conn.Delete(TABLE_CIRCUIT, cmp.Circuito.CreatePrimaryKeyCondition());
                        }
                        return cmp.Update(conn, uData);
                    },
                    TaskCompleted = (Object obj) => { task_completed(obj); },
                };
                tr.Run(new Object[] { component, updateData, circuit, tablero });
            }
        }
        /// <summary>
        /// Define la transacción que elimina un componente
        /// </summary>
        /// <param name="component">El componente a eliminar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void DeleteComponentTr(this Componente component, Action<Object> task_completed)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Componente cmp = input as Componente;
                    return cmp.Delete(conn);
                },
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(component);
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
            return cto.GetLastId<Circuito>(conn, tab);
        }
        /// <summary>
        /// Define la transacción que elimina un circuito
        /// </summary>
        /// <param name="circuit">El circuito a eliminar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void DeleteCircuitTr(this Circuito circuit, Action<Object> task_completed)
        {
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Circuito cto = input as Circuito;
                    return cto.Delete(conn);
                },
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(circuit);
        }
        /// <summary>
        /// Actualiza un elemento mediante una condición
        /// </summary>
        /// <param name="element">El elemento actualizar.</param>
        /// <param name="condition">La condición a evaluar.</param>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Boolean UpdateTr(this ISQLiteParser element, string condition, SQLite_Connector conn, KeyValuePair<string, object>[] input)
        {
            UpdateField[] data = input.Select(x => element.PickUpdateFields(x)).Where(y => y != null).ToArray();
            if (conn.Update(data, condition))
            {
                element.UpdateFields(input);
                return true;
            }
            else
                return false;
        }

    }
}
