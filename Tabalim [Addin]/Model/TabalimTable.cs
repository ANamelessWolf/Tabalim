using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Controller;
using static Tabalim.Addin.Assets.AddinConstants;
namespace Tabalim.Addin.Model
{
    /// <summary>
    /// Realizá una definición de la tabla Tabalim
    /// </summary>
    public abstract class TabalimTable
    {
        /// <summary>
        /// La tabla de AutoCAD
        /// </summary>
        public Table Table;
        /// <summary>
        /// La contenido de la tabla de AutoCAD
        /// </summary>
        protected ITableroFunctionability Content;
        /// <summary>
        /// Define el tamaño de la tabla
        /// </summary>
        public abstract void SetTableSize();
        /// <summary>
        /// Inicializa la instancia
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// Inicializa los titulos y cabeceras de la tabla
        /// </summary>
        public abstract void InitTitlesAndHeaders();
        /// <summary>
        /// Define las cabeceras de la tabla.
        /// Con el formato Header - SubHeader - Ancho de columna 
        /// </summary>
        /// <value>
        /// Los valores de la cabecera.
        /// </value>
        public abstract HeaderOptions[] Headers { get; }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TabalimTable"/>.
        /// </summary>
        /// <param name="insPt">El punto de inserción de la tabla</param>
        /// <param name="content">El contenido de la tabla.</param>
        public TabalimTable(Point3d insPt, ITableroFunctionability content)
        {
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            this.Table = new Table();
            this.Table.TableStyle = db.Tablestyle;

            this.Content = content;
            this.Table.Position = insPt;
            this.SetTableSize();
            for (int r = 0; r < this.Table.Rows.Count; r++)
                this.Table.Rows[r].Height = ROWHEIGHT;
            for (int c = 0; c < this.Table.Columns.Count; c++)
                this.Table.Columns[c].Width = COLUMNWIDTH;
        }
        /// <summary>
        /// Inserta la tabla en el plano
        /// </summary>
        /// <param name="doc">El documento activo</param>
        /// <param name="tr">La transacción activa</param>
        public void Insert(Document doc, Transaction tr)
        {
            BlockTableRecord cSpace = doc.Database.CurrentSpaceId.GetObject(OpenMode.ForWrite) as BlockTableRecord;
            this.Table.DrawEntity(tr, cSpace);
        }
        /// <summary>
        /// Crea los headers
        /// </summary>
        /// <param name="row">La fila inicial para los headers.</param>
        /// <param name="column">La columna inicial de los headers.</param>
        /// <returns>La lista de filas a fucionar</returns>
        public virtual List<CellRange> CreateHeaders(int row, int column)
        {
            List<CellRange> cells = new List<CellRange>();
            CellRange cell;
            String header, subheader;
            double colWidth;
            for (int i = 0; i < Headers.Length; i++)
            {
                if (Headers[i] != null)
                {
                    header = Headers[i].Header;
                    subheader = Headers[i].SubHeader;
                    colWidth = Headers[i].ColumnWidth;
                    cell = this.AddHeader(header, subheader, row, column, i, colWidth);
                    if (subheader == String.Empty)
                        cell = CellRange.Create(this.Table, row, column + i, row + Headers[i].RowSpan, column + i);
                    //else if (subheader != string.Empty && Headers[i].RowSpan > 2)
                    //    cell = CellRange.Create(this.Table, row + 1, column + i, row +Headers[i].RowSpan, column + i);
                    cells.Add(cell);
                }
            }
            return cells;
        }
    }
}
