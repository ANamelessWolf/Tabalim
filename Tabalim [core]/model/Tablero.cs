using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la tabla de tablero que utiliza la aplicación
    /// </summary>
    public class Tablero
    {
        /// <summary>
        /// El id del proyecto al que pertenecen los tableros.
        /// </summary>
        public int ProjectId;
        /// <summary>
        /// El id del tablero.
        /// </summary>
        public int TableroId;
        /// <summary>
        /// El sistema que ocupa el tablero.
        /// </summary>
        public SistemaFases Sistema;
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string NombreTablero;
        /// <summary>
        /// La colección de circuitos que compone un tablero
        /// </summary>
        public Dictionary<String, Circuito> Circuitos;
        /// <summary>
        /// La colección de componentes que se conectan en un tablero
        /// </summary>
        public Dictionary<String, Componente> Componentes;
    }
}
