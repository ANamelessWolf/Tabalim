using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Obtiene el resultado de selección
        /// </summary>
        /// <param name="query">El query de selección</param>
        /// <param name="conn">El objeto de conexión</param>
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
        /// Convierte un resultado de selección a un arreglo de strings
        /// </summary>
        /// <param name="result">El resultado de selección.</param>
        /// <returns>El resultado de selección</returns>
        public static String[] ParseAsString(this SelectionResult[] result)
        {
            return result.Select(x => x.Value.ToString()).ToArray();
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
        public static string GetString(this SelectionResult[] result, string columnName)
        {
            SelectionResult selResult = result.FirstOrDefault(x => x.ColumnName == columnName);
            if (selResult != null)
                return selResult.Value.ToString();
            else
                return null;
        }

    }
}
