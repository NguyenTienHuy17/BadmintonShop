using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class CartsExcelExporter : EpPlusExcelExporterBase, ICartsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CartsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCartForViewDto> carts)
        {
            return CreateExcelPackage(
                "Carts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Carts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("quantity"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, carts,
                        _ => _.Cart.quantity,
                        _ => _.ProductName
                        );

                });
        }
    }
}