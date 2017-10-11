using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    public interface ISQLiteParser
    {
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        /// <returns>El resultado del parsing</returns>
        void Parse(SelectionResult[] result);
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        InsertField[] GetInsertFields();

    }
}
