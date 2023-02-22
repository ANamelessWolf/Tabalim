using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Controller;

namespace Tabalim.Addin.Model
{
    /// <summary>
    /// Define una columna de circuito
    /// </summary>
    public class CircuitoRow
    {
        /// <summary>
        /// El nombre del circuito
        /// </summary>
        public String Cto;
        /// <summary>
        /// La cantidad del circuito,
        /// cantidades separadas por comas
        /// </summary>
        public string Count;
        /// <summary>
        /// La potencia del circuito
        /// </summary>
        public Double Potencia;
        /// <summary>
        /// La Tensión del circuito
        /// </summary>
        public Double Tension;
        /// <summary>
        /// Las Fases del circuito
        /// </summary>
        public int Fases;
        /// <summary>
        /// Las corriente del circuito
        /// </summary>
        public double Corriente;
        /// <summary>
        /// La corriente continua
        /// </summary>
        public double Continua;
        /// <summary>
        /// La longitud del circuito
        /// </summary>
        public double Longitud;
        /// <summary>
        /// El factor de agrupación
        /// </summary>
        public double FactorAgrupacion;
        /// <summary>
        /// La temperatura del circuito
        /// </summary>
        public double Temperatura;
        /// <summary>
        /// El calibre del circuito
        /// </summary>
        public double Calibre;
        /// <summary>
        /// La sección del circuito
        /// </summary>
        public double Section;
        /// <summary>
        /// La caida del voltaje
        /// </summary>
        public double CaidaVoltaje;
        /// <summary>
        /// La potencia en la fase A
        /// </summary>
        public double PotenciaA;
        /// <summary>
        /// La potencia en la fase B
        /// </summary>
        public double PotenciaB;
        /// <summary>
        /// La potencia en la fase C
        /// </summary>
        public double PotenciaC;
        /// <summary>
        /// El interruptor del circuito
        /// </summary>
        public String Interruptor;
        /// <summary>
        /// La corriente de protección 
        /// </summary>
        public double Protecion;
        public double Tierra;
        /// <summary>
        /// Regresa los valores con el formato deseado
        /// </summary>
        /// <returns>Los valores del circuito con el formato deseado</returns>
        internal string[] GetValues()
        {
            return new string[]{
                this.Potencia.ToNumberFormat(),
                this.Tension.ToNumberFormat(),
                this.Fases.ToNumberFormat(),
                this.Corriente.ToNumberFormat(),
                this.Continua.ToNumberFormat(),
                this.Longitud.ToNumberFormat(),
                this.FactorAgrupacion.ToNumberFormat(),
                this.Temperatura.ToNumberFormat(),
                this.Calibre.ToNumberFormat(),
                this.Section.ToNumberFormat(),
                this.Tierra.ToNumberFormat(),
                this.CaidaVoltaje.ToString("P2"),
                this.PotenciaA.ToNumberFormat(),
                this.PotenciaB.ToNumberFormat(),
                this.PotenciaC.ToNumberFormat(),
                this.Protecion.ToNumberFormat(),
                this.Interruptor
        };
        }
    }
}
