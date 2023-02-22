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
        static int[] Corrientes = new int[] { 15, 20, 30, 40, 50, 70, 100, 125, 150, 175, 200, 225, 250, 300, 350, 400, 450, 500, 600, 700, 800, 1000, 1200, 1600 };
        public static Interruptor GetInterruptor(int polos, double corriente)
        {
            return new Interruptor() { Polos = polos, CorrienteMaxima = corriente <= Corrientes.Last() ? Corrientes.First(x => x > corriente) : Corrientes.Last() };
        }
        public override string ToString()
        {
            return String.Format("{0}P-{1}A", Polos, CorrienteMaxima);
        }
    }
}
