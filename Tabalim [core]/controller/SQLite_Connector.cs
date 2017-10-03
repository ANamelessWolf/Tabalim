using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Se encarga de realizar la conexión a SQLite
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class SQLite_Connector : IDisposable
    {
        /// <summary>
        /// El objeto de conexión
        /// </summary>
        public SQLiteConnection Connection;
        /// <summary>
        /// El estado de conexión actual
        /// </summary>
        public ConnectionState Status;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SQLite_Connector"/>.
        /// </summary>
        ///<param name="path">La ruta del archivo SQL</param>
        public SQLite_Connector(string path)
        {
            try
            {
                string connString = String.Format("Data Source = {0}; Version = {1};", path, 3);
                this.Connection = new SQLiteConnection(connString);
                this.Connection.Open();
                this.Status = this.Connection.State;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Realizá un query de selección
        /// </summary>
        /// <param name="query">El query de selección</param>
        /// <returns>La colección de elementos seleccionados</returns>
        public List<string[]> Select(string query)
        {
            List<SelectionResult[]> result = this.GetCommandResult(query);
            return result.Select(x => x.ParseAsString()).ToList();
        }
        /// <summary>
        /// Realizá un query de selección
        /// </summary>
        /// <param name="query">El query de selección</param>
        /// <returns>La colección de elementos seleccionados</returns>
        public List<T> Select<T>(string query) where T : ISQLiteParser
        {
            List<SelectionResult[]> result = this.GetCommandResult(query);
            return result.Select(x => (T)Activator.CreateInstance(typeof(T), x)).ToList();
        }
        /// <summary>
        /// Selecciona el nombre de las tablas seleccionadas
        /// </summary>
        /// <returns>La lista de las tablas</returns>
        public List<String> SelectTables()
        {
            String query = "SELECT tbl_name FROM sqlite_master";
            return this.Select(query).Select(x => x[0]).ToList();
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Connection.Close();
            this.Connection.Dispose();
        }
       
    }
}
