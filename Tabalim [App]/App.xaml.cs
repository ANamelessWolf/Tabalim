using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tabalim.Core.runtime;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// El objeto de la aplicación TabalimApp
        /// </summary>
        public static TabalimApp Tabalim;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="App"/>.
        /// </summary>
        public App() : base()
        {
            Tabalim = new TabalimApp();
        }
    }
}
