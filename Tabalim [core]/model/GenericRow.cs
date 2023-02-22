using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public interface GenericRow
    {
        bool IsValid { get; }
        String Nombre { get; }
        int Potencia { get; }
        string Tension { get; }
        string Fases { get; }
        String Corriente { get; }
        String Continua { get; }
        String Longitud { get; }
        String FacAgr { get; }
        String FacTmp { get; }
        String Calibre { get; }
        String Seccion { get; }
        String Caida { get; }
        String PotenciaA { get; }
        String PotenciaB { get; }
        String PotenciaC { get; }
        String Proteccion { get; }
        String Interruptor { get; }
        string this[string name] { get; }
        String Tierra { get; }
    }
}
