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
        public String From { get; set; }
        public DestinationType ToType { get; set; }
        public Double PotenciaAparente { get; set; }
        public Double Potencia { get; set; }
        public Double PotenciaAlumbrado { get; set; }
        public Double PotenciaContactos { get; set; }
        public Double PotenciaFuerza { get; set; }
        public Double PotenciaDemandadaAparente { get; set; }
        public Double PotenciaDemandada { get; set; }
        public Double FactorDemanda { get; set; }
        public Double FactorTemperatura { get; set; }
        public Double FactorAgrupamiento { get; set; }
        public Double FactorPotencia { get; set; }
        public Double VoltajeNominal { get; set; }
        public Double CorrienteNominal { get; set; }
        public Double CorrienteContinua { get; set; }
        public Double CorrienteCorregida { get; set; }
        public String AlimentResult { get; set; }
        public String Canalizacion { get; set; }
        public Double Longitud { get; set; }
        public Double Impedancia { get; set; }
        public Double Resistancia { get; set; }
        public Double Reactancia { get; set; }
        public Boolean IsCobre { get; set; }
        public String Interruptor { get; set; }
    }
}
