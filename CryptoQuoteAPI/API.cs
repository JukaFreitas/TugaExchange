using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoQuoteAPI
{
    public class API
    {
        // Para utilizar a lista das moedas já pre-definidas, defini na class currencies.
        // Lista sobre o qual vou fazer operações
        private int _priceUpdateInSeconds;

        private List<Coin> _coins;

        //Rever se é preciso isto, porque o GetPriceUpdateInSeconds permite obter os preços de cotações -> get return_PriceUpdateInSeconds
        // DefinePrriceUpdateInSeconds permite difinir o valor de tempo -> _priceUpdateInSeconds = value;

        // Declarei em cima a Lista de coins, mas só quero que inicializá-la aquando iniciar a API;
        // Defino o valor de tempo, por omissão.
        public API()
        {
            _coins = new List<Coin>();
            _priceUpdateInSeconds = 15;
        }

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
            foreach (var coin in _coins)
            {
                if (coin.Name.Equals(coinName))
                {
                    throw new Exception("A moeda já consta na base de dados.\n Tente novamente:");
                }
            }
            // Criação de nava moeda, pois não é necessário antes da verificação se há uma igual.
            var coinToAdd = new Coin(coinName, 1.00m, DateTime.Now);
            _coins.Add(coinToAdd);
            Save();
        }

        public List<Coin> GetCoins()
        {
            return _coins;
        }

        public void RemoveCoin(string coinName)
        {
            /* bool hasCoin = false;
             foreach (var coin in _coins)
             {
                 // para aceder à propriedade Name da coin, temos de colocar coin.Name, porque coin(string name, decimal value) é um objeto da Class Coin
                 if (coin.Name.Equals(coinName))
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
             }*/
            //quero mostrar a lista de todas as coins
            //tenho de validar se tenho alguma moeda com aquele nome.
            // O codigo de cima não funciona, porque estou a iterar e a remover. Solução: criação de lista fantasma. ToList(), por exemplo.

            //Retorna a quantidade de elementos removidos da lista, segundo a condição.

            var removedQuantity = _coins.RemoveAll(coin => coin.Name.Equals(coinName));
            if (removedQuantity == 0)
            {
                throw new Exception("A moeda não consta na base de dados");
            }
            Save();
        }

        public void DefinePriceUpdateInSeconds(int seconds)
        {
            _priceUpdateInSeconds = seconds;
        }

        public int GetPriceUpdateInSeconds()
        {
            return _priceUpdateInSeconds;
        }

        public void GetPrices(out List<decimal> prices, out List<string> coins)
        {
            prices = new List<decimal>();
            coins = new List<string>();

            foreach (var coin in _coins)
            {
                // Tenho que devolver a lista dos preços atualizados, por isso tenho utilizar o UpdateExhangeRate para ter a lista atualizada de preços,
                // para conseguir adicionar os preços e os nomes das coins às listas criadas.
                coin.UpdateExchangeRate(_priceUpdateInSeconds);
                prices.Add(coin.ExchangeRateInEur);
                coins.Add(coin.Name);
            }
            Save();
        }

        public void Save()
        {
            var fileName = "Coins.json";
            var directory = @"C:\Users\Utilizador\Desktop\Restart2\C#_repositorio\TugaExchange";
            string filePath = Path.Combine(directory, fileName);

            //var fileInfo = new FileInfo(filePath);
            //if (!fileInfo.Exists)
            //{
            //    fileInfo.Create();
            //}

            //Criação json a partir da Lista de Coins
            
            string json = System.Text.Json.JsonSerializer.Serialize(_coins);
            File.WriteAllText(filePath, json);
        }

        public void Read()
        {
            var fileName = "Coins.json";
            var directory = @"C:\Users\Utilizador\Desktop\Restart2\C#_repositorio\TugaExchange";
            string filePath = Path.Combine(directory, fileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var jason = File.ReadAllText(filePath);
                _coins = System.Text.Json.JsonSerializer.Deserialize<List<Coin>>(jason);
            }
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