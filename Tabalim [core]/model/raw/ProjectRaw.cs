using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model.raw
{
    public class ProjectRaw
    {
        public string Tablero;
        /// <summary>
        /// La descripción del tablero
        /// </summary>
        public string Description;
        /// <summary>
        /// Las líneas del Alimentador
        /// </summary>
        public AlimentadorRaw[] Lineas;
        public ProjectRaw(Project project, String description = "")
        {
            Tablero = project.ProjectName;
            this.Description = description;
            Lineas = project.Lineas.Values.Select(x => new AlimentadorRaw(x)).ToArray();
        }
    }
}
