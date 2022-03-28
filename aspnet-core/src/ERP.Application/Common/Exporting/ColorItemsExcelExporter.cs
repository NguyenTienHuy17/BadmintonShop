using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class ColorItemsExcelExporter : EpPlusExcelExporterBase, IColorItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ColorItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetColorItemForViewDto> colorItems)
        {
            return CreateExcelPackage(
                "ColorItems.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ColorItems"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Product")) + L("Name"),
                        (L("Color")) + L("ColorName")
                        );

                    AddObjects(
                        sheet, 2, colorItems,
                        _ => _.ProductName,
                        _ => _.ColorColorName
                        );

                });
        }
    }
}