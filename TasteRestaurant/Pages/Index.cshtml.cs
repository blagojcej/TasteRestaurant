using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IndexViewModel IndexVM { get; set; }

        public async Task OnGetAsync()
        {
            IndexVM = new IndexViewModel()
            {
                MenuItems = await _context.MenuItems.Include(c => c.CategoryType).Include(f => f.FoodType).ToListAsync(),
                CategoryTypes = await _context.CategoryTypes.OrderBy(c => c.DisplayOrder).ToListAsync()
            };
        }
    }
}
