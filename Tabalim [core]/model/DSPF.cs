using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class DSPF
    {
        public int CorrienteMaxima = 0;
        int Polos = 0;
        static int[] Corrientes = new int[] { 30, 60, 100, 200, 400, 600, 800, 1200, 1400, 1600, 1800 };
        public static DSPF GetDSPF(int polos, double corriente)
        {
            return new DSPF() { Polos = polos, CorrienteMaxima = corriente <= Corrientes.Last() ? Corrientes.First(x => x > corriente) : Corrientes.Last() };
        }
        public override string ToString()
        {
            if (Polos == 0 && CorrienteMaxima == 0)
                return String.Empty;
            return String.Format("{0}P-{1}A", Polos, CorrienteMaxima);
        }
    }
}
