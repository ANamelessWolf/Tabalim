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
        /// Obtiene la potencia aparente.
        /// </summary>
        /// <value>
        /// Potencia aparente(VA).
        /// </value>
        public Double PotenciaAparente { get { return Potencia.Watts / 0.9; } }
        /// <summary>
        /// The image index
        /// </summary>
        public int ImageIndex;
        /// <summary>
        /// Define los circuitos a lso que se conecta, así como el numero de componentes por circuito
        /// </summary>
        public Dictionary<String, int> Circuitos;
        /// <summary>
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public abstract double FactorProteccion { get; }
    }
}
