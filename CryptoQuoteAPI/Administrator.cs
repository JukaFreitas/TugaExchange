using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoQuoteAPI
{
    class Administrator
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

        public void AddComission(string name, decimal comissionTotal)
        {
            var comissionToAdd = new Comission(name, DateTime.Now, comissionTotal);
            _comissions.Add(comissionToAdd); 
        }

    }
}
