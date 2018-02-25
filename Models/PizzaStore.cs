using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizzeria.Models
{
    public class PizzaStore : IStore<Pizza>
    {
        private List<Pizza> _cachedCollection;

        public string Path { get; set; }

        public List<Pizza> GetCollection()
        {
            if(_cachedCollection == null)
            {
                var data = File.ReadAllLines(Path);
                _cachedCollection = data
                    .Skip(1)
                    .Select(x => ConvertItem(x))
                    .ToList();
            }
            
            return _cachedCollection;
        }

        public Pizza ConvertItem(string item)
        {
            var itemList = item.Split(';');

            return new Pizza()
            {
                Id = Convert.ToInt32(itemList[0]),
                PizzaName = itemList[1],
                Ingredients = itemList[2],
                PizzaCost = Convert.ToInt32(itemList[3]),
                PizzeriaHouseId = Convert.ToInt32(itemList[4])
            };
        }
    }
}