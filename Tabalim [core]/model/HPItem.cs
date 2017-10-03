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
            this.HPFormat = result.GetString("format");
            if (this.HPFormat != null)
                this.HPFormat += " HP";
        }
    }
}
