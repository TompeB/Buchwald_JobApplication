using Microsoft.EntityFrameworkCore;
using PointOfSale.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Infrastructure.Entities;

[Index(nameof(Id))]
public class Sale : ISale
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string? ArticleNumber { get; set; }
    public decimal? SalesPrice { get; set; }
    public DateTimeOffset TimeStampCreated { get; set; } = DateTimeOffset.Now;
}