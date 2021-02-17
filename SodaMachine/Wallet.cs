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
        // Leaving Coins as public since Customer needs to directly interact with it 
        public List<Coin> Coins;
        private Card card;
        //Constructor (Spawner)

        public Wallet()
        {
            Coins = new List<Coin>();
            FillWallet();
            PutCardIntoWallet();
        }

        // Card only has getter. Can get Card object and can call its methods, but cannot assign Wallet's card to a new Card. 
        public Card Card
        {
            get { return card; }
        }
        //Member Methods (Can Do)
        //Fills wallet with starting money

        private void PutCardIntoWallet()
        {
            //DEBUG change back to $2.0 after debugging
            card = new Card(2.00);
        }
        private void FillWallet()
        {
            // At least 5 dollars in change. Going with $5.55
            // 12 quarters = 3 dollars, 20 dimes = 2 dollars, 10 nickels = 50 cents, 5 pennies = 5 cents
            for (int i = 0; i < 12; i++)
            {
                Coins.Add(new Quarter());
            }
            for (int i = 0; i < 20; i++)
            {
                Coins.Add(new Dime());
            }
            for (int i = 0; i < 10; i++)
            {
                Coins.Add(new Nickel());
            }
            for (int i = 0; i < 5; i++)
            {
                Coins.Add(new Penny());
            }
        }

        // Customer can call this to check what is in the wallet
        public void CoinsInWallet()
        {
            UserInterface.SeparatorLine();
            UserInterface.OutputText($"You have ${Coin.TotalCoinValue(Coins):F2} in coins");
            UserInterface.DisplayObjects(Coins);
            UserInterface.SeparatorLine();
        }
    }
}
