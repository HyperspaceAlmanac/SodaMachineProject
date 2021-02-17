using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Coin
    {
        //Member Variables (Has A)
        protected double value;
        public string Name;

        public double Value
        {
            get
            {
                return value;
            }


        }
        //Constructor (Spawner)

        //Member Methods (Can Do)
        public static double TotalCoinValue(List<Coin> coins)
        {
            double result = 0.0;
            foreach (Coin c in coins)
            {
                result += c.Value;
            }
            return result;
        }
    }
}
