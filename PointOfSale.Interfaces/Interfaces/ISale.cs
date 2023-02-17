namespace PointOfSale.Shared.Interfaces;
public interface ISale
{
    /// <summary>
    /// The article number of the sold article. The max length lenght is 32
    /// </summary>
    string? ArticleNumber { get; set; }
    /// <summary>
    /// The sales price of the sold article in EUR
    /// </summary>
    decimal? SalesPrice { get; set; }
}