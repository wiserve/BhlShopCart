using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhlShopCart
{
    public class Program
    {
        // sample shopping lists
        protected string[] listAAOA = { "Apple", "Apple", "Orange", "Apple" };
        protected string[] listAAOAOO = { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange" };
        protected string[] listAAOAOOA = { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Apple" };
        protected string[] listAAOAOOAO = { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Apple", "Orange" };
        protected string[] listABO = { "Apple", "Banana", "Orange" };
        protected string[] listABOB = { "Apple", "Banana", "Orange", "Banana" };
        protected string[] listABOBA = { "Apple", "Banana", "Orange", "Banana", "Apple" };
        protected string[] listABOBB = { "Apple", "Banana", "Orange", "Banana", "Banana" };
        protected string[] listABOBBMO = { "Apple", "Banana", "Orange", "Banana", "Banana", "Melon", "Orange" };
        protected string[] listABOBBMOOMM = { "Apple", "Banana", "Orange", "Banana", "Banana", "Melon", "Orange", "Orange", "Melon", "Melon" };
        protected string[] listOOOOMM = { "Orange", "Orange", "Orange", "Orange", "Melon", "Melon" };

        protected int errors = 0;

        static void Main(string[] args)
        {
            Program me = new Program();
            me.Process();
        }

        public void Process()
        {
            CombinedPriceList plist = new CombinedPriceList();
            plist.AddItem("Apple", 0.45M);
            plist.AddItem("Orange", 0.65M);

            var sc = new ShopCart();
            try
            {
                if (2.0M != sc.Price(listAAOA, plist))
                    ErrorDetected("Failed pricing step 1.1");
                if (3.3M != sc.Price(listAAOAOO, plist))
                    ErrorDetected("Failed pricing step 1.2");
                //if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Grape" }, plist))
                //ErrorDetected("Failed pricing step 3");
                Console.WriteLine("Completed step 1 testing.");

                // for step 2, we will add specials testing
                plist.AddSpecial("bogo", 2, 1);
                plist.AddSpecial("3for2", 3, 2);
                plist.SetSpecial("Apple", "Bogo");
                plist.SetSpecial("Orange", "3for2");

                if (1.55M != sc.Price(listAAOA, plist))
                    ErrorDetected("Failed pricing step 2.1");
                if (2.2M != sc.Price(listAAOAOO, plist))
                    ErrorDetected("Failed pricing step 2.2");
                //if (3.3M != sc.Price(new string[] { "Apple", "Apple", "Orange", "Apple", "Orange", "Orange", "Grape" }, plist))
                //    ErrorDetected("Failed pricing step 2.3");
                if (2.2M != sc.Price(listAAOAOOA, plist))
                    ErrorDetected("Failed pricing step 2.3");
                if (2.85M != sc.Price(listAAOAOOAO, plist))
                    ErrorDetected("Failed pricing step 2.4");
                Console.WriteLine("Completed step 2 testing.");

                // step 3 tests, adding bananas and more special options
                plist.AddItem("banana", 0.60M);
                plist.SetSpecial("Banana", "Bogo");
                if (1.25M != sc.Price(listABO, plist))
                    ErrorDetected("Failed pricing step 3.1");
                if (1.85M != sc.Price(listABOB, plist))
                    ErrorDetected("Failed pricing step 3.2");
                if (1.85M != sc.Price(listABOBB, plist))
                    ErrorDetected("Failed pricing step 3.3");
                if (1.85M != sc.Price(listABOBA, plist))
                    ErrorDetected("Failed pricing step 3.4");
                Console.WriteLine("Completed step 3 testing.");

                // step 4 tests, added melon with separate 3for2 offer
                plist.AddItem("melon", 1.0M);
                plist.AddSpecial("3for2m", 3, 2);
                plist.SetSpecial("melon", "3for2m");
                if (3.5M != sc.Price(listABOBBMO, plist))
                    ErrorDetected("Failed pricing step 4.1");
                if (4.5M != sc.Price(listABOBBMOOMM, plist))
                    ErrorDetected("Failed pricing step 4.2");
                if (3.95M != sc.Price(listOOOOMM, plist))
                    ErrorDetected("Failed pricing step 4.3");
                Console.WriteLine("Completed step 4 testing.");

                // step 5 tests, need to do a real-time, running total of checkout
                ShopCartRT scrt = new ShopCartRT();
                foreach (string item in listABOBBMOOMM)
                {
                    Console.WriteLine("{0:C} for {1}", scrt.Price(item, plist), scrt.DisplayList());
                }
                Console.WriteLine("Completed step 5 testing.");
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
