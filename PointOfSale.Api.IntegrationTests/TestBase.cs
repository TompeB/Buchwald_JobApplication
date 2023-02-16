using NUnit.Framework;

namespace PointOfSale.IntegrationTests;
public class TestBase
{
    [OneTimeSetUp]
    public async Task Setup()
    {
        HostingContext.StartUpHost();
    }
}