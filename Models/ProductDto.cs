using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Demo_Code_First.Models
{
    public class ProductDto
    {
        public string productName { get; set; }
        public Nullable<decimal> Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        [DisplayName("danh mục sản phẩm ")]
        public int categoryid { get; set; }

        [ForeignKey("categoryid")]
        public Category category { get; set; }
    }
}
