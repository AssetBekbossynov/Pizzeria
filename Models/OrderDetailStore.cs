using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizzeria.Models
{
    public class OrderDetailStore : IStore<OrderDetail>
    {
        private List<OrderDetail> _cachedCollection;

        public string Path { get; set; }

        public List<OrderDetail> GetCollection()
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

        public OrderDetail ConvertItem(string item)
        {
            var itemList = item.Split(';');

            return new OrderDetail()
            {
                Id = Convert.ToInt32(itemList[0]),
                Amount = Convert.ToInt32(itemList[1]),
                TotalCost = Convert.ToDouble(itemList[2])
            };
        }
    }
}