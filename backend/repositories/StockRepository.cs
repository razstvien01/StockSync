using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.dtos.Stock;
using backend.interfaces;
using backend.mappers;
using backend.models;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<Stock?> CreateStockAsync(Stock stockDto)
        {
            string symbol = stockDto.Symbol;
            
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);

            if (existingStock != null)
            {
                return existingStock;
            }

            await _context.AddAsync(stockDto);
            await _context.SaveChangesAsync();
            return stockDto;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return null;
            }

            _context.Comments.RemoveRange(stock.Comments);
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> FindStockAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return null;
            }

            return stock;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto request)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if(stock == null){
                return null;
            }
            
            stock.Symbol = request.Symbol;
            stock.CompanyName = request.CompanyName;
            stock.Purchase = request.Purchase;
            stock.LastDividend = request.LastDividend;
            stock.Industry = request.Industry;
            stock.MarketCap = request.MarketCap;
            stock.CreatedAt = request.CreatedAt;
            
            await _context.SaveChangesAsync();
            
            return stock;
        }
    }
}