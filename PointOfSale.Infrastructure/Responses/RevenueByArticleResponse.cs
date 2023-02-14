using PointOfSale.Shared.Dto;

namespace PointOfSale.Infrastructure.Responses;
public class RevenueByArticleResponse
{
    public RevenueByArticleResponse(IEnumerable<SaleDto> revenueByArticle)
    {
        RevenueByArticle = revenueByArticle;
    }
    public IEnumerable<SaleDto> RevenueByArticle { get; set; }
}
