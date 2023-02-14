using PointOfSale.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Shared.Dto
{
    public class SaleDto : ISale
    {
        /// <summary>
        /// The article number of the sold article
        /// </summary>
        [MaxLength(32)]
        public string? ArticleNumber { get; set; }
        
        /// <summary>
        /// The sales price of the sold article in EUR
        /// </summary>
        public decimal? SalesPrice { get; set; }
    }
}
