using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class Aire : Componente
    {
        public Aire(double potencia) : base(potencia)
        {
        }

        public Aire(SelectionResult[] result) :
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
