using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Updater.Core.Model;
using static Tabalim.Updater.Core.Assets.Constants;
namespace Tabalim.Updater.Core.Controller
{
    /// <summary>
    /// Creates an application package
    /// </summary>
    public class PackageMaker
    {
        /// <summary>
        /// True if the package is for an assembly
        /// </summary>
        public readonly Boolean IsAssemblyPackage;
        /// <summary>
        /// True if is a full package
        /// </summary>
        public readonly Boolean IsFullPackage;
        /// <summary>
        /// The options
        /// </summary>
        public FullPackageOptions Options;
        /// <summary>
        /// The files to add to the package
        /// </summary>
        public IEnumerable<InstalledFile> Files;
        /// <summary>
        /// The background worker
        /// </summary>
        BackgroundWorker Worker;
        /// <summary>
        /// The xml information schema
        /// </summary>
        public object Info;
        /// <summary>
        /// Update details
        /// </summary>
        public string Comments;
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageMaker"/> class.
        /// </summary>
        /// <param name="files">The files to add to the package.</param>
        /// <param name="isAssemblyPackage">if set to <c>true</c> [is assembly package].</param>
        public PackageMaker(IEnumerable<InstalledFile> files, Boolean isAssemblyPackage = true)
        {
            this.IsAssemblyPackage = isAssemblyPackage;
            this.IsFullPackage = !isAssemblyPackage;
            this.Files = files;
        }

        public void MakePackage()
        {
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.DoWork += CreatingPackageWork;
            Worker.ProgressChanged += Progress_Changed;
            Worker.WorkerReportsProgress = true;
            Worker.RunWorkerCompleted += Package_Completed;
            Worker.RunWorkerAsync(Files);
        }
        /// <summary>
        /// Handles the Completed event of the Package control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void Package_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            AssemblyFile versionFile = this.Files.FirstOrDefault(x => x.Name == APP_VER_FILE) as AssemblyFile;
            if (this.IsAssemblyPackage)
                this.Info = new assembly_patch()
                {
                    app_name = APP_NAME,
                    description = Comments,
                    version = versionFile.GetVersion().ToString()
                };
            else
                this.Info = new full_patch()
                {

                };
            if (e.Result is Exception)
                Console.WriteLine((e.Result as Exception).Message);
            else
                Console.WriteLine(e.Result as String);
        }
        /// <summary>
        /// Handles the Changed event of the Progress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void Progress_Changed(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(String.Format("{0:P2} {1}", e.ProgressPercentage, (string)e.UserState));
        }
        /// <summary>
        /// Creatings the package work.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void CreatingPackageWork(object sender, DoWorkEventArgs e)
        {
            IEnumerable<InstalledFile> files = (IEnumerable<InstalledFile>)e.Argument;
            AssemblyFile versionFile = files.FirstOrDefault(x => x.Name == APP_VER_FILE) as AssemblyFile;
            String tmpDir = Path.Combine(TABALIM_APP_DIR, TMP);
            String package = Path.Combine(TABALIM_APP_DIR, String.Format(this.IsAssemblyPackage ? PACKAGE_NAME : FULL_PACKAGE_NAME, versionFile.GetVersion().ToString()));
            try
            {
                //1: Se realiza la limpieza del directorio temporal
                if (Directory.Exists(tmpDir))
                    new DirectoryInfo(tmpDir).GetFiles().ToList().ForEach(x => x.Delete());
                else
                    Directory.CreateDirectory(tmpDir);
                (sender as BackgroundWorker).ReportProgress(0, "Copiando archivos del paquete...");
                //2: Copiamos los archivos al directorio temporal
                foreach (var file in files)
                    File.Copy(file.GetPath(), Path.Combine(tmpDir, file.EntryName));
                (sender as BackgroundWorker).ReportProgress(50, "Archivos copiados");
                //3: Se crea el archivo zip
                (sender as BackgroundWorker).ReportProgress(50, "Generando paquete...");
                ZipFile.CreateFromDirectory(tmpDir, package);
                e.Result = String.Format("Paquete creado en {0}", package);
            }
            catch (Exception exc)
            {
                e.Result = exc;
            }
        }
    }
}
