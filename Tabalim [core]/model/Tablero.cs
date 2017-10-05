using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Core.controller;
using static Tabalim.Core.assets.Constants;
namespace Tabalim.Core.model
{
    /// <summary>
    /// Define la tabla de tablero que utiliza la aplicación
    /// </summary>
    public class Tablero : IDatabaseMappable
    {
        /// <summary>
        /// El id del proyecto al que pertenecen los tableros.
        /// </summary>
        public int ProjectId;
        /// <summary>
        /// El id del tablero.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// El nombre de la tabla que administra tableros
        /// </summary>
        public string TableName { get { return TABLE_TABLERO; } }
        /// <summary>
        /// El sistema que ocupa el tablero.
        /// </summary>
        public SistemaFases Sistema;
        /// <summary>
        /// El nombre del tablero
        /// </summary>
        public string NombreTablero;
        /// <summary>
        /// La colección de circuitos que compone un tablero
        /// </summary>
        public Dictionary<String, Circuito> Circuitos;
        /// <summary>
        /// La colección de componentes que se conectan en un tablero
        /// </summary>
        public Dictionary<int, Componente> Componentes;
        /// <summary>
        /// Crea un registro del objeto en la base de datos.
        /// </summary>
        /// <param name="input">La entrada para crear un tablero es un sistema</param>
        public void Create(object input)
        {

        }
        /// <summary>
        /// Guarda el componente seleccionado en el tablero actual
        /// </summary>
        /// <param name="componente">El componente seleccionado.</param>
        /// <param name="circuito">El circuito del componente seleccionado.</param>
        public void Update(Object input)
        {
            Object[] data = input as Object[];
            Componente componente = data[0] as Componente;
            Circuito circuito = data[1] as Circuito;
            if (componente.Id == -1)//Nuevo componente
                componente.Create(this);
            //Guardamos el circuito
            if (Circuitos.ContainsKey(circuito.ToString()))
            {
                Circuito cto = Circuitos[circuito.ToString()];
                componente.Circuito = cto;
                if (!cto.Componentes.ContainsKey(componente.Id))
                    cto.Componentes.Add(componente.Id, componente);
            }
            else
            {
                circuito.Componentes.Add(componente.Id, componente);
                circuito.Create(this);
            }

        }

    }
}
