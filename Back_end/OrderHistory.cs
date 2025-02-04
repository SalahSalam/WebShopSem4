using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class OrderHistory
    {
      public int OrderId { get; set; }
      public int UserId { get; set; }
      public int OrderDate { get; set; }
      public List<string> Products { get; set; } 
      public decimal TotalAmount { get; set; }

      public override string ToString()
      {
        return $"OrderId: {OrderId}, UserId: {UserId}, OrderDate: {OrderDate}, TotalAmount: {TotalAmount}, Products: {string.Join(", ", Products)}";
      }
    }
}
