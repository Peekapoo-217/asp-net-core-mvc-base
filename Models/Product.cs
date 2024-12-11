using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_Code_First.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }

        [DisplayName("Product Name ")]
        public string productName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int Quantity { get; set; }

        public string Description { get; set; }

        [DisplayName("Danh mục sản phẩm ")]

        public int categoryid { get; set; }

        public Category? category { get; set; }

        public string? ImagesDirectory { get; set; }
    }
}
