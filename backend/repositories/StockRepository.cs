using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.interfaces;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDBContext _context;
        public StockRepository(AppDBContext context)
        {
            _context = context;
        }
        public Task<List<Stock>> GetAllStocksAsync()
        {
            return _context.Stocks.ToListAsync();
        }
    }
}