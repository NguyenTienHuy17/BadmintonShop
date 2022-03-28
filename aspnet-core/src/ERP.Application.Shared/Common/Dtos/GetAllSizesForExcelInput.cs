using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllSizesForExcelInput
    {
        public string Filter { get; set; }

        public string DisplayNameFilter { get; set; }

        public int? MaxSizeNumFilter { get; set; }
        public int? MinSizeNumFilter { get; set; }

    }
}