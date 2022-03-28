using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public long? ImageId { get; set; }

    }
}