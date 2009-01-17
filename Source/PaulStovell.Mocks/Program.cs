using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaulStovell.Mocks.MockGeneration;

namespace PaulStovell.Mocks
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            // Setup the expectatons of the mock GST rate provider
            MockRepository repository = new MockRepository();
            IGstRateProvider provider = repository.CreateMock<IGstRateProvider>();
            provider.GetGstRate("Bread").WillReturn(0.00M);
            provider.GetGstRate("Milk").WillReturn(0.00M);
            provider.DefaultGstRate.WillReturn(0.10M);
            repository.ReplayAll();

            // Test the GST Calculator using the mock provider (instead of, say, the database 
            // GST rate provider it would use in production).
            GstCalculator calculator = new GstCalculator(provider);
            Assert.AreEqual(2.57M, calculator.ApplyGst("Bread", 2.57M), "Bread should be exempt from GST");
            Assert.AreEqual(4.00M, calculator.ApplyGst("Milk", 4.00M), "Milk should be exempt from GST");
            Assert.AreEqual(2200M, calculator.ApplyGst("Laptop", 2000M), "Laptops should not be exempt from GST");
            Assert.AreEqual(26400M, calculator.ApplyGst("Car", 24000M), "Cars should not be exempt from GST");
            repository.VerifyAll();
            Console.ReadKey();
        }
    }

    /// <summary>
    /// An example class that will be mocked.
    /// </summary>
    public interface IGstRateProvider
    {
        decimal? GetGstRate(string productType);
        decimal DefaultGstRate { get; }
    }

    /// <summary>
    /// An example class that will be unit tested.
    /// </summary>
    internal class GstCalculator
    {
        private IGstRateProvider _rateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GstCalculator"/> class.
        /// </summary>
        /// <param name="rateProvider">The rate provider.</param>
        public GstCalculator(IGstRateProvider rateProvider)
        {
            _rateProvider = rateProvider;
        }

        /// <summary>
        /// Applies the GST.
        /// </summary>
        public decimal ApplyGst(string product, decimal grossPrice)
        {
            decimal gstRate = _rateProvider.GetGstRate(product) ?? _rateProvider.DefaultGstRate;
            return grossPrice * (1.00M + gstRate);
        }
    }
}
