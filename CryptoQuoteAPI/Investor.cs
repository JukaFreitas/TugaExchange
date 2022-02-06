using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuoteAPI
{
    class Investor
    {
        private List<Coin> _coins;
        private decimal _quantityCoins;
        private decimal _fundsInEuros;

        public List<Coin> Coins
        {
            get
            {
                return _coins;
            }
            set
            {
                _coins = value;
            }
        }

        public decimal QuantityCoins
        {
            get 
            {
                return _quantityCoins;
            }
            set 
            {
                _quantityCoins = value; 
            }
        }

        public decimal FundsInEuros
        {
            get 
            {
                return _fundsInEuros;
            }
            set 
            {
                _fundsInEuros = value; 
            }
        }
        public Investor()
        {
            _coins = new List<Coin>();
            _fundsInEuros = 0;
            _quantityCoins = 0; 
        }
            
        public void Deposit()
        {

        }

    }
}
