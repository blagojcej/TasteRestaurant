using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderPickupDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderPickupDetailsModel(ApplicationDbContext context)
        {
            _context = context;
            OrderDetailsViewModel = new OrderDetailsViewModel();
        }

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public void OnGet(int orderId)
        {
            OrderDetailsViewModel.OrderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
            OrderDetailsViewModel.OrderHeader.ApplicationUser = _context.Users
                .Where(u => u.Id == OrderDetailsViewModel.OrderHeader.UserId).FirstOrDefault();
            OrderDetailsViewModel.OrderDetails = _context.OrderDetails
                .Where(o => o.OrderId == OrderDetailsViewModel.OrderHeader.Id).ToList();
        }

        public IActionResult OnPost(int orderId)
        {
            OrderHeader orderHeader = _context.OrderHeaders.Find(orderId);
            orderHeader.Status = SD.StatusCompleted;

            _context.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }
    }
}