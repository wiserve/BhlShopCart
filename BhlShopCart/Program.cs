using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class Program
    {
        protected int errors = 0;

        static void Main(string[] args)
        {
            Program me = new Program();
            me.Process();
        }

        public void Process()
        {
            var sc = new ShopCart();
            try
            {
                if (2.0M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple" }))
                    ErrorDetected("Failed pricing step 1");
                if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange" }))
                    ErrorDetected("Failed pricing step 1");
                if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Grape" }))
                    ErrorDetected("Failed pricing step 1");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                errors++;
            }

            Console.WriteLine("Total errors detected: {0}", errors);
        }

        public void ErrorDetected(string msg)
        {
            errors++;
            Console.WriteLine(msg);
        }
    }
}
