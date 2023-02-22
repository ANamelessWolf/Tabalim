using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define las transacciones del proyecto
    /// </summary>
    public static partial class TransactionUtils
    {
        /// <summary>
        /// Define la transacción que realizá la importación del proyecto
        /// </summary>
        /// <param name="project">El proyecto a importar.</param>
        /// <param name="task_completed">La tarea a ejecutar una vez que se termine de guardar.</param>
        public static void ImportProjectTr(this TabalimApp app, Action<Object> task_completed)
        {
            ImportTablero(TabalimApp.CurrentProject, TabalimApp.AppDBPath, task_completed);
        }
        /// <summary>
        /// Importa el tablero
        /// </summary>
        /// <param name="project">El proyecto actual.</param>
        /// <param name="localDB">La base de datos local.</param>
        /// <param name="task_completed">La tarea a ejecutar al terminar la transacción.</param>
        public static void ImportTablero(Project project, String localDB, Action<Object> task_completed)
        {
            FileManager fm = new FileManager("Tabalim", "Abrir Proyecto Alimentadores", "alim");
            String filePath;
            if (fm.PickPath(out filePath))
            {
                try
                {
                    //Se sobreescribe la base de datos actual
                    File.Copy(localDB, Path.Combine(Path.GetDirectoryName(localDB), "tabalim.sqlite.tmp"), true);
                    File.Copy(filePath, localDB, true);
                    //Se ejecuta una transacción para cambiar la ruta del proyecto
                    SQLiteWrapper tr = new SQLiteWrapper(filePath)
                    {
                        TransactionTask = (SQLite_Connector conn, Object input) =>
                        {
                            try
                            {
                                var tData = (Object[])input;
                                Project prj = tData[0] as Project;
                                String path = tData[1] as String;
                                var newPath = new KeyValuePair<string, object>[]
                                { new KeyValuePair<string, object>("ruta", path)};
                                Boolean flag = prj.Update(conn, newPath);
                                if (flag)
                                {
                                    prj.Path = path;
                                    return new object[] { true, String.Format("Proyecto de alimentadores cargado de forma correcta en \n{0}.", filePath) };
                                }
                                else
                                    return new object[] { false, String.Format("Error al cargar el proyecto de alimentadores\nDetalles: {0}", conn.Error) };
                            }
                            catch (Exception exc)
                            {
                                return new object[] { false, String.Format("Error al abrir el proyecto de alimentadores.\nDetalles: {0}", exc.Message) };
                            }
                        },
                        TaskCompleted = (Object result) =>
                        {
                            var res = (Object[])result;
                            //Si existe error se regresa a la última base de datos guardada
                            if (!(Boolean)res[0])
                                File.Copy(Path.Combine(Path.GetDirectoryName(localDB), "tabalim.sqlite.tmp"), localDB, true);
                            task_completed(result);
                        }
                    };
                    tr.Run(new Object[] { project, filePath });
                }
                catch (Exception exc)
                {
                    task_completed(new object[] { false, String.Format("Error al abrir el tablero\nDetalles: {0}", exc.Message) });
                }
            }
            else
                task_completed(new object[] { false, String.Empty });
        }
        /// <summary>
        /// Define la transacción que realizá la exportación del proyecto
        /// </summary>
        /// <param name="project">El proyecto a exportar.</param>
        /// <param name="task_completed">La tarea a ejecutar una vez que se termine de guardar.</param>
        public static void ExportProjectTr(this Project project, Action<Object> task_completed, Boolean saveAs = false)
        {
            if (File.Exists(project.Path) && !saveAs)
                SaveProject(project.Path, new Object[] { project, task_completed });
            else
            {
                FileManager fm = new FileManager("Tabalim", "Guardar Proyecto de Alimentadores", "alim");
                string savePath;
                fm.SaveDialog(SaveProject, new Object[] { project, task_completed }, out savePath);
            }
        }
        /// <summary>
        /// Saves the project.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="saveInput">The save input.</param>
        private static bool SaveProject(String filePath, object saveInput)
        {
            var data = (Object[])saveInput;
            Project project = data[0] as Project;
            Action<Object> task_completed = data[1] as Action<Object>;
            try
            {
                //Se copia la ruta de la base datos
                File.Copy(TabalimApp.AppDBPath, filePath, true);
                //Actualizamos la ruta del proyecto
                SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                {
                    TransactionTask = (SQLite_Connector conn, Object input) =>
                    {
                        try
                        {
                            var tData = (Object[])input;
                            Project prj = tData[0] as Project;
                            String path = tData[1] as String;
                            var newPath = new KeyValuePair<string, object>[]
                            { new KeyValuePair<string, object>("ruta", path)};
                            Boolean flag = prj.Update(conn, newPath);
                            if (flag)
                            {
                                prj.Path = path;
                                return new object[] { true, String.Format("Proyecto de alimentadores guardado de forma correcta en \n{0}.", filePath) };
                            }
                            else
                                return new object[] { false, String.Format("Error al guardar el proyecto de alimentadores\nDetalles: {0}", conn.Error) };
                        }
                        catch (Exception exc)
                        {
                            return new object[] { false, String.Format("Error al guardar el proyecto de alimentadores\nDetalles: {0}", exc.Message) };
                        }
                    },
                    TaskCompleted = (Object result) => { task_completed(result); }
                };
                tr.Run(new Object[] { project, filePath });
            }
            catch (Exception exc)
            {
                task_completed(new object[] { false, String.Format("Error al guardar el proyecto de alimentadores\nDetalles: {0}", exc.Message) });
            }
            return true;
        }
    }
}
