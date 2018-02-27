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
        static readonly string orderPath = "Data/Order.csv";
        static readonly string orderDetailPath = "Data/OrderDetail.csv";

        public static void Main(string[] args)
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

            // File.AppendAllText(path, "text content" + Environment.NewLine);
            Console.WriteLine("Choose option:\n[1]Show Pizzerias\n[2]Show Pizza\n[3]Order Pizza\n[4]Add Pizza\n[5]Add Client");

            int option = Convert.ToInt32(Console.ReadLine());

            if(option == 1){
                ShowPizzerias(pizzeriaHouseList);
            }else if(option == 2){
                ShowPizza(pizzaList);
            }else if(option == 3){
                OrderPizza();
            }else if(option == 4){
                Console.WriteLine("Enter Pizza Name:");
                string pizzaName = Console.ReadLine();
                Console.WriteLine("Enter ingredients:");
                string ingredients = Console.ReadLine();
                Console.WriteLine("Enter Pizza Cost:");
                int pizzaCost = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter pizzeriaHOuseId:");
                int pizzeriaHouseId = Convert.ToInt32(Console.ReadLine());
                AddPizza(pizzaName, ingredients, pizzaCost, pizzeriaHouseId);
            }else if(option == 5){
                Console.WriteLine("Enter your phone number:");
                string phone = Console.ReadLine();
                Console.WriteLine("Enter your address:");
                string address = Console.ReadLine();
                AddClient(phone, address);
            }
        }

        public static void ShowPizzerias(List<PizzeriaHouse> pizzeriaHouseList){
            foreach(var item in pizzeriaHouseList){
                Console.WriteLine(string.Format("{0} {1}",
                    item.Id,
                    item.Address));
            }
        }

        public static void ShowPizza(List<Pizza> pizzaList){
            foreach(var item in pizzaList){
                Console.WriteLine(string.Format("{0} {1} {2} {3} {4}",
                    item.Id,
                    item.PizzaName,
                    item.PizzaCost, 
                    item.Ingredients,
                    item.PizzeriaHouseId));
            }
        }

        public static void OrderPizza(){
            
            Console.WriteLine("Enter your phone");
            string requiredPhoneNUmber = Console.ReadLine();
            
            takeOrder:var clientStore = new ClientStore() { Path = clientPath };
            var clientList = clientStore.GetCollection();
            var pizzaStore = new PizzaStore() { Path = pizzaPath };
            var pizzaList = pizzaStore.GetCollection();

            List<Client> clients = clientList.Where(x => x.PhoneNumber.Equals(requiredPhoneNUmber)).ToList();
            Console.WriteLine("Test" + requiredPhoneNUmber + clients.Count);
            if(clients.Count > 0){
                int oneMoreOrder = 1;
                List<Order> orderList = new List<Order>();
                while(true){
                    foreach(var item in pizzaList){
                        Console.WriteLine(string.Format("{0} {1} {2} {3} {4}",
                            item.Id,
                            item.PizzaName,
                            item.PizzaCost, 
                            item.Ingredients,
                            item.PizzeriaHouseId));
                    }

                    Console.WriteLine("Enter pizza Id");
                    int pizzaId = Convert.ToInt32(Console.ReadLine());
                    Pizza orderedPizza = pizzaList.Find(x => x.Id == pizzaId);

                    Console.WriteLine("How many pizza you want");
                    int amount = Convert.ToInt32(Console.ReadLine());
                    OrderDetail orderDetail = new OrderDetail(amount, orderedPizza.PizzaCost*amount);
                    AddOrderDetail(amount,  orderedPizza.PizzaCost*amount);
                    Order order = new Order(clients.ElementAt(0).Id, orderedPizza.PizzeriaHouseId, orderedPizza.Id, orderDetail.Id);
                    AddOrder(clients.ElementAt(0).Id, orderedPizza.PizzeriaHouseId, orderedPizza.Id, orderDetail.Id);

                    Console.WriteLine("Do you want to order one more pizza?\n[0]No\n[1]Yes");
                    oneMoreOrder = Convert.ToInt32(Console.ReadLine());
                    
                    orderList.Add(order);
                    
                    if(oneMoreOrder == 0){
                        break;
                    }else{
                        goto takeOrder;
                    }
                }
                
                Console.WriteLine("Thank you for order\nYour order:\n");

                foreach(var item in orderList){
                    Client client = getClientById(item.ClientId);
                    Console.WriteLine(item.Id + "\n");
                }

            }else{
                Console.WriteLine("We have not found your phone in our db, please enter some information about you\nEnter Address:");

                string address = Console.ReadLine();
                
                AddClient(requiredPhoneNUmber, address);
                
                goto takeOrder;
            }
        }

        public static void AddClient(string PhoneNumber, string Address){
            Client client = new Client(Address, PhoneNumber);
            File.AppendAllText(clientPath, client.DataToString() + Environment.NewLine);

            Console.WriteLine("New client was added\nClient: " + client.DataToString());
        }

        public static void AddOrder(int ClientId, int PizzeriaHouseId, int PizzaId, int OrderDetailId){
            Order order = new Order(ClientId, PizzeriaHouseId, PizzaId, OrderDetailId);
            File.AppendAllText(orderPath, order.DataToString() + Environment.NewLine);
            Console.WriteLine("New order was added\nClient: " + order.DataToString());
        }
        
        public static void AddOrderDetail(int Amount, double TotalCost){
            OrderDetail orderDetail = new OrderDetail(Amount, TotalCost);
            File.AppendAllText(orderDetailPath, orderDetail.DataToString() + Environment.NewLine);
            Console.WriteLine("New order detail was added\nClient: " + orderDetail.DataToString());
        }

        public static void AddPizza(string PizzaName, string Ingredients, int PizzaCost, int PizzeriaHouseId){
            Pizza pizza = new Pizza(PizzaName, Ingredients, PizzaCost, PizzeriaHouseId);
            File.AppendAllText(pizzaPath, pizza.DataToString() + Environment.NewLine);
            Console.WriteLine("New order detail was added\nClient: " + pizza.DataToString());
        }
        public static Client getClientById(int Id){
            var clientStore = new ClientStore() { Path = clientPath };
            var clientList = clientStore.GetCollection();
            foreach(var item in clientList){
                if(item.Id == Id){
                    return item; 
                }
            }
            return null;
        }

    }
}
