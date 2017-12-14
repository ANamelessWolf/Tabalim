using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class Destination
    {
        const string PROPERTY_NAME = "I_{0}_{1}";
        DestinationType type;
        IEnumerable<BigMotor> motors;
        IEnumerable<Tablero> cargas;
        ExtraData extraData;
        double demanda;
        public IEnumerable<BigMotor> Motors => motors;
        public IEnumerable<Tablero> Cargas => cargas;
        public ExtraData ExtraData => extraData;
        public Double FactorDemanda { get { return demanda; } set { this.demanda = value; } }
        public Double PotenciaInstalada => GetPotencia();
        public Double PotenciaDemandadaAlumbrado => GetPotenciaDemandada(0);
        public Double PotenciaDemandadaContactos => GetPotenciaDemandada(1);
        public Double PotenciaDemandadaFuerza => GetPotenciaDemandada(2);
        public Double PotenciaDemandada => GetPotenciaDemandada();
        public Double CorrienteNominal => GetCorrienteNominal();
        public Double CorrienteContinua => GetCorrienteContinua();
        public int Fases => GetFases();
        public double Tension => GetTension();
        public Destination(DestinationType type, Double demanda = 0.5, IEnumerable<BigMotor> motors = null, IEnumerable<Tablero> cargas = null, ExtraData extraData = null)
        {
            this.motors = motors;
            this.cargas = cargas;
            this.extraData = extraData;
            this.demanda = demanda;
            this.type = type;
        }
        private double GetPotencia()
        {
            double potencia = 0;
            if (type.OnlyOneCarga != null && cargas != null)
                potencia += cargas.SelectMany(x => x.Componentes.Values).Sum(x => x.Potencia.PotenciaAparente);
            if (type.OnlyOneMotor != null && motors != null)
                potencia += motors.Sum(x => x.Potencia.PotenciaAparente);
            if (type.UseExtraData && extraData != null)
                potencia += extraData.KVar / 1000;
            return potencia;
        }
        private double GetPotenciaDemandada()
        {
            double potencia = 0;
            if (type.OnlyOneCarga != null && cargas != null)
                potencia += cargas.Sum(x => GetPotenciaDemandadaTotal(x));
            if (type.OnlyOneMotor != null && motors != null)
                potencia += motors.Sum(x => x.Potencia.PotenciaAparente);
            if (type.UseExtraData && extraData != null)
                potencia += extraData.KVar / 1000;
            return potencia;
        }
        private double GetPotenciaDemandada(int v)
        {
            switch (v)
            {
                default: return 0;
                case 0:
                    if (type.OnlyOneCarga != null && cargas != null)
                        return cargas.Sum(x => GetPotenciaDemandada<Alumbrado>(x));
                    return 0;
                case 1:
                    if (type.OnlyOneCarga != null && cargas != null)
                        return cargas.Sum(x => GetPotenciaDemandada<Contacto>(x) * demanda);
                    return 0;
                case 2:
                    if (type.OnlyOneCarga != null && cargas != null)
                        return cargas.Sum(x => GetPotenciaDemandada<Motor>(x));
                    else
                        return 0;
            }
        }
        private double GetCorrienteNominal()
        {
            if (type.OnlyOneCarga != null && cargas == null)
                return 0;
            if (type.OnlyOneMotor != null && motors == null)
                return 0;
            if (type.UseExtraData && extraData == null)
                return 0;
            switch (type.Id)
            {
                default: return 0;
                case 0:
                    return CalculateCorrienteCarga(cargas.First());
                case 1:
                    return CalculateCorrienteMotor(motors.First());
                case 2:
                    return CalculateCorrienteMotor(motors.First()) * 1.25 + cargas.Sum(x => CalculateCorrienteCarga(x));
                case 3:
                    var tmp = motors.Select(x => CalculateCorrienteMotor(x)).OrderByDescending(x => x);
                    return tmp.First() * 1.25 + tmp.Skip(1).Sum();
                case 4:
                    tmp = motors.Select(x => CalculateCorrienteMotor(x)).OrderByDescending(x => x);
                    return tmp.First() * 1.25 + tmp.Skip(1).Sum() + cargas.Sum(x => CalculateCorrienteCarga(x));
                case 5:
                case 6:
                    return extraData.KVar / 1000 / (extraData.Tension.Value * extraData.Fases == 3 ? Math.Sqrt(3) : 1);
                case 7:
                    tmp = cargas.Select(x => CalculateCorrienteCarga(x)).OrderByDescending(x => x);
                    return tmp.First() * 1.25 + tmp.Skip(1).Sum();
            }
        }
        private double GetCorrienteContinua()
        {
            switch (type.Id)
            {
                case 0: case 1: case 6:
                    return CorrienteNominal * 1.25;
                case 5:
                    return CorrienteNominal * 1.35;
                case 2:
                    return (CalculateCorrienteMotor(motors.First()) + cargas.Sum(x => CalculateCorrienteCarga(x))) * 1.25;
                case 3:
                    return motors.Sum(x => CalculateCorrienteMotor(x)) * 1.25;
                case 4:
                    return (motors.Sum(x => CalculateCorrienteMotor(x)) + cargas.Sum(x => CalculateCorrienteCarga(x))) * 1.25;
                case 7:
                    return cargas.Sum(x => CalculateCorrienteCarga(x)) * 1.25;
                default:
                    return 0;
            }
        }
        private double CalculateCorrienteMotor(BigMotor motor)
        {
            return (double)runtime.TabalimApp.Motores.First(x => x.HP == motor.Potencia.HP).GetType().GetProperty(String.Format(PROPERTY_NAME, motor.Fases, GetColumnTension(motor.Tension.Value))).GetValue(runtime.TabalimApp.Motores.First(x => x.HP == motor.Potencia.HP));
        }

        private double CalculateCorrienteCarga(Tablero tablero)
        {
            return GetPotenciaDemandadaTotal(tablero) / (tablero.Sistema.Tension.Value * (tablero.Sistema.Fases == 3 ? Math.Sqrt(3) : 1));
        }
        private double GetPotenciaDemandadaTotal(Tablero tablero)
        {
            return GetPotenciaDemandada<Alumbrado>(tablero) + GetPotenciaDemandada<Contacto>(tablero) * demanda + GetPotenciaDemandada<Motor>(tablero);
        }
        private double GetPotenciaDemandada<T>(Tablero tablero)
        {
            return tablero.Componentes.Values.OfType<T>().Cast<Componente>().Sum(x => x.Potencia.PotenciaAparente);
        }
        private double GetTension()
        {
            switch (type.Id)
            {
                case 0:
                    return cargas.First().Sistema.Tension.Value;
                case 1:
                    return motors.First().Tension.Value;
                case 2:
                    return Math.Max(motors.First().Tension.Value, cargas.Max(x => x.Sistema.Tension.Value));
                case 3:
                    return motors.Max(x => x.Tension.Value);
                case 4:
                    return Math.Max(motors.Max(x => x.Tension.Value), cargas.Max(x => x.Sistema.Tension.Value));
                case 6: case 5:
                    return extraData.Tension.Value;
                case 7:
                    return cargas.Max(x => x.Sistema.Tension.Value);
            }
            return 0;
        }
        private string GetColumnTension(int value)
        {
            int[] values = new int[] { 230, 460 };
            return (value < 230 ? values[0] : values[1]).ToString();
        }
        private int GetFases()
        {
            switch (type.Id)
            {
                case 0:
                    return cargas.First().Sistema.Fases;
                case 1:
                    return motors.First().Fases;
                case 2:
                    return Math.Max(motors.First().Fases, cargas.Max(x => x.Sistema.Fases));
                case 3:
                    return motors.Max(x => x.Fases);
                case 4:
                    return Math.Max(motors.Max(x => x.Fases), cargas.Max(x => x.Sistema.Fases));
                case 5: case 6:
                    return extraData.Fases;
                case 7:
                    return cargas.Max(x => x.Sistema.Fases);
            }
            return 3;
        }

    }
}
