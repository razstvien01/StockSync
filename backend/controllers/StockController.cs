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
        private readonly IStockRepository _stockRepository;
        public StockController(AppDBContext context, IStockRepository stockRepository)
        {
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
            var stock = await _stockRepository.FindStockAsync(id);
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
            var existingStock = await _stockRepository.CreateStockAsync(stockDto);

            if (existingStock != null)
            {
                return Conflict(new { message = "Stock already exists" });
            }

            return CreatedAtAction(nameof(GetStockById), new { id = stockDto.Id }, stockDto.ToStockDto());

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequestDto request)
        {
            var stock = await _stockRepository.UpdateStockAsync(id, request);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stock = await _stockRepository.DeleteStockAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Accepted(new { message = "Stock deleted successfully", Id = id });
        }
    }
}