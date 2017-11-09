using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;
using Tabalim.Addin.Controller;
using static Tabalim.Addin.Assets.AddinConstants;
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
        public override Tuple<string, string, double>[] Headers
        {
            get
            {
                return new Tuple<String, String, Double>[]
                {
                    new Tuple<string, string, double>( "Potencia\nInst.", String.Empty, COLUMNWIDTH * 2.4d),
                    new Tuple<string, string, double>( "Potencia\nInst.", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Factor\nde\nDemanda", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Potencia\nDemandada\nAlumbrados", String.Empty, COLUMNWIDTH * 2.4d),
                    new Tuple<string, string, double>( "Potencia\nDemandada\nContactos", String.Empty, COLUMNWIDTH * 2.4d),
                    new Tuple<string, string, double>( "Potencia\nDemandada\nFuerza", String.Empty, COLUMNWIDTH * 2.4d),
                    new Tuple<string, string, double>( "Potencia\nDeman.", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Potencia\nDeman.", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Factor\nde\nPotencia", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Voltaje\nNominal", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Corriente\nNominal", String.Empty, COLUMNWIDTH * 2d),
                    null,//Factores //TEMP
                    null,//AGR
                    new Tuple<string, string, double>( "Corriente\nCorregida", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Aliment.\ny\nCanaliz.", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Longitud", String.Empty, COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Imped.", "Ohms/m", COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Resist.", "Ohms/m", COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "React.", "Ohms/m", COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Caidas de\n Voltaje", "Alim.\ne%", COLUMNWIDTH * 2d),
                    new Tuple<string, string, double>( "Interruptor", "Polos Amp", COLUMNWIDTH * 2.8d)
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
                };
            }
        }
        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void InitTitlesAndHeaders()
        {
            throw new NotImplementedException();
        }

        public override void SetTableSize()
        {
            throw new NotImplementedException();
        }
    }
}
