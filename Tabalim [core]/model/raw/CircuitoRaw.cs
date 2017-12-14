using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model.raw
{
    public class CircuitoRaw
    {
        /// <summary>
        /// El nombre del circuito
        /// </summary>
        public String Cto => Circuito.ToString();
        /// <summary>
        /// La cantidad del circuito,
        /// cantidades separadas por comas
        /// </summary>
        public string Count => String.Join(",", Tablero.Componentes.Values.GroupBy(x => x.Key).Select(x => x.First()).Select(x => Circuito.Componentes.Values.Count(y => y.Key == x.Key) > 0 ? Circuito.Componentes.Values.First(y => y.Key == x.Key).Count.ToString() : ""));
        /// <summary>
        /// La potencia del circuito
        /// </summary>
        public Double Potencia => Circuito.PotenciaTotal;
        /// <summary>
        /// La Tensión del circuito
        /// </summary>
        public Double Tension => Circuito.Polos.Length == 1 ? Circuito.Tension.TensionAlNeutro : Circuito.Tension.Value;
        /// <summary>
        /// Las Fases del circuito
        /// </summary>
        public int Fases => Circuito.Polos.Length;
        /// <summary>
        /// Las corriente del circuito
        /// </summary>
        public double Corriente => Circuito.Corriente;
        /// <summary>
        /// La longitud del circuito
        /// </summary>
        public double Longitud => Circuito.Longitud;
        /// <summary>
        /// El factor de agrupación
        /// </summary>
        public double FactorAgrupacion => Circuito.FactorAgrupacion;
        /// <summary>
        /// La temperatura del circuito
        /// </summary>
        public double Temperatura => Circuito.FactorTemperatura;
        /// <summary>
        /// El calibre del circuito
        /// </summary>
        public double Calibre => double.Parse(Circuito.Calibre.AWG);
        /// <summary>
        /// La sección del circuito
        /// </summary>
        public double Section => Circuito.Calibre.AreaTransversal;
        /// <summary>
        /// La caida del voltaje
        /// </summary>
        public double CaidaVoltaje => Circuito.CaidaVoltaje / 100d;
        /// <summary>
        /// La potencia en la fase A
        /// </summary>
        public double PotenciaA => Circuito.PotenciaEnFases[0];
        /// <summary>
        /// La potencia en la fase B
        /// </summary>
        public double PotenciaB => Circuito.PotenciaEnFases[1];
        /// <summary>
        /// La potencia en la fase C
        /// </summary>
        public double PotenciaC => Circuito.PotenciaEnFases[2];
        /// <summary>
        /// El interruptor del circuito
        /// </summary>
        public String Interruptor => Circuito.Interruptor;
        /// <summary>
        /// La corriente de protección 
        /// </summary>
        public double Protecion => Circuito.CorrienteProteccion;
        [JsonIgnore]
        Circuito Circuito;
        [JsonIgnore]
        Tablero Tablero;
        public CircuitoRaw(Tablero t, Circuito c)
        {
            this.Circuito = c;
            this.Tablero = t;
        }
    }
}
