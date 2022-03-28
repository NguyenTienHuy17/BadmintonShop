using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Purchase.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Purchase.Exporting
{
    public class OrderItemsExcelExporter : EpPlusExcelExporterBase, IOrderItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems)
        {
            return CreateExcelPackage(
                "OrderItems.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OrderItems"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Quantity"),
                        (L("Product")) + L("Name"),
                        (L("Order")) + L("OrderCode")
                        );

                    AddObjects(
                        sheet, 2, orderItems,
                        _ => _.OrderItem.Quantity,
                        _ => _.ProductName,
                        _ => _.OrderOrderCode
                        );

                });
        }
    }
}