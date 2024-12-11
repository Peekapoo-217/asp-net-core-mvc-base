using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo_Code_First.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }


        //[StringLength(100)]
        //public string CategoryDescription { get; set; }
    }
}
