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

            for (int i = 0; i < menuPrincipal.Count; i++)
            {
                Console.WriteLine(menuPrincipal[i]);
            }

            Console.WriteLine("Insira a opção:");
            var opcao = Console.ReadLine();

            Console.Clear();

            if (opcao == "1")
            {
                for (int i = 0; i < menuInvestidor.Count; i++)
                {
                    Console.WriteLine(menuInvestidor[i]);
                }

                Console.WriteLine("Insira a operação:");
                var opcaoInvestidor = Console.ReadLine();

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
                for (int i = 0; i < menuAdministrador.Count; i++)
                {
                    Console.WriteLine(menuAdministrador[i]);
                }

                Console.WriteLine("Insira a operação:");
                var opcaoAdministrador = Console.ReadLine();

                switch (opcaoAdministrador)
                {
                    case "1": // Adicionar moeda 
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