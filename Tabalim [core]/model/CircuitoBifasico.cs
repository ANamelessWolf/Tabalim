using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
