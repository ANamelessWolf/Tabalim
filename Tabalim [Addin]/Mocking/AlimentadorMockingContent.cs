using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Model;

namespace Tabalim.Addin.Mocking
{
    /// <summary>
    /// Crea una instancia de la informacio que llena la tabla con
    /// motivos de prueba
    /// </summary>
    /// <seealso cref="Tabalim.Addin.Model.AlimentadorContent" />
    public class AlimentadorMockingContent : AlimentadorContent
    {
        static Random ran;
        /// <summary>
        /// Devuelve un valor aleatorio.
        /// </summary>
        /// <value>
        /// El número aleatorio a regresar.
        /// </value>
        static double RandomNum
        {
            get
            {
                if(ran==null)
                ran = new Random((int)DateTime.Now.Ticks);
                return (double)ran.Next(0, 200) * ran.NextDouble();
            }
        }
        static AlimentadorRow NewAlim(int index)
        {
            return new AlimentadorRow()
            {
                No = String.Format("L{0}", index),
                Origin = "TGN",
                To = "CRIBA",
                ToDesc = "Un solo Motor, CRIBA",
                PotInstVA = RandomNum,
                PotInstWatts = RandomNum,
                FacDemanda = RandomNum,
                PotDemAlum = RandomNum,
                PotDemCont = RandomNum,
                PotDemForce = RandomNum,
                PotDemVA = RandomNum,
                PotDemWatts = RandomNum,
                FacPotencia = RandomNum,
                Aliment = "3#12, 1#12",
                CaidaDeVoltaje = RandomNum,
                Canal = "1T-16mm",
                CorrienteCorregida = RandomNum,
                CorrienteNominal = RandomNum,
                FacAgr = 1,
                FacTem = 1,
                Imped = RandomNum,
                Resist = RandomNum,
                Interruptor = "3P-20A/10V",
                Length = RandomNum,
                React = RandomNum,
                VoltajeNominal = RandomNum
            };
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AlimentadorMockingContent"/>.
        /// </summary>
        public AlimentadorMockingContent()
        {
            this.Tablero = "Resumen de Alimentadores";
            this.Description = "Planta de Tratamiento de aguas residuales consorcio zero calmaya edo de México Agosto 2014.";
            this.Lineas = new AlimentadorRow[]
            {
                NewAlim(1),
                NewAlim(2),
                NewAlim(3),
                NewAlim(4),
                NewAlim(5),
                NewAlim(6),
                NewAlim(7),
                NewAlim(8),
                NewAlim(9),
                NewAlim(10),
                                NewAlim(11),
                NewAlim(12),
                NewAlim(13),
                NewAlim(14),
                NewAlim(15),
                NewAlim(16),
                NewAlim(17),
                NewAlim(18),
                NewAlim(19),
                NewAlim(20),
                                NewAlim(21),
                NewAlim(22),
                NewAlim(23),
                NewAlim(24),
                NewAlim(25),
                NewAlim(26),
                NewAlim(27),
                NewAlim(28),
                NewAlim(29),
                NewAlim(30)
            };
        }
    }
}
