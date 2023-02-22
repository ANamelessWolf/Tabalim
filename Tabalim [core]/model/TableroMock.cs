using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;

namespace Tabalim.Core.model
{
    public class TableroMock : IDatabaseMappable, ISQLiteParser
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Fases { get; set; }
        public Tension Tension { get; set; }
        public double PotenciaAlumbrado { get; set; }
        public double PotenciaContactos { get; set; }
        public double PotenciaMotores { get; set; }
        public double PotenciaTotal => PotenciaAlumbrado + PotenciaContactos + PotenciaMotores;

        public string TableName => "tableros_mock";

        public string PrimaryKey => "tab_id";

        public TableroMock()
        {

        }

        public TableroMock(SelectionResult[] result) :
            this()
        {
            this.Parse(result);
        }

        public bool Create(SQLite_Connector conn, object input)
        {
            return conn.Insert(this);
        }

        public bool Delete(SQLite_Connector conn)
        {
            bool succed = conn.DeletebyColumn(this.TableName, this.PrimaryKey, this.Id);
            if (succed)
            {
                string condition = String.Format(" conn_id = {0} AND conn_type = 1 ", this.Id);
                succed = conn.Delete("destination", condition);
                if (succed)//Actualizar la memoria
                {
                    //Undone: Accion tras eliminacion exitosa
                }
            }
            return succed;
        }

        public InsertField[] GetInsertFields()
        {
            return new InsertField[]
            {
                this.CreateFieldAsString("name", this.Name),
                this.CreateFieldAsNumber("fases", this.Fases),
                this.CreateFieldAsNumber("tension", this.Tension.Value),
                this.CreateFieldAsNumber("p_alum", this.PotenciaAlumbrado),
                this.CreateFieldAsNumber("p_cont", this.PotenciaContactos),
                this.CreateFieldAsNumber("p_mot", this.PotenciaMotores)
            };
        }

        public void Parse(SelectionResult[] result)
        {
            try
            {
                this.Id = (int)result.GetValue<long>("tab_id");
                this.Name = result.GetString("name");
                this.Fases = (int)result.GetValue<long>("fases");
                int tension = (int)result.GetValue<double>("tension");
                this.Tension = new Tension((TensionVal)tension, this.Fases);
                this.PotenciaAlumbrado = result.GetValue<double>("p_alum");
                this.PotenciaContactos = result.GetValue<double>("p_cont");
                this.PotenciaMotores = result.GetValue<double>("p_mot");
            }
            catch(Exception exc)
            {
                throw exc;
            }
        }

        public UpdateField PickUpdateFields(KeyValuePair<string, object> input)
        {
            UpdateField value;
            switch (input.Key)
            {
                case "name":
                    value = input.CreateFieldAsString("name", input.Value);
                    break;
                case "fases":
                    value = input.CreateFieldAsNumber("fases", input.Value);
                    break;
                case "tension":
                    Tension tval = (Tension)input.Value;
                    value = input.CreateFieldAsNumber("tension", tval.Value);
                    break;
                case "p_alum":
                    value = input.CreateFieldAsNumber("p_alum", input.Value);
                    break;
                case "p_cont":
                    value = input.CreateFieldAsNumber("p_cont", input.Value);
                    break;
                case "p_mot":
                    value = input.CreateFieldAsNumber("p_mot", input.Value);
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
        }

        public bool Update(SQLite_Connector conn, KeyValuePair<string, object>[] input)
        {
            return this.UpdateTr(this.CreatePrimaryKeyCondition(), conn, input);
        }

        public void UpdateFields(KeyValuePair<string, object>[] input)
        {
            foreach(var val in input)
                switch (val.Key)
                {
                    case "name":
                        this.Name = (String)val.Value;
                        break;
                    case "fases":
                        this.Fases = (int)val.Value;
                        break;
                    case "tension":
                        this.Tension = (Tension)val.Value;
                        break;
                    case "p_alum":
                        this.PotenciaAlumbrado = (double)val.Value;
                        break;
                    case "p_cont":
                        this.PotenciaContactos = (double)val.Value;
                        break;
                    case "p_mot":
                        this.PotenciaMotores = (double)val.Value;
                        break;
                }
        }
    }
}
