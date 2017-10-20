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
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey { get { return "tab_id"; } }
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
        /// Values the tuple.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        public void LoadComponentesAndCircuits(SQLite_Connector conn)
        {
            var ctos = conn.Select<Circuito>(TABLE_CIRCUIT.SelectAll(this.CreatePrimaryKeyCondition()), Circuito.CircuitoParser);
            //Guarda los circuitos existentes
            string compQ;
            foreach (Circuito c in ctos)
            {
                compQ = TABLE_COMPONENT.SelectAll(String.Format("\"cir_id\" = {0}", c.Id));
                if (!this.Circuitos.ContainsKey(c.ToString()))
                    this.Circuitos.Add(c.ToString(), c);
                var cmps = conn.Select<Componente>(compQ, Componente.ComponentParser);
                cmps.ForEach(x =>
                {
                    x.Circuito = c;
                    x.CircuitoName = c.ToString();
                    this.Componentes.Add(x.Id, x);
                    c.Componentes.Add(x.Id, x);
                });
            }
        }
        /// <summary>
        /// Realizá una copia del tablero actual
        /// </summary>
        /// <returns>La copia del tablero actual</returns>
        public Tablero Clone(out List<Componente> cmps, out List<Circuito> ctos)
        {
            Tablero t = new Tablero()
            {
                Description = this.Description,
                NombreTablero = this.NombreTablero,
                ProjectId = 1,
                Sistema = this.Sistema
            };
            cmps = new List<Componente>();
            ctos = new List<Circuito>();
            foreach (Circuito cto in this.Circuitos.Values)
                ctos.Add(cto.Clone());
            foreach (Componente cmp in this.Componentes.Values)
                cmps.Add(cmp.Clone());
            return t;
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
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
        public Boolean Update(SQLite_Connector conn, params KeyValuePair<String, Object>[] input)
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
        public bool Delete(SQLite_Connector conn)
        {
            Boolean ctoFlag = false, tabFlag = false;
            //El circuito borrará los componentes
            string[] keys = this.Circuitos.Keys.ToArray();
            foreach (string key in keys)
                ctoFlag = this.Circuitos[key].Delete(conn);
            tabFlag = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (tabFlag)
                TabalimApp.CurrentProject.Tableros.Remove(this.Id);
            return ctoFlag && tabFlag;
        }
        #region ISQLiteParser
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>(this.PrimaryKey);
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
        /// <summary>
        /// Obtiene los campos de actualización de un objeto
        /// </summary>
        /// <param name="input">La entrada del campo actualizar</param>
        /// <returns>
        /// El campo actualizar
        /// </returns>
        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            UpdateField value;
            switch (input.Key)
            {
                case "prj_id":
                case "temperature":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "tab_name":
                case "tab_desc":
                    value = input.CreateFieldAsString(this.TableName, input.Value);
                    break;
                case "is_interruptor":
                    TipoAlimentacion tpAlim = (TipoAlimentacion)input.Value;
                    int val = tpAlim == TipoAlimentacion.Interruptor ? 1 : 0;
                    value = input.CreateFieldAsNumber(this.TableName, val);
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
        }
        /// <summary>
        /// Actualiza el modelo en caso que el query fuese actualizado de manera correcta
        /// </summary>
        /// <param name="input">Los datos de entrada que se usarón para actualizar</param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                switch (val.Key)
                {
                    case "prj_id":
                        this.ProjectId = (int)val.Value;
                        break;
                    case "tab_name":
                        this.NombreTablero = val.Value.ToString();
                        break;
                    case "tab_desc":
                        this.Description = val.Value.ToString();
                        break;
                    case "is_interruptor":
                        this.Sistema.TpAlimentacion = (TipoAlimentacion)val.Value;
                        break;
                    case "temperature":
                        this.Sistema.Temperatura = (double)val.Value;
                        break;
                }
        }
        #endregion
    }
}
