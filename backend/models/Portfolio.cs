using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.models
{
    [Table("Portfolio")]
    public class Portfolio
    {
        public required string AppUserId { get; set; }
        public int StockId { get; set; }
        public required AppUser AppUser { get; set; }
        public required Stock Stock { get; set; }
    }
}