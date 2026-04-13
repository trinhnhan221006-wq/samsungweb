using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace samsungweb.Models
{
    public class Category
    {
        [Key] // Đánh dấu đây là Khóa chính (Primary Key)
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }


        public List<Product>? Products { get; set; }

        // Công tắc: Có muốn đưa danh mục này lên Menu chính không?
        public bool ShowOnHeader { get; set; } = false;
    }
}
