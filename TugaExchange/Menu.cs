using CryptoQuoteAPI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TugaExchange
{
    internal class Menu
    {//tenho de declarar os objetos e depois inicializá-los
        private API _api;
        private Investor _investor;
        private Administrator _administrator;

        public void Initialize()
        {
            _api = new API();
            _api.Read();
            _investor = new Investor();
            _administrator = new Administrator();
            _administrator.Read();

            List<string> menuPrincipal = new List<string>()
            {
                "Investidor",
                "Administrador"
            };

            do
            {
                Stats.Print(menuPrincipal);

                var opcao = Stats.ReadString("Insira a opção pretendida:");

                Console.Clear();

                if (opcao.Equals("1"))
                {
                    MenuInvestidor();
                }
                else if (opcao.Equals("2"))
                {
                    MenuAdministrador();
                }
                else
                {
                    break;
                }
                Console.Clear();
            } while (true);
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

            Stats.Print(menuAdministrador);

            var opcaoAdministrador = Stats.ReadString("Insira a opção pretendida:");

            switch (opcaoAdministrador)
            {
                case "1": // Adicionar moeda
                    try
                    {
                        Console.WriteLine("Insira o nome da moeda:");
                        var name = Console.ReadLine();

                        _api.AddCoin(name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    Thread.Sleep(5000);
                    break;

                case "2": // Remover moeda
                    try
                    {
                        Console.WriteLine("Insira o nome da moeda a remover:");
                        var nameToRemove = Console.ReadLine();
                        _api.RemoveCoin(nameToRemove);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    Thread.Sleep(5000);
                    break;

                case "3":
                    foreach (var comission in _administrator.Comissions)
                    {
                        Console.WriteLine(comission.CoinName + "|" + comission.ComissionValue + "|" + comission.Date + "|" + comission.IsBuy);
                    }

                    Thread.Sleep(3000);
                    break;

                default: // sair
                    break;
            }
        }

        private void MenuInvestidor()
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

            Stats.Print(menuInvestidor);

            var opcaoInvestidor = Stats.ReadString("Insira a opção pretendida:");

            switch (opcaoInvestidor)
            {
                case "1": //Depositar
                    Console.WriteLine("Insira o montante:");
                    var cashInEuros = decimal.Parse(Console.ReadLine());
                    _investor.Deposit(cashInEuros);
                    break;

                case "2": // Comprar moeda
                    try
                    {
                        Console.WriteLine("Insira a moeda e a quantidade a comprar");
                        Console.WriteLine("Moeda:");
                        var coinName = Console.ReadLine();
                        Console.WriteLine("Quantidade:");
                        var coinQuantity = decimal.Parse(Console.ReadLine());
                        bool existsCoin = false;

                        foreach (var coin in _api.GetCoins())
                        {
                            if (coinName == coin.Name)
                            {
                                // return do coinIneuros = coinToBuy.ExchangeRateInEur * quantity; para calcular o total da comissão.
                                var coinInEuros = _investor.BuyCoin(coin, coinQuantity);
                                var newComission = new Comission(coinName, DateTime.Now, coinInEuros * _administrator.ComissionRate, "Buy");
                                _administrator.AddComission(newComission);
                                existsCoin = true;
                                break;
                            }
                        }

                        if (!existsCoin)
                        {
                            throw new Exception("A moeda não existe.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    Thread.Sleep(3000);
                    break;

                case "3": // vender moeda
                    try
                    {
                        Console.WriteLine("Insira a moeda e a quantidade a vender");
                        Console.WriteLine("Moeda");
                        var coinToSell = Console.ReadLine();
                        Console.WriteLine("Quantidade");
                        var coinQuant = decimal.Parse(Console.ReadLine());
                        bool existsCoin = false;

                        foreach (var coin in _api.GetCoins())
                        {
                            if (coinToSell == coin.Name)
                            {
                                var coinInEuros = _investor.SellCoin(coin, coinQuant);
                                var newComission = new Comission(coinToSell, DateTime.Now, coinInEuros * _administrator.ComissionRate, "Sell");
                                _administrator.AddComission(newComission);
                                existsCoin = true;
                                break;
                            }
                        }

                        if (!existsCoin)
                        {
                            throw new Exception("A moeda não existe.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex); ;
                    }
                    Thread.Sleep(3000);
                    break;

                case "4": // mostrar portfólio
                    break;

                case "5": // sera que há execçõeS?
                    _api.GetPrices(out List<decimal> prices, out List<string> coins);

                    for (int i = 0; i < coins.Count; i++)
                    {
                        Console.WriteLine(coins[i] + "|" + Math.Round(prices[i], 2));
                    }
                    Thread.Sleep(5000);
                    break;

                default: // sair
                    break;
            }
        }
    }
}