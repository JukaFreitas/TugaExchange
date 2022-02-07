using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuoteAPI
{
    public class Investor
    {
        private List<Coin> _coins;
        private List<decimal> _coinsQuantities; 
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
        public  Investor()
        {
            _coins = new List<Coin>();
            _coinsQuantities = new List<decimal>();
            _fundsInEuros = 0;
        }
          
        public void Deposit(decimal cashInEuros)
        {
            _fundsInEuros += cashInEuros; 
        }

        // TODO: Fazer as validações-> se a moeda existe
        public void BuyCoin(Coin coinToBuy, decimal quantity)
        {
            //converter a quantitdade que quero comprar, para euros. 
            var coinInEuros = coinToBuy.ExchangeRateInEur * quantity;

            if (_fundsInEuros <= coinInEuros)
            {
                var index = _coins.FindIndex(coin => coin.Name.Equals(coinToBuy.Name));
                // se é dif de -1, é porque encontrou há um index. 0 == lista vazia. 
                if (index != -1)
                {
                    _coinsQuantities[index] += quantity;
                }
                else
                {
                    _coins.Add(coinToBuy);
                    _coinsQuantities.Add(quantity);
                }
                _fundsInEuros -= coinInEuros; 
            }
            else
            {
                throw new Exception("Não tem fundo suficiente para fazer a compra."); 
            }
        }

        public void SellCoin(Coin coinToSell, decimal quantity)
        {

            //TODO: remover a lista 
            var index = _coins.FindIndex(coin => coin.Name.Equals(coinToSell.Name));
            var coinInEuros = coinToSell.ExchangeRateInEur * quantity;

            if (index != -1 && quantity <= _coinsQuantities[index])
            {
                _coinsQuantities[index] -= quantity;
                _fundsInEuros += coinInEuros; 
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
    }
}
