using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
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
        public List<string> Select(string query)
        {
            List<string> fields = new List<string>();
            string item;
            SQLiteCommand cmd = new SQLiteCommand(query, this.Connection);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                var hasRows = reader.HasRows;
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var value = reader[i];
                        var dotNetType = reader.GetFieldType(i);
                        var sqlType = reader.GetDataTypeName(i);
                        var specificType = reader.GetProviderSpecificFieldType(i);
            
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Selecciona el nombre de las tablas seleccionadas
        /// </summary>
        /// <returns>La lista de las tablas</returns>
        public List<String> SelectTables()
        {
            String query = "SELECT tbl_name FROM sqlite_master";
            List<String> rows = this.Select(query);
            return rows;
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
        /// <summary>
        /// Ejecuta una transacción
        /// </summary>
        /// <param name="path">La ruta de la base de datos.</param>
        /// <param name="task">La tarea a ejecutar.</param>
        /// <returns>El resultado de la transacción</returns>
        public static Object Run(String path, Object input, Func<Object, SQLite_Connector, Object> task)
        {
            try
            {
                using (SQLite_Connector conn = new SQLite_Connector(path))
                {
                    try
                    {
                        return task(input, conn);
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
