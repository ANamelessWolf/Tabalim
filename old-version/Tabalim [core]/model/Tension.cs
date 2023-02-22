using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la tensión del sistema
    /// </summary>
    public class Tension
    {
        /// <summary>
        /// El sistema al que pertenece la tensión
        /// </summary>
        public readonly SistemaFases Sistema;
        /// <summary>
        /// El valor de la tension
        /// </summary>
        public readonly int Value;
        /// <summary>
        /// Devuelve la tensión al neutro
        /// </summary>
        /// <value>
        /// La tension al Neutro.
        /// </value>
        public readonly int TensionAlNeutro;

        /// <summary>
        /// Inicializa una nueva instancia de la <see cref="Tension"/>.
        /// </summary>
        /// <param name="tension">El valor de la tensión.</param>
        /// <param name="sistema">El sistema de fases.</param>
        public Tension(TensionVal tension, SistemaFases sistema)
        {
            this.Sistema = sistema;
            this.Value = (int)tension;
            int tn = (int)(Math.Round((double)this.Value / Math.Sqrt(3)));
            if (sistema.Fases == 1)
            {
                this.Value = tn;
                this.TensionAlNeutro = tn;
            }
            else
                this.TensionAlNeutro = tn;
        }
        /// <summary>
        /// Inicializa una nueva instancia de la <see cref="Tension"/>.
        /// </summary>
        /// <param name="tension">El valor de la tensión.</param>
        /// <param name="fases">El número de fases del sistema</param>
        public Tension(TensionVal tension, int fases)
        {
            this.Sistema = null;
            this.Value = (int)tension;
            int tn = (int)(Math.Round((double)this.Value / Math.Sqrt(3)));
            if (fases == 1)
            {
                this.Value = tn;
                this.TensionAlNeutro = tn;
            }
            else
                this.TensionAlNeutro = tn;
        }
        /// <summary>
        /// Devuelve la <see cref="System.String" /> que representa la tensión
        /// </summary>
        /// <returns>
        /// El valor de la tensión
        /// </returns>
        public override string ToString()
        {
            if (this.Sistema != null && this.Sistema.Fases == 3 )
                return String.Format("{0} - {1}", this.Value, this.TensionAlNeutro);
            else
                return this.Value.ToString();
        }
    }
}
