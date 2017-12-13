﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Linea
    {
        const string NUMBER_FORMAT = "L{0}";
        public String No;
        public String From;
        public DestinationType Type;
        public Destination Destination;
        public Double FactorPotencia;
        public Double FactorTemperartura;
        public Double FactorAgrupamiento;
        public Double CorrienteCorregida => GetCorrienteCorregida();
        public Double CorrienteProteccion => GetCorrienteProteccion();
        public Conductor Conductor;
        public double Longitud;
        public bool IsCobre;
        public int Temperatura;
        AlimValues AlimValues => Conductor != null ? AlimValues.GetValues(Conductor.Calibre) : AlimValues.GetValues();
        public Double Reactancia => AlimValues.Reactancia;
        public Double Resistencia => AlimValues.Resistencia[IsCobre ? 0 : 1];
        public Double Impedancia => AlimValues.Impedancia[IsCobre ? 0 : 1];
        public Double CaidaVoltaje => Destination.CorrienteNominal * Longitud * Impedancia * (Destination.Fases == 3 ? Math.Sqrt(3) : 1) * 100 / Destination.Tension;
        public String Interruptor => model.Interruptor.GetInterruptor(Destination.Fases, CorrienteProteccion).ToString();

        internal void GetNumber()
        {
            int i;
            var lineas = runtime.TabalimApp.CurrentProject.Lineas;
            if (lineas.Count == 0)
                i = 1;
            else if (lineas.Count == lineas.Max(x => x.Key))
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
    }
}
