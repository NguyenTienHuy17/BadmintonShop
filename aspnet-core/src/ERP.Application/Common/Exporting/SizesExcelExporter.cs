using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class SizesExcelExporter : EpPlusExcelExporterBase, ISizesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SizesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSizeForViewDto> sizes)
        {
            return CreateExcelPackage(
                "Sizes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Sizes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DisplayName"),
                        L("SizeNum")
                        );

                    AddObjects(
                        sheet, 2, sizes,
                        _ => _.Size.DisplayName,
                        _ => _.Size.SizeNum
                        );

                });
        }
    }
}