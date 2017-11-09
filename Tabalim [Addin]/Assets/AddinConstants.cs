using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Addin.Assets
{
    /// <summary>
    /// Define los encabezados del tablero
    /// </summary>
    public static class AddinConstants
    {
        public const String TABLERO_HEADER= "TABLERO";
        public const String BLOCK_DIR = "blocks";
        /***********************************
         ********** Tamaños de tabla *******
         **********************************/
        public const double COLUMNWIDTH = 10;
        public const double ROWHEIGHT = 3d;
        public const double TEXTHEIGHT = 3d;
        public const double HEADER_TEXTHEIGHT = TEXTHEIGHT * 0.9d;
        public const double SUB_HEADER_TEXTHEIGHT = TEXTHEIGHT * 0.8d;
    }
}
