using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    class CircuitoBifasico : Circuito
    {

        public override double Corriente
        {
            get
            {
                if (HasMotor)
                    return 0;
                return PotenciaTotal / Tension.Value;
            }
        }

        public override double CaidaVoltaje { get => (2 * Math.Sqrt(3) * Longitud * CorrienteProteccion) / (Tension.Value * Calibre.AreaTransversal); }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoBifasico"/>.
        /// </summary>
        public CircuitoBifasico(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoBifasico"/>.
        /// </summary>
        public CircuitoBifasico() : base() { }
    }
}
