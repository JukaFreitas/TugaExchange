using System;

namespace CryptoQuoteAPI
{
    public class Comission
    {
        private string _coinName;
        private DateTime _date;
        private decimal _comissionValue;
        private string _type;

        public string Type
        {
            get 
            {
                return _type;
            }
            set
            {
                _type = value;
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

        public Comission()
        {

        }

        public Comission(string name, DateTime dateNow, decimal comissionValue, string type)
        {
            _coinName = name;
            _date = dateNow;
            _comissionValue = comissionValue;
            _type = type;  
            
        }
    }
}