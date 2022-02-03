using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    class Coin
    {

        public Coin()
        {

        }

        public static void AddCoin()
        {
            List<Coin> totalCoins = new List<Coin>();

            while (true)
            {
                Console.WriteLine("Insira a nova moeda:");
                var newCoin= Console.ReadLine();

                //cão de guarda!
                if (newCoin == "0")
                    break;

                totalCoins.Add(newCoin.);
            }

            if (!totalCoins.Any())
            {
                Console.WriteLine("Erro: lista vazia");
            }


        }
    }

}
