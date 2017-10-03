using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define una transacción a SQLite
    /// </summary>
    public class SQLiteWrapper
    {
        /// <summary>
        /// Define la acción que realiza la transacción
        /// </summary>
        /// <param name="conn">El connector a SQLite.</param>
        /// <param name="input">La entrada de la transacción.</param>
        /// <returns>El resultado de la transacción</returns>
        public delegate Object TransactionTaskHandler(SQLite_Connector conn, Object input);
        /// <summary>
        /// Define la acción a ejecutar una vez que la transacción a concluido
        /// </summary>
        /// <param name="result">El resultado de la transacción.</param>
        public delegate void TaskCompletedHandler(Object result);
        /// <summary>
        /// La acción que realizá la transacción
        /// </summary>
        public TransactionTaskHandler TransactionTask;
        /// <summary>
        /// La acción que se realizá una vez que termino la transacción
        /// </summary>
        public TaskCompletedHandler TaskCompleted;
        /// <summary>
        /// La ruta al archivo SQLite
        /// </summary>
        public string DBPath;
        /// <summary>
        /// El controlador encargado de realizar la tarea de manera asincrona
        /// </summary>
        private BackgroundWorker BGW;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SQLiteWrapper"/>.
        /// </summary>
        ///<param name="path">La ruta del archivo SQL</param>
        public SQLiteWrapper(string path)
        {
            this.DBPath = path;
            this.BGW = new BackgroundWorker();
            this.BGW.DoWork += BGW_DoWork;
            this.BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;
        }
        /// <summary>
        /// Ejecuta la transacción
        /// </summary>
        /// <param name="input">La entrada de la transación.</param>
        public void Run(Object input)
        {
            this.BGW.RunWorkerAsync(new object[] { this.DBPath, input });
        }
        /// <summary>
        /// Maneja el evento que se ejecuta al finalizar la tárea
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RunWorkerCompletedEventArgs"/> que contienen la información del evento.</param>
        private void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.TaskCompleted != null)
                this.TaskCompleted(e.Result);
        }
        /// <summary>
        /// Maneja el evento que realizá la tárea 
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="DoWorkEventArgs"/> que contienen la información del evento.</param>
        private void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.TransactionTask == null)
                throw new Exception("No se ha definido acción para la transacción.");
            try
            {
                Object[] input = (Object[])e.Argument;
                String dpPath = (string)input[0];
                using (SQLite_Connector conn = new SQLite_Connector(dpPath))
                {
                    try
                    {
                        e.Result = this.TransactionTask(conn, input[1]);
                    }
                    catch (System.Exception exc)
                    {
                        throw exc;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
