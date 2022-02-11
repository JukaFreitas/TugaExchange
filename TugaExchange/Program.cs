using System;
using System.Collections.Generic;
using System.IO;

namespace TugaExchange
{
    class Program 
    {
        static void Main(string[] args)
        {
            var dirInfo = new DirectoryInfo(@"C:temp\tugaExchange");
            if (!dirInfo.Exists)
            {
               dirInfo.Create();
            }

            var menu = new Menu();
            menu.Initialize(); 
            
        }
    }
}
