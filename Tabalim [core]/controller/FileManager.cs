using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Esta clase se encarga de administrar el manejo de archivos
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// Las extensiones permitidas que maneja el dialogo
        /// </summary>
        public readonly string[] AllowedExtensions;
        /// <summary>
        /// La categoría de los archivos
        /// </summary>
        public string Category;
        /// <summary>
        /// El título del dialogo
        /// </summary>
        public string SaveTitle;
        /// <summary>
        /// El directorio en donde comienza el administrador de archivos
        /// </summary>
        public string StartDirectory;
        /// <summary>
        /// Crea un nuevo manejador de archivos
        /// </summary>
        /// <param name="categoryName">El nombre de la categoría de archivo</param>
        /// <param name="saveTitle">El título que usará el dialogo que guarda</param>
        /// <param name="allowedExtensions">Se especifican las extensiones permitidas sin puntos</param>
        public FileManager(string categoryName, string saveTitle, params string[] allowedExtensions)
        {
            string vals = String.Empty;
            foreach (string ext in allowedExtensions)
                vals += ext + ", ";
            vals = vals.Substring(0, vals.Length - 2);
            this.AllowedExtensions = allowedExtensions;
        }
        /// <summary>
        /// Muestra el dialogo que solicita guardar un archivo
        /// </summary>
        /// <param name="input">The saving parameters.</param>
        /// <param name="saveTask">La tarea que se ejecuta una vez seleccionada la ruta</param>
        /// <param name="savePth">Como parámetro de salida la ruta seleccionada</param>
        /// <exception cref="Exception">Una Excepción puede suceder al guardar en un área restringida</exception>
        /// <returns>Verdadero si el archivo fue guardado en la ruta seleccionada</returns>
        public Boolean SaveDialog(Func<String, Object, Boolean> saveTask, Object input, out string savePth)
        {
            try
            {
                Boolean flag;
                SaveFileDialog saveDialog = new SaveFileDialog();
                savePth = null;
                saveDialog.Filter = this.CreateFilter();
                saveDialog.Title = this.SaveTitle;
                saveDialog.FileName = ((input as Object[])[0] as model.Tablero)?.NombreTablero;
                //Se permite seleccionar un directorio de inicio
                if (this.StartDirectory != String.Empty && Directory.Exists(this.StartDirectory))
                    saveDialog.InitialDirectory = this.StartDirectory;
                flag = saveDialog.ShowDialog().Value && saveDialog.FileName != String.Empty;
                if (flag)
                {
                    flag = saveTask(saveDialog.FileName, input);
                    savePth = saveDialog.FileName;
                }
                return flag;
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Selecciona la ruta de un archivo
        /// </summary>
        /// <returns>La ruta del archivo seleccionado</returns>
        public Boolean PickPath(out String selPath)
        {
            Boolean flag;
            OpenFileDialog oDialog = new OpenFileDialog();
            selPath = null;
            oDialog.Filter = this.CreateFilter();
            oDialog.Title = this.SaveTitle;
            oDialog.Multiselect = false;
            //Se permite seleccionar un directorio de inicio
            if (this.StartDirectory != String.Empty && Directory.Exists(this.StartDirectory))
                oDialog.InitialDirectory = this.StartDirectory;
            flag = oDialog.ShowDialog().Value;
            if (flag)
                selPath = oDialog.FileName;
            return flag;
        }
        /// <summary>
        /// Crea el filtro que usará el dialogo
        /// </summary>
        /// <returns>El filtro del dialogo</returns>
        private string CreateFilter()
        {
            StringBuilder filter = new StringBuilder();
            filter.Append(this.Category);
            filter.Append(" (");
            for (int i = 0; i < this.AllowedExtensions.Length; i++)
            {
                filter.Append("*.");
                filter.Append(this.AllowedExtensions[i].ToUpper());
                if (i < this.AllowedExtensions.Length - 1)
                    filter.Append(";");
            }
            filter.Append(")|");
            for (int i = 0; i < this.AllowedExtensions.Length; i++)
            {
                filter.Append("*.");
                filter.Append(this.AllowedExtensions[i].ToUpper());
                if (i < this.AllowedExtensions.Length - 1)
                    filter.Append(";");
            }
            return filter.ToString();
        }
    }
}
