﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages.FoodTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FoodType> FoodType { get;set; }

        public async Task OnGetAsync()
        {
            FoodType = await _context.FoodTypes.ToListAsync();
        }
    }
}