using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;

namespace Tabalim.Core.runtime
{
    /// <summary>
    /// Define el acceso a la aplicación Tabalim
    /// </summary>
    public class TabalimApp
    {
        /// <summary>
        /// Define
        /// </summary>
        public static Tablero CurrentTablero;
        /// <summary>
        /// La lista de proyectos cargados
        /// </summary>
        public List<Tablero> Tableros;
    }
}
