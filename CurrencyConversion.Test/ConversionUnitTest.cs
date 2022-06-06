using CurrencyConversion.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace CurrencyConversion.Test
{
    public class ConversionUnitTest
    {
        List<string> allCurrencies;
        int allCurrenciesCount;
        List<Tuple<string, string, double>> conversionRates;
        CurrencyConverter currencyConverter;

        const string USD = "USD";
        const string CAD = "CAD";
        const string GBR = "GBR";
        const string QAR = "QAR";
        const string EUR = "EUR";
        const string RUB = "RUB";
        const string AUD = "AUD";
        const string CHF = "CHF";
        const string KWD = "KWD";
        const string KRW = "KRW";
        

        public ConversionUnitTest()
        {
            PrepareCurrenciesToTest();

            currencyConverter = new CurrencyConverter();
            currencyConverter.UpdateConfiguration(conversionRates);

            allCurrencies = currencyConverter.GetAllCurrencies();
            allCurrenciesCount = allCurrencies.Count;
        }

        private void PrepareCurrenciesToTest()
        {
            conversionRates = new List<Tuple<string, string, double>> { };

            conversionRates.Add(new Tuple<string, string, double>(USD, EUR, 0.93));
            conversionRates.Add(new Tuple<string, string, double>(USD, CAD, 1.26));
            conversionRates.Add(new Tuple<string, string, double>(CAD, RUB, 50.30));
            conversionRates.Add(new Tuple<string, string, double>(CAD, GBR, 0.64));
            conversionRates.Add(new Tuple<string, string, double>(EUR, QAR, 3.90));
            conversionRates.Add(new Tuple<string, string, double>(AUD, CAD, 0.91));
            conversionRates.Add(new Tuple<string, string, double>(GBR, CHF, 1.20));
            conversionRates.Add(new Tuple<string, string, double>(CHF, CAD, 1.31));
        }

        [Fact]
        public void TestDirectCurrenciesSimpleTest()
        {
            double result;
            result = currencyConverter.Convert(USD, CAD, 1000);

            Assert.Equal(1260, result);
        }

        [Theory]
        [InlineData(USD, CAD, 1000, 1260)]
        [InlineData(USD, EUR, 100, 93)]
        [InlineData(CAD, USD, 1000, 793.650794)]
        [InlineData(EUR, QAR, 1000, 3900)]
        public void TestDirectCurrencies(string fromCurrency, string toCurrency, double amount, double expectedResult)
        {
            double result;
            result = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(USD, GBR, 1000, 806.4)]
        [InlineData(QAR, USD, 10000, 2757.099531)]
        [InlineData(RUB, AUD, 1000, 21.84694)]
        [InlineData(RUB, GBR, 10000, 127.236581)]
        public void TestIndirectCurrenciesWithLengthTwo(string fromCurrency, string toCurrency, double amount, double expectedResult)
        {
            double result;
            result = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(EUR, CHF, 1000, 1034.228023)]
        [InlineData(QAR, CAD, 1000, 347.394541)]
        public void TestIndirectCurrenciesWithLengthThree(string fromCurrency, string toCurrency, double amount, double expectedResult)
        {
            double result;
            result = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(AUD, QAR, 1000, 2619.5)]
        [InlineData(QAR, CHF, 1000, 265.186672)]
        public void TestIndirectCurrenciesWithLengthFour(string fromCurrency, string toCurrency, double amount, double expectedResult)
        {
            double result;
            result = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(KWD, CAD, 1000, double.NaN)]
        [InlineData(USD, KWD, 100, double.NaN)]
        [InlineData(KRW, USD, 1000, double.NaN)]
        [InlineData(EUR, KWD, 1000, double.NaN)]
        [InlineData(KRW, KWD, 1000, double.NaN)]
        public void TestCurrenciesNoWay(string fromCurrency, string toCurrency, double amount, double expectedResult)
        {
            double result;
            result = currencyConverter.Convert(fromCurrency, toCurrency, amount);

            Assert.Equal(expectedResult, result);
        }
    }
}
