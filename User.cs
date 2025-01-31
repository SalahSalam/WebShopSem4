using System.Text;

namespace WebShopSem4
{
    public enum Role
    {
        Admin,
        Customer
    }

    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public Role UserRole { get; set; }
        public ShoppingCart Cart { get; private set; } = new ShoppingCart();
        private OrderRepository OrderRepository = new OrderRepository();

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Role: {UserRole}";
        }
    }
}