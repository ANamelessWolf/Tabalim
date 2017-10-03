﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la potencia
    /// </summary>
    public class Potencia
    {
        /// <summary>
        /// Factor de conversión Caballos de fuerza a Watts
        /// </summary>
        const double HP_to_Watts = 745.7;
        /// <summary>
        /// Alamacena potencia en watts
        /// </summary>
        readonly double watts;
        /// <summary>
        /// Define si la potencia es de un motor
        /// </summary>
        bool motor;
        /// <summary>
        /// Regresa potencia en Watts
        /// </summary>
        public Double Watts { get { return watts; } }
        /// <summary>
        /// Regresa potencia en HP
        /// </summary>
        public Double HP { get { return watts / HP_to_Watts; } }
        /// <summary>
        /// Devuelve la <see cref="System.String" /> que representa la tensión
        /// </summary>
        /// <value>
        /// El valor de la potencia y sus unidades
        /// </value>
        public String PFormat { get { return this.ToString(); } }
        /// <summary>
        /// Inicializa una instancia de la <see cref="Tension"/>.
        /// </summary>
        /// <param name="n">El valor de la potencia</param>
        /// <param name="hp">True si la potencia es en HP</param>
        public Potencia(double n, bool hp = false)
        {
            watts = hp ? n * HP_to_Watts : n;
            motor = hp;
        }
        /// <summary>
        /// Devuelve la <see cref="System.String" /> que representa la tensión
        /// </summary>
        /// <returns>El valor de la potencia y sus unidades</returns>
        public override string ToString()
        {
            return !motor ? Watts + "W" : HP + "HP";
        }
    }
}