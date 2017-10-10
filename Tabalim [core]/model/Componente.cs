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
                    return ComponentType.Alumbrado;
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
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public abstract double FactorProteccion { get; }
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
        /// Inicializa una nueva instancia de la clase <see cref="Alumbrado"/>.
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
        /// <param name="conn">El objeto de conexión actual</param>
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
        /// <param name="input"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(object input)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>("comp_id");
                int cirId = (int)result.GetValue<long>("cir_id");
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
    }
}
