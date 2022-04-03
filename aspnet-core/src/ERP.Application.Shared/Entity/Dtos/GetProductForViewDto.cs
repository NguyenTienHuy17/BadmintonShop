using System.Collections.Generic;

namespace ERP.Entity.Dtos
{
    public class GetProductForViewDto
    {
        public ProductDto Product { get; set; }

        public string ProductImageUrl { get; set; }

        public string BrandName { get; set; }

        public string CategoryName { get; set; }

    }
}