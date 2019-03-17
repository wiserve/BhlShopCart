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
            PriceList plist = new PriceList();
            plist.Items.Add("APPLE", new Item() { Name = "Apple", Price = 0.45M });
            plist.Items.Add("ORANGE", new Item() { Name = "Orange", Price = 0.65M });

            var sc = new ShopCart();
            try
            {
                if (2.0M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple" }, plist))
                    ErrorDetected("Failed pricing step 1");
                if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange" }, plist))
                    ErrorDetected("Failed pricing step 2");
                //if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Grape" }, plist))
                //ErrorDetected("Failed pricing step 3");

                // for step 2, we will add specials testing
                plist.Specials.Add("APPLE", new Special(2, 1));
                plist.Specials.Add("ORANGE", new Special(3, 2));
                if (1.55M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple" }, plist))
                    ErrorDetected("Failed pricing step 2.1");
                if (2.2M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange" }, plist))
                    ErrorDetected("Failed pricing step 2.2");
                //if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Grape" }, plist))
                //    ErrorDetected("Failed pricing step 2.3");
                if (2.2M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Apple" }, plist))
                    ErrorDetected("Failed pricing step 2.4");
                if (2.85M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Apple", "Orange" }, plist))
                    ErrorDetected("Failed pricing step 2.4");
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
