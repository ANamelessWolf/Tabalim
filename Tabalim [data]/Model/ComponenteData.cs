using System;

namespace Tabalim.Data.Model
{
    public class ComponenteData
    {
        public int Id { get; set; }
        public int TableroId { get; set; }
        public PotenciaData Potencia { get; set; }
        public String Descripcion { get; set; }
        public int DeltaKey { get; set; }
        public int ImageIndex { get; set; }
        public int Count { get; set; }
        public Circuito Circuito { get; set; }
        public String CircuitoName { get; set; }
    }
}