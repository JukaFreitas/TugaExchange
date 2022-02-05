using System;
using System.Linq;

namespace CryptoQuoteAPI
{
    public class API
    {
        // Para utilizar a lista das moedas já pre-definidas, defini na class currencies. 
        private (string, decimal) Currencies { get; set; }
        Random rnd = new Random();

        public static void AddCoin()
        {
            List<Coin> totalCoins = new List<Coin>();

            while (true)
            {
                Console.Clear();

                var newCoin = Stats.ReadString("Insira a nova moeda:");


                if (newCoin == "0")
                    break;

                totalCoins.Add(new Coin(newCoin));

            }

            foreach (var item in totalCoins)
            {
                item.Print();
            }


            if (!totalCoins.Any())
            {
                Console.WriteLine("Erro: lista vazia");
            }
        }

        public List<Currency> GetCoins()
        {

        }

        public static void RemoveCoins(List<Coin> totalCoins)
        {
            var nameCoin = Stats.ReadString("Insira a moeda que deseja eliminar:");
            var removeCoin = new Coin(nameCoin);

            if (totalCoins.Contains(removeCoin))
            {
                totalCoins.Remove(removeCoin);
            }
            else
            {
                Console.WriteLine("A moeda inserida, não se encontra na base de dados. ");
            }
        }

        public API(string name, decimal value)
        {
            List<Coin> currencies = new List<Coin>()
            {
                new Coin {Name = "Doce", Value = 0.95m },
                new Coin {Name = "Galo", Value = 2.03m },
                new Coin {Name = "Doce", Value = 1.23m },
                new Coin {Name = "Tuga", Value = 2.03m }
            };

        }
    }
}
