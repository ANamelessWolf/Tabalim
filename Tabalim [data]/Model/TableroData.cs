using System;
using System.Collections.Generic;

namespace Tabalim.Data.Model
{
    public class TableroData
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Path { get; set; }
        public SistemaFaseData Sistema { get; set; }
        public string NombreTablero { get; set; }
        public string Description { get; set; }
        public Dictionary<String, CircuitoData> Circuitos { get; set; }
        
    }
}