using CurrencyConversion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CurrencyConversion.Services;
using CurrencyConversion.Models.ViewModels;
using CurrencyConversion.Dto;

namespace CurrencyConversion.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;

        public HomeController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        public IActionResult Index()
        {
            CurrencyConfigJsonFile currencyConfigJsonFile = new CurrencyConfigJsonFile(_configuration);

            IEnumerable<Tuple<string, string, double>> conversionRates;
            conversionRates = currencyConfigJsonFile.InitConfigJsonFile();

            CurrencyConverter currencyConverter = new CurrencyConverter();

            // Clear Configuration to test Singleton Pattern
            currencyConverter.ClearConfiguration();

            // In this case, class should calculate the Rates Matrix again
            currencyConverter.UpdateConfiguration(conversionRates);

            PageViewModel pageViewModel = new()
            {
                Currencies = currencyConverter.GetAllCurrencies(),
            };

            return View(pageViewModel);
        }
               
        public IActionResult CalcRate(PageDto pageDto)
        {
            double calculatedAmount;
            CurrencyConverter currencyConverter = new CurrencyConverter();

            calculatedAmount = currencyConverter.Convert(pageDto.FromCurrency, pageDto.ToCurrency, pageDto.Amount);

            PageViewModel pageViewModel = new()
            {
                Currencies = currencyConverter.GetAllCurrencies(),
                FromCurrency = pageDto.FromCurrency,
                ToCurrency = pageDto.ToCurrency,
                Amount = pageDto.Amount,
                CalculatedAmount = calculatedAmount
            };

            return View("Index", pageViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier , 
                Message = "Error Message!" 
            });
        }
    }
}
