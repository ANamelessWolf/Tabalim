using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class CtoRow : GenericRow
    {
        const String DOUBLE_FORMAT = "0.##;;#.##";
        public Circuito Circuito;
        public bool IsValid => Circuito.Longitud > 0 && Circuito.Calibre != null;
        public String Nombre => Circuito.ToString();
        public int Potencia => (int)Math.Round(Circuito.PotenciaTotal);
        public string Tension => Circuito.Polos.Length == 1 ? Circuito.Tension.TensionAlNeutro.ToString() : Circuito.Tension.Value.ToString();
        public string Fases => Circuito.Polos.Length.ToString();
        public String Corriente => Circuito.Corriente.ToString(DOUBLE_FORMAT);
        public String Continua => Circuito.CorrienteContinua.ToString(DOUBLE_FORMAT);
        public String Longitud => Circuito.Longitud.ToString(DOUBLE_FORMAT);
        public String FacAgr => Circuito.FactorAgrupacion.ToString(DOUBLE_FORMAT);
        public String FacTmp => Circuito.FactorTemperatura.ToString(DOUBLE_FORMAT);
        public String Calibre => Circuito.Calibre?.AWG;
        public String Seccion => Circuito.Calibre?.AreaTransversal.ToString(DOUBLE_FORMAT);
        public String Caida => Circuito.CaidaVoltaje.ToString(DOUBLE_FORMAT);
        public String PotenciaA => Circuito.PotenciaEnFases[0].ToString(DOUBLE_FORMAT);
        public String PotenciaB => Circuito.PotenciaEnFases[1].ToString(DOUBLE_FORMAT);
        public String PotenciaC => Circuito.PotenciaEnFases[2].ToString(DOUBLE_FORMAT);
        public String Proteccion => Circuito.CorrienteProteccion.ToString(DOUBLE_FORMAT);
        public String Interruptor => Circuito.Interruptor;
        public string Tierra => Circuito.Tierra;
        public Dictionary<String, int> Componentes;
        public string this[string name] => Componentes.ContainsKey(name) ? Componentes[name].ToString() : String.Empty;
        public CtoRow(Circuito c)
        {
            this.Circuito = c;
            this.Componentes = c.Componentes.Values.ToDictionary(k => k.Key, e => e.Count);
        }
    }
}
