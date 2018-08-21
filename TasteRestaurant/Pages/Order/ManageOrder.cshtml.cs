using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class ManageOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageOrderModel(ApplicationDbContext context)
        {
            _context = context;
            OrderDetailsViewModel = new List<OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet()
        {
            List<OrderHeader> OrderHeaderList = _context.OrderHeaders
                .Where(u => u.Status != SD.StatusCompleted && u.Status != SD.StatusReady &&
                            u.Status != SD.StatusCancelled).OrderByDescending(u => u.PickUpTime).ToList();

            foreach (OrderHeader header in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel();
                individual.OrderDetails = _context.OrderDetails.Where(o => o.OrderId == header.Id).ToList();
                individual.OrderHeader = header;

                OrderDetailsViewModel.Add(individual);
            }
        }

        public IActionResult OnPostOrderPrepare(int orderId)
        {
            OrderHeader orderHeader = _context.OrderHeaders.Find(orderId);
            orderHeader.Status = SD.StatusInProcess;

            _context.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }

        public IActionResult OnPostOrderReady(int orderId)
        {
            OrderHeader orderHeader = _context.OrderHeaders.Find(orderId);
            orderHeader.Status = SD.StatusReady;

            _context.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            OrderHeader orderHeader = _context.OrderHeaders.Find(orderId);
            orderHeader.Status = SD.StatusCancelled;

            _context.SaveChanges();

            return RedirectToPage("/Order/ManageOrder");
        }
    }
}