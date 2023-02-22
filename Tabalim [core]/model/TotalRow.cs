using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class TotalRow : GenericRow
    {
        const String DOUBLE_FORMAT = "0.##;;#.##";
        public string this[string name] => Componentes.ContainsKey(name) ? Componentes[name].ToString() : String.Empty;
        public bool IsValid => Data.All(x => x.Longitud > 0);
        public string Nombre => "Totales:";

        public int Potencia => (int)Math.Round(Data.Sum(x => x.PotenciaTotal));

        public string Tension => String.Empty;

        public string Fases => String.Empty;

        public string Corriente => String.Empty;

        public string Continua => String.Empty;

        public string Longitud => String.Empty;

        public string FacAgr => String.Empty;

        public string FacTmp => String.Empty;

        public string Calibre => String.Empty;

        public string Seccion => String.Empty;

        public string Caida => String.Empty;

        public string PotenciaA => Data.Sum(x => x.PotenciaEnFases[0]).ToString(DOUBLE_FORMAT);

        public string PotenciaB => Data.Sum(x => x.PotenciaEnFases[1]).ToString(DOUBLE_FORMAT);

        public string PotenciaC => Data.Sum(x => x.PotenciaEnFases[2]).ToString(DOUBLE_FORMAT);

        public string Proteccion => String.Empty;

        public string Interruptor => String.Empty;

        public string Tierra => String.Empty;

        public Dictionary<String, int> Componentes;
        public IEnumerable<Circuito> Data;
        public TotalRow(IEnumerable<Circuito> data)
        {
            Data = data;
            Componentes = new Dictionary<string, int>();
            foreach (Circuito c in Data)
                foreach (Componente cp in c.Componentes.Values)
                    if (Componentes.ContainsKey(cp.Key))
                        Componentes[cp.Key] += cp.Count;
                    else
                        Componentes.Add(cp.Key, cp.Count);
        }
    }
}
