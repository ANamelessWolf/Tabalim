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
    /// Lógica de interacción para ComponentGalleryPicker.xaml
    /// </summary>
    public partial class ComponentGalleryPicker : MetroWindow
    {
        /// <summary>
        /// Devuelve el elmento seleccionado de la lista
        /// </summary>
        /// <value>
        /// El componente seleccionado.
        /// </value>
        public ComponentGalleryItem SelectedItem
        {
            get
            {
                return this.gallery.SelectedItem;
            }
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ComponentGalleryPicker"/>.
        /// </summary>
        public ComponentGalleryPicker()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void btnClick(object sender, RoutedEventArgs e)
        {
            if (btnOk.Name == (sender as Button).Name)
            {
                if (this.gallery.SelectedItem != null)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                    await this.ShowMessageAsync("Ningun componente seleccionado", "Debe seleccionar un componente para continuar");
            }
            else
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
