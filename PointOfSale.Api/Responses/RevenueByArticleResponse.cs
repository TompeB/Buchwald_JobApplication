using PointOfSale.Shared.Dto;

namespace PointOfSale.Api.Responses;

/// <summary>
/// A wrapper for the revenue by article statistic
/// </summary>
public class RevenueByArticleResponse
{
    /// <summary>
    /// Revenue y article constructor
    /// </summary>
    /// <param name="revenueByArticle">revenue by article</param>
    public RevenueByArticleResponse(IEnumerable<SaleDto> revenueByArticle)
    {
        RevenueByArticle = revenueByArticle;
    }

    /// <summary>
    /// The total revenue by articles
    /// </summary>
    public IEnumerable<SaleDto> RevenueByArticle { get; set; }
}
