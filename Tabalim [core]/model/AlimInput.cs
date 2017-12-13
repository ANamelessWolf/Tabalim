using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la entrada necesaria para crear un alimentador
    /// </summary>
    public class AlimInput : IDatabaseMappable, ISQLiteParser
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
        /// El id del proyecto al que pertenecen los tableros.
        /// </summary>
        public int ProjectId;
        /// <summary>
        /// El inicio de la línea de conexión
        /// </summary>
        public String Start;
        /// <summary>
        /// El fin de la línea de conexión
        /// </summary>
        public DestinationType End;
        /// <summary>
        /// El factor de demanda
        /// </summary>
        public double FactorDemanda;
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FactorTemperatura;
        /// <summary>
        /// El factor de agrupamiento
        /// </summary>
        public double FactorAgrupamiento;
        /// <summary>
        /// El factor de potencia
        /// </summary>
        public double FactorPotencia;
        /// <summary>
        /// La longitud del alimentador
        /// </summary>
        public double Longitud;
        /// <summary>
        /// Evalua que el cable sea de cobre
        /// </summary>
        public Boolean IsCobre;
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
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(SQLite_Connector conn)
        {
            Boolean succed = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (succed)
            {
                //Tambien se debe borrar de la tabla destination
                string condition = String.Format(" alim_id = {0} ", this.Id);
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
                this.CreateFieldAsNumber("prj_id", this.ProjectId),
                this.CreateFieldAsString("dest_from", this.Start),
                this.CreateFieldAsString("dest_end",this.End.Id),
                this.CreateFieldAsNumber("fact_demanda", this.FactorDemanda),
                this.CreateFieldAsNumber("fact_temperatura", this.FactorTemperatura),
                this.CreateFieldAsNumber("fact_agrupamiento", this.FactorAgrupamiento),
                this.CreateFieldAsNumber("fact_potencia", this.FactorPotencia),
                this.CreateFieldAsNumber("longitud", this.Longitud),
                this.CreateFieldAsNumber("is_cobre", this.IsCobre?1:0),
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
                this.ProjectId = (int)result.GetValue<long>("prj_id");
                this.Start = result.GetString("dest_from");
                int endId = (int)result.GetValue<long>("dest_end");
                this.End = DestinationType.Types.FirstOrDefault(x => x.Id == endId);
                this.FactorDemanda = result.GetValue<double>("fact_demanda");
                this.FactorTemperatura = result.GetValue<double>("fact_temperatura");
                this.FactorAgrupamiento = result.GetValue<double>("fact_agrupamiento");
                this.FactorPotencia = result.GetValue<double>("fact_potencia");
                this.Longitud = result.GetValue<double>("longitud");
                this.IsCobre = result.GetInteger("is_cobre") == 1;
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
                case "dest_from":
                    value = input.CreateFieldAsString(this.TableName, input.Value);
                    break;
                case "fact_demanda":
                case "fact_temperatura":
                case "fact_agrupamiento":
                case "fact_potencia":
                case "longitud":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "is_cobre":
                    Boolean val = (Boolean)input.Value;
                    value = input.CreateFieldAsNumber(this.TableName, val ? 1 : 0);
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
                    case "dest_from":
                        this.Start = input.ToString();
                        break;
                    case "fact_demanda":
                        this.FactorAgrupamiento = (Double)val.Value;
                        break;
                    case "fact_temperatura":
                        this.FactorTemperatura = (Double)val.Value;
                        break;
                    case "fact_agrupamiento":
                        this.FactorAgrupamiento = (Double)val.Value;
                        break;
                    case "fact_potencia":
                        this.FactorPotencia = (Double)val.Value;
                        break;
                    case "longitud":
                        this.Longitud = (Double)val.Value;
                        break;
                    case "is_cobre":
                        this.IsCobre = ((int)val.Value) == 1;
                        break;
                }
        }


    }
}
