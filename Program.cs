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
            Console.WriteLine("Choose option:\n[1]Show Pizzerias\n[2]Show Pizza\n[3]Order Pizza\n[4]Add Pizza\n[5]Add Client\n[6]Sort Pizza by cost\n[7]Filter Pizza by cost");

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
                Console.WriteLine("Enter pizzeriaHouseId:");
                int pizzeriaHouseId = Convert.ToInt32(Console.ReadLine());
                AddPizza(pizzaName, ingredients, pizzaCost, pizzeriaHouseId);
            }else if(option == 5){
                Console.WriteLine("Enter your phone number:");
                string phone = Console.ReadLine();
                Console.WriteLine("Enter your address:");
                string address = Console.ReadLine();
                AddClient(phone, address);
            }else if(option == 6){
                sortPizzaByItsCost();
            }else if(option == 7){
                Console.WriteLine("Enter cost(We will display all pizzas less than this cost)");
                int cost = Convert.ToInt32(Console.ReadLine());
                filterPizzaByCost(cost);
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
            var pizzeriaHouseStore = new PizzeriaHouseStore() { Path = pizzeriaHousePath };
            var pizzeriaHouseList = pizzeriaHouseStore.GetCollection();

            List<Client> clients = clientList.Where(x => x.PhoneNumber.Equals(requiredPhoneNUmber)).ToList();
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
            
                JoinTables(orderList);

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
            Console.WriteLine("New order was added\nOrder: " + order.DataToString());
        }
        
        public static void AddOrderDetail(int Amount, double TotalCost){
            OrderDetail orderDetail = new OrderDetail(Amount, TotalCost);
            File.AppendAllText(orderDetailPath, orderDetail.DataToString() + Environment.NewLine);
            Console.WriteLine("New order detail was added\nOrderDetail: " + orderDetail.DataToString());
        }

        public static void AddPizza(string PizzaName, string Ingredients, int PizzaCost, int PizzeriaHouseId){
            Pizza pizza = new Pizza(PizzaName, Ingredients, PizzaCost, PizzeriaHouseId);
            File.AppendAllText(pizzaPath, pizza.DataToString() + Environment.NewLine);
            Console.WriteLine("New pizza was added\nPizza: " + pizza.DataToString());
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

        public static void JoinTables(List<Order> orderList){

            Console.WriteLine("Thank you for order\nYour order:");
            Console.WriteLine("PhoneNumber, Client address, Pizza Name, Total Cost, Amount, Pizzeria House Address");

            var orderDetailStore = new OrderDetailStore() { Path = orderDetailPath };
            var orderDetailList = orderDetailStore.GetCollection();
            var clientStore = new ClientStore() { Path = clientPath };
            var clientList = clientStore.GetCollection();
            var pizzaStore = new PizzaStore() { Path = pizzaPath };
            var pizzaList = pizzaStore.GetCollection();
            var pizzeriaHouseStore = new PizzeriaHouseStore() { Path = pizzeriaHousePath };
            var pizzeriaHouseList = pizzeriaHouseStore.GetCollection();

            var orderAndOrderDetail = orderList
                .Join(orderDetailList, x => x.OrderDetailId, y => y.Id, (x, y) => new {
                    x, y
                }).Join(pizzaList, x => x.x.PizzaId, y => y.Id, (x, y) => new {
                    x, y
                }).Join(pizzeriaHouseList, x=>x.x.x.PizzeriaHouseId, y => y.Id, (x, y) => new {
                    x, y
                }).Join(clientList, x=>x.x.x.x.ClientId, y=>y.Id, (x, y)=> new {
                    x, y
                });
            Console.WriteLine("Test" + orderAndOrderDetail.Count() + orderList.Count());
            foreach(var item in orderAndOrderDetail){
                Console.WriteLine(item.y.PhoneNumber + ";" + item.y.Address + ";" + item.x.x.y.PizzaName + ";" + item.x.x.x.y.TotalCost + ";" + item.x.x.x.y.Amount + ";" + item.y.Address + "\n");
            }
        }

        public static void sortPizzaByItsCost() {
            var pizzaStore = new PizzaStore() { Path = pizzaPath };
            var pizzaList = pizzaStore.GetCollection();
            var pizzeriaHouseStore = new PizzeriaHouseStore() { Path = pizzeriaHousePath };
            var pizzeriaHouseList = pizzeriaHouseStore.GetCollection();            
            var sortedPizzaByCost = pizzaList
                    .Join(pizzeriaHouseList, x => x.PizzeriaHouseId, y => y.Id, (x, y) => new {
                        x, y
                    }).OrderBy(x => x.x.PizzaCost).ToList();
            var pInf = "{0} | {1} | {2} | {3} | {4}";
            Console.WriteLine(string.Format(pInf, 
                "Id", 
                "Name", 
                "Ingredients", 
                "Cost", "PizzeriaID"));
            foreach(var item in sortedPizzaByCost) {
                Console.WriteLine(string.Format(pInf,
                    item.x.Id, item.x.PizzaName, item.x.Ingredients, item.x.PizzaCost, item.y.Id
                ));
            }
        }


        public static void filterPizzaByCost(int cost) {
            var pizzaStore = new PizzaStore() { Path = pizzaPath };
            var pizzaList = pizzaStore.GetCollection();
            var pizzeriaHouseStore = new PizzeriaHouseStore() { Path = pizzeriaHousePath };
            var pizzeriaHouseList = pizzeriaHouseStore.GetCollection();            
            var lessThan3000 = pizzaList
                    .Join(pizzeriaHouseList, x => x.PizzeriaHouseId, y => y.Id, (x, y) => new {
                        x, y
                    }).Where(cur => cur.x.PizzaCost < cost).ToList();
            var pInf = "{0} | {1} | {2} | {3} | {4}";
            Console.WriteLine(string.Format(pInf, 
                "Id", 
                "Name", 
                "Ingredients", 
                "Cost", "PizzeriaID"));
            foreach(var item in lessThan3000) {
                Console.WriteLine(string.Format(pInf,
                    item.x.Id, item.x.PizzaName, item.x.Ingredients, item.x.PizzaCost, item.y.Id
                ));
            }
        }
    }
}
