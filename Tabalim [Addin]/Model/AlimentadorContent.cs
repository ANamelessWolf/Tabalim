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
