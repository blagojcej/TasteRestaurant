using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsVM { get; set; }

        public void OnGet(int id)
        {
            //Get the user to find order for that user, not all orders
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            OrderDetailsVM = new OrderDetailsViewModel()
            {
                OrderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == id && o.UserId==claim.Value),
                OrderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList()
            };
        }
    }
}