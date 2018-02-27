using System;
using System.Threading;
using Pizzeria.Models;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class OrderDetail 
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public double TotalCost {get; set;} 

        public static int globalOrderDetailID;

        public OrderDetail(){
        }

        public OrderDetail(int Amount, double TotalCost){
            this.Amount = Amount;
            this.TotalCost = TotalCost;
            this.Id = Interlocked.Increment(ref globalOrderDetailID);
        }

        public string DataToString(){
            return Id + ";" + Amount + ";" + TotalCost; 
        }
    }
}