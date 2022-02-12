using CryptoQuoteAPI;
using System;
using System.Collections.Generic;

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
                        MenuInvestidor();
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
                                try
                                {
                                    if (coin.Name.Equals(coinName))
                                    {
                                        existCoins = true;
                                        var comissionValue = _investor.SellAllCoin(coin, _administrator.ComissionRate);
                                        _administrator.AddComission(coinName, comissionValue, DateTime.Now, "Venda");       
                                    }
                                }// Se apanhar o erro, pq o investidor não tem a moeda, o catch apanha o erro e prossegue no ciclo
                                catch
                                {
                                }
                            }
                            if (existCoins == false)
                            {
                                throw new Exception("A moeda não existe no sistema");
                            }
                            else
                            {
                                _api.RemoveCoin(coinName);
                                _api.SaveInvestor(_investor);
                                _api.SaveAdministrator(_administrator);
                                Console.WriteLine(Stats.MessageToAdvance("Operação concluida com sucesso"));
                            }
                            break;

                        case 3: // Relatorio de comissões
                            if (_administrator.Comissions.Count == 0)
                            {
                                Console.WriteLine("Não foi efetuada nenhuma transação no sistema");
                            }
                            foreach (var comission in _administrator.Comissions)
                            {
                                Console.WriteLine(comission.CoinName + "|" + comission.ComissionValue + "|" + comission.Date + "|" + comission.Type);
                            }
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
                            _investor.Deposit(cashInDecimals);
                            _api.SaveInvestor(_investor);

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
                                        var comissionValue = _investor.BuyCoin(coin, coinQuantityDecimals, _administrator.ComissionRate);
                                        // ja insiro o valor calculado da venda/compra;
                                        _administrator.AddComission(coinNameFinal, comissionValue, DateTime.Now, "Compra");
                                        existsCoin = true;
                                        break;
                                    }
                                }

                                if (existsCoin)
                                {
                                    _api.SaveInvestor(_investor);
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
                                        var comissionValue = _investor.SellCoin(coin, coinQuantDecimal, _administrator.ComissionRate);
                                        _administrator.AddComission(coinNameFinal, comissionValue, DateTime.Now, "Venda");
                                        existsCoin = true;
                                        break;
                                    }
                                }

                                if (existsCoin)
                                {
                                    _api.SaveInvestor(_investor);
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
                            Console.WriteLine("EUR" + "|" + Math.Round(_investor.FundsInEuros, 2) + "|" + Math.Round(_investor.FundsInEuros, 2) + "|" + "1");
                            _api.GetPrices(out List<Coin> coins);
                            //Total do dinheiro na conta, que começa sempre com o primeiro deposito.
                            var totalEuros = _investor.FundsInEuros;

                            for (int i = 0; i < _investor.Coins.Count; i++)
                            {
                                foreach (var updatedCoin in coins)
                                {
                                    if (updatedCoin.Name == _investor.Coins[i].Name)
                                    {
                                        var valueInEuros = _investor.CoinsQuantities[i] * updatedCoin.ExchangeRateInEur;
                                        totalEuros += valueInEuros;
                                        Console.WriteLine(updatedCoin.Name + "|" + _investor.CoinsQuantities[i] + "|" + Math.Round(valueInEuros, 2) +
                                                          "|" + Math.Round(updatedCoin.ExchangeRateInEur, 2));
                                    }
                                }
                            }

                            Console.WriteLine("\nTotal em EUR: " + Math.Round(totalEuros, 2));
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            break;

                        case 5:
                            _api.GetPrices(out List<Coin> updatedCoins);
                            if (updatedCoins.Count == 0)
                            {
                                Console.WriteLine("Não existem moedas inseridas no sistema");
                            }
                            foreach (var coin in updatedCoins)
                            {
                                Console.WriteLine(coin.Name + "|" + Math.Round(coin.ExchangeRateInEur, 2));
                            }
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