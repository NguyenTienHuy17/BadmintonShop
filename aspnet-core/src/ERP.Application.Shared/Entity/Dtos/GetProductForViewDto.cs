using System.Collections.Generic;

namespace ERP.Entity.Dtos
{
    public class GetProductForViewDto
    {
        public ProductDto Product { get; set; }

        public List<string> ProductImageUrl { get; set; }
        public List<string> ProductColor { get; set; }
        public List<string> ProductSize { get; set; }

        public string BrandName { get; set; }

        public string CategoryName { get; set; }

        public bool IsColor { get; set; }
        public bool IsSize { get; set; }

    }
}