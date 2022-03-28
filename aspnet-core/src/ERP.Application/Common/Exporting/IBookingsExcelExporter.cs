using System.Collections.Generic;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common.Exporting
{
    public interface IBookingsExcelExporter
    {
        FileDto ExportToFile(List<GetBookingForViewDto> bookings);
    }
}