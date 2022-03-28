using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditSizeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(SizeConsts.MaxDisplayNameLength, MinimumLength = SizeConsts.MinDisplayNameLength)]
        public string DisplayName { get; set; }

        public int SizeNum { get; set; }

    }
}