using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Conductor
    {
        const string ALIM_FORMAT = "{0}#{1}, {2}#{3}d";
        const string CANAL_FORMAT = "{0}T - {1}";
        //Salidas
        public String Alimentador => String.Format(ALIM_FORMAT, this.NoHilos[SelectedIndex] * (this.Single ? 1 : 2), this.Calibre, Tierra * (this.Single ? 1 : 2), this.CalibreTierra);
        public String Canalizacion => String.Format(CANAL_FORMAT, this.NoTubos * (this.Single ? 1 : 2), this.DiametroTubos[SelectedIndex]);
        //Datos
        int CorrienteMaxima;
        int CorrienteMaximaAl;
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
            new Conductor() { CorrienteMaxima = 20, CorrienteMaximaAl = 0, NoTubos = 1, DiametroTubos = new String[] { "16mm", "16mm", "16mm" }, Calibre = "14", CalibreTierra = "14" },
            new Conductor() { CorrienteMaxima = 25, CorrienteMaximaAl = 0, NoTubos = 1, DiametroTubos = new String[] { "16mm", "16mm", "16mm" }, Calibre = "12", CalibreTierra = "12" },
            new Conductor() { CorrienteMaxima = 30, CorrienteMaximaAl = 0, NoTubos = 1, DiametroTubos = new String[] { "21mm", "16mm", "16mm" }, Calibre = "10", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 40, CorrienteMaximaAl = 0, NoTubos = 1, DiametroTubos = new String[] { "27mm", "21mm", "21mm" }, Calibre = "8", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 50, CorrienteMaximaAl = 50, NoTubos = 1, DiametroTubos = new String[] { "35mm", "27mm", "27mm" }, Calibre = "6", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 60, CorrienteMaximaAl = 50, NoTubos = 1, DiametroTubos = new String[] { "35mm", "27mm", "27mm" }, Calibre = "6", CalibreTierra = "10" },
            new Conductor() { CorrienteMaxima = 70, CorrienteMaximaAl = 65, NoTubos = 1, DiametroTubos = new String[] { "35mm", "35mm", "35mm" }, Calibre = "4", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 80, CorrienteMaximaAl = 90, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 90, CorrienteMaximaAl = 90, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 100, CorrienteMaximaAl = 90, NoTubos = 1, DiametroTubos = new String[] { "41mm", "35mm", "35mm" }, Calibre = "2", CalibreTierra = "8" },
            new Conductor() { CorrienteMaxima = 125, CorrienteMaximaAl = 100, NoTubos = 1, DiametroTubos = new String[] { "53mm", "41mm", "41mm" }, Calibre = "1/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 150, CorrienteMaximaAl = 100, NoTubos = 1, DiametroTubos = new String[] { "53mm", "53mm", "53mm" }, Calibre = "1/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 175, CorrienteMaximaAl = 135, NoTubos = 1, DiametroTubos = new String[] { "53mm", "53mm", "53mm" }, Calibre = "2/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 200, CorrienteMaximaAl = 155, NoTubos = 1, DiametroTubos = new String[] { "63mm", "53mm", "53mm" }, Calibre = "3/0", CalibreTierra = "6" },
            new Conductor() { CorrienteMaxima = 225, CorrienteMaximaAl = 180, NoTubos = 1, DiametroTubos = new String[] { "63mm", "63mm", "63mm" }, Calibre = "4/0", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 250, CorrienteMaximaAl = 205, NoTubos = 1, DiametroTubos = new String[] { "78mm", "63mm", "63mm" }, Calibre = "250MCM", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 300, CorrienteMaximaAl = 250, NoTubos = 1, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "4" },
            new Conductor() { CorrienteMaxima = 350, CorrienteMaximaAl = 270, NoTubos = 1, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 400, CorrienteMaximaAl = 310, NoTubos = 1, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 500, CorrienteMaximaAl = 0, NoTubos = 2, DiametroTubos = new String[] { "78mm", "63mm", "63mm" }, Calibre = "250MCM", CalibreTierra = "2" },
            new Conductor() { CorrienteMaxima = 600, CorrienteMaximaAl = 0, NoTubos = 2, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 700, CorrienteMaximaAl = 0, NoTubos = 2, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 800, CorrienteMaximaAl = 0, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "300MCM", CalibreTierra = "1/0" },
            new Conductor() { CorrienteMaxima = 900, CorrienteMaximaAl = 0, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "2/0" },
            new Conductor() { CorrienteMaxima = 1000, CorrienteMaximaAl = 0, NoTubos = 3, DiametroTubos = new String[] { "78mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "2/0" },
            new Conductor() { CorrienteMaxima = 1100, CorrienteMaximaAl = 0, NoTubos = 3, DiametroTubos = new String[] { "103mm", "103mm", "103mm" }, Calibre = "500MCM", CalibreTierra = "3/0" },
            new Conductor() { CorrienteMaxima = 1200, CorrienteMaximaAl = 0, NoTubos = 4, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "350MCM", CalibreTierra = "3/0" },
            new Conductor() { CorrienteMaxima = 1600, CorrienteMaximaAl = 0, NoTubos = 5, DiametroTubos = new String[] { "103mm", "78mm", "78mm" }, Calibre = "400MCM", CalibreTierra = "4/0" }
        };
        public static Conductor[] GetConductorOptions (int fases, double corriente, bool normal = true)
        {
            List<Conductor> result = new List<Conductor>();
            Conductor tmp =  Data.FirstOrDefault(x => x.CorrienteMaxima > corriente) ?? Data.Last();
            tmp.SelectedIndex = fases == 3 ? normal ? 0 : 1 : 2;
            result.Add(tmp);
            tmp = Data.FirstOrDefault(x => x.CorrienteMaxima > corriente / 2) ?? Data.Last();
            if (result[0] != tmp)
            {
                tmp.SelectedIndex = result[0].SelectedIndex;
                tmp.Single = false;
                result.Add(tmp);
            }
            return result.ToArray();
        }
        public override string ToString()
        {
            return Alimentador + ", " + Canalizacion;
        }
    }
}
