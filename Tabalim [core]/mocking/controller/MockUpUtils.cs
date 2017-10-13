using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.mocking.controller
{
    /// <summary>
    /// Contiene un conjunto de herramientas para relizar prubas del control
    /// </summary>
    public static class MockUpUtils
    {
        /// <summary>
        /// Exporta el archivo de tablero
        /// </summary>
        /// <param name="tablero">El tablero a exportar.</param>
        /// <param name="task_completed">La tarea a ejecutar</param>
        /// <param name="mockTimeMs">El tiempo que pretende trabajar la función</param>
        public static void ExportTableroTr(this Tablero tablero, Action<Object> task_completed, int mockTimeMs = 6000)
        {
            FileManager fm = new FileManager("Tabalim", "Guardar Tableros", "tabalim");
            string savePath;
            fm.SaveDialog((String filePath, Object tab) =>
            {
                try
                {
                    //Copiamos el archivo base de tableros
                    File.Copy(TabalimApp.TableroDBPath, filePath, true);
                    SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                    {
                        TransactionTask = (SQLite_Connector conn, Object input) =>
                        {
                            try
                            {
                                System.Threading.Thread.Sleep(mockTimeMs);
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
    }
}
