using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderHistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsVM { get; set; }

        //if id=0 display only 5 past orders else display all the orders
        public void OnGet(int id = 0)
        {
            //Get the user to find order for that user, not all orders
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            OrderDetailsVM = new List<OrderDetailsViewModel>();

            List<OrderHeader> OrderHeaderList = _context.OrderHeaders.Where(u => u.UserId == claim.Value)
                .OrderByDescending(u => u.OrderDate).ToList();

            if (id == 0 && OrderHeaderList.Count > 4)
            {
                OrderHeaderList = OrderHeaderList.Take(5).ToList();
            }

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel();
                individual.OrderHeader = item;
                individual.OrderDetails = _context.OrderDetails.Where(o => o.Id == item.Id).ToList();

                OrderDetailsVM.Add(individual);
            }
        }
    }
}