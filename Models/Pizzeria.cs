using System;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Pizzeria
    {
        public int Id { get; set; }
        public string PizzeriaName { get; set; }
        public string Address { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public List<Order> Orders { get; set; }
    }
}