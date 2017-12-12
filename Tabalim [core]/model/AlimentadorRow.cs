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
        public string No { get; set; }
        public String From { get; set; }
        public DestinationType ToType { get; set; }
        public String To { get { return ToType.ToString(); } }
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// Descripción
        /// </summary>
        public string ToDesc { get; set; }
        public Double PotInstVA { get; set; }
        public Double PotInstWatts { get; set; }
        public Double FacDemanda { get; set; }
        

        public Double PotDemAlum { get; set; }
        public Double PotDemCont { get; set; }
        public Double PotDemForce { get; set; }
        public Double PotDemVA { get; set; }
        public Double PotDemWatts { get; set; }
        public Double FactorDemanda { get; set; }
        public Double FacTem { get; set; }
        public Double FactorAgrupamiento { get; set; }
        public Double FacPotencia { get; set; }
        public Double VoltajeNominal { get; set; }
        public Double CorrienteNominal { get; set; }
        public Double CorrienteContinua { get; set; }
        public Double CorrienteCorregida { get; set; }
        public String Aliment { get; set; }
        public String Canal { get; set; }
        public Double Length { get; set; }
        public Double Imped { get; set; }
        public Double Resist { get; set; }
        public Double React { get; set; }
        public Double CaidaDeVoltaje { get; set; }
        public String Interruptor { get; set; }
    }
}
