using System.Collections.Generic;
using ERP.Purchase.Dtos;
using ERP.Dto;

namespace ERP.Purchase.Exporting
{
    public interface IOrderItemsExcelExporter
    {
        FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems);
    }
}