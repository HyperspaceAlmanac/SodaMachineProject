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
            // Since available funds uses double subtraction, need to check for error margin after first transaction
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
                UserInterface.OutputText("The card has been declined");
                return false;
            }
        }
    }
}
