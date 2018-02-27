using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizzeria.Models
{
    public class OrderStore : IStore<Order>
    {
        private List<Order> _cachedCollection;

        public string Path { get; set; }

        public List<Order> GetCollection()
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

        public Order ConvertItem(string item)
        {
            var itemList = item.Split(';');

            return new Order()
            {
                Id = Convert.ToInt32(itemList[0]),
                ClientId = Convert.ToInt32(itemList[1]),
                PizzaId = Convert.ToInt32(itemList[2]),
                PizzeriaHouseId = Convert.ToInt32(itemList[3]),
                OrderDetailId = Convert.ToInt32(itemList[4]),
            };
        }
    }
}