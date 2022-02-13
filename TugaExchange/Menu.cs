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

        public void Initialize()
        {
            _api = new API();
            _api.Read();
            //Estou a retornar um new Investor, no metodo Read, logo se ele não encontrar, vai criar um novo.
            _investors = _api.ReadInvestor();
            _administrator = _api.ReadAdministrator();

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
                        MenuLoginInvestidor();
                    }
                    else if (opcao == 2)
                    {
                        MenuAdministrador();
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
                    Console.WriteLine(Stats.MessageToAdvance(ex.Message));
                    Console.ReadKey();
                }
            } while (true);
        }

        private void MenuLoginInvestidor()
        {
            List<string> menuLoginInvestidor = new List<string>()
            {
                "1) Registar",
                "2) Login",
                "3) Sair"
            };
            do
            {
                try
                {
                    Console.Clear();
                    Stats.Print(menuLoginInvestidor);

                    var opcaoLogin = Stats.OptionToNum("Insira a opção pretendida:");

                    if (!(opcaoLogin >= 1 && opcaoLogin <= 3))
                    {
                        throw new Exception("Tem de inserir um opção entre 1 e 3");
                    }
                    Console.Clear();

                    switch (opcaoLogin)
                    {
                        case 1:
                            Register();
                            break;

                        case 2:
                            Login();
                            break;

                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Stats.MessageToAdvance(ex.Message));
                    Console.ReadKey();
                }
            } while (true);
        }

        private void Register()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("UserName");
                    string userName = Console.ReadLine();
                    userName.Trim();
                    foreach (var existingInvestor in _investors)
                    {
                        if (existingInvestor.UserName.Equals(userName))
                        {
                            throw new Exception("Este UserName já se encontra registado.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
                        }
                    }
                    // quando o código chega a esta parte, o validUserName continua em falso e volta para cima e não faz o break do ciclo.
                    // validUserName se não se encontrar registado, continua com o valor de true e sair do ciclo.

                    Console.WriteLine("Password:");
                    var password = Console.ReadLine();
                    var investor = new Investor(userName, password);
                    _investors.Add(investor);
                    _api.SaveInvestor(_investors);
                    MenuInvestidor(investor);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        return;
                    }
                }
            }
            while (true);
        }

        private void Login()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("UserName:");
                    var userName = Console.ReadLine();
                    userName.Trim();
                    Console.WriteLine("Password:");
                    var password = Console.ReadLine();
                    bool userNameValid = false;
                    foreach (var investor in _investors)
                    {
                        if (investor.UserName.Equals(userName))
                        {
                            userNameValid = true;
                            if (investor.Password.Equals(password))
                            {
                                MenuInvestidor(investor);
                                return;
                            }
                            else
                            {
                                throw new Exception("A password está errada.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
                            }
                        }
                    }
                    if (userNameValid == false)
                    {
                        throw new Exception("O UserName não existe.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    //https://stackoverflow.com/questions/31996519/listen-on-esc-while-reading-console-line
                    //Console ReadKey retorna um ConsoleKeyInfo que tem como atributo a Key.  
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        return; 
                    }
                   
                }
            }
            while (true);
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

            // Try catchs dentro de cada sub menu, para me manter neles e não voltar sempre para o menu principal.
            do
            {
                try
                {
                    Console.Clear();
                    Stats.Print(menuAdministrador);

                    var opcaoAdministrador = Stats.OptionToNum("Insira a opção pretendida:");

                    if (!(opcaoAdministrador >= 1 && opcaoAdministrador <= 4))
                    {
                        throw new Exception("Tem de inserir um opção entre 1 e 4");
                    }

                    Console.Clear();
                    switch (opcaoAdministrador)
                    {
                        case 1: // Adicionar moeda
                            //CoinNameValidation return string e o AddCoin recebe uma string

                            _api.AddCoin(Stats.CoinNameToUpper("Insira o nome da moeda a adicionar:"));
                            Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso."));
                            break;

                        case 2: // Remover moeda
                            var coinName = Stats.CoinNameToUpper("Insira o nome da moeda a remover");
                            _api.GetPrices(out List<Coin> updatedCoins);
                            var existCoins = false;
                            foreach (var coin in updatedCoins)
                            {
                                if (coin.Name.Equals(coinName))
                                {
                                    try
                                    {
                                        existCoins = true;
                                        foreach (var investor in _investors)
                                        {
                                            var comissionValue = investor.SellAllCoin(coin, _administrator.ComissionRate);
                                            _administrator.AddComission(coinName, comissionValue, DateTime.Now, "Venda");
                                        }
                                    } // Se apanhar o erro, pq o investidor não tem a moeda, o catch apanha o erro e prossegue no ciclo
                                    catch
                                    {
                                    }
                                }
                            }
                            if (existCoins == false)
                            {
                                throw new Exception("A moeda não existe no sistema");
                            }
                            else
                            {
                                _api.RemoveCoin(coinName);
                                _api.SaveInvestor(_investors);
                                _api.SaveAdministrator(_administrator);
                                Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso"));
                            }
                            break;

                        case 3: // Relatorio de comissões
                            if (_administrator.Comissions.Count == 0)
                            {
                                Console.WriteLine("Não foi efetuada nenhuma transação no sistema");
                                Console.WriteLine(Stats.MessageToAdvance(""));
                                break;
                            }
                            Console.WriteLine("----------------------------------------------------------------------------------------------");
                            Console.WriteLine($"{"Nome",0 - 20} | {"Valor Comissão",0 - 20} | {"Data",0 - 20} | {"Tipo da Transação",0 - 20}");
                            Console.WriteLine("----------------------------------------------------------------------------------------------");
                            var total = 0.0m;
                            foreach (var comission in _administrator.Comissions)
                            {
                                total += comission.ComissionValue;
                                Console.WriteLine($"{comission.CoinName,0 - 20} | {Math.Round(comission.ComissionValue, 2),0 - 20} | {comission.Date,0 - 20} | {comission.Type,0 - 10}");
                            }
                            Console.WriteLine("----------------------------------------------------------------------------------------------");
                            Console.WriteLine("Comissão total: {0}", Math.Round(total, 2));
                            Console.WriteLine("Quantidade de moedas:");
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            break;

                        default: // sair
                            // Return sem valor, sai do metodo, de forma a sair do ciclo do while e voltar ao menu principal.
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Stats.MessageToAdvance(ex.Message));
                }
                Console.ReadKey();
            } while (true);
        }

        private void MenuInvestidor(Investor investor)
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

            do
            {
                try
                {
                    Console.Clear();
                    Stats.Print(menuInvestidor);

                    var opcaoInvestidor = Stats.OptionToNum("Insira a opção pretendida:");

                    if (!(opcaoInvestidor >= 1 && opcaoInvestidor <= 6))
                    {
                        throw new Exception("Tem de inserir um opção entre 1 e 6");
                    }

                    Console.Clear();
                    switch (opcaoInvestidor)
                    {
                        case 1: //Depositar
                            var cashInDecimals = Stats.CoinQuantityValidation("Insira o montante a depositar em Euros:");
                            investor.Deposit(cashInDecimals);
                            _api.SaveInvestor(_investors);

                            Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso"));
                            break;

                        case 2: // Comprar moeda
                            {
                                Console.WriteLine("Insira a moeda e a quantidade a comprar");
                                var coinNameFinal = Stats.CoinNameToUpper("Moeda:");
                                var coinQuantityDecimals = Stats.CoinQuantityValidation("Quantidade:");
                                bool existsCoin = false;
                                _api.GetPrices(out List<Coin> updatedCoinsBuy);
                                foreach (var coin in updatedCoinsBuy)
                                {
                                    if (coinNameFinal == coin.Name)
                                    {
                                        // return do coinIneuros = coinToBuy.ExchangeRateInEur * quantity; para calcular o total da comissão.
                                        var comissionValue = investor.BuyCoin(coin, coinQuantityDecimals, _administrator.ComissionRate);
                                        // ja insiro o valor calculado da venda/compra;
                                        _administrator.AddComission(coinNameFinal, comissionValue, DateTime.Now, "Compra");
                                        existsCoin = true;
                                        break;
                                    }
                                }

                                if (existsCoin)
                                {
                                    _api.SaveInvestor(_investors);
                                    _api.SaveAdministrator(_administrator);
                                    Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso."));
                                }
                                else
                                {
                                    throw new Exception("A moeda não existe.");
                                }
                            }
                            break;

                        case 3: // vender moeda
                            {
                                Console.WriteLine("Insira a moeda e a quantidade a vender");
                                var coinNameFinal = Stats.CoinNameToUpper("Moeda:");
                                var coinQuantDecimal = Stats.CoinQuantityValidation("Quantidade:");
                                bool existsCoin = false;

                                _api.GetPrices(out List<Coin> updatedCoinsSell);

                                foreach (var coin in updatedCoinsSell)
                                {
                                    if (coinNameFinal == coin.Name)
                                    {
                                        //SellCoin está a retornar a comissionValue, valor da comissão que fica para o administrador.
                                        var comissionValue = investor.SellCoin(coin, coinQuantDecimal, _administrator.ComissionRate);
                                        _administrator.AddComission(coinNameFinal, comissionValue, DateTime.Now, "Venda");
                                        existsCoin = true;
                                        break;
                                    }
                                }

                                if (existsCoin)
                                {
                                    _api.SaveInvestor(_investors);
                                    _api.SaveAdministrator(_administrator);
                                    Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso"));
                                }
                                else
                                {
                                    throw new Exception("A moeda não existe");
                                }
                            }
                            break;

                        case 4: //portfolio
                            Console.WriteLine("---------------------------------------------------------------------------------------");
                            Console.WriteLine($"{"Nome",0 - 16} {"Quantidade",0 - 16} {"Valor EUR",0 - 16} {"Valor Câmbio",0 - 16}");
                            Console.ResetColor();
                            Console.WriteLine("---------------------------------------------------------------------------------------");
                            Console.WriteLine($"{"EUR",0 - 16}| {Math.Round(investor.FundsInEuros, 2),0 - 16}| {Math.Round(investor.FundsInEuros, 2),0 - 16}| {Math.Round(1.00m, 2),0 - 16}");

                            _api.GetPrices(out List<Coin> coins);
                            //Total do dinheiro na conta, que começa sempre com o primeiro deposito.
                            var totalEuros = investor.FundsInEuros;

                            for (int i = 0; i < investor.Coins.Count; i++)
                            {
                                foreach (var updatedCoin in coins)
                                {
                                    if (updatedCoin.Name == investor.Coins[i].Name)
                                    {
                                        var valueInEuros = investor.CoinsQuantities[i] * updatedCoin.ExchangeRateInEur;
                                        totalEuros += valueInEuros;

                                        Console.WriteLine($"{updatedCoin.Name,0 - 16}| {investor.CoinsQuantities[i],0 - 16}| " +
                                                          $"{Math.Round(valueInEuros, 2),0 - 16}| {Math.Round(updatedCoin.ExchangeRateInEur, 2),0 - 16}");
                                    }
                                }
                            }
                            Console.WriteLine("---------------------------------------------------------------------------------------");
                            Console.WriteLine("\nTotal em EUR: " + Math.Round(totalEuros, 2));
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            break;

                        case 5:// cambio
                            _api.GetPrices(out List<Coin> updatedCoins);
                            if (updatedCoins.Count == 0)
                            {
                                Console.WriteLine("Não existem moedas inseridas no sistema");
                            }
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine($"{"Nome",0 - 10}| {"Valor EUR",0 - 10}");
                            Console.WriteLine("-----------------------------------------");
                            foreach (var coin in updatedCoins)
                            {
                                Console.WriteLine($"{coin.Name,0 - 10}| {Math.Round(coin.ExchangeRateInEur, 2),0 - 10}");
                            }
                            Console.WriteLine("-----------------------------------------");
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            break;

                        default: // sair
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Stats.MessageToAdvance(ex.Message));
                }
                Console.ReadKey();
            } while (true);
        }
    }
}