namespace PointOfSale.Infrastructure.Responses;
public class RevenueByDayResponse
{
    public RevenueByDayResponse(Dictionary<DateTime, decimal?> revenueByDay)
    {
        RevenueByDay = revenueByDay;
    }
    public Dictionary<DateTime, decimal?> RevenueByDay { get; set; }
}
