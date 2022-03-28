using System;
using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class OrderItemDto : EntityDto<long>
    {
        public string Quantity { get; set; }

        public long ProductId { get; set; }

        public long OrderId { get; set; }

    }
}