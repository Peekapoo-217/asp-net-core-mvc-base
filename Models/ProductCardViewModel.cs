using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo_Code_First.Models
{
    public class ProductCardViewModel
    {
        [Key]
        public int productID { get; set; }

        [DisplayName("Tên sản phẩm ")]
        public string productName { get; set; }
        public Nullable<decimal> Price { get; set; }

        public int Quantity { get; set; }
        public string MainImage { get; set; }
        public string CategoryName { get; set; }

    }
}
