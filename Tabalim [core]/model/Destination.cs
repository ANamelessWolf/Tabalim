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
        ExtraData kvar;
        double demanda;
        public Double PotenciaInstalada => GetPotencia();
        public Double PotenciaDemandadaAlumbrado => GetPotenciaDemandada(0);
        public Double PotenciaDemandadaContactos => GetPotenciaDemandada(1);
        public Double PotenciaDemandadaFuerza => GetPotenciaDemandada(2);
        public Double PotenciaDemandada => GetPotenciaDemandada();
        public Double CorrienteNominal => GetCorrienteNominal();
        public Double CorrienteContinua => GetCorrienteContinua();
        

        public Destination(DestinationType type, Double demanda = 0.5, IEnumerable<BigMotor> motors = null, IEnumerable<Tablero> cargas = null, ExtraData kvar = null)
        {
            this.motors = motors;
            this.cargas = cargas;
            this.kvar = kvar;
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
            if (type.UseExtraData && kvar != null)
                potencia += kvar.KVar / 1000;
            return potencia;
        }
        private double GetPotenciaDemandada()
        {
            double potencia = 0;
            if (type.OnlyOneCarga != null && cargas != null)
                potencia += cargas.Sum(x => GetPotenciaDemandadaTotal(x));
            if (type.OnlyOneMotor != null && motors != null)
                potencia += motors.Sum(x => x.Potencia.PotenciaAparente);
            if (type.UseExtraData && kvar != null)
                potencia += kvar.KVar / 1000;
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
            if (type.UseExtraData && kvar == null)
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
                    return kvar.KVar / 1000 / (kvar.Tension.Value * kvar.Fases == 3 ? Math.Sqrt(3) : 1);
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
                default:
                    return 0;
            }
        }
        private double CalculateCorrienteMotor(BigMotor motor)
        {
            return (double)typeof(HPItem).GetType().GetProperty(String.Format(PROPERTY_NAME, motor.Fases, GetColumnTension(motor.Tension.Value))).GetValue(runtime.TabalimApp.Motores.First(x => x.HP == motor.Potencia.HP));
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

        private string GetColumnTension(int value)
        {
            int[] values = new int[] { 230, 460 };
            return (value > 230 ? values[0] : values[1]).ToString();
        }
    }
}
