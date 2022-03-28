using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entity.Dtos
{
    public class CreateOrEditProductDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxMadeInLength, MinimumLength = ProductConsts.MinMadeInLength)]
        public string MadeIn { get; set; }

        [StringLength(ProductConsts.MaxCodeLength, MinimumLength = ProductConsts.MinCodeLength)]
        public string Code { get; set; }

        public long Price { get; set; }

        public int InStock { get; set; }

        public string Description { get; set; }

        [StringLength(ProductConsts.MaxTitleLength, MinimumLength = ProductConsts.MinTitleLength)]
        public string Title { get; set; }

        public long? ImageId { get; set; }

        public long? BrandId { get; set; }

        public long? CategoryId { get; set; }

    }
}