using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public abstract class Circuito
    {
        public int[] Polos;
        public Dictionary<Componente, int> Componentes;
        public Tension Tension;
        public double PotenciaTotal;
        public abstract double Corriente { get; set; }
        public double FactorTemperatura;
        public double FactorAgrupacion;
        public double CorrienteCorregida { get { return Corriente / (FactorTemperatura * FactorAgrupacion); } }
        public double Longitud;
        public abstract double CorrienteProteccion { get; }

        public Calibre Calibre { get { return Calibre.GetCalibre(CorrienteProteccion); } }
        public abstract double CaidaVoltaje { get; }
        public double[] PotenciaEnFases
        {
            get
            {
                double[] arr = new double[3];
                if (Polos.Length > 0 && PotenciaTotal > 0)
                    foreach (int polo in Polos)
                        arr[(int)((polo - 1) / 2 % 3)] = PotenciaTotal / Polos.Length;
                return arr;
            }
        }
        public String Interruptor;
    }
}
