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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tabalim.Core.controller;
using Tabalim.Core.model;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para ComponentGallery.xaml
    /// </summary>
    public partial class ComponentGallery : UserControl
    {
        /// <summary>
        /// El filtro de componente
        /// </summary>
        private ComponentType[] Filter;
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
                if (this.listOfComponents.SelectedIndex != -1)
                    return (ComponentGalleryItem)this.listOfComponents.SelectedItem;
                else
                    return null;
            }
        }
        /// <summary>
        /// Inicializa una nueva instancia del selector de componentes.
        /// </summary>
        public ComponentGallery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Filter = UpdateFilter();
            var items = ComponentsUtils.GetGallery();
            this.listOfComponents.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.listOfComponents.ItemsSource);
            view.Filter = ComponentFilter;
            if (this.listOfComponents.Items.Count > 0)
                this.listOfComponents.SelectedIndex = 0;
        }
        /// <summary>
        /// Actualiza el filtro de componentes
        /// </summary>
        /// <returns>El tipo de componente</returns>
        private ComponentType[] UpdateFilter()
        {
            List<ComponentType> availableTypes = new List<ComponentType>();
            if (this.comp_contacts.IsChecked.Value)
                availableTypes.Add(ComponentType.Contacto);
             if (this.comp_lights.IsChecked.Value)
                availableTypes.Add(ComponentType.Alumbrado);
             if (this.comp_motors.IsChecked.Value)
                availableTypes.Add(ComponentType.Motor);
            return availableTypes.ToArray();
        }
        /// <summary>
        /// Verifica que el componente sea del tipo especificado
        /// </summary>
        /// <param name="item">El elemento a validar.</param>
        /// <returns>Verdadero si el elemento esta dentro del filtro</returns>
        private bool ComponentFilter(object item)
        {
            return this.Filter.Contains(((item as ComponentGalleryItem).CType));
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void comp_Checked(object sender, RoutedEventArgs e)
        {
            if (this.comp_contacts != null && this.comp_lights != null && this.comp_motors != null && this.listOfComponents!=null)
            {
                this.Filter = UpdateFilter();
                CollectionViewSource.GetDefaultView(listOfComponents.ItemsSource).Refresh();
            }
        }
    }
}
