using System;
using System.Collections.Generic;
using System.Threading;
namespace ActivityDiagramShop
{
    public class Product
    { public string Name { get; set; }
        public decimal Price { get; set; }
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
    public class Order
    {
        public List<Product> Products { get; private set; } = new List<Product>();
        public string ShippingAddress { get; set; }
        public string CustomerName { get; set; }
        public bool IsPaid { get; set; } = false;
        public void AddProduct(Product product)
        {
            Products.Add(product);
            Console.WriteLine($"[Корзина] Добавлен товар: {product.Name} (${product.Price})");
        }
        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var p in Products) total += p.Price;
            return total;
        }
    }
    public class OrderProcessor
    { public bool Pay(decimal amount)
        {
            Console.Write($"\n[Система оплаты] Списание суммы ${amount}... ");
            
            // Эмуляция задержки
            Thread.Sleep(1000); 

            //30% шанс сбоя оплаты 
            Random rnd = new Random();
            if (rnd.Next(0, 10) < 3) 
            { Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ОШИБКА: Недостаточно средств или сбой сети.");
                Console.ResetColor();
                return false;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("УСПЕШНО.");
            Console.ResetColor();
            return true;
        } public void ProcessOrder(Order order)
        {
            Console.WriteLine("\n[Склад] Проверка наличия товаров...");
            Thread.Sleep(800);
            Console.WriteLine("[Склад] Упаковка заказа для " + order.CustomerName);
        }
        public void ShipOrder(Order order)
        {
            Thread.Sleep(500);
            Console.WriteLine($"[Доставка] Заказ передан курьеру. Адрес: {order.ShippingAddress}");
            Console.WriteLine("[Система] Статус: ЗАВЕРШЕНО.");
        }
    }
    class Program
    {
        static void Main(string[] args)
        { Console.WriteLine("=== МАГАЗИН: ДИАГРАММА ДЕЯТЕЛЬНОСТИ ===");
            Order myOrder = new Order();
            OrderProcessor processor = new OrderProcessor();
            
            Console.WriteLine("\n--- ЭТАП 1: Выбор товаров ---");
            myOrder.AddProduct(new Product("iPhone 15", 999));
            myOrder.AddProduct(new Product("Чехол MagSafe", 50));
            myOrder.AddProduct(new Product("Зарядное устройство", 30));
            
            Console.WriteLine("\n--- ЭТАП 2: Оформление заказа ---");
            Console.Write("Введите ваше имя: ");
            myOrder.CustomerName = Console.ReadLine();
            
            Console.Write("Введите адрес доставки: ");
            myOrder.ShippingAddress = Console.ReadLine();

            if (string.IsNullOrEmpty(myOrder.ShippingAddress))
            {
                // Альтернативный путь
                Console.WriteLine("Ошибка: Адрес не указан. Заказ отменен.");
                return; 
            }
            Console.WriteLine("\n--- ЭТАП 3: Оплата ---");
            decimal totalAmount = myOrder.GetTotal();
            Console.WriteLine($"К оплате: ${totalAmount}");

            bool paymentSuccess = false;
            int attempts = 0;
            while (!paymentSuccess && attempts < 3)
            { Console.WriteLine($"Попытка оплаты #{attempts + 1}...");
                Console.WriteLine("Нажмите Enter, чтобы приложить карту...");
                Console.ReadLine();
                paymentSuccess = processor.Pay(totalAmount);
                if (!paymentSuccess)
                {
                    attempts++;
                    Console.WriteLine("Хотите попробовать снова? (y/n)");
                    string retry = Console.ReadLine();
                    if (retry?.ToLower() != "y")
                    { Console.WriteLine("Заказ отменен покупателем.");
                        return;
                    }
                }
            }
            if (!paymentSuccess)
            {
                Console.WriteLine("Превышено количество попыток оплаты. Заказ аннулирован.");
                return;
            }
            Console.WriteLine("\n--- ЭТАП 4: Обработка ---");
            processor.ProcessOrder(myOrder);
            Console.WriteLine("\n--- ЭТАП 5: Отправка ---");
            processor.ShipOrder(myOrder);
            Console.WriteLine("\n=== КОНЕЦ ===");
            Console.ReadKey();
        }
    }
}