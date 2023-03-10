using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la clase que llena la información de potencia en
    /// caballos de fuerza
    /// </summary>
    /// <seealso cref="Tabalim.Core.controller.ISQLiteParser" />
    public class HPItem : ISQLiteParser
    {
        /// <summary>
        /// Define o establece el valor númerico de kilo watts.
        /// </summary>
        /// <value>
        /// Los kilo watts
        /// </value>
        public double KW { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de caballos de fuerza.
        /// </summary>
        /// <value>
        /// Los caballos de fuerza
        /// </value>
        public double HP { get; set; }
        /// <summary>
        /// Define o establece formato de los caballos de fuerza.
        /// </summary>
        /// <value>
        /// El formato de los caballos de fuerza
        /// </value>
        public string HPFormat { get; set; }
        /// <summary>
        /// Define o establece la corriente para 1 fase con tension de 120-127 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_1_127 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 1 fase con tension de 254-277 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_1_230 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 2 fases con tension de 120-127 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_2_115 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 2 fases con tension de 208-220 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_2_230 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 2 fases con tension de 440-480 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_2_460 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 3 fases con tension de 120-127 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_3_115 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 3 fases con tension de 208 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_3_208 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 3 fases con tension de 220 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_3_230 { get; set; }
        /// <summary>
        /// Define o establece la corriente para 3 fases con tension de 440-480 V
        /// </summary>
        /// <value>
        /// Corriente
        /// </value>
        public double I_3_460 { get; set; }

        /// <summary>
        /// Constructor usado para deserializar con JSON
        /// </summary>
        public HPItem() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SQLite_Connector"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        public HPItem(SelectionResult[] result)
        {
            this.Parse(result);
        }
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        /// <returns>
        /// El resultado del parsing
        /// </returns>
        public void Parse(SelectionResult[] result)
        {
            this.HP = result.GetValue<double>("hp");
            this.KW = result.GetValue<double>("kW");
            this.I_1_127 = result.GetValue<double>("1p_127");
            this.I_1_230 = result.GetValue<double>("1p_230");
            this.I_2_115 = result.GetValue<double>("2p_115");
            this.I_2_230 = result.GetValue<double>("2p_230");
            this.I_2_460 = result.GetValue<double>("2p_460");
            this.I_3_115 = result.GetValue<double>("3p_115");
            this.I_3_208 = result.GetValue<double>("3p_208");
            this.I_3_230 = result.GetValue<double>("3p_230");
            this.I_3_460 = result.GetValue<double>("3p_460");
            this.HPFormat = result.GetString("format");
            if (this.HPFormat != null)
                this.HPFormat += " HP";
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.HPFormat;
        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>Los campos a insertar</returns>
        /// <exception cref="Exception">La inserción de la tabla esta deshabilitada.</exception>
        public InsertField[] GetInsertFields()
        {
            throw new Exception("Esta tabla no permite insertar desde la aplicación.");
        }
        /// <summary>
        /// Obtiene los campos de actualización de un objeto
        /// </summary>
        /// <param name="input">La entrada del campo actualizar</param>
        /// <returns>
        /// El campo actualizar
        /// </returns>
        /// <exception cref="Exception">Esta tabla no permite actualizar desde la aplicación.</exception>
        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            throw new Exception("Esta tabla no permite actualizar desde la aplicación.");
        }
        /// <summary>
        /// Actualiza el modelo en caso que el query fuese actualizado de manera correcta
        /// </summary>
        /// <param name="input">Los datos de entrada que se usarón para actualizar</param>
        /// <exception cref="Exception">Esta tabla no permite actualizar desde la aplicación.</exception>
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            throw new Exception("Esta tabla no permite actualizar desde la aplicación.");
        }
    }
}
