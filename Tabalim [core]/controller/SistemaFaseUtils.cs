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
