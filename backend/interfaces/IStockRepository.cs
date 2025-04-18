using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Stock;
using backend.helpers;
using backend.models;

namespace backend.interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync(QueryObjects query);
        Task<List<Stock>> GetAllStocksWithCommentsAsync(QueryObjects query);
        Task<Stock?> FindStockAsync(int id);
        Task<Stock?> FindStockWithCommentsAsync(int id);
        Task<Stock?> CreateStockAsync(Stock stockDto);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto request);
        Task<Stock?> DeleteStockAsync(int id);
        Task<bool> StockExistsAsync(int id);
    }
}