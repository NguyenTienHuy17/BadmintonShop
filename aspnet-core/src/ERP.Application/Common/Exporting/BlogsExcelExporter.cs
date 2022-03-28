using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class BlogsExcelExporter : EpPlusExcelExporterBase, IBlogsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BlogsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBlogForViewDto> blogs)
        {
            return CreateExcelPackage(
                "Blogs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Blogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("title"),
                        L("content")
                        );

                    AddObjects(
                        sheet, 2, blogs,
                        _ => _.Blog.title,
                        _ => _.Blog.content
                        );

                });
        }
    }
}