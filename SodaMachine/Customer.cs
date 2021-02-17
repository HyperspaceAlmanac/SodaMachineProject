using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Customer
    {
        //Member Variables (Has A)
        // Changing to private to give more protection.
        // Code has been structured so that the wallet and backpack are never directly accessed outside of Customer
        private Wallet wallet;
        private Backpack backpack;

        //Constructor (Spawner)
        public Customer()
        {
            wallet = new Wallet();
            backpack = new Backpack();
        }
        //Member Methods (Can Do)

        //This method will be the main logic for a customer to retrieve coins form their wallet.
        //Takes in the selected can for price reference
        //Will need to get user input for coins they would like to add.
        //When all is said and done this method will return a list of coin objects that the customer will use a payment for their soda.
        public List<Coin> GatherCoinsFromWallet(Can selectedCan)
        {
            List<Coin> payment = new List<Coin>();
            string userInput;
            Coin coin;

            // Display contents of wallet once
            wallet.CoinsInWallet();
            do
            {
                userInput = UserInterface.CoinSelection(selectedCan, payment);
                if (userInput == "Reset")
                {
                    AddCoinsIntoWallet(payment, false);
                    payment = new List<Coin>();
                }
                else if (userInput != "Done")
                {
                    coin = GetCoinFromWallet(userInput);
                    if (coin != null)
                    {
                        payment.Add(coin);
                    }
                    else
                    {
                        UserInterface.WaitToContinue("There are no more " + userInput + "s left in the wallet!");
                    }
                }
            }
            while (userInput != "Done");
            
            return payment;
        }

        public Card UseCreditCard(Can selectedCan)
        {
            if (UserInterface.InsertCard(selectedCan))
            {
                return wallet.Card;
            }
            else
            {
                return null;
            }
        }

        //Returns a coin object from the wallet based on the name passed into it.
        //Returns null if no coin can be found
        public Coin GetCoinFromWallet(string coinName)
        {
            Coin result = null;
            for (int i = 0; i < wallet.Coins.Count; i++)
            {
                if (wallet.Coins[i].Name == coinName)
                {
                    result = wallet.Coins[i];
                    wallet.Coins.RemoveAt(i);
                    break;
                }
            }
            return result;
        }
        //Takes in a list of coin objects to add into the customers wallet.
        public void AddCoinsIntoWallet(List<Coin> coinsToAdd, bool display = true)
        {
            foreach (Coin c in coinsToAdd)
            {
                wallet.Coins.Add(c);
            }
            if (display)
            {
                wallet.CoinsInWallet();
            }
        }

        //Takes in a can object to add to the customers backpack.
        public void AddCanToBackpack(Can purchasedCan)
        {
            backpack.cans.Add(purchasedCan);
            // Display content
            backpack.DisplayContent();
        }
    }
}
