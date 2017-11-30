using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using Tabalim.Core.runtime;
using Tabalim.Core.view;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the Click event of the tabBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void tabBtn_Click(object sender, RoutedEventArgs e)
        {
            InitModule(() => 
            {
                new TabWindow().Show();
                this.Close();
            });
        }
        /// <summary>
        /// Inicializa un modulo en la aplicación y realiza una validación antes
        /// de ejecutar el modulo
        /// </summary>
        /// <param name="task">La acción que inicia el modulo.</param>
        public async void InitModule(Action task)
        {
            var tabalim = App.Tabalim;
            if (tabalim.OpenProjects == null && !File.Exists(TabalimApp.AppDBPath))
            {
                await this.ShowMessageAsync("Error en la aplicación", String.Format("No existe la BD. Favor de revisar que este instalado el archivo\n{0}\nUna vez instalado reinicie la aplicación.", TabalimApp.AppDBPath));
            }
            else
            {
                task();
            }
        }
        /// <summary>
        /// Handles the Click event of the alimBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void alimBtn_Click(object sender, RoutedEventArgs e)
        {
            InitModule(() =>
            {
                new AlimentadorPicker().Show();
                this.Close();
            });
        }
    }
}
