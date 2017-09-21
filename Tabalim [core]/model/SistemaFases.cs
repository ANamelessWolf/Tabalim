using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public abstract class SistemaFases
    {
        /// <summary>
        /// Devuelve el número de fases que define el sistema.
        /// </summary>
        /// <value>
        /// El número de fases.
        /// </value>
        public abstract int Fases { get; }
        /// <summary>
        /// La tensión del sistema en volts
        /// </summary>
        public int Tension;
        /// <summary>
        /// Devuelve los polos que utilizá el sistema
        /// </summary>
        /// <value>
        /// Los polos disponibles para el sistema seleccionado
        /// </value>
        public int[] Polos { get; }
        /// <summary>
        /// La frecuencia del sistema
        /// </summary>
        public int Frecuencia;
        /// <summary>
        /// El número de hilos o líneas de cables que define el sistema.
        /// </summary>
        /// <value>
        /// El número de hilos.
        /// </value>
        public int Hilos { get { return this.Fases + 1; } }
    }
}
