using System;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Order 
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PizzeriaHouseId { get; set; }
        public int PizzaId { get; set; }
        
    }
}