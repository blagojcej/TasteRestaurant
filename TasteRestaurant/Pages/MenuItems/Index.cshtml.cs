using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;

namespace TasteRestaurant.Pages.MenuItems
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> MenuItems { get; set; }

        public async Task OnGet()
        {
            MenuItems = await _context.MenuItems.Include(c => c.CategoryType).Include(f => f.FoodType).ToListAsync();
        }
    }
}