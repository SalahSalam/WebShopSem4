using System.Collections.Generic;
using System.Linq;

namespace WebShopSem4
{
    public class OrderRepository
    {
        private readonly List<OrderHistory> orderHistories = new List<OrderHistory>();

        public void AddOrder(OrderHistory order)
        {
            orderHistories.Add(order);
        }

        public IEnumerable<OrderHistory> GetOrdersByUserId(int userId)
        {
            return orderHistories.Where(o => o.UserId == userId);
        }
    }
}