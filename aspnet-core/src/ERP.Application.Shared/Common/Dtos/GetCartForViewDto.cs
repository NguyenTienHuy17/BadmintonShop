namespace ERP.Common.Dtos
{
    public class GetCartForViewDto
    {
        public CartDto Cart { get; set; }

        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }

        public long? ProductPrice { get; set; }

    }
}