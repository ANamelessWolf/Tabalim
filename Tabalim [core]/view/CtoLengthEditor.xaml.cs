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
            PropertyGroupDescription groupBy = new PropertyGroupDescription("CtoFormat");
            view.GroupDescriptions.Add(groupBy);
        }
        /// <summary>
        /// Obtiene un componente con circuito especificando el id
        /// del circuito
        /// </summary>
        /// <param name="id">El identificador del componente.</param>
        /// <returns>El componente y ciscuito seleccionado.</returns>
        public CtoCompItem GetComponentById(int id)
        {
            return this.listOfCircuits.ItemsSource.OfType<CtoCompItem>().Where(x => x.CompId == id).FirstOrDefault();
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
            string idString = ((sender as Button).Parent as Grid).Children.OfType<FrameworkElement>().Where(x => x is TextBlock).Select(y => (y as TextBlock).Text).FirstOrDefault();
            int id = int.TryParse(idString, out id) ? id : -1;
            CtoCompItem item = this.GetComponentById(id);
        }
        /// <summary>
        /// Maneja el evento que envia la instrucción de eliminar un componente
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnDelComp_Click(object sender, RoutedEventArgs e)
        {
            string idString = ((sender as Button).Parent as Grid).Children.OfType<FrameworkElement>().Where(x => x is TextBlock).Select(y => (y as TextBlock).Text).FirstOrDefault();
            int id = int.TryParse(idString, out id) ? id : -1;
            CtoCompItem item = this.GetComponentById(id);
        }
        /// <summary>
        /// Maneja el evento que que envia la instrucción de editar un circuito
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnEditCircuito_Click(object sender, RoutedEventArgs e)
        {
            var cKey = (((sender as Button).Parent as Grid).Children[0] as TextBlock).Text;
            cKey = cKey.Split('(')[1].Split(')')[0];
            CtoCompItem item = this.listOfCircuits.ItemsSource.OfType<CtoCompItem>().Where(x => x.CtoKey == cKey).FirstOrDefault();
            Circuito cto = item.Circuit;
            String ctoFormat = "Cto({0}) L {1} [m]";
            item.CtoFormat = String.Format(ctoFormat, cto, cto.Longitud);
            ((CollectionView)CollectionViewSource.GetDefaultView(this.listOfCircuits.ItemsSource)).Refresh();
        }
    }
}
