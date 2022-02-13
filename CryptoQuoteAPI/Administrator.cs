using System;
using System.Collections.Generic;

namespace CryptoQuoteAPI
{
    public class Administrator
    {
        private List<Comission> _comissions;
        private decimal _comissionRate;

        public List<Comission> Comissions
        {
            get
            {
                return _comissions;
            }
            set
            {
                _comissions = value;
            }
        }

        public decimal ComissionRate
        {
            get
            {
                return _comissionRate;
            }
            set
            {
                _comissionRate = value;
            }
        }

        public Administrator()
        {
            _comissions = new List<Comission>();
            _comissionRate = 0.01m;
        }

        public void AddComission(string coin, decimal comissionValue, DateTime dateTime, string type)
        {
            var comission = new Comission(coin, dateTime, comissionValue, type);
            _comissions.Add(comission);
        }
    }
}