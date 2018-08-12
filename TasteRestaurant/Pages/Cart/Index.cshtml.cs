using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderDetailsCart DetailsCart { get; set; }

        public void OnGet()
        {
            DetailsCart = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader()
            };
            DetailsCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            //Get shopping cart from logged user
            var cart = _context.ShoppingCarts.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                DetailsCart.ListCart = cart.ToList();
            }

            foreach (var cartItem in DetailsCart.ListCart)
            {
                cartItem.MenuItem = _context.MenuItems.FirstOrDefault(m => m.Id == cartItem.MenuItemId);
                DetailsCart.OrderHeader.OrderTotal =
                    DetailsCart.OrderHeader.OrderTotal + (cartItem.MenuItem.Price * cartItem.Count);
                if (cartItem.MenuItem.Description.Length > 100)
                {
                    cartItem.MenuItem.Description = cartItem.MenuItem.Description.Substring(0, 99) + "...";
                }

                DetailsCart.OrderHeader.PickUpTime = DateTime.Now;
            }
        }

        public async Task<IActionResult> OnPostPlus(int cartId)
        {
            var cart = await _context.ShoppingCarts.Where(c => c.Id == cartId).FirstOrDefaultAsync();
            cart.Count += 1;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _context.ShoppingCarts.Remove(cart);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("CartCount",
                    _context.ShoppingCarts.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count());
            }
            else
            {
                cart.Count -= 1;
                _context.SaveChanges();
            }

            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            DetailsCart.ListCart = _context.ShoppingCarts.Where(c => c.ApplicationUserId == claim.Value).ToList();

            OrderHeader orderHeader = DetailsCart.OrderHeader;
            DetailsCart.OrderHeader.OrderDate=DateTime.Now;
            DetailsCart.OrderHeader.UserId=claim.Value;
            DetailsCart.OrderHeader.Status=SD.StatusSubmitted;

            _context.OrderHeaders.Add(orderHeader);
            _context.SaveChanges();

            foreach (var item in DetailsCart.ListCart)
            {
                item.MenuItem = _context.MenuItems.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetail orderDetail=new OrderDetail()
                {
                    MenuItemId=item.MenuItemId,
                    OrderId = orderHeader.Id,
                    Name = item.MenuItem.Name,
                    Description = item.MenuItem.Description,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };

                _context.OrderDetails.Add(orderDetail);
            }

            //Remove ordered items from shopping cart
            _context.ShoppingCarts.RemoveRange(DetailsCart.ListCart);

            //Update/Reset session shopping cart (navbar)
            HttpContext.Session.SetInt32("CartCount", 0);

            _context.SaveChanges();

            return RedirectToPage("../Index");
        }
    }
}