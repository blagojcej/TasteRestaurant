using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages
{
    //[Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [TempData]
        public string StatusMessage { get; set; }

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingCart CartObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            var MenuItemFromDb = _context.MenuItems.Include(c => c.CategoryType)
                .Include(f => f.FoodType)
                .FirstOrDefault(x => x.Id == id);

            CartObj = new ShoppingCart()
            {
                MenuItemId = MenuItemFromDb.Id,
                MenuItem = MenuItemFromDb
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity) this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                CartObj.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb =
                    _context.ShoppingCarts
                        .FirstOrDefault(c => c.ApplicationUserId == CartObj.ApplicationUserId && c.MenuItemId == CartObj.MenuItemId);

                if (cartFromDb == null)
                {
                    _context.ShoppingCarts.Add(CartObj);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObj.Count;
                }

                _context.SaveChanges();

                //Add Session and increment count
                var count = _context.ShoppingCarts.Where(c => c.ApplicationUserId == CartObj.ApplicationUserId)
                    .ToList().Count();
                HttpContext.Session.SetInt32("CartCount", count);
                StatusMessage = "Item Added to Cart";
                return RedirectToPage("Index");
            }
            else
            {
                var MenuItemFromDb = _context.MenuItems.Include(c => c.CategoryType).Include(f => f.FoodType)
                    .FirstOrDefault(m => m.Id == CartObj.MenuItemId);

                CartObj = new ShoppingCart()
                {
                    MenuItemId = MenuItemFromDb.Id,
                    MenuItem = MenuItemFromDb
                };

                return Page();
            }
        }
    }
}