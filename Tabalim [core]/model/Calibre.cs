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
        public int CorrienteMaximaCu;
        public int CorrienteMaximaAl;
        static IEnumerable<Calibre> Calibres = new Calibre[]{
            new Calibre() { AreaTransversal = 2.082, AWG = "14", CorrienteMaximaCu = 15 },
            new Calibre() { AreaTransversal = 3.307, AWG = "12", CorrienteMaximaCu = 20 },
            new Calibre() { AreaTransversal = 5.26, AWG = "10", CorrienteMaximaCu = 30 },
            new Calibre() { AreaTransversal = 8.367, AWG = "8", CorrienteMaximaCu = 40 },
            new Calibre() { AreaTransversal = 13.3, AWG = "6", CorrienteMaximaCu = 50, CorrienteMaximaAl = 40 },
            new Calibre() { AreaTransversal = 21.2, AWG = "4", CorrienteMaximaCu = 70, CorrienteMaximaAl = 55 },
            new Calibre() { AreaTransversal = 33.6, AWG = "2", CorrienteMaximaCu = 115, CorrienteMaximaAl = 90 },
            new Calibre() { AreaTransversal = 53.49, AWG = "1/0", CorrienteMaximaCu = 150, CorrienteMaximaAl = 120 },
            new Calibre() { AreaTransversal = 67.43, AWG = "2/0", CorrienteMaximaCu = 175, CorrienteMaximaAl = 135 },
            new Calibre() { AreaTransversal = 85.01, AWG = "3/0", CorrienteMaximaCu = 200, CorrienteMaximaAl = 155 },
            new Calibre() { AreaTransversal = 107.2, AWG = "4/0", CorrienteMaximaCu = 230, CorrienteMaximaAl = 180 },
            new Calibre() { AreaTransversal = 127, AWG = "250", CorrienteMaximaCu = 255, CorrienteMaximaAl = 205 },
            new Calibre() { AreaTransversal = 152, AWG = "300", CorrienteMaximaCu = 285, CorrienteMaximaAl = 230 },
            new Calibre() { AreaTransversal = 177, AWG = "350", CorrienteMaximaCu = 310, CorrienteMaximaAl = 250 },
            new Calibre() { AreaTransversal = 203, AWG = "400", CorrienteMaximaCu = 335, CorrienteMaximaAl = 270 },
            new Calibre() { AreaTransversal = 253, AWG = "500", CorrienteMaximaCu = 380, CorrienteMaximaAl = 310 },
            new Calibre() { AreaTransversal = 304, AWG = "600", CorrienteMaximaCu = 420, CorrienteMaximaAl = 340 },
            new Calibre() { AreaTransversal = 355, AWG = "700", CorrienteMaximaCu = 460, CorrienteMaximaAl = 375 },
            new Calibre() { AreaTransversal = 380, AWG = "750", CorrienteMaximaCu = 475, CorrienteMaximaAl = 385 },
        };
        public static Calibre GetCalibre(double corriente, bool isCu = true)
        {
            if(isCu)
                return Calibres.FirstOrDefault(x => x.CorrienteMaximaCu > corriente) ?? Calibres.Last();
            return Calibres.FirstOrDefault(x => x.CorrienteMaximaAl > corriente) ?? Calibres.Last();
        }
        public static IEnumerable<Calibre> GetTableroCalibres(double corriente)
        {
            return Calibres.Where(x => x.CorrienteMaximaCu <= 115 && x.CorrienteMaximaCu > corriente);
        }
        public static Calibre GetCalibre(string awg)
        {
            if (Calibres.Count(x => x.AWG.CompareTo(awg) == 0) > 0)
                return Calibres.First(x => x.AWG.CompareTo(awg) == 0);
            //return Calibres.First();
            return null;
        }
        public override string ToString()
        {
            return "#" + this.AWG;
        }
    }
}
