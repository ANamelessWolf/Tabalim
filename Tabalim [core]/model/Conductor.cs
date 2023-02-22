using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Conductor
    {
        const string ALIM_FORMAT = "{0}#{1}, {2}#{3}d - {4}";
        const string CANAL_FORMAT = "{0}{1} - {2}";
        //Salidas
        public String Alimentador => String.Format(ALIM_FORMAT, this.NoHilos[SelectedIndex] * this.NoTubos * (this.Single ? 1 : 2), this.Calibre + (IsCharola ? "Tipo CT" : ""), Tierra * this.NoTubos * (this.Single ? 1 : 2), this.CalibreTierra, this.Extra);
        public String Canalizacion => String.Format(CANAL_FORMAT, this.NoTubos * (this.Single ? 1 : 2), IsCharola ? "CH" : "T", IsCharola ? "91cm" : this.DiametroTubos[SelectedIndex]);
        //Datos
        int CorrienteMaxima;
        int CorrienteMaximaAl;
        int CorrienteMaximaCharola;
        public int NoTubos;
        String[] DiametroTubos;
        int[] NoHilos = new int[] { 4, 3, 2 };
        public String Calibre;
        int Tierra = 1;
        String CalibreTierra;
        int SelectedIndex = 0;
        bool Single = true;
        public bool IsCharola;
        public string Extra;
        //conductor SelectedConductor;
        /*static IEnumerable<Conductor> Data = new Conductor[]
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
        };*/
        static IEnumerable<Conductor> Data = new Conductor[]
        {
            new Conductor() { Calibre = "14", CorrienteMaxima = 15, CorrienteMaximaAl = 0, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "16mm", "16mm", "16mm" } },
            new Conductor() { Calibre = "12", CorrienteMaxima = 20, CorrienteMaximaAl = 0, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "16mm", "16mm", "16mm" } },
            new Conductor() { Calibre = "10", CorrienteMaxima = 30, CorrienteMaximaAl = 0, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "21mm", "16mm", "16mm" } },
            new Conductor() { Calibre = "8", CorrienteMaxima = 40, CorrienteMaximaAl = 0, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "27mm", "21mm", "21mm" } },
            new Conductor() { Calibre = "6", CorrienteMaxima = 55, CorrienteMaximaAl = 40, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "35mm", "27mm", "27mm" } },
            new Conductor() { Calibre = "4", CorrienteMaxima = 70, CorrienteMaximaAl = 55, CorrienteMaximaCharola = 125, DiametroTubos = new String[] { "35mm", "35mm", "35mm" } },
            new Conductor() { Calibre = "2", CorrienteMaxima = 95, CorrienteMaximaAl = 75, CorrienteMaximaCharola = 170, DiametroTubos = new String[] { "41mm", "35mm", "35mm" } },
            new Conductor() { Calibre = "1/0", CorrienteMaxima = 150, CorrienteMaximaAl = 120, CorrienteMaximaCharola = 230, DiametroTubos = new String[] { "53mm", "53mm", "53mm" } },
            new Conductor() { Calibre = "2/0", CorrienteMaxima = 175, CorrienteMaximaAl = 135, CorrienteMaximaCharola = 265, DiametroTubos = new String[] { "53mm", "53mm", "53mm" } },
            new Conductor() { Calibre = "3/0", CorrienteMaxima = 200, CorrienteMaximaAl = 155, CorrienteMaximaCharola = 310, DiametroTubos = new String[] { "63mm", "53mm", "53mm" } },
            new Conductor() { Calibre = "4/0", CorrienteMaxima = 230, CorrienteMaximaAl = 180, CorrienteMaximaCharola = 360, DiametroTubos = new String[] { "63mm", "63mm", "63mm" } },
            new Conductor() { Calibre = "250KCM", CorrienteMaxima = 255, CorrienteMaximaAl = 205, CorrienteMaximaCharola = 405, DiametroTubos = new String[] { "78mm", "63mm", "63mm" } },
            new Conductor() { Calibre = "300KCM", CorrienteMaxima = 285, CorrienteMaximaAl = 230, CorrienteMaximaCharola = 445, DiametroTubos = new String[] { "78mm", "78mm", "78mm" } },
            new Conductor() { Calibre = "350KCM", CorrienteMaxima = 310, CorrienteMaximaAl = 250, CorrienteMaximaCharola = 505, DiametroTubos = new String[] { "78mm", "78mm", "78mm" } },
            new Conductor() { Calibre = "400KCM", CorrienteMaxima = 350, CorrienteMaximaAl = 270, CorrienteMaximaCharola = 545, DiametroTubos = new String[] { "103mm", "78mm", "78mm" } },
            new Conductor() { Calibre = "500KCM", CorrienteMaxima = 400, CorrienteMaximaAl = 310, CorrienteMaximaCharola = 620, DiametroTubos = new String[] { "103mm", "103mm", "103mm" } },
            new Conductor() { Calibre = "600KCM", CorrienteMaxima = 0, CorrienteMaximaAl = 340, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "103mm", "103mm", "103mm" } },
            new Conductor() { Calibre = "700KCM", CorrienteMaxima = 0, CorrienteMaximaAl = 375, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "103mm", "103mm", "103mm" } },
            new Conductor() { Calibre = "750KCM", CorrienteMaxima = 0, CorrienteMaximaAl = 385, CorrienteMaximaCharola = 0, DiametroTubos = new String[] { "103mm", "103mm", "103mm" } }
        };
        static Tuple<int, string, string>[] Tierras = new Tuple<int, string, string>[]
        {
            new Tuple<int, string, string>(15, "14", "6"),
            new Tuple<int, string, string>(20, "12", "6"),
            new Tuple<int, string, string>(60, "10", "6"),
            new Tuple<int, string, string>(100, "8", "6"),
            new Tuple<int, string, string>(200, "6", "4"),
            new Tuple<int, string, string>(300, "4", "2"),
            new Tuple<int, string, string>(400, "2", "1/0"),
            new Tuple<int, string, string>(500, "2", "1/0"),
            new Tuple<int, string, string>(600, "1/0", "2/0"),
            new Tuple<int, string, string>(800, "1/0", "3/0"),
            new Tuple<int, string, string>(1000, "2/0", "4/0"),
            new Tuple<int, string, string>(1200, "3/0", "250KCM"),
            new Tuple<int, string, string>(1600, "4/0", "350KCM"),
            new Tuple<int, string, string>(2000, "250KCM", "400KCM"),
            new Tuple<int, string, string>(2500, "350KCM", "600KCM"),
            new Tuple<int, string, string>(3000, "400KCM", "600KCM"),
            new Tuple<int, string, string>(4000, "500KCM", "750KCM"),
            new Tuple<int, string, string>(5000, "700KCM", "1200KCM"),
            new Tuple<int, string, string>(6000, "800KCM", "1200KCM")
        };
        static Tuple<String, double>[] Areas = new Tuple<string, double>[]
        {
            new Tuple<String, double>("14", 2.082),
            new Tuple<String, double>("12", 3.307),
            new Tuple<String, double>("10", 5.26),
            new Tuple<String, double>("8", 8.367),
            new Tuple<String, double>("6", 13.3),
            new Tuple<String, double>("4", 21.2),
            new Tuple<String, double>("2", 33.6),
            new Tuple<String, double>("1/0", 53.49),
            new Tuple<String, double>("2/0", 67.43),
            new Tuple<String, double>("3/0", 85.01),
            new Tuple<String, double>("4/0", 107.2),
            new Tuple<String, double>("250KCM", 127),
            new Tuple<String, double>("300KCM", 152),
            new Tuple<String, double>("350KCM", 177),
            new Tuple<String, double>("400KCM", 203),
            new Tuple<String, double>("500KCM", 253),
            new Tuple<String, double>("600KCM", 304),
            new Tuple<String, double>("700KCM", 355),
            new Tuple<String, double>("750KCM", 380)
        };
        public static Conductor[] GetConductorOptions (int fases, double corriente, bool isCobre = true, int hilos = 0)
        {
            List<Conductor> result = new List<Conductor>();
            Conductor tmp =  Data.FirstOrDefault(x => (isCobre ? x.CorrienteMaxima : x.CorrienteMaximaAl) > corriente) ?? Data.Last();
            tmp.SelectedIndex = fases == 3 ? hilos == 4 ? 0 : 1 : 2;
            result.Add(tmp);
            tmp = Data.FirstOrDefault(x => (isCobre ? x.CorrienteMaxima : x.CorrienteMaximaAl) > corriente / 2) ?? Data.Last();
            if (result[0] != tmp)
            {
                tmp.SelectedIndex = result[0].SelectedIndex;
                tmp.Single = false;
                result.Add(tmp);
            }
            return result.ToArray();
        }
        public static String[] GetAvailableCalibres(double corriente, bool isCobre = true, bool isCharola = false)
        {
            if (isCharola)
                return Data.Where(x => corriente <= x.CorrienteMaximaCharola).Select(x => x.Calibre).ToArray();
            int maxValue = isCobre ? 150 : 120;
            if (corriente > maxValue)
                return Data.Where(x => x.CorrienteMaxima >= 150).Select(x => x.Calibre).ToArray();
            return Data.Where(x => corriente < (isCobre ? x.CorrienteMaxima : x.CorrienteMaximaAl)).Select(x => x.Calibre).ToArray();
        }
        public static int[] GetAllowedPipes(double corriente, string calibre, bool isCobre = true)
        {
            if (Data.Select(x => x.Calibre).ToList().IndexOf(calibre) < Data.Select(x => x.Calibre).ToList().IndexOf("1/0"))
                return new int[] { 1 };
            else {
                Conductor tmp = Data.First(x => x.Calibre == calibre);
                int value = isCobre ? tmp.CorrienteMaxima : tmp.CorrienteMaximaAl;
                return Enumerable.Range((int)Math.Ceiling(corriente / value), 15).ToArray();
            }
        }
        public static String GetMinCalibre(double corriente, bool isCobre = true)
        {
            return Data.First(x => corriente < (isCobre ? x.CorrienteMaxima : x.CorrienteMaximaAl))?.Calibre;
        }
        public static String GetTierra(double corriente, bool isCobre = true)
        {
            var t = Tierras.First(x => x.Item1 > corriente);
            var tierra = isCobre ? t.Item2 : t.Item3;
            return tierra;
        }

        public static String CalculateModifiedTierra(String calibreInicial, String calibreFinal, double corriente, bool isCobre = true, int noTubos = 1)
        {
            double factor = noTubos* Areas.First(x => x.Item1 == calibreFinal).Item2 / Areas.First(x => x.Item1 == calibreInicial).Item2;
            var t = Tierras.First(x => x.Item1 > corriente);
            var tierra = isCobre ? t.Item2 : t.Item3;
            double area = Areas.First(x => x.Item1 == tierra).Item2 * factor;
            return Areas.First(x => x.Item2 >= area).Item1;
        }
        public Conductor()
        {

        }

        internal Conductor(Conductor c)
        {
            this.Calibre = c.Calibre;
            this.CorrienteMaxima = c.CorrienteMaxima;
            this.CorrienteMaximaAl = c.CorrienteMaximaAl;
            this.CorrienteMaximaCharola = c.CorrienteMaximaCharola;
            this.DiametroTubos = c.DiametroTubos;
            //Test
            this.NoTubos = c.NoTubos;
            this.SelectedIndex = c.SelectedIndex;
            this.CalibreTierra = c.CalibreTierra;
            this.IsCharola = c.IsCharola;
            this.Extra = c.Extra;
        }
        public void SetCalibreTierra(string calibre)
        {
            this.CalibreTierra = calibre;
        }

        public static Conductor GetConductor (string calibre, double corriente, int hilos, int noTubos, bool isCobre, bool isCharola, string extra)
        {
            Conductor tmp = new Conductor(Data.First(x => x.Calibre == calibre));
            tmp.SelectedIndex = hilos == 4 ? 0 : hilos == 3 ? 1 : 2;
            tmp.NoTubos = noTubos;
            var t = Tierras.First(x => x.Item1 > corriente);
            tmp.CalibreTierra = isCobre ? t.Item2 : t.Item3;
            tmp.IsCharola = isCharola;
            tmp.Extra = extra;
            return tmp;
        }
        public override string ToString()
        {
            return Alimentador + ", " + Canalizacion;
        }
    }
}
