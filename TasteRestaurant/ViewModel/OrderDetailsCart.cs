using System.Collections.Generic;
using TasteRestaurant.Data;

namespace TasteRestaurant.ViewModel
{
    public class OrderDetailsCart
    {
        public List<ShoppingCart> ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
