using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditColorDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ColorConsts.MaxColorNameLength, MinimumLength = ColorConsts.MinColorNameLength)]
        public string ColorName { get; set; }

        public int ColorCode { get; set; }

    }
}