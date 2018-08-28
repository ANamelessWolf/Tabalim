using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Tabalim.Core.runtime;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la clase que mapea la información que muestra la tabla
    /// </summary>
    public class CtoCompItem
    {
        /// <summary>
        /// Define o establece la miniatura del circuito.
        /// </summary>
        /// <value>
        /// La miniatura del circuito
        /// </value>
        public ImageSource Icon { get; set; }
        /// <summary>
        /// Define o establece el nombre del circuito.
        /// </summary>
        /// <value>
        /// El nombre circuito
        /// </value>
        public String CtoKey { get; set; }
        /// <summary>
        /// Define o establece la descripción del circuito
        /// </summary>
        /// <value>
        /// La descripción del circuito
        /// </value>
        public String CtoFormat { get; set; }
        /// <summary>
        /// Define o establece la longitud del cto en metros.
        /// </summary>
        /// <value>
        /// La longitud del circuito en metros
        /// </value>
        public double CtoLength { get; set; }
        /// <summary>
        /// Define o establece el texto que describe al componente.
        /// </summary>
        /// <value>
        /// La descripción del componente
        /// </value>
        public String ComKey { get; set; }
        /// <summary>
        /// Define o establece el id del componente.
        /// </summary>
        /// <value>
        /// El id del componente
        /// </value>
        public int CompId { get; set; }
        /// <summary>
        /// Devuelve el componente que componente del elemento
        /// </summary>
        /// <value>
        /// El componente
        /// </value>
        public Componente Component
        {
            get
            {
                if (TabalimApp.CurrentTablero != null &&
                    TabalimApp.CurrentTablero.Componentes != null &&
                    TabalimApp.CurrentTablero.Componentes.ContainsKey(this.CompId))
                    return TabalimApp.CurrentTablero.Componentes[this.CompId];
                else
                    return null;
            }
        }
        /// <summary>
        /// Devuelve el circuito que compone al elemento
        /// </summary>
        /// <value>
        /// El circuito
        /// </value>
        public Circuito Circuit
        {
            get
            {
                if (TabalimApp.CurrentTablero != null &&
                    TabalimApp.CurrentTablero.Circuitos != null &&
                    TabalimApp.CurrentTablero.Circuitos.ContainsKey(this.CtoKey))
                    return TabalimApp.CurrentTablero.Circuitos[this.CtoKey];
                else
                    return null;
            }
        }
    }
}
