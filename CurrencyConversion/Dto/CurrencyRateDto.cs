using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConversion.Dto
{
    public class CurrencyRateDto
    {
        public string FromCurrency { get; set; } 
        public string ToCurrency { get; set; } 
        public double Rate { get; set; }
    }
}
