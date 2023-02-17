using PointOfSale.Shared.Exceptions;
using PointOfSale.Shared.Interfaces;

namespace PointOfSale.Domain.Models;

/// <inheritdoc/>
public class SaleBlo : ISale
{
    /// <inheritdoc/>
    public string? ArticleNumber { get; set; }
    /// <inheritdoc/>
    public decimal? SalesPrice { get; set; }

    /// <summary>
    /// Validates if the sale has an article number and a price
    /// </summary>
    /// <exception cref="ValidationException">Is thrown when the ArticleNumber is empty or the price is null.</exception>
    public void ValidateSale()
    {
        if (string.IsNullOrWhiteSpace(ArticleNumber))
        {
            throw new ValidationException(this);
        }

        if (SalesPrice == null)
        {
            throw new ValidationException(this);
        }
    }
}