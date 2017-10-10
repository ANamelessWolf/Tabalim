using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define un objeto que es mapeable a una base de datos
    /// </summary>
    public interface IDatabaseMappable
    {
        /// <summary>
        /// Establece el nombre de tabla
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        string TableName { get; }
        /// <summary>
        /// Representa el id de la instancia en la 
        /// base de datos
        /// </summary>
        /// <value>
        /// El id que usa el elemento en la tabla de la base de datos.
        /// </value>
        int Id { get; set; }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">El conector activo</param>
        /// <param name="input">La entrada que recibe la operación</param>
        /// <returns>Verdadero si realizá la inserción.</returns>
        Boolean Create(SQLite_Connector conn, Object input);
        /// <summary>
        /// Actualiza un registro del objeto en la base de datos
        /// </summary>
        void Update(Object input);
    }
}
