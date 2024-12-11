namespace Demo_Code_First.Models
{
    public class ProductDetailsViewModel
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public List<string> Images { get; set; }
    }
}
