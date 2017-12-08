using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class AlimValues
    {
        public string Calibre;
        public Double Reactancia;
        public Double[] Resistencia;
        public Double[] Impedancia;
        static IEnumerable<AlimValues> Data = new AlimValues[]
        {
            new AlimValues() { Calibre = "14", Reactancia = 0.239501312335958, Resistencia = new Double[] { 10.1706036745407, 0 }, Impedancia = new Double[] { 9.25853018372703, 0 } },
            new AlimValues() { Calibre = "12", Reactancia = 0.223097112860892, Resistencia = new Double[] { 6.56167979002625, 10.498687664042 }, Impedancia = new Double[] { 6.00393700787402, 9.54724409448819 } },
            new AlimValues() { Calibre = "10", Reactancia = 0.206692913385827, Resistencia = new Double[] { 3.93700787401575, 6.56167979002625 }, Impedancia = new Double[] { 3.63188976377953, 5.99409448818898 } },
            new AlimValues() { Calibre = "8", Reactancia = 0.213254593175853, Resistencia = new Double[] { 2.55905511811024, 4.26509186351706 }, Impedancia = new Double[] { 2.39501312335958, 3.93044619422572 } },
            new AlimValues() { Calibre = "6", Reactancia = 0.20997375328084, Resistencia = new Double[] { 1.60761154855643, 2.65748031496063 }, Impedancia = new Double[] { 1.53871391076115, 2.48359580052493 } },
            new AlimValues() { Calibre = "4", Reactancia = 0.196850393700787, Resistencia = new Double[] { 1.01706036745407, 1.67322834645669 }, Impedancia = new Double[] { 1.000656167979, 2.77230971128609 } },
            new AlimValues() { Calibre = "2", Reactancia = 0.187007874015748, Resistencia = new Double[] { 0.656167979002625, 1.0498687664042 }, Impedancia = new Double[] { 0.67257217847769, 1.02690288713911 } },
            new AlimValues() { Calibre = "1/0", Reactancia = 0.180446194225722, Resistencia = new Double[] { 0.393700787401575, 0.656167979002625 }, Impedancia = new Double[] { 0.433070866141732, 0.669291338582677 } },
            new AlimValues() { Calibre = "2/0", Reactancia = 0.177165354330709, Resistencia = new Double[] { 0.328083989501312, 0.5249343832021 }, Impedancia = new Double[] { 0.374015748031496, 0.551181102362205 } },
            new AlimValues() { Calibre = "3/0", Reactancia = 0.170603674540682, Resistencia = new Double[] { 0.318241469816273, 0.426509186351706 }, Impedancia = new Double[] { 0.360892388451444, 0.459317585301837 } },
            new AlimValues() { Calibre = "4/0", Reactancia = 0.167322834645669, Resistencia = new Double[] { 0.206692913385827, 0.328083989501312 }, Impedancia = new Double[] { 0.259186351706037, 0.36745406824147 } },
            new AlimValues() { Calibre = "250", Reactancia = 0.170603674540682, Resistencia = new Double[] { 0.177165354330709, 0.282152230971129 }, Impedancia = new Double[] { 0.232939632545932, 0.328083989501312 } },
            new AlimValues() { Calibre = "300", Reactancia = 0.167322834645669, Resistencia = new Double[] { 0.147637795275591, 0.236220472440945 }, Impedancia = new Double[] { 0.206692913385827, 0.285433070866142 } },
            new AlimValues() { Calibre = "350", Reactancia = 0.164041994750656, Resistencia = new Double[] { 0.127952755905512, 0.206692913385827 }, Impedancia = new Double[] { 0.187007874015748, 0.255905511811024 } },
            new AlimValues() { Calibre = "400", Reactancia = 0.160761154855643, Resistencia = new Double[] { 0.114829396325459, 0.180446194225722 }, Impedancia = new Double[] { 0.173884514435696, 0.232939632545932 } },
            new AlimValues() { Calibre = "500", Reactancia = 0.15748031496063, Resistencia = new Double[] { 0.0951443569553806, 0.147637795275591 }, Impedancia = new Double[] { 0.154199475065617, 0.200131233595801 } },
            new AlimValues() { Calibre = "600", Reactancia = 0.15748031496063, Resistencia = new Double[] { 0.0820209973753281, 0.124671916010499 }, Impedancia = new Double[] { 0.141076115485564, 0.180446194225722 } },
            new AlimValues() { Calibre = "750", Reactancia = 0.15748031496063, Resistencia = new Double[] { 0.0688976377952756, 0.101706036745407 }, Impedancia = new Double[] { 0.131233595800525, 0.160761154855643 } },
            new AlimValues() { Calibre = "1000", Reactancia = 0.150918635170604, Resistencia = new Double[] { 0.0590551181102362, 0.0820209973753281 }, Impedancia = new Double[] { 0.118110236220472, 0.141076115485564 } }
        };
        public static AlimValues GetValues(String calibre = "")
        {
            if(calibre != "")
                return Data.First(x => x.Calibre == calibre);
            return Data.First();
        }
    }
}
