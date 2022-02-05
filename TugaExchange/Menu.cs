using System; 
using System.Collections.Generic;

namespace TugaExchange
{
    internal class Menu
    {
        public static void Initialize()
        {
            List<string> menuPrincipal = new List<string>()
            {
                "Investidor",
                "Administrador"
            };


            List<string> menuInvestidor = new List<string>()
            {
                "Depositar",
                "Comprar Moeda",
                "Vender Moeda",
                "Mostrar Portfólio",
                "Mostrar Câmbio",
                "Sair"
            };

            List<string> menuAdministrador = new List<string>()
            {
                "Adicionar Moeda",
                "Remover Moeda",
                "Sair"
            };

            Stats.Print(menuPrincipal);

            var opcao = Stats.ReadString("Insira a opção pretendida:");

            Console.Clear();

            if (opcao == "1")
            {
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
                    case "5": // mostrar câmbio 
                        break;
                    default: // sair 
                        break;
                }
            }
            else
            {
                Stats.Print(menuAdministrador);

                var opcaoAdministrador = Stats.ReadString("Insira a opção pretendida:");

                switch (opcaoAdministrador)
                {
                    case "1": // Adicionar moeda 
                        CryptoQuoteAPI.AddCoin();
                        break;
                    case "2": // Remover moeda 
                        break;
                    default: // sair 
                        break;
                }
            }
        }

    }
}