using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alithea2.Models
{
    public class Product : IValidatableObject
    {
        [Key]
        public int ProductID { get; set; }

        [DisplayName("Mã sản phẩm")]
        [Required]
        [StringLength(50)]
        [Index("Ix_RoleNumber", Order = 1, IsUnique = true)]
        public string RoleNumber { get; set; }

        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }

        [DisplayName("Mô tả")]
        public string ProductDescription { get; set; }

        [DisplayName("Đơn giá")]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }

        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Ảnh")]
        public string ProductImage { get; set; }

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
        public ProductStatus Status { get; set; }
        public enum ProductStatus
        {
            [Display(Name = "Đang hoạt động")]
            Active = 1,
            [Display(Name = "Đang hết hàng")]
            DeActive = 0,
            Saved = 2,
            [Display(Name = "Đã xóa")]
            Deleted = -1
        }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<Attribute> Attributes { get; set; }

        public int Color;
        public string NameColor;

        public int Size;
        public string NameSize;

        public Product()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            DeletedAt = null;
        }

        public Dictionary<string, string> Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(RoleNumber))
            {
                errors.Add("RoleNumber", "Bạn chưa nhập mã sản phẩm");
            }

            if (string.IsNullOrEmpty(ProductName))
            {
                errors.Add("ProductName", "Bạn chưa nhập tên sản phẩm");
            }

            if (string.IsNullOrEmpty(ProductDescription))
            {
                errors.Add("ProductDescription", "Bạn chưa nhập mô tả sản phẩm");
            }

            if (string.IsNullOrEmpty(ProductImage))
            {
                errors.Add("ProductImage", "Sản phẩm chưa có ảnh");
            }

            return errors;
        }

        public void Display()
        {
            Debug.WriteLine("Id: " + ProductID);
            Debug.WriteLine("RoleNumber: " + RoleNumber);
            Debug.WriteLine("Name: " + ProductName);
            Debug.WriteLine("Description: " + ProductDescription);
            Debug.WriteLine("Image: " + ProductImage);
            Debug.WriteLine("Quantity: " + Quantity);
            Debug.WriteLine("Price: " + UnitPrice);
            Debug.WriteLine("CreatedAt: " + CreatedAt);
            Debug.WriteLine("UpdatedAt: " + UpdatedAt);
            Debug.WriteLine("DeletedAt: " + DeletedAt);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            MyDbContext db = new MyDbContext();
            List<ValidationResult> validationResult = new List<ValidationResult>();
            var validateName = db.Products.FirstOrDefault(x => x.RoleNumber == RoleNumber && x.ProductID != ProductID);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Mã sản phẩm này đã tồn tại.", new[] { "RoleNumber" });
                validationResult.Add(errorMessage);
                
            }
           
            return validationResult;
        }
    }
}