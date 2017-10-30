using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class CtoRow
    {
        public Circuito Circuito;
        public String Nombre => Circuito.ToString();
        public int Potencia => (int)Math.Round(Circuito.PotenciaTotal);
        public int Tension => Circuito.Polos.Length == 1 ? Circuito.Tension.TensionAlNeutro : Circuito.Tension.Value;
        public int Fases => Circuito.Polos.Length;
        public String Corriente => Circuito.Corriente.ToString("#.##");
        public int Longitud => (int)Circuito.Longitud;
        public String FacAgr => Circuito.FactorAgrupacion.ToString("#.##");
        public String FacTmp => Circuito.FactorTemperatura.ToString("#.##");
        public String Calibre => Circuito.Calibre?.AWG;
        public String Seccion => Circuito.Calibre?.AreaTransversal.ToString("#.##");
        public String Caida => Circuito.CaidaVoltaje.ToString("#.##");
        public String PotenciaA => Circuito.PotenciaEnFases[0].ToString("#.##");
        public String PotenciaB => Circuito.PotenciaEnFases[1].ToString("#.##");
        public String PotenciaC => Circuito.PotenciaEnFases[2].ToString("#.##");
        public String Proteccion => Circuito.CorrienteProteccion.ToString("#.##");
        public String Interruptor => Circuito.Interruptor;
        public Dictionary<String, int> Componentes;
        public string this[string name] => Componentes.ContainsKey(name) ? Componentes[name].ToString() : String.Empty;
        public CtoRow(Circuito c)
        {
            this.Circuito = c;
            this.Componentes = c.Componentes.Values.ToDictionary(k => k.Key, e => e.Count);
        }
    }
}
