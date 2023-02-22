using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Fusible
    {
        public int CorrienteMaxima = 0;
        static int[] Corrientes = new int[] { 1, 2, 4, 6, 10, 16, 25, 32, 40, 50, 63, 75, 100, 125, 160, 200 };
        public static Fusible GetFusible(double corriente)
        {
            return new Fusible() { CorrienteMaxima = corriente <= Corrientes.Last() ? Corrientes.First(x => x >= corriente) : Corrientes.Last() };
        }
        public override string ToString()
        {
            if (CorrienteMaxima == 0)
                return String.Empty;
            return String.Format("{0}A", CorrienteMaxima);
        }
    }
}
