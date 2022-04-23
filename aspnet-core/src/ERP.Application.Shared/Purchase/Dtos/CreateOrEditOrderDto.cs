using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class CreateOrEditOrderDto : EntityDto<long?>
    {
        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public string OrderCode { get; set; }

        [Required]
        public long TotalPrice { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxShippingAddressLength, MinimumLength = OrderConsts.MinShippingAddressLength)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxShippingNumberLength, MinimumLength = OrderConsts.MinShippingNumberLength)]
        public string ShippingNumber { get; set; }

        public long DiscountAmount { get; set; }

        public long ActualPrice { get; set; }

        public long? StatusId { get; set; }

        public long? DiscountId { get; set; }

    }
}