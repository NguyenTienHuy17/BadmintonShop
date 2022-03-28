using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllSizeItemsForExcelInput
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string SizeDisplayNameFilter { get; set; }

    }
}