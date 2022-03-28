using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditStatusDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StatusConsts.MaxNameLength, MinimumLength = StatusConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}