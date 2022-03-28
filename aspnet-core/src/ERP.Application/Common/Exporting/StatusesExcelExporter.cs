using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class StatusesExcelExporter : EpPlusExcelExporterBase, IStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStatusForViewDto> statuses)
        {
            return CreateExcelPackage(
                "Statuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Statuses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, statuses,
                        _ => _.Status.Name,
                        _ => _.Status.Description
                        );

                });
        }
    }
}