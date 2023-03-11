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
        IEnumerable<TableroMock> cargas;
        IEnumerable<BigAire> aires;
        ExtraData extraData;
        double demanda;
        public IEnumerable<BigMotor> Motors => motors;
        public IEnumerable<TableroMock> Cargas => cargas;
        public IEnumerable<BigAire> Aires => aires;
        public ExtraData ExtraData => extraData;
        public Double FactorDemanda { get { return demanda; } set { this.demanda = value; } }
        public Double PotenciaInstalada => GetPotencia();
        public Double PotenciaHP => GetPotenciaHP();
        public Double PotenciaDemandadaAlumbrado => GetPotenciaDemandada(0);
        public Double PotenciaDemandadaContactos => GetPotenciaDemandada(1);
        public Double PotenciaDemandadaFuerza => GetPotenciaDemandada(2);
        public Double PotenciaDemandada => GetPotenciaDemandada();
        public Double CorrienteNominal => GetCorrienteNominal();
        public Double CorrienteContinua => GetCorrienteContinua();
        public int Fases => GetFases();
        public int Hilos => GetHilos();
        public double Tension => GetTension();
        public double CorrienteFusible => GetCorrienteFusible();
        public Destination(DestinationType type, Double demanda = 0.5, IEnumerable<BigMotor> motors = null, IEnumerable<TableroMock> cargas = null, IEnumerable<BigAire> aires = null, ExtraData extraData = null)
        {
            this.motors = motors;
            this.cargas = cargas;
            this.aires = aires;
            this.extraData = extraData;
            this.demanda = demanda;
            this.type = type;
        }
        private double GetPotenciaHP()
        {
            double potencia = 0;
            if (type.OnlyOneMotor != null && motors != null)
                potencia += motors.Sum(x => x.Potencia.HP);
            return potencia;
        }
        private double GetPotencia()
        {
            double potencia = 0;
            if (type.OnlyOneCarga != null && cargas != null)
                //potencia += cargas.SelectMany(x => x.Componentes.Values).Sum(x => x.Potencia.PotenciaAparente * x.Count);
                potencia += cargas.Sum(x => x.PotenciaTotal);
            if (type.OnlyOneMotor != null && motors != null)
                potencia += motors.Sum(x => x.Potencia.PotenciaAparente);
            if (type.UseExtraData && extraData != null)
                potencia += extraData.KVar * 1000;
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
                potencia += extraData.KVar * 1000;
            return potencia;
        }
        private double GetPotenciaDemandada(int v)
        {
            switch (v)
            {
                default: return 0;
                case 0:
                    if (type.OnlyOneCarga != null && cargas != null)
                        //return cargas.Sum(x => GetPotenciaDemandada<Alumbrado>(x) * demanda);
                        return cargas.Sum(x => x.PotenciaAlumbrado * demanda);
                    return 0;
                case 1:
                    if (type.OnlyOneCarga != null && cargas != null)
                        //return cargas.Sum(x => GetPotenciaDemandada<Contacto>(x) * demanda);
                        return cargas.Sum(x => x.PotenciaContactos * demanda);
                    return 0;
                case 2:
                    if (type.OnlyOneCarga != null && cargas != null)
                        //return cargas.Sum(x => GetPotenciaDemandada<Motor>(x));
                        return cargas.Sum(x => x.PotenciaMotores);
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
                case 7:
                case 8:
                case 9:
                case 11:
                    return extraData.KVar * 1000 / (extraData.Tension.Value * (extraData.Fases == 3 ? Math.Sqrt(3) : 1));
                case 10:
                    tmp = cargas.Select(x => CalculateCorrienteCarga(x)).OrderByDescending(x => x);
                    return tmp.First() * 1.25 + tmp.Skip(1).Sum();
                case 12:
                    return aires.First().FLA;
                case 13:
                    tmp = aires.Select(x => x.FLA).OrderByDescending(x => x);
                    return tmp.First() * 1.25 + tmp.Skip(1).Sum();

            }
        }
        private double GetCorrienteContinua()
        {
            switch (type.Id)
            {
                case 0:
                case 1:
                case 6:
                case 7:
                case 8:
                case 9:
                case 12:
                    return CorrienteNominal * 1.25;
                case 5:
                    return CorrienteNominal * 1.35;
                case 2:
                    return ((CalculateCorrienteMotor(motors.First()) + cargas.Sum(x => CalculateCorrienteCarga(x)))) * 1.25;
                case 3:
                    return CorrienteNominal;
                case 4:
                case 11:
                case 13:
                    return CorrienteNominal;
                case 10:
                    return cargas.Sum(x => CalculateCorrienteCarga(x)) * 1.25;
                default:
                    return 0;
            }
        }
        public static double CalculateCorrienteMotor(BigMotor motor)
        {
            return (double)runtime.TabalimApp.Motores.First(x => x.HP == motor.Potencia.HP).GetType().GetProperty(String.Format(PROPERTY_NAME, motor.Fases, GetColumnTension(motor.Tension.Value))).GetValue(runtime.TabalimApp.Motores.First(x => x.HP == motor.Potencia.HP));
        }

        private double CalculateCorrienteCarga(TableroMock tablero)
        {
            return GetPotenciaDemandadaTotal(tablero) / (tablero.Tension.Value * (tablero.Fases == 3 ? Math.Sqrt(3) : 1));
        }
        public static double CalculateCorrienteCarga(TableroMock tablero, double demanda)
        {
            return GetPotenciaDemandadaTotal(tablero, demanda) / (tablero.Tension.Value * (tablero.Fases == 3 ? Math.Sqrt(3) : 1));
        }
        private double GetPotenciaDemandadaTotal(TableroMock tablero)
        {
            //return GetPotenciaDemandada<Alumbrado>(tablero) + GetPotenciaDemandada<Contacto>(tablero) * demanda + GetPotenciaDemandada<Motor>(tablero);
            return tablero.PotenciaAlumbrado + tablero.PotenciaContactos * demanda + tablero.PotenciaMotores;
        }
        private static double GetPotenciaDemandadaTotal(TableroMock tablero, double demanda)
        {
            //return GetPotenciaDemandada<Alumbrado>(tablero) + GetPotenciaDemandada<Contacto>(tablero) * demanda + GetPotenciaDemandada<Motor>(tablero);
            return tablero.PotenciaAlumbrado + tablero.PotenciaContactos * demanda + tablero.PotenciaMotores;
        }
        private double GetPotenciaDemandada<T>(Tablero tablero)
        {
            var componentes = tablero.Componentes;
            return componentes.Values.OfType<T>().Cast<Componente>().Sum(x => x.Potencia.PotenciaAparente * x.Count);
        }
        private double GetTension()
        {
            switch (type.Id)
            {
                case 0:
                    return cargas.First().Tension.Value;
                case 1:
                    return motors.First().Tension.Value;
                case 2:
                    return Math.Max(motors.First().Tension.Value, cargas.Max(x => x.Tension.Value));
                case 3:
                    return motors.Max(x => x.Tension.Value);
                case 4:
                    return Math.Max(motors.Max(x => x.Tension.Value), cargas.Max(x => x.Tension.Value));
                case 5: case 6: case 7: case 8: case 9: case 11:
                    return extraData.Tension.Value;
                case 10:
                    return cargas.Max(x => x.Tension.Value);
                case 12:
                case 13:
                    return aires.First().Tension.Value;
                
            }
            return 0;
        }
        private static string GetColumnTension(int value)
        {
            int[] values = new int[] { 230, 460 };
            return (value < 230 ? values[0] : values[1]).ToString();
        }
        private int GetFases()
        {
            switch (type.Id)
            {
                case 0:
                    return cargas.First().Fases;
                case 1:
                    return motors.First().Fases;
                case 2:
                    return Math.Max(motors.First().Fases, cargas.Max(x => x.Fases));
                case 3:
                    return motors.Max(x => x.Fases);
                case 4:
                    return Math.Max(motors.Max(x => x.Fases), cargas.Max(x => x.Fases));
                case 5: case 6: case 7: case 8: case 9: case 11:
                    return extraData.Fases;
                case 10:
                    return cargas.Max(x => x.Fases);
                case 12:
                case 13:
                    return aires.First().Fases;
            }
            return 3;
        }
        private int GetHilos()
        {
            switch (type.Id)
            {
                case 0:
                    return cargas.First().Fases + 1;
                case 1:
                    return motors.First().Hilos;
                case 2:
                    return Math.Max(motors.First().Hilos, cargas.Max(x => x.Fases) + 1);
                case 3:
                    return motors.Max(x => x.Hilos);
                case 4:
                    return Math.Max(motors.Max(x => x.Hilos), cargas.Max(x => x.Fases) + 1);
                case 5: case 6: case 7: case 8: case 9: case 11:
                    return extraData.Hilos;
                case 10:
                    return cargas.Max(x => x.Fases) + 1;
                case 12:
                case 13:
                    return aires.First().Hilos;
            }
            return 3;
        }

        private double GetCorrienteFusible()
        {
            switch (type.Id)
            {
                case 6: case 7: case 8: case 9:
                    return 1.15 * ExtraData.KVar * 1000 / ExtraData.Tension.Value;
                default:
                    return 0;
            }
        }

    }
}
