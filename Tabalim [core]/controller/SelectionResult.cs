using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define un elemento seleccionado de un query a la bd
    /// </summary>
    public class SelectionResult
    {
        /// <summary>
        /// El nombre de la columna
        /// </summary>
        public String ColumnName;
        /// <summary>
        /// El valor leido de la selección
        /// </summary>
        public Object Value;
        /// <summary>
        /// El tipo de dato en .NET
        /// </summary>
        public Type DotNetType;
        /// <summary>
        /// El tipo de dato en SQLite
        /// </summary>
        public String SQLiteType;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SelectionResult"/>.
        /// </summary>
        /// <param name="reader">El lector de datos de SQLite.</param>
        /// <param name="columnIndex">El indice de la columna a leer.</param>
        public SelectionResult(SQLiteDataReader reader, int columnIndex)
        {
            this.ColumnName = reader.GetName(columnIndex);
            this.Value = reader[columnIndex];
            this.DotNetType = reader.GetFieldType(columnIndex);
            this.SQLiteType = reader.GetDataTypeName(columnIndex);
        }
        /// <summary>
        /// Generá una <see cref="System.String" /> que representa la instancia.
        /// </summary>
        /// <returns>
        /// La <see cref="System.String" /> que representa a la instancia.
        /// </returns>
        public override string ToString()
        {
            return String.Format(".NetType: {0}, SQLiteType: {1}, Value: {2}", this.DotNetType, this.SQLiteType, this.Value);
        }
    }
}
