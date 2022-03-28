using Abp.Extensions;
using Abp.IO;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using ERP.Authorization.Users.Profile.Dto;
using ERP.Dto;
using ERP.Storage;
using ERP.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Web.Controllers
{
    public abstract class UploadImageControllerBase : ERPControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxProfilePictureSize = 5242880; //5MB

        protected UploadImageControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public string UploadPicture()
        {
            try
            {
				var file = Request.Form.Files.First();

				//Check input
				if (file == null)
				{
					throw new UserFriendlyException(L("File_Empty_Error"));
				}

				if (file.Length > 10000000) //10MB
				{
					throw new UserFriendlyException(L("File_SizeLimit_Error"));
				}

				byte[] fileBytes;
				using (var stream = file.OpenReadStream())
				{
					fileBytes = stream.GetAllBytes();
				}

				if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
				{
					throw new Exception(L("IncorrectImageFormat"));
				}

				DirectoryHelper.CreateIfNotExists("E:/BadmintonShopPictures");

				var tempFileName = System.Guid.NewGuid() + Path.GetExtension(file.FileName);
				var tempFilePath = Path.Combine("E:/BadmintonShopPictures", tempFileName);

				System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);

				return tempFilePath;
			}
            catch (Exception ex)
            {

                throw;
            }
		}
    }
}
