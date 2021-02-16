using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Wallet
    {
        //Member Variables (Has A)
        public List<Coin> Coins;
        //Constructor (Spawner)
        public Wallet()
        {
            Coins = new List<Coin>();
            FillRegister();
        }
        //Member Methods (Can Do)
        //Fills wallet with starting money
        private void FillRegister()
        {
          // At least 5 dollars in change. Going with $5.55
          // 12 quarters = 3 dollars, 20 dimes = 2 dollars, 10 nickels = 50 cents, 5 pennies = 5 cents
          
        }
    }
}
