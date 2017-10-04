using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Tabalim.Core.assets
{
    /// <summary>
    /// Las constantes que usa la aplicación
    /// </summary>
    public static class Constants 
    {
        /***********************************/
        /************* Formatos ************/
        /***********************************/
        /// <summary>
        /// El formato que se usa para desplegar el nombre de un sistema bifasico o mofasico
        /// </summary>
        public const String FORMAT_SYS_N_FASE = "{0} Volts, {1} Fases, {2} Hilos, No. de Polos {3}";
        /// <summary>
        /// El formato que se usa para desplegar el nombre de un sistema bifasico o mofasico
        /// </summary>
        public const String FORMAT_SYS_ITEM = "Sistema {0} {1} Volts, {2} Fases, {3} Hilos {4} Hz";
        /***********************************/
        /************* CAPTIONS ************/
        /***********************************/
        public const string CAP_F_3 = "Trifásico";
        public const string CAP_F_2 = "Bifásico";
        public const string CAP_F_1 = "Monofásico";
        /***********************************/
        /************* CARPETAS ************/
        /***********************************/
        public const string IMG_FOLDER = "img";
        public const string DATA_FOLDER = "data";
        public const string COMPONENT_FOLDER = "componentes";
        /***********************************/
        /************* ARCHIVOS ************/
        /***********************************/
        public const string APP_DB_FILE = "tabalim.sqlite";
        /***********************************/
        /************* TABLAS **************/
        /***********************************/
        public const string TABLE_HP_WATTS = "hp_watss";
    }
}
