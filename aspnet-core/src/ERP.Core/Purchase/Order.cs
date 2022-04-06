using ERP.Common;
using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Purchase
{
    [Table("Orders")]
    public class Order : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string ReceiverName { get; set; }


        [Required]
        public virtual string OrderCode { get; set; }

        [Required]
        public virtual string TotalPrice { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxShippingAddressLength, MinimumLength = OrderConsts.MinShippingAddressLength)]
        public virtual string ShippingAddress { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxShippingNumberLength, MinimumLength = OrderConsts.MinShippingNumberLength)]
        public virtual string ShippingNumber { get; set; }

        public virtual long DiscountAmount { get; set; }

        public virtual long ActualPrice { get; set; }

        public virtual long? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status StatusFk { get; set; }

        public virtual long? DiscountId { get; set; }

        [ForeignKey("DiscountId")]
        public Discount DiscountFk { get; set; }

    }
}