using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class BigMotor
    {
        const string MOTOR_FORMAT = "Motor {0}, {1} fases, {2}V";
        public Tension Tension { get; set; }
        public int Fases { get; set; }
        public Potencia Potencia { get; set; }
        public String PotenciaString { get { if (Potencia != null) return runtime.TabalimApp.Motores.First(x => x.HP == Potencia.HP).ToString(); return String.Empty; } }
        public override string ToString()
        {
            return String.Format(MOTOR_FORMAT, PotenciaString, Fases, Tension);
        }
    }
}
