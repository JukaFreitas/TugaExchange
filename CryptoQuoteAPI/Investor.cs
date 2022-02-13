using System;
using System.Collections.Generic;

namespace CryptoQuoteAPI
{
    public class Investor
    {
        private List<Coin> _coins;
        private List<decimal> _coinsQuantities;
        private decimal _fundsInEuros;
        private string _userName;
        private string _password;

        //expor atributos privados-> para ter acesso nas classes
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public List<Coin> Coins
        {
            get
            {
                return _coins;
            }
            set
            {
                _coins = value;
            }
        }

        public List<decimal> CoinsQuantities
        {
            get
            {
                return _coinsQuantities;
            }
            set
            {
                _coinsQuantities = value;
            }
        }

        public decimal FundsInEuros
        {
            get
            {
                return _fundsInEuros;
            }
            set
            {
                _fundsInEuros = value;
            }
        }

        public Investor(string userName, string password)
        {
            _coins = new List<Coin>();
            _coinsQuantities = new List<decimal>();
            _fundsInEuros = 0;
            _userName = userName;
            _password = password;
        }

        public void Deposit(decimal cashInEuros)
        {
            _fundsInEuros += cashInEuros;
        }

        public decimal BuyCoin(Coin coinToBuy, decimal quantity, decimal comissionRate)
        {
            //converter a quantidade que quero comprar, para euros.
            var coinInEuros = coinToBuy.ExchangeRateInEur * quantity;
            //valor da comissao
            var comissionValue = coinInEuros * comissionRate;
            //o dinheiro que vai gastar na compra é o valor da moeda + dinheiro da comissão -> fundo tem de ser maior ou igual que esta condição.
            if (_fundsInEuros >= (coinInEuros + comissionValue))
            {
                //procurar o indice da moeda a comprar.
                var index = _coins.FindIndex(coin => coin.Name.Equals(coinToBuy.Name));
                // se é -1 indica que o elemento não está na lista. se for diferente é porque tem a moeda naquele indice.
                if (index != -1)
                {
                    //adicionar a quantidade ja existente
                    _coinsQuantities[index] += quantity;
                }
                else
                {
                    _coins.Add(coinToBuy);
                    _coinsQuantities.Add(quantity);
                }
                //retirar o dinheiro pago pela transação
                _fundsInEuros -= (coinInEuros + comissionValue);

                // return para que consiga utilizar o valor no Administrador.
                return comissionValue;
            }
            else
            {
                throw new Exception("Não tem fundo suficiente para fazer a compra.");
            }
        }

        public decimal SellCoin(Coin coinToSell, decimal quantity, decimal comissionRate)
        {
            //indice da moeda
            var index = _coins.FindIndex(coin => coin.Name.Equals(coinToSell.Name));
            //conversão
            var coinInEuros = coinToSell.ExchangeRateInEur * quantity;
            // se tenho a moeda e a quantidade é igual ou menor à quantidade que tem na lista(carteira) -> faz transção
            if (index != -1 && quantity <= _coinsQuantities[index])
            {
                //calculo comissão
                var comissionValue = coinInEuros * comissionRate;
                _coinsQuantities[index] -= quantity;
                // recebe o valor da venda menos a comissão.
                _fundsInEuros += (coinInEuros - comissionValue);

                // se não tiver nenhuma quantidade na carteira -> eliminar na lista de coins e eliminar na lista de quantidades.
                if (_coinsQuantities[index] == 0)
                {
                    _coinsQuantities.RemoveAt(index);
                    _coins.RemoveAt(index);
                }
                // return valor da comissão para o administrador
                return comissionValue;
            }
            else if (index == -1)
            {
                throw new Exception("Só pode vender as moedas que tem na carteira");
            }
            else
            {
                throw new Exception("Só pode vender no limite da quantidade que tem na carteira");
            }
        }

        public decimal SellAllCoin(Coin coinToSell, decimal comissionRate)
        {
            // no caso do administrador eliminar uma moeda, devolve a venda aos investidores que tiverem a moeda na carteira.
            var index = _coins.FindIndex(coin => coin.Name.Equals(coinToSell.Name));
            if (index == -1)
            {
                throw new Exception("O investidor não tem esta moeda.");
            }

            return SellCoin(coinToSell, _coinsQuantities[index], comissionRate);
        }
    }
}