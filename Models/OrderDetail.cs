using System;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class OrderDetail 
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public float TotalCost {get; set;} 
    }
}