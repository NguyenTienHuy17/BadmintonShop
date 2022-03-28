using Abp.Application.Services.Dto;
using System;

namespace ERP.Entity.Dtos
{
    public class GetAllBrandsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CountryFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ImageNameFilter { get; set; }

    }
}