using System.Collections.Generic;

namespace ERP.Entity.Dtos
{
	public class ProductImageUrl
	{
		public ProductImageUrl()
		{
			ListImageUrl = new List<string>();
		}

		public long ProductId { get; set; }
		public List<string> ListImageUrl { get; set; }
	}
}