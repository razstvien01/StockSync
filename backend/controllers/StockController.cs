using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.data;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    public class StockController: ControllerBase
    {
        private readonly AppDBContext _context;
        public StockController(AppDBContext context)
        {
            _context = context;
        }
    }
}