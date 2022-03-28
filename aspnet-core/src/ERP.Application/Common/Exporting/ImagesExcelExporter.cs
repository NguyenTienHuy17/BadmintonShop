using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class ImagesExcelExporter : EpPlusExcelExporterBase, IImagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ImagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetImageForViewDto> images)
        {
            return CreateExcelPackage(
                "Images.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Images"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Url"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, images,
                        _ => _.Image.Name,
                        _ => _.Image.Url,
                        _ => _.Image.Description
                        );

                });
        }
    }
}