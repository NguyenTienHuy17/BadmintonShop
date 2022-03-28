using System.Collections.Generic;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common.Exporting
{
    public interface ISizesExcelExporter
    {
        FileDto ExportToFile(List<GetSizeForViewDto> sizes);
    }
}