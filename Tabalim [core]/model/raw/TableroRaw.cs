﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model.raw
{
    public class TableroRaw
    {
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string Tablero => t.Description;
        /// <summary>
        /// El nombre de la imagen del tablero
        /// </summary>
        public String ImagenTablero => t.Sistema.ImageName;
        /// <summary>
        /// El sistema a utilizar
        /// </summary>
        public string Sistema => t.Sistema.ToString();
        /// <summary>
        /// La temperatura del sistema
        /// </summary>
        public Double Temperatura => t.Sistema.Temperatura;
        /// <summary>
        /// El total de VA en alumbrados
        /// </summary>
        public Double AlumbradosVA => CmpColumns.Where(x => x.Type == ComponentType.Alumbrado).Sum(x => x.VATotales);
        /// <summary>
        /// El total de VA en contactos
        /// </summary>
        public Double ContactosVA => CmpColumns.Where(x => x.Type == ComponentType.Contacto).Sum(x => x.VATotales);
        /// <summary>
        /// El total de VA en motores
        /// </summary>
        public Double MotoresVA => CmpColumns.Where(x => x.Type == ComponentType.Motor).Sum(x => x.VATotales);
        /// <summary>
        /// El total de VA en reserva
        /// </summary>
        public Double ReservaVA => 0;
        /// <summary>
        /// El desbalanceo máxima
        /// </summary>
        public double DesbMax
        {
            get
            {
                double max, min;
                if (t.Sistema.Fases == 3)
                {
                    max = Math.Max(TotalPTA, Math.Max(TotalPTB, TotalPTC));
                    min = Math.Min(TotalPTA, Math.Min(TotalPTB, TotalPTC));
                    return (max - min) / max;
                }
                else if (t.Sistema.Fases == 2)
                {
                    max = Math.Max(TotalPTA, TotalPTB);
                    min = Math.Min(TotalPTA, TotalPTB);
                    return (max - min) / max;
                }
                else
                    return 0;
            }
        }
        /// <summary>
        /// Obtiene el total de la potencia A
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        [JsonIgnore]
        public double TotalPTA { get { return this.CtoRows.Sum(x => x.PotenciaA); } }
        /// <summary>
        /// Obtiene el total de la potencia B
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        [JsonIgnore]
        public double TotalPTB { get { return this.CtoRows.Sum(x => x.PotenciaB); } }
        /// <summary>
        /// Obtiene el total de la potencia C
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        [JsonIgnore]
        public double TotalPTC { get { return this.CtoRows.Sum(x => x.PotenciaC); } }
        /// <summary>
        /// Las filas de circuito
        /// </summary>
        public CircuitoRaw[] CtoRows => t.Circuitos.Values.Select(x => new CircuitoRaw(t, x)).ToArray();
        /// <summary>
        /// Las columnas del circuito
        /// </summary>
        public ComponenteRaw[] CmpColumns
        {
            get
            {
                var componentes = t.Componentes;
                return componentes.Values.GroupBy(x => x.Key).Select(x => new ComponenteRaw(x.ToArray())).ToArray();
            }
        }
        [JsonIgnore]
        Tablero t;
        public TableroRaw(Tablero t)
        {
            this.t = t;
        }
    }
}