using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Define un circuito
    /// </summary>
    public abstract class Circuito
    {
        /// <summary>
        /// El id del componente es único en la aplicación.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Establece el nombre de la base de datos
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName { get { return TABLE_CIRCUIT; } }
        /// <summary>
        /// Los polos alos que se conecta
        /// </summary>
        public int[] Polos;
        /// <summary>
        /// Los componentes que se conectan al circuito
        /// </summary>
        public Dictionary<int, Componente> Componentes;
        /// <summary>
        /// Tension en base al sistema al que pertenece
        /// </summary>
        public Tension Tension;
        /// <summary>
        /// La potencia total 
        /// </summary>
        public double PotenciaTotal { get { return Componentes != null ? Componentes.Values.Sum(x => x.Potencia.Watts * x.Count) : 0; } }
        /// <summary>
        /// Obtiene la corriente.
        /// </summary>
        /// <value>
        /// La corriente dependiendo del numero de polos.
        /// </value>
        public abstract double Corriente { get; }
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FactorTemperatura;
        /// <summary>
        /// El factor de agrupacion
        /// </summary>
        public double FactorAgrupacion;
        /// <summary>
        /// Obtiene la  corriente corregida.
        /// </summary>
        /// <value>
        /// Corriente corregida.
        /// </value>
        public double CorrienteCorregida { get { return Corriente / (FactorTemperatura * FactorAgrupacion); } }
        /// <summary>
        /// Longitud del circuito
        /// </summary>
        public double Longitud;
        /// <summary>
        /// Obtiene la corriente de proteccion.
        /// </summary>
        /// <value>
        /// Corriente de proteccion.
        /// </value>
        public double CorrienteProteccion { get { return Corriente * Componentes.First().Value.FactorProteccion; } }
        /// <summary>
        /// Obtiene el calibre utilizando la corriente de protección.
        /// </summary>
        /// <value>
        /// Calibre.
        /// </value>
        public Calibre Calibre { get { return Calibre.GetCalibre(CorrienteProteccion); } }
        /// <summary>
        /// Obtiene la caida de voltaje.
        /// </summary>
        /// <value>
        /// Caida de voltaje.
        /// </value>
        public abstract double CaidaVoltaje { get; }
        /// <summary>
        /// Obtiene potencia en fases.
        /// </summary>
        /// <value>
        /// Potencia en fases.
        /// </value>
        public double[] PotenciaEnFases
        {
            get
            {
                double[] arr = new double[3];
                if (Polos.Length > 0 && PotenciaTotal > 0)
                    foreach (int polo in Polos)
                        arr[(int)((polo - 1) / 2 % 3)] = PotenciaTotal / Polos.Length;
                return arr;
            }
        }
        /// <summary>
        /// Interruptor
        /// </summary>
        public String Interruptor;
        /// <summary>
        /// Gets a value indicating whether this instance has motor.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has motor; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasMotor { get { return Componentes.Values.Count(x => x is Motor) > 0; } }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Join(",", Polos);
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ComponentPicker"/>.
        /// </summary>
        public Circuito()
        {
            this.Componentes = new Dictionary<int, Componente>();
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="input">La entrada es un tablero</param>
        public void Create(Object input)
        {
            Tablero tablero = input as Tablero;
            this.Tension = tablero.Sistema.Tension;
            this.FactorTemperatura = Temperatura.GetFactor((int)tablero.Sistema.Temperatura);
            tablero.Circuitos.Add(this.ToString(), this);
        }
        /// <summary>
        /// Actualiza un registro del objeto en la base de datos
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(object input)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Crea instancia de circuito  a partir del numero de fases
        /// </summary>
        /// <param name="fases">The fases.</param>
        /// <param name="polos">The polos.</param>
        /// <returns></returns>
        public static Circuito GetCircuito(int fases, int[] polos)
        {
            if (runtime.TabalimApp.CurrentTablero.Circuitos.ContainsKey(String.Join(",", polos)))
                return runtime.TabalimApp.CurrentTablero.Circuitos[String.Join(",", polos)];
            if (fases == 1) return new CircuitoMonofasico() { Polos = polos };
            else if (fases == 2) return new CircuitoBifasico() { Polos = polos };
            else return new CircuitoTrifasico() { Polos = polos };
        }
    }
}
