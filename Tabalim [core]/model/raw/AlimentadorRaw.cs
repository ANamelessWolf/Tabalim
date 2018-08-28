using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model.raw
{
    public class AlimentadorRaw
    {
        /// <summary>
        /// El número de línea
        /// </summary>
        public string No => Linea.No;
        /// <summary>
        /// El origen de la línea usado en el campo "DE"
        /// </summary>
        public string Origin => Linea.From;
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// </summary>
        public string To => Linea.To;
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// Descripción
        /// </summary>
        public string ToDesc => Linea.ToDesc;
        /// <summary>
        /// La potencia de instalación como VA
        /// </summary>
        public double PotInstVA => Linea.Destination.PotenciaInstalada;
        /// <summary>
        /// La potencia de instalación como Watts
        /// </summary>
        public double PotInstWatts => PotInstVA * FacPotencia;
        /// <summary>
        /// El factor de demanda
        /// </summary>
        public double FacDemanda => Linea.Destination.FactorDemanda;
        /// <summary>
        /// La potencia demandada del alumbrado
        /// </summary>
        public double PotDemAlum => Linea.Destination.PotenciaDemandadaAlumbrado;
        /// <summary>
        /// La potencia demandada de los contactos
        /// </summary>
        public double PotDemCont => Linea.Destination.PotenciaDemandadaContactos;
        /// <summary>
        /// La potencia demandada de los motores
        /// </summary>
        public double PotDemForce => Linea.Destination.PotenciaDemandadaFuerza;
        /// <summary>
        /// La potencia demandada en VA
        /// </summary>
        public double PotDemVA => Linea.Destination.PotenciaDemandada;
        /// <summary>
        /// La potencia demandada en Watts
        /// </summary>
        public double PotDemWatts => PotDemVA * FacPotencia;
        /// <summary>
        /// El factor de potencia
        /// </summary>
        public double FacPotencia => Linea.FactorPotencia;
        /// <summary>
        /// El voltaje nominal
        /// </summary>
        public double VoltajeNominal => Linea.Destination.Tension;
        /// <summary>
        /// La corriente nominal
        /// </summary>
        public double CorrienteNominal => Linea.Destination.CorrienteNominal;
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FacTem => Linea.FactorTemperartura;
        /// <summary>
        /// El factor de agrupación
        /// </summary>
        public double FacAgr => Linea.FactorAgrupamiento;
        /// <summary>
        /// La corriente corregida
        /// </summary>
        public double CorrienteCorregida => Linea.CorrienteCorregida;
        /// <summary>
        /// Alimentación
        /// </summary>
        public string Aliment => Linea.Conductor.Alimentador;
        /// <summary>
        /// Canalización
        /// </summary>
        public string Canal => Linea.Conductor.Canalizacion;
        /// <summary>
        /// Material.
        /// </summary>
        public string Material => Linea.Material;
        /// <summary>
        /// La longitud en metros de la línea
        /// </summary>
        public double Length => Linea.Longitud;
        /// <summary>
        /// El valor de la impedancia
        /// </summary>
        public double Imped => Linea.Impedancia;
        /// <summary>
        /// El valor de la resistencia
        /// </summary>
        public double Resist => Linea.Resistencia;
        /// <summary>
        /// El valor de la reactancia
        /// </summary>
        public double React => Linea.Reactancia;
        /// <summary>
        /// La caida del voltaje
        /// </summary>
        public double CaidaDeVoltaje => Linea.CaidaVoltaje;
        /// <summary>
        /// EL interruptor asociado a la línea
        /// </summary>
        public string Interruptor => Linea.Interruptor;
        Linea Linea;
        public AlimentadorRaw(Linea linea)
        {
            Linea = linea;
        }
    }
}
