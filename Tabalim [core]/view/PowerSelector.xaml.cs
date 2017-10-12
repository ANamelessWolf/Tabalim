using MahApps.Metro.Controls;
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
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.view
{
    /// <summary>
    /// Lógica de interacción para PowerSelector.xaml
    /// </summary>
    public partial class PowerSelector : UserControl
    {
        /// <summary>
        /// Define el número de fases
        /// </summary>
        public int Fases
        {
            get { return (int)GetValue(FasesProperty); }
            set { SetValue(FasesProperty, value); }
        }
        /// <summary>
        /// Define el número de fases
        /// </summary>
        public PowerType Power
        {
            get { return (PowerType)GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }
        /// <summary>
        /// Devuelve la potencia seleccionada
        /// </summary>
        /// <value>
        /// La potencia seleccionada.
        /// </value>
        public Potencia SelectedPower
        {
            get
            {
                if (Power == PowerType.HP)
                    return new Potencia(((HPItem)this.cboHP.SelectedItem).HP, true);
                else
                {
                    double val, watts;
                    watts = double.TryParse(this.tboWatts.Text, out val) ? val : 0d;
                    return new Potencia(watts);
                }
            }
            set
            {
                if (Power == PowerType.HP)
                {
                    int index = this.cboHP.ItemsSource.OfType<HPItem>().Select(x => new Potencia(x.HP, true).HP).ToList().IndexOf(value.HP);
                    this.cboHP.SelectedIndex = index;
                }
                else
                    this.tboWatts.Text = value.Watts.ToString();
            }
        }
        /// <summary>
        /// La propiedad de fases
        /// </summary>
        public static DependencyProperty FasesProperty;
        /// <summary>
        /// La propiedad de tipo de potencia
        /// </summary>
        public static DependencyProperty PowerProperty;
        /// <summary>
        /// Creates a new TextBox tag control property
        /// </summary>
        static PowerSelector()
        {
            FasesProperty = DependencyProperty.Register("Fases", typeof(int), typeof(PowerSelector),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, OnFases_Changed));
            PowerProperty = DependencyProperty.Register("Power", typeof(PowerType), typeof(PowerSelector),
                new FrameworkPropertyMetadata(PowerType.None, FrameworkPropertyMetadataOptions.AffectsRender, OnPower_Changed));
        }
        ///<summary>
        /// The event when the Text blank message change its value
        /// </summary>
        static void OnFases_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PowerSelector ctrl = sender as PowerSelector;
            int fases = (int)e.NewValue;
            CollectionViewSource.GetDefaultView(ctrl.cboHP.ItemsSource).Refresh();
            if (ctrl.cboHP.Items.Count > 0)
                ctrl.cboHP.SelectedIndex = 0;
        }
        ///<summary>
        /// The event when the Text blank message change its value
        /// </summary>
        static void OnPower_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PowerSelector ctrl = sender as PowerSelector;
            PowerType ptp = (PowerType)e.NewValue;
            if (ptp == PowerType.HP)
            {
                ctrl.Watts.Visibility = Visibility.Collapsed;
                ctrl.Hp.Visibility = Visibility.Visible;
            }
            else
            {
                ctrl.Watts.Visibility = Visibility.Visible;
                ctrl.Hp.Visibility = Visibility.Collapsed;
                if (ptp == PowerType.Watts)
                    ctrl.wattsInput.IsChecked = true;
                else
                    ctrl.wattsInput.IsChecked = false;
            }
        }
        /// <summary>
        /// The existant input
        /// </summary>
        public Object ExistantInput;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PowerSelector"/>.
        /// </summary>
        public PowerSelector(int fases, PowerType pt, Potencia p)
        {
            this.ExistantInput = new Object[] { fases, pt, p };
            InitializeComponent();
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PowerSelector"/>.
        /// </summary>
        public PowerSelector()
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
            wattsInput.Checked += WattsInput_Checked;
            wattsInput.Unchecked += WattsInput_Checked;
            TextBox[] tbos = new TextBox[] { this.tboWatts, this.tboVA, this.tbokW, this.tboHpVA };
            foreach (TextBox tb in tbos)
                tb.PreviewTextInput += OnInput_Changed;
            this.tboWatts.TextChanged += Txt_Changed;
            this.tboVA.TextChanged += Txt_Changed;
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, Object input) =>
                {
                    string query = TABLE_HP_WATTS.SelectAll();
                    List<HPItem> items = conn.Select<HPItem>(query);
                    return items;
                },
                TaskCompleted = (Object result) =>
                {
                    this.cboHP.ItemsSource = result as List<HPItem>;
                    CollectionView view = (CollectionView)
                    CollectionViewSource.GetDefaultView(this.cboHP.ItemsSource);
                    view.Filter = HPFilter;
                    if (this.cboHP.Items.Count > 0)
                        this.cboHP.SelectedIndex = 0;
                    this.cboHP.SelectionChanged += cboHP_SelectionChanged;
                    if (ExistantInput != null)
                    {
                        Object[] input =   this.ExistantInput as Object[];
                        this.Fases = (int)input[0];
                        this.Power = (PowerType)input[1];
                        this.SelectedPower = (Potencia)input[2];
                    }
                }
            };
            tr.Run(null);

        }
        /// <summary>
        /// Verifica que el componente sea del tipo especificado
        /// </summary>
        /// <param name="item">El elemento a validar.</param>
        /// <returns>Verdadero si el elemento esta dentro del filtro</returns>
        private bool HPFilter(object item)
        {
            HPItem hpItem = (item as HPItem);
            if (this.Fases == 1)
                return hpItem.HP <= 10;
            else if (this.Fases == 2)
                return hpItem.HP <= 200;
            else if (this.Fases == 3)
                return hpItem.HP <= 500;
            else
                return false;
        }
        /// <summary>
        /// Maneja el evento que realizá el cambio de opción en el switch del control
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="RoutedEventArgs"/> que contienen la información del evento.</param>
        private void WattsInput_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleSwitch).IsChecked.Value)
            {
                this.tboWatts.IsEnabled = true;
                this.tboVA.IsEnabled = false;
            }
            else
            {
                this.tboWatts.IsEnabled = false;
                this.tboVA.IsEnabled = true;
            }
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnInput_Changed(object sender, TextCompositionEventArgs e)
        {
            Double num;
            if (!Double.TryParse(e.Text, out num))
                e.Handled = true;
        }
        /// <summary>
        /// Validates the input pasted string
        /// </summary>
        void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                Double num;
                TextBox txt = sender as TextBox;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Double.TryParse(txt.Text, out num))
                    e.CancelCommand();
            }
        }
        /// <summary>
        /// When the text changes, the control validates the blank message visibility
        /// </summary>
        void Txt_Changed(object sender, TextChangedEventArgs e)
        {
            Double num;
            if ((sender as TextBox).Name == this.tboWatts.Name &&
                this.wattsInput.IsChecked.Value &&
                Double.TryParse((sender as TextBox).Text, out num))
            {
                Potencia p = new Potencia(num);
                this.tboVA.Text = p.PotenciaAparenteAsString;
            }
            else if ((sender as TextBox).Name == this.tboVA.Name &&
                !this.wattsInput.IsChecked.Value &&
                Double.TryParse((sender as TextBox).Text, out num))
            {
                Potencia p = new Potencia(num * 0.9);
                this.tboWatts.Text = p.WattsAsString;
            }
        }
        /// <summary>
        /// Maneja el evento que realizá el cambio de selección de potencia
        /// </summary>
        /// <param name="sender">La fuente del evento.</param>
        /// <param name="e">Los argumentos de tipo <see cref="SelectionChangedEventArgs"/> que contienen la información del evento.</param>
        private void cboHP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboHP.SelectedIndex != -1)
            {
                HPItem item = this.cboHP.SelectedItem as HPItem;
                Potencia p = new Potencia(item.HP, true);
                this.tbokW.Text = item.KW.ToString();
                this.tboHpVA.Text = p.PotenciaAparenteAsString;
            }
        }
    }
}
