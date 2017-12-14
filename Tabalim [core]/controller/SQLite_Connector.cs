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
        /// El último error registrado
        /// </summary>
        public string Error;
        /// <summary>
        /// El estado de conexión actual
        /// </summary>
        public ConnectionState Status;
        /// <summary>
        /// Devuelve true si el último query fue satisfactorio.
        /// </summary>
        public bool QuerySucced;
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
            try
            {
                List<SelectionResult[]> result = this.GetCommandResult(query);
                return result.Select(x => x.ParseAsString()).ToList();
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
                this.QuerySucced = false;
                return new List<string[]>();
            }
        }
        /// <summary>
        /// Selecciona un valor de la tabla
        /// </summary>
        /// <typeparam name="T">El tipo de dato que se usa en la selección</typeparam>
        /// <param name="query">El query de selección</param>
        /// <returns>El valor seleccionado</returns>
        public T SelectValue<T>(string query) where T : struct
        {
            this.QuerySucced = true;
            try
            {
                List<SelectionResult[]> result = this.GetCommandResult(query);
                if (result.Count > 0 && result[0].Length > 0)
                    return (T)result[0][0].Value;
                else
                    return default(T);
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
                this.QuerySucced = false;
                return default(T);
            }
        }
        /// <summary>
        /// Realizá un query de selección
        /// </summary>
        /// <param name="query">El query de selección</param>
        /// <param name="parsingTask">En caso de que los elementos definan una clase abstracta se debe definir un parsing avanzado.</param>
        /// <returns>La colección de elementos seleccionados</returns>
        public List<T> Select<T>(string query, Func<List<SelectionResult[]>, List<T>> parsingTask = null) where T : ISQLiteParser
        {
            try
            {
                List<SelectionResult[]> result = this.GetCommandResult(query);
                if (parsingTask == null)
                    return result.Select(x => (T)Activator.CreateInstance(typeof(T), new Object[] { x })).ToList();
                else
                    return parsingTask(result);
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
                this.QuerySucced = false;
                return new List<T>();
            }
        }
        /// <summary>
        /// Inserta una nuevo registro en la base de datos
        /// </summary>
        /// <param name="data">La información a insertar en la tabla</param>
        /// <param name="db">La base de datos por defecto</param>
        /// <returns>Verdadero cuando realizá la inserción de manera correcta</returns>
        public Boolean Insert(ISQLiteParser data, string db = "main")
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                InsertField[] fields = data.GetInsertFields();
                //Query definition
                String query = "INSERT INTO \"{0}\".\"{1}\"({2}) VALUES ({3})",
                tableName = fields[0].Tablename,
                fieldsStr = String.Empty, valuesStr = String.Empty;
                foreach (InsertField field in fields)
                {
                    fieldsStr += String.Format("\"{0}\", ", field.ColumnName);
                    valuesStr += String.Format("@{0}, ", field.ColumnName);
                    cmd.Parameters.Add(field.GetSQLiteParameter());
                }
                fieldsStr = fieldsStr.Substring(0, fieldsStr.Length - 2);
                valuesStr = valuesStr.Substring(0, valuesStr.Length - 2);
                query = String.Format(query, db, tableName, fieldsStr, valuesStr);
                cmd.CommandText = query;
                this.Query(cmd);
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
            }
            return this.QuerySucced;
        }
        /// <summary>
        /// Actualiza un nuevo registro en la base de datos
        /// </summary>
        /// <param name="data">La información a actualizar en la tabla</param>
        /// <param name="condition">La condición para realizar el update</param>
        /// <param name="db">La base de datos por defecto</param>
        /// <returns>Verdadero cuando realizá la inserción de manera correcta</returns>
        public Boolean Update(UpdateField[] data, string condition, string db = "main")
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                //Query definition
                String query = "UPDATE \"{0}\".\"{1}\" SET {2} WHERE {3} ",
                tableName = data[0].Tablename,
                setStr = String.Empty;
                foreach (UpdateField field in data)
                {
                    setStr += String.Format("\"{0}\" = @{0}, ", field.ColumnName);
                    cmd.Parameters.Add(field.GetSQLiteParameter());
                }
                setStr = setStr.Substring(0, setStr.Length - 2);
                query = String.Format(query, db, tableName, setStr, condition);
                cmd.CommandText = query;
                this.Query(cmd);
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
            }
            return this.QuerySucced;
        }
        /// <summary>
        /// Borra uno o más elementos que satisfacen la condición
        /// </summary>
        /// <param name="condition">La condición a evaluar</param>
        /// <param name="tableName">El nombre de la tabla</param>
        /// <param name="db">La base de datos por defecto</param>
        /// <returns>Verdadero cuando se borra el elemento de manera correcta.</returns>
        public Boolean Delete(string tableName, string condition, string db = "main")
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                //Query definition
                String query = "DELETE FROM \"{0}\".\"{1}\" WHERE {2} ";
                query = String.Format(query, db, tableName, condition);
                cmd.CommandText = query;
                this.Query(cmd);
            }
            catch (Exception exc)
            {
                this.Error = exc.Message;
            }
            return this.QuerySucced;
        }
        /// <summary>
        /// Borra uno o más elementos por condición que la columna
        /// sea igual a un valor especifico
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="columnValue">El valor al que es igual la columna.</param>
        /// <param name="tableName">El nombre de la tabla</param>
        /// <param name="db">La base de datos por defecto</param>
        /// <returns>Verdadero cuando se borra el elemento de manera correcta.</returns>
        public Boolean DeletebyColumn(string tableName, string columnName, Object columnValue, string db = "main")
        {
            string condition;
            if (columnValue.GetType().IsValueType)
                condition = "\"{0}\" = {1}";
            else
                condition = "\"{0}\" = \"{1}\"";
            condition = String.Format(condition, columnName, columnValue);
            return this.Delete(tableName, condition, db);
        }
    /// <summary>
    /// Ejecuta un query mediante un comando
    /// </summary>
    /// <param name="cmd">El comando a ejecutar</param>
    public int Query(SQLiteCommand cmd)
    {
        try
        {
            cmd.Connection = this.Connection;
            int result = cmd.ExecuteNonQuery();
            this.QuerySucced = true;
            return result;
        }
        catch (Exception exc)
        {
            this.QuerySucced = false;
            throw exc;
        }
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
