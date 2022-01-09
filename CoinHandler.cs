using System;
using System.Collections.Generic;
using System.Linq;

namespace Experian_TechTest
{
    /// <summary>
    /// This is just a coin handler method. I'd create an interface and implement it then DI it in the system if
    /// we are dealing with multiple currencies and also for the sake of unit testing.
    /// </summary>
    class CoinHandler
    {
        //List of all UK coins that are currently used, expressed in penny format.
        //1p = 1
        //£5 = 5 * 100(p)
        static List<int> coins = new List<int>()
            {
                1,
                2,
                5,
                10,
                20,
                50,
                100,
                200,
                500,
                1000,
                2000,
                5000,
                10000,
                50000
            };

        public static Dictionary<string, long> GetDenominations(string totalString, string amountString)
        {
            Dictionary<string, long> denominations = new();
            long remaining = 0;

            var (total, amount) = ValidateInput(totalString, amountString);

            remaining = (long)((total - amount) * 100);

            var allApplicable = coins.Where(x => x <= remaining).OrderByDescending(x => x);

            //I've implemented a greedy algorithm here since we're only dealing with UK currency.
            //Other currencies might be a bit different so such a solution might not be suitable for those currencies.
            foreach (var item in allApplicable)
            {
                if (remaining <= 0)
                    break;

                var b = (remaining / item);

                denominations.Add(item.ToString(), b);

                remaining -= (item * b);
            }

            return denominations;
        }

        private static (decimal total, decimal amount) ValidateInput(string totalString, string amountString)
        {
            // This is to double up on the validation. If this were an api service or something external
            // then ideally we'd want to also validate on this end too.

            //I've opted to handle exceptions this way. It's not ideal but for the sake of this tech test, it's sufficient.
            if (!decimal.TryParse(totalString, out decimal total))
            {
                throw new ArithmeticException($"{totalString} is not a valid decimal value.");
            }

            if (!decimal.TryParse(amountString, out decimal amount))
            {
                throw new ArithmeticException($"{totalString} is not a valid decimal value.");
            }

            if (total <= 0 || amount < 0)
            {
                throw new ArgumentException($"Please enter positive values only.");
            }

            if(amount > total)
            {
                throw new ArgumentException($"Purchase price cannot be larger than wallet value.");
            }

            return (total, amount);
        }
    }
}
