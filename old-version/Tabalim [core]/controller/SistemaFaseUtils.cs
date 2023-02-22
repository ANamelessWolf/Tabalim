using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tabalim.Core.model;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define el conjunto de herramientas auxiliares en el funcionamiento del objeto 
    /// sistema de fases
    /// </summary>
    public static class SistemaFaseUtils
    {
        /// <summary>
        /// Devuelve los sistemas de fases que utiliza la aplicación.
        /// </summary>
        /// <returns>Los sistemas de fases que utiliza la aplicación.</returns>
        public static IEnumerable<SistemaFases> GetApplicationSystemPhases()
        {
            SistemaFases[] sistemas = new SistemaFases[12];
            TensionVal[] tensiones = new TensionVal[]
            { TensionVal.T_220, TensionVal.T_480, TensionVal.T_208, TensionVal.T_440 };
            SistemaFases sys;
            for (int index = 0, i = 1; index < sistemas.Length; index++, i++)
            {
                if (index < 4)
                    sys = new SistemaTrifasico();
                else if (index < 8)
                    sys = new SistemaBifasico();
                else
                    sys = new SistemaMonofasico();
                sys.SetTension(tensiones[i - 1]);
                sistemas[index] = sys;
                if (i % 4 == 0)
                    i = 1;
            }
            return sistemas;
        }
        /// <summary>
        /// Devuelve el indice del sistema
        /// </summary>
        /// <param name="sistema">El sistema a extraer el indice</param>
        /// <returns>El indice del sistema</returns>
        public static int GetIndexOfSystem(this SistemaFases sistema)
        {
            TensionVal[] tensiones = new TensionVal[]
                { TensionVal.T_220, TensionVal.T_480, TensionVal.T_208, TensionVal.T_440 };
            int tIndex = tensiones.ToList().IndexOf((TensionVal)sistema.Tension.Value);
            if (sistema.Fases == 3)
                tIndex += 0;
            else if (sistema.Fases == 2)
                tIndex += 4;
            else
                tIndex += 7;
            return tIndex;
        }
        /// <summary>
        /// Crea un sistema mediante un indice de selección
        /// </summary>
        /// <param name="sysIndex">El indice del sistema</param>
        /// <returns>El indice del sistema</returns>
        public static SistemaFases GetSystem(this int sysIndex)
        {
            SistemaFases sys;
            TensionVal[] tensiones = new TensionVal[]
                  { TensionVal.T_220, TensionVal.T_480, TensionVal.T_208, TensionVal.T_440 };
            if (sysIndex < 4)
                sys = new SistemaTrifasico();
            else if (sysIndex < 8)
            {
                sys = new SistemaBifasico();
                sysIndex -= 4;
            }
            else
            {
                sys = new SistemaMonofasico();
                sysIndex -= 8;
            }
            sys.SetTension(tensiones[sysIndex]);
            return sys;
        }

        /// <summary>
        /// Realizá la selección del primer elemento de la lista
        /// </summary>
        /// <param name="cbo">El combo box.</param>
        public static void SelectFirst(this ComboBox cbo)
        {
            if (cbo.Items.Count > 0)
                cbo.SelectedIndex = 0;
        }
    }
}
