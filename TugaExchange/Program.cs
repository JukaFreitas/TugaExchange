using System.IO;

namespace TugaExchange
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Criar directoria se não existir.
            var dirInfo = new DirectoryInfo(@"C:\temp\tugaexchange");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var menu = new Menu();
            menu.Initialize();
        }
    }
}