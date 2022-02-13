using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
            //valor default
            _priceUpdateInSeconds = 60;
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
                    throw new Exception("A moeda já consta na base de dados.\n");
                }
            }

            var coinToAdd = new Coin(coinName, 1.00m, DateTime.Now);
            _coins.Add(coinToAdd);
            Save();
        }

        public void RemoveCoin(string coinName)
        {
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

        public void GetPrices(out List<Coin> coins)
        {
            coins = new List<Coin>();

            foreach (var coin in _coins)
            {
                // Tenho que devolver a lista dos preços atualizados, por isso tenho utilizar o UpdateExhangeRate para ter a lista atualizada de preços,
                coin.UpdateExchangeRate(_priceUpdateInSeconds);
                coins.Add(coin);
            }
            Save();
        }

        public void Save()
        {
            var fileName = "Coins.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            string json = System.Text.Json.JsonSerializer.Serialize(_coins);
            File.WriteAllText(filePath, json);

            fileName = "UpdateInSeconds.json";
            filePath = Path.Combine(directory, fileName);
            json = System.Text.Json.JsonSerializer.Serialize(_priceUpdateInSeconds);
            //writealltext cria-me o ficheiro
            File.WriteAllText(filePath, json);
        }

        public void Read()
        {
            var fileName = "Coins.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var json = File.ReadAllText(filePath);
                _coins = System.Text.Json.JsonSerializer.Deserialize<List<Coin>>(json);
            }
            else
            {
                // Se não existir o ficheiro, ao criar o programa já vai adicionar estas coins por default
                _coins.Add(new Coin("DOCE", 1.00m, DateTime.Now));
                _coins.Add(new Coin("GALO", 1.00m, DateTime.Now));
                _coins.Add(new Coin("TUGA", 1.00m, DateTime.Now));
                _coins.Add(new Coin("CHOW", 1.00m, DateTime.Now));
                Save();
            }

            fileName = "UpdateInSeconds.json";
            filePath = Path.Combine(directory, fileName);
            fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var json = File.ReadAllText(filePath);
                //por default é 60 -> definido no construtor da API
                _priceUpdateInSeconds = System.Text.Json.JsonSerializer.Deserialize<int>(json);
            }
        }

        public void SaveInvestors(List<Investor> investors)
        {
            var fileName = "Investors.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            string jsonInvestor = JsonSerializer.Serialize<List<Investor>>(investors);

            File.WriteAllText(filePath, jsonInvestor);
        }

        public List<Investor> ReadInvestors()
        {
            var fileName = "Investors.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var json = File.ReadAllText(filePath);
                var investors = JsonSerializer.Deserialize<List<Investor>>(json);
                return investors;
            }
            else
            {
                // se não existir cria um novo investidor.
                return new List<Investor>();
            }
        }

        public void SaveAdministrator(Administrator administrator)
        {
            var fileName = "Administrator.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            string json = System.Text.Json.JsonSerializer.Serialize<Administrator>(administrator);
            File.WriteAllText(filePath, json);
        }

        public Administrator ReadAdministrator()
        {
            var fileName = "Administrator.json";
            var directory = @"C:\temp\tugaexchange";
            string filePath = Path.Combine(directory, fileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var json = File.ReadAllText(filePath);
                var administrator = System.Text.Json.JsonSerializer.Deserialize<Administrator>(json);
                return administrator;
            }
            else
            {
                return new Administrator();
            }
        }
    }
}