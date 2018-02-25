using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizzeria.Models
{
    public class ClientStore : IStore<Client>
    {
        private List<Client> _cachedCollection;

        public string Path { get; set; }

        public List<Client> GetCollection()
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

        public Client ConvertItem(string item)
        {
            var itemList = item.Split(';');

            return new Client()
            {
                Id = Convert.ToInt32(itemList[0]),
                Address = itemList[1],
                PhoneNumber = itemList[2]
            };
        }
    }
}