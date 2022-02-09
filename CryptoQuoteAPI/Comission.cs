using System;

namespace CryptoQuoteAPI
{
    public class Comission
    {
        private string _coinName;
        private DateTime _date;
        private decimal _comissionValue;
        private string _type; 

        public bool IsBuy
        {
            get 
            {
                return _isBuy;
            }
            set
            {
                _isBuy = value;
            }
        }

        public string CoinName
        {
            get
            {
                return _coinName;
            }
            set
            {
                _coinName = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public decimal ComissionValue
        {
            get
            {
                return _comissionValue;
            }
            set
            {
                _comissionValue = value;
            }
        }

        public Comission(string name, DateTime dateNow, decimal comissionValue, bool isBuy)
        {
            _coinName = name;
            _date = dateNow;
            _comissionValue = comissionValue;
            _isBuy = ; 
            
        }
    }
}