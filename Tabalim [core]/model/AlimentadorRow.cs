using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class AlimentadorRow
    {
        public int Id { get; set; }
        /// <summary>
        /// El número de línea
        /// </summary>
        public string No { get { return Linea.No; } }
        public String From { get { return Linea.From; } }
        public DestinationType ToType => Linea.Type;
        public String To { get { return ToType.ToString(); } }
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// Descripción
        /// </summary>
        public string ToDesc { get; set; }
        public string PotInstVA => Linea.Destination.PotenciaInstalada.ToString("#.##");
        public string PotInstWatts => (Linea.Destination.PotenciaInstalada * 0.9).ToString("#.##");
        public Double FacDemanda => Linea.Destination.FactorDemanda;


        public string PotDemAlum => Linea.Destination.PotenciaDemandadaAlumbrado.ToString("#.##");
        public string PotDemCont => Linea.Destination.PotenciaDemandadaContactos.ToString("#.##");
        public string PotDemForce => Linea.Destination.PotenciaDemandadaFuerza.ToString("#.##");
        public string PotDemVA => Linea.Destination.PotenciaDemandada.ToString("#.##");
        public string PotDemWatts => (Linea.Destination.PotenciaDemandada * 0.9).ToString("#.##");
        public Double FacTem => Linea.FactorTemperartura;
        public Double FacAgr => Linea.FactorAgrupamiento;
        public Double FacPotencia => Linea.FactorPotencia;
        public int VoltajeNominal => (int)Linea.Destination.Tension;
        public string CorrienteNominal => Linea.Destination.CorrienteNominal.ToString("#.##");
        public string CorrienteContinua => Linea.Destination.CorrienteContinua.ToString("#.##");
        public string CorrienteCorregida => Linea.CorrienteCorregida.ToString("#.##");
        public String Aliment => Linea.Conductor.Alimentador;
        public String Canal => Linea.Conductor.Canalizacion;
        public String Length => Linea.Longitud.ToString("#.##");
        public String Imped => Linea.Impedancia.ToString("#.##");
        public String Resist => Linea.Resistencia.ToString("#.##");
        public String React => Linea.Reactancia.ToString("#.##");
        public String CaidaDeVoltaje => Linea.CaidaVoltaje.ToString("#.##");
        /// <summary>
        /// Gets or sets the interruptor.
        /// </summary>
        /// <value>
        /// The interruptor.
        /// </value>
        public String Interruptor => Linea.Interruptor;
        Linea Linea;
        public AlimentadorRow(Linea linea)
        {
            Linea = linea;
        }
    }
}
