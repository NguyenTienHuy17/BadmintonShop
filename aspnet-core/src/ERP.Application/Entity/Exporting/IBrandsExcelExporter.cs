using System.Collections.Generic;
using ERP.Entity.Dtos;
using ERP.Dto;

namespace ERP.Entity.Exporting
{
    public interface IBrandsExcelExporter
    {
        FileDto ExportToFile(List<GetBrandForViewDto> brands);
    }
}