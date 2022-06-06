using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace CurrencyConversion.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private IConfiguration _configuration;
        private static IEnumerable<Tuple<string, string, double>> _conversionRates;
        private static Dictionary<string, List<string>> _graph;
        private static string[] AdjacentMatrix_Names;
        private static double[,] AdjacentMatrix;
        private static int graphCount;

        /// <summary> 
        /// Clears any prior configuration. 
        /// </summary> 
        public void ClearConfiguration()
        {
            _conversionRates = null;
            _graph = null;
            AdjacentMatrix_Names = null;
            AdjacentMatrix = null;
        }

        /// <summary>  
        /// Updates the configuration. Rates are inserted or replaced internally. 
        /// </summary> 
        /// 
        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            if (AdjacentMatrix is null)
            {
                _conversionRates = conversionRates;
                InitGraph();
                InitAdjacentMatrix();
                CalculateAllRates();
            }
        }

        /// <summary> 
        /// Converts the specified amount to the desired currency. 
        /// </summary> 
        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            double result, rate;

            int pos_i = -1, pos_j = -1;

            pos_i = GetArrayIndex(fromCurrency);
            pos_j = GetArrayIndex(toCurrency);
 
            if (pos_i == -1 || pos_j == -1)
                return double.NaN;

            rate = AdjacentMatrix[pos_i, pos_j];

            if (rate is double.NaN)
                return double.NaN;

            result = Math.Round(amount * rate, 6);

            return result;
        }

        /// <summary> 
        /// It initializes the graph of currencies.
        /// </summary> 
        private void InitGraph()
        {
            if (_graph == null)
            {
                _graph = new Dictionary<string, List<string>>();
                foreach (var conversionRate in _conversionRates)
                {
                    if (!_graph.ContainsKey(conversionRate.Item1))
                        _graph[conversionRate.Item1] = new List<string>();
                    if (!_graph.ContainsKey(conversionRate.Item2))
                        _graph[conversionRate.Item2] = new List<string>();

                    _graph[conversionRate.Item1].Add(conversionRate.Item2);
                    _graph[conversionRate.Item2].Add(conversionRate.Item1);
                }
            }

            graphCount = _graph.Count;
        }

        /// <summary> 
        /// It initializes the adjacent matrix of currencies.
        /// </summary> 
        private void InitAdjacentMatrix()
        {
            int k = 0;
            AdjacentMatrix_Names = new string[graphCount];
            AdjacentMatrix = new double[graphCount, graphCount];

            foreach (var item in _graph)
            {
                AdjacentMatrix_Names[k++] = item.Key;
            }

            for (int i = 0; i < graphCount; i++)
            {
                for (int j = 0; j < graphCount; j++)
                {
                    AdjacentMatrix[i, j] = GetDirectRate(AdjacentMatrix_Names[i], AdjacentMatrix_Names[j]);
                }
                AdjacentMatrix[i, i] = 1;
            }

        }

        /// <summary> 
        /// In a nested loop, it tries to examin all the adjacent matrix for probable rate by exploiting Relax method.
        /// </summary> 
        private void CalculateAllRates()
        {
            for (int i = 0; i < graphCount; i++)
            {
                for (int j = 0; j < graphCount; j++)
                {
                    if (AdjacentMatrix[i, j] is double.NaN)
                        AdjacentMatrix[i, j] = Relax(i, j);
                }
            }
        }

        /// <summary> 
        /// It investigates the adjacent matrix to calculate rate according to existing adjacent rates.
        /// </summary> 
        private double Relax(int i, int j)
        {
            double result = double.MaxValue;

            for (int x = 0; x < graphCount; x++)
            {
                if (i != x &&
                       AdjacentMatrix[i, x] is not double.NaN &&
                       AdjacentMatrix[x, j] is not double.NaN)
                {
                    if (result > AdjacentMatrix[i, x] * AdjacentMatrix[x, j])
                        result = AdjacentMatrix[i, x] * AdjacentMatrix[x, j];
                }

            }

            if (result is double.MaxValue)
                result = double.NaN;

            return result; 
        }

        /// <summary> 
        /// If there is a direct way for converting two currencies, it returns their rate. 
        /// </summary> 
        public double GetDirectRate(string fromCurr, string toCurr)
        {
            double finalRate = 0;

            if (_graph[fromCurr].Contains(toCurr))
            {
                var rate = _conversionRates.SingleOrDefault(fr => fr.Item1 == fromCurr && fr.Item2 == toCurr);
                var rate_i = _conversionRates.SingleOrDefault(fr => fr.Item1 == toCurr && fr.Item2 == fromCurr);

                if (rate == null)
                    finalRate = 1 / rate_i.Item3;
                else
                    finalRate = rate.Item3;
            }
            else
                finalRate = double.NaN;

            return finalRate;
        }

        /// <summary> 
        /// Returns index of array for currency names. 
        /// </summary> 
        private int GetArrayIndex(string itemName)
        {
            int pos = -1;
            for (int i = 0; i < graphCount; i++)
            {
                if (AdjacentMatrix_Names[i] == itemName)
                    pos = i;
            }

            return pos;
        }

        /// <summary> 
        /// Returns list of all currencies. 
        /// </summary> 
        public List<string> GetAllCurrencies()
        {
            return AdjacentMatrix_Names.ToList<string>();
        }
    }
}
