using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using Tabalim.Core.model.raw;
using Tabalim.Core.runtime;
using Tabalim.Core.view;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for AlimWindow.xaml
    /// </summary>
    public partial class AlimWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlimWindow"/> class.
        /// </summary>
        public AlimWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la tarea de crear una nueva línea
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void CreateLinea_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AlimentadorPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
            {
                App.Tabalim.CreateAlimentadorTr(dialog.SelectedLinea.ToAlimInput(TabalimApp.CurrentProject), dialog.SelectedLinea.Destination,
                (Object result, int alimId) =>
                    {
                        if (result is bool && (bool)result)
                        {
                            TabalimApp.CurrentProject.Lineas.Add(alimId, dialog.SelectedLinea);
                            alimTable.SetItemSource(TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                        }

                    });
            }
        }
        /// <summary>
        /// Maneja el evento que realizá la tarea de hacer clic en el boton de abrir
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void Abrir_Click(object sender, RoutedEventArgs e)
        {
            this.ImportProject(App.Tabalim, (Boolean isLoaded) =>
            {
                if (isLoaded)
                {
                    SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
                    {
                        TransactionTask = App.Tabalim.InitApplication,
                        TaskCompleted = (Object result) =>
                        {
                            App.Tabalim.AppLoaded(result);
                            alimTable.SetItemSource(TabalimApp.CurrentProject.Lineas.Values.Select(x => new AlimentadorRow(x)));
                        }
                    };
                    tr.Run(null);
                }
            });
        }
        /// <summary>
        /// Maneja el evento que realizá la tarea de hacer clic en el botón guardar
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            this.ExportCurrentProject(null);
        }
        /// <summary>
        /// Maneja el evento que realizá la tarea de hacer clic en el botón guardar como
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void Guardar_Como_Click(object sender, RoutedEventArgs e)
        {
            this.ExportCurrentProject(null, true);
        }
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            new MainWindow().Show();
        }

        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            alimTable.Copy();
        }
    }
}
