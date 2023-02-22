using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Addin.Model
{
    /// <summary>
    /// Defines the header options
    /// </summary>
    public class HeaderOptions
    {
        /// <summary>
        /// El titulo principal de la cabecera
        /// </summary>
        public string Header;
        /// <summary>
        /// El titulo secundario de la cabecera
        /// </summary>
        public string SubHeader;
        /// <summary>
        /// El ancho de la columna
        /// </summary>
        public Double ColumnWidth;
        /// <summary>
        /// The número de filas que define a la columna
        /// </summary>
        public int RowSpan;
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderOptions"/> class.
        /// </summary>
        public HeaderOptions()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderOptions"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="subheader">The subheader.</param>
        /// <param name="colWidth">Width of the col.</param>
        /// <param name="rowSpan">The row span.</param>
        public HeaderOptions(string header, string subheader, double colWidth, int rowSpan = 3)
        {
            this.Header = header;
            this.SubHeader = subheader;
            this.ColumnWidth = colWidth;
            this.RowSpan = rowSpan;
        }
        /// <summary>
        /// Define una cabecera sin sub cabecera
        /// </summary>
        /// <param name="header">La cadena de la cabecera.</param>
        /// <param name="colWidth">El ancho de la columna.</param>
        /// <param name="rowSpan">El número de filas que abarca la columna.</param>
        /// <returns>Las opciones de cabecera</returns>
        public static HeaderOptions NoSubHeader(String header, Double colWidth, int rowSpan = 3)
        {
            return new HeaderOptions()
            {
                ColumnWidth = colWidth,
                Header = header,
                SubHeader = String.Empty,
                RowSpan = rowSpan
            };
        }
    }
}

