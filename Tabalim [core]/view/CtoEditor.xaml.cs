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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using Tabalim.Core.runtime;

namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para CtoLengthEditor.xaml
    /// </summary>
    public partial class CtoEditor : UserControl
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CtoEditor"/>.
        /// </summary>
        public CtoEditor()
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
            var cmpEditor = new ComponentPicker(item.Component);
            cmpEditor.ShowDialog();
            if (cmpEditor.DialogResult.Value)
            {
                Componente updateC = cmpEditor.SelectedComponent;
                item.Component.UpdateComponentTr(TabalimApp.CurrentTablero,
                    (Object result) =>
                    {
                        if ((Boolean)result)
                            this.Refresh();
                    },
                    updateC.Circuito, updateC.Count, updateC.Potencia);
            }
        }
        /// <summary>
        /// Maneja el evento que envia la instrucción de eliminar un componente
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private async void btnDelComp_Click(object sender, RoutedEventArgs e)
        {
            string idString = ((sender as Button).Parent as Grid).Children.OfType<FrameworkElement>().Where(x => x is TextBlock).Select(y => (y as TextBlock).Text).FirstOrDefault();
            int id = int.TryParse(idString, out id) ? id : -1;
            CtoCompItem item = this.GetComponentById(id);
            if (await this.ShowQuestionDialog("Eliminar componente", 
                String.Format("Esta seguro de querer eliminar el componente {0}", item.ComKey)))
                item.Component.DeleteComponentTr(
                    (Object result) =>
                    {
                        if ((Boolean)result)
                            this.Refresh();
                    });
        }
        /// <summary>
        /// Maneja el evento que que envia la instrucción de editar un circuito
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void btnEditCircuito_Click(object sender, RoutedEventArgs e)
        {
            string cKey;
            Circuito cto = GetCircuito(sender as Button, out cKey);
            var inp = new CircuitInput(cto);
            inp.ShowDialog();
            if (inp.DialogResult.Value)
            {
                String ctoFormat = "Cto({0}) L {1} [m]";
                foreach (var it in this.listOfCircuits.ItemsSource.OfType<CtoCompItem>().Where(x => x.CtoKey == cKey))
                    it.CtoFormat = String.Format(ctoFormat, cto, cto.Longitud);
                ((CollectionView)CollectionViewSource.GetDefaultView(this.listOfCircuits.ItemsSource)).Refresh();
            }
        }
        /// <summary>
        /// Handles the Click event of the btnDeleteCircuito control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void btnDeleteCircuito_Click(object sender, RoutedEventArgs e)
        {
            string cKey;
            Circuito cto = GetCircuito(sender as Button, out cKey);
            if (await this.ShowQuestionDialog("Eliminar circuito", String.Format("Esta seguro de querere eliminar el cto {0}", cto)))
                cto.DeleteCircuitTr(
                    (Object result) =>
                    {
                        if ((Boolean)result)
                            this.Refresh();
                    });
        }
        /// <summary>
        /// Obtiene el circuito asociado al botón presionado
        /// </summary>
        /// <param name="sender">El botón que envía la acción.</param>
        /// <returns>El circuito seleccionado</returns>
        private Circuito GetCircuito(Button sender, out string cKey)
        {
            cKey = (((sender.Parent as StackPanel).Parent as Grid).Children[0] as TextBlock).Text;
            cKey = cKey.Split('(')[1].Split(')')[0];
            string key = cKey;
            CtoCompItem item = this.listOfCircuits.ItemsSource.OfType<CtoCompItem>().Where(x => x.CtoKey == key).FirstOrDefault();
            return item.Circuit;
        }
    }
}
