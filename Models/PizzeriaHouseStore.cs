using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizzeria.Models
{
    public class PizzeriaHouseStore : IStore<PizzeriaHouse>
    {
        private List<PizzeriaHouse> _cachedCollection;

        public string Path { get; set; }

        public List<PizzeriaHouse> GetCollection()
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

        public PizzeriaHouse ConvertItem(string item)
        {
            var itemList = item.Split(';');

            return new PizzeriaHouse()
            {
                Id = Convert.ToInt32(itemList[0]),
                Address = itemList[1]
            };
        }
    }
}