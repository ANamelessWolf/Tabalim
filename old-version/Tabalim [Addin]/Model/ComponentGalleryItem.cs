using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tabalim.Addin.Model
{
    /// <summary>
    /// Define un elemento que utiliza la lista de componentes
    /// </summary>
    public class ComponentGalleryItem
    {
        /// <summary>
        /// La imagen del componente
        /// </summary>
        /// <value>
        /// La fuente de la imagen del componente.
        /// </value>
        public ImageSource Src { get; set; }
        /// <summary>
        /// Obtiene el tipo de componente al que pertenece la imagen
        /// </summary>
        /// <value>
        /// El tipo de componente de la imagen
        /// </value>
        public ComponentType CType { get; set; }
        /// <summary>
        /// El indice del componente seleccionado
        /// </summary>
        public int Index;
    }
}
