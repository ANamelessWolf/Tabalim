using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define un motor
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.Componente" />
    class Motor : Componente
    {
        /// <summary>
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public override double FactorProteccion { get { return 1.5; } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Motor"/>.
        /// </summary>
        /// <param name="potencia">Potencia en HP.</param>
        public Motor(double potencia) : base(new Potencia(potencia, true).Watts, true)
        {
        }
    }
}
