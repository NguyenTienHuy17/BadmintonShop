using System.Collections.Generic;
using ERP.Entity.Dtos;
using ERP.Dto;

namespace ERP.Entity.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}