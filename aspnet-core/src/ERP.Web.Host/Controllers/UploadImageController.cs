using Abp.AspNetCore.Mvc.Authorization;
using ERP.Storage;

namespace ERP.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UploadImageController : UploadImageControllerBase
    {
        public UploadImageController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}