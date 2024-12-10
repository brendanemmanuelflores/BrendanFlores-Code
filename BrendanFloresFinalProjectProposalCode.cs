using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<Item> inventory = new List<Item>();
    static List<string> actions = new List<string>();
    const string inventoryFile = "inventory.txt";
    const string actionsFile = "actions.txt";

    class Item
    {
        public int ProductNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ProductType { get; set; }
    }

    static void Main()
    {
        LoadData();
        while (true)
        {
            Console.WriteLine("\n--- Inventory Management System ---");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. Remove Item");
            Console.WriteLine("3. Show Inventory");
            Console.WriteLine("4. Locate Item by Product Number");
            Console.WriteLine("5. Sell Item");
            Console.WriteLine("6. Modify Item");
            Console.WriteLine("7. Calculate Weekly and Monthly Sales");
            Console.WriteLine("8. Compute Total Stock Value");
            Console.WriteLine("9. Display Total Stock");
            Console.WriteLine("10. Show Actions Log");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1: AddItem(); break;
                case 2: RemoveItem(); break;
                case 3: ShowInventory(); break;
                case 4: LocateItem(); break;
                case 5: SellItem(); break;
                case 6: ModifyItem(); break;
                case 7: CalculateSales(); break;
                case 8: ComputeStockValue(); break;
                case 9: DisplayTotalStock(); break;
                case 10: ShowActionsLog(); break;
                case 0: SaveData(); return;
                default: Console.WriteLine("Invalid choice!"); break;
            }
        }
    }

    static void AddItem()
    {
        Console.Write("Enter Product Number: ");
        int productNumber = int.Parse(Console.ReadLine());


        if (inventory.Any(i => i.ProductNumber == productNumber))
        {
            Console.WriteLine("Error: A product with this Product Number already exists!");
            return;
        }

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();


        if (inventory.Any(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Error: A product with this Name already exists!");
            return;
        }

        Console.Write("Enter Price: ");
        double price = double.Parse(Console.ReadLine());

        Console.Write("Enter Stock: ");
        int stock = int.Parse(Console.ReadLine());


        Console.Write("Enter Product Type: ");
        string productType = Console.ReadLine();


        inventory.Add(new Item
        {
            ProductNumber = productNumber,
            Name = name,
            Price = price,
            Stock = stock,
            ProductType = productType
        });


        actions.Add($"[{DateTime.Now}] Added Item: {name} (PN: {productNumber}, Type: {productType})");
        Console.WriteLine("Item added successfully!");
    }


    static void RemoveItem()
    {
        Console.Write("Enter Product Number to Remove: ");
        int productNumber = int.Parse(Console.ReadLine());
        var item = inventory.FirstOrDefault(i => i.ProductNumber == productNumber);
        if (item != null)
        {
            inventory.Remove(item);
            actions.Add($"Removed Item: {item.Name} (PN: {productNumber})");
            Console.WriteLine("Item removed successfully!");
        }
        else
        {
            Console.WriteLine("Item not found!");
        }
    }

    static void ShowInventory()
    {
        Console.WriteLine("\n--- Inventory ---");
        Console.WriteLine("+-----------------+-----------------+------------+------------+-----------------+");
        Console.WriteLine("| Product Number  | Name            | Price      | Stock      | Type            |");
        Console.WriteLine("+-----------------+-----------------+------------+------------+-----------------+");

        foreach (var item in inventory)
        {
            Console.WriteLine("| {0,-15} | {1,-15} | P{2,-9:F2} | {3,-10} | {4,-15} |",
                item.ProductNumber,
                item.Name,
                item.Price,
                item.Stock,
                item.ProductType);  
        }

        Console.WriteLine("+-----------------+-----------------+------------+------------+-----------------+");

        
    }





    static void LocateItem()
    {
        Console.Write("Enter Product Number to Locate: ");
        int productNumber = int.Parse(Console.ReadLine());

        var item = inventory.FirstOrDefault(i => i.ProductNumber == productNumber);
        if (item != null)
        {
            Console.WriteLine("\n--- Located Item ---");
            Console.WriteLine("{0,-15}{1,-15}{2,-10}{3,-10}", "Product Number", "Name", "Price", "Stock");
            Console.WriteLine("{0,-15}{1,-15}P{2,-10:F2}{3,-10}", item.ProductNumber, item.Name, item.Price, item.Stock);
        }
        else
        {
            Console.WriteLine("Item not found!");
        }
    }

    static void SellItem()
    {
        Console.Write("Enter Product Number to Sell: ");
        int productNumber = int.Parse(Console.ReadLine());
        Console.Write("Enter Quantity to Sell: ");
        int quantity = int.Parse(Console.ReadLine());

        var item = inventory.FirstOrDefault(i => i.ProductNumber == productNumber);
        if (item != null && item.Stock >= quantity)
        {
            item.Stock -= quantity;
            string timestamp = DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss tt]");
            actions.Add($"{timestamp} Sold {quantity} of {item.Name} (PN: {productNumber})");
            Console.WriteLine("Item sold successfully!");
        }
        else
        {
            Console.WriteLine("Not enough stock or item not found!");
        }
    }

    static void ModifyItem()
    {
        Console.Write("Enter Product Number to Modify: ");
        int productNumber = int.Parse(Console.ReadLine());
        var item = inventory.FirstOrDefault(i => i.ProductNumber == productNumber);
        if (item != null)
        {
            Console.WriteLine("1. Modify Stock");
            Console.WriteLine("2. Modify Price");
            Console.Write("Choose an option: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                int oldStock = item.Stock;
                Console.Write("Enter New Stock: ");
                int newStock = int.Parse(Console.ReadLine());
                item.Stock = newStock;
                actions.Add($"[{DateTime.Now}] Modified Stock for {item.Name} from {oldStock} to {newStock} (PN: {productNumber})");
            }
            else if (choice == 2)
            {
                double oldPrice = item.Price;
                Console.Write("Enter New Price: ");
                double newPrice = double.Parse(Console.ReadLine());
                item.Price = newPrice;
                actions.Add($"[{DateTime.Now}] Modified Price for {item.Name} from P{oldPrice:F2} to P{newPrice:F2} (PN: {productNumber})");
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }
        else
        {
            Console.WriteLine("Item not found!");
        }
    }

    static void CalculateSales()
    {
        Console.WriteLine("\n--- Weekly and Monthly Sales ---");
        double weeklySales = 0;

        foreach (var action in actions)
        {
            if (action.Contains("Sold"))
            {
                int quantityStart = action.IndexOf("Sold") + 5;
                int quantityEnd = action.IndexOf("of", quantityStart) - 1;
                int quantity = int.Parse(action.Substring(quantityStart, quantityEnd - quantityStart + 1).Trim());

                int pnStart = action.IndexOf("(PN:") + 4;
                int pnEnd = action.IndexOf(')', pnStart);
                int productNumber = int.Parse(action.Substring(pnStart, pnEnd - pnStart).Trim());

                var item = inventory.FirstOrDefault(i => i.ProductNumber == productNumber);
                if (item != null)
                {
                    weeklySales += quantity * item.Price;
                }
            }
        }

        Console.WriteLine($"Weekly Sales: P{weeklySales:F2}");
        Console.WriteLine($"Monthly Sales: P{(weeklySales * 4):F2}");
    }

    static void ComputeStockValue()
    {
        double totalValue = inventory.Sum(item => item.Stock * item.Price);
        Console.WriteLine($"Total Stock Value: P{totalValue:F2}");
    }

    static void DisplayTotalStock()
    {
        int totalStock = inventory.Sum(item => item.Stock);
        Console.WriteLine($"Total Stock: {totalStock}");
    }

    static void ShowActionsLog()
    {

        Console.WriteLine("\n--- Actions Log ---");
        Console.WriteLine("+--------------------------+---------------------------------------------------------------+");
        Console.WriteLine("| Timestamp                | Action                                                     |");
        Console.WriteLine("+--------------------------+---------------------------------------------------------------+");

        foreach (var action in actions)
        {

            int timestampEndIndex = action.IndexOf(']') + 2;

            if (timestampEndIndex > 2)
            {

                string timestamp = action.Substring(0, timestampEndIndex).Trim();


                string logAction = action.Substring(timestampEndIndex).Trim();


                string formattedTimestamp = timestamp.PadRight(25);


                string formattedAction = logAction.PadRight(60);


                Console.WriteLine($"| {formattedTimestamp} | {formattedAction} |");
            }
        }


        Console.WriteLine("+--------------------------+---------------------------------------------------------------+");
    }


    static void LoadData()
    {

        if (File.Exists(inventoryFile))
        {
            inventory.Clear();
            foreach (var line in File.ReadAllLines(inventoryFile))
            {
                var parts = line.Split(',');
                inventory.Add(new Item
                {
                    ProductNumber = int.Parse(parts[0]),
                    Name = parts[1],
                    Price = double.Parse(parts[2]),
                    Stock = int.Parse(parts[3]),
                    ProductType = parts.Length > 4 ? parts[4] : ""
                });
            }
        }


        if (File.Exists(actionsFile))
        {
            actions.Clear();
            actions.AddRange(File.ReadAllLines(actionsFile));
        }
    }



    static void SaveData()
    {

        File.WriteAllLines(inventoryFile, inventory.Select(i => $"{i.ProductNumber},{i.Name},{i.Price},{i.Stock},{i.ProductType}"));


        File.WriteAllLines(actionsFile, actions);
        Console.WriteLine("Data saved successfully!");
    }
}