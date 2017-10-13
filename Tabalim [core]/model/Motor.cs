using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

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
        public Motor(double potencia) : base(new Potencia(potencia, true).HP, true)
        {

        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Motor"/>.
        /// </summary>
        /// <param name="result">El resultado del query de selección</param>
        public Motor(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>
        /// Regresa el nuevo circuito creado
        /// </returns>
        public override Componente Clone()
        {
            return new Motor(this.Potencia.HP)
            {
                Id = -1,
                CircuitoName = this.Circuito.ToString(),
                Count = this.Count,
                ImageIndex = this.ImageIndex,
            };
        }
    }
}
