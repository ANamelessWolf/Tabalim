using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tabalim.Core.runtime;

namespace Tabalim.Core.controller
{
    public class TableroEventArgs : RoutedEventArgs
    {
        public TabalimApp Application;
        public int TableroId;
        public TableroEventArgs()
        {
        }

        public TableroEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        public TableroEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }
    }
}
