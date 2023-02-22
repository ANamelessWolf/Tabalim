using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define un conjunto de herramientas auxiliares para
    /// realizar transacciones en SQLite
    /// </summary>
    public static class SQLiteUtils
    {
        /// <summary>
        /// Obtiene el resultado de selección de la fila actual.
        /// </summary>
        /// <param name="reader">El lector del query de selección.</param>
        /// <returns>La fila seleccionada.</returns>
        public static SelectionResult[] GetResult(this SQLiteDataReader reader)
        {
            SelectionResult[] selRes = new SelectionResult[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                selRes[i] = new SelectionResult(reader, i);
            return selRes;
        }
        /// <summary>
        /// Crea una condición que pide que la llave primaria sea igual
        /// al valor del Id del elemento.
        /// </summary>
        /// <param name="element">El elemento a evaluar.</param>
        /// <returns>La condición como una cadena de SQL</returns>
        public static string CreatePrimaryKeyCondition(this IDatabaseMappable element)
        {
            String condition = "\"{0}\" = {1} ";
            return String.Format(condition, element.PrimaryKey, element.Id);
        }
        /// <summary>
        /// Inserta un elemento, selecciona el último id del elemento insertado.
        /// El id es asignado al elemento y es regresado en la función
        /// </summary>
        /// <typeparam name="T">El tipo de elemento a retornar</typeparam>
        /// <param name="element">El elmento generico a insertar.</param>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada necesaria para insertar el elemento.</param>
        /// <returns></returns>
        public static T GetLastId<T>(this IDatabaseMappable element, SQLite_Connector conn, Object input)
            where T : IDatabaseMappable
        {
            if (element.Create(conn, input))
                element.Id = (int)conn.SelectValue<long>(element.TableName.SelectLastId(element.PrimaryKey));
            return (T)element;
        }
        /// <summary>
        /// Obtiene el resultado de selección
        /// </summary>
        /// <param name="query">El query de selección</param>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <returns>El resultado de selección.</returns>
        public static List<SelectionResult[]> GetCommandResult(this SQLite_Connector conn, string query)
        {
            List<SelectionResult[]> result = new List<SelectionResult[]>();
            SQLiteCommand cmd = new SQLiteCommand(query, conn.Connection);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
                while (reader.HasRows && reader.Read())
                    result.Add(reader.GetResult());
            return result;
        }
        /// <summary>
        /// Crea el campo como una cadena
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo a insertar</returns>
        public static InsertField CreateFieldAsString(this IDatabaseMappable data, string columnName, Object value)
        {
            return new InsertField()
            {
                ColumnName = columnName,
                DataType = InsertFieldType.STRING,
                Tablename = data.TableName,
                Value = value == null ? String.Empty : value.ToString()
            };
        }
        /// <summary>
        /// Crea el campo como un número
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo a insertar</returns>
        public static InsertField CreateFieldAsNumber(this IDatabaseMappable data, string columnName, Object value)
        {
            return new InsertField()
            {
                ColumnName = columnName,
                DataType = InsertFieldType.NUMBER,
                Tablename = data.TableName,
                Value = value
            };
        }
        /// <summary>
        /// Crea el campo como fecha
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo a insertar</returns>
        public static InsertField CreateFieldAsDate(this IDatabaseMappable data, string columnName, DateTime value)
        {
            return new InsertField()
            {
                ColumnName = columnName,
                DataType = InsertFieldType.DATE,
                Tablename = data.TableName,
                Value = value
            };
        }
        /// <summary>
        /// Crea el campo como una cadena
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo actualizar</returns>
        public static UpdateField CreateFieldAsString(this KeyValuePair<string, Object> data, string table_name, Object value)
        {
            return new UpdateField()
            {
                ColumnName = data.Key,
                DataType = InsertFieldType.STRING,
                Tablename = table_name,
                Value = value == null ? String.Empty : value.ToString()
            };
        }
        /// <summary>
        /// Crea el campo como un número
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo actualizar</returns>
        public static UpdateField CreateFieldAsNumber(this KeyValuePair<string, Object> data, string table_name, Object value)
        {
            return new UpdateField()
            {
                ColumnName = data.Key,
                DataType = InsertFieldType.NUMBER,
                Tablename = table_name,
                Value = value
            };
        }
        /// <summary>
        /// Crea el campo como fecha
        /// </summary>
        /// <param name="data">El objeto a crear el campo.</param>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="value">El valor de la columna.</param>
        /// <returns>El campo actualizar</returns>
        public static UpdateField CreateFieldAsDate(this KeyValuePair<string, Object> data, string table_name, Object value)
        {
            return new UpdateField()
            {
                ColumnName = data.Key,
                DataType = InsertFieldType.DATE,
                Tablename = table_name,
                Value = value
            };
        }
        /// <summary>
        /// Convierte un resultado de selección a un arreglo de strings
        /// </summary>
        /// <param name="result">El resultado de selección.</param>
        /// <returns>El resultado de selección</returns>
        public static String[] ParseAsString(this SelectionResult[] result)
        {
            return result.Select(x => x.Value.ToString()).ToArray();
        }
        /// <summary>
        /// Convierte un resultado de selección a un arreglo de enteros
        /// </summary>
        /// <param name="result">El resultado de selección.</param>
        /// <returns>El resultado de selección</returns>
        public static int[] ParsePolos(this string polos)
        {
            int num;
            return polos.Split(',').Where(x => int.TryParse(x, out num)).Select(x => int.Parse(x)).ToArray();
        }
        /// <summary>
        /// Obtiene el valor de un resultado especifico
        /// </summary>
        /// <typeparam name="T">El tipo de dato a extraer del resultado.</typeparam>
        /// <param name="result">El resultado de la selección.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <returns>El valor del resultado con el tipo de dato especifico.</returns>
        public static T GetValue<T>(this SelectionResult[] result, string columnName) where T : struct
        {
            SelectionResult selResult = result.FirstOrDefault(x => x.ColumnName == columnName);
            if (selResult != null)
                return (T)selResult.Value;
            else
                return default(T);
        }
        /// <summary>
        /// Obtiene el valor de un resultado especifico
        /// </summary>
        /// <typeparam name="T">El tipo de dato a extraer del resultado.</typeparam>
        /// <param name="result">El resultado de la selección.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <returns>El valor del resultado con el tipo de dato especifico.</returns>
        public static int GetInteger(this SelectionResult[] result, string columnName)
        {
            SelectionResult selResult = result.FirstOrDefault(x => x.ColumnName == columnName);
            if (selResult != null && selResult.Value is long)
            {
                long val = (long)selResult.Value;
                return (int)val;
            }
            else
                return default(int);
        }
        /// <summary>
        /// Obtiene el valor de un resultado especifico
        /// </summary>
        /// <typeparam name="T">El tipo de dato a extraer del resultado.</typeparam>
        /// <param name="result">El resultado de la selección.</param>
        /// <param name="columnName">El nombre de la columna.</param>
        /// <returns>El valor del resultado con el tipo de dato especifico.</returns>
        public static string GetString(this SelectionResult[] result, string columnName)
        {
            SelectionResult selResult = result.FirstOrDefault(x => x.ColumnName == columnName);
            if (selResult != null)
                return selResult.Value.ToString();
            else
                return null;
        }
        /// <summary>
        /// Define el query de selección
        /// </summary>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="condition">La condición a evaluar.</param>
        /// <returns>El query de selección.</returns>
        public static string SelectAll(this string table_name, string condition = "")
        {
            if (condition == "")
                return String.Format("SELECT * FROM {0}", table_name);
            else
                return String.Format("SELECT * FROM {0} WHERE {1}", table_name, condition);
        }
        /// <summary>
        /// Define el query de selección
        /// </summary>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="column">El nombre de la columna a seleccionar</param>
        /// <param name="condition">La condición a evaluar.</param>
        /// <returns>El query de selección.</returns>
        public static string SelectColumn(this string table_name, string column, string condition = "")
        {
            if (condition == "")
                return String.Format("SELECT {0} FROM {1}", table_name, column);
            else
                return String.Format("SELECT {0} FROM {1} WHERE {2}", table_name, column, condition);
        }
        /// <summary>
        /// Define el query de selección
        /// </summary>
        /// <param name="table_name">El nombre de la tabla.</param>
        /// <param name="column">El nombre de la columna a seleccionar</param>
        /// <returns>El query de selección.</returns>
        public static string SelectLastId(this string table_name, string column)
        {
            return String.Format("SELECT MAX({1}) FROM {0}", table_name, column);
        }
    }
}
