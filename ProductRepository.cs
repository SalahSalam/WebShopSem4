using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class ProductRepository
    {
        private int _nextId = 1;
        private readonly List<Product> _products = new();
        public ProductRepository()
        {
            Add(new Product { Name = "Yellow", Serialnumber = 12345, Category = ProductCategory.Clothing , Price = 225,InStock = true });
            Add(new Product { Name = "Red", Serialnumber = 123456, Category = ProductCategory.Clothing, Price = 2252, InStock = true });
            Add(new Product { Name = "J&J", Serialnumber = 1234512, Category = ProductCategory.Clothing, Price = 2250, InStock = true });
            Add(new Product { Name = "ProtienPowderIsolate", Serialnumber = 123, Category = ProductCategory.Food, Price = 150, InStock = true });
            Add(new Product { Name = "BCAA", Serialnumber = 1234500, Category = ProductCategory.Food, Price = 200, InStock = true });
        }
        public List<Product> GetAll()
        {
            return new List<Product>(_products);
        }
        public Product? GetById(int id)
        {
            Product? p = _products.FirstOrDefault(p => p.Id == id);

            if (p == null)
            {
                return null;
            }
            else
                return p;
        }
        public Product? Add(Product product)
        {
            product.Id = _nextId++;
            product.Validate();
            //•	Data Integrity: Ensures all properties meet criteria (e.g., name length, race length, age range) before adding to the repository
            //Error Prevention: Catches errors early, preventing invalid data from being added.
            _products.Add(product);
            return product;
        }
        public Product? Remove(int id)
        {
            Product? product = GetById(id);
            if (product == null)
            {
                return null;
            }
            _products.Remove(product);
            return product;
        }
    }
}
