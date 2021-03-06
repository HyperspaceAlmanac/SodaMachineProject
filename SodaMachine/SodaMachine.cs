﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class SodaMachine
    {
        //Member Variables (Has A)
        private List<Coin> _register;
        private List<Can> _inventory;

        // Soda Machine is tied to an online bank account to process Credit Card
        private double bankAccount;

        // Private error margin for double comparison
        private double doubleErrorMargin;

        //Constructor (Spawner)
        public SodaMachine()
        {
            _register = new List<Coin>();
            _inventory = new List<Can>();
            FillInventory();
            FillRegister();

            bankAccount = 0.0;
            // Set error margin
            doubleErrorMargin = 0.001;
        }

        //Member Methods (Can Do)

        //A method to fill the sodamachines register with coin objects.
        public void FillRegister()
        {
            StandardInitialChange();
        }

        private void StandardInitialChange()
        {
            // 20 quarters
            // 10 dimes
            // 20 nickels
            // 50 pennies
            for (int i = 0; i < 10; i++)
            {
                _register.Add(new Quarter());
                _register.Add(new Quarter());

                _register.Add(new Dime());

                _register.Add(new Nickel());
                _register.Add(new Nickel());

                _register.Add(new Penny());
                _register.Add(new Penny());
                _register.Add(new Penny());
                _register.Add(new Penny());
                _register.Add(new Penny());
            }
        }
        //A method to fill the sodamachines inventory with soda can objects.
        public void FillInventory()
        {
            // Just do 5 of each
            //DEBUG change back to 5 after debugging
            for (int i = 0; i < 5; i++)
            {
                _inventory.Add(new RootBeer());
                _inventory.Add(new Cola());
                _inventory.Add(new OrangeSoda());
            }
            
        }
        //Method to be called to start a transaction.
        //Takes in a customer which can be passed freely to which ever method needs it.
        public void BeginTransaction(Customer customer)
        {
            if (!ProductsLeft())
            {
                UserInterface.OutputText("The Soda Machine is empty.");
            }
            bool willProceed = UserInterface.DisplayWelcomeInstructions(_inventory);
            if (willProceed)
            {
                Transaction(customer);
            }
        }
        
        //This is the main transaction logic think of it like "runGame".  This is where the user will be prompted for the desired soda.
        //grab the desired soda from the inventory.
        //get payment from the user.
        //pass payment to the calculate transaction method to finish up the transaction based on the results.
        private void Transaction(Customer customer)
        {
            bool useCard = UserInterface.AskCustomerForCard();
            Can product = GetSodaFromInventory(UserInterface.SodaSelection(_inventory));
            if (useCard)
            {
                Card card = customer.UseCreditCard(product);
                CalculateTransaction(card, product, customer);
            }
            else
            {
                List<Coin> payment = customer.GatherCoinsFromWallet(product);
                CalculateTransaction(payment, product, customer);
            }
            
        }
        //Gets a soda from the inventory based on the name of the soda.
        private Can GetSodaFromInventory(string nameOfSoda)
        {
            Can canObj;
            // If it does not exist, return null
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Name == nameOfSoda)
                {
                    canObj = _inventory[i];
                    _inventory.RemoveAt(i);
                    return canObj;
                }
            }
            // Helper method from UserInterface already checks, but will have something here too
            return null;
        }
        // OVERLOAD
        private void CalculateTransaction(Card card, Can chosenSoda, Customer customer)
        {
            // After first transaction with card, the value can be off
            if (card == null)
            {
                UserInterface.OutputText("Credit Card transaction has been cancelled");
                _inventory.Add(chosenSoda);
            }
            else if (card.ChargeCard(chosenSoda.Price))
            {
                bankAccount += chosenSoda.Price;
                UserInterface.EndMessage(chosenSoda.Name, 0);
                customer.AddCanToBackpack(chosenSoda);
            }
            else
            {
                UserInterface.OutputText("Insufficient funds");
                _inventory.Add(chosenSoda);
            }
            DisplayStatus();
        }

        //This is the main method for calculating the result of the transaction.
        //It takes in the payment from the customer, the soda object they selected, and the customer who is purchasing the soda.
        //This is the method that will determine the following:
        //If the payment is greater than the price of the soda, and if the sodamachine has enough change to return: Despense soda, and change to the customer.
        //If the payment is greater than the cost of the soda, but the machine does not have ample change: Despense payment back to the customer.
        //If the payment is exact to the cost of the soda:  Despense soda.
        //If the payment does not meet the cost of the soda: despense payment back to the customer.
        private void CalculateTransaction(List<Coin> payment, Can chosenSoda, Customer customer)
        {
            double totalAmount = Coin.TotalCoinValue(payment);
            // Need to give back extra
            // Because of Double inaccuraies, it would be easiest to check for equals first

            // if Equal
            if (Math.Abs(totalAmount - chosenSoda.Price) < doubleErrorMargin)
            {
                UserInterface.EndMessage(chosenSoda.Name, 0);
                customer.AddCanToBackpack(chosenSoda);
                DepositCoinsIntoRegister(payment);
            } // If the value + error margin that could make it go slightly below is greater than Soda price
            else if (totalAmount + doubleErrorMargin > chosenSoda.Price)
            {
                // Check to see if machine has enough change
                double change = DetermineChange(Coin.TotalCoinValue(payment), chosenSoda.Price);
                List<Coin> changeInCoins = GatherChange(change);
                if (changeInCoins is null)
                {
                    UserInterface.OutputText($"The soda machine does not have enough change, returning ${Coin.TotalCoinValue(payment):F2}");
                    // Return money and put soda back
                    customer.AddCoinsIntoWallet(payment);
                    _inventory.Add(chosenSoda);
                }
                else
                {
                    UserInterface.EndMessage(chosenSoda.Name, Coin.TotalCoinValue(changeInCoins));
                    customer.AddCanToBackpack(chosenSoda);
                    DepositCoinsIntoRegister(payment);
                    customer.AddCoinsIntoWallet(changeInCoins);
                }
            }
            else // Has to be less than
            {
                // Give back the money, and put soda back into inventory
                UserInterface.OutputText("Not enough for soda, returning money");
                customer.AddCoinsIntoWallet(payment);
                _inventory.Add(chosenSoda);
            }
            DisplayStatus();
        }
        //Takes in the value of the amount of change needed.
        //Attempts to gather all the required coins from the sodamachine's register to make change.
        //Returns the list of coins as change to despense.
        //If the change cannot be made, return null.
        private List<Coin> GatherChange(double changeValue)
        {
            List<Coin> result = new List<Coin>();

            // Not as complicated as I thought. Can just check for 
            bool foundChange = true;

            Coin[] coinTypes = new Coin[] { new Quarter(), new Dime(), new Nickel(), new Penny() };
            while (changeValue > 0 && foundChange)
            {
                foundChange = false;
                // quarter
                foreach (Coin coinType in coinTypes)
                {
                    // Need to handle case of change being slightly less
                    // 0.6 - 0.5 is not equal to 0.1
                    if (coinType.Value <= changeValue + doubleErrorMargin && RegisterHasCoin(coinType.Name))
                    {
                        result.Add(GetCoinFromRegister(coinType.Name));
                        changeValue -= coinType.Value;
                        foundChange = true;
                        break;
                    }
                }
                // If value is not 0, but less than error margin, return
                // Abs as safety feature, since the numbers can go slightly negative due to errorMargin added
                // to change amount left due to how double subtraction can make number slight larger or smaller
                if (Math.Abs(changeValue) < doubleErrorMargin)
                {
                    break;
                }
            }
            if (foundChange && Math.Abs(changeValue) < doubleErrorMargin) {
                return result;
            } else {
                DepositCoinsIntoRegister(result);
                return null;
            }
        }

        //Reusable method to check if the register has a coin of that name.
        //If it does have one, return true.  Else, false.
        private bool RegisterHasCoin(string name)
        {
            foreach (Coin c in _register)
            {
                if (c.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        //Reusable method to return a coin from the register.
        //Returns null if no coin can be found of that name.
        private Coin GetCoinFromRegister(string name)
        {
            Coin result = null;
            for (int i = 0; i < _register.Count; i++)
            {
                if (_register[i].Name == name)
                {
                    result = _register[i];
                    _register.RemoveAt(i);
                    break;
                }
            }
            return result;
        }
        //Takes in the total payment amount and the price of can to return the change amount.
        private double DetermineChange(double totalPayment, double canPrice)
        {
            return totalPayment - canPrice;
        }
        //Takes in a list of coins to returnt he total value of the coins as a double.
        // TotalCoinValue moved to Coin class as public static method

        //Puts a list of coins into the soda machines register.
        private void DepositCoinsIntoRegister(List<Coin> coins)
        {
            foreach (Coin c in coins)
            {
                _register.Add(c);
            }
        }
        private void DisplayStatus()
        {
            UserInterface.SeparatorLine();
            UserInterface.OutputText($"The Soda Machine has made a total of ${bankAccount:F2} from credit card sales");
            UserInterface.OutputText("The Soda Machine has these in stock:");
            UserInterface.DisplayObjects(_inventory);
            UserInterface.OutputText($"The Soda Machine has ${Coin.TotalCoinValue(_register):F2} in change");
            UserInterface.DisplayObjects(_register);
            UserInterface.SeparatorLine();
        }
        private bool ProductsLeft()
        {
            return _inventory.Count > 0;
        }
        private void CustomInitialChange()
        {
            _register.Add(new Nickel());
            _register.Add(new Nickel());

            _register.Add(new Penny());
            _register.Add(new Penny());
            _register.Add(new Penny());
            _register.Add(new Penny());
            _register.Add(new Penny());
        }
    }
}
