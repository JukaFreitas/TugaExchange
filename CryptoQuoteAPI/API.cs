using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoQuoteAPI
{
    internal class API
    {
        // Para utilizar a lista das moedas já pre-definidas, defini na class currencies. 
        // Lista sobre o qual vou fazer operações
        private int _priceUpdateInSeconds; 
        private List<Coin> _coins = new List<Coin>();

        //Rever se é preciso isto, porque o GetPriceUpdateInSeconds permite obter os preços de cotações -> get return_PriceUpdateInSeconds
        // DefinePrriceUpdateInSeconds permite difinir o valor de tempo -> _priceUpdateInSeconds = value; 
        public int PriceUpdateInSeconds
        {
            get
            {
                return _priceUpdateInSeconds; 
            }
            set
            {
                _priceUpdateInSeconds = value; 
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

        public void AddCoin(string coinName)
        {
            //Validar primeiro se ha uma moeda igual. 
            var coin = new Coin(coinName, 1, DateTime.Now);
            _coins.Add(coin); 

        }

        public List<Coin> GetCoins()
        {
            return _coins; 
        }

        public void RemoveCoin(string coinName)
        {
            bool hasCoin = false; 
            foreach (var coin in _coins)
            { 
                // para aceder à propriedade Name da coin, temos de colocar coin.Name, porque coin(string name, decimal value) é um objeto da Class Coin 
                if (coinName == coin.Name)
                {
                    _coins.Remove(coin);
                    hasCoin = true; 
                }
            }
            // isto para validar se tem Coin ou não que queremos remover da lista. Primeiro, entra no if, se encontra termina com o hasCoin = true; 
            // Caso não aconteca, quer dizer que não há moeda e por isso a criação da excepção. 
            if (!hasCoin)
            {
                throw new Exception("A moeda não consta na base de dados"); 
            }
            //quero mostrar a lista de todas as coins
            //tenho de validar se tenho alguma moeda com aquele nome. 
            
        }

        public void DefinePriceUpdateInSeconds(int seconds)
        {
            _priceUpdateInSeconds = seconds; 
        }

        public int GetPriceUpdateInSeconds()
        {
            return _priceUpdateInSeconds; 
        }

        public List<Coin> GetPrices(out List<decimal> prices, out List<string> coins)
        {

        }



     /*   public API(string name, decimal value)
        {
            List<Coin> currencies = new List<Coin>()
            {
                new Coin {Name = "Doce", Value = 0.95m },
                new Coin {Name = "Galo", Value = 2.03m },
                new Coin {Name = "Doce", Value = 1.23m },
                new Coin {Name = "Tuga", Value = 2.03m }
            };

        }*/
    }
}
