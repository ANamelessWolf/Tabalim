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
        /// Realizá un clon de esta instancia.
        /// </summary>
        /// <returns>Regresa el nuevo circuito creado</returns>
        public abstract Circuito Clone();
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
        public double PotenciaTotal { get { return Componentes != null ? Componentes.Values.Sum(x => x.Potencia.PotenciaAparente * x.Count) : 0; } }
        /// <summary>
        /// Obtiene la corriente.
        /// </summary>
        /// <value>
        /// La corriente dependiendo del numero de polos.
        /// </value>
        public abstract double Corriente { get; }
        /// <summary>
        /// Gets the corriente continua.
        /// </summary>
        /// <value>
        /// The corriente continua.
        /// </value>
        public double CorrienteContinua { get { return Corriente * 1.25; } }
        /// <summary>
        /// El factor de temperatura
        /// </summary>
        public double FactorTemperatura
        {
            get
            {
                try
                {
                    return Temperatura.GetFactor((int)TabalimApp.CurrentProject.Tableros[TableroId].Sistema.Temperatura);
                }
                catch (Exception)
                {
                    return 1;
                }

            }
        }
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
        public double CorrienteCorregida { get { return CorrienteContinua / (FactorTemperatura * FactorAgrupacion); } }
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
        public double CorrienteProteccion { get { return Corriente * Componentes.First().Value?.FactorProteccion ?? 1; } }
        /// <summary>
        /// Obtiene el calibre utilizando la corriente de protección.
        /// </summary>
        /// <value>
        /// Calibre.
        /// </value>
        public Calibre Calibre;
        /// <summary>
        /// Obtiene la caida de voltaje.
        /// </summary>
        /// <value>
        /// Caida de voltaje.
        /// </value>
        public abstract double CaidaVoltaje { get; }
        public int FasesTablero
        {
            get
            {
                try
                {
                    return TabalimApp.CurrentProject.Tableros[TableroId].Sistema.Fases;
                }
                catch(Exception ex)
                {
                    return 3;
                }
            }
        }
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
                int fasesTablero = FasesTablero;
                if (fasesTablero == 2)
                {
                    if (Polos.Length > 0 && PotenciaTotal > 0)
                        foreach (int polo in Polos)
                            arr[(int)((polo - 1) % fasesTablero)] = PotenciaTotal / Polos.Length;
                }
                else
                    if (Polos.Length > 0 && PotenciaTotal > 0)
                    foreach (int polo in Polos)
                        arr[(int)((polo - 1) / 2 % fasesTablero)] = PotenciaTotal / Polos.Length;
                return arr;
            }
        }
        /// <summary>
        /// Interruptor
        /// </summary>
        public String Interruptor { get { return model.Interruptor.GetInterruptor(Polos.Length, CorrienteProteccion).ToString(); } }
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
        /// Devuelve si el circuito requiere ser revisado
        /// </summary>
        /// <value>
        /// The needs review.
        /// </value>
        public int NeedsReview { get { return CorrienteProteccion < 50 ? 0 : CorrienteProteccion < 55 ? 1 : 2; } }
        public String Tierra
        {
            get
            {
                Calibre original = Calibre.GetTableroCalibres(CorrienteContinua).First();
                if (Calibre == null)
                    return "";
                String tierra = Conductor.CalculateModifiedTierra(original.AWG, Calibre.AWG, CorrienteContinua);
                return tierra;
            }
        }
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
            this.FactorAgrupacion = 1;
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Circuito"/>.
        /// </summary>
        /// <param name="result">El resultado de selección</param>
        public Circuito(SelectionResult[] result) : this()
        {
            this.Parse(result);
            //if (TabalimApp.CurrentTablero != null)
            //    this.Tension = TabalimApp.CurrentTablero.Sistema.Tension;
            if(TabalimApp.CurrentProject.Tableros.ContainsKey(TableroId))
                this.Tension = TabalimApp.CurrentProject.Tableros[TableroId].Sistema.Tension;
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
        public void UpdateCalibre()
        {
            foreach (Calibre calibre in Calibre.GetTableroCalibres(CorrienteCorregida))
            {
                Calibre = calibre;
                if (CaidaVoltaje <= 2)
                    break;
            }
            this.UpdateCircuitTr(null, calibre: Calibre.AWG);
            
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
                this.Longitud = result.GetValue<Double>("longitud");
                this.Calibre = Calibre.GetCalibre(result.GetString("calibre"));
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
                this.CreateFieldAsNumber("fac_agrup", this.FactorAgrupacion),
                this.CreateFieldAsNumber("longitud", this.Longitud),
                this.CreateFieldAsString("calibre", this.Calibre?.AWG ?? String.Empty)
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
                case "longitud":
                    value = input.CreateFieldAsNumber(this.TableName, input.Value);
                    break;
                case "calibre":
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
                    case "fac_agrup":
                        this.FactorAgrupacion = (double)val.Value;
                        break;
                    case "longitud":
                        this.Longitud = (double)val.Value;
                        break;
                    case "calibre":
                        this.Calibre = Calibre.GetCalibre(val.Value.ToString());
                        break;
                }
        }
        #endregion
    }
}
