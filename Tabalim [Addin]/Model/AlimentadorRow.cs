using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Controller;

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
        /// La potencia de instalación como HP's
        /// </summary>
        public double PotInstHp;
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
        /// El número de fases
        /// </summary>
        public double Fases;
        /// <summary>
        /// El voltaje nominal
        /// </summary>
        public double VoltajeNominal;
        /// <summary>
        /// La corriente nominal
        /// </summary>
        public double CorrienteNominal;
        /// <summary>
        /// La corriente corregida
        /// </summary>
        public double CorrienteCorregida;
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FacTem;
        /// <summary>
        /// El factor de agrupación
        /// </summary>
        public double FacAgr;
        /// <summary>
        /// Material
        /// </summary>
        public string Material;
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
        public string DSPF;
        public string Fusible;
        /// <summary>
        /// Gets the horizontal format.
        /// </summary>
        /// <returns>The line format</returns>
        public string GetHorizontalFormat()
        {
            return String.Format("{5}@In={0:N2}@{1}@{2}@L={3:N2}m@e%={4:N2}", CorrienteNominal, Aliment, Canal, Length, CaidaDeVoltaje, No);
        }
        /// <summary>
        /// Gets the vertical format.
        /// </summary>
        /// <returns>The line format</returns>
        public string GetVerticalFormat()
        {
            return String.Format("{5}@{1}@{2}@In={0:N2}@L={3:N2}m@e%={4:N2}", CorrienteNominal, Aliment, Canal, Length, CaidaDeVoltaje, No);
        }
        /// <summary>
        /// Obtiene los valores de la línea seleccionada
        /// </summary>
        /// <returns>Los valores de la línea seleccionada.</returns>
        public string[] GetValues()
        {
            return new String[]
            {
                this.No,                                        //0
                this.Origin,                                    //1
                this.To,                                        //2
                this.PotInstVA.ToNumberFormat(),                //3
                this.PotInstWatts.ToNumberFormat(),             //4
                this.PotInstHp.ToNumberFormat(),
                this.FacDemanda.ToNumberFormat(),               //5
                this.PotDemAlum.ToNumberFormat(),               //6
                this.PotDemCont.ToNumberFormat(),               //7
                this.PotDemForce.ToNumberFormat(),              //8
                this.PotDemVA.ToNumberFormat(),                 //9
                this.PotDemWatts.ToNumberFormat(),              //10
                this.FacPotencia.ToNumberFormat(),              //11
                this.Fases.ToNumberFormat(),                    //12
                this.VoltajeNominal.ToNumberFormat(),           //13
                this.CorrienteNominal.ToNumberFormat(),         //14
                this.FacTem.ToNumberFormat(),                   //15
                this.FacAgr.ToNumberFormat(),                   //16
                this.CorrienteCorregida.ToNumberFormat(),       //17
                this.Material,                                  //18
                this.Aliment,                                   //19
                this.Length.ToNumberFormat(),                   //20
                this.Imped.ToExpFormat(),                       //21
                this.Resist.ToExpFormat(),                      //22
                this.React.ToExpFormat(),                       //23
                this.CaidaDeVoltaje.ToNumberFormat(),           //24
                this.Interruptor,                               //25
                this.DSPF,
                this.Fusible
            };
        }
    }
}
