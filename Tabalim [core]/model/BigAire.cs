using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using static Tabalim.Core.assets.Constants;

namespace Tabalim.Core.model
{
    public class BigAire
    {
        const string AIRE_FORMAT = "AA {0}A - {1} fases";
        public string PrimaryKey => "motor_id";
        /// <summary>
        /// El id del componente es único en la aplicación.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de la tensión del motor
        /// </summary>
        /// <value>
        /// Los Tensión
        /// </value>
        public Tension Tension { get; set; }
        /// <summary>
        /// Define o establece el número de fases del motor
        /// </summary>
        /// <value>
        /// El número de fases del motor
        /// </value>
        public int Fases { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de la corriente del aire
        /// </summary>
        /// <value>
        /// Full Load Amp
        /// </value>
        public Double FLA { get; set; }
        /// <summary>
        /// Define o establece el número de hilos del motor
        /// </summary>
        /// <value>
        /// El número de hilos del motor
        /// </value>
        public int Hilos { get; set; }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BigAire"/>.
        /// </summary>
        public BigAire()
        {

        }
        public BigAire(BigMotor motor)
        {
            this.Id = motor.Id;
            this.Tension = motor.Tension;
            this.Fases = motor.Fases;
            this.Hilos = motor.Hilos;
            this.FLA = motor.Potencia.HP;
        }
        public BigMotor ToBigMotor()
        {
            return new BigMotor()
            {
                Id = this.Id,
                Fases = this.Fases,
                Tension = this.Tension,
                Hilos = this.Hilos,
                Potencia = new Potencia(this.FLA, true)
            };
        }
        /// <summary>
        /// Generá una <see cref="System.String" /> que representa la instancia.
        /// </summary>
        /// <returns>
        /// La <see cref="System.String" /> que representa a la instancia.
        /// </returns>
        public override string ToString()
        {
            return String.Format(AIRE_FORMAT, FLA, Fases, Tension);
        }
    }
}
