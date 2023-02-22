using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class AlimentadorRow
    {
        const String DOUBLE_FORMAT = "0.##;;#.##";
        public int Id => Linea.Id;
        /// <summary>
        /// El número de línea
        /// </summary>
        public string No { get { return Linea.No; } }
        public String From { get { return Linea.From; } }
        public DestinationType ToType => Linea.Type;
        public String To { get { return Linea.To; } }
        /// <summary>
        /// El destino de la línea usado en el campo "A"
        /// Descripción
        /// </summary>
        public string ToDesc { get { return Linea.ToDesc; } }
        public string PotInstVA => Linea.Destination.PotenciaInstalada.ToString(DOUBLE_FORMAT);
        public string PotInstWatts => (Linea.Destination.PotenciaInstalada * 0.9).ToString(DOUBLE_FORMAT);
        public string PotInstHP => Linea.Destination.PotenciaHP.ToString(DOUBLE_FORMAT);
        public Double FacDemanda => Linea.Destination.FactorDemanda;


        public string PotDemAlum => Linea.Destination.PotenciaDemandadaAlumbrado.ToString(DOUBLE_FORMAT);
        public string PotDemCont => Linea.Destination.PotenciaDemandadaContactos.ToString(DOUBLE_FORMAT);
        public string PotDemForce => Linea.Destination.PotenciaDemandadaFuerza.ToString(DOUBLE_FORMAT);
        public string PotDemVA => Linea.Destination.PotenciaDemandada.ToString(DOUBLE_FORMAT);
        public string PotDemWatts => (Linea.Destination.PotenciaDemandada * 0.9).ToString(DOUBLE_FORMAT);
        public Double FacTem => Linea.FactorTemperartura;
        public Double FacAgr => Linea.FactorAgrupamiento;
        public Double FacPotencia => Linea.FactorPotencia;
        public int VoltajeNominal => (int)Linea.Destination.Tension;
        public string CorrienteNominal => Linea.Destination.CorrienteNominal.ToString(DOUBLE_FORMAT);
        public string CorrienteContinua => Linea.Destination.CorrienteContinua.ToString(DOUBLE_FORMAT);
        public string CorrienteCorregida => Linea.CorrienteCorregida.ToString(DOUBLE_FORMAT);
        public string Material => Linea.Material;
        public String Aliment => Linea.Conductor.Alimentador;
        public String Canal => Linea.Conductor.Canalizacion;
        public String Length => Linea.Longitud.ToString(DOUBLE_FORMAT);
        public String Imped => Linea.Impedancia.ToString("0.0####E+0");
        public String Resist => Linea.Resistencia.ToString("0.0####E+0");
        public String React => Linea.Reactancia.ToString("0.0####E+0");
        public String CaidaDeVoltaje => Linea.CaidaVoltaje.ToString(DOUBLE_FORMAT);
        /// <summary>
        /// Gets or sets the interruptor.
        /// </summary>
        /// <value>
        /// The interruptor.
        /// </value>
        public String Interruptor => Linea.Interruptor;
        public String DSPF => Linea.DSPF;
        public String Fusible => Linea.Fusible;
        Linea Linea;
        public AlimentadorRow(Linea linea)
        {
            Linea = linea;
            //Crear conductor aqui
            if (Linea.Conductor == null)
                Linea.Conductor = new Conductor(Conductor.GetConductor(Linea.SelectedCalibre, Linea.CorrienteCorregida, Linea.Destination.Hilos, Linea.SelectedConductor, Linea.IsCobre, Linea.IsCharola, Linea.Extra));
            //Linea.Conductor = Conductor.GetConductorOptions(Linea.Destination.Fases, Linea.CorrienteCorregida, Linea.IsCobre, Linea.Destination.Hilos)[Linea.SelectedConductor];
        }
        public override string ToString()
        {
            return String.Format("RENGLON - {0} {1} {2} {3} {4}", No, ToDesc, Material, Aliment, Canal);
        }
    }
}
