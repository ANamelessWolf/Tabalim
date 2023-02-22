using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Updater.Core.Model
{
    public class AssemblyFile : InstalledFile
    {
        /// <summary>
        /// Verdadero si se trata del archivo principal
        /// </summary>
        public Boolean IsMainAssembly;
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFile"/> class.
        /// </summary>
        /// <param name="file">The assembly file</param>
        public AssemblyFile(FileInfo file) : base(file)
        {
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The assembly version</returns>
        public Version GetVersion()
        {
            Assembly assembly = Assembly.LoadFrom(this.GetPath());
            return assembly.GetName().Version;
        }
    }
}
