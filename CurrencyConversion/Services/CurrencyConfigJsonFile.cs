using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConversion.Dto;
using Microsoft.Extensions.Configuration;


namespace CurrencyConversion.Services
{
    public class CurrencyConfigJsonFile : ICurrencyConfigJsonFile
    {
        private IConfiguration _configuration;

        public CurrencyConfigJsonFile(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        /// <summary> 
        /// It Initialize currency settings from a json file. 
        /// </summary> 
        public IEnumerable<Tuple<string, string, double>> InitConfigJsonFile()
        {
            List<Tuple<string, string, double>> conversionRates = new List<Tuple<string, string, double>> { };

            int i = 1;
            try
            {
                List <CurrencyRateDto> currencyRateList = _configuration.GetSection("Currencies").Get<List<CurrencyRateDto>>();
                foreach (var currencyRate in currencyRateList)
                {
                    conversionRates.Add(new Tuple<string, string, double>(currencyRate.FromCurrency, currencyRate.ToCurrency, currencyRate.Rate));
                } 
            }
            catch
            {
                throw new Exception("Invalid Currency Conversion File!");
            }

            return conversionRates;
        }
    }
}
