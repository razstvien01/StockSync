using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.dtos.Comment;

namespace backend.dtos.Stock
{
    public class StockWCDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string  CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDividend { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}