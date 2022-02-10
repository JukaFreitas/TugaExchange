using System.Collections.Generic;
using System.IO;

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

        public void AddComission(Comission newcomission)
        {
            _comissions.Add(newcomission);
           
        }
    }
}
