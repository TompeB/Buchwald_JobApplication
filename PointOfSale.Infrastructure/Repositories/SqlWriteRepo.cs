using AutoMapper;
using PointOfSale.Infrastructure.Context;
using PointOfSale.Infrastructure.Entities;
using PointOfSale.Shared.Dto;
using PointOfSale.Shared.Interfaces.DataAccess;

namespace PointOfSale.Infrastructure.Repositories
{
    public class SqlWriteRepo : IWriteRepo
    {
        private readonly SalesContext _dbContext;
        private readonly IMapper _mapper;
        public SqlWriteRepo(SalesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task SaveSale(SaleDto sale)
        {
            await _dbContext.Sales.AddAsync(_mapper.Map<Sale>(sale));
            await _dbContext.SaveChangesAsync();
        }
    }
}
