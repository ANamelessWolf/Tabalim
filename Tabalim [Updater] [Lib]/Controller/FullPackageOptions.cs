using System;
using System.Collections.Generic;
using Tabalim.Updater.Core.Model;

namespace Tabalim.Updater.Core.Controller
{
    public class FullPackageOptions
    {
        /// <summary>
        /// Include assemblies from R19
        /// </summary>
        public Boolean IncludeAddinR19;
        /// <summary>
        /// Include assemblies from R22
        /// </summary>
        public Boolean IncludeAddinR22;
        /// <summary>
        /// The include assemblies from application
        /// </summary>
        public Boolean IncludeApp;
        /// <summary>
        /// The include database files
        /// </summary>
        public Boolean IncludeDatabase;
        /// <summary>
        /// The include resources files
        /// </summary>
        public Boolean IncludeResources;
        /// <summary>
        /// Filters the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>True if the file applies the filter</returns>
        public static Boolean Filter(FullPackageOptions opt, IEnumerable<InstalledFile> files)
        {
            return true;
        }

    }
}