using Abp.Application.Services.Dto;
using System;

namespace ERP.Purchase.Dtos
{
    public class GetAllReturnProdsForExcelInput
    {
        public string Filter { get; set; }

        public string ReasonFilter { get; set; }

        public string OrderOrderCodeFilter { get; set; }

    }
}