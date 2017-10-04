using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define un contacto
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.Componente" />
    public class Contacto : Componente
    {
        /// <summary>
        /// El factor de proteccion utilizado para calcular intensidad de corriente
        /// </summary>
        public override double FactorProteccion { get { return 1.25; } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Contacto"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public Contacto(double potencia) : base(potencia)
        {
          
        }
    }
}
