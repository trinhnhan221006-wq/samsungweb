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

        // Cột này lưu ID của danh mục cha. Nếu là danh mục lớn thì nó sẽ là null.
        public int? ParentId { get; set; }

        // Khai báo mối quan hệ Cha - Con cho Entity Framework hiểu
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}
