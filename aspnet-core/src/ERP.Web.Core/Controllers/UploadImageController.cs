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

        private const int MaxImageBytes = 5242880; //5MB

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

				DirectoryHelper.CreateIfNotExists("C:/inetpub/webs/Live/ng/assets/common/images");

				var tempFileName = System.Guid.NewGuid() + Path.GetExtension(file.FileName);
				var tempFilePath = Path.Combine("C:/inetpub/webs/Live/ng/assets/common/images", tempFileName);

				System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);

				return tempFileName;
			}
            catch (Exception ex)
            {

                throw;
            }
		}

		public List<string> UploadMultipleFileToServer()
		{
			try
			{
				var listFile = Request.Form.Files;

				var result = new List<string>();
				if (listFile.Count > 20)
				{
					throw new UserFriendlyException(L("Upload_FileCount_Err"));
				}

				foreach (var file in listFile)
				{
					//Check input
					if (file == null)
					{
						throw new UserFriendlyException(L("File_Empty_Error"));
					}

					//if (file.Length > MaxImageBytes)
					//{
					//	throw new UserFriendlyException(L("Upload_FileSize_Err"));
					//}

					byte[] fileBytes;
					using (var stream = file.OpenReadStream())
					{
						fileBytes = stream.GetAllBytes();
					}

					var isImage = CheckFileIsImage(file.ContentType);

					if (!isImage)
					{
						throw new Exception(L("IncorrectImageFormat"));
					}

					if (fileBytes.Length > MaxImageBytes)
					{
						throw new UserFriendlyException(L("Upload_FileSize_Err"));
					}

					// TODO: create folder images with tenant and store
					// TODO: Get product id from Request Form

					var productId = Convert.ToInt32(Request.Form[Request.Form.Keys.FirstOrDefault()]);

					DirectoryHelper.CreateIfNotExists("C:/inetpub/webs/Live/ng/assets/common/images");

					var tempFileName = System.Guid.NewGuid() + Path.GetExtension(file.FileName);
					var tempFilePath = Path.Combine("C:/inetpub/webs/Live/ng/assets/common/images", tempFileName);

					System.IO.File.WriteAllBytes(tempFilePath, fileBytes);

					result.Add(tempFileName);
				}
				return result;
			}
			catch (UserFriendlyException ex)
			{
				return new List<string>();
			}
		}

		private static bool CheckFileIsImage(string contentType)
		{
            //var imageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
            //var fileExtension = MimeTypeMap.GetExtension(contentType);

            //if (imageExtensions.Contains(fileExtension.ToUpperInvariant()))
            //{
            //    return true;
            //}
            return true;
        }
	}
}
