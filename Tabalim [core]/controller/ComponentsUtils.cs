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
            ComponentType[] ct = (ComponentType[])Enum.GetValues(typeof(ComponentType));
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
        /// Devuelve la colección de componentes conectados a un circuito
        /// </summary>
        /// <returns>La lista de componentes</returns>
        public static List<CtoCompItem> GetComponentsWithCircuits()
        {
            List<CtoCompItem> items = new List<CtoCompItem>();
            string imgGalleryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ComponentGalleryItem)).Location), IMG_FOLDER, COMPONENT_FOLDER);
            ComponentType[] ct = new ComponentType[] { ComponentType.Motor, ComponentType.Alumbrado, ComponentType.Contacto, ComponentType.MotorWatts, ComponentType.Aire, ComponentType.Subtablero };
            String cFormat = "{0}x {1} W {2} \nPotencia: {3}, VA: {4}";
            String ctoFormat = "Cto({0}) L {1} [m]";
            if (TabalimApp.CurrentTablero != null)
                foreach (Circuito cto in TabalimApp.CurrentTablero.Circuitos.Values)
                    foreach (Componente com in cto.Componentes.Values)
                        items.Add(new CtoCompItem()
                        {
                            ComKey = String.Format(cFormat, com.Count, com.Potencia.WattsAsString, com.CType, com.Potencia.PFormat, com.Potencia.PotenciaAparenteAsString),
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
            //TODO: Agregar indices de componentes desconocidos, copiar imágenes tambien
            int[] alumbrado = new int[] { 1, 2, 4, 6, 7, 9, 11, 19, 20, 21, 22, 23, 25, 26, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 43, 44, 45, 46, 47 },
                 contacto = new int[] { 3, 5, 8, 10, 12, 14, 15, 16, 17, 18, 24, 27, 40, 41, 42 },
                 motor = new int[] { 13 },
                 motorWatts = new int[] { 48 },
                 aire = new int[] { 49 },
                 subtablero = new int[] { 50 };
            switch (type)
            {
                case ComponentType.Alumbrado: return alumbrado;
                case ComponentType.Contacto: return contacto;
                case ComponentType.Motor: return motor;
                case ComponentType.MotorWatts: return motorWatts;
                case ComponentType.Aire: return aire;
                case ComponentType.Subtablero: return subtablero;
                case ComponentType.None: default: return new int[0];
            }
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
        public static Double GetCorriente(Componente componente, Circuito circuito, Tension tension)
        {
            if (!(componente is Motor)) return 0;
            HPItem SelectedMotor = TabalimApp.Motores.First(x => x.HP == componente.Potencia.HP);
            switch (circuito.Polos.Length) 
            {
                case 1: return tension.TensionAlNeutro <= 127 ? SelectedMotor.I_1_127 : SelectedMotor.I_1_230;
                case 2: return tension.Value <= 220 ? SelectedMotor.I_2_230 : SelectedMotor.I_2_460;
                case 3: return tension.Value <= 208 ? SelectedMotor.I_3_208 : tension.Value <= 220 ? SelectedMotor.I_3_230 : SelectedMotor.I_3_460;
            }
            return 1;
        }
    }
}