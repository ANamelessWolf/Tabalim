using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define un alumbrado
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.Componente" />
    public class Alumbrado : Componente
    {
        /// <summary>
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public override double FactorProteccion { get { return 1.25; } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Alumbrado"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public Alumbrado(double potencia) :
            base(potencia)
        {

        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Alumbrado"/>.
        /// </summary>
        /// <param name="result">El resultado del query de selección</param>
        public Alumbrado(SelectionResult[] result) : base(result) { }
    }
}
