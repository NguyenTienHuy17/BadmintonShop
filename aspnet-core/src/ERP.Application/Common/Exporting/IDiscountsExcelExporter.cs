using System.Collections.Generic;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common.Exporting
{
    public interface IDiscountsExcelExporter
    {
        FileDto ExportToFile(List<GetDiscountForViewDto> discounts);
    }
}