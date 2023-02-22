using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
using PathIO = System.IO.Path;
namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para NewComponent.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class NewComponent : UserControl
    {
        /// <summary>
        /// Especifica el sistema al que pertenece el componente.
        /// </summary>
        public SistemaFases Sistema;
        /// <summary>
        /// El tipo de componentes
        /// </summary>
        public ComponentType CType;
        /// <summary>
        /// Establece el no de fases que componen al circuito
        /// </summary>
        public int Fases
        {
            get
            {
                if (this.optOne.IsChecked.Value)
                    return 1;
                else if (this.optTwo.IsChecked.Value)
                    return 2;
                else if (this.optThree.IsChecked.Value)
                    return 3;
                else
                    return 0;
            }
            set
            {
                if (value == 3)
                    this.optThree.IsChecked = true;
                else if (value == 2)
                    this.optTwo.IsChecked = true;
                else
                    this.optOne.IsChecked = true;
            }
        }
        bool isAlumbradoDelta { get { return this.isDelta.IsChecked.Value; } set { this.isDelta.IsChecked = value; } }
        /// <summary>
        /// Regresa verdadero cuando el componente seleccionado es válido,
        /// en otro caso se arroja una excepción que indica un componente incompleto
        /// </summary>
        /// <returns>
        ///   <c>true</c> Verdadero cuando la instancia es válida; en otro caso, <c>falso</c>.
        /// </returns>
        /// <exception cref="Exception">En caso de que el componente no seleccionado sea invalido</exception>
        internal bool IsValid()
        {
            if (this.powerSelector.SelectedPower.Watts <= 0)
                throw new Exception("Faltan definir la potencia");
            else if (this.listOfCircuits.SelectedIndex == -1)
                throw new Exception("Ningun circuito seleccionado");
            else
                return true;
        }
        /// <summary>
        /// El indice del componente insertado
        /// </summary>
        public int ImageIndex;
        /// <summary>
        /// En caso de que se abra el controlador con un componente
        /// activo.
        /// </summary>
        public Componente ExistantComponent;
        /// <summary>
        /// Devuelve el circuito seleccionado
        /// </summary>
        /// <value>
        /// El circuito seleccionado
        /// </value>
        public Circuito SelectedCircuito
        {
            get { return this.listOfCircuits.SelectedIndex != -1 ? (this.listOfCircuits.SelectedItem as CtoItem).Circuito : null; }
        }
        /// <summary>
        /// La cantidad de componentes a insertar
        /// </summary>
        /// <value>
        /// El número de componentes a insertar.
        /// </value>
        public int Count
        {
            get
            {
                int count;
                return this.tboComCount == null ? 0 : int.TryParse(this.tboComCount.Text, out count) ? count : 0;
            }
            set
            {
                this.tboComCount.Text = value.ToString();
            }
        }
        bool IsCtrlLoaded;
        /// <summary>
        /// Crea un componente con las opciones del controlador actual
        /// </summary>
        /// <returns>El componente seleccionado</returns>
        public Componente GetComponent()
        {
            Componente com = null;
            switch (this.CType)
            {
                case ComponentType.Alumbrado:
                    com = new Alumbrado(this.powerSelector.SelectedPower.Watts) { DeltaKey = isAlumbradoDelta ? 0 : -1 };
                    break;
                case ComponentType.Contacto:
                    com = new Contacto(this.powerSelector.SelectedPower.Watts);
                    break;
                case ComponentType.Motor:
                    com = new Motor(this.powerSelector.SelectedPower.HP);
                    break;
                case ComponentType.MotorWatts:
                    com = new MotorWatts(this.powerSelector.SelectedPower.Watts);
                    break;
                case ComponentType.Aire:
                    com = new Aire(this.powerSelector.SelectedPower.Watts);
                    break;
                case ComponentType.Subtablero:
                    com = new Subtablero(this.powerSelector.SelectedPower.Watts);
                    break;
            }
            if (com != null && this.CType != ComponentType.None)
            {
                com.Circuito = this.SelectedCircuito;
                com.Count = this.Count;
                com.ImageIndex = this.ImageIndex;
                if(this.ExistantComponent == null)
                if (com.Circuito.Componentes.Values.Count(x => x.Key == com.Key) > 0)
                    throw new Exception("El circuito contiene un componenete con las mismas caracteristicas.");
            }
            return com;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewComponent"/> class.
        /// </summary>
        public NewComponent(Componente existantComponent)
        {
            this.ExistantComponent = existantComponent;
            this.IsCtrlLoaded = false;
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewComponent"/> class.
        /// </summary>
        public NewComponent()
        {
            this.IsCtrlLoaded = false;
            InitializeComponent();
        }
        /// <summary>
        /// Maneja el evento que realizá la carga inicial del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string imgGalleryPath = PathIO.Combine(PathIO.GetDirectoryName(Assembly.GetAssembly(typeof(NewComponent)).Location), IMG_FOLDER, COMPONENT_FOLDER);
            this.Sistema = TabalimApp.CurrentTablero.Sistema;
            this.FillCircuitosList(this.ExistantComponent);
            if (this.ExistantComponent != null)
            {
                this.CType = this.ExistantComponent is Motor ? ComponentType.Motor :
                             this.ExistantComponent is Alumbrado ? ComponentType.Alumbrado :
                             this.ExistantComponent is Contacto ? ComponentType.Contacto :
                             this.ExistantComponent is MotorWatts ? ComponentType.MotorWatts :
                             this.ExistantComponent is Aire ? ComponentType.Aire :
                             this.ExistantComponent is Subtablero ? ComponentType.Subtablero : ComponentType.None;
                this.ImageIndex = this.ExistantComponent.ImageIndex;
                this.Count = this.ExistantComponent.Count;
                this.miniature.Source = this.ImageIndex.LoadImage(imgGalleryPath, 32, true);
                if (this.ExistantComponent.Circuito != null)
                {
                    this.Fases = this.ExistantComponent.Circuito.Polos.Length;
                    this.powerSelector.ExistantInput = new Object[]
                    {
                        this.Fases,
                        this.CType == ComponentType.Motor?PowerType.HP: PowerType.Watts,
                        this.ExistantComponent.Potencia
                    };
                    int index = 0, selectedIndex = -1;
                    foreach (CtoItem item in this.listOfCircuits.ItemsSource)
                    {
                        if (item.CtoName == this.ExistantComponent.Circuito.ToString())
                        {
                            selectedIndex = index;
                            break;
                        }
                        index++;
                    }
                    this.listOfCircuits.SelectedIndex = selectedIndex;
                    this.btnPickComponent.IsEnabled = false;
                    this.optOne.IsEnabled = false;
                    this.optTwo.IsEnabled = false;
                    this.optThree.IsEnabled = false;
                    var circuits = UiUtils.GetAvailableCircuitos(TabalimApp.CurrentTablero, this.Fases, this.CType == ComponentType.Motor);
                    if (this.ExistantComponent is Motor)
                        circuits = circuits.Union(new Circuito[] { this.ExistantComponent.Circuito }).OrderBy(x => x.ToString());
                    this.listOfCircuits.ItemsSource = circuits.Select(x => new CtoItem(x));
                    int ctoIndex = this.listOfCircuits.ItemsSource.OfType<CtoItem>().Select(x => x.CtoName).ToList().IndexOf(this.ExistantComponent.Circuito.ToString());
                    this.listOfCircuits.SelectedIndex = ctoIndex;
                }
                else
                {
                    this.Fases = 1;
                    this.listOfCircuits.SelectedIndex = -1;
                    this.UpdatePowerSelector();
                }
            }
            else
            {
                this.Count = 1;
                this.Fases = 1;
                this.CType = ComponentType.None;
            }
            this.IsCtrlLoaded = true;
        }
        /// <summary>
        /// Fills the circuitos list.
        /// </summary>
        /// <param name="existantComponent">The existant component.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void FillCircuitosList(Componente existantComponent)
        {
            var ctos = UiUtils.GetAvailableCircuitos(TabalimApp.CurrentTablero, this.Fases, this.CType == ComponentType.Motor);
            var item = new CtoItem(ctos.ElementAt(0));
            this.listOfCircuits.ItemsSource = ctos.Select(x => new CtoItem(x));
        }

        private void btnPickComponent_Click(object sender, RoutedEventArgs e)
        {
            string imgGalleryPath = PathIO.Combine(PathIO.GetDirectoryName(Assembly.GetAssembly(typeof(NewComponent)).Location), IMG_FOLDER, COMPONENT_FOLDER);
            var picker = new ComponentGalleryPicker();
            picker.ShowDialog();
            if (picker.DialogResult.Value)
            {
                this.CType = picker.SelectedItem.CType;
                this.ImageIndex = picker.SelectedItem.Index;
                this.miniature.Source = picker.SelectedItem.Index.LoadImage(imgGalleryPath, 32, true);
                this.AllowDelta();
                this.UpdatePowerSelector();
            }
        }

        private void AllowDelta()
        {
            if (TabalimApp.CurrentTablero.Sistema.Fases == 3 && this.CType == ComponentType.Alumbrado)
                deltaOptions.Visibility = Visibility.Visible;
            else
                deltaOptions.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Actualiza la vista del selector
        /// </summary>
        private void UpdatePowerSelector()
        {
            if (this.CType != ComponentType.None && this.Fases > 0)
            {
                this.powerSelector.Fases = this.Fases;
                this.powerSelector.Tension = this.Sistema.Tension;
                if (this.CType == ComponentType.Motor)
                    this.powerSelector.Power = PowerType.HP;
                else
                    this.powerSelector.Power = PowerType.Watts;
            }
        }
        /// <summary>
        /// Maneja el evento que realizá la actualización de la vista de potencia
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void FasesChanged(object sender, RoutedEventArgs e)
        {
            if (!this.IsCtrlLoaded)
                return;
            this.UpdatePowerSelector();
            this.listOfCircuits.ItemsSource = UiUtils.GetAvailableCircuitos(TabalimApp.CurrentTablero, this.Fases, this.CType == ComponentType.Motor).Select(x => new CtoItem(x));
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnInput_Changed(object sender, TextCompositionEventArgs e)
        {
            int num;
            if (!int.TryParse(e.Text, out num))
                e.Handled = true;
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                int num;
                TextBox txt = sender as TextBox;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!int.TryParse(txt.Text, out num))
                    e.CancelCommand();
            }
        }

        private void isDelta_Checked(object sender, RoutedEventArgs e)
        {
            optOne.IsEnabled = optTwo.IsEnabled = !isAlumbradoDelta;
            optThree.IsChecked = isAlumbradoDelta;            
        }
    }
}
