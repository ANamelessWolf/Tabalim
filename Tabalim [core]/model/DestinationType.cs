using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabalim.Core.model
{
    public class DestinationType
    {
        public bool? OnlyOneMotor { get; private set; }
        public bool? OnlyOneCarga { get; private set; }
        public bool? OnlyOneAire { get; private set; }
        public bool UseExtraData { get; private set; }
        public String Name { get; private set; }
        public int Id { get; private set; }
        public static IEnumerable<DestinationType> Types = new DestinationType[]
        {
            new DestinationType() { Id = 0, Name = "Tablero derivado", OnlyOneCarga = true, OnlyOneMotor = null, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 1, Name = "Un solo motor", OnlyOneCarga = null, OnlyOneMotor = true, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 2, Name = "Un solo motor y otras cargas", OnlyOneCarga = false, OnlyOneMotor = true, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 3, Name = "Varios motores", OnlyOneCarga = null, OnlyOneMotor = false, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 4, Name = "Varios motores y otras cargas", OnlyOneCarga = false, OnlyOneMotor = false, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 5, Name = "Capacitor", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            new DestinationType() { Id = 6, Name = "Transformador Primario MT", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            new DestinationType() { Id = 7, Name = "Transformador Secundario MT", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            new DestinationType() { Id = 8, Name = "Transformador Primario BT", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            new DestinationType() { Id = 9, Name = "Transformador Secundario BT", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            //new DestinationType() { Id = 10, Name = "Sub-alimentador", OnlyOneCarga = false, OnlyOneMotor = null, UseExtraData = false, OnlyOneAire = null},
            new DestinationType() { Id = 11, Name = "Generador", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = true, OnlyOneAire = null},
            new DestinationType() { Id = 12, Name = "Una sola unidad de AA", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = false, OnlyOneAire = true},
            new DestinationType() { Id = 13, Name = "Varias unidades de AA", OnlyOneCarga = null, OnlyOneMotor = null, UseExtraData = false, OnlyOneAire = false},
        };
        public override string ToString()
        {
            return Name;
        }
    }
}
