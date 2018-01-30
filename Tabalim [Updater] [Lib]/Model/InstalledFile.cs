using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tabalim.Updater.Core.Assets.Constants;
namespace Tabalim.Updater.Core.Model
{
    public class InstalledFile
    {
        /// <summary>
        /// La ruta relativa al directorio de instalación
        /// del archivo
        /// </summary>
        [JsonProperty]
        public string[] RelativePath;
        /// <summary>
        /// The file name
        /// </summary>
        public string Name => RelativePath.LastOrDefault();
        /// <summary>
        /// Gets the name of the entry.
        /// </summary>
        /// <value>
        /// The name of the entry.
        /// </value>
        public string EntryName => String.Join("@", (IsAppFile ? new string[] { "App" } : new string[] { "Addin" }).Union(this.RelativePath).ToArray());
        /// <summary>
        /// Initializes a new instance of the <see cref="InstalledFile"/> class.
        /// </summary>
        /// <param name="pth">The file.</param>
        public InstalledFile(FileInfo file)
        {
            String rootDir;
            string pth = file.FullName.ToUpper();
            if (file.FullName.Contains(TABALIM_ADDIN_DIR))
            {
                IsAddinFile = true;
                rootDir = TABALIM_ADDIN_DIR;
                IsAddinR19File = pth.Contains("R19");
                IsAddinR22File = pth.Contains("R22");
            }
            else
            {
                IsAppFile = true;
                rootDir = TABALIM_APP_DIR;
            }
            IsAssembly = file.Extension.ToUpper() == ".DLL";
            IsDatabaseFile = new String[] { ".TABALIM", ".SQLITE" }.Contains(file.Extension.ToUpper());
            IsResourceFile = !IsAssembly;
            RelativePath = file.FullName.Replace(rootDir, "").Split('\\').Where(x => x != "").ToArray();
        }
        /// <summary>
        /// Verdadero si se trata de una librería dll
        /// </summary>
        [JsonProperty]
        public Boolean IsAssembly;
        /// <summary>
        /// Verdadero si se trata de un archivo que pertenece al addin
        /// </summary>
        [JsonProperty]
        public Boolean IsAddinFile;
        /// <summary>
        /// Verdadero si se trata de un archivo que pertenece a la aplicación
        /// </summary>
        [JsonProperty]
        public Boolean IsAppFile;
        /// <summary>
        /// Verdadero si se trata de un archivo que pertenece al addin de la
        /// versión de AutoCAD 2014
        /// </summary>
        [JsonProperty]
        public Boolean IsAddinR19File;
        /// <summary>
        /// Verdadero si se trata de un archivo que pertenece al addin de la
        /// versión de AutoCAD 2018
        /// </summary>
        [JsonProperty]
        public Boolean IsAddinR22File;
        /// <summary>
        /// Verdadero si se trata de un archivo que se utilizá como recurso
        /// </summary>
        [JsonProperty]
        public Boolean IsResourceFile;
        /// <summary>
        /// Verdadero si se trata de un archivo de base de datos
        /// </summary>
        [JsonProperty]
        public Boolean IsDatabaseFile;
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns>The installed file</returns>
        public String GetPath()
        {
            string pth = String.Empty;
            if (IsAppFile)
                pth = TABALIM_APP_DIR;
            else
                pth = TABALIM_ADDIN_DIR;
            foreach (string p in this.RelativePath)
                pth = Path.Combine(pth, p);
            return pth;
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(IsAppFile ? "App File: " : "Addin File: ");
            foreach (string p in this.RelativePath)
                sb.Append(p + "/");
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }
    }
}
