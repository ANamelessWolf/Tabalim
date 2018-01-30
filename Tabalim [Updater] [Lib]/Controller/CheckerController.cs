using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Updater.Core.Model;
using static Tabalim.Updater.Core.Assets.Constants;
namespace Tabalim.Updater.Core.Controller
{
    /// <summary>
    /// Se encarga de revisar que este instalado el producto de tabalim
    /// </summary>
    public class CheckerController
    {
        /// <summary>
        /// Gets the installed files.
        /// </summary>
        /// <returns>The list of installed files</returns>
        public IEnumerable<InstalledFile> GetFiles()
        {
            List<FileInfo> files = new List<FileInfo>();
            this.GetFiles(ref files, new DirectoryInfo(TABALIM_APP_DIR));
            this.GetFiles(ref files, new DirectoryInfo(TABALIM_ADDIN_DIR));
            return files.Select(x => ToInstalledFile(x));
        }
        /// <summary>
        /// Converts a file info to an installed file
        /// </summary>
        /// <param name="file">The file to convert.</param>
        /// <returns>The installed file</returns>
        private InstalledFile ToInstalledFile(FileInfo file)
        {
            InstalledFile iFile;
            if (file.Extension.ToUpper() == ".DLL")
                iFile = new AssemblyFile(file);
            else
                iFile = new AssetFile(file);
            return iFile;
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="dir">The dir.</param>
        private void GetFiles(ref List<FileInfo> files, DirectoryInfo dir)
        {
            foreach (DirectoryInfo d in dir.GetDirectories())
                GetFiles(ref files, d);
            foreach (FileInfo f in dir.GetFiles())
                files.Add(f);
        }

    }
}
