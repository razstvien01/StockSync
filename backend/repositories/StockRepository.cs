using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.dtos.Stock;
using backend.helpers;
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
            return null;
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

        public async Task<Stock?> FindStockWithCommentsAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return null;
            }

            return stock;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObjects query)
        {
            var stocks = _context.Stocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }

            int skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<List<Stock>> GetAllStocksWithCommentsAsync(QueryObjects query)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }

            int skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto request)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
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