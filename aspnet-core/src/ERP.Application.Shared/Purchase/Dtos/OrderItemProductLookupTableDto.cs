using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class OrderItemProductLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }
    }
}