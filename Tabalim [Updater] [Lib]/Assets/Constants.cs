using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Updater.Core.Assets
{
    public static class Constants
    {
        /// <summary>
        /// La compañía encargada de la aplicación
        /// </summary>
        public const String COMPANY = "CadLabs";
        /// <summary>
        /// El nombre del directorio temporal
        /// </summary>
        public const String TMP = "tmp";
        /// <summary>
        /// El nombre de la aplicación.
        /// </summary>
        public const String APP_NAME = "Tabalim";
        /// <summary>
        /// El nombre que se usa para escribir la versión del paquete
        /// </summary>
        public const String APP_VER_FILE = "elekid.dll";
        /// <summary>
        /// El nombre del paquete de actualización
        /// </summary>
        public const String PACKAGE_NAME = "package[{0}].uTab";
        /// <summary>
        /// El nombre del paquete de actualización
        /// </summary>
        public const String FULL_PACKAGE_NAME = "package[{0}].uFullTab";
        /// <summary>
        /// Devuelve la dirección del directorio de insatalación
        /// </summary>
        /// <value>
        /// El direcctorio de Tabalim
        /// </value>
        public static String TABALIM_APP_DIR
        {
            get
            {
                string rootDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(rootDir, COMPANY, APP_NAME);
            }
        }
        /// <summary>
        /// Devuelv ela dirección del directorio de plugin
        /// </summary>
        /// <value>
        /// El direcctorio de plugin
        /// </value>
        public static String TABALIM_ADDIN_DIR
        {
            get
            {
                string rootDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(rootDir, "Autodesk", "ApplicationPlugins", String.Format("{0}.bundle", APP_NAME));
            }
        }
    }
}
