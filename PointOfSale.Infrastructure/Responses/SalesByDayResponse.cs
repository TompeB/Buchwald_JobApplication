namespace PointOfSale.Infrastructure.Responses;
public class SalesByDayResponse
{
    public SalesByDayResponse(Dictionary<DateTime, int> salesByDay)
    {
        SalesByDay = salesByDay;
    }
    public Dictionary<DateTime, int> SalesByDay { get; set; }
}
