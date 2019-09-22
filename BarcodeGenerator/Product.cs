namespace BarcodeGenerator
{
    public class Product
    {
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public string Code128Text { get; set; }
        public decimal Price { get; internal set; }
    }
}