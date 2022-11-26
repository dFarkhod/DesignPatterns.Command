using System;
using System.Collections.Generic;
using System.Linq;

namespace Virtualdars.DesignPatterns.Command
{

    /// <summary>
    /// Invoker
    /// </summary>
    public class Kiosk
    {
        private OrderCommand _orderCommand;
        private MenuItem _menuItem;
        private FoodOrder _order;

        public Kiosk()
        {
            _order = new FoodOrder();
        }

        public void PlaceOrderItem(MealType mealType, int count)
        {
            _orderCommand = CommandFactory.GetCommand(CommandOption.Add);
            _menuItem = MenuItemFactory.GetMenuItem(mealType, count);
            _order.ExecuteCommand(_orderCommand, _menuItem);
        }

        public void RemoveOrderItem(MealType mealType)
        {
            _orderCommand = CommandFactory.GetCommand(CommandOption.Remove);
            _menuItem = MenuItemFactory.GetMenuItem(mealType, 0);
            _order.ExecuteCommand(_orderCommand, _menuItem);
        }

        public void SubmitOrder()
        {
            _order.ShowCurrentItems();
        }
    }

    /// <summary>
    /// Receiver
    /// </summary>
    public class FoodOrder
    {
        public List<MenuItem> currentItems { get; set; }
        public FoodOrder()
        {
            currentItems = new List<MenuItem>();
        }

        public void ExecuteCommand(OrderCommand command, MenuItem item)
        {
            command.Execute(currentItems, item);
        }

        public void ShowCurrentItems()
        {
            double totalPrice = 0;
            Console.WriteLine("Buyurtma:");
            for (int i = 0; i < currentItems.Count; i++)
            {
                totalPrice += currentItems[i].Meal.Price * currentItems[i].Count;
                Console.WriteLine($"#{i+1}");
                currentItems[i].Display();
            }
            Console.WriteLine($"\nJami: {totalPrice}");
            Console.WriteLine("-----------------------\n");
        }

    }

    /// <summary>
    /// Restoranda buyurtma beriladigan taom
    /// </summary>
    public class MenuItem
    {
        public Meal Meal { get; set; }
        public int Count { get; set; }

        public MenuItem(Meal meal, int count)
        {
            Meal = meal;
            Count = count;
        }

        public void Display()
        {
            Console.WriteLine("  Taom: " + Meal.Type);
            Console.WriteLine("  Soni: " + Count.ToString());
            Console.WriteLine("  Narxi: $" + Meal.Price.ToString());
        }
        
    }

    public class Meal
    {
        public Meal(MealType type)
        {
            Type = type;
            switch (type)
            {
                case MealType.Burger:
                    Price = 5.49;
                    break;
                case MealType.CrispyTender:
                    Price = 6.49;
                    break;
                case MealType.Nugget:
                    Price = 7.49;
                    break;
                case MealType.Chicken:
                    Price = 8.49;
                    break;
                default:
                    Price = 4.49;
                    break;
            }
        }
        public MealType Type { get; set; }
        public double Price { get; init; }
    }

    /// <summary>
    /// Command Abstract Class
    /// </summary>
    public abstract class OrderCommand
    {
        public abstract void Execute(List<MenuItem> orderItems, MenuItem newItem);
    }

    /// <summary>
    /// Concrete Command
    /// </summary>
    public class AddCommand : OrderCommand
    {
        public override void Execute(List<MenuItem> orderItems, MenuItem newItem)
        {
            if (newItem.Count <= 0)
                throw new Exception("Buyurtma miqdori kamida 1ta bo'lishi kerak.");

            orderItems.Add(newItem);
        }
    }

    /// <summary>
    /// Concrete command
    /// </summary>
    public class RemoveCommand : OrderCommand
    {
        public override void Execute(List<MenuItem> currentItems, MenuItem newItem)
        {
            var itemToRemove = currentItems?.Where(x => x.Meal.Type.Equals(newItem.Meal.Type)).FirstOrDefault();
            if (itemToRemove != null)
                currentItems.Remove(itemToRemove);
        }
    }


    public enum MealType
    {
        Burger,
        Chicken,
        CrispyTender,
        Nugget
    }

    public enum CommandOption
    {
        Add,
        Remove
    }

    public static class CommandFactory
    {
        // Factory method
        public static OrderCommand GetCommand(CommandOption commandOption)
        {
            switch (commandOption)
            {
                case CommandOption.Add:
                    return new AddCommand();
                case CommandOption.Remove:
                    return new RemoveCommand();
                default:
                    return new AddCommand();
            }
        }
    }

    public static class MenuItemFactory
    {
        // Factory method
        public static MenuItem GetMenuItem(MealType mealType, int count)
        {
            return new MenuItem(new Meal(mealType), count);
        }
    }
}