using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.dtos.Stock;
using backend.interfaces;
using backend.mappers;
using backend.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IStockRepository _stockRepository;
        public StockController(AppDBContext context, IStockRepository stockRepository)
        {
            _context = context;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockRepository.GetAllStocksAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            var stockDto = stock.ToStockDto();

            return Ok(stockDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto request)
        {
            var stockDto = request.ToStockFromCreateDto();

            //* Check if that stock already exists
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == stockDto.Symbol);
            if (existingStock != null)
            {
                return Conflict(new { message = "Stock already exists" });
            }

            _context.Add(stockDto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStockById), new { id = stockDto.Id }, stockDto.ToStockDto());

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequestDto request)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            stock.Symbol = request.Symbol;
            stock.CompanyName = request.CompanyName;
            stock.Purchase = request.Purchase;
            stock.LastDividend = request.LastDividend;
            stock.Industry = request.Industry;
            stock.MarketCap = request.MarketCap;
            stock.CreatedAt = request.CreatedAt;

            _context.SaveChanges();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            _context.Comments.RemoveRange(stock.Comments);

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return Accepted(new { message = "Stock deleted successfully", Id = id });
        }
    }
}