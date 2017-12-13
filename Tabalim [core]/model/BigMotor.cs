using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    public class BigMotor : IDatabaseMappable, ISQLiteParser
    {
        const string MOTOR_FORMAT = "Motor {0}, {1} fases, {2}V";
        /// <summary>
        /// El id del componente es único en la aplicación.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Establece el nombre de la base de datos
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName => TABLE_MOTOR;
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey => "motor_id";
        /// <summary>
        /// Define o establece el valor númerico de la tensión del motor
        /// </summary>
        /// <value>
        /// Los Tensión
        /// </value>
        public Tension Tension { get; set; }
        /// <summary>
        /// Define o establece el número de fases del motor
        /// </summary>
        /// <value>
        /// El número de fases del motor
        /// </value>
        public int Fases { get; set; }
        /// <summary>
        /// Define o establece el valor númerico de la potencia del motor
        /// </summary>
        /// <value>
        /// La potencía del motor
        /// </value>
        public Potencia Potencia { get; set; }
        /// <summary>
        /// Devuelve el valor de la potencia como string
        /// </summary>
        /// <value>
        /// El valor de la potencia en string.
        /// </value>
        public String PotenciaString { get { if (Potencia != null) return runtime.TabalimApp.Motores.First(x => x.HP == Potencia.HP).ToString(); return String.Empty; } }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BigMotor"/>.
        /// </summary>
        public BigMotor()
        {

        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BigMotor"/>.
        /// </summary>
        /// <param name="potencia">Potencia en watts.</param>
        public BigMotor(SelectionResult[] result)
            : this()
        {
            this.Parse(result);
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
                string condition = String.Format(" conn_id = {0} AND conn_type = 0 ", this.Id);
                succed = conn.Delete("destination", condition);
                if (succed)//Actualizar la memoria
                {
                    //ToDo
                }
            }
            return succed;
        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>
        /// Los campos a insertar
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsNumber("potencia", this.Potencia.HP),
                this.CreateFieldAsNumber("fases", this.Fases),
                this.CreateFieldAsNumber("tension", this.Tension.Value),
            };
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
                double potencia = result.GetValue<double>("potencia");
                int tension = (int)result.GetValue<double>("tension");
                this.Fases = (int)result.GetValue<long>("fases");
                this.Potencia = new Potencia(potencia, true);
                this.Tension = new Tension((TensionVal)tension, this.Fases);
            }
            catch (Exception exc)
            {
                throw exc;
            }
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
                case "potencia":
                    Potencia val = (Potencia)input.Value;
                    value = input.CreateFieldAsNumber(this.TableName, val.HP);
                    break;
                case "fases":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "tension":
                    Tension tval = (Tension)input.Value;
                    value = input.CreateFieldAsNumber(this.TableName, tval.Value);
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
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
        /// Actualiza el modelo en caso que el query fuese actualizado de manera correcta
        /// </summary>
        /// <param name="input">Los datos de entrada que se usarón para actualizar</param>
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                switch (val.Key)
                {
                    case "potencia":
                        this.Potencia = (Potencia)val.Value;
                        break;
                    case "comp_count":
                        this.Tension = (Tension)val.Value;
                        break;
                    case "fases":
                        this.Fases = (int)val.Value;
                        break;
                }
        }
        /// <summary>
        /// Generá una <see cref="System.String" /> que representa la instancia.
        /// </summary>
        /// <returns>
        /// La <see cref="System.String" /> que representa a la instancia.
        /// </returns>
        public override string ToString()
        {
            return String.Format(MOTOR_FORMAT, PotenciaString, Fases, Tension);
        }
    }
}
