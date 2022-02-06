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
                "1) Adicionar Moeda",
                "2) Remover Moeda",
                "3) Ver relatório das comissões",
                "4) Sair"
            };

            Stats.Print(menuAdministrador);

            var opcaoAdministrador = Stats.ReadString("Insira a opção pretendida:");
            
            switch (opcaoAdministrador)
            {
                case "1": // Adicionar moeda
                    try
                    {
                        Console.WriteLine("Insira o nome da moeda:");
                        var name = Console.ReadLine();
                        _api.AddCoin(name);
                    }
                    catch (Exception baseDados)
                    {
                        Console.WriteLine(baseDados);
                    }
                    Thread.Sleep(5000); 
                    break;

                case "2": // Remover moeda 
                    try
                    {
                        Console.WriteLine("Insira o nome da moeda a remover:");
                        var nameToRemove = Console.ReadLine();
                        _api.RemoveCoin(nameToRemove);
                    }
                    catch (Exception baseDados)
                    {

                        Console.WriteLine(baseDados);
                    }
                    Thread.Sleep(5000); 
                    break;


                default: // sair 
                    break;
            }
        }

        private void MenuInvestidor()
        {
            List<string> menuInvestidor = new List<string>()
            {
                "1) Depositar",
                "2) Comprar Moeda",
                "3) Vender Moeda",
                "4) Mostrar Portfólio",
                "5) Mostrar Câmbio",
                "6) Sair"
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

                    for (int i = 0; i < coins.Count; i++)
                    {
                        Console.WriteLine(coins[i] + "|" + Math.Round(prices[i],2));
                    }
                    Thread.Sleep(5000);
                    break;
                default: // sair 
                    break;
            }
        }
    }
}