using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    class CircuitoTrifasico : Circuito
    {

        public override double Corriente
        {
            get
            {
                if (HasMotor)
                    return 0;
                return PotenciaTotal / (Math.Sqrt(3) * Tension.Value);
            }
        }

        public override double CaidaVoltaje { get => (2 * Math.Sqrt(3) * Longitud * CorrienteProteccion) / (Tension.Value * Calibre.AreaTransversal); }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoTrifasico"/>.
        /// </summary>
        public CircuitoTrifasico(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoTrifasico"/>.
        /// </summary>
        public CircuitoTrifasico() : base() { }
    }
}
