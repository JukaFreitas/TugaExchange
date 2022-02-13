using CryptoQuoteAPI;
using System;
using System.Collections.Generic;

namespace TugaExchange
{
    internal class Menu 
    {//tenho de declarar os objetos e depois inicializá-los
        private API _api;
        private List<Investor> _investors;
        private Administrator _administrator;
        private MenuInvestor _menuInvestor;
        private MenuAdministrator _menuAdministrator;

        public void Initialize()
        {
            _api = new API();
            _api.Read();
            //Estou a retornar um new Investor, no metodo Read, logo se ele não encontrar, vai criar um novo.
            _investors = _api.ReadInvestor();
            _administrator = _api.ReadAdministrator();
            _menuInvestor = new MenuInvestor(_api, _investors, _administrator);
            _menuAdministrator = new MenuAdministrator(_api, _investors, _administrator);

            List<string> menuPrincipal = new List<string>()
            {
                "1) Investidor",
                "2) Administrador",
                "3) Sair"
            };

            do
            {
                try
                {
                    Console.Clear();
                    Stats.Print(menuPrincipal);

                    var opcao = Stats.OptionToNum("Insira a opção pretendida:");

                    if (opcao == 1)
                    {
                        _menuInvestor.MenuLoginInvestidor();
                    }
                    else if (opcao == 2)
                    {
                        _menuAdministrator.MenuAdministrador(); 
                    }
                    else if (opcao == 3)
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception("Tem de inserir opções válidas");
                    }
                }
                catch (Exception ex)
                {
                    Stats.MessageToAdvance(ex.Message);
                    Console.ReadKey();
                }
            } while (true);
        }
    }
}