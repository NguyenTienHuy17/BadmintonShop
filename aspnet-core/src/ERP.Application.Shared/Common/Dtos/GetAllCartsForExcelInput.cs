using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllCartsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxquantityFilter { get; set; }
        public int? MinquantityFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}