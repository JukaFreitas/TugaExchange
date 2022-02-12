using System;
using System.Collections.Generic;

namespace TugaExchange
{
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
            opcao.Trim();

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
            var coinNameUpper = name.Replace(" ", "").ToUpper();
            return coinNameUpper;
        }

        public static decimal CoinQuantityValidation(string message)
        {
            Console.WriteLine(message);
            var cashInEuros = decimal.TryParse(Console.ReadLine(), out decimal cashInDecimals);
            if (cashInEuros == false)
            {
                throw new Exception("Insira montante válido\n" + "Exemplo: 50,50");
            }
            return cashInDecimals; 
        }

        public static string MessageToAdvance(string message)
        {
            return message + "\nPrima qualquer tecla para avançar"; 
        }
    }
}