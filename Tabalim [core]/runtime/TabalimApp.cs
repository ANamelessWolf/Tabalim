using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tabalim.Core.controller;
using Tabalim.Core.model;
using static Tabalim.Core.assets.Constants;
using TabalimCats = Tabalim.Data.Repository.CatalogueRepository;

namespace Tabalim.Core.runtime
{
    /// <summary>
    /// Define el acceso a la aplicación Tabalim
    /// </summary>
    public class TabalimApp
    {
        public Action DataLoaded;
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
        /// Equivalencias Hp a Watts
        /// </summary>
        public static List<HPItem> Motores;
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
            if (File.Exists(AppDBPath))
            {
                SQLiteWrapper tr = new SQLiteWrapper(AppDBPath)
                {
                    TransactionTask = InitApplication,
                    TaskCompleted = AppLoaded
                };
                tr.Run(null);
            }
        }
        /// <summary>
        /// Inicializa la información de la aplicación
        /// </summary>
        /// <returns>La información de la base de datos</returns>
        public Object InitApplication(SQLite_Connector conn, Object input)
        {
            try
            {
                VerifyTable(conn);
                //Carga del proyecto
                CurrentProject = JsonConvert.DeserializeObject<Project>(TabalimCats.EMPTY_PROJECT);
                //Carga de Catalogos
                List<HPItem> items = Catalogos.HP_WATTS;
                //Se carga las referencias de los tableros
                //sin cargar circuitos ni componentes
                var tabs = conn.Select<Tablero>(TABLE_TABLERO.SelectAll(CurrentProject.CreatePrimaryKeyCondition()));
                foreach (Tablero tab in tabs)
                    CurrentProject.Tableros.Add(tab.Id, tab);
                var alims = conn.Select<AlimInput>("alimentador".SelectAll(CurrentProject.CreatePrimaryKeyCondition()));
                var motores = conn.Select<BigMotor>("motores".SelectAll());
                var extras = conn.Select<ExtraData>("extras".SelectAll());
                var tabs_mock = conn.Select<TableroMock>(TABLE_TABLERO_MOCK.SelectAll());
                Linea line;
                foreach (var alimInput in alims)
                {
                    var destinations = conn.Select<DestinationRow>("destination".SelectAll(alimInput.CreatePrimaryKeyCondition()));
                    line = alimInput.CreateLinea(tabs_mock, motores, extras, destinations, conn);
                    CurrentProject.Lineas.Add(line.Id, line);
                }
                //Se cargan las referencias del tablero actual que es el último creado
                CurrentTablero = tabs.LastOrDefault();
                if (CurrentTablero != null)
                    CurrentTablero.LoadComponentesAndCircuits(conn);
                return new Object[] { new Project[]{ CurrentProject }, tabs, items };
            }
            catch (Exception exc)
            {
                return new Object[] { exc };
            }
        }

        private void VerifyTable(SQLite_Connector conn)
        {
            string tablename = "alimentador";
            int correctColumns = 17;
            List<string> colums = conn.SelectTableColumns(tablename);
            if(colums.Count < correctColumns)
            {
                conn.AddColumnsToTable(tablename,
                    new InsertField() { ColumnName = "is_charola", DataType = InsertFieldType.BOOLEAN, Value = 0 },
                    new InsertField() { ColumnName = "extra", DataType = InsertFieldType.STRING, Value = "''" }
                    );
            }
        }

        /// <summary>
        /// Se ejecuta una vez que la aplicación a sido cargada
        /// </summary>
        /// <param name="result">Los resultados cargados de la base de datos.</param>
        public void AppLoaded(Object result)
        {
            if (result is Exception)
            {
                this.OpenProjects = new List<Project>();
                this.Tableros = new List<Tablero>();
                Motores = new List<HPItem>();
                System.Windows.MessageBox.Show("Error de carga de aplicación\n" + (result as Exception).Message, "Error de aplicación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                var qResult = (Object[])(result);
                this.OpenProjects = qResult[0] as List<Project>;
                this.Tableros = qResult[1] as List<Tablero>;
                Motores = qResult[2] as List<HPItem>;
                if (DataLoaded != null)
                {
                    //DataLoaded();
                    DataLoaded = null;
                }
            }
        }
    }
}
