using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;
using Tabalim.Addin.Controller;
using static Tabalim.Addin.Assets.AddinConstants;
using Autodesk.AutoCAD.DatabaseServices;

namespace Tabalim.Addin.Model
{
    public class AlimTable : TabalimTable
    {
        public AlimTable(Point3d insPt, ITableroFunctionability content) : base(insPt, content)
        {
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
                    HeaderOptions.NoSubHeader( "Potencia\nInst.", COLUMNWIDTH * 2.4d,2),
                    HeaderOptions.NoSubHeader( "Potencia\nInst.",  COLUMNWIDTH * 2.4d,2),
                    HeaderOptions.NoSubHeader( "Factor\nde\nDemanda",  COLUMNWIDTH * 2.2d),
                    HeaderOptions.NoSubHeader( "Potencia\nDemandada\nAlumbrados",  COLUMNWIDTH * 2.8d,2),
                    HeaderOptions.NoSubHeader( "Potencia\nDemandada\nContactos", COLUMNWIDTH * 2.7d,2),
                    HeaderOptions.NoSubHeader( "Potencia\nDemandada\nFuerza",  COLUMNWIDTH * 2.7d,2),
                    HeaderOptions.NoSubHeader( "Potencia\nDeman.",  COLUMNWIDTH * 2.2d,2),
                    HeaderOptions.NoSubHeader( "Potencia\nDeman.",  COLUMNWIDTH * 2.2d,2),
                    HeaderOptions.NoSubHeader( "Factor\nde\nPotencia",  COLUMNWIDTH * 2.4d,2),
                    HeaderOptions.NoSubHeader( "Voltaje\nNominal",  COLUMNWIDTH * 2.4d,3),
                    HeaderOptions.NoSubHeader( "Corriente\nNominal",  COLUMNWIDTH * 2.5d,3),
                    null,//Factores //TEMP
                    null,//AGR
                    HeaderOptions.NoSubHeader( "Corriente\nCorregida",  COLUMNWIDTH * 2.5d),
                    HeaderOptions.NoSubHeader( "Aliment.\ny\nCanaliz.",  COLUMNWIDTH * 3.2d),
                    HeaderOptions.NoSubHeader( "Longitud",  COLUMNWIDTH * 2.2d),
                    new HeaderOptions( "Imped.", "Ohms/m", COLUMNWIDTH * 3d,2),
                    new HeaderOptions( "Resist.", "Ohms/m", COLUMNWIDTH * 3d,2),
                    new HeaderOptions( "React.", "Ohms/m", COLUMNWIDTH * 3d,2),
                    HeaderOptions.NoSubHeader( "Caidas de\n Voltaje\nAlim.", COLUMNWIDTH * 2.4d,2),
                    HeaderOptions.NoSubHeader( "Interruptor\nP-AMPS",COLUMNWIDTH * 3.2d)
                };
            }
        }

        public Tuple<string, double>[] SubHeaders
        {
            get
            {
                return new Tuple<string, double>[]
                {
                    new Tuple<string, double>( "No.",  COLUMNWIDTH * 2.4d),
                    new Tuple<string, double>( "De", COLUMNWIDTH * 2d),
                    new Tuple<string, double>( "A",  COLUMNWIDTH * 2d),
                    new Tuple<string, double>( "V.A.", Double.NaN),
                    new Tuple<string, double>( "WATTS",  Double.NaN),
                    null,
                    new Tuple<string, double>( "V.A.", Double.NaN),
                    new Tuple<string, double>( "V.A.", Double.NaN),
                    new Tuple<string, double>( "V.A.", Double.NaN),
                    new Tuple<string, double>( "V.A.", Double.NaN),
                    new Tuple<string, double>( "WATTS",  Double.NaN),
                    new Tuple<string, double>( "COS",  Double.NaN),
                    null,
                    null,
                    new Tuple<string, double>( "TEMP",  COLUMNWIDTH * 2d),
                    new Tuple<string, double>( "AGR.",  COLUMNWIDTH * 2d),
                    new Tuple<string, double>( "AMPS.",  Double.NaN),
                    null,
                    new Tuple<string, double>( "MTS",  Double.NaN),
                    null,
                    new Tuple<string, double>( "(NEC)", Double.NaN),
                    new Tuple<string, double>( "(NEC)", Double.NaN),
                    new Tuple<string, double>( "e%", Double.NaN),
                };
            }
        }
        public override void Init()
        {
            this.InitTitlesAndHeaders();
            this.InitLines();
        }

        private void InitLines()
        {
            int startRow = 9, row;
            AlimentadorContent content = this.Content as AlimentadorContent;
            var lines = content.Lineas;
            String[] values;
            CellRange cellRange;
            row = startRow;
            var simRows = new int[] { 1, 2, 3, 4, 17 };
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].GetValues();
                for (int j = 0; j < values.Length; j++)
                {
                    this.Write(values[j], row, j);
                    if (!simRows.Contains(j))
                    {
                        cellRange = CellRange.Create(this.Table, row, j, row + 1, j);
                        this.Table.MergeCells(cellRange);
                    }
                }
                //Línea descripción
                row++;
                //Descripción del Alimentador
                this.Write(lines[i].ToDesc, row, 1);
                this.Table.MergeCells(CellRange.Create(this.Table, row, 1, row, 4));
                //Canalización
                this.Write(lines[i].Canal, row, 17);
                //Nueva línea
                row++;
            }
        }
        public override void InitTitlesAndHeaders()
        {
            var content = (this.Content as AlimentadorContent);
            this.Write(content.Tablero, 0, 0);
            this.Write(content.Description, 1, 0);
            this.Table.Cells[1, 0].Alignment = CellAlignment.BottomLeft;
            this.Write("Alimentador", 5, 0);
            this.Write("Factores", 5, 14);
            this.Write(content.PageFormat, this.Table.Rows.Count - 1, 0);
            this.Table.Cells[this.Table.Rows.Count - 1, 0].Alignment = CellAlignment.MiddleRight;
            List<CellRange> mergeCells = new CellRange[]
            {
                CellRange.Create(this.Table, 1, 0, 4, 23),
                CellRange.Create(this.Table, 5, 0, 7, 2),
                CellRange.Create(this.Table, 5, 14, 7, 15),
                CellRange.Create(this.Table, this.Table.Rows.Count-1, 0, this.Table.Rows.Count-1, this.Table.Columns.Count-1),
            }.Union(this.CreateHeaders(5, 3)).ToList();
            //Se actualizan las filas mezcladas
            mergeCells.ForEach(cell => this.Table.MergeCells(cell));
            var sHeaders = this.SubHeaders;
            for (int i = 0; i < sHeaders.Length; i++)
            {
                if (sHeaders[i] != null)
                    this.Write(sHeaders[i].Item1, 8, i, columnWidth: sHeaders[i].Item2);
            }
        }

        public override void SetTableSize()
        {
            int numOfRows = (this.Content as AlimentadorContent).Lineas.Length * 2 + 10,
                numOfCols = 24;
            this.Table.SetSize(numOfRows, numOfCols);
        }
    }
}
