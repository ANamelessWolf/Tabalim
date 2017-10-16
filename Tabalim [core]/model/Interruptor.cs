using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Interruptor
    {
        int CorrienteMaxima;
        int Polos;
        static int[] Corrientes = new int[] { 15, 20, 30, 40, 50 };
        public static Interruptor GetInterruptor(int polos, double corriente)
        {
            return new Interruptor() { Polos = polos, CorrienteMaxima = Corrientes.First(x => x > corriente) };
        }
        public override string ToString()
        {
            return String.Format("{0}P-{1}A", Polos, CorrienteMaxima);
        }
    }
}
