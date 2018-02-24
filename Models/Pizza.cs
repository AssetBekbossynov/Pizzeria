using System;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Pizza 
    {
        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string Ingredients { get; set; }
        public float PizzaCost { get; set; }

    }
}