using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Addin.Model
{
    /// <summary>
    /// Define una fila de alimentador
    /// </summary>
    public class AlimentadorRow
    {
        /// <summary>
        /// El número de línea
        /// </summary>
        public string No;
        /// <summary>
        /// El origen de la línea usado en el campo "DE"
        /// </summary>
        public string Origin;
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// </summary>
        public string To;
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// Descripción
        /// </summary>
        public string ToDesc;
        /// <summary>
        /// La potencia de instalación como VA
        /// </summary>
        public double PotInstVA;
        /// <summary>
        /// La potencia de instalación como Watts
        /// </summary>
        public double PotInstWatts;
        /// <summary>
        /// El factor de demanda
        /// </summary>
        public double FacDemanda;
        /// <summary>
        /// La potencia demandada del alumbrado
        /// </summary>
        public double PotDemAlum;
        /// <summary>
        /// La potencia demandada de los contactos
        /// </summary>
        public double PotDemCont;
        /// <summary>
        /// La potencia demandada de los motores
        /// </summary>
        public double PotDemForce;
        /// <summary>
        /// La potencia demandada en VA
        /// </summary>
        public double PotDemVA;
        /// <summary>
        /// La potencia demandada en Watts
        /// </summary>
        public double PotDemWatts;
        /// <summary>
        /// El factor de potencia
        /// </summary>
        public double FacPotencia;
        /// <summary>
        /// El voltaje nominal
        /// </summary>
        public double VoltajeNominal;
        /// <summary>
        /// La corriente nominal
        /// </summary>
        public double CorrienteNominal;
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FacTem;
        /// <summary>
        /// El factor de agrupación
        /// </summary>
        public double FacAgr;
        /// <summary>
        /// La corriente corregida
        /// </summary>
        public double CorrienteCorregida;
        /// <summary>
        /// Alimentación
        /// </summary>
        public string Aliment;
        /// <summary>
        /// Canalización
        /// </summary>
        public string Canal;
        /// <summary>
        /// La longitud en metros de la línea
        /// </summary>
        public double Length;
        /// <summary>
        /// El valor de la impedancia
        /// </summary>
        public double Imped;
        /// <summary>
        /// El valor de la resistencia
        /// </summary>
        public double Resist;
        /// <summary>
        /// El valor de la reactancia
        /// </summary>
        public double React;
        /// <summary>
        /// La caida del voltaje
        /// </summary>
        public double CaidaDeVoltaje;
        /// <summary>
        /// EL interruptor asociado a la línea
        /// </summary>
        public string Interruptor;
    }
}
