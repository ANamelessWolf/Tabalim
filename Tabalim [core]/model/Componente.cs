using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define el componente que se conectara al sistema
    /// </summary>
    public abstract class Componente
    {
        /// <summary>
        /// Especifica la potencia del componente
        /// </summary>
        public Potencia Potencia;
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
        }
    }
}
