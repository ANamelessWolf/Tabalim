using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Linea
    {
        const string NUMBER_FORMAT = "L{0}";
        public int Id;
        public String No;
        public String From;
        public String To;
        public String ToDesc;
        public DestinationType Type;
        public Destination Destination;
        public Double FactorPotencia;
        public Double FactorTemperartura;
        public Double FactorAgrupamiento;
        public Double CorrienteCorregida => GetCorrienteCorregida();
        public Double CorrienteProteccion => GetCorrienteProteccion();
        public Conductor Conductor;
        public int SelectedConductor;
        public String SelectedCalibre;
        public double Longitud;
        public bool IsCobre;
        public int Temperatura;
        public string Material => IsCobre ? "Cobre" : "Aluminio";
        AlimValues AlimValues => Conductor != null ? AlimValues.GetValues(Conductor.Calibre) : AlimValues.GetValues();
        public Double Reactancia => AlimValues.Reactancia / 1000;
        public Double Resistencia => AlimValues.Resistencia[IsCobre ? 0 : 1] / 1000;
        public Double Impedancia => AlimValues.Impedancia[IsCobre ? 0 : 1] / 1000 / Conductor.NoTubos;
        public Double CaidaVoltaje => Destination.CorrienteNominal * Longitud * Impedancia * (Destination.Fases == 3 ? Math.Sqrt(3) : 1) * 100 / Destination.Tension;
        public String Interruptor => model.Interruptor.GetInterruptor(Destination.Fases, CorrienteProteccion).ToString();

        internal void GetNumber()
        {
            int i;
            var lineas = runtime.TabalimApp.CurrentProject.Lineas;
            if (lineas.Count == 0)
                i = 1;
            else if (lineas.Count == lineas.Max(x => int.Parse(x.Value.No.Substring(1))))
                i = lineas.Count + 1;
            else
            {
                i = Enumerable.Range(1, lineas.Max(x => x.Key)).First(x => !lineas.Keys.Contains(x));
            }
            this.No = String.Format(NUMBER_FORMAT, i.ToString());
        }

        private double GetCorrienteCorregida()
        {
            if (Destination != null)
                if (Type.Id == 1)
                    return Destination.CorrienteNominal / (FactorTemperartura * FactorAgrupamiento);
                else
                    return Destination.CorrienteContinua / (FactorTemperartura * FactorAgrupamiento);
            return 0;
        }
        private double GetCorrienteProteccion()
        {
            switch (Type.Id)
            {
                case 0:
                    return CorrienteCorregida;
                case 1:
                    return Destination.CorrienteNominal * 1.5;
                case 2:
                    return Destination.CorrienteContinua * 1.5;
                case 3:
                    return Destination.CorrienteContinua * 1.25;
                case 4:
                    return Destination.CorrienteContinua * 1.25;
                case 5:
                    return Destination.CorrienteNominal * 1.35;
                case 6:
                    return Destination.CorrienteNominal;
                case 7:
                    return CorrienteCorregida * 1.25;
                default:
                    return 0;

            }
        }
        public Linea()
        {
            No = String.Empty;
        }

        public AlimInput ToAlimInput(Project parent)
        {
            return new AlimInput() {
                End = this.Type,
                FactorAgrupamiento = this.FactorAgrupamiento,
                FactorDemanda = this.Destination.FactorDemanda,
                FactorPotencia = this.FactorPotencia,
                Temperatura = this.Temperatura,
                ToDesc = this.ToDesc,
                ToName = this.To,
                IsCobre = this.IsCobre,
                Longitud = this.Longitud,
                Start = this.From,
                Conductor = this.Conductor.NoTubos,
                ProjectId = parent.Id,
                Id = this.Id,
                No = this.No,
                Calibre = this.Conductor.Calibre
            };
        }

        internal void Update(AlimInput alimInput)
        {
            this.Type = alimInput.End;
            this.FactorAgrupamiento = alimInput.FactorAgrupamiento;
            this.Destination.FactorDemanda = alimInput.FactorDemanda;
            this.FactorPotencia = alimInput.FactorPotencia;
            this.Temperatura = (int)alimInput.Temperatura;
            this.To = alimInput.ToName;
            this.ToDesc = alimInput.ToDesc;
            this.IsCobre = alimInput.IsCobre;
            this.Longitud = alimInput.Longitud;
            this.From = alimInput.Start;
            this.SelectedConductor = alimInput.Conductor;
            this.Conductor = Conductor.GetConductor(alimInput.Calibre, this.CorrienteCorregida, this.Destination.Hilos, alimInput.Conductor, alimInput.IsCobre);
        }
    }
}
