using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Model;
using static Tabalim.Addin.Assets.AddinConstants;
namespace Tabalim.Addin.Controller
{
    public static class TableUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="header"></param>
        /// <param name="subheader"></param>
        /// <param name="startColumn"></param>
        /// <param name="columnOffset"></param>
        /// <param name="colWidth"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public static CellRange AddHeader(this TabalimTable table, string header, string subheader,
            int row, int startColumn, int columnOffset, double colWidth = COLUMNWIDTH)
        {
            table.Write(header, row, startColumn + columnOffset, columnWidth: colWidth);
            table.Table.Cells[row, startColumn + columnOffset].TextHeight = HEADER_TEXTHEIGHT;
            table.Write(subheader, row + 1, startColumn + columnOffset);
            table.Table.Cells[row + 1, startColumn + columnOffset].TextHeight = SUB_HEADER_TEXTHEIGHT;
            return CellRange.Create(table.Table, row + 1, startColumn + columnOffset, row + 2, startColumn + columnOffset);
        }
        /// <summary>
        /// Escribe en la celda especificada.
        /// </summary>
        /// <param name="table">La tabla a escribir</param>
        /// <param name="text">El texto</param>
        /// <param name="row">La posición de la fila</param>
        /// <param name="column">La posición de la columna</param>
        /// <param name="textHeight">La altura del texto</param>
        /// <param name="columnWidth">El ancho de la columna</param>
        /// <param name="rowHeight">El alto de la fila</param>
        public static void Write(this TabalimTable table, string text, int row, int column,
            double textHeight = TEXTHEIGHT,
            double columnWidth = Double.NaN,
            double rowHeight = Double.NaN)
        {
            table.Table.Cells[row, column].TextString = text;
            table.Table.Cells[row, column].TextHeight = TEXTHEIGHT;
            table.Table.Cells[row, column].Alignment = CellAlignment.MiddleCenter;
            if (!Double.IsNaN(columnWidth))
                table.Table.Columns[column].Width = columnWidth;
            if (!Double.IsNaN(rowHeight))
                table.Table.Rows[row].Height = rowHeight;
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
        public static void ChangeBorders(this TabalimTable table, int row, int column,
            bool leftIsVisible = true, bool topIsVisible = true,
            bool rightIsVisible = true, bool bottomIsVisible = true)
        {
            var borders = table.Table.Cells[row, column].Borders;
            borders.Left.IsVisible = leftIsVisible;
            borders.Top.IsVisible = topIsVisible;
            borders.Right.IsVisible = rightIsVisible;
            borders.Bottom.IsVisible = bottomIsVisible;
        }
        /// <summary>
        /// Cambia los bordes de una columna
        /// </summary>
        /// <param name="row">El indice de la fila.</param>
        /// <param name="column">El indice de la columna.</param>
        /// <param name="horizontalVisibility">if set to <c>true</c> [left and right is visible].</param>
        /// <param name="verticalVisibility">if set to <c>true</c> [top and bottom is visible].</param>
        public static void ChangeBorders(this TabalimTable table, int row, int column,
            bool horizontalVisibility = true, bool verticalVisibility = true)
        {
            var borders = table.Table.Cells[row, column].Borders;
            borders.Left.IsVisible = horizontalVisibility;
            borders.Top.IsVisible = verticalVisibility;
            borders.Right.IsVisible = horizontalVisibility;
            borders.Bottom.IsVisible = verticalVisibility;
        }
        /// <summary>
        /// Converts a double to a string number format.
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <returns>El texto del número en el formato válido</returns>
        public static string ToNumberFormat(this double value)
        {
            return value.ToString("0.##;;#.##");
        }
        /// <summary>
        /// Converts a double to a string exponencial number format.
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <returns>El texto del número en el formato válido</returns>
        public static string ToExpFormat(this double value)
        {
            return value.ToString("0.00000E0");
        }
        /// <summary>
        /// Converts a integer to a string number format.
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <returns>El texto del número en el formato válido</returns>
        public static string ToNumberFormat(this int value)
        {
            return value.ToString();
        }

    }
}
