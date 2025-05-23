using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public required string AppUserId { get; set; }
        public int StockId { get; set; }
        public AppUser? AppUser { get; set; }
        public Stock? Stock { get; set; }
    }
}