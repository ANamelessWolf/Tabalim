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
            this.Table.SetSize(17 + this.Content.CtoRows.Length, 20 + this.Content.CmpColumns.Length);
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
            this.InitTotales();
            this.Table.GenerateLayout();
        }

        private void InitTotales()
        {
            int valuesStartColumn = 12;
            double[] t = new double[]
            {
                this.Content.AlumbradosVA, this.Content.ContactosVA,
                this.Content.MotoresVA, this.Content.ReservaVA,
                this.Content.TotalVA
            };
            string[] h = new string[]
            {
                "Alumbrados", "Contactos",
                "Motores", "Reserva",
                "Totales"
            };

            for (int i = 0; i < t.Length; i++)
            {
                this.Write(String.Format("{0} = {1:N2} VA", h[i], t[i]), 1 + i, valuesStartColumn);
                this.Table.Cells[1 + i, valuesStartColumn].Alignment = CellAlignment.MiddleLeft;
                this.Table.MergeCells(CellRange.Create(this.Table, 1 + i, valuesStartColumn, 1 + i, valuesStartColumn + 3));
                this.Table.MergeCells(CellRange.Create(this.Table, 1 + i, valuesStartColumn + 4, 1 + i, this.Table.Columns.Count - 1));
                if (i > 0)
                {
                    this.ChangeBorders(1 + i, valuesStartColumn, false, false, false, false);
                    this.ChangeBorders(1 + i, valuesStartColumn + 4, false, false, false, false);
                }
                else
                {
                    this.ChangeBorders(1 + i, valuesStartColumn, false, true, false, false);
                    this.ChangeBorders(1 + i, valuesStartColumn + 4, false, true, false, false);
                }
            }
//            this.Write("Sistema:", 1, 4, COLUMNWIDTH * 2.5);
            this.Write(String.Format("DESB. MAX = {0:P2}", this.Content.DesbMax), 1, valuesStartColumn + 4);
            this.Table.MergeCells(CellRange.Create(this.Table, 1, 4, 2, 4));
            this.Table.Cells[1, 4].Alignment = CellAlignment.MiddleLeft;
            this.ChangeBorders(1, 4, leftIsVisible: true, bottomIsVisible: false);
            this.Write(this.Content.Sistema, 3, 4);
            this.ChangeBorders(3, 4, true, false, false, false);
            this.Table.MergeCells(CellRange.Create(this.Table, 3, 4, 3, 11));
            this.Table.MergeCells(CellRange.Create(this.Table, t.Length + 1, valuesStartColumn, t.Length + 2, this.Table.Columns.Count - 1));
            this.ChangeBorders(t.Length + 1, valuesStartColumn, false, false, false, false);
        }
        /// <summary>
        /// Cambia los bordes de una columna
        /// </summary>
        /// <param name="row">El indice de la fila.</param>
        /// <param name="column">El indice de la columna.</param>
        /// <param name="leftIsVisible">if set to <c>true</c> [left is visible].</param>
        /// <param name="topIsVisible">if set to <c>true</c> [top is visible].</param>
        /// <param name="rightIsVisible">if set to <c>true</c> [right is visible].</param>
        /// <param name="bottomIsVisible">if set to <c>true</c> [bottom is visible].</param>
        private void ChangeBorders(int row, int column, bool leftIsVisible = true, bool topIsVisible = true, bool rightIsVisible = true, bool bottomIsVisible = true)
        {
            var borders = this.Table.Cells[row, column].Borders;
            borders.Left.IsVisible = leftIsVisible;
            borders.Top.IsVisible = topIsVisible;
            borders.Right.IsVisible = rightIsVisible;
            borders.Bottom.IsVisible = bottomIsVisible;
        }

        /// <summary>
        /// Realiza el proceso de inserción de las filas.
        /// </summary>
        /// <param name="circuitos">Los circuitos a insertar</param>
        private void InitRows(CircuitoRow[] circuitos)
        {
            string[] count;
            int startRow = 14;
            int valuesStartColumn = 5 + this.Content.CmpColumns.Length;
            string[] values;
            for (int i = 0; i < circuitos.Length; i++)
            {
                count = circuitos[i].Count.Split(',');
                for (int j = 0; j < count.Length; j++)
                    this.Write(count[j], startRow + i, 5 + j);
                values = circuitos[i].GetValues();
                for (int k = 0; k < values.Length; k++)
                    this.Write(values[k], startRow + i, valuesStartColumn + k);
                //Texto del circuito
                this.Write(circuitos[i].Cto, startRow + i, 4);
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
                this.Write(components[i].VA.ToString("N2"), 11, 5 + i);

                this.Write(components[i].Unidades.ToString("N2"), rowCount - 2, 5 + i);
                this.Write(components[i].WattsTotales.ToString("N2"), rowCount - 1, 5 + i);
                this.Write(components[i].VATotales.ToString("N2"), rowCount, 5 + i);
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
                CellRange.Create(this.Table, 1, 4, 2, 11),
                CellRange.Create(this.Table, 4, 4, 7, 11),
                CellRange.Create(this.Table, 8, this.Content.CmpColumns.Length+5,8,this.Table.Columns.Count-1),
                CellRange.Create(this.Table,12,4,12,this.Table.Columns.Count-1),
                CellRange.Create(this.Table,13,5,13,this.Table.Columns.Count-7),
                CellRange.Create(this.Table,13,this.Table.Columns.Count-5,13,this.Table.Columns.Count-1),
                CellRange.Create(this.Table,this.Table.Rows.Count - 2,6 + this.Content.CmpColumns.Length,this.Table.Rows. Count - 2,this.Table.Columns.Count-1),
                CellRange.Create(this.Table,this.Table.Rows.Count - 1,6 + this.Content.CmpColumns.Length,this.Table.Rows. Count - 1,this.Table.Columns.Count-6),
                             CellRange.Create(this.Table,this.Table.Rows.Count - 1,this.Table.Columns.Count-2,this.Table.Rows.Count - 1,this.Table.Columns.Count-1),
            }.ToList();
            //mergeCells.Where(i => mergeCells.IndexOf(i) < 3).ToList().ForEach(x => this.ChangeBorders(x.TopRow, x.LeftColumn, false, false, false, false));
            this.ChangeBorders(1, 0, true, true, false, false);
            this.ChangeBorders(8, 0, true, true, false, false);
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
            this.Write(this.Content.TotalWatts.ToString("N2"), rowCount - 1, 5 + this.Content.CmpColumns.Length);
            this.Write(this.Content.TotalVA.ToString("N2"), rowCount, 5 + this.Content.CmpColumns.Length);
            int valuesStartColumn = 5 + this.Content.CmpColumns.Length;

            this.AddValueColumn(ref mergeCells, "Potencia", "V.A.", valuesStartColumn, 0, COLUMNWIDTH * 2.4);
            this.AddValueColumn(ref mergeCells, "Tensión", "VOLT", valuesStartColumn, 1, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Fases", "", valuesStartColumn, 2, COLUMNWIDTH * 1.5, false);
            mergeCells.Add(CellRange.Create(this.Table, 9, valuesStartColumn + 2, 11, valuesStartColumn + 2));
            this.AddValueColumn(ref mergeCells, "I", "AMP", valuesStartColumn, 3, COLUMNWIDTH * 1.5);
            this.AddValueColumn(ref mergeCells, "Longitud De\nInstalaciones", "L mts.", valuesStartColumn, 4, COLUMNWIDTH * 3);
            this.AddValueColumn(ref mergeCells, "Factor\nAgrupación", String.Empty, valuesStartColumn, 5, COLUMNWIDTH * 2.5, false);
            mergeCells.Add(CellRange.Create(this.Table, 9, valuesStartColumn + 5, 11, valuesStartColumn + 5));
            this.AddValueColumn(ref mergeCells, "Temp", (int)this.Content.Temperatura + "C°", valuesStartColumn, 6, COLUMNWIDTH * 1.8);
            this.AddValueColumn(ref mergeCells, "Calibre", "#", valuesStartColumn, 7, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Sección", "mm2", valuesStartColumn, 8, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Caida de voltaje", "e%", valuesStartColumn, 9, COLUMNWIDTH * 3);
            this.Write(String.Format("máximo = {0:P2}", this.Content.CaidaMax), 13, 17);
            this.Table.Cells[13, 17].TextHeight = TEXTHEIGHT * 0.6d;
            this.AddValueColumn(ref mergeCells, "Potencia", "fase a", valuesStartColumn, 10, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Potencia", "fase b", valuesStartColumn, 11, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Potencia", "fase c", valuesStartColumn, 12, COLUMNWIDTH * 2);
            this.AddValueColumn(ref mergeCells, "Protección", "Ideal", valuesStartColumn, 13, COLUMNWIDTH * 2.5);
            this.AddValueColumn(ref mergeCells, "Interruptor", "Polos Amp", valuesStartColumn, 14, COLUMNWIDTH * 2.8);

            //Totales de potencia
            this.Write(this.Content.TotalPTA.ToString("N2"), this.Table.Rows.Count - 1, this.Table.Columns.Count - 5);
            this.Write(this.Content.TotalPTB.ToString("N2"), this.Table.Rows.Count - 1, this.Table.Columns.Count - 4);
            this.Write(this.Content.TotalPTC.ToString("N2"), this.Table.Rows.Count - 1, this.Table.Columns.Count - 3);

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

