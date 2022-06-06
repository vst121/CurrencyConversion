using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string currenyNth = "ConversionCurrency1";
            string fromCurrency, toCurrency;
            double rate;

            List<Tuple<string, string, double>> conversionRates = new List<Tuple<string, string, double>> { };

            int i = 1;
            try
            {
                while (_configuration.GetSection(currenyNth).GetSection("FromCurrency").Value is not null)
                {
                    fromCurrency = _configuration.GetSection(currenyNth).GetSection("FromCurrency").Value;
                    toCurrency = _configuration.GetSection(currenyNth).GetSection("ToCurrency").Value;
                    rate = System.Convert.ToDouble(_configuration.GetSection(currenyNth).GetSection("Rate").Value);

                    conversionRates.Add(new Tuple<string, string, double>(fromCurrency, toCurrency, rate));

                    i++;
                    currenyNth = "ConversionCurrency" + i;
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
