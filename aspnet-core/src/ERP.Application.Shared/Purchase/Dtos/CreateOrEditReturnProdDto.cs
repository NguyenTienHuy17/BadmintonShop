using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class CreateOrEditReturnProdDto : EntityDto<long?>
    {

        [Required]
        public string Reason { get; set; }

        public long? OrderId { get; set; }

    }
}