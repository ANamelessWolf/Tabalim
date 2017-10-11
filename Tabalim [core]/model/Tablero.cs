using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la tabla de tablero que utiliza la aplicación
    /// </summary>
    public class Tablero : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// El id del tablero.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// El id del proyecto al que pertenecen los tableros.
        /// </summary>
        public int ProjectId;
        /// <summary>
        /// El nombre de la tabla que administra tableros
        /// </summary>
        public string TableName { get { return TABLE_TABLERO; } }
        /// <summary>
        /// El sistema que ocupa el tablero.
        /// </summary>
        public SistemaFases Sistema;
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string NombreTablero;
        /// <summary>
        /// La descripición del tablero
        /// </summary>
        public string Description;
        /// <summary>
        /// La colección de circuitos que compone un tablero
        /// </summary>
        public Dictionary<String, Circuito> Circuitos;
        /// <summary>
        /// La colección de componentes que se conectan en un tablero
        /// </summary>
        public Dictionary<int, Componente> Componentes;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Tablero"/>.
        /// </summary>
        /// <param name="result">La fila obtenida en el query de selección</param>
        public Tablero(SelectionResult[] result) : this()
        {
            this.Parse(result);
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Tablero"/>.
        /// </summary>
        public Tablero()
        {
            Circuitos = new Dictionary<string, Circuito>();
            Componentes = new Dictionary<int, Componente>();
            this.Description = String.Empty;
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">El objeto de conexión actual</param>
        /// <param name="input">La entrada que necesita la conexión.</param>
        public Boolean Create(SQLite_Connector conn, Object input)
        {
            return conn.Insert(this);
        }
        /// <summary>
        /// Guarda el componente seleccionado en el tablero actual
        /// </summary>
        /// <param name="componente">El componente seleccionado.</param>
        /// <param name="circuito">El circuito del componente seleccionado.</param>
        public void Update(Object input)
        {
        }
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>("tab_id");
                this.ProjectId = (int)result.GetValue<long>("prj_id");
                this.NombreTablero = result.GetString("tab_name");
                this.Description = result.GetString("tab_desc");
                this.Sistema = result.GetInteger("sys_index").GetSystem();
                this.Sistema.TpAlimentacion = result.GetValue<Boolean>("is_interruptor") ? TipoAlimentacion.Interruptor : TipoAlimentacion.Zapata;
                this.Sistema.Polo = result.GetInteger("polos");
                this.Sistema.Temperatura = result.GetInteger("temperature");
            }
            catch (Exception exc)
            {

                throw exc;
            }

        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>Los campos de inserción del objeto.</returns>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsNumber("prj_id", this.ProjectId),
                this.CreateFieldAsString("tab_name", this.NombreTablero),
                this.CreateFieldAsString("tab_desc", this.Description),
                this.CreateFieldAsNumber("sys_index", this.Sistema.GetIndexOfSystem()),
                this.CreateFieldAsNumber("is_interruptor", this.Sistema.TpAlimentacion == TipoAlimentacion.Interruptor ? 1 : 0),
                this.CreateFieldAsNumber("polos", this.Sistema.Polo),
                this.CreateFieldAsNumber("temperature", this.Sistema.Temperatura)
            };
        }
    }
}
