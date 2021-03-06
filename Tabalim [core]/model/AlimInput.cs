﻿using System;
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
        public string TableName => "alimentador";
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
        public int Id { get; set; }
        /// <summary>
        /// El id del proyecto al que pertenecen los tableros.
        /// </summary>
        public int ProjectId;
        /// <summary>
        /// El inicio de la línea de conexión
        /// </summary>
        public String No;
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
        public double Temperatura;
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
        public String ToName;
        public String ToDesc;
        public int Conductor;
        public String Calibre;
        public AlimInput()
        {

        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AlimInput"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        public AlimInput(SelectionResult[] result)
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
                    runtime.TabalimApp.CurrentProject.Lineas.Remove(this.Id);
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
                this.CreateFieldAsNumber("fact_temperatura", this.Temperatura),
                this.CreateFieldAsNumber("fac_agrupamiento", this.FactorAgrupamiento),
                this.CreateFieldAsNumber("fac_potencia", this.FactorPotencia),
                this.CreateFieldAsNumber("longitud", this.Longitud),
                this.CreateFieldAsNumber("is_cobre", this.IsCobre?1:0),
                this.CreateFieldAsString("dest_name", this.ToName),
                this.CreateFieldAsString("dest_desc", this.ToDesc),
                this.CreateFieldAsNumber("conductor", this.Conductor),
                this.CreateFieldAsString("label", this.No),
                this.CreateFieldAsString("calibre", this.Calibre)
            };
        }
        /// <summary>
        /// Creates the linea.
        /// </summary>
        /// <param name="tabs">The tabs.</param>
        /// <param name="motores">The motores.</param>
        /// <param name="extras">The extras.</param>
        /// <param name="destinations">The destinations.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal Linea CreateLinea(List<Tablero> tabs, List<BigMotor> motores, List<ExtraData> extras, List<DestinationRow> destinations, SQLite_Connector conn )
        {
            Linea linea = new Linea();
            linea.Id = this.Id;
            linea.No = this.No;
            linea.From = this.Start;
            linea.Type = this.End;
            linea.To = this.ToName;
            linea.ToDesc = this.ToDesc;
            ExtraData extraData = null;
            if (this.End.UseExtraData)
                extraData = extras.FirstOrDefault(y => destinations.Where(x => x.TypeId == 2 && x.AlimId == this.Id).Select(x => x.ConnId).Contains(y.Id));
            var tableros = tabs.Where(x => destinations.Where(y => y.TypeId == 1 && y.AlimId == this.Id).Select(y => y.ConnId).Contains(x.Id));
            if(tableros != null)
                tableros.ToList().ForEach(x => x.LoadComponentesAndCircuits(conn));
            linea.Destination = new Destination(this.End, 
                this.FactorDemanda, 
                motores.Where( x => destinations.Where(y => y.TypeId == 0 && y.AlimId == this.Id).Select(y => y.ConnId).Contains(x.Id)),
                tableros, extraData);
            linea.IsCobre = this.IsCobre;
            linea.FactorAgrupamiento = this.FactorAgrupamiento;
            linea.FactorPotencia = this.FactorPotencia;
            linea.Temperatura = (int)this.Temperatura;
            linea.FactorTemperartura = model.Temperatura.GetFactor(linea.Temperatura);
            linea.Longitud = this.Longitud;
            //linea.Conductor = model.Conductor.GetConductor(this.Calibre, linea.CorrienteCorregida, linea.Destination.Hilos, this.Conductor, linea.IsCobre);
            linea.SelectedConductor = this.Conductor;
            linea.SelectedCalibre = this.Calibre;
            //linea.GetNumber();
            return linea;
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
                this.No = result.GetString("label");
                this.ProjectId = (int)result.GetValue<int>("prj_id");
                this.Start = result.GetString("dest_from");
                int endId = (int)result.GetValue<long>("dest_end");
                this.End = DestinationType.Types.FirstOrDefault(x => x.Id == endId);
                this.FactorDemanda = result.GetValue<double>("fact_demanda");
                this.Temperatura = result.GetValue<double>("fact_temperatura");
                this.FactorAgrupamiento = result.GetValue<double>("fac_agrupamiento");
                this.FactorPotencia = result.GetValue<double>("fac_potencia");
                this.Longitud = result.GetValue<double>("longitud");
                this.IsCobre = result.GetValue<bool>("is_cobre");
                this.ToName = result.GetString("dest_name");
                this.ToDesc = result.GetString("dest_desc");
                this.Conductor = result.GetValue<int>("conductor");
                this.Calibre = result.GetString("calibre");
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
                case "dest_name":
                case "dest_desc":
                case "label":
                case "calibre":
                    value = input.CreateFieldAsString(this.TableName, input.Value);
                    break;
                case "fact_demanda":
                case "fact_temperatura":
                case "fac_agrupamiento":
                case "fac_potencia":
                case "longitud":
                case "conductor":
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
                        this.Start = val.Value.ToString();
                        break;
                    case "dest_name":
                        this.ToName = val.Value.ToString();
                        break;
                    case "dest_desc":
                        this.ToDesc = val.Value.ToString();
                        break;
                    case "label":
                        this.ToDesc = val.Value.ToString();
                        break;
                    case "fact_demanda":
                        this.FactorDemanda = (Double)val.Value;
                        break;
                    case "fact_temperatura":
                        this.Temperatura = (int)val.Value;
                        break;
                    case "fac_agrupamiento":
                        this.FactorAgrupamiento = (Double)val.Value;
                        break;
                    case "fac_potencia":
                        this.FactorPotencia = (Double)val.Value;
                        break;
                    case "longitud":
                        this.Longitud = (Double)val.Value;
                        break;
                    case "is_cobre":
                        this.IsCobre = (bool)val.Value;
                        break;
                    case "conductor":
                        this.Conductor = (int)val.Value;
                        break;
                    case "calibre":
                        this.Calibre = val.Value.ToString();
                        break;
                }
        }
        

    }
}
