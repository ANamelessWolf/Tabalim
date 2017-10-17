using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Model;

namespace Tabalim.Addin.Mocking
{
    /// <summary>
    /// Crea una instancia de la información que llena la tabla
    /// </summary>
    public class TableroMockingContent : TableroContent
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TableroMockingContent"/>.
        /// </summary>
        public TableroMockingContent()
        {
            this.AlumbradosVA = 500;
            this.CmpColumns = new ComponentColumn[]
            {
                new ComponentColumn()
                {
                    ImageIndex =1,
                    Potencia ="250W",
                    Unidades =2,
                    VA =278,
                    VATotales =556,
                    W =250,
                    WattsTotales =500
                },
                new ComponentColumn()
                {
                    ImageIndex =4,
                    Potencia ="50W",
                    Unidades =1,
                    VA =50,
                    VATotales =56,
                    W =50,
                    WattsTotales =50
                },
                new ComponentColumn()
                {
                    ImageIndex =13,
                    Potencia ="1/6 HP",
                    Unidades =4,
                    VA =621,
                    VATotales =2240,
                    W =560,
                    WattsTotales =2484
                }
            };
            this.ContactosVA = 200;
            this.CtoRows = new CircuitoRow[]
            {
                new CircuitoRow()
                {
                    CaidaVoltaje = 0.07,
                    Calibre = 10,
                    Corriente = 1.25,
                    Cto = "1,3",
                    Count="1,1,1",
                    Fases = 2,
                    FactorAgrupacion =1,
                    Interruptor = "2P-15A",
                    Longitud=10,
                    Potencia=200,
                    PotenciaA=139,
                    PotenciaB=139,
                    PotenciaC =139,
                    Section=5.26,
                    Temperatura=25,
                    Tension =220,
                    Protecion=2.45,
                },
                new CircuitoRow()
                {
                    CaidaVoltaje = 0.07,
                    Calibre = 10,
                    Corriente = 1.25,
                    Cto = "5,7",
                    Count="1,2",
                    Fases = 2,
                    FactorAgrupacion =1,
                    Interruptor = "2P-15A",
                    Longitud=10,
                    Potencia=200,
                    PotenciaA=139,
                    PotenciaB=139,
                    PotenciaC =139,
                    Section=5.26,
                    Temperatura=25,
                    Tension =220,
                    Protecion=2.38,
                },
                new CircuitoRow()
                {
                    CaidaVoltaje = 0.07,
                    Calibre = 10,
                    Corriente = 1.25,
                    Cto = "8,10,12",
                    Count =",,1",
                    Fases = 3,
                    FactorAgrupacion =1,
                    Interruptor = "3P-15A",
                    Longitud=10,
                    Potencia=200,
                    PotenciaA=139,
                    PotenciaB=139,
                    PotenciaC =139,
                    Section=5.26,
                    Temperatura=25,
                    Tension =220,
                    Protecion=2.35,
                },
                new CircuitoRow()
                {
                    CaidaVoltaje = 0.07,
                    Calibre = 10,
                    Corriente = 1.25,
                    Cto = "17",
                    Count =",,1",
                    Fases = 1,
                    FactorAgrupacion =1,
                    Interruptor = "1P-15A",
                    Longitud=10,
                    Potencia=200,
                    PotenciaA=139,
                    PotenciaB=139,
                    PotenciaC =139,
                    Section=5.26,
                    Temperatura=25,
                    Tension =220,
                     Protecion=2.26,
                },
            };
            this.DesbMax = 10;
            this.ImagenTablero = 1;
            this.MotoresVA = 100;
            this.ReservaVA = 120;
            this.Sistema = "220-127 Volts, 3 fases, 4 hilos No. de polos 24";
            this.Tablero = "Tablero de prueba";
        }
    }
}
