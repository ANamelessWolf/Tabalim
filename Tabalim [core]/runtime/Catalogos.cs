using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Tabalim.Core.model;
using static Tabalim.Core.assets.Constants;
using TabalimCats = Tabalim.Data.Repository.CatalogueRepository;

namespace Tabalim.Core.runtime
{
    public static class Catalogos
    {
        /// <summary>
        /// El catalogo de HP Watts
        /// </summary>
        public static List<HPItem> HP_WATTS { get => LoadCatalogue<HPItem>(TABLE_HP_WATTS); }
        
        /// <summary>
        /// Maneja la carga de catalogos desde un JSON definido en
        /// el proyecto de datos de Tabalim
        /// </summary>
        /// <typeparam name="T">El tipo de dato en tabalim core a cargar</typeparam>
        /// <param name="catName">El nombre del catálogo a cargar</param>
        /// <returns>El catalogo como una lista</returns>
        private static List<T> LoadCatalogue<T>(String catName) where T : class
        {
            string json = null;
            try
            {
                switch (catName)
                {
                    case TABLE_HP_WATTS:
                        json = TabalimCats.TABLE_HP_WATTS;
                        break;
                }
                if (json == null)
                    throw new Exception(String.Format(
                        "No se puede cargar el catálogo {0}, revise el JSON de entrada", catName));
                var data = JsonConvert.DeserializeObject<T[]>(json);
                return data.ToList();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message.ToString());
                throw;
            }
        }

    }
}
