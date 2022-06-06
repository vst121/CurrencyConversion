using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConversion.Models.ViewModels
{
    public class PageViewModel
    {
        public List<string> Currencies { get; set; }
        public string FromCurrency { get; set; } 
        public string ToCurrency { get; set; } 
        public double? Amount { get; set; }
        public double? CalculatedAmount { get; set; }
    }
}
