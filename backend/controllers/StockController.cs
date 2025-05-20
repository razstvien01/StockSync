using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using backend.dtos.Stock;
using backend.helpers;
using backend.interfaces;
using backend.mappers;
using Microsoft.AspNetCore.Mvc;
namespace backend.controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObjects query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = await _stockRepository.GetAllStocksAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDto);
        }

        [HttpGet("stockwc")]
        public async Task<IActionResult> GrtAllStocksWithComments([FromQuery] QueryObjects query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks = await _stockRepository.GetAllStocksWithCommentsAsync(query);
            var stockDto = stocks.Select(s => s.ToStockWCDto());

            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepository.FindStockAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            var stockDto = stock.ToStockDto();

            return Ok(stockDto);
        }

        [HttpGet("stockwc/{id:int}")]
        public async Task<IActionResult> GetStockWithCommentsById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepository.FindStockWithCommentsAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            var stockDto = stock.ToStockWCDto();

            return Ok(stockDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockDto = request.ToStockFromCreateDto();
            var existingStock = await _stockRepository.CreateStockAsync(stockDto);

            if (existingStock != null)
            {
                return Conflict(new { message = "Stock already exists" });
            }

            return CreatedAtAction(nameof(GetStockById), new { id = stockDto.Id }, stockDto.ToStockDto());

        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepository.UpdateStockAsync(id, request);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepository.DeleteStockAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Accepted(new { message = "Stock deleted successfully", Id = id });
        }
    }
}