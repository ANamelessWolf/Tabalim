using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Data
{
    public class Circuito
    {
        public int cir_id { get; set; }
        public int tab_id { get; set; }
        public string polos { get; set; }
        public double fac_agrup { get; set; }
        public double longitud { get; set; }
        public string calibre { get; set; }

        public Componente[] Componentes;
    }
}
