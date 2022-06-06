using System;
using System.Collections.Generic;

namespace CurrencyConversion.Services
{
    public interface ICurrencyConfigJsonFile
    {
        /// <summary> 
        /// It Initialize currency settings from a json file. 
        /// </summary> 
        IEnumerable<Tuple<string, string, double>> InitConfigJsonFile();
    }
}