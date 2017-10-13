using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using Tabalim.Core.runtime;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Define un circuito
    /// </summary>
    public abstract class Circuito : IDatabaseMappable, ISQLiteParser
    {
        /// <summary>
        /// El id del componente es único en la aplicación.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// El id del tablero al que pertenece el circuito
        /// </summary>
        public int TableroId;
        /// <summary>
        /// Establece el nombre de la base de datos
        /// </summary>
        /// <value>
        /// El nombre de la base de datos
        /// </value>
        public string TableName { get { return TABLE_CIRCUIT; } }
        /// <summary>
        /// Establece el nombre de la columna usada como llave primaria
        /// </summary>
        /// <value>
        /// El nombre de la llave primaria
        /// </value>
        public string PrimaryKey { get { return "cir_id"; } }
        /// <summary>
        /// Los polos alos que se conecta
        /// </summary>
        public int[] Polos;
        /// <summary>
        /// Los componentes que se conectan al circuito
        /// </summary>
        public Dictionary<int, Componente> Componentes;
        /// <summary>
        /// Tension en base al sistema al que pertenece
        /// </summary>
        public Tension Tension;
        /// <summary>
        /// La potencia total 
        /// </summary>
        public double PotenciaTotal { get { return Componentes != null ? Componentes.Values.Sum(x => x.Potencia.Watts * x.Count) : 0; } }
        /// <summary>
        /// Obtiene la corriente.
        /// </summary>
        /// <value>
        /// La corriente dependiendo del numero de polos.
        /// </value>
        public abstract double Corriente { get; }
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FactorTemperatura;
        /// <summary>
        /// El factor de agrupacion
        /// </summary>
        public double FactorAgrupacion;
        /// <summary>
        /// Obtiene la  corriente corregida.
        /// </summary>
        /// <value>
        /// Corriente corregida.
        /// </value>
        public double CorrienteCorregida { get { return Corriente / (FactorTemperatura * FactorAgrupacion); } }
        /// <summary>
        /// Longitud del circuito
        /// </summary>
        public double Longitud;
        /// <summary>
        /// Obtiene la corriente de proteccion.
        /// </summary>
        /// <value>
        /// Corriente de proteccion.
        /// </value>
        public double CorrienteProteccion { get { return Corriente * Componentes.First().Value.FactorProteccion; } }
        /// <summary>
        /// Obtiene el calibre utilizando la corriente de protección.
        /// </summary>
        /// <value>
        /// Calibre.
        /// </value>
        public Calibre Calibre { get { return Calibre.GetCalibre(CorrienteProteccion); } }
        /// <summary>
        /// Obtiene la caida de voltaje.
        /// </summary>
        /// <value>
        /// Caida de voltaje.
        /// </value>
        public abstract double CaidaVoltaje { get; }
        /// <summary>
        /// Obtiene potencia en fases.
        /// </summary>
        /// <value>
        /// Potencia en fases.
        /// </value>
        public double[] PotenciaEnFases
        {
            get
            {
                double[] arr = new double[3];
                if (Polos.Length > 0 && PotenciaTotal > 0)
                    foreach (int polo in Polos)
                        arr[(int)((polo - 1) / 2 % 3)] = PotenciaTotal / Polos.Length;
                return arr;
            }
        }
        /// <summary>
        /// Interruptor
        /// </summary>
        public String Interruptor;
        /// <summary>
        /// Gets a value indicating whether this instance has motor.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has motor; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasMotor { get { return Componentes.Values.Count(x => x is Motor) > 0; } }
        /// <summary>
        /// Devuelve el formato de la longitud en el formato especificado de la aplicación
        /// </summary>
        /// <value>
        /// La longitud del circuito como una cadena
        /// </value>
        public string LongitudAsString { get { return String.Format("{0:N2}", this.Longitud); } }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Join(",", Polos);
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Circuito"/>.
        /// </summary>
        public Circuito()
        {
            this.Componentes = new Dictionary<int, Componente>();
            
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Circuito"/>.
        /// </summary>
        /// <param name="result">El resultado de selección</param>
        public Circuito(SelectionResult[] result) : this()
        {
            this.Parse(result);
            this.Tension = TabalimApp.CurrentTablero.Sistema.Tension;
        }
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="conn">La conexión a SQLite.</param>
        /// <param name="input">La entrada que necesita la conexión.</param>
        public bool Create(SQLite_Connector conn, object input)
        {
            Tablero tablero = input as Tablero;
            this.TableroId = tablero.Id;
            this.Tension = tablero.Sistema.Tension;
            this.FactorTemperatura = Temperatura.GetFactor((int)tablero.Sistema.Temperatura);
            return conn.Insert(this);
        }
        /// <summary>
        /// Crea instancia de circuito  a partir del numero de fases
        /// </summary>
        /// <param name="fases">The fases.</param>
        /// <param name="polos">The polos.</param>
        /// <returns></returns>
        public static Circuito GetCircuito(int fases, int[] polos)
        {
            if (runtime.TabalimApp.CurrentTablero.Circuitos.ContainsKey(String.Join(",", polos)))
                return runtime.TabalimApp.CurrentTablero.Circuitos[String.Join(",", polos)];
            if (fases == 1) return new CircuitoMonofasico() { Polos = polos };
            else if (fases == 2) return new CircuitoBifasico() { Polos = polos };
            else return new CircuitoTrifasico() { Polos = polos };
        }
        /// <summary>
        /// Crea un parser que generá los circuitos apartir de la información en la base de datos
        /// </summary>
        /// <typeparam name="SelectionResult">El resultado de la selección.</typeparam>
        /// <returns>La lista de componentes seleccionados</returns>
        public static List<Circuito> CircuitoParser(List<SelectionResult[]> qResult)
        {
            List<Circuito> result = new List<Circuito>();
            foreach (SelectionResult[] selResult in qResult)
            {
                int fases = selResult.GetString("polos").Split(',').Length;
                if (fases == 1)
                    result.Add(new CircuitoMonofasico(selResult));
                else if (fases == 2)
                    result.Add(new CircuitoBifasico(selResult));
                else
                    result.Add(new CircuitoTrifasico(selResult));
            }
            return result;
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
            Boolean cmpFlag = false, ctoFlag = false;
            int[] ids = this.Componentes.Keys.ToArray();
            foreach (int id in ids)
                cmpFlag = this.Componentes[id].Delete(conn);
            ctoFlag = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (ctoFlag)
                TabalimApp.CurrentTablero.Circuitos.Remove(this.ToString());
            return ctoFlag && cmpFlag;
        }
        #region ISQLiteParser
        /// <summary>
        /// Realiza el parsing de un elemento seleccionado en SQLite
        /// </summary>
        /// <param name="result">El resultado seleccionado.</param>
        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>(this.PrimaryKey);
                this.TableroId = (int)result.GetValue<long>("tab_id");
                this.Polos = result.GetString("polos").ParsePolos();
                this.FactorAgrupacion = result.GetValue<Double>("fac_agrup");
                this.FactorTemperatura = result.GetValue<Double>("fac_temp");
                this.Longitud = result.GetValue<Double>("longitud");
                this.Interruptor = result.GetString("interruptor");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// Obtiene los campos de inserción de un objeto
        /// </summary>
        /// <returns>Obtiene los campos de inserción</returns>
        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsNumber("tab_id", this.TableroId),
                this.CreateFieldAsString("polos", String.Join(",", Polos)),
                this.CreateFieldAsNumber("corriente", this.Corriente),
                this.CreateFieldAsNumber("fac_temp", this.FactorTemperatura),
                this.CreateFieldAsNumber("fac_agrup", this.FactorAgrupacion),
                this.CreateFieldAsNumber("longitud", this.Longitud),
                this.CreateFieldAsString("interruptor", this.Interruptor)
            };
        }
        /// <summary>
        /// Obtiene los campos de actualización de un objeto
        /// </summary>
        /// <param name="input">La entrada del campo actualizar</param>
        /// <returns>
        /// El campo actualizar
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            UpdateField value;
            switch (input.Key)
            {
                case "fac_agrup":
                case "fac_temp":
                case "longitud":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "interruptor":
                    value = input.CreateFieldAsString(this.TableName, input.Value);
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
                    case "fac_temp":
                        this.FactorTemperatura = (double)val.Value;
                        break;
                    case "fac_agrup":
                        this.FactorAgrupacion = (double)val.Value;
                        break;
                    case "longitud":
                        this.Longitud = (double)val.Value;
                        break;
                    case "interruptor":
                        this.Interruptor = (val.Value.ToString());
                        break;
                }
        }
        #endregion
    }
}
