using Abp.Application.Services.Dto;
using System;

namespace ERP.Entity.Dtos
{
    public class GetAllProductsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string MadeInFilter { get; set; }

        public string CodeFilter { get; set; }

        public long? MaxPriceFilter { get; set; }
        public long? MinPriceFilter { get; set; }

        public int? MaxInStockFilter { get; set; }
        public int? MinInStockFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string TitleFilter { get; set; }

        public string ImageNameFilter { get; set; }

        public string BrandNameFilter { get; set; }

        public string CategoryNameFilter { get; set; }

    }
}