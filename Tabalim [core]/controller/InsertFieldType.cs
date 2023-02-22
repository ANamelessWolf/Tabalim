using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.controller
{
    /// <summary>
    /// Define el tipo de campo que se usará en el insert
    /// </summary>
    public enum InsertFieldType
    {
        NULL = 0,
        NUMBER = 1,
        STRING = 2,
        DATE = 3, 
        BOOLEAN = 4
    }
}
