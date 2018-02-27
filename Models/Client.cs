using System;
using Pizzeria.Models;
using System.Threading;
using System.Collections.Generic;

namespace Pizzeria.Models
{
    public class Client 
    {

        public int Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public static int globalClientID;

        public Client(){
        }

        public Client(string Address, string PhoneNumber){
            this.Address = Address;
            this.PhoneNumber = PhoneNumber;
            this.Id = Interlocked.Increment(ref globalClientID);
        }
        public string DataToString(){
            return Id + ";" + Address + ";" + PhoneNumber; 
        }
    }
}