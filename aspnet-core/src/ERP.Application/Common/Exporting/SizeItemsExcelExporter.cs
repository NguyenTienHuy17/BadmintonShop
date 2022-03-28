using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class SizeItemsExcelExporter : EpPlusExcelExporterBase, ISizeItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SizeItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSizeItemForViewDto> sizeItems)
        {
            return CreateExcelPackage(
                "SizeItems.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SizeItems"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Product")) + L("Name"),
                        (L("Size")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, sizeItems,
                        _ => _.ProductName,
                        _ => _.SizeDisplayName
                        );

                });
        }
    }
}