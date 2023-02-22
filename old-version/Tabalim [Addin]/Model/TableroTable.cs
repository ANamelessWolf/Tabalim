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
    public class TableroTable : TabalimTable
    {
        /// <summary>
        /// La contenido de la tabla de AutoCAD
        /// </summary>
        public new TableroContent Content
        {
            get { return base.Content as TableroContent; }
        }
        /// <summary>
        /// Define las cabeceras de la tabla.
        /// Con el formato Header - SubHeader - Ancho de columna 
        /// </summary>
        /// <value>
        /// Los valores de la cabecera.
        /// </value>
        public override HeaderOptions[] Headers
        {
            get
            {
                return new HeaderOptions[]
                {
                    new HeaderOptions( "Potencia", "V.A.", COLUMNWIDTH * 2.4d),
                    new HeaderOptions( "Tensión", "VOLT", COLUMNWIDTH * 2d),
                    HeaderOptions.NoSubHeader( "Fases", COLUMNWIDTH * 2d,2),
                    new HeaderOptions( "I", "AMP", COLUMNWIDTH * 1.5d),
                    new HeaderOptions( "Longitud del\nCircuito", "L mts.", COLUMNWIDTH * 3d),
                    HeaderOptions.NoSubHeader( "Factor de\nAgrup.", COLUMNWIDTH * 2.8d,2),
                    new HeaderOptions( "Temp\n{0}C°", "Factor de\nTemp", COLUMNWIDTH * 1.9d),
                    new HeaderOptions( "Calibre", "#", COLUMNWIDTH * 2d),
                    new HeaderOptions( "Sección", "mm2", COLUMNWIDTH * 2d),
                    new HeaderOptions( "Caida de voltaje", "e%", COLUMNWIDTH * 3d),
                    new HeaderOptions( "Potencia", "fase a", COLUMNWIDTH * 2.4d),
                    new HeaderOptions( "Potencia", "fase b", COLUMNWIDTH * 2.4d),
                    new HeaderOptions( "Potencia", "fase c", COLUMNWIDTH * 2.4d),
                    HeaderOptions.NoSubHeader("Protección\nIdeal", COLUMNWIDTH * 2.9d, 2),
                    new HeaderOptions( "Interruptor", "Polos Amp", COLUMNWIDTH * 3.2d)
                };
            }
        }
        /// <summary>
        /// La relación exitente de los bloques cargados
        /// </summary>
        /// <value>
        /// La colección de bloques cargados.
        /// </value>
        public Dictionary<string, ObjectId> Blocks;
        /// <summary>
        /// Initializes a new instance of the <see cref="TableroTable"/> class.
        /// </summary>
        /// <param name="insPt">El punto de inserción de la tabla</param>
        /// <param name="content">El contenido de la tabla.</param>
        public TableroTable(Point3d insPt, TableroContent content) :
            base(insPt, content)
        { }
        /// <summary>
        /// Define el tamaño de la tabla
        /// </summary>
        public override void SetTableSize()
        {
            int numOfRows = 17 + this.Content.CtoRows.Length,
            numOfCols = 20 + this.Content.CmpColumns.Length;
            this.Table.SetSize(numOfRows, numOfCols);
        }
        /// <summary>
        /// Inicializa la instancia
        /// </summary>
        public override void Init()
        {
            this.InitTitlesAndHeaders();
            this.InitComponentes(this.Content.CmpColumns);
            this.InitCircuitos(this.Content.CtoRows);
            this.InitTotales();
            this.InsertTablero(this.Content.ImagenTablero);
            this.Table.GenerateLayout();
        }
        /// <summary>
        /// Regresa las filas mezcladas
        /// </summary>
        /// <param name="row">La fila inicial</param>
        /// <param name="column">La columma inicial</param>
        /// <returns>La lista de celdas a mezclar</returns>
        public override List<CellRange> CreateHeaders(int row, int column)
        {
            this.Write(String.Format("máximo = {0:P2}", this.Content.CaidaMax), 13, 17);
            return base.CreateHeaders(row, column);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void InitTitlesAndHeaders()
        {
            //Se insertan los títulos de la tabla
            this.Table.Cells[0, 0].TextString = this.Content.Tablero;
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
            }.Union(this.CreateHeaders(9, 5 + this.Content.CmpColumns.Length)).ToList();
            this.ChangeBorders(1, 0, true, true, false, false);
            this.ChangeBorders(8, 0, true, true, false, false);
            //Se insertan los titulos de circuitos
            int rowCount = this.Table.Rows.Count - 1;
            this.Write("W", 8, 4, columnWidth: COLUMNWIDTH * 2.5);
            this.Write("Simbolo", 9, 4, rowHeight: COLUMNWIDTH * 2);
            this.Write("Potencia", 10, 4);
            this.Write("V.A.", 11, 4);
            this.Write("C.T.O", 13, 4);
            this.Write("Unidades", rowCount - 2, 4);
            this.Write("Watts", rowCount - 1, 4);
            this.Write("VA", rowCount, 4);
            this.Write(this.Content.TotalWatts.ToNumberFormat(), rowCount - 1, 5 + this.Content.CmpColumns.Length);
            this.Write(this.Content.TotalVA.ToNumberFormat(), rowCount, 5 + this.Content.CmpColumns.Length);
            //Se actualizan las filas mezcladas
            mergeCells.ForEach(cell => this.Table.MergeCells(cell));
        }
        /// <summary>
        /// Inserta la imagen del tablero
        /// </summary>
        /// <param name="imagenTablero">La imagen del tablero a insertar.</param>
        private void InsertTablero(string imagenTablero)
        {
            if (this.Blocks != null && this.Blocks.ContainsKey(imagenTablero))
                this.Table.Cells[8, 0].BlockTableRecordId = this.Blocks[imagenTablero];
            this.Table.Cells[8, 0].Alignment = CellAlignment.TopCenter;
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
            //Totales de potencia
            this.Write(this.Content.TotalPTA.ToNumberFormat(), this.Table.Rows.Count - 1, this.Table.Columns.Count - 5);
            this.Write(this.Content.TotalPTB.ToNumberFormat(), this.Table.Rows.Count - 1, this.Table.Columns.Count - 4);
            this.Write(this.Content.TotalPTC.ToNumberFormat(), this.Table.Rows.Count - 1, this.Table.Columns.Count - 3);
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
        private void InitCircuitos(CircuitoRow[] circuitos)
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
                this.Write(components[i].W.ToString(), 8, 5 + i, columnWidth: COLUMNWIDTH * 2.5);
                this.CreateBlock(components[i].ImageIndex, 9, 5 + i);
                this.Write(components[i].Potencia, 10, 5 + i);
                this.Write(components[i].VA.ToNumberFormat(), 11, 5 + i);

                this.Write(components[i].Unidades.ToNumberFormat(), rowCount - 2, 5 + i);
                this.Write(components[i].WattsTotales.ToNumberFormat(), rowCount - 1, 5 + i);
                this.Write(components[i].VATotales.ToNumberFormat(), rowCount, 5 + i);
            }
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
        }
    }
}

