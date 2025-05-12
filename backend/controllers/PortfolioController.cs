using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Portfolio;
using backend.extensions;
using backend.interfaces;
using backend.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username!);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser!);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromBody] CreatePortfolioRequestDto requestDto)
        {
            var symbol = requestDto.Symbol;
            var username = User.GetUserName();
            
            if(string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required");
                
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            
            if(stock == null)
                return NotFound("Stock not found");
            
            if(appUser == null)
                return NotFound("User not found");
                
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            
            if(userPortfolio.ToList().Any(e => e.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)))
                return BadRequest("Stock already exists in the portfolio");
                
            Portfolio portfolioModel = new Portfolio{
                AppUserId = appUser.Id,
                StockId = stock.Id
            };
            
            await _portfolioRepository.CreateAsync(portfolioModel);
            
            return Created();
        }
    }
}