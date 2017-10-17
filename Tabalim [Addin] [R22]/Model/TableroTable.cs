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
    /// Esta clase define el contenido del tablero
    /// </summary>
    public class TableroTable
    {
        public const double COLUMNWIDTH = 10;
        public const double ROWHEIGHT = 3d;
        public const double TEXTHEIGHT = 3d;
        /// <summary>
        /// La tabla de AutoCAD
        /// </summary>
        public Table Table;
        /// <summary>
        /// La contenido de la tabla de AutoCAD
        /// </summary>
        public TableroContent Content;
        /// <summary>
        /// La relación exitente de los bloques cargados
        /// </summary>
        /// <value>
        /// La colección de bloques cargados.
        /// </value>
        public Dictionary<string, ObjectId> Blocks;



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
        /// Initializes a new instance of the <see cref="TableroTable"/> class.
        /// </summary>
        /// <param name="insPt">El punto de inserción de la tabla</param>
        /// <param name="content">El contenido de la tabla.</param>
        public TableroTable(Point3d insPt, TableroContent content)
        {
            this.Table = new Table();
            this.Content = content;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            this.Table.TableStyle = db.Tablestyle;
            this.Table.SetSize(18 + this.Content.CtoRows.Length, 18 + this.Content.CmpColumns.Length);
            for (int r = 0; r < this.Table.Rows.Count; r++)
                this.Table.Rows[r].Height = ROWHEIGHT;
            for (int c = 0; c < this.Table.Columns.Count; c++)
                this.Table.Columns[c].Width = COLUMNWIDTH;
            this.Table.Position = insPt;
        }
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.SetTableroTitle(this.Content.Tablero);
            this.InitComponentes(this.Content.CmpColumns);
            this.InitRows(this.Content.CtoRows);
            this.Table.GenerateLayout();
        }
        /// <summary>
        /// Realiza el proceso de inserción de las filas.
        /// </summary>
        /// <param name="circuitos">Los circuitos a insertar</param>
        private void InitRows(CircuitoRow[] circuitos)
        {
            string[] count;
            int startRow = 13;
            int valuesStartColumn = 4 + this.Content.CmpColumns.Length;
            string[] values;
            for (int i = 0; i < circuitos.Length; i++)
            {
                count = circuitos[i].Count.Split(',');
                for (int j = 0; j < count.Length; j++)
                    this.Write(count[j], startRow + i, 5 + j);
                values = circuitos[i].GetValues();
                for (int k = 0; k < values.Length-2; k++)
                    this.Write(values[k], startRow + i, valuesStartColumn + k);
            }
        }

        /// <summary>
        /// Inicializa la carga de los componentes
        /// </summary>
        /// <param name="components">Los componentes a insertar.</param>
        private void InitComponentes(ComponentColumn[] components)
        {
            int rowCount = this.Table.Rows.Count - 1;
            for (int i = 0; i < components.Length; i++)
            {
                this.Write(components[i].W.ToString(), 8, 5 + i, COLUMNWIDTH * 2.5);
                this.CreateBlock(components[i].ImageIndex, 9, 5 + i);
                this.Write(components[i].Potencia, 10, 5 + i);
                this.Write(components[i].VA.ToString(), 11, 5 + i);

                this.Write(components[i].Unidades.ToString(), rowCount - 2, 5 + i);
                this.Write(components[i].WattsTotales.ToString(), rowCount - 1, 5 + i);
                this.Write(components[i].VATotales.ToString(), rowCount, 5 + i);
            }
        }



        /// <summary>
        /// Establece el titulo de la tabla con el nombre del tablero
        /// </summary>
        /// <param name="tablero_name">El nombre de la tabla.</param>
        public void SetTableroTitle(String tablero_name)
        {
            //Las filas que deben mezclarse
            List<CellRange> mergeCells = new CellRange[]
            {
                CellRange.Create(this.Table, 1, 0, 7, 3),
                CellRange.Create(this.Table, 8, 0, this.Table.Rows.Count - 1, 3),
                CellRange.Create(this.Table, 8, 8,8,this.Table.Columns.Count-1)
            }.ToList();

            //Se insertan los títulos de la tabla
            this.Table.Cells[0, 0].TextString = tablero_name;
            this.Write(tablero_name, 0, 0);
            int rowCount = this.Table.Rows.Count - 1;
            this.Write("W", 8, 4, COLUMNWIDTH * 2.5);
            this.Write("Simbolo", 9, 4, rowHeight: COLUMNWIDTH * 2);
            this.Write("Potencia", 10, 4);
            this.Write("V.A.", 11, 4);
            this.Write("C.T.O", 13, 4);
            this.Write("Unidades", rowCount - 2, 4);
            this.Write("Watts", rowCount - 1, 4);
            this.Write("VA", rowCount, 4);

            int valuesStartColumn = 4 + this.Content.CmpColumns.Length;

            this.AddValueColumn(ref mergeCells, "Potencia", "V.A.", valuesStartColumn, 0, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Tensión", "VOLT", valuesStartColumn, 1, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "I", "AMP", valuesStartColumn, 2, COLUMNWIDTH * 1.5);
            this.AddValueColumn(ref mergeCells, "Longitud De\nInstalaciones", "L mts.", valuesStartColumn, 3, COLUMNWIDTH * 3);
            this.AddValueColumn(ref mergeCells, "Factor\nAgrupación", String.Empty, valuesStartColumn, 4, COLUMNWIDTH * 2.5);
            this.AddValueColumn(ref mergeCells, "Temp", "C°", valuesStartColumn, 5, COLUMNWIDTH * 1.5);
            this.AddValueColumn(ref mergeCells, "Calibre", "#", valuesStartColumn, 6, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Sección", "mm2", valuesStartColumn, 7, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Caida de voltaje", "e%", valuesStartColumn, 8, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Potencia", "fase a", valuesStartColumn, 9, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Potencia", "fase b", valuesStartColumn, 10, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Potencia", "fase c", valuesStartColumn, 11, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Protección", "Ideal", valuesStartColumn, 12, COLUMNWIDTH * 2.5);
            this.AddValueColumn(ref mergeCells, "Interruptor", "Polos Amp", valuesStartColumn, 13, COLUMNWIDTH * 2.8);

            //Totales de potencia
            this.Write(this.Content.TotalPTA.ToString("N2"), this.Table.Rows.Count-1, 9);
            this.Write(this.Content.TotalPTB.ToString("N2"), this.Table.Rows.Count - 1, 10);
            this.Write(this.Content.TotalPTC.ToString("N2"), this.Table.Rows.Count - 1, 11);

            mergeCells.ToList().ForEach(cell => this.Table.MergeCells(cell));

        }
        /// <summary>
        /// Agregá una columna de valor.
        /// </summary>
        /// <param name="mergeCells">La columna a mezclar.</param>
        /// <param name="header">La cabecera de la columna.</param>
        /// <param name="subheader">La cabecera sub.</param>
        /// <param name="startColumn">La columna inicial.</param>
        /// <param name="colIndex">El indice de la columna de valor.</param>
        /// <param name="colWidth">El ancho de la columna.</param>
        /// <param name="merge">if set to <c>true</c> [merge].</param>
        private void AddValueColumn(ref List<CellRange> mergeCells, string header, string subheader, int startColumn, int colIndex, double colWidth = Double.NaN, Boolean merge = true)
        {
            if (Double.IsNaN(colWidth))
                colWidth = COLUMNWIDTH;
            this.Write(header, 9, startColumn + colIndex, colWidth);
            this.Table.Cells[9, startColumn + colIndex].TextHeight = TEXTHEIGHT * 0.8d;
            this.Write(subheader, 10, startColumn + colIndex);
            this.Table.Cells[10, startColumn + colIndex].TextHeight = TEXTHEIGHT * 0.9d;
            mergeCells.Add(CellRange.Create(this.Table, 10, startColumn + colIndex, 11, startColumn + colIndex));
        }

        /// <summary>
        /// Escribe en la celda especificada.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="columnWidth">Width of the column.</param>
        /// <param name="rowHeight">Height of the row.</param>
        private void Write(string text, int row, int column, double columnWidth = Double.NaN, double rowHeight = Double.NaN)
        {
            this.Table.Cells[row, column].TextString = text;
            this.Table.Cells[row, column].TextHeight = TEXTHEIGHT;
            this.Table.Cells[row, column].Alignment = CellAlignment.MiddleCenter;
            if (!Double.IsNaN(columnWidth))
                this.Table.Columns[column].Width = columnWidth;
            if (!Double.IsNaN(rowHeight))
                this.Table.Rows[row].Height = rowHeight;
        }
        /// <summary>
        /// Inserta el block en el espacion indicado
        /// </summary>
        /// <param name="imageIndex">Index of the image.</param>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        private void CreateBlock(int imageIndex, int row, int column)
        {
            if (this.Blocks != null && this.Blocks.ContainsKey(imageIndex.ToString()))
                this.Table.Cells[row, column].BlockTableRecordId = this.Blocks[imageIndex.ToString()];
            this.Table.Cells[row, column].Alignment = CellAlignment.MiddleCenter;
            //this.Table.Cells[row, column].Contents.FirstOrDefault().IsAutoScale = false;
            //this.Table.Cells[row, column].Contents.FirstOrDefault().Scale = 10;
        }
    }
}

