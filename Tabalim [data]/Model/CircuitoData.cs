using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Data.Model
{
    public class CircuitoData
    {
        public int Id { get; set; }
        public int TableroId { get; set; }
        public int[] Polos { get; set; }
        public Dictionary<int, ComponenteData> Componentes { get; set; }
        public TensionData Tension { get; set; }
        public double FactorAgrupacion { get; set; }
        public double Longitud { get; set; }
        public CalibreData Calibre { get; set; }
    }
}
