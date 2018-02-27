using System;
using Pizzeria.Models;
using System.Threading;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Order 
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PizzeriaHouseId { get; set; }
        public int PizzaId { get; set; }
        public int OrderDetailId {get; set; }

        public static int globalOrderID;

        public Order(int ClientId, int PizzeriaHouseIdm, int PizzaId, int OrderDetailId){
            this.ClientId = ClientId;
            this.PizzaId = PizzaId;
            this.PizzeriaHouseId = PizzeriaHouseId;
            this.OrderDetailId = OrderDetailId;
            this.Id = Interlocked.Increment(ref globalOrderID);
        }
        public Order(){
        }

        public string DataToString(){
            return Id + ";" + ClientId + ";" + PizzeriaHouseId + ";" + PizzaId + ";" + OrderDetailId; 
        }

    }
}