using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages.MenuItems
{
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