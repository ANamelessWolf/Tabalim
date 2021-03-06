﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    class CircuitoBifasico : Circuito
    {
        /// <summary>
        /// Obtiene la corriente.
        /// </summary>
        /// <value>
        /// La corriente dependiendo del numero de polos.
        /// </value>
        public override double Corriente
        {
            get
            {
                if (HasMotor)
                    return ComponentsUtils.GetCorriente(Componentes.Values.First(x => x is Motor), this, Tension);
                return PotenciaTotal / Tension.Value;
            }
        }

        public override double CaidaVoltaje { get => (2 * Math.Sqrt(3) * Longitud * CorrienteCorregida) / (Tension.Value * Calibre?.AreaTransversal ?? 1); }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoBifasico"/>.
        /// </summary>
        public CircuitoBifasico(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoBifasico"/>.
        /// </summary>
        public CircuitoBifasico() : base() { }
        /// <summary>
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>
        /// Regresa el nuevo circuito creado
        /// </returns>
        public override Circuito Clone()
        {
            return new CircuitoBifasico()
            {
                Polos = this.Polos,
                FactorAgrupacion = this.FactorAgrupacion,
                //FactorTemperatura = this.FactorTemperatura,
                Longitud = this.Longitud,
                Id = -1,
                TableroId = -1,
                Tension = this.Tension,
                Calibre = this.Calibre
            };
        }
    }
}
