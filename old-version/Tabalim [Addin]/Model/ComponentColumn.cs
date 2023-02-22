using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Addin.Model
{
    /// <summary>
    /// La columna del componente
    /// </summary>
    public class ComponentColumn
    {
        /// <summary>
        /// Los watts del componente
        /// </summary>
        public double W;
        /// <summary>
        /// El indice de la imagen del componente
        /// </summary>
        public int ImageIndex;
        /// <summary>
        /// El valor de la potencia a utilizar
        /// </summary>
        public string Potencia;
        /// <summary>
        /// El valor de VA
        /// </summary>
        public double VA;
        /// <summary>
        /// Total de unidades
        /// </summary>
        public int Unidades;
        /// <summary>
        /// Total de watts totales
        /// </summary>
        public double WattsTotales;
        /// <summary>
        /// Total de VA
        /// </summary>
        public double VATotales;
    }
}
