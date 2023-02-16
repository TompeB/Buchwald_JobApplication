using NUnit.Framework;
using PointOfSale.Infrastructure.Context;
using AutoFixture;
using System.Net.Http.Json;
using PointOfSale.Shared.Dto;
using System.Net;

namespace PointOfSale.IntegrationTests.Tests
{
    [TestFixture()]
    public class SalesTests : TestBase
    {
        private Fixture _fixture;

        private const string SaleEndpoint = "Sales";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            var salesContext = HostingContext.GetService<SalesContext>();
            salesContext.Sales.RemoveRange(salesContext.Sales.ToList());
            salesContext.SaveChanges();
        }

        [Test()]
        public async Task PostSale_GoodCase_RequestRespoinseAndDbAreEqual()
        {
            //Arrange 
            var salesContext = HostingContext.GetService<SalesContext>();
            var payload = _fixture.Create<SaleDto>();

            // Act
            var response = await HostingContext.Client.PostAsJsonAsync(SaleEndpoint, payload);

            // Assert
            Assert.DoesNotThrow(() => response.EnsureSuccessStatusCode());
            var result = await response.Content.ReadFromJsonAsync<SaleDto>();
            Assert.That(result.SalesPrice, Is.EqualTo(payload.SalesPrice));
            Assert.That(result.ArticleNumber, Is.EqualTo(payload.ArticleNumber));

            var sales = salesContext.Sales.ToList();
            Assert.That(sales.Count(), Is.EqualTo(1));
            Assert.That(sales.First().SalesPrice, Is.EqualTo(payload.SalesPrice));
            Assert.That(sales.First().ArticleNumber, Is.EqualTo(payload.ArticleNumber));
        }

        [Test()]
        [TestCase("")]
        [TestCase("ArticleNumberThatIsWayTooLongForTheTableInTheDatabase")]
        public async Task PostSale_BadCases_ValidationErrorIsReturned(string articleNumber)
        {
            //Arrange
            var sale = new SaleDto
            {
                ArticleNumber = articleNumber
            };

            //Act
            var response = await HostingContext.Client.PostAsJsonAsync(SaleEndpoint, sale);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
