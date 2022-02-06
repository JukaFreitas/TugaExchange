using System;

namespace CryptoQuoteAPI
{
    public class Coin
    {
        // A classe tem de ser inicializada 
        private string _name;
        private decimal _exchangeRateInEur;
        private DateTime _lastExchangeDate; 

        public DateTime LastExchangeDate
        {
            get
            {
                return _lastExchangeDate; 
            }
            set
            {
                _lastExchangeDate = value; 
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set 
            {
                _name = value; 
            }
        }

        public decimal ExchangeRateInEur
        {
            get
            {
                return _exchangeRateInEur;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Valor tem de ser maior que zero");
                }
               _exchangeRateInEur = value;
            }
        }
        // Construtor é um método especial.Mas não retorna valor, mas pode receber parametros. Objetivo é inicilizar os parametros da classe. 
        // O construtor da Moeda, que tem de ter o nome, o valor e a ultima data de transação. 
        public Coin(string name, decimal exchangeRateInEur, DateTime lastExchangeDate)
        {
            _name = name;
            _exchangeRateInEur = exchangeRateInEur;
            _lastExchangeDate = lastExchangeDate; 
        }
        public override string ToString()
        {
            return _name + " " + _exchangeRateInEur + " " + _lastExchangeDate ; 
        }
        
        public void Print()
        {
               Console.WriteLine($"{_name} {_exchangeRateInEur} {_lastExchangeDate}");
        }

        public void UpdateExchangeRate(int priceUpdateInSeconds)
        { //time span que representa o intervalo de tempo que passou, desde a última exchangeData -> data de cambio. 

            var time = DateTime.Now - _lastExchangeDate;
            var iterationsCount = Convert.ToInt32(time.TotalSeconds) / priceUpdateInSeconds;

            for (int i = 0; i < iterationsCount; i++)
            {
                // valor das moedas 
               _exchangeRateInEur += _exchangeRateInEur * Utils.GetRandomDecimal(-0.005m,0.005m);
                // data e hora atualizada por cada atualização 
                _lastExchangeDate = _lastExchangeDate.AddSeconds(priceUpdateInSeconds); 
            }
        }
    }
}