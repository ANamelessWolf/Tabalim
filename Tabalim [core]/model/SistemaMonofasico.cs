﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Establece un sistema con una fase y un neutro.
    /// </summary>
    /// <seealso cref="Tabalim.Core.model.SistemaFases" />
    public class SistemaMonofasico : SistemaFases
    {
        /// <summary>
        /// Devuelve el número de fases que define el sistema.
        /// </summary>
        /// <value>
        /// El número de fases.
        /// </value>
        public override int Fases { get { return 1; } }
        /// <summary>
        /// Devuelve los polos que soporta el sistema
        /// </summary>
        /// <value>
        /// Los polos disponibles para el sistema seleccionado
        /// </value>
        public override int[] Polos { get { return new int[] { 2, 6, 4, 8 }; } }
    }
}
