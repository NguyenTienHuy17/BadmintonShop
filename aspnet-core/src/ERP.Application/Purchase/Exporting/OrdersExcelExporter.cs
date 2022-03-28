using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Purchase.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Purchase.Exporting
{
    public class OrdersExcelExporter : EpPlusExcelExporterBase, IOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderForViewDto> orders)
        {
            return CreateExcelPackage(
                "Orders.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Orders"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("OrderCode"),
                        L("TotalPrice"),
                        L("ShippingAddress"),
                        L("ShippingNumber"),
                        L("DiscountAmount"),
                        L("ActualPrice"),
                        (L("Status")) + L("Name"),
                        (L("Discount")) + L("DiscountCode")
                        );

                    AddObjects(
                        sheet, 2, orders,
                        _ => _.Order.OrderCode,
                        _ => _.Order.TotalPrice,
                        _ => _.Order.ShippingAddress,
                        _ => _.Order.ShippingNumber,
                        _ => _.Order.DiscountAmount,
                        _ => _.Order.ActualPrice,
                        _ => _.StatusName,
                        _ => _.DiscountDiscountCode
                        );

                });
        }
    }
}