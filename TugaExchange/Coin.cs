using System;

namespace TugaExchange
{
    internal class Currency
    {
        // A classe tem de ser inicializada 
        private string _coin;
        private decimal _value;

        public string Coin
        {
            get
            {
                return _coin;
            }

            set 
            {
                _coin = value;
            }
        }

        public decimal Value
        {
            get
            {
                return _value;
            }

            set
            {
               _value = value;
            }
        }


        // Construtor é um método especial.Mas não retorna valor, mas pode receber parametros. Objetivo é inicilizar os parametros da classe. 
        public Currency(string name, decimal value)
        {
            this.Coin = name;
            this.Value = value; 
        }
        public override string ToString()
        {
            return Coin + " " + Value ; 
        }

        public void Print()
        {
               Console.WriteLine($"{Coin} {Value}");
        }
    }
}
