using PointOfSale.Shared.Exceptions;
using PointOfSale.Shared.Interfaces;

namespace PointOfSale.Domain.Models;
public class SaleBlo : ISale
{
    public string? ArticleNumber { get; set; }
    public decimal? SalesPrice { get; set; }

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