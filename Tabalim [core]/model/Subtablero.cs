using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class Subtablero : Componente
    {
        public Subtablero(double potencia) : base(potencia)
        {
        }

        public Subtablero(SelectionResult[] result) :
            base(result)
        {

        }

        public override double FactorProteccion => 1.25;

        public override Componente Clone()
        {
            return new Aire(this.Potencia.Watts)
            {
                Id = -1,
                CircuitoName = this.Circuito.ToString(),
                Count = this.Count,
                ImageIndex = this.ImageIndex
            };
        }
    }
}
