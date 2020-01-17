using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Alithea2.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendEmail(string _name, string _phone, string _email, string _description)
        {
            string senderID = "bangnguyenzero@gmail.com";
            string senderPassword = "Bang@123";
            string result = "Email Sent Successfully";

            string body = " " + _name + " has sent an email from " + _email;
            body += "Phone : " + _phone;
            body += _description;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(senderID);
                mail.From = new MailAddress(senderID);
                mail.Subject = "My Test Email!";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                result = "problem occurred";
                Response.Write("Exception in sendEmail:" + ex.Message);
            }
            return Json(result);
        }

    }
}