using System;
using System.Collections.Generic;
using System.Linq;

namespace TugaExchange
{
    internal class Currency
    {
        public string Coin { get; set; }
        public decimal Value { get; set; }


        public Currency()
        {
           
        }

        public Currency(string name)
        {
            this.Coin = name ; 
        }

    }
}
