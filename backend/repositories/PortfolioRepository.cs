using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using backend.data;
using backend.interfaces;
using backend.models;

namespace backend.repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDBContext _context;
        public PortfolioRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolio(AppUser user, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUser!.Id == user.Id && x.Stock!.Symbol.ToLower() == symbol.ToLower());

            if (portfolioModel == null)
                return null;

            _context.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
                .Select(p => new Stock
                {
                    Id = p.StockId,
                    Symbol = p.Stock!.Symbol,
                    CompanyName = p.Stock.CompanyName,
                    Purchase = p.Stock.Purchase,
                    LastDividend = p.Stock.LastDividend,
                    Industry = p.Stock.Industry,
                    MarketCap = p.Stock.MarketCap
                }).ToListAsync();
        }
    }
}