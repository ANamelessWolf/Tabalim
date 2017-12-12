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
        /// El id del tablero.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Establece el nombre de tabla
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName { get { return TABLE_PROYECTOS; } }
        /// <summary>
        /// La colección de tableros que contiene el proyecto
        /// </summary>
        public Dictionary<int, Tablero> Tableros;
        /// <summary>
        /// La colección de lineas de alimentadores que contiene el proyecto
        /// </summary>
        public Dictionary<int, Linea> Lineas;
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey { get { return "prj_id"; } }
        /// <summary>
        /// El nombre del proyecto
        /// </summary>
        public string ProjectName;
        /// <summary>
        /// La fecha de inicio del proyecto
        /// </summary>
        public DateTime Start;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Project"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        public Project(SelectionResult[] result)
        {
            this.Tableros = new Dictionary<int, Tablero>();
            this.Lineas = new Dictionary<int, Linea>();
            this.Parse(result);
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="input">La información base para crear un tablero</param>
        public Boolean Create(SQLite_Connector conn, Object input)
        {
            return conn.Insert(this);
        }
        /// <summary>
        /// Actualiza un registro del objeto en la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que recibe la operación</param>
        /// <returns>
        /// Verdadero si realizá la actualización.
        /// </returns>
        public bool Update(SQLite_Connector conn, KeyValuePair<string, object>[] input)
        {
            return this.UpdateTr(this.CreatePrimaryKeyCondition(), conn, input);
        }
        /// <summary>
        /// Borra la instancia de la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <returns>
        /// Verdadero si se borra el elemento
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(SQLite_Connector conn)
        {
            Boolean prjFlag = false, tabFlag = true;
            int[] keys = this.Tableros.Keys.ToArray();
            foreach (int key in keys)
                tabFlag = this.Tableros[key].Delete(conn) && tabFlag;
            return prjFlag && tabFlag;
        }
        #region ISQLiteParser
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
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>
        /// Los campos a insertar
        /// </returns>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsString("prj_name", this.ProjectName),
                this.CreateFieldAsDate("start_date", this.Start)
            };
        }
        /// <summary>
        /// Obtiene los campos de actualización de un objeto
        /// </summary>
        /// <param name="input">La entrada del campo actualizar</param>
        /// <returns>
        /// El campo actualizar
        /// </returns>
        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            if (input.Key == "prj_name")
                return input.CreateFieldAsString(this.TableName, input.Value);
            else
                return null;
        }
        /// <summary>
        /// Actualiza el modelo en caso que el query fuese actualizado de manera correcta
        /// </summary>
        /// <param name="input">Los datos de entrada que se usarón para actualizar</param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                if (val.Key == "prj_name")
                    this.ProjectName = val.Value.ToString();
        }
        #endregion
    }
}
