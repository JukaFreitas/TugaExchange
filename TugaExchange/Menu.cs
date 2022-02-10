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
            //Estou a retornar um new Investor, no metodo Read, logo se ele não encontrar, vai criar um novo. 
            _investor = _api.ReadInvestor();
            _administrator = _api.ReadAdministrator(); 

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

                case "3": // Relatorio de comissões
                    foreach (var comission in _administrator.Comissions)
                    {
                        Console.WriteLine(comission.CoinName + "|" + comission.ComissionValue + "|" + comission.Date + "|" + comission.Type);
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
                    _api.SaveInvestor(_investor); 
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
                        _api.GetPrices(out List<Coin> updatedCoinsBuy);
                        foreach (var coin in updatedCoinsBuy)
                        {
                            if (coinName == coin.Name)
                            {
                                // return do coinIneuros = coinToBuy.ExchangeRateInEur * quantity; para calcular o total da comissão.
                                var comissionValue = _investor.BuyCoin(coin, coinQuantity, _administrator.ComissionRate);
                                // ja insiro o valor calculado da venda/compra; 
                                var newComission = new Comission(coinName, DateTime.Now, comissionValue, "Compra");
                                _administrator.AddComission(newComission);
                                existsCoin = true;
                                break;
                            }
                        }

                        if (existsCoin)
                        {
                            _api.SaveInvestor(_investor);
                            _api.SaveAdministrator(_administrator); 
                        }
                        else
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

                        _api.GetPrices(out List<Coin> updatedCoinsSell);
                        foreach (var coin in updatedCoinsSell)
                        {
                            if (coinToSell == coin.Name)
                            {
                                //SellCoin está a retornar a comissionValue, valor da comissão que fica para o administrador. 
                                var comissionValue = _investor.SellCoin(coin, coinQuant, _administrator.ComissionRate);
                                var newComission = new Comission(coinToSell, DateTime.Now, comissionValue, "Venda");
                                _administrator.AddComission(newComission);
                                existsCoin = true;
                                break;
                            }
                        }

                        if (existsCoin)
                        {
                            _api.SaveInvestor(_investor);
                            _api.SaveAdministrator(_administrator); 
                        }
                        else
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

                case "4": //portfolio 
                    /*for (int i = 0; i < _investor.Coins.Count; i++)
                    {
                        Console.WriteLine(_investor.Coins[i]);
                    }*/
                    for (int i = 0; i < _investor.Coins.Count; i++)
                    {
                        _api.GetPrices(out List<Coin> coins);
                        foreach (var updatedCoin in coins)
                        {
                            if (updatedCoin.Name == _investor.Coins[i].Name)
                            {
                                var valueInEuros = _investor.CoinsQuantities[i] * updatedCoin.ExchangeRateInEur;
                                Console.WriteLine(updatedCoin.Name + "|" + _investor.CoinsQuantities[i] + "|" + valueInEuros + "|" + updatedCoin.ExchangeRateInEur + "|" + updatedCoin.LastExchangeDate);
                            }
                        }
                    }
                    Thread.Sleep(5000);

                    // mostrar portfólio
                    break;

                case "5": // sera que há execçõeS?
                    _api.GetPrices(out List<Coin> updatedCoins);
                    foreach (var coin in updatedCoins)
                    {
                        Console.WriteLine(coin.Name + "|" + Math.Round(coin.ExchangeRateInEur,2));
                    }
                   /* for (int i = 0; i < coins.Count; i++)
                    {
                        Console.WriteLine(coins[i] + "|" + Math.Round(prices[i], 2));
                    }*/
                    Thread.Sleep(5000);
                    break;

                default: // sair
                    break;
            }
        }
    }
}