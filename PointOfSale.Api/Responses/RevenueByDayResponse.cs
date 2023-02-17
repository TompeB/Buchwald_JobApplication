namespace PointOfSale.Api.Responses;

/// <summary>
/// A wrapper for the revenue by day report
/// </summary>
public class RevenueByDayResponse
{
    /// <summary>
    /// Revenue by day constructor
    /// </summary>
    /// <param name="revenueByDay"></param>
    public RevenueByDayResponse(Dictionary<DateTime, decimal?> revenueByDay)
    {
        RevenueByDay = revenueByDay;
    }

    /// <summary>
    /// Revenue by day
    /// </summary>
    public Dictionary<DateTime, decimal?> RevenueByDay { get; set; }
}