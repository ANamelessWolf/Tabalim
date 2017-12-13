using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class DestinationRow : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// The alim identifier
        /// </summary>
        public int AlimId;
        /// <summary>
        /// The connection identifier
        /// </summary>
        public int ConnId;
        /// <summary>
        /// The type identifier
        /// </summary>
        public int TypeId;
        /// <summary>
        /// Establece el nombre de tabla
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName => "destination";
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey => "alim_id";
        /// <summary>
        /// Representa el id de la instancia en la
        /// base de datos
        /// </summary>
        /// <value>
        /// El id que usa el elemento en la tabla de la base de datos.
        /// </value>
        public int Id { get { return this.AlimId; } set { this.AlimId = value; } }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que recibe la operación</param>
        /// <returns>
        /// Verdadero si realizá la inserción.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
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
            String condition = String.Format("alim_id = {0} AND conn_id = {1}", this.AlimId, this.ConnId);
            return conn.Delete(this.TableName, condition);
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
                this.CreateFieldAsNumber("alim_id", this.AlimId),
                this.CreateFieldAsNumber("conn_id", this.ConnId),
                this.CreateFieldAsNumber("conn_type", TypeId )
            };
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
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.AlimId = (int)result.GetValue<long>(this.PrimaryKey);
                this.ConnId = (int)result.GetValue<long>("conn_id");
                this.TypeId = (int)result.GetValue<long>("conn_type");
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
                case "conn_type":
                case "conn_id":
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
        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach (var val in input)
                switch (val.Key)
                {
                    case "conn_type":
                        this.TypeId = (int)val.Value;
                        break;
                    case "conn_id":
                        this.ConnId = (int)val.Value;
                        break;
                }
        }
    }
}
