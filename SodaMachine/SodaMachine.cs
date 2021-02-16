using System;
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

        //Constructor (Spawner)
        public SodaMachine()
        {
            _register = new List<Coin>();
            _inventory = new List<Can>();
            FillInventory();
            FillRegister();
        }

        //Member Methods (Can Do)

        //A method to fill the sodamachines register with coin objects.
        public void FillRegister()
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
            // Just do 10 of each
            for (int i = 0; i < 10; i++)
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
            return null;
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
           
        }
        //Takes in the value of the amount of change needed.
        //Attempts to gather all the required coins from the sodamachine's register to make change.
        //Returns the list of coins as change to despense.
        //If the change cannot be made, return null.
        private List<Coin> GatherChange(double changeValue)
        {
            List<Coin> result = new List<Coin>;

            // Not as complicated as I thought. Can just check for 
            bool foundChange = true;
            while (changeValue > 0 && foundChange)
            {
                foundChange = false;
                // quarter
                foreach (Tuple<string, double> tuple in Coin.coinNames) {
                    if (changeValue >= tuple.Item2)
                    {
                        if (RegisterHasCoin(tuple.Item1)) {
                            result.Add(GetCoinFromRegister(tuple.Item1));
                            changeValue -= tuple.Item2;
                            foundChange = true;
                            break;
                        }
                    }
                }
            }
            if (foundChange && changeValue == 0) {
                return result;
            } else {
                foreach (Coin c in result)
                {
                    DepositCoinsIntoRegister(c);
                }
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
            return canPrice - totalPayment;
        }
        //Takes in a list of coins to returnt he total value of the coins as a double.
        private double TotalCoinValue(List<Coin> payment)
        {
            double result = 0.0;
            foreach (Coin c in payment)
            {
                result += c.Value;
            }
            return result;
        }
        //Puts a list of coins into the soda machines register.
        private void DepositCoinsIntoRegister(List<Coin> coins)
        {
            foreach (Coin c in coins)
            {
                _register.Add(c);
            }
        }
    }
}
