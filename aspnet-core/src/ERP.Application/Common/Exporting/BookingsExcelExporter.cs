using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class BookingsExcelExporter : EpPlusExcelExporterBase, IBookingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Bookings.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Bookings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Time"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => _timeZoneConverter.Convert(_.Booking.Time, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Booking.Description
                        );

                    var timeColumn = sheet.Column(1);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    timeColumn.AutoFit();

                });
        }
    }
}