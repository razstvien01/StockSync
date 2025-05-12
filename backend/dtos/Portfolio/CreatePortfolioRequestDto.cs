using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.dtos.Portfolio
{
    public class CreatePortfolioRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must be 10 characters or less")]
        public string Symbol { get; set; } = string.Empty;
    }
}