using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.runtime
{
    /// <summary>
    /// Define el acceso a la aplicación Tabalim
    /// </summary>
    public class TabalimApp
    {
        /// <summary>
        /// Define el acceso a la base de datos de la aplicación
        /// </summary>
        /// <value>
        /// La ruta de la aplicación
        /// </value>
        public static string AppDBPath
        {
            get
            {
                string path = Assembly.GetAssembly(typeof(TabalimApp)).Location;
                path = Path.Combine(Path.GetDirectoryName(path), DATA_FOLDER, APP_DB_FILE);
                return path;
            }
        }
        /// <summary>
        /// Define el acceso al archivo base de tableros
        /// </summary>
        /// <value>
        /// La ruta del archivo base de tableros
        /// </value>
        public static string TableroDBPath
        {
            get
            {
                string path = Assembly.GetAssembly(typeof(TabalimApp)).Location;
                path = Path.Combine(Path.GetDirectoryName(path), DATA_FOLDER, APP_TABLERO_FILE);
                return path;
            }
        }
        /// <summary>
        /// Accede al último tablero seleccionado
        /// </summary>
        public static Tablero CurrentTablero;
        /// <summary>
        /// Accede al último proyecto seleccionado
        /// </summary>
        public static Project CurrentProject;
        /// <summary>
        /// La lista de tableros cargados
        /// </summary>
        public List<Tablero> Tableros;

        /// <summary>
        /// La lista de proyectos cargados
        /// </summary>
        public List<Project> OpenProjects;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TabalimApp"/>.
        /// </summary>
        public TabalimApp()
        {
            this.Tableros = new List<Tablero>();
            SQLiteWrapper tr = new SQLiteWrapper(AppDBPath)
            {
                TransactionTask = InitApplication,
                TaskCompleted = AppLoaded
            };
            tr.Run(null);
        }
        /// <summary>
        /// Inicializa la información de la aplicación
        /// </summary>
        /// <returns>La información de la base de datos</returns>
        private Object InitApplication(SQLite_Connector conn, Object input)
        {
            var prjs = conn.Select<Project>(TABLE_PROYECTOS.SelectAll("\"prj_name\" = 'Sin Proyecto'"));
            CurrentProject = prjs[0];
            //Se carga las referencias de los tableros
            //sin cargar circuitos ni componentes
            var tabs = conn.Select<Tablero>(TABLE_TABLERO.SelectAll(CurrentProject.CreatePrimaryKeyCondition()));
            foreach (Tablero tab in tabs)
                CurrentProject.Tableros.Add(tab.Id, tab);
            //Se cargan las referencias del tablero actual que es el último creado
            CurrentTablero = tabs.LastOrDefault();
            if (CurrentTablero != null)
                CurrentTablero.LoadComponentesAndCircuits(conn);
            return new Object[] { prjs, tabs };
        }
        /// <summary>
        /// Se ejecuta una vez que la aplicación a sido cargada
        /// </summary>
        /// <param name="result">Los resultados cargados de la base de datos.</param>
        private void AppLoaded(Object result)
        {
            var qResult = (Object[])(result);
            this.OpenProjects = qResult[0] as List<Project>;
            this.Tableros = qResult[1] as List<Tablero>;
        }
    }
}
