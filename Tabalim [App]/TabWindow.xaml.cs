using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using Tabalim.Core.runtime;
using Tabalim.Core.view;

namespace Tabalim.App
{
    /// <summary>
    /// Interaction logic for TabWindow.xaml
    /// </summary>
    public partial class TabWindow : MetroWindow
    {
        public TabWindow()
        {
            InitializeComponent();
        }

        private void CreateTablero_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TableroPicker();
            dialog.ShowDialog();
            if (dialog.DialogResult.Value)
                App.Tabalim.CreateTableroTr(dialog.CreateTablero(), (Object result) => { tablerosList.UpdateList(); });
        }

        private async void CreateComponente_Click(object sender, RoutedEventArgs e)
        {
            if (TabalimApp.CurrentProject?.Tableros.Count > 0)
            {
                var dialog = new ComponentPicker();
                dialog.ShowDialog();
                if (dialog.DialogResult.Value)
                {
                    TabalimApp.CurrentTablero.AddComponentTr(dialog.SelectedComponent,
                        (Object result) =>
                        {
                            circuitosList.Refresh();
                        });
                }
            }
            else
                await (this as MetroWindow).ShowMessageAsync("", "Debe existir al menos un tablero.");
        }

        private void circuitosList_IsRefreshed(object sender, RoutedEventArgs e)
        {
            tablero.UpdateData();
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (TabalimApp.CurrentProject?.Tableros.Count > 0)
            {
                var win = new WinTableroSettings(TabalimApp.CurrentTablero);
                win.ShowDialog();
                if (win.DialogResult.Value)
                {
                    this.ExporCurrentTablero();
                    tablerosList.UpdateList();
                }
            }
            else
                await (this as MetroWindow).ShowMessageAsync("", "Debe existir al menos un tablero.");
        }

        private void Abrir_Click(object sender, RoutedEventArgs e)
        {
            this.ImportTablero(TabalimApp.CurrentProject, (object result) => { tablerosList.UpdateList(); });
        }

        private void tablerosList_TableroChanged(object sender, RoutedEventArgs e)
        {
            circuitosList.Refresh();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            new MainWindow().Show();
        }
    }
}
