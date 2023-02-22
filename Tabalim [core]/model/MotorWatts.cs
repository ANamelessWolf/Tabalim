using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class MotorWatts : Componente
    {
        public override double FactorProteccion => 1.25;

        public MotorWatts(double potencia) : 
            base(potencia)
        {

        }
        public MotorWatts(SelectionResult[] results) :
            base(results)
        {

        }

        public override Componente Clone()
        {
            return new MotorWatts(this.Potencia.Watts)
            {
                Id = -1,
                CircuitoName = this.Circuito.ToString(),
                Count = this.Count,
                ImageIndex = this.ImageIndex
            };
        }
    }
}
