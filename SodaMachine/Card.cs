using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Card
    {

        private double availableFunds;

        public double AvailableFunds
        {
            get { return availableFunds; }
        }

        public Card(double funds)
        {
            availableFunds = funds;
        }
        public bool ChargeCard(double amount) {
            // Need to check for double error margin if double subtraction happened at some point
            // Though this is most likely compared against price of a single can
            if (availableFunds >= amount - 0.001)
            {
                availableFunds -= amount;
                if (Math.Abs(availableFunds) < 0.001)
                {
                    availableFunds = 0.0;
                }
                UserInterface.OutputText($"Card transaction successful. Remaining fund: ${availableFunds}.2f");
                return true;
            }
            else
            {
                UserInterface.OutputText("The card has been declind");
                return false;
            }
        }
    }
}
