using System.Collections.Generic;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common.Exporting
{
    public interface IBlogsExcelExporter
    {
        FileDto ExportToFile(List<GetBlogForViewDto> blogs);
    }
}