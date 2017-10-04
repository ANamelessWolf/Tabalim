using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;

namespace Tabalim.Core.mocking.model
{
    /// <summary>
    /// Define el elemento del circuito que se despliega en la lista
    /// Solo de manera de pruebas
    /// </summary>
    public class CtoItemMocking : CtoItem
    {
        /// <summary>
        /// Un grupo de circuitos que puede mostrar el control
        /// </summary>
        string[] Ctos = new String[] { "1,3", "1", "5,7", "2,4,6", "1,3", "1", "5,7", "2,4,6", "1,3", "1", "5,7", "2,4,6" };
        /// <summary>
        /// Un grupo de cantidades 
        /// </summary>
        int[] cCount = new int[] { 0, 1, 2, 3, 4, 5, 6, 0, 1, 2, 3, 4, 5, 6, 0, 1, 2, 3, 4, 5, 6 };
        /// <summary>
        /// El circuito definido en la aplicación.
        /// </summary>
        public override string CtoName
        {
            get
            {
                int index = new Random((int)DateTime.Now.Ticks).Next(Ctos.Length);
                return this.Ctos[index];
            }
        }
        /// <summary>
        /// El circuito definido en la aplicación.
        /// </summary>
        public override int CtoCount
        {
            get
            {
                int index = new Random((int)DateTime.Now.Ticks).Next(cCount.Length);
                return this.cCount[index];
            }
        }

    }
}
