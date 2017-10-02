using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class CircuitoMonofasico : Circuito
    {
        public override double Corriente
        {
            get; set;
        }

        public override double CorrienteProteccion => throw new NotImplementedException();

        public override double CaidaVoltaje => throw new NotImplementedException();
    }
}
