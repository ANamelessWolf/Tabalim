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
using Tabalim.Core.view;
using static Tabalim.Core.assets.Constants;
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
        public static async Task ShowMessageDialog(this System.Windows.Controls.UserControl control, String title, String msg)
        {
            var metroWindow = Application.Current.Windows.OfType<Window>()
                             .SingleOrDefault(x => x.IsActive) as MetroWindow;
            MessageDialogResult res = await metroWindow.ShowMessageAsync(title, msg);
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
        /// Muestra un dialogo que realizá una pregunta al usuario
        /// </summary>
        /// <param name="control">El control que solicita la pregunta.</param>
        /// <param name="title">El titulo del mensaje</param>
        /// <param name="msg">El mensaje.</param>
        /// <returns>Verdadero cuando el usuario da clic en Ok</returns>
        public static async Task<Boolean> ShowQuestionDialog(String title, String msg)
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
        /// <param name="updateTask">La tarea a ejecutar al terminar de actualizar</param>
        public static async void ExporCurrentTablero(this MetroWindow window, Action updateTask)
        {
            var controller = await window.ShowProgressAsync("Guardando por favor espere...", "Guardando tablero");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            TabalimApp.CurrentTablero.ExportTableroTr((async (object result) =>
             {
                 Object[] rData = result as Object[];
                 Boolean succed = (Boolean)rData[0];
                 String msg = (string)rData[1];
                 //await controller.CloseAsync();
                 if (msg.Length > 0)
                     await window.ShowMessageAsync(succed ? "Tablero Guardado" : "Error", msg);
                 if (succed)
                     updateTask();
             }));
            await controller.CloseAsync();
        }
        /// <summary>
        /// Exporta el proyecto actual
        /// </summary>
        /// <param name="window">La ventana del proyecto actual.</param>
        /// <param name="updateTask">The update task.</param>
        public static async void ExportCurrentProject(this MetroWindow window, Action updateTask, Boolean saveAs = false)
        {
            var controller = await window.ShowProgressAsync("Guardando por favor espere...", "Guardando proyecto de Alimentadores");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            TabalimApp.CurrentProject.ExportProjectTr((async (object result) =>
            {
                Object[] rData = result as Object[];
                Boolean succed = (Boolean)rData[0];
                String msg = (string)rData[1];
                //await controller.CloseAsync();
                if (msg.Length > 0)
                    await window.ShowMessageAsync(succed ? "Proyecto alimentador guardado" : "Error", msg);
                if (succed && updateTask != null)
                    updateTask();
            }), saveAs);
            await controller.CloseAsync();
        }

        /// <summary>
        /// Ejecuta la acción que realizá el guardado como del tablero actual
        /// </summary>
        /// <param name="window">La ventana metro activa.</param>
        public static async void SaveCurrentTableroAs(this MetroWindow window, string tabName, string tabDesc, Action updateTask)
        {
            var controller = await window.ShowProgressAsync("Guardando por favor espere...", "Guardando tablero");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            //Se realizá la copia del tablero a exportar
            List<Componente> cmps;
            List<Circuito> ctos;
            var copy = TabalimApp.CurrentTablero.Clone(out cmps, out ctos);
            copy.NombreTablero = tabName;
            copy.Description = tabDesc;
            copy.ExportTableroAsTr(cmps, ctos, (async (object result) =>
              {
                  Object[] rData = result as Object[];
                  Boolean succed = (Boolean)rData[0];
                  String msg = (string)rData[1];
                  //await controller.CloseAsync();
                  if (msg.Length > 0)
                      await window.ShowMessageAsync(succed ? "Tablero Guardado" : "Error", msg);
                  if (succed)
                      updateTask();
              }));
            await controller.CloseAsync();
        }
        public static void CloneCurrentTablero(this TabalimApp app, Tablero tablero, Action<Object> tableroAddedTask = null)
        {
            List<Componente> components;
            List<Circuito> circuits;
            var copy = TabalimApp.CurrentTablero.Clone(out components, out circuits);
            SQLiteWrapper tr = new SQLiteWrapper(TabalimApp.AppDBPath)
            {
                TransactionTask = (SQLite_Connector conn, object input) =>
                {
                    Object[] data = input as Object[];
                    Tablero tab = (Tablero)data[0];
                    List<Componente> cmps = (List<Componente>)data[1];
                    List<Circuito> ctos = (List<Circuito>)data[2];
                    tab.NombreTablero = String.Format("Tablero {0:000}", TabalimApp.CurrentProject.Tableros.Count + 1);
                    tab.Description = "Sin descripción";
                    Boolean flag = false;
                    flag = tab.Create(conn, null);
                    if (flag)
                    {
                        tab.Id = (int)conn.SelectValue<long>(TABLE_TABLERO.SelectLastId(tab.PrimaryKey));
                        //Se se insertan los circuitos
                        ctos.ForEach(x => x.GetLastId<Circuito>(conn, tab));
                        //Se insertan los componentes
                        cmps.ForEach(cmp => cmp.GetLastId<Componente>(conn,
                            ctos.FirstOrDefault(cto => cto.ToString() == cmp.CircuitoName)));
                        tab.LoadComponentesAndCircuits(conn);
                        return new Object[] { true, tab };
                    }
                    else
                        return new Object[] { false, tab };
                },
                TaskCompleted = (Object qResult) =>
                {
                    var result = (Object[])qResult;
                    Boolean flag = (Boolean)result[0];
                    Tablero newTab = (Tablero)result[1];
                    if (flag && newTab != null)
                    {
                        app.Tableros.Add(newTab);
                        TabalimApp.CurrentTablero = newTab;
                        if (!TabalimApp.CurrentProject.Tableros.ContainsKey(newTab.Id))
                            TabalimApp.CurrentProject.Tableros.Add(newTab.Id, newTab);
                        tableroAddedTask?.Invoke(tablero);
                    }
                }
            };
            tr.Run(new Object[] { copy, components, circuits });
        }
        /// <summary>
        /// Ejecuta la acción que realizá la importación de un tablero al proyecto actual
        /// </summary>
        /// <param name="window">La ventana metro activa.</param>
        public static async void ImportTablero(this MetroWindow window, Project prj, Action<object> TableroLoaded = null)
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
                     if (!TabalimApp.CurrentProject.Tableros.ContainsKey(TabalimApp.CurrentTablero.Id))
                         TabalimApp.CurrentProject.Tableros.Add(TabalimApp.CurrentTablero.Id, TabalimApp.CurrentTablero);
                     await window.ShowMessageAsync(succed ? "Tablero Cargado" : "Error", msg);
                     TableroLoaded?.Invoke(TabalimApp.CurrentTablero);
                 }
                 await controller.CloseAsync();
             })));
        }
        /// <summary>
        /// Exporta el proyecto actual
        /// </summary>
        /// <param name="window">La ventana del proyecto actual.</param>
        /// <param name="updateTask">The update task.</param>
        public static async void ImportProject(this MetroWindow window, TabalimApp app, Action<Boolean> importEnded)
        {
            var controller = await window.ShowProgressAsync("Abriendo por favor espere...", "Abriendo proyecto de Alimentadores");
            controller.SetCancelable(false);
            controller.SetIndeterminate();
            app.ImportProjectTr(((async (object result) =>
            {
                Object[] rData = result as Object[];
                Boolean succed = (Boolean)rData[0];
                String msg = (string)rData[1];
                await window.ShowMessageAsync(succed ? "Tablero Cargado" : "Error", msg);
                await controller.CloseAsync();
                if (succed && importEnded != null)
                    importEnded(succed);
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
            IEnumerable<int> motorPolos = tablero.Circuitos.Values.Where(x => isMotor || x.HasMotor).SelectMany(x => x.Polos);
            IEnumerable<int> usedPolos = tablero.Circuitos.Values.Where(x => isMotor || !x.HasMotor).SelectMany(x => x.Polos);
            List<int> odd = new List<int>(), even = new List<int>();
            int oddCount = cFases, evenCount = cFases; Circuito c;
            foreach (int i in Enumerable.Range(1, tablero.Sistema.Polo).Except(motorPolos))
            {
                if (!usedPolos.Contains(i))
                    if (i % 2 == 1)
                        odd.Add(i);
                    else
                        even.Add(i);
                else if (!isMotor && (c = tablero.Circuitos.Values.First(x => x.Polos.Contains(i))).Polos.Count() == cFases && !circuitos.Contains(c))
                    circuitos.Add(c);
                if (odd.Count == cFases)
                {
                    if (odd.Last() - odd.First() == (cFases - 1) * 2)
                    {
                        circuitos.Add(Circuito.GetCircuito(cFases, odd.ToArray()));
                        odd.Clear();
                    }
                    else
                        odd.RemoveAt(0);
                }
                if (even.Count == cFases)
                {
                    if (even.Last() - even.First() == (cFases - 1) * 2)
                    {
                        circuitos.Add(Circuito.GetCircuito(cFases, even.ToArray()));
                        even.Clear();
                    }
                    else
                        even.RemoveAt(0);
                }
            }
            return circuitos.OrderByDescending(x => x.Polos.First() % 2).ThenBy(x => x.Polos.First());
        }
    }
}
