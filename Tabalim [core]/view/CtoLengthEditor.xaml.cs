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

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para CtoLengthEditor.xaml
    /// </summary>
    public partial class CtoLengthEditor : UserControl
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CtoLengthEditor"/>.
        /// </summary>
        public CtoLengthEditor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Actualiza la vista de los circuitos cargados en la aplicación
        /// </summary>
        public void Refresh()
        {
            this.listOfCircuits.ItemsSource = ComponentsUtils.GetComponentsWithCircuits();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.listOfCircuits.ItemsSource);
            PropertyGroupDescription groupBy = new PropertyGroupDescription("CtoKey");
            view.GroupDescriptions.Add(groupBy);
        }
        /// <summary>
        /// Maneja el evento que refresca la vista actual de circuitos.
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }
        /// <summary>
        /// Maneja el evento que envia la instrucción de editar un componente
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnEditComp_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Maneja el evento que envia la instrucción de eliminar un componente
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnDelComp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
