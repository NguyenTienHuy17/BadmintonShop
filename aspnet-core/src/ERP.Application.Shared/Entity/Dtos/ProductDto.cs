using System;
using Abp.Application.Services.Dto;

namespace ERP.Entity.Dtos
{
    public class ProductDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string MadeIn { get; set; }

        public string Code { get; set; }

        public long Price { get; set; }

        public int InStock { get; set; }

        public string Description { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }

        public long? ImageId { get; set; }

        public long? BrandId { get; set; }

        public long? CategoryId { get; set; }

        public string ProductImageUrl { get; set; }

        public string BrandName { get; set; }

        public string CategoryName { get; set; }

    }
}