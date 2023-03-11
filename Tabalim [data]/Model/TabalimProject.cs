using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Data.Model
{
    public class TabalimProjectData
    {
        public String Path { get; set; }
        public Dictionary<int, TableroData> Tableros { get; set; }
        public Dictionary<int, TableroData> Lineas;
        public String ProjectName { get; set; }
        public DateTime Start { get; set; }
        public int Id { get; set; }
    }
}
