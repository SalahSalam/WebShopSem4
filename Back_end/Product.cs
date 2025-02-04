using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Furniture,
        Other
    }
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Serialnumber { get; set; }
        public ProductCategory Category { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string Tostring()
        {
            return $"Id: {Id}, Name: {Name}, Serialnumber: {Serialnumber}, Category: {Category}, Price: {Price}, InStock: {InStock}";
        }
        public  void ValidateId()
        {
            if (Id < 0)
            {
                throw new ArgumentException("Id must be greater than 0");
            }
        }
        public void Validate()
        {
            ValidateId();
        }
    }
}
