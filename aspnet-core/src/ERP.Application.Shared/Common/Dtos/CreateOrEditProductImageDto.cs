using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditProductImageDto : EntityDto<long?>
    {

        public long? ProductId { get; set; }

        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [StringLength(ImageConsts.MaxDescriptionLength, MinimumLength = ImageConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}