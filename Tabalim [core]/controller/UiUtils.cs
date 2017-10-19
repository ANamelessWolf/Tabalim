using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Tabalim.Core.model;
using Tabalim.Core.runtime;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Clase auxiliar que contiene funciones que administren el comportamiento
    /// de la UI en general
    /// </summary>
    public static class UiUtils
    {
        /// <summary>
        /// Crea una imagen de tipo Bitmap
        /// </summary>
        /// <param name="file">El archivo de la imagen</param>
        /// <param name="ignoreCache">Verdadero si se ignora la cache</param>
        /// <returns>La imagen bitmaps</returns>
        public static BitmapImage LoadImage(this FileInfo file, Boolean ignoreCache = false)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(file.FullName, UriKind.Absolute);
            if (ignoreCache)
                img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }
        /// <summary>
        /// Muestra un dialogo que realizá una pregunta al usuario
        /// </summary>
        /// <param name="control">El control que solicita la pregunta.</param>
        /// <param name="title">El titulo del mensaje</param>
        /// <param name="msg">El mensaje.</param>
        /// <returns>Verdadero cuando el usuario da clic en Ok</returns>
        public static async Task<Boolean> ShowQuestionDialog(this System.Windows.Controls.UserControl control, String title, String msg)
        {
            var metroWindow = Application.Current.Windows.OfType<Window>()
                             .SingleOrDefault(x => x.IsActive) as MetroWindow;
            MessageDialogResult res = await metroWindow.ShowMessageAsync(title, msg, MessageDialogStyle.AffirmativeAndNegative);
            return res == MessageDialogResult.Affirmative;
        }
        /// <summary>
        /// Ejecuta la acción que realizá la exportación del tablero actual
        /// </summary>
        /// <param name="window">La ventana metro activa.</param>
        public static async void ExporCurrentTablero(this MetroWindow window)
        {
            var controller = await window.ShowProgressAsync("Guardando por favor espere...", "Guardando tablero");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            TabalimApp.CurrentTablero.ExportTableroTr((async (object result) =>
             {
                 Object[] rData = result as Object[];
                 Boolean succed = (Boolean)rData[0];
                 String msg = (string)rData[1];
                 await controller.CloseAsync();
                 await window.ShowMessageAsync(succed ? "Tablero Guardado" : "Error", msg);
             }));
        }
        /// <summary>
        /// Ejecuta la acción que realizá la importación de un tablero al proyecto actual
        /// </summary>
        /// <param name="window">La ventana metro activa.</param>
        public static async void ImportTablero(this MetroWindow window, Project prj)
        {
            var controller = await window.ShowProgressAsync("Abriendo por favor espere...", "Abriendo tablero");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            window.ImportTableroTr(prj, ((async (object result) =>
             {
                 Object[] rData = result as Object[];
                 Boolean succed = (Boolean)rData[0];
                 String msg = (string)rData[1];
                 if (msg.Length > 0)
                 {
                     TabalimApp.CurrentTablero = (Tablero)rData[2];
                     await window.ShowMessageAsync(succed ? "Tablero Cargado" : "Error", msg);
                 }
                 await controller.CloseAsync();
             })));
        }
        /// <summary>
        /// Devuelve la colección de circuitos disponibles.
        /// </summary>
        /// <param name="tablero">El tablero actual.</param>
        /// <param name="cFases">El número de fases del circuito.</param>
        /// <param name="isMotor">En caso de ser <c>true</c> [es un motor].</param>
        /// <returns>La colección de circuitos disponibles</returns>
        public static IEnumerable<Circuito> GetAvailableCircuitos(this Tablero tablero, int cFases, bool isMotor)
        {
            List<Circuito> circuitos = new List<Circuito>();
            IEnumerable<int> fullPolos = tablero.Circuitos.Values.Where(x => isMotor || x.HasMotor || cFases != x.Polos.Length).SelectMany(x => x.Polos);
            List<int> odd = new List<int>(), even = new List<int>();
            int oddCount = cFases, evenCount = cFases;
            foreach (int i in Enumerable.Range(1, tablero.Sistema.Polo).Except(fullPolos))
            {
                if (oddCount == 0)
                {
                    if (odd.Count == cFases)
                        circuitos.Add(Circuito.GetCircuito(cFases, odd.ToArray()));
                    oddCount = cFases;
                    odd = new List<int>();
                }
                if (i % 2 == 1)
                {
                    if (!fullPolos.Contains(i))
                        odd.Add(i);
                    oddCount--;
                }
                else
                {
                    if (!fullPolos.Contains(i))
                        even.Add(i);
                    evenCount--;
                }
                if (evenCount == 0)
                {
                    if (even.Count == cFases)
                        circuitos.Add(Circuito.GetCircuito(cFases, even.ToArray()));
                    evenCount = cFases;
                    even = new List<int>();
                }
            }
            return circuitos;
        }
    }
}
