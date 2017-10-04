using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    class CircuitoBifasico : Circuito
    {
        public override double Corriente => throw new NotImplementedException();

        public override double CaidaVoltaje => throw new NotImplementedException();
    }
}
