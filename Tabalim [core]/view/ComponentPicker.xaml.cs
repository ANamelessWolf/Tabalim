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
using Tabalim.Core.model;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para ComponentPicker.xaml
    /// </summary>
    public partial class ComponentPicker : MetroWindow
    {
        /// <summary>
        /// Devuelve el componente seleccionado
        /// </summary>
        public Componente SelectedComponent;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ComponentPicker"/>.
        /// </summary>
        /// <param name="existantComponent">Utilizá el dialogo para editar un componente existente</param>
        public ComponentPicker(Componente existantComponent = null)
        {
            InitializeComponent();
            this.componentPicker.ExistantComponent = existantComponent;
        }
        /// <summary>
        /// Maneja el evento que realizá dar clic en el botón de OK
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedComponent = this.componentPicker.GetComponent();
            if (this.SelectedComponent != null)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
                await this.ShowMessageAsync("Ningún componente seleccionado", "Debe seleccionar un componente válido para continuar");
        }
        /// <summary>
        /// Maneja el evento que realizá dar clic en el botón de Cancel
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
