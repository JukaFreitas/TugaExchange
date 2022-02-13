using System;
using System.Collections.Generic;

namespace TugaExchange
{
    //class de apoio aos menus
    internal static class Stats
    {
        public static void Print(List<string> menu)
        {
            foreach (var item in menu)
            {
                Console.WriteLine(item);
            }
        }

        public static int OptionToNum(string message)
        {
            Console.WriteLine(message);
            var opcao = Console.ReadLine();
            opcao = opcao.Trim();

            if (!int.TryParse(opcao, out int num))
            {
                throw new Exception("Tem de inserir um número");
            }

            return num;
        }

        public static string CoinNameToUpper(string message)
        {
            Console.WriteLine(message);
            var name = Console.ReadLine();
            //efeito de Trim dos espaços no meio
            var coinNameUpper = name.Replace(" ", "").ToUpper();
            if (coinNameUpper == "")
            {
                throw new Exception("Tem inserir uma moeda.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
            }
            return coinNameUpper;
        }

        public static decimal CoinQuantityValidation(string message)
        {
            Console.WriteLine(message);
            var cashInEuros = decimal.TryParse(Console.ReadLine(), out decimal cashInDecimals);
            if (cashInEuros == false)
            {
                throw new Exception(Stats.MessageToAdvance("Insira montante válido\n" + "Exemplo: 50,50"));
            }
            return cashInDecimals;
        }

        public static string MessageToAdvance(string message)
        {
            return message + "\nPrima qualquer tecla para avançar";
        }

        public static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintSucessMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso"));
            Console.ResetColor();
        }
    }
}