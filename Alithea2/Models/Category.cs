using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class Category : IValidatableObject
    {
        [Key]
        public int CategoryID { get; set; }

        [DisplayName("Mã Danh mục")]
        [Required]
        [StringLength(50)]
        [Index("Ix_RoleNumber", Order = 1, IsUnique = true)]
        public string RoleNumber { get; set; }

        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }

        [DisplayName("Mô tả")]
        public string CategoryDescription { get; set; }

        [DisplayName("Ảnh")]
        public string CategoryImage { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        [DisplayName("Ngày tạo")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Ngày chỉnh sửa")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeletedAt { get; set; }

        [DisplayName("Trạng thái")]
        public CategoryStatus Status { get; set; }
        public enum CategoryStatus
        {
            [Display(Name = "Đang hoạt động")]
            Active = 1,
            [Display(Name = "Đang không hoạt động")]
            DeActive = 0,
            Saved = 2,
            [Display(Name = "Đã xóa")]
            Deleted = -1
        }

        public Category()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            DeletedAt = null;
        }

        public Dictionary<string, string> Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(CategoryName))
            {
                errors.Add("CategoryName", "Name is required!");
            }

            if (string.IsNullOrEmpty(CategoryDescription))
            {
                errors.Add("CategoryDescription", "Description is required!");
            }

            if (string.IsNullOrEmpty(CategoryImage))
            {
                errors.Add("CategoryImage", "Image is required!");
            }
            return errors;
        }

        public void Display()
        {
            Debug.WriteLine("Id: " + CategoryID);
            Debug.WriteLine("Name: " + CategoryName);
            Debug.WriteLine("Description: " + CategoryDescription);
            Debug.WriteLine("Image: " + CategoryImage);
            Debug.WriteLine("CreatedAt: " + CreatedAt);
            Debug.WriteLine("UpdatedAt: " + UpdatedAt);
            Debug.WriteLine("DeletedAt: " + DeletedAt);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            MyDbContext db = new MyDbContext();
            List<ValidationResult> validationResult = new List<ValidationResult>();
            var validateName = db.Categories.FirstOrDefault(x => x.RoleNumber == RoleNumber && x.CategoryID != CategoryID);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Mã danh mục này đã tồn tại", new[] { "RoleNumber" });
                validationResult.Add(errorMessage);
                return validationResult;
            }

            else
            {
                return validationResult;
            }

        }
    }
}