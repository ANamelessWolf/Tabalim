using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Temperatura
    {
        public int Grados;
        public double Factor;
        static IEnumerable<Temperatura> Temperaturas = new Temperatura[] {
            new Temperatura() { Grados = 25, Factor = 1.05 },
            new Temperatura() { Grados = 30, Factor = 1 },
            new Temperatura() { Grados = 35, Factor = 0.94 },
            new Temperatura() { Grados = 40, Factor = 0.88 },
            new Temperatura() { Grados = 45, Factor = 0.82 },
            new Temperatura() { Grados = 50, Factor = 0.75 },
            new Temperatura() { Grados = 55, Factor = 0.67 },
            new Temperatura() { Grados = 60, Factor = 0.58 },
            new Temperatura() { Grados = 65, Factor = 0.47 },
            new Temperatura() { Grados = 70, Factor = 0.33 }
        };
        public static double GetFactor(int Temperatura)
        {
            return Temperaturas.First(x => x.Grados >= Temperatura).Factor;
        }
    }
}
