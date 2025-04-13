using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Stock;
using backend.models;

namespace backend.mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock dto)
        {
            return new StockDto
            {
                Id = dto.Id,
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDividend = dto.LastDividend,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap,
                CreatedAt = dto.CreatedAt,
            };
        }
        
        public static StockWCDto ToStockWCDto(this Stock dto)
        {
            return new StockWCDto
            {
                Id = dto.Id,
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDividend = dto.LastDividend,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap,
                CreatedAt = dto.CreatedAt,
                Comments = dto.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
        
        public static Stock ToStockFromCreateDto(this CreateStockRequestDto dto){
            return new Stock{
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDividend = dto.LastDividend,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap,
                CreatedAt = dto.CreatedAt
            };
        }
    }
}