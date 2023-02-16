using NUnit.Framework;
using PointOfSale.Shared.Exceptions;

namespace PointOfSale.Domain.Models.Tests
{
    [TestFixture]
    public class SaleBloTest
    {
        [Test]
        public void ValidateSaleTest_GoodCase()
        {
            var payload = new SaleBlo
            {
                ArticleNumber = "GoodCase",
                SalesPrice = 10
            };

            Assert.DoesNotThrow(() => payload.ValidateSale());
        }

        [Test]
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("ArticlNumber", null)]
        public void ValidateSaleTest_BadCase(string? articleNumber, decimal? price)
        {
            var payload = new SaleBlo
            {
                ArticleNumber = articleNumber,
                SalesPrice = price
            };

            var exception = Assert.Throws<ValidationException>(() => payload.ValidateSale());
        }
    }
}