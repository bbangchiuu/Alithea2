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
    public class UserAccount : IValidatableObject
    {
        [Key]
        public int UserID { get; set; }

        [DisplayName("Mã tài khoản")]
        [Required]
        [StringLength(100)]
        [Index("Ix_RoleNumber", Order = 1, IsUnique = true)]
        public string RoleNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Index("Ix_Username", Order = 1, IsUnique = true)]
        [DisplayName("Tài khoản")]
        public string Username { get; set; }

        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        [Index("Ix_Email", Order = 1, IsUnique = true)]
        public string Email { get; set; }

        [DisplayName("Ảnh")]
        public string Image { get; set; }

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Ngày sinh")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDay { get; set; }

        [DisplayName("Ngày tạo tại khoản")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatAt { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeleteAt { get; set; }

        [DisplayName("Ngày chỉnh sửa gần đây nhất")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? UpdateAt { get; set; }

        [DisplayName("Trạng thái")]
        public UserAccountStatus Status { get; set; }
        public enum UserAccountStatus
        {
            [Display(Name = "Đang hoạt động")]
            Active = 1,
            [Display(Name = "Đang bị khóa")]
            DeActive = 0,
            [Display(Name = "Đã xóa")]
            Deleted = -1
        }

        [DisplayName("Quyền")]
        public Decentralization? admin { get; set; }
        public enum Decentralization
        {
            Admin = 1,
            Customer = 0
        }

        public ICollection<Order> Orders { get; set; }

        public Dictionary<string, string> ValidateLogin()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(Username))
            {
                errors.Add("Username", "Bạn chưa nhập tài khoản");
            }

            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("Password", "Bạn chưa nhập mật khẩu");
            }

            return errors;
        }

        public Dictionary<string, string> ValidateRegister()
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
            else if (Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
            {
                errors.Add("Email", "Email không hợp lệ");
            }

            if (string.IsNullOrEmpty(Username))
            {
                errors.Add("Username", "Bạn chưa nhập tài khoản");
            }
            else if (Regex.IsMatch(Username, @"^\d"))
            {
                errors.Add("Username", "Username không hợp lệ");
            }

            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("Password", "Bạn chưa nhập mật khẩu");
            }
            else if (Password.Length <= 6)
            {
                errors.Add("Password", "Mật khẩu phải nhiều hơn 6 ký tự");
            }
            else if (Regex.IsMatch(Password, @"[A-Z]+") == false)
            {
                errors.Add("Password", "Mật khẩu phải có chữ in hoa");
            }
            else if (Regex.IsMatch(Password, @"^[a-zA-Z0-9\+]*$"))
            {
                errors.Add("Password", "Mật khẩu phải có ký tự đặc biệt");
            }

            if (BirthDay.Date == DateTime.MinValue)
            {
                errors.Add("Birthday", "Bạn chưa nhập ngày sinh");
            }
            return errors;
        }

        public void Display()
        {
            Debug.WriteLine("Id: " + UserID);
            Debug.WriteLine("Name: " + FullName);
            Debug.WriteLine("Pass: " + Password);
            Debug.WriteLine("Email: " + Email);
            Debug.WriteLine("Phone: " + Phone);
            Debug.WriteLine("Address: " + Address);
            Debug.WriteLine("CreatedAt: " + CreatAt);
            Debug.WriteLine("UpdatedAt: " + UpdateAt);
            Debug.WriteLine("DeletedAt: " + DeleteAt);
            Debug.WriteLine("admin: " + admin);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            MyDbContext db = new MyDbContext();
            List<ValidationResult> validationResult = new List<ValidationResult>();
            var validateName = db.UserAccounts.FirstOrDefault(x => x.RoleNumber == RoleNumber && x.UserID != UserID);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Mã người dùng này đã tồn tại", new[] { "RoleNumber" });
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