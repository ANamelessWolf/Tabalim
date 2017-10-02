using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Calibre
    {
        public double AreaTransversal;
        public String AWG;
        public int CorrienteMaxima;
        static IEnumerable<Calibre> Calibres = new Calibre[]{
            new Calibre() { AreaTransversal = 2.082, AWG = "14", CorrienteMaxima = 15 },
            new Calibre() { AreaTransversal = 3.307, AWG = "12", CorrienteMaxima = 20 },
            new Calibre() { AreaTransversal = 5.26, AWG = "10", CorrienteMaxima = 30 },
            new Calibre() { AreaTransversal = 8.367, AWG = "8", CorrienteMaxima = 40 },
            new Calibre() { AreaTransversal = 13.3, AWG = "6", CorrienteMaxima = 55 }
        };
        public static Calibre GetCalibre(double corriente)
        {
            return Calibres.First(x => x.CorrienteMaxima > corriente);
        }
    }
}
