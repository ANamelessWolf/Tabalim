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
    /// Define el componente que se conectara al sistema
    /// </summary>
    public abstract class Componente: IDatabaseMappable
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
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public abstract double FactorProteccion { get; }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Alumbrado"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public Componente(double potencia, bool ismotor=false)
        {
            this.Potencia = new Potencia(potencia, ismotor);
            this.Id = -1;
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="input">La entrada es un tablero</param>
        public void Create(Object input)
        {
            Tablero tablero = input as Tablero;
            this.Id = tablero.Componentes.Count+1;
            tablero.Componentes.Add(this.Id, this);
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
    }
}
