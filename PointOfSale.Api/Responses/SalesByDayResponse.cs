namespace PointOfSale.Api.Responses;

/// <summary>
/// A wrapper for the sales by day report
/// </summary>
public class SalesByDayResponse
{

    /// <summary>
    /// Sales by day constructor
    /// </summary>
    /// <param name="salesByDay"></param>
    public SalesByDayResponse(Dictionary<DateTime, int> salesByDay)
    {
        SalesByDay = salesByDay;
    }

    /// <summary>
    /// Sales by day
    /// </summary>
    public Dictionary<DateTime, int> SalesByDay { get; set; }
}
