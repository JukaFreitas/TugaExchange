using System; 
using System.Collections.Generic;
using System.Threading;
using CryptoQuoteAPI; 

namespace TugaExchange
{
    internal class Menu
    {
        private API _api; 

        public void Initialize()
        {
            _api = new API(); 

            List<string> menuPrincipal = new List<string>()
            {
                "Investidor",
                "Administrador"
            };

            do
            {
                Stats.Print(menuPrincipal);

                var opcao = Stats.ReadString("Insira a opção pretendida:");

                Console.Clear();

                if (opcao.Equals("1"))
                {
                    MenuInvestidor();
                }
                else if (opcao.Equals("2"))
                {
                    MenuAdministrador();
                }
                else
                {
                    break; 
                }
                Console.Clear(); 
            } while (true);

        }

        private void MenuAdministrador()
        {
            List<string> menuAdministrador = new List<string>()
            {
                "Adicionar Moeda",
                "Remover Moeda",
                "Sair"
            };

            Stats.Print(menuAdministrador);

            var opcaoAdministrador = Stats.ReadString("Insira a opção pretendida:");
            
            switch (opcaoAdministrador)
            {
                case "1": // Adicionar moeda 
                    Console.WriteLine("Insira o nome da moeda:");
                    var name = Console.ReadLine();
                    _api.AddCoin(name);
                    break;

                case "2": // Remover moeda 
                    Console.WriteLine("Insira o nome da moeda a remover:");
                    var nameToRemove = Console.ReadLine();
                    _api.RemoveCoin(nameToRemove);
                    break;

                default: // sair 
                    break;
            }
        }

        private void MenuInvestidor()
        {
            List<string> menuInvestidor = new List<string>()
            {
                "Depositar",
                "Comprar Moeda",
                "Vender Moeda",
                "Mostrar Portfólio",
                "Mostrar Câmbio",
                "Sair"
            };

            Stats.Print(menuInvestidor);

            var opcaoInvestidor = Stats.ReadString("Insira a opção pretendida:");

            switch (opcaoInvestidor)
            {
                case "1": //Depositar 
                    break;
                case "2": // Comprar moeda 
                    break;
                case "3": // vender moeda 
                    break;
                case "4": // mostrar portfólio 
                    break;
                case "5":
                    _api.GetPrices(out List<decimal> prices, out List<string> coins);
                    Console.WriteLine(prices);
                    Console.WriteLine(coins);
                    Thread.Sleep(5000);
                    break;
                default: // sair 
                    break;
            }
        }
    }
}