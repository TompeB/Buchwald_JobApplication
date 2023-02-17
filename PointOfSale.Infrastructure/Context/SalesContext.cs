using PointOfSale.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace PointOfSale.Infrastructure.Context;
public class SalesContext : DbContext
{
    public SalesContext(DbContextOptions<SalesContext> options)
        : base(options)
    { }

    public DbSet<Sale> Sales { get; set; }
}