using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditImageDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ImageConsts.MaxNameLength, MinimumLength = ImageConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [StringLength(ImageConsts.MaxDescriptionLength, MinimumLength = ImageConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}