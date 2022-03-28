using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditDiscountDto : EntityDto<long?>
    {

        [Required]
        [StringLength(DiscountConsts.MaxDiscountCodeLength, MinimumLength = DiscountConsts.MinDiscountCodeLength)]
        public string DiscountCode { get; set; }

        [Range(DiscountConsts.MinDiscountValue, DiscountConsts.MaxDiscountValue)]
        public int DiscountNum { get; set; }

        [StringLength(DiscountConsts.MaxDescriptionLength, MinimumLength = DiscountConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}