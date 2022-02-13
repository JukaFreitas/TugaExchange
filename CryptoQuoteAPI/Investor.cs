using System;
using System.Collections.Generic;

namespace CryptoQuoteAPI
{
    public class Investor
    {
        private List<Coin> _coins;
        private List<decimal> _coinsQuantities;
        private decimal _fundsInEuros;
        private string _userName;
        private string _password;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
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

        public List<decimal> CoinsQuantities
        {
            get
            {
                return _coinsQuantities;
            }
            set
            {
                _coinsQuantities = value;
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

        public Investor(string userName, string password)
        {
            _coins = new List<Coin>();
            _coinsQuantities = new List<decimal>();
            _fundsInEuros = 0;
            _userName = userName;
            _password = password; 
        }

        public void Deposit(decimal cashInEuros)
        {
            _fundsInEuros += cashInEuros;
        }

        public decimal BuyCoin(Coin coinToBuy, decimal quantity, decimal comissionRate)
        {
            //converter a quantidade que quero comprar, para euros.
            var coinInEuros = coinToBuy.ExchangeRateInEur * quantity;
            var comissionValue = coinInEuros * comissionRate;
            if (_fundsInEuros >= coinInEuros + comissionValue)
            {
                var index = _coins.FindIndex(coin => coin.Name.Equals(coinToBuy.Name));
                // se é -1 indica que o elemento não está na lista.
                if (index != -1)
                {
                    _coinsQuantities[index] += quantity;
                }
                else
                {
                    _coins.Add(coinToBuy);
                    _coinsQuantities.Add(quantity);
                }
                _fundsInEuros -= (coinInEuros + comissionValue);

                return comissionValue;
            }
            else
            {
                throw new Exception("Não tem fundo suficiente para fazer a compra.");
            }
        }

        public decimal SellCoin(Coin coinToSell, decimal quantity, decimal comissionRate)
        {
            var index = _coins.FindIndex(coin => coin.Name.Equals(coinToSell.Name));
            var coinInEuros = coinToSell.ExchangeRateInEur * quantity;

            if (index != -1 && quantity <= _coinsQuantities[index])
            {
                var comissionValue = coinInEuros * comissionRate; 
                _coinsQuantities[index] -= quantity;
                _fundsInEuros += (coinInEuros - comissionValue);
                
                if (_coinsQuantities[index] == 0)
                {
                    _coinsQuantities.RemoveAt(index);
                    _coins.RemoveAt(index);
                }
                return comissionValue;
                
            }
            else if (index == -1)
            {
                throw new Exception("Só pode vender as moedas que tem na carteira");
            }
            else
            {
                throw new Exception("Só pode vender no limite da quantidade que tem na carteira");
            }
        }

        public decimal SellAllCoin(Coin coinToSell, decimal comissionRate)
        {
            var index = _coins.FindIndex(coin => coin.Name.Equals(coinToSell.Name));
            if (index == -1)
            {
                throw new Exception("O investidor não tem esta moeda.");
            }
            var coinInEuros = coinToSell.ExchangeRateInEur * _coinsQuantities[index];
            return SellCoin(coinToSell, _coinsQuantities[index], comissionRate);

        }
    }
}