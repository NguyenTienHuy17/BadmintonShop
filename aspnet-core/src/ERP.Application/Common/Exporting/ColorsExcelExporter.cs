using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class ColorsExcelExporter : EpPlusExcelExporterBase, IColorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ColorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetColorForViewDto> colors)
        {
            return CreateExcelPackage(
                "Colors.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Colors"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ColorName"),
                        L("ColorCode")
                        );

                    AddObjects(
                        sheet, 2, colors,
                        _ => _.Color.ColorName,
                        _ => _.Color.ColorCode
                        );

                });
        }
    }
}