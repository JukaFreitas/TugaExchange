using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuoteAPI
{
    public class Comission
    {
        private string _coinName;
        private DateTime _date;
        private decimal _comissionValue;

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


        public Comission(string name, DateTime dateNow, decimal comissionValue )
        {
            _coinName = name;  
            _date = dateNow;
            _comissionValue = comissionValue; 
        }
    }
}
