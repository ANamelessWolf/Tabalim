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
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para WinTableroSettings.xaml
    /// </summary>
    public partial class WinTableroSettings : MetroWindow
    {
        /// <summary>
        /// Si esta activa la bandera la información se guarda como
        /// </summary>
        public Boolean SaveAsMode;
        /// <summary>
        /// El tablero selecionado
        /// </summary>
        public Tablero ActiveTablero;
        /// <summary>
        /// El nombrero del tablero
        /// </summary>
        private String TabName { get { return this.ActiveTablero.NombreTablero; } }
        /// <summary>
        /// La descripción  del tablero
        /// </summary>
        private String Description { get { return this.ActiveTablero.Description; } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WinTableroSettings"/>.
        /// </summary>
        /// <param name="name">El nombre del tablero</param>
        /// <param name="desc">La descripción del tablero</param>
        public WinTableroSettings(Tablero tablero, Boolean saveMode = false)
        {
            this.SaveAsMode = saveMode;
            this.ActiveTablero = tablero;
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que la funcionalidad del botón cancelar
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        /// <summary>
        /// Maneja el evento que la funcionalidad del botón guardar
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtName.Text != String.Empty && this.txtDexc.Text != String.Empty && !this.SaveAsMode)
            {
                var controller = await this.ShowProgressAsync("Guardando cambios...", "Por favor espere");
                controller.SetCancelable(false);
                controller.SetIndeterminate();
                this.ActiveTablero.UpdateTableroTr(TabalimApp.CurrentProject, ((async (object result) =>
                  {
                      Object[] rData = result as Object[];
                      Boolean succed = (Boolean)rData[0];
                      String msg = (string)rData[1];
                      await controller.CloseAsync();
                      //await this.ShowMessageAsync(succed ? "Tablero Actualizado" : "Error", msg);
                      if (succed)
                      {
                          this.DialogResult = true;
                          this.Close();
                      }
                  })), this.txtName.Text, this.txtDexc.Text);
            }
            else if (this.txtName.Text != String.Empty && this.txtDexc.Text != String.Empty && SaveAsMode && TabalimApp.CurrentProject.Tableros.Values.Count(x => x.NombreTablero == this.txtName.Text) == 0)
                this.Close();
            else if (this.txtName.Text != String.Empty && this.txtDexc.Text != String.Empty && SaveAsMode && TabalimApp.CurrentProject.Tableros.Values.Count(x => x.NombreTablero == this.txtName.Text) > 0)
                await this.ShowMessageAsync("Nombre de Tablero en uso", "Favor de proporcionar un nombre único para guardar como el tablero.");
            else
                await this.ShowMessageAsync("Información incompleta", "Favor de proporcionar un nombre y una descripción.");
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtName.Text = this.TabName == String.Empty ? String.Format("Tablero {0:000}", TabalimApp.CurrentProject.Tableros.Count) : this.TabName;
            this.txtDexc.Text = this.Description == String.Empty ? "No hay descripción" : this.Description;
        }
    }
}
