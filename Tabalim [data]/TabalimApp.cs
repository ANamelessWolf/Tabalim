using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Data
{
    public class TabalimApp
    {
        /// <summary>
        /// El id del proyecto actual, en el nuevo modelo solo
        /// se tendra un proyecto por aplicación
        /// </summary>
        public int prj_id { get; set; } = 1;
        /// <summary>
        /// El nombre del proyecto
        /// </summary>
        public string prj_name { get; set; }
        /// <summary>
        /// La fecha de inicio del proyecto
        /// </summary>
        public DateTime start_date { get; set; }
        /// <summary>
        /// La última fecha de actualización
        /// </summary>
        public DateTime update_date { get; set; }
        /// <summary>
        /// El tablero asignado al proyecto
        /// </summary>
        public Tablero Tablero { get; set; }


    }
}
