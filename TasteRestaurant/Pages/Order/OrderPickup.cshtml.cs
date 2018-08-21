using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderPickupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderPickupModel(ApplicationDbContext context)
        {
            _context = context;
            OrderDetailsViewModel = new List<OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet(string option = null, string search = null)
        {
            if (search != null)
            {
                var user = new ApplicationUser();
                List<OrderHeader> OrderHeaderList = new List<OrderHeader>();
                if (option == "order")
                {
                    OrderHeaderList = _context.OrderHeaders.Where(o => o.Id == Convert.ToInt32(search)).ToList();
                }
                else
                {
                    if (option == "email")
                    {
                        user = _context.Users.FirstOrDefault(u => u.Email.ToLower().Contains(search.ToLower()));
                    }
                    else
                    {
                        if (option == "phone")
                        {
                            user = _context.Users.FirstOrDefault(u => u.PhoneNumber.ToLower().Contains(search.ToLower()));
                        }
                        else
                        {
                            if (option == "name")
                            {
                                user = _context.Users.FirstOrDefault(u =>
                                    u.FirstName.ToLower().Contains(search.ToLower()) ||
                                    u.LastName.ToLower().Contains(search.ToLower()));
                            }
                        }
                    }
                }

                if (user != null || OrderHeaderList.Count > 0)
                {
                    if (OrderHeaderList.Count == 0)
                    {
                        OrderHeaderList = _context.OrderHeaders.Where(o => o.UserId == user.Id)
                            .OrderByDescending(o => o.PickUpTime).ToList();
                    }

                    foreach (OrderHeader header in OrderHeaderList)
                    {
                        OrderDetailsViewModel individual = new OrderDetailsViewModel();
                        individual.OrderDetails = _context.OrderDetails.Where(o => o.OrderId == header.Id).ToList();
                        individual.OrderHeader = header;

                        OrderDetailsViewModel.Add(individual);
                    }
                }
            }
            else
            {
                //If there is no search criteria
                List<OrderHeader> OrderHeaderList = _context.OrderHeaders
                    .Where(u => u.Status == SD.StatusReady).OrderByDescending(u => u.PickUpTime).ToList();

                foreach (OrderHeader header in OrderHeaderList)
                {
                    OrderDetailsViewModel individual = new OrderDetailsViewModel();
                    individual.OrderDetails = _context.OrderDetails.Where(o => o.OrderId == header.Id).ToList();
                    individual.OrderHeader = header;

                    OrderDetailsViewModel.Add(individual);
                }
            }
        }
    }
}