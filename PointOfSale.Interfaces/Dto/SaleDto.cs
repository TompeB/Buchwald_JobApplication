using System.ComponentModel.DataAnnotations;
using PointOfSale.Shared.Interfaces;

namespace PointOfSale.Shared.Dto
{
    public class SaleDto : ISale
    {
        /// <inheritdoc/>
        [MaxLength(32)]
        public string? ArticleNumber { get; set; }

        /// <inheritdoc/>
        public decimal? SalesPrice { get; set; }
    }
}
