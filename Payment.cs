using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class Payment
    {
        public string? CardNumber { get; set; }
        public string? CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? CVV { get; set; }
        public decimal Amount { get; set; }

        public string ProcessPayment(string cardNumber, string cardHolder, DateTime expirationDate, string cvv, decimal amount)
        {
            // Simulate payment processing
            // In a real-world scenario, you would integrate with a payment gateway here

            // For now, we'll just return a confirmation message
            return $"Payment of {amount:C} has been processed for {cardHolder}.";
        }
    }
}
