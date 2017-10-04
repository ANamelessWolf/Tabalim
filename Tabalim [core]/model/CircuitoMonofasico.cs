using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define Circuito de un polo
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.Circuito" />
    public class CircuitoMonofasico : Circuito
    {
        public override double Corriente
        {
            get
            {
                if (HasMotor)
                    return 0;
                else
                    return PotenciaTotal / Tension.TensionAlNeutro;
            }
        }

        public override double CaidaVoltaje { get => (4 * Longitud * CorrienteProteccion) / (Tension.TensionAlNeutro * Calibre.AreaTransversal); }
    }
}
