using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Especifica un tipo de sisteme que utilizá fases para la conexión
    /// </summary>
    public abstract class SistemaFases
    {
        /// <summary>
        /// La frecuencia del sistema
        /// </summary>
        public const int Frecuencia = 60;
        /// <summary>
        /// Devuelve el nombre del elemento que se utiliza en una lista de sistemas
        /// </summary>
        /// <value>
        /// El nombre del elemento.
        /// </value>
        public string ItemName
        {
            get
            {
                string sys = this.Fases == 1 ? CAP_F_1 : this.Fases == 2 ? CAP_F_2 : CAP_F_3;
                return string.Format(FORMAT_SYS_ITEM, sys, this.Tension, this.Fases, this.Hilos, Frecuencia);
            }
        }
        /// <summary>
        /// Devuelve el número de fases que define el sistema.
        /// </summary>
        /// <value>
        /// El número de fases.
        /// </value>
        public abstract int Fases { get; }
        /// <summary>
        /// La tensión del sistema en volts
        /// </summary>
        public Tension Tension;
        /// <summary>
        /// Devuelve los polos que soporta el sistema
        /// </summary>
        /// <value>
        /// Los polos disponibles para el sistema seleccionado
        /// </value>
        public abstract int[] Polos { get; }
        /// <summary>
        /// Define el polo que utilizá el sistema
        /// </summary>
        public int Polo;
        /// <summary>
        /// La temperatura del sistema en grados centigrados
        /// </summary>
        public double Temperatura;
        /// <summary>
        /// Define si la alimentación se realiza mediante un interruptor o una zapata.
        /// </summary>
        public TipoAlimentacion TpAlimentacion;
        /// <summary>
        /// El número de hilos o líneas de cables que define el sistema.
        /// </summary>
        /// <value>
        /// El número de hilos.
        /// </value>
        public int Hilos { get { return this.Fases + 1; } }
        /// <summary>
        /// Establece el valor de la tensión
        /// </summary>
        /// <returns>El valor de la tensión</returns>
        public void SetTension(TensionVal val)
        {
            this.Tension = new Tension(val, this);
        }
        /// <summary>
        /// Devuelve una <see cref="System.String" /> que representa el nombre del sistema.
        /// </summary>
        /// <returns>
        /// El nombre del sistema
        /// </returns>
        public override string ToString()
        {
            return String.Format(FORMAT_SYS_N_FASE, this.Tension, this.Fases, this.Hilos, this.Polo);
        }
    }
}
