using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Tabalim.Addin.Controller;
using System.Reflection;
using System.IO;
using static Tabalim.Addin.Assets.AddinConstants;
using Autodesk.AutoCAD.ApplicationServices;


namespace Tabalim.Addin.Model
{
    public class AlimentadorContent : ITableroFunctionability
    {
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string Tablero;
        /// <summary>
        /// La descripción del tablero
        /// </summary>
        public string Description;
        /// <summary>
        /// Las líneas del Alimentador
        /// </summary>
        public AlimentadorRow[] Lineas;
        /// <summary>
        /// Define el número máximo de páginas que despliega la tabla
        /// </summary>
        public int MaxLinesPerPage = 20;
        /// <summary>
        /// El indice de la página a inserta
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// El total de páginas a insertar
        /// </summary>
        public int PageCount;
        /// <summary>
        /// Devuelve el formato de página.
        /// </summary>
        /// <value>
        /// The page format.
        /// </value>
        public string PageFormat
        {
            get
            {
                return String.Format("{0}/{1}", this.PageIndex, this.PageCount);
            }

        }
        /// <summary>
        /// Fixes the content by pages.
        /// </summary>
        /// <param name="fullContent">The full content.</param>
        /// <returns>La lista de contenidos</returns>
        /// <exception cref="Exception">Ningun contenido especificado</exception>
        internal static List<AlimentadorContent> FixContentByPages(AlimentadorContent fullContent)
        {
            List<AlimentadorContent> contentByPages = new List<AlimentadorContent>();
            if (fullContent == null)
                throw new Exception("Ningun contenido especificado");
            else if (fullContent.Lineas.Length > 20)
            {
                int max = (int)Math.Ceiling((double)fullContent.Lineas.Length / 20d);
                int pgIndex = 1;
                fullContent.PageCount = max;
                fullContent.PageIndex = 1;
                contentByPages.Add(fullContent);
                while (contentByPages.Last().Lineas.Length > 20)
                {
                    var lines = contentByPages.Last().Lineas;
                    var row = lines.Take(20);
                    var skip = lines.Skip(20);
                    lines = row.ToArray();
                    contentByPages.Last().Lineas = lines;
                    var copy = contentByPages.Last().Clone(skip);
                    copy.PageCount = max;
                    copy.PageIndex = ++pgIndex;
                    contentByPages.Add(copy);
                }
            }
            else
            {
                fullContent.PageCount = 1;
                fullContent.PageIndex = 1;
                contentByPages.Add(fullContent);
            }
            return contentByPages;
        }

        /// <summary>
        /// Inicia el proceso de la carga de bloques
        /// </summary>
        /// <exception cref="NotImplementedException">No hay bloques que cargar, este metodo no tiene utilidad para alimentadores.</exception>
        public void LoadBlocks()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Devuelve una copia de la tabla con distintas filas.
        /// </summary>
        /// <param name="rows">Las líneas que componen a la table.</param>
        /// <returns>El nuevo objeto que compone la tabla</returns>
        public AlimentadorContent Clone(IEnumerable<AlimentadorRow> rows)
        {
            return new AlimentadorContent()
            {
                Description = this.Description,
                Lineas = rows.ToArray(),
                MaxLinesPerPage = this.MaxLinesPerPage,
                Tablero = this.Tablero
            };
        }
    }
}
