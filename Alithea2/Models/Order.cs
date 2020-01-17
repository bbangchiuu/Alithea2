using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Alithea2.Models
{
    public class Order : IValidatableObject
    {
        [Key] public int OrderID { get; set; }

        [DisplayName("Mã đặt hàng")]
        [Required]
        [StringLength(50)]
        [Index("Ix_RoleNumber", Order = 1, IsUnique = true)]
        public string RoleNumber { get; set; }

        [DisplayName("Ngày đặt hàng")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? RequireDate { get; set; }

        [DisplayName("Ngày giao hàng")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ShippedDate { get; set; }

        [DisplayName("Tổng số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Tổng đơn hàng")]
        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }

        [DisplayName("Chú thích")]
        public string Commnet { get; set; }

        [ForeignKey("UserAccount")] 
        public int? UserID { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }

        public string Email { get; set; }

        [DisplayName("Trạng thái")]
        public StatusOrder Status { get; set; }
        public enum StatusOrder
        {
            [Display(Name = "Đang xác nhận")]
            DeActive = 0,
            [Display(Name = "Đang chuẩn bị")]
            Prepare = 1,
            [Display(Name = "Đang vận chuyển")]
            Proccesing = 2,
            [Display(Name = "Đã giao hàng")]
            Finish = 3,
            [Display(Name = "Đã xóa")]
            Deleted = -1
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            MyDbContext db = new MyDbContext();
            List<ValidationResult> validationResult = new List<ValidationResult>();
            var validateName = db.Orders.FirstOrDefault(x => x.RoleNumber == RoleNumber && x.OrderID != OrderID);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Mã đơn hàng này đã tồn tại.", new[] { "RoleNumber" });
                validationResult.Add(errorMessage);

            }

            return validationResult;
        }


        public Dictionary<string, string> Validate()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(FullName))
            {
                errors.Add("FullName", "Bạn chưa nhập họ tên");
            }

            //bool validPhone = Regex.IsMatch(Phone, @"\D");
            if (string.IsNullOrEmpty(Phone))
            {
                errors.Add("Phone", "Bạn chưa nhập số điện thoại");
            }
            else if (Regex.IsMatch(Phone, @"\D") || Regex.IsMatch(Phone, @"^0") == false)
            {
                errors.Add("Phone", "Số điện thoại không hợp lệ");
            }
            else if (Phone.Length < 10 || Phone.Length > 13)
            {
                errors.Add("Phone", "Số điện thoại không hợp lệ");
            }

            if (string.IsNullOrEmpty(Address))
            {
                errors.Add("Address", "Bạn chưa nhập địa chỉ");
            }

            if (string.IsNullOrEmpty(Email))
            {
                errors.Add("Email", "Bạn chưa nhập email");
            }
            else if (Regex.IsMatch(Email, @"@gmail.com$") == false)
            {
                errors.Add("Email", "Email không hợp lệ");
            }

            return errors;
        }

        public void Display()
        {
            Debug.WriteLine("---------------");
            Debug.WriteLine("Id: " + OrderID);
            Debug.WriteLine("rollnumber: " + RoleNumber);
            Debug.WriteLine("user Id: " + UserID);
            Debug.WriteLine("Total quantity: " + Quantity);
            Debug.WriteLine("Total price: " + TotalPrice);
            Debug.WriteLine("Create at: " + OrderDate);
            Debug.WriteLine("Require: " + RequireDate);
            Debug.WriteLine("Shipped: " + ShippedDate);
            Debug.WriteLine("Status: " + Status);
            Debug.WriteLine("Commnet: " + Commnet);
            Debug.WriteLine("Name: " + FullName);
            Debug.WriteLine("Email: " + Email);
            Debug.WriteLine("Phone: " + Phone);
            Debug.WriteLine("Address: " + Address);
        }
    }
}