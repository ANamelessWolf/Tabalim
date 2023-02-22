using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class TableroMockItem
    {
        public int Id => Tablero.Id;
        public String Name => Tablero.Name;
       
        public TableroMock Tablero;
        public TableroMockItem(TableroMock t)
        {
            this.Tablero = t;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
