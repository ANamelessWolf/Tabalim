using System;
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
                    return ComponentsUtils.GetCorriente(Componentes.Values.First(x => x is Motor), this, Tension);
                else
                    return PotenciaTotal / Tension.TensionAlNeutro;
            }
        }

        public override double CaidaVoltaje { get => (4 * Longitud * CorrienteCorregida) / (Tension.TensionAlNeutro * Calibre?.AreaTransversal ?? 1); }
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
                //FactorTemperatura = this.FactorTemperatura,
                Longitud = this.Longitud,
                Id = -1,
                TableroId = -1,
                Tension = this.Tension
            };
        }
    }
}
