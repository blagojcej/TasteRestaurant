using System.Collections.Generic;
using TasteRestaurant.Data;

namespace TasteRestaurant.ViewModel
{
    public class OrderDetailsViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
