using CryptoQuoteAPI;
using System;
using System.Collections.Generic;

namespace TugaExchange
{
    public class MenuInvestor
    {
        private API _api;
        private List<Investor> _investors;
        private Administrator _administrator;

        public MenuInvestor(API api, List<Investor> investors, Administrator administrator)
        {
            _api = api;
            _investors = investors;
            _administrator = administrator;
        }

        public void MenuLoginInvestidor()
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

        private void Login()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("UserName:");
                    var userName = Console.ReadLine();
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

        private void Register()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("UserName:");
                    string userName = Console.ReadLine();
                    if (userName.Trim() == "")
                    {
                        throw new Exception("Tem inserir um UserName.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
                    }
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
                    if (password.Trim() == "")
                    {
                        throw new Exception("Tem inserir uma password.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
                    }

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

                            Stats.PrintSucessMessage();
                            break;

                        case 2: // Comprar moeda
                            {
                                WriteExchangeRates();
                                Console.WriteLine("\nInsira a moeda e a quantidade a comprar");
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
                                    Stats.PrintSucessMessage();
                                    Console.ReadKey(); 
                                }
                                else
                                {
                                    throw new Exception(Stats.MessageToAdvance("A moeda não existe."));
                                }
                            }
                            break;

                        case 3: // vender moeda
                            {
                                WritePortfolio(investor);
                                Console.WriteLine("\nInsira a moeda e a quantidade a vender");
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
                                    Stats.PrintSucessMessage();
                                    Console.ReadKey(); 
                                }
                                else
                                {
                                    throw new Exception(Stats.MessageToAdvance("A moeda não existe"));
                                }
                            }
                            break;

                        case 4: //portfolio
                            WritePortfolio(investor);
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            Console.ReadKey();
                            break;

                        case 5:// cambio
                            WriteExchangeRates();
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            Console.ReadKey();
                            break;

                        default: // sair
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Stats.PrintErrorMessage(ex.Message);
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        return;
                    }

                }
            } while (true);
        }

        private void WritePortfolio(Investor investor)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------");
            Console.WriteLine($"{"Nome",0 - 16} {"Quantidade",0 - 16} {"Valor EUR",-2 - 16} {"Valor Câmbio",-2 - 16}");
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
        }

        private void WriteExchangeRates()
        {
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
        }
    }
}