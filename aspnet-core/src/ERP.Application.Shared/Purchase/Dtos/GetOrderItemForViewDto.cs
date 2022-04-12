
using ERP.Entity.Dtos;

namespace ERP.Purchase.Dtos
{
    public class GetOrderItemForViewDto
    {
        public OrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderCode { get; set; }

        public ProductDto Product { get; set; }

    }
}