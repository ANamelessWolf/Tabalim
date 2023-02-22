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
using System.Windows.Shapes;
using Tabalim.Core.controller;
using Tabalim.Core.model;
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
                    if (dialog.SelectedComponent.DeltaKey == -1)
                        TabalimApp.CurrentTablero.AddComponentTr(dialog.SelectedComponent,
                            (Object result) =>
                            {
                                Circuito cto = result as Circuito;
                                cto.UpdateCalibre();
                                //foreach (Calibre calibre in Calibre.GetTableroCalibres(cto.CorrienteCorregida)) {
                                //    cto.Calibre = calibre;
                                //    if (cto.CaidaVoltaje <= 2)
                                //        break;
                                //}
                                //cto.UpdateCircuitTr(null, calibre: cto.Calibre.AWG);
                                circuitosList.Refresh();
                            });
                    else
                        CreateDelta(dialog.SelectedComponent);
                }
            }
            else
                await (this as MetroWindow).ShowMessageAsync("", "Debe existir al menos un tablero.");
        }

        private void CreateDelta(Componente selectedComponent)
        {
            int originalCount = selectedComponent.Count;
            List<Componente> components = new List<Componente>();
            for(int i = 0; i < 3; i++)
            {
                Componente componente = selectedComponent.Clone();
                componente.Count = originalCount / 3 + (originalCount % 3 > i ? 1 : 0);
                //Permutar circuitos
                Circuito circuito = selectedComponent.Circuito.Clone();
                circuito.Polos = new int[] { selectedComponent.Circuito.Polos[i % 3], selectedComponent.Circuito.Polos[(i + 1) % 3] }.OrderBy(x => x).ToArray();
                componente.Circuito = circuito;
                //Obtener mayor Delta Key
                componente.DeltaKey = GetMaxDeltaKey(TabalimApp.CurrentTablero) + 1;

                components.Add(componente);
            }

            components.ForEach(x => TabalimApp.CurrentTablero.AddComponentTr(x,
                (Object result) =>
                {
                    circuitosList.Refresh();
                }));
        }

        private int GetMaxDeltaKey(Tablero currentTablero)
        {
            return currentTablero.Componentes.Select(x => x.Value.DeltaKey).Union(new int[] { 0 }).Max();
        }

        private void circuitosList_IsRefreshed(object sender, RoutedEventArgs e)
        {
            tablero.UpdateData();
        }

        private async void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (TabalimApp.CurrentProject?.Tableros.Count > 0)
            {
                if (File.Exists(TabalimApp.CurrentTablero.Path))
                    this.ExporCurrentTablero(() => this.tablerosList.UpdateList());
                else
                {
                    var win = new WinTableroSettings(TabalimApp.CurrentTablero);
                    win.ShowDialog();
                    if (win.DialogResult.Value)
                        this.ExporCurrentTablero(() => this.tablerosList.UpdateList());
                }
            }
            else
                await (this as MetroWindow).ShowMessageAsync("", "Debe existir al menos un tablero.");
        }
        /// <summary>
        /// Maneja el evento que realizá la tarea de hacer clic en el botón guardar como
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (TabalimApp.CurrentProject?.Tableros.Count > 0)
            {
                var win = new WinTableroSettings(TabalimApp.CurrentTablero, true);
                win.ShowDialog();
                if (win.DialogResult.Value)
                    this.SaveCurrentTableroAs(win.SelectedTabName, win.SelectedDescription, 
                        () => this.tablerosList.UpdateList());
            }
            else
                await (this as MetroWindow).ShowMessageAsync("Error", "Debe existir al menos un tablero.");
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

        private void tablerosList_TableroClonned(object sender, RoutedEventArgs e)
        {
            TableroEventArgs args = e as TableroEventArgs;
            Tablero tab = App.Tabalim.Tableros.FirstOrDefault(x => x.Id == args.TableroId);
            if (tab != null)
                App.Tabalim.CloneCurrentTablero(tab, (Object result) => { tablerosList.UpdateList(); });
        }

    }
}
