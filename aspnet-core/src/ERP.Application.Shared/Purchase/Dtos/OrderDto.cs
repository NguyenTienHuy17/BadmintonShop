using System;
using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class OrderDto : EntityDto<long>
    {
        public string ReceiverName { get; set; }
        public string OrderCode { get; set; }

        public string TotalPrice { get; set; }

        public string ShippingAddress { get; set; }

        public string ShippingNumber { get; set; }

        public long DiscountAmount { get; set; }

        public long ActualPrice { get; set; }

        public long? StatusId { get; set; }

        public long? DiscountId { get; set; }

    }
}