using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entity.Dtos
{
    public class CreateOrEditBrandDto : EntityDto<long?>
    {

        [Required]
        [StringLength(BrandConsts.MaxNameLength, MinimumLength = BrandConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxCountryLength, MinimumLength = BrandConsts.MinCountryLength)]
        public string Country { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

    }
}