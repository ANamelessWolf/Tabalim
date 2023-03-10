using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Data
{
    public class Tablero
    {
        public int tab_id { get; set; }
        public string tab_name { get; set; }
        public string tab_desc { get; set; }
        /// <summary>
        /// Guarda el indice al tipo de sistema que pertenece el sistema
        /// </summary>
        public string sys_index { get; set; }
        /// <summary>
        /// Una bandera que indica si el tablero es de tipo interruptor
        /// </summary>
        public bool is_interruptor { get; set; }
        /// <summary>
        /// El número de polos del sistema
        /// </summary>
        public int polos { get; set; }
        /// <summary>
        /// La temperatura del sistema
        /// </summary>
        public int temperature { get; set; }
        
    }
}
