using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

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
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Contacto"/>.
        /// </summary>
        /// <param name="result">El resultado del query de selección</param>
        public Contacto(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>
        /// Regresa el nuevo circuito creado
        /// </returns>
        public override Componente Clone()
        {
            return new Contacto(this.Potencia.Watts)
            {
                Id = -1,
                CircuitoName = this.Circuito.ToString(),
                Count = this.Count,
                ImageIndex = this.ImageIndex,
            };
        }
    }
}
