using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class TableroItem
    {
        public int Id => Tablero.Id;
        public String Name => Tablero.NombreTablero;
        public String Description => Tablero.Description;
        public int Percentage => (Tablero.Circuitos.Values.SelectMany(x => x.Polos).Count() * 100 / Tablero.Sistema.Polo);
        public Tablero Tablero;
        public TableroItem(Tablero t)
        {
            this.Tablero = t;
        }
    }
}
