using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    class Stats
    {
        public static void Print(List<string> menu)
        {
            foreach (var item in menu)
            {
                Console.WriteLine(item);
            }
        }

        public static string ReadString(string message)
        {
            Console.WriteLine(message);
            var opcao = Console.ReadLine();

            return opcao;

        }
    }
}
