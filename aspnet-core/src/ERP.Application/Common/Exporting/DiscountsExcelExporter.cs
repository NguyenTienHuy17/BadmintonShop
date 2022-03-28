using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class DiscountsExcelExporter : EpPlusExcelExporterBase, IDiscountsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountForViewDto> discounts)
        {
            return CreateExcelPackage(
                "Discounts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Discounts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DiscountCode"),
                        L("Discount"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, discounts,
                        _ => _.Discount.DiscountCode,
                        _ => _.Discount.DiscountNum,
                        _ => _.Discount.Description
                        );

                });
        }
    }
}