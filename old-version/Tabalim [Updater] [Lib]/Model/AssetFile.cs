using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Updater.Core.Model
{
    public class AssetFile : InstalledFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFile"/> class.
        /// </summary>
        /// <param name="file">The assembly file</param>
        public AssetFile(FileInfo file) : base(file)
        {
        }
        /// <summary>
        /// Obtiene la fecha de creación del archivo
        /// </summary>
        /// <returns>La fecha de creación del archivo</returns>
        public DateTime GetFileDate()
        {
            return new FileInfo(this.GetPath()).CreationTime;
        }
    }
}
