using System;
using System.Collections.Generic;
using System.Linq;

namespace Experian_TechTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var walletValueInput = string.Empty;
            var productPurchaseValueInput = string.Empty;

            Console.WriteLine("Enter wallet value  (without currency symbol): ");
            while (!IsAValidInput(walletValueInput = Console.ReadLine()))
            {
                Console.WriteLine("Please enter a positive money value");
            }

            Console.WriteLine("Enter product purchase price (without currency symbol): ");
            while (!IsAValidInput(productPurchaseValueInput = Console.ReadLine()))
            {
                Console.WriteLine("Please enter a positive money value");
            }

            Console.WriteLine();

            var denominations = CoinHandler.GetDenominations(walletValueInput, productPurchaseValueInput);

            foreach (var item in denominations.Where(x => x.Value != 0))
            {
                Console.WriteLine(Humanize(item));
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        //Basic validation
        static bool IsAValidInput(string input)
        {
            if (!decimal.TryParse(input, out decimal total))
            {
                return false;
            }

            if (total <= 0)
            {
                return false;
            }

            return true;
        }

        static string Humanize(KeyValuePair<string, long> denomination)
        {
            long.TryParse(denomination.Key, out long keyAsInt);

            if (keyAsInt < 100)
                return $"{denomination.Value} x {denomination.Key}p";
            else
                return $"{denomination.Value} x £{(keyAsInt / 100)}";
        }
    }
}
