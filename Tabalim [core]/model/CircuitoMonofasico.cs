﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define Circuito de un polo
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.Circuito" />
    public class CircuitoMonofasico : Circuito
    {
        public override double Corriente
        {
            get
            {
                if (HasMotor)
                    return 0;
                else
                    return PotenciaTotal / Tension.TensionAlNeutro;
            }
        }

        public override double CaidaVoltaje { get => (4 * Longitud * CorrienteProteccion) / (Tension.TensionAlNeutro * Calibre.AreaTransversal); }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoMonofasico"/>.
        /// </summary>
        public CircuitoMonofasico(SelectionResult[] result) : base(result) { }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CircuitoMonofasico"/>.
        /// </summary>
        public CircuitoMonofasico() : base() { }
        /// <summary>
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>
        /// Regresa el nuevo circuito creado
        /// </returns>
        public override Circuito Clone()
        {
            return new CircuitoMonofasico()
            {
                Polos = this.Polos,
                FactorAgrupacion = this.FactorAgrupacion,
                FactorTemperatura = this.FactorTemperatura,
                Interruptor = this.Interruptor,
                Longitud = this.Longitud,
                Id = -1,
                TableroId = -1,
                Tension = this.Tension
            };
        }
    }
}
