using CryptoQuoteAPI;
using System;
using System.Collections.Generic;

namespace TugaExchange
{
    public class MenuAdministrator
    {
        private API _api;

        //investors -> no caso de remover a moeda
        private List<Investor> _investors;

        private Administrator _administrator;

        public MenuAdministrator(API api, List<Investor> investors, Administrator administrator)
        {
            _api = api;
            _investors = investors;
            _administrator = administrator;
        }

        public void MenuAdministrador()
        {
            List<string> menuAdministrador = new List<string>()
            {
                "1) Adicionar Moeda",
                "2) Remover Moeda",
                "3) Ver relatório das comissões",
                "4) Alteração do intervalo de tempo",
                "5) Sair"
            };

            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Password:");
                    var password = Console.ReadLine();
                    if ("1234".Equals(password))
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception("Password Errada.\n\nPrima ESPAÇO para sair ou outra tecla para tentar novamente.");
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

            // Try catchs dentro de cada sub menu, para me manter neles e não voltar sempre para o menu principal.
            do
            {
                try
                {
                    Console.Clear();
                    Stats.Print(menuAdministrador);

                    var opcaoAdministrador = Stats.OptionToNum("Insira a opção pretendida:");

                    if (!(opcaoAdministrador >= 1 && opcaoAdministrador <= 5))
                    {
                        throw new Exception("Tem de inserir um opção entre 1 e 5");
                    }

                    Console.Clear();
                    switch (opcaoAdministrador)
                    {
                        case 1: // Adicionar moeda
                            //CoinNameToUpper return string e o AddCoin recebe uma string
                            _api.AddCoin(Stats.CoinNameToUpper("Insira o nome da moeda a adicionar:"));
                            Stats.PrintSucessMessage();
                            Console.ReadKey();
                            break;

                        case 2: // Remover moeda
                            var coinName = Stats.CoinNameToUpper("Insira o nome da moeda a remover");
                            //eliminar a moeda com a lista atualizada dos preços
                            _api.GetPrices(out List<Coin> updatedCoins);
                            //para validar se a moeda existe
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
                                    } // Se apanhar o erro, pq o investidor não tem a moeda, o catch apanha o erro e prossegue no ciclo -> class Investor
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
                                _api.RemoveCoin(coinName); // Save() -> investor
                                // investor -> tem de retirar a moeda das suas carteiras e devolver o dinheiro da venda
                                _api.SaveInvestors(_investors);
                                _api.SaveAdministrator(_administrator);
                                Stats.PrintSucessMessage();
                                Console.ReadKey();
                            }
                            break;

                        case 3: // Relatorio de comissões
                            if (_administrator.Comissions.Count == 0)
                            {
                                Console.WriteLine("Não foi efetuada nenhuma transação no sistema");
                                Console.WriteLine(Stats.MessageToAdvance(""));
                                Console.ReadKey();
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
                            Console.WriteLine("Comissão total EUR: {0}", Math.Round(total, 2));
                            Console.WriteLine(Stats.MessageToAdvance(""));
                            Console.ReadKey();
                            break;

                        case 4: // definir intervalo de tempo
                            Console.WriteLine("Intervalo de tempo atual: {0}", _api.GetPriceUpdateInSeconds());
                            Console.WriteLine("\nInsira o intervalo de tempo:");
                            var spanTime = int.TryParse(Console.ReadLine(), out int numTime);
                            if (spanTime == true)
                            {
                                _api.DefinePriceUpdateInSeconds(numTime);
                                Stats.PrintSucessMessage();
                                Console.ReadKey();
                                _api.Save();
                            }
                            else
                            {
                                throw new Exception(Stats.MessageToAdvance("Não é permitido o intervalo de tempo inserido."));
                            }
                            break;

                        default: // sair
                            // Return sem valor, sai do metodo, de forma a sair do ciclo do while e voltar ao menu principal.
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Stats.PrintErrorMessage(ex.Message);
                    Console.ReadKey();
                }
            } while (true);
        }
    }
}