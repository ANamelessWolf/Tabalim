using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class ExtraData : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// Establece el nombre de tabla
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName => "extras";
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey => "extra_id";
        /// <summary>
        /// Representa el id de la instancia en la
        /// base de datos
        /// </summary>
        /// <value>
        /// El id que usa el elemento en la tabla de la base de datos.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de kilo vars.
        /// </summary>
        /// <value>
        /// Los kilo vars
        /// </value>
        public Double KVar { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de fases.
        /// </summary>
        /// <value>
        /// El número de fases
        /// </value>
        public int Fases { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de hilos.
        /// </summary>
        /// <value>
        /// El número de hilos
        /// </value>
        public int Hilos { get; set; }
        /// <summary>
        /// La tensión del componente
        /// </summary>
        public Tension Tension;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BigMotor"/>.
        /// </summary>
        public ExtraData()
        {

        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BigMotor"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public ExtraData(SelectionResult[] result)
            : this()
        {
            this.Parse(result);
        }
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>(this.PrimaryKey);
                this.KVar = (double)result.GetValue<double>("kvar");
                this.Fases = (int)result.GetValue<long>("fases");
                this.Hilos = (int)result.GetValue<int>("hilos");
                int tension = (int)result.GetValue<double>("tension");
                this.Tension = new Tension((TensionVal)tension, this.Fases);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>
        /// Los campos a insertar
        /// </returns>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsNumber("kvar", this.KVar),
                this.CreateFieldAsNumber("fases", this.Fases),
                this.CreateFieldAsNumber("tension", this.Tension.Value),
                this.CreateFieldAsNumber("hilos", this.Hilos)
            };
        }
        /// <summary>
        /// Obtiene los campos de actualización de un objeto
        /// </summary>
        /// <param name="input">La entrada del campo actualizar</param>
        /// <returns>
        /// El campo actualizar
        /// </returns>
        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            UpdateField value;
            switch (input.Key)
            {
                case "kvar":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
        }
        /// <summary>
        /// Actualiza el modelo en caso que el query fuese actualizado de manera correcta
        /// </summary>
        /// <param name="input">Los datos de entrada que se usarón para actualizar</param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                switch (val.Key)
                {
                    case "kvar":
                        this.KVar = (Double)val.Value;
                        break;
                }
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que recibe la operación</param>
        /// <returns>
        /// Verdadero si realizá la inserción.
        /// </returns>
        public bool Create(SQLite_Connector conn, object input)
        {
            return conn.Insert(this);
        }
        /// <summary>
        /// Actualiza un registro del objeto en la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que recibe la operación</param>
        /// <returns>
        /// Verdadero si realizá la actualización.
        /// </returns>
        public bool Update(SQLite_Connector conn, KeyValuePair<string, object>[] input)
        {
            return this.UpdateTr(this.CreatePrimaryKeyCondition(), conn, input);
        }
        /// <summary>
        /// Borra la instancia de la base de datos
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <returns>
        /// Verdadero si se borra el elemento
        /// </returns>
        public bool Delete(SQLite_Connector conn)
        {
            Boolean succed = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (succed)
            {
                //Tambien se debe borrar de la tabla destination
                string condition = String.Format(" conn_id = {0} AND conn_type = 2 ", this.Id);
                succed = conn.Delete("destination", condition);
                if (succed)//Actualizar la memoria
                {
                    //Undone: Accion tras eliminacion exitosa
                }
            }
            return succed;
        }
    }
}
