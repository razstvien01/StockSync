using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.models;

namespace backend.interfaces
{
    public interface IFMPService
    {
        Task<Stock?> FindStockBySymbolAsync(string symbol);
    }
}