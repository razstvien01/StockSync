using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Stock;
using backend.models;

namespace backend.interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock?> FindStockAsync(int id);
        Task<Stock?> CreateStockAsync(Stock stockDto);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto request);
        Task<Stock?> DeleteStockAsync(int id);        
    }
}