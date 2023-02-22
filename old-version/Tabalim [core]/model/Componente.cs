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
    /// Define el componente que se conectara al sistema
    /// </summary>
    public abstract class Componente : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// El id del componente es único en la aplicación.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Establece el nombre de la base de datos
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName { get { return TABLE_COMPONENT; } }
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey { get { return "comp_id"; } }
        /// <summary>
        /// Especifica la potencia del componente
        /// </summary>
        public Potencia Potencia;
        /// <summary>
        /// Define o establece el valor de tipo de componente.
        /// </summary>
        /// <value>
        /// El tipo de componente
        /// </value>
        public ComponentType CType
        {
            get
            {
                if (this is Motor)
                    return ComponentType.Motor;
                else if (this is Alumbrado)
                    return ComponentType.Alumbrado;
                else if (this is Contacto)
                    return ComponentType.Contacto;
                else
                    return ComponentType.None;
            }
        }
        /// <summary>
        /// The image index
        /// </summary>
        public int ImageIndex;
        /// <summary>
        /// La cantidad de elementos asociados a un circuito
        /// </summary>
        public int Count;
        /// <summary>
        /// Define el circuito al que se conecta el componente
        /// </summary>
        public Circuito Circuito;
        /// <summary>
        /// Define el nombre del circuito al que pertenece el componente
        /// </summary>
        public String CircuitoName;
        /// <summary>
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>
        /// Regresa el nuevo clon creado
        /// </returns>
        public abstract Componente Clone();
        /// <summary>
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public abstract double FactorProteccion { get; }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public String Key { get { return ImageIndex + "-" + Potencia; } }
        public String XamlKey { get { return "k" + Key.Replace("-", "_").Replace(".", "_"); } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Alumbrado"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public Componente(double potencia, bool ismotor = false)
        {
            this.Potencia = new Potencia(potencia, ismotor);
            this.Id = -1;
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Componente"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public Componente(SelectionResult[] result)
        {
            this.Parse(result);
        }
        /// <summary>
        /// Crea un parser que generá un componente apartir de la información en la base de datos
        /// </summary>
        /// <typeparam name="SelectionResult">El resultado de la selección.</typeparam>
        /// <returns>La lista de componentes seleccionados</returns>
        public static List<Componente> ComponentParser(List<SelectionResult[]> qResult)
        {
            List<Componente> result = new List<Componente>();
            foreach (SelectionResult[] selResult in qResult)
            {
                int imgIndex = selResult.GetInteger("img_index");
                if (imgIndex.IsComponent(ComponentType.Alumbrado))
                    result.Add(new Alumbrado(selResult));
                else if (imgIndex.IsComponent(ComponentType.Contacto))
                    result.Add(new Contacto(selResult));
                else if (imgIndex.IsComponent(ComponentType.Motor))
                    result.Add(new Motor(selResult));
            }
            return result;
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que necesita la conexión.</param>
        public Boolean Create(SQLite_Connector conn, Object input)
        {
            Circuito cto = input as Circuito;
            this.Circuito = cto;
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
        /// <exception cref="NotImplementedException"></exception>
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
        public bool Delete(SQLite_Connector conn)
        {
            Boolean cmpFlag = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (cmpFlag)
            {
                Circuito.Componentes.Remove(this.Id);
                if (Circuito.Componentes.Count == 0)
                    Circuito.Delete(conn);
                TabalimApp.CurrentTablero.Componentes.Remove(this.Id);
            }
            return cmpFlag;
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
                int cirId = (int)result.GetValue<long>("cir_id");
                if (TabalimApp.CurrentTablero != null)
                    this.Circuito = TabalimApp.CurrentTablero.Circuitos.Values.FirstOrDefault(x => x.Id == cirId);
                this.ImageIndex = result.GetInteger("img_index");
                double potencia = result.GetValue<double>("potencia");
                this.Potencia = new Potencia(potencia, this.ImageIndex.IsComponent(ComponentType.Motor));
                this.Count = result.GetInteger("comp_count");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>Los campos a insertar de componentes</returns>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsNumber("cir_id",this.Circuito != null ? this.Circuito.Id : -1),
                this.CreateFieldAsNumber("potencia", this is Motor ? this.Potencia.HP : this.Potencia.Watts),
                this.CreateFieldAsNumber("img_index", this.ImageIndex),
                this.CreateFieldAsNumber("comp_count", this.Count),
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
                case "cir_id":
                    Circuito cto = (Circuito)input.Value;
                    value = input.CreateFieldAsNumber(this.TableName, cto.Id);
                    break;
                case "comp_count":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "potencia":
                    Potencia val = (Potencia)input.Value;
                    value = input.CreateFieldAsNumber(this.TableName, this is Motor ? val.HP : val.Watts);
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
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                switch (val.Key)
                {
                    case "potencia":
                        this.Potencia = (Potencia)val.Value;
                        break;
                    case "comp_count":
                        this.Count = (int)val.Value;
                        break;
                    case "cir_id":
                        this.Circuito.Componentes.Remove(this.Id);
                        if (this.Circuito.Componentes.Count == 0)
                            TabalimApp.CurrentTablero.Circuitos.Remove(this.Circuito.ToString());
                        this.Circuito = (Circuito)val.Value;
                        this.Circuito.Componentes.Add(this.Id, this);
                        break;
                }
        }
        #endregion
    }
}
