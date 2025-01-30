using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class ShoppingCart
    {
        // A private list to store the products added to the shopping cart
        private readonly List<Product> items = new List<Product>();

        // Method to add a product to the shopping cart
        public void AddItem(Product product)
        {
            // Check if the product is not null and is in stock
            if (product != null && product.InStock)
            {
                // Add the product to the items list
                items.Add(product);
            }
        }

        // Method to remove a product from the shopping cart by its ID
        public void RemoveItem(int productId)
        {
            // Find the product in the items list by its ID
            var item = items.FirstOrDefault(p => p.Id == productId);
            // If the product is found, remove it from the items list
            if (item != null)
            {
                items.Remove(item);
            }
        }

        // Method to get all items in the shopping cart
        public IEnumerable<Product> GetItems()
        {
            // Return a read-only collection of the items list
            return items.AsReadOnly();
        }

        // Method to calculate the total amount of all products in the shopping cart
        public decimal GetTotalAmount()
        {
            // Sum the prices of all products in the items list and return the total amount
            return items.Sum(p => p.Price);
        }

        // Override the ToString method to provide a string representation of the shopping cart
        public override string ToString()
        {
            // Create a string representation of each product in the items list
            var productDetails = items.Select(p => $"Id: {p.Id}, Name: {p.Name}, Price: {p.Price}");
            // Join the string representations with newline characters and return the result
            return string.Join("\n", productDetails);
        }
    }
}