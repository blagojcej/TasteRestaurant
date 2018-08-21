using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.MenuItems
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public EditModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM = new MenuItemViewModel()
            {
                MenuItem = _context.MenuItems.Include(c => c.CategoryType).Include(f => f.FoodType)
                    .SingleOrDefault(m => m.Id == id),
                CategoryTypes = _context.CategoryTypes.ToList(),
                FoodTypes = _context.FoodTypes.ToList()
            };

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string webRootPath = _environment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _context.MenuItems.Where(m => m.Id == MenuItemVM.MenuItem.Id).FirstOrDefaultAsync();

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                //After uploading image, this code doesn't remove old *.png image
                //var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                //files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
                var extension = menuItemFromDb.Image.Substring(menuItemFromDb.Image.LastIndexOf("."),
                    menuItemFromDb.Image.Length - menuItemFromDb.Image.LastIndexOf("."));
                //var extension = Path.GetExtension(files[0].FileName);

                if (System.IO.File.Exists(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension));
                }

                //After uploading image, this code doesn't remove old *.png image
                extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MenuItemVM.MenuItem.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }


            if (MenuItemVM.MenuItem.Image != null)
                menuItemFromDb.Image = MenuItemVM.MenuItem.Image;

            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.FoodTypeId = MenuItemVM.MenuItem.FoodTypeId;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}