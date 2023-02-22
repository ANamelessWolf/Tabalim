using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Updater.Sync
{
    /// <summary>
    /// The task command
    /// </summary>
    public struct TaskCommand
    {
        /// <summary>
        /// The command string
        /// </summary>
        public String CommandString;

        /// <summary>
        /// The command parameters
        /// </summary>
        public String[] CommandParameters;
    }
}
