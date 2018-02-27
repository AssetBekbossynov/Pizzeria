using System;
using System.Threading;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Pizza 
    {

        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string Ingredients { get; set; }
        public int PizzaCost { get; set; }
        public int PizzeriaHouseId { get; set; }

        public static int globalPizzaID;


        public Pizza(){}

        public Pizza(string PizzaName, string Ingredients, int PizzaCost, int PizzeriaHouseId){
            this.PizzaName = PizzaName;
            this.Ingredients = Ingredients;
            this.PizzaCost = PizzaCost;
            this.PizzeriaHouseId = PizzeriaHouseId;
            this.Id = Interlocked.Increment(ref globalPizzaID);
        }

        public string DataToString(){
            return Id + ";" + PizzaName + ";" + Ingredients + ";" + PizzaCost + ";" + PizzeriaHouseId; 
        }
    }
}