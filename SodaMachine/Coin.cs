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

        // Tuple array of coin name and value
        public static readonly Tuple<string, double>[] coinNames = new Tuple<string, double>[] {
            new Tuple<string, double> ("Quarter", 0.25),
            new Tuple<string, double> ("Dime", 0.1),
            new Tuple<string, double> ("Nickel", 0.05),
            new Tuple<string, double> ("Penny", 0.01),
        };

        public double Value
        {
            get
            {
                return value;
            }


        }
        //Constructor (Spawner)

        //Member Methods (Can Do)
    }
}
