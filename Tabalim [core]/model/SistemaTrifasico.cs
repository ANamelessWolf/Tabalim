using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
   public class SistemaTrifasico:SistemaFases
    {
        public override int Fases { get { return 3; } }
        public SistemaTrifasico()
        {

        }

        public int TensionAlNeutro
        {
            get { return (int)(Math.Round((double)this.Tension / Math.Sqrt(3))); }
        }
    }
}
