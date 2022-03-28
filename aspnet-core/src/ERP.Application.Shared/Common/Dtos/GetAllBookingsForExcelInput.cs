using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllBookingsForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxTimeFilter { get; set; }
        public DateTime? MinTimeFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}