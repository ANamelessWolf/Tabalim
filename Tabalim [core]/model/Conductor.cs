using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Conductor
    {
        const string ALIM_FORMAT = "{0}#{1}, {2}#{3}";
        const string CANAL_FORMAT = "{1}T - {2}";
        //Salidas
        public String Alimentador => String.Format(ALIM_FORMAT, this.NoHilos[SelectedIndex] * (Single ? 1 : 2), this.Calibre, Tierra * (Single ? 1 : 2), this.CalibreTierra);
        public String Canalizacion => String.Format(CANAL_FORMAT, this.NoTubos * (Single ? 1 : 2), this.DiametroTubos[SelectedIndex]);
        //Datos
        int CorrienteMaxima;
        int NoTubos;
        String[] DiametroTubos;
        int[] NoHilos = new int[] { 4, 3, 3 };
        public String Calibre;
        int Tierra = 1;
        String CalibreTierra;
        int SelectedIndex = 0;
        bool Single = true;
        Conductor SelectedConductor;
        static IEnumerable<Conductor> Data = new Conductor[]
        {
            new Conductor() { CorrienteMaxima = 20, NoTubos = 1, DiametroTubos = new String[] { "16mm", "16mm", "16mm" }, Calibre = "14", CalibreTierra = "14" },
            new Conductor() { CorrienteMaxima = 25, NoTubos = 1, DiametroTubos = new String[] { "16mm", "16mm", "16mm" }, Calibre = "12", CalibreTierra = "12" },
            new Conductor() { CorrienteMaxima = 30, NoTubos = 1, DiametroTubos = new String[] { "21mm", "16mm", "16mm" }, Calibre = "10", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 40, NoTubos = 1, DiametroTubos = new String[] { "27mm", "21mm", "21mm" }, Calibre = "8", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 50, NoTubos = 1, DiametroTubos = new String[] { "35mm", "27mm", "27mm" }, Calibre = "6", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 60, NoTubos = 1, DiametroTubos = new String[] { "35mm", "27mm", "27mm" }, Calibre = "6", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 70, NoTubos = 1, DiametroTubos = new String[] { "35mm", "35mm", "35mm" }, Calibre = "4", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 80, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 90, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 100, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 125, NoTubos = 1, DiametroTubos = new String[] { "53mm", "41mm", "41mm" }, Calibre = "1/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 150, NoTubos = 1, DiametroTubos = new String[] { "53mm", "53mm", "53mm" }, Calibre = "1/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 175, NoTubos = 1, DiametroTubos = new String[] { "53mm", "53mm", "53mm" }, Calibre = "2/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 200, NoTubos = 1, DiametroTubos = new String[] { "63mm", "53mm", "53mm" }, Calibre = "3/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 225, NoTubos = 1, DiametroTubos = new String[] { "63mm", "63mm", "63mm" }, Calibre = "4/0", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 250, NoTubos = 1, DiametroTubos = new String[] { "78mm", "63mm", "63mm" }, Calibre = "250MCM", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 300, NoTubos = 1, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 350, NoTubos = 1, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 400, NoTubos = 1, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 500, NoTubos = 2, DiametroTubos = new String[] { "78mm", "63mm", "63mm" }, Calibre = "250MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 600, NoTubos = 2, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 700, NoTubos = 2, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 800, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "300MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 900, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "2/0" },
            new Conductor() { CorrienteMaxima = 1000, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "2/0" },
            new Conductor() { CorrienteMaxima = 1100, NoTubos = 3, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "3/0" },
            new Conductor() { CorrienteMaxima = 1200, NoTubos = 4, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "3/0" },
            new Conductor() { CorrienteMaxima = 1600, NoTubos = 5, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "4/0" }
        };
        public static Conductor[] GetConductorOptions (int fases, double corriente, bool normal = true)
        {
            Conductor[] result = new Conductor[2];
            result[0] = Data.First(x => x.CorrienteMaxima > corriente);
            result[0].SelectedIndex = fases == 3 ? normal ? 0 : 1 : 2;
            result[1] = Data.First(x => x.CorrienteMaxima > corriente / 2);
            result[1].SelectedIndex = result[0].SelectedIndex;
            result[1].Single = false;
            return result;
        }
    }
}
