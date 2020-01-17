using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Alithea2.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [DisplayName("Họ và tên")]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }

        public string Email { get; set; }

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
            else if (Regex.IsMatch(Email, @"@gmail.com$") == false)
            {
                errors.Add("Email", "Email không hợp lệ");
            }

            return errors;
        }

        public void Display()
        {
            Debug.WriteLine("Name: " + FullName);
            Debug.WriteLine("Email: " + Email);
            Debug.WriteLine("Phone: " + Phone);
            Debug.WriteLine("Address: " + Address);
        }
    }
}