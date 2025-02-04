using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class Order
    {
        public int Id { get; set; }
        public int Serialnumber { get; set; }
        public double Totalprice { get; set; }
        public int Moms { get; set; }
    }
}
