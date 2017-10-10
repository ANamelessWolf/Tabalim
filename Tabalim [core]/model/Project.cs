using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Inicializa una nueva instancia de proyecto
    /// </summary>
    public class Project : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// Establece el nombre de tabla
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName { get { return TABLE_PROYECTOS; } }
        /// <summary>
        /// El id del tablero.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// El nombre del proyecto
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// La fecha de inicio del proyecto
        /// </summary>
        public DateTime Start;
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="input">La información base para crear un tablero</param>
        public Boolean Create(SQLite_Connector conn, Object input)
        {
            return false;
        }
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            this.Id = (int)result.GetValue<long>("prj_id");
            this.ProjectName = result.GetString("prj_name");
            this.Start = DateTime.Parse(result.GetString("start_date"));
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Project"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        public Project(SelectionResult[] result)
        {
            this.Parse(result);
        }

        /// <summary>
        /// Actualiza un registro del objeto en la base de datos
        /// </summary>
        /// <param name="input">La información para actualizar el tablero</param>
        public void Update(object input)
        {

        }

        public InsertField[] GetInsertFields()
        {
            throw new NotImplementedException();
        }
    }
}
