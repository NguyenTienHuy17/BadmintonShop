using System.Collections.Generic;
using ERP.Purchase.Dtos;
using ERP.Dto;

namespace ERP.Purchase.Exporting
{
    public interface IReturnProdsExcelExporter
    {
        FileDto ExportToFile(List<GetReturnProdForViewDto> returnProds);
    }
}