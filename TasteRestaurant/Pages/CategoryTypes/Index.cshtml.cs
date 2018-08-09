using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages.CategoryTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CategoryType> CategoryTypes { get; set; }

        public async Task OnGet()
        {
            CategoryTypes = await _context.CategoryTypes.OrderBy(ct => ct.DisplayOrder).ToListAsync();
        }
    }
}