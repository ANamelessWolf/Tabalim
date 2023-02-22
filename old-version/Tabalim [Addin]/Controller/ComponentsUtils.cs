using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Tabalim.Addin.Model;

namespace Tabalim.Addin.Controller
{
    /// <summary>
    /// Una clase auxiliar que se encarga de realizar la carga de los
    /// componentes
    /// </summary>
    public static class ComponentsUtils
    {
        public const string IMG_FOLDER = "img";
        public const string COMPONENT_FOLDER = "componentes";
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
                            Index = cp[j],
                            Src = cp[j].LoadImage(imgGalleryPath, 64)
                        });
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return items;
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
    }
}
