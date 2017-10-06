using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Tabalim.Core.model;
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.controller
{
    /// <summary>
    /// Una clase auxiliar que se encarga de realizar la carga de los
    /// componentes
    /// </summary>
    public static class ComponentsUtils
    {
        /// <summary>
        /// Devuelve la galería de componentes
        /// </summary>
        /// <returns>La lista de componentes</returns>
        public static List<ComponentGalleryItem> GetGallery()
        {
            List<ComponentGalleryItem> items = new List<ComponentGalleryItem>();
            string imgGalleryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ComponentGalleryItem)).Location), IMG_FOLDER, COMPONENT_FOLDER);
            ComponentType[] ct = new ComponentType[] { ComponentType.Motor, ComponentType.Alumbrado, ComponentType.Contacto };
            for (int i = 0; i < ct.Length; i++)
            {
                int[] cp = ct[i].GetComponentList();
                for (int j = 0; j < cp.Length; j++)
                {
                    try
                    {
                        items.Add(new ComponentGalleryItem()
                        {
                            CType = ct[i],
                            Index = j + 1,
                            Src = (j + 1).LoadImage(imgGalleryPath, 64)
                        });
                    }
                    catch (Exception exc)
                    {

                    }
                }
            }
            return items;
        }
        /// <summary>
        /// Devuelve la colección de componentes conectados a un circuito
        /// </summary>
        /// <returns>La lista de componentes</returns>
        public static List<CtoCompItem> GetComponentsWithCircuits()
        {
            List<CtoCompItem> items = new List<CtoCompItem>();
            string imgGalleryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ComponentGalleryItem)).Location), IMG_FOLDER, COMPONENT_FOLDER);
            ComponentType[] ct = new ComponentType[] { ComponentType.Motor, ComponentType.Alumbrado, ComponentType.Contacto };
            String cFormat = "{0} W {1} Potencia: {2}, VA: {3}";
            String ctoFormat = "Cto({0}) L {1} [m]";
            if (TabalimApp.CurrentTablero != null)
                foreach (Circuito cto in TabalimApp.CurrentTablero.Circuitos.Values)
                    foreach (Componente com in cto.Componentes.Values)
                        items.Add(new CtoCompItem()
                        {
                            ComKey = String.Format(cFormat, com.Potencia.WattsAsString, com.CType, com.Potencia.PFormat, com.Potencia.PotenciaAparenteAsString),
                            CompId = com.Id,
                            Icon = com.ImageIndex.LoadImage(imgGalleryPath, 32, true),
                            CtoKey = cto.ToString(),
                            CtoFormat = String.Format(ctoFormat, cto, cto.Longitud),
                            CtoLength = cto.Longitud
                        });
            return items;
        }
        /// <summary>
        /// Carga la imagen del componente.
        /// </summary>
        /// <param name="index">El indice del componente.</param>
        /// <param name="imgGalleryPath">La ruta de la galería.</param>
        /// <param name="size">El tamaño de la imagen.</param>
        /// <param name="ignoreCache">En caso de ser verdadero <c>true</c> ignorar el cache.</param>
        /// <returns>La imagen seleccionada</returns>
        /// <exception cref="FileNotFoundException">Falta el archivo de imagen default</exception>
        public static BitmapImage LoadImage(this int index, string imgGalleryPath, int size, Boolean ignoreCache = false)
        {
            string pth = Path.Combine(imgGalleryPath, String.Format("{0}_{1}x{1}.png", index, size)),
                   noImgPth = Path.Combine(imgGalleryPath, String.Format("no_img_{0}x{0}.png", size));
            FileInfo imgFile;
            if (File.Exists(pth))
                imgFile = new FileInfo(pth);
            else if (File.Exists(noImgPth))
                imgFile = new FileInfo(noImgPth);
            else
                throw new FileNotFoundException(String.Format("Falta los archivos de imagen {0}, {1}", pth, noImgPth));
            return imgFile.LoadImage(ignoreCache);
        }
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static int[] GetComponentList(this ComponentType type)
        {
            int[] alumbrado = new int[] { 1, 2, 4, 6, 7, 9, 11, 19, 20, 21, 22, 23, 25, 26, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 43, 44, 45, 46, 47 },
                 contacto = new int[] { 3, 5, 8, 10, 12, 14, 15, 16, 17, 18, 24, 27, 40, 41, 42 },
                 motor = new int[] { 13 };
            if (type == ComponentType.Alumbrado)
                return alumbrado;
            else if (type == ComponentType.Contacto)
                return contacto;
            else if (type == ComponentType.Motor)
                return motor;
            else
                return new int[0];
        }
        /// <summary>
        /// Determina si un indice de componente pertenece a un tipo especifico.
        /// </summary>
        /// <param name="index">El indice del componente.</param>
        /// <param name="type">El tipo de componente.</param>
        /// <returns>
        ///   <c>Verdadero</c> si el componente es del tipo especifico; en otro caso, <c>falso</c>.
        /// </returns>
        public static Boolean IsComponent(this int index, ComponentType type)
        {
            return type.GetComponentList().Contains(index);
        }
    }
}