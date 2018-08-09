using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.MenuItems
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            MenuItemVM = new MenuItemViewModel
            {
                MenuItem = new MenuItem(),
                FoodTypes = _context.FoodTypes.ToList(),
                CategoryTypes = _context.CategoryTypes.ToList()
            };

            return Page();
        }

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MenuItems.Add(MenuItemVM.MenuItem);
            await _context.SaveChangesAsync();

            //Image Being Saved

            string webRootPath = _environment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb = _context.MenuItems.Find(MenuItemVM.MenuItem.Id);

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."),
                    files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
                //var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, menuItemFromDb.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                menuItemFromDb.Image = @"\images\" + menuItemFromDb.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + menuItemFromDb.Id + ".png");
                menuItemFromDb.Image = @"\images\" + menuItemFromDb.Id + ".png";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}