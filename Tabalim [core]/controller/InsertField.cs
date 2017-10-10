using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define un campo de inserción en la tabla
    /// </summary>
    public class InsertField
    {
        /// <summary>
        /// El nombre de la tabla
        /// </summary>
        public String Tablename;
        /// <summary>
        /// El tipo de dato del valor a insertar
        /// </summary>
        public InsertFieldType DataType;
        /// <summary>
        /// El valor del dato a insertar
        /// </summary>
        public Object Value;
        /// <summary>
        /// El nombre de la columna en la BD
        /// </summary>
        public String ColumnName;
        /// <summary>
        /// Generá una <see cref="System.String" /> que representa la instancia.
        /// </summary>
        /// <returns>
        /// La <see cref="System.String" /> que representa a la instancia.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}: {1} [{2}]", this.ColumnName, this.Value, this.DataType);
        }
        /// <summary>
        /// Generá una <see cref="System.SQLiteParameter" /> que representa el valor del dato a insertar
        /// </summary>
        /// <returns>
        /// El parámetro de SQLite.
        /// </returns>
        public SQLiteParameter GetSQLiteParameter()
        {
            return new SQLiteParameter("@" + this.ColumnName, this.GetValue());
        }
        /// <summary>
        /// Devulve el valor del parametro en el formato necesario.
        /// </summary>
        /// <param name="value">El valor a formatear</param>
        /// <returns>El valor fomateado</returns>
        public string GetValue()
        {
            if (this.DataType == InsertFieldType.DATE)
            {
                string dateTimeFormat = "{0}-{1:00}-{2:00}";
                DateTime datetime = (DateTime)this.Value;
                return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day);
            }
            else
                return this.Value.ToString();
        }
    }
}
