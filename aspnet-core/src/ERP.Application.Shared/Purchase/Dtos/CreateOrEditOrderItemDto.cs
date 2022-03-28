using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class CreateOrEditOrderItemDto : EntityDto<long?>
    {

        [Required]
        public string Quantity { get; set; }

        public long ProductId { get; set; }

        public long OrderId { get; set; }

    }
}