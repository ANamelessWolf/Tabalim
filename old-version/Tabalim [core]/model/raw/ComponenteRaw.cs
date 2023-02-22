using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model.raw
{
    public class ComponenteRaw
    {
        /// <summary>
        /// Los watts del componente
        /// </summary>
        public double W => Componente.First().Potencia.Watts;
        /// <summary>
        /// El indice de la imagen del componente
        /// </summary>
        public int ImageIndex => Componente.First().ImageIndex;
        /// <summary>
        /// El valor de la potencia a utilizar
        /// </summary>
        public string Potencia => Componente.First() is Motor ? runtime.TabalimApp.Motores.FirstOrDefault(x => x.HP == Componente.First().Potencia.HP)?.HPFormat : Componente.First().Potencia.ToString();
        /// <summary>
        /// El valor de VA
        /// </summary>
        public double VA => Componente.First().Potencia.Watts / 0.9;
        /// <summary>
        /// Total de unidades
        /// </summary>
        public int Unidades => Componente.Sum(x => x.Count);
        /// <summary>
        /// Total de watts totales
        /// </summary>
        public double WattsTotales => W * Unidades;
        /// <summary>
        /// Total de VA
        /// </summary>
        public double VATotales => VA * Unidades;
        [JsonIgnore]
        public ComponentType Type => Componente.First().CType; 
        [JsonIgnore]
        Componente[] Componente;
        public ComponenteRaw(Componente[] c)
        {
            this.Componente = c;
        }
    }
}
