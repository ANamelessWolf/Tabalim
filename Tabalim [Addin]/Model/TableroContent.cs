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
    /// <summary>
    /// Define el contenido del tablero
    /// </summary>
    public class TableroContent :  ITableroFunctionability
    {
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string Tablero;
        /// <summary>
        /// La temperatura del tablero
        /// </summary>
        public double Temperatura;
        /// <summary>
        /// El nombre de la imagen del tablero
        /// </summary>
        public string ImagenTablero;
        /// <summary>
        /// El sistema a utilizar
        /// </summary>
        public string Sistema;
        /// <summary>
        /// El total de VA en alumbrados
        /// </summary>
        public Double AlumbradosVA;
        /// <summary>
        /// El total de VA en contactos
        /// </summary>
        public Double ContactosVA;
        /// <summary>
        /// El total de VA en motores
        /// </summary>
        public Double MotoresVA;
        /// <summary>
        /// El total de VA en reserva
        /// </summary>
        public Double ReservaVA;
        /// <summary>
        /// El total de VA
        /// </summary>
        public Double TotalVA { get { return AlumbradosVA + ContactosVA + MotoresVA + ReservaVA; } }
        /// <summary>
        /// Obtiene la caída máxima
        /// </summary>
        /// <value>
        /// La caída máxima
        /// </value>
        public Double CaidaMax { get { return this.CtoRows.Max(x => x.CaidaVoltaje); } }
        /// <summary>
        /// Devuelve los watts totales
        /// </summary>
        /// <value>
        /// Los watts totales.
        /// </value>
        public Double TotalWatts { get { return this.CmpColumns.Sum(x => x.WattsTotales); } }


        /// <summary>
        /// La desviación máxima
        /// </summary>
        public double DesbMax;
        /// <summary>
        /// Obtiene el total de watts del componente
        /// </summary>
        /// <value>
        /// El total de los watts del componentes.
        /// </value>
        public double TotalCmpWatts { get { return this.CmpColumns.Sum(x => x.WattsTotales); } }
        /// <summary>
        /// Obtiene el total de va del componente
        /// </summary>
        /// <value>
        /// El total de los va del componentes.
        /// </value>
        public double TotalCmpVA { get { return this.CmpColumns.Sum(x => x.VATotales); } }
        /// <summary>
        /// Obtiene el total de la potencia A
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        public double TotalPTA { get { return this.CtoRows.Sum(x => x.PotenciaA); } }
        /// <summary>
        /// Obtiene el total de la potencia B
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        public double TotalPTB { get { return this.CtoRows.Sum(x => x.PotenciaB); } }
        /// <summary>
        /// Obtiene el total de la potencia C
        /// </summary>
        /// <value>
        /// El total de la potencia
        /// </value>
        public double TotalPTC { get { return this.CtoRows.Sum(x => x.PotenciaC); } }
        /// <summary>
        /// Las filas de circuito
        /// </summary>
        public CircuitoRow[] CtoRows;
        /// <summary>
        /// Las columnas del circuito
        /// </summary>
        public ComponentColumn[] CmpColumns;
        /// <summary>
        /// Inicia el proceso de la carga de bloques
        /// </summary>
        public void LoadBlocks()
        {
            String blkDirectory = Assembly.GetAssembly(typeof(Tabalim.Addin.Model.TableroContent)).Location;
            blkDirectory = Path.GetDirectoryName(blkDirectory);
            blkDirectory = Path.Combine(blkDirectory, BLOCK_DIR);
            var blocks = this.CmpColumns.Select(x => new KeyValuePair<String, String>(x.ImageIndex.ToString(), String.Format("{0}.dwg", x.ImageIndex)));
            blocks = blocks.Union(new KeyValuePair<String, String>[] { new KeyValuePair<String, String>(this.ImagenTablero, String.Format("{0}.dwg", this.ImagenTablero)) });
            string file;
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                foreach (var blkName in blocks)
                {
                    file = Path.Combine(blkDirectory, blkName.Value);
                    tr.LoadBlock(doc.Database, file, blkName.Key);
                }
             //   tr.UpdatePath(doc, this.ImagenTablero);
            });
        }
    }
}
