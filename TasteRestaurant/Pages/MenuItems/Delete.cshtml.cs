using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages.MenuItems
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public DeleteModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItem = await _context.MenuItems.Include(c => c.CategoryType).Include(f => f.FoodType)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string webRootPath = _environment.WebRootPath;
            MenuItem = await _context.MenuItems.FindAsync(id);

            if (MenuItem!=null)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = MenuItem.Image.Substring(MenuItem.Image.LastIndexOf("."),
                    MenuItem.Image.Length - MenuItem.Image.LastIndexOf("."));
                var ImagePath = Path.Combine(uploads, MenuItem.Id + extension);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }

                _context.MenuItems.Remove(MenuItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");

        }
    }
}