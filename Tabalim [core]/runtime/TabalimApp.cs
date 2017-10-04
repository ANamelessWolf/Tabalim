using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.model;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.runtime
{
    /// <summary>
    /// Define el acceso a la aplicación Tabalim
    /// </summary>
    public class TabalimApp
    {
        /// <summary>
        /// Define el acceso a la base de datos de la aplicación
        /// </summary>
        /// <value>
        /// La ruta de la aplicación
        /// </value>
        public static string AppDBPath
        {
            get
            {
                string path = Assembly.GetAssembly(typeof(TabalimApp)).Location;
                path = Path.Combine(Path.GetDirectoryName(path), DATA_FOLDER, APP_DB_FILE);
                return path;
            }
        }
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
