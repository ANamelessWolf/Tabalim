﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
            tablero.CreateTableroTr(
                (Object qResult) =>
            {
                var result = (Object[])qResult;
                Tablero tab = (Tablero)result[1];
                if (tab.NombreTablero == "")
                    tab.NombreTablero = String.Format("Tablero {0:000}", TabalimApp.CurrentProject.Tableros.Count);
                if (tab.Description == "")
                    tab.Description = "Sin descripción";
                if ((Boolean)result[0] && tab != null)
                {
                    app.Tableros.Add(tablero);
                    TabalimApp.CurrentTablero = tab;
                }
            });
        }
        /// <summary>
        /// Crea un nuevo tablero y lo guarda en la aplicación
        /// </summary>
        /// <param name="tablero">El tablero a insertar</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void CreateTableroTr(this Tablero tablero, Action<Object> task_completed)
        {
            //1: Se agregá el tablero a la aplicación
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = InsertTableroTask,
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(tablero);
        }
        /// <summary>
        /// Define la tarea que inserta un tablero 
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada de la conexión debe ser un tablero.</param>
        /// <returns>El resultado de la transacción</returns>
        private static object InsertTableroTask(SQLite_Connector conn, object input)
        {
            Tablero tab = (Tablero)input;
            Boolean flag = false;
            flag = tab.Create(conn, null);
            if (flag)
                tab.Id = (int)conn.SelectValue<long>(TABLE_TABLERO.SelectLastId(tab.PrimaryKey));
            return new Object[] { flag, tab.Id > 0 ? tab : null };
        }
        /// <summary>
        /// Exporta el archivo de tablero
        /// </summary>
        /// <param name="tablero">El tablero a exportar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void ExportTableroTr(this Tablero tablero, Action<Object> task_completed)
        {
            FileManager fm = new FileManager("Tabalim", "Guardar Tablero", "tabalim");
            string savePath;
            fm.SaveDialog((String filePath, Object tab) =>
            {
                try
                {
                    //Copiamos el archivo base de tableros
                    File.Copy(TabalimApp.TableroDBPath, filePath, true);
                    SQLiteWrapper tr = new SQLiteWrapper(filePath)
                    {
                        TransactionTask = (SQLite_Connector conn, Object input) =>
                        {
                            try
                            {
                                List<Componente> cmps;
                                List<Circuito> ctos;
                                Tablero t = ((Tablero)input).Clone(out cmps, out ctos);
                                //Se inserta el tablero
                                InsertTableroTask(conn, t);
                                //Se se insertan los circuitos
                                ctos.ForEach(x => x.GetLastId<Circuito>(conn, t));
                                //Se insertan los componentes
                                cmps.ForEach(cmp => cmp.GetLastId<Componente>(conn, ctos.FirstOrDefault(cto => cto.ToString() == cmp.CircuitoName)));
                                return new object[] { true, String.Format("Tablero guardado de forma correcta en \n{0}.", filePath) };
                            }
                            catch (Exception exc)
                            {
                                return new object[] { false, String.Format("Error al exportar el tablero\nDetalles: {0}", exc.Message) };
                            }
                        },
                        TaskCompleted = (Object result) => { task_completed(result); }
                    };
                    tr.Run(tablero);
                }
                catch (Exception exc)
                {
                    task_completed(new object[] { false, String.Format("Error al exportar el tablero\nDetalles: {0}", exc.Message) });
                }
            }, tablero, out savePath);
        }
        /// <summary>
        /// Importa un tablero al proyecto seleccionado
        /// </summary>
        /// <param name="window">La ventana que llama a esta función</param>
        /// <param name="project">El proyecto al cual se le importará un tablero.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void ImportTableroTr(this MetroWindow window, Project project, Action<Object> task_completed)
        {
            FileManager fm = new FileManager("Tabalim", "Abrir Tablero", "tabalim");
            String filePath;
            if (fm.PickPath(out filePath))
            {
                try
                {
                    SQLiteWrapper tr = new SQLiteWrapper(filePath)
                    {
                        TransactionTask = PickTableroName,
                        TaskCompleted = async (Object result) =>
                        {
                            String tabName = result as String;
                            Tablero existant = project.Tableros.Values.FirstOrDefault(x => x.NombreTablero == tabName);
                            if (existant != null)
                            {
                                MessageDialogResult res = await window.ShowMessageAsync("Tablero Existente", "El tablero ya existe en el proyecto actual\n¿Quiere remplazarlo?", MessageDialogStyle.AffirmativeAndNegative);
                                if (res == MessageDialogResult.Affirmative)
                                    DeleteTableroAndLoadTr(project, existant, filePath, task_completed);
                            }
                            else
                                LoadTableroTr(project, filePath, task_completed);
                        }
                    };
                    tr.Run(null);
                }
                catch (Exception exc)
                {
                    task_completed(new object[] { false, String.Format("Error al abrir el tablero\nDetalles: {0}", exc.Message) });
                }
            }
        }
        /// <summary>
        /// Picks the name of the tablero.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada de la transacción.</param>
        /// <returns>El nombre del tablero</returns>
        private static object PickTableroName(SQLite_Connector conn, object input)
        {
            Tablero tab = conn.Select<Tablero>(TABLE_TABLERO.SelectAll()).FirstOrDefault();
            return tab.NombreTablero;
        }
        /// <summary>
        /// Realizá la carga del tablero borrando el tablero existente
        /// </summary>
        /// <param name="project">El nombre del proyecto.</param>
        /// <param name="existant">El tablero existente.</param>
        /// <param name="filePath">La ruta del archivo a importar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        private static void DeleteTableroAndLoadTr(Project project, Tablero existant, string filePath, Action<object> task_completed)
        {
            SQLiteWrapper tr = new SQLiteWrapper(filePath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = input as Object[];
                    Tablero tab = data[0] as Tablero;
                    Project prj = data[1] as Project;
                    String pth = data[2] as String;
                    if (tab.Delete(conn))
                    {
                        prj.Tableros.Remove(tab.Id);
                        return InsertTableroTr(conn, prj);
                    }
                    else
                        throw new Exception("Error al borrar el tablero existente");
                },
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(new Object[] { existant, project, filePath });
        }
        /// <summary>
        /// Realizá la carga del tablero
        /// </summary>
        /// <param name="project">El nombre del proyecto.</param>
        /// <param name="existant">El tablero existente.</param>
        /// <param name="filePath">La ruta del archivo a importar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        private static void LoadTableroTr(Project project, string filePath, Action<object> task_completed)
        {
            SQLiteWrapper tr = new SQLiteWrapper(filePath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    Object[] data = input as Object[];
                    Project prj = data[0] as Project;
                    String pth = data[1] as String;
                    return InsertTableroTr(conn, prj);
                },
                TaskCompleted = (Object result) => { task_completed(result); }
            };
            tr.Run(new Object[] { project, filePath });
        }
        /// <summary>
        /// Inserta un nuevo tablero en la aplicación
        /// </summary>
        /// <param name="conn">La conexión activa.</param>
        /// <param name="prj">El proyecto seleccionado.</param>
        /// <param name="pth">La ruta del tablero a insertar.</param>
        /// <returns>El resultado de la transacción</returns>
        private static object InsertTableroTr(SQLite_Connector conn, Project prj)
        {
            Tablero tab = conn.Select<Tablero>(TABLE_TABLERO.SelectAll()).FirstOrDefault();
            Boolean flag = false;
            String msg = String.Empty;
            try
            {
                flag = true;

                TabalimApp.CurrentTablero = tab;
                msg = String.Format("El tablero se cargo en el proyecto actual");
            }
            catch (Exception exc)
            {
                msg = String.Format("Error al cargar el tablero\nDetalles: {0}", exc.Message);
            }
            return new Object[] { flag, msg };
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
        /// Define la transaccción que actualiza un circuito
        /// </summary>
        /// <param name="circuit">El circuito actualizar.</param>
        /// <param name="task_completed">La tarea que se ejecuta al terminar la transacción.</param>
        public static void UpdateCircuitTr(this Circuito circuit, Action<Object> task_completed, double longitud = Double.NaN, double fac_agrup = Double.NaN, string interruptor = null)
        {
            KeyValuePair<string, object>[] updateData = new KeyValuePair<string, object>[]
            {
                !Double.IsNaN(longitud) ? new KeyValuePair<string, object>("longitud", longitud) : new KeyValuePair<string, object>(String.Empty, null),
                !Double.IsNaN(fac_agrup) ? new KeyValuePair<string, object>("fac_agrup", fac_agrup) : new KeyValuePair<string, object>(String.Empty, null),
                interruptor !=null ? new KeyValuePair<string, object>("interruptor", interruptor) : new KeyValuePair<string, object>(String.Empty, null)
            }.Where(x => x.Key != String.Empty).ToArray();
            if (updateData.Length > 0)
            {
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = (SQLite_Connector conn, Object input) =>
                    {
                        Object[] data = input as Object[];
                        var cto = data[0] as Circuito;
                        var uData = data[1] as KeyValuePair<string, object>[];
                        return cto.Update(conn, uData);
                    },
                    TaskCompleted = (Object result) => { task_completed(result); }
                };
                tr.Run(new Object[] { circuit, updateData });
            }
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