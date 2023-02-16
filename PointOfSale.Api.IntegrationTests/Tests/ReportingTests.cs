using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PointOfSale.Infrastructure.Context;
using PointOfSale.Infrastructure.Entities;
using AutoFixture;
using System.Net.Http.Json;
using PointOfSale.Infrastructure.Responses;

namespace PointOfSale.IntegrationTests.Tests
{
    [TestFixture()]
    public class ReportingTests : TestBase
    {
        private Fixture _fixture;

        private const string SaleByDayString = "Reporting/SalesByDay";
        private const string RevenueByArticleString = "Statistics/RevenueByArticle";
        private const string RevenueByDayString = "Reporting/RevenueByDay";

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
        [TestCase(SaleByDayString)]
        [TestCase(RevenueByArticleString)]
        [TestCase(RevenueByDayString)]
        public async Task GetReporting_All_NoExceptionWhenTheDatabaseIsEmpty(string url)
        {
            // Act
            var response = await HostingContext.Client.GetAsync(url);

            // Assert
            Assert.DoesNotThrow(() => response.EnsureSuccessStatusCode());
        }

        [Test()]
        public async Task GetReporting_SalesByDay_SingleSaleIsReturnedSuccessfully()
        {
            //Arrange
            var salesContext = HostingContext.GetService<SalesContext>();
            var saleData = _fixture
                .Build<Sale>()
                .With(x => x.ArticleNumber, "Article")
                .With(x => x.TimeStampCreated, DateTimeOffset.Now)
                .Create();

            salesContext.Add(saleData);
            salesContext.SaveChanges();
            //Act
            var response = await GetAsync<SalesByDayResponse>(SaleByDayString);

            //Assert
            Assert.That(response.SalesByDay.Count, Is.EqualTo(1));
            var result = response.SalesByDay.First();
            Assert.That(result.Value, Is.EqualTo(1));
            Assert.That(result.Key.Date, Is.EqualTo(DateTime.Today));
        }

        [Test()]
        public async Task GetReporting_SalesByDay_MultipleValuesAreAggregatedCorretly()
        {
            //Arrange
            var salesContext = HostingContext.GetService<SalesContext>();
            var saleData = _fixture.CreateMany<KeyValuePair<string, DateTimeOffset>>(3);

            salesContext.AddRange(saleData
                .SelectMany(x => _fixture
                    .Build<Sale>()
                    .With(y => y.ArticleNumber, x.Key)
                    .With(y => y.TimeStampCreated, x.Value)
                    .CreateMany(3)));

            salesContext.SaveChanges();

            //Act
            var result = await GetAsync<SalesByDayResponse>(SaleByDayString);

            //Assert
            Assert.That(result.SalesByDay.Select(y => y.Key.Date), Is.EquivalentTo(saleData.Select(x => x.Value.Date)));
            Assert.That(result.SalesByDay.All(x => x.Value == 3));
        }

        [Test()]
        public async Task GetReporting_RevenueByDay_SingleSaleIsReturnedSuccessfully()
        {
            //Arrange
            var salesContext = HostingContext.GetService<SalesContext>();
            var price = 50;
            var saleData = _fixture
                .Build<Sale>()
                .With(x => x.SalesPrice, price)
                .With(x => x.TimeStampCreated, DateTimeOffset.Now)
                .Create();

            salesContext.Add(saleData);
            salesContext.SaveChanges();

            //Act
            var response = await GetAsync<RevenueByDayResponse>(RevenueByDayString);
                            

            //Assert
            Assert.That(response.RevenueByDay.Count, Is.EqualTo(1));
            var result = response.RevenueByDay.First();
            Assert.That(result.Value, Is.EqualTo(price));
            Assert.That(result.Key.Date, Is.EqualTo(DateTime.Today));
        }

        [Test()]
        public async Task GetReporting_RevenueByDay_MultipleValuesAreAggregatedCorretly()
        {
            //Arrange
            var salesContext = HostingContext.GetService<SalesContext>();
            var saleData = _fixture.CreateMany<KeyValuePair<decimal, DateTimeOffset>>(3);

            salesContext.AddRange(saleData
                .SelectMany(x => _fixture
                    .Build<Sale>()
                    .With(y => y.SalesPrice, x.Key)
                    .With(y => y.TimeStampCreated, x.Value)
                    .CreateMany(3)));

            salesContext.SaveChanges();

            //Act
            var result = await GetAsync<RevenueByDayResponse>(RevenueByDayString);

            //Assert
            Assert.That(result.RevenueByDay.Select(y => y.Key.Date), Is.EquivalentTo(saleData.Select(x => x.Value.Date)));
            
            //Assert that the revenue is calculated right
            foreach(var r in result.RevenueByDay)
            {
                Assert.That(r.Value / 3, Is.EqualTo(saleData.First(x => x.Value.Date == r.Key.Date).Key));
            }
        }

        [Test()]
        public async Task GetReporting_RevenueByArticle_SingleSaleIsReturnedSuccessfully()
        {
            //Arrange
            var articleName = "Article";
            var price = 50;
            var salesContext = HostingContext.GetService<SalesContext>();
            var saleData = _fixture
                .Build<Sale>()
                .With(x => x.ArticleNumber, articleName)
                .With(x => x.SalesPrice, price)
                .Create();

            salesContext.Add(saleData);
            salesContext.SaveChanges();

            //Act
            var response = await GetAsync<RevenueByArticleResponse>(RevenueByArticleString);

            //Assert
            Assert.That(response.RevenueByArticle.Count(), Is.EqualTo(1));
            var result = response.RevenueByArticle.First();
            Assert.That(result.ArticleNumber, Is.EqualTo(articleName));
            Assert.That(result.SalesPrice, Is.EqualTo(price));
        }

        [Test()]
        public async Task GetReporting_RevenueByArticle_MultipleValuesAreAggregatedCorretly()
        {
            //Arrange
            var salesContext = HostingContext.GetService<SalesContext>();
            var saleData = _fixture.CreateMany<KeyValuePair<string, decimal>>(3);
            var saleEventIterations = 5;

            salesContext.AddRange(saleData
                .SelectMany(x => _fixture
                    .Build<Sale>()
                    .With(y => y.ArticleNumber, x.Key)
                    .With(y => y.SalesPrice, x.Value)
                    .CreateMany(saleEventIterations)));

            salesContext.SaveChanges();

            //Act
            var result = await GetAsync<RevenueByArticleResponse>(RevenueByArticleString);

            //Assert
            Assert.That(result.RevenueByArticle.Select(y => y.ArticleNumber), Is.EquivalentTo(saleData.Select(x => x.Key)));

            //Assert that the revenue is calculated right
            foreach (var r in result.RevenueByArticle)
            {
                Assert.That(r.SalesPrice / saleEventIterations, Is.EqualTo(saleData.First(x => x.Key == r.ArticleNumber).Value));
            }
        }

        #region Helper

        private async Task<T> GetAsync<T>(string url)
            where T : class
        {
            var httpResult = await HostingContext.Client.GetAsync(url);

            Assert.True(httpResult.IsSuccessStatusCode);

            var result = await httpResult.Content.ReadFromJsonAsync<T>();
         
            return result;
        }

        #endregion Helper
    }
}
