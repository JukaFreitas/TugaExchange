using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    class CryptoQuoteAPI
    {
        public static void AddCoin()
        {
            List<Currency> totalCoins = new List<Currency>();
           
            while (true)
            {
                Console.Clear();

                var newCoin = Stats.ReadString("Insira a nova moeda:");
       

                if (newCoin == "0")
                    break;

                totalCoins.Add(new Currency(newCoin));

                GetCoins(totalCoins); 
            }

            if (!totalCoins.Any())
            {
                Console.WriteLine("Erro: lista vazia");
            }
        }

        public static void GetCoins(List<Currency> totalCoins)
        {
            foreach (var coin in totalCoins)
            {
                Console.WriteLine;
            }

        }


    }
}
