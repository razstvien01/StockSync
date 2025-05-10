using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol must be at least 1 character long.")]
        [MaxLength(10, ErrorMessage = "Symbol must be at most 10 characters long.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(1, ErrorMessage = "Company name must be at least 1 character long.")]
        [MaxLength(50, ErrorMessage = "Company name must be at most 50 characters long.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000, ErrorMessage = "Price must be between 1 and 1,000,000,000.")]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100, ErrorMessage = "Dividend must be between 0.001 and 100.")]
        public decimal LastDividend { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Industry must be at least 1 character long.")]
        [MaxLength(10, ErrorMessage = "Industry must be at most 10 characters long.")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 5000000000000, ErrorMessage = "Market cap must be between 1 and 5,000,000,000,000.")]
        public long MarketCap { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}