﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    /// <summary>
    /// Define el elemento del circuito que se despliega en la lista
    /// </summary>
    public class CtoItem
    {
        /// <summary>
        /// El circuito definido en la aplicación.
        /// </summary>
        public Circuito Circuito;
        /// <summary>
        /// El circuito definido en la aplicación.
        /// </summary>
        public virtual String CtoName { get { return Circuito.ToString(); } }
        /// <summary>
        /// El circuito definido en la aplicación.
        /// </summary>
        public virtual int CtoCount { get { return this.Circuito.Componentes == null ? 0 : this.Circuito.Componentes.Values.Sum(x => x.Count); } }
        public CtoItem(Circuito c)
        {
            this.Circuito = c;
        }
        public override string ToString()
        {
            return String.Format("{0}, Componenets conectados: {1}", this.CtoName, this.CtoCount);
        }

    }
}
