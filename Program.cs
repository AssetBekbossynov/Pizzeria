using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Pizzeria.Models;

namespace Pizzeria
{
    class Program
    {
        static readonly string clientPath = "Data/Client.csv";
        static readonly string pizzaPath = "Data/Pizza.csv";
        static readonly string pizzeriaHousePath = "Data/PizzaHouse.csv";

        static void Main(string[] args)
        {
            var clientStore = new ClientStore() { Path = clientPath };
            var pizzaStore = new PizzaStore() { Path = pizzaPath };
            var pizzeriaHouseStore = new PizzeriaHouseStore() { Path = pizzeriaHousePath };

            // example 1. get markets with products
            // group markets with products

            var clientList = clientStore.GetCollection();
            Console.WriteLine("Hello World! client" + (clientList == null));
            var pizzaList = pizzaStore.GetCollection();
            Console.WriteLine("Hello World! pizza" + (pizzaList == null));
            var pizzeriaHouseList = pizzeriaHouseStore.GetCollection();
            Console.WriteLine("Hello World! pizzeria house" + (pizzeriaHouseList == null));

            foreach(var item in clientList){
                Console.WriteLine(string.Format("{0} {1} {2}", 
                    item.Id,
                    item.Address,
                    item.PhoneNumber));
            }

            foreach(var item in pizzaList){
                Console.WriteLine(string.Format("{0} {1} {2} {3} {4}",
                    item.Id,
                    item.PizzaName,
                    item.PizzaCost, 
                    item.Ingredients,
                    item.PizzeriaHouseId));
            }

            foreach(var item in pizzeriaHouseList){
                Console.WriteLine(string.Format("{0} {1}",
                    item.Id,
                    item.Address));
            }

            string path = "Data/Client.csv";
            string readText = File.ReadAllText(path);
            File.AppendAllText(path, "text content" + Environment.NewLine);
            Console.WriteLine("Choose option:\n[1]Show Pizzeria {0}", readText);

            int option = Convert.ToInt32(Console.ReadLine());

        }


    }
}
