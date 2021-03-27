using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using notesmarketplace.Models;
using System.IO;

namespace notesmarketplace.Controllers
{
    public class pagesController : Controller
    {
        NotesMarketPlaceEntities db = new NotesMarketPlaceEntities();

        // GET: pages
        public ActionResult SearchNotes()
        {
            var data = db.SellerNotes.ToList();
            return View(data);
        }

        public ActionResult AddNotes(int? id)
        {
            ViewBag.CountryList = db.Countries.ToList();
            ViewBag.CategoryList = db.NoteCategories.ToList();
            ViewBag.TypeList = db.NoteTypes.ToList();
            if (id == null)
            {
                if (Session["email"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("LogIn", "pages");
                }
            }
            else
            {
                var data = db.SellerNotes.Where(x => x.ID == id).SingleOrDefault();
                if(data == null)
                {
                    return RedirectToAction("Dashboard");
                }
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult AddNotes(SellerNote sellerNote, String save)
        {
            int userid = (int)Session["email"];
            ViewBag.CountryList = db.Countries.ToList();
            ViewBag.CategoryList = db.NoteCategories.ToList();
            ViewBag.TypeList = db.NoteTypes.ToList();
            if (sellerNote != null)
            {
                if(sellerNote.Category.Equals("Select your category"))
                {
                    ViewBag.category = "Please Select Category";
                    return View(sellerNote);
                }
                if (sellerNote.NoteType.Equals("Select your mode type"))
                {
                    sellerNote.NoteType = null;
                }
                if (sellerNote.Country.Equals("Select your country"))
                {
                    sellerNote.Country = null;
                }
                if (sellerNote.Note_Attachment == null && sellerNote.NoteAttachment == null)
                {
                    ViewBag.note_attachment = "Please attach your notes here...";
                    return View(sellerNote);
                }
            }
            sellerNote.IsActive = true;
            sellerNote.CreatedDate = DateTime.Now;

            if ((sellerNote.Display_Picture != null) && (sellerNote.DisplayPicture == null))
            {
                if (sellerNote.Display_Picture.ContentLength > 1024 * 1024 * 10)
                {
                    ViewBag.display_picture = "File size should be less than 10 MB";
                    return View(sellerNote);
                }
                String path = Server.MapPath("~/Notes/DisplayPicture/");
                String extension = Path.GetExtension(sellerNote.Display_Picture.FileName);
                var supportType = new[] {".jpg", ".png", ".jpeg" };
                if (supportType.Contains(extension.ToUpper()) || supportType.Contains(extension.ToLower()))
                {
                    String Filename = "DP" + DateTime.Now.ToString("yyyyMMddhhmmss") + System.IO.Path.GetExtension(sellerNote.Display_Picture.FileName);
                    String finalpath = System.IO.Path.Combine(path + Filename);
                    sellerNote.Display_Picture.SaveAs(finalpath);
                    sellerNote.DisplayPicture = Filename;
                }
                else
                {
                    ViewBag.display_picture = "Choose .jpg, .jpeg, .png file only ";
                    sellerNote.SellingPrice = 0;
                    return View(sellerNote);
                }
            }
            if (sellerNote.Note_Attachment != null && sellerNote.NoteAttachment == null)
            {
                String path1 = Server.MapPath("~/Notes/");
                String extension1 = Path.GetExtension(sellerNote.Note_Attachment.FileName);
                var supportedTypes = new[] { ".pdf" };
                if (supportedTypes.Contains(extension1.ToUpper()) || supportedTypes.Contains(extension1.ToLower()))
                {
                    String Filename = "Note" + DateTime.Now.ToString("yyyyMMddhhmmss") + System.IO.Path.GetExtension(sellerNote.Note_Attachment.FileName);
                    String finalpath1 = System.IO.Path.Combine(path1 + Filename);
                    sellerNote.Note_Attachment.SaveAs(finalpath1);
                    sellerNote.UploadNote = Filename;
                }
                else
                {
                    ViewBag.note_attachment = "Choose .pdf file only";
                    sellerNote.SellingPrice = 0;
                    return View(sellerNote);
                }
            }
            if(sellerNote.Note_Preview == null)
            {
                ViewBag.Note_Preview = "Please add note preview here !";
                return View(sellerNote);
            }
            if ((sellerNote.Note_Preview != null) && (sellerNote.NotePreview == null))
            {
                if (sellerNote.Note_Preview.ContentLength > 1024 * 1024 * 10)
                {
                    ViewBag.Note_Preview = "Attach Preview is more than 10 MB !!";
                    return View(sellerNote);
                }
                String path2 = Server.MapPath("~/Notes/NotePreview/");
                String extension2 = Path.GetExtension(sellerNote.Note_Preview.FileName);
                var supportedTypes = new[] { ".pdf" };
                if (supportedTypes.Contains(extension2.ToUpper()) || supportedTypes.Contains(extension2.ToLower()))
                {
                    String Filename = "NotePreview" + DateTime.Now.ToString("yyyyMMddhhmmss") + System.IO.Path.GetExtension(sellerNote.Note_Preview.FileName);
                    String finalpath2 = System.IO.Path.Combine(path2 + Filename);
                    sellerNote.Note_Preview.SaveAs(finalpath2);
                    sellerNote.NotePreview = Filename;
                }
                else
                {
                    ViewBag.Note_Preview = "Choose .pdf file only";
                    sellerNote.SellingPrice = 0;
                    return View(sellerNote);
                }
            }

            sellerNote.Title = sellerNote.Title;
            sellerNote.Category = sellerNote.Category;
            sellerNote.NoteType = sellerNote.NoteType;
            sellerNote.NumberOfPage = sellerNote.NumberOfPage;
            sellerNote.Description = sellerNote.Description;
            sellerNote.Country = sellerNote.Country;
            sellerNote.UniversityName = sellerNote.InstitutionName;
            sellerNote.Course = sellerNote.Course;
            sellerNote.CourseCode = sellerNote.CourseCode;
            sellerNote.Professor = sellerNote.Professor;
            sellerNote.SellingPrice = sellerNote.SellingPrice;
            sellerNote.SellerID = userid;
            sellerNote.IsPaid = false;

            if (!string.IsNullOrEmpty(save))
            {
                sellerNote.Status = 11;
                db.SellerNotes.Add(sellerNote);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                sellerNote.Status = 12;
                db.SellerNotes.Add(sellerNote);
                db.SaveChanges();
                return RedirectToAction("PublishMail");
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult EditNote (SellerNote sellerNote, string save)
        //{
        //    db.Entry(sellerNote).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Dashboard");
        //}
        [HttpPost]
        public ActionResult SaveNote(SellerNote sellerNote)
        {
            if(ModelState.IsValid)
            {

                if (sellerNote.ID == null)
                {
                    db.SellerNotes.Add(sellerNote);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(sellerNote).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Dashboard");
            }
            return View("AddNotes");
        }
            public ActionResult PublishMail(SellerNote sellerNote)
        {
            int userid = (int)Session["email"];
            User user = db.Users.SingleOrDefault(x => x.ID == userid);
            MailMessage mm = new MailMessage("yunadkat1712@gmail.com", "yunadkat1712@gmail.com");
            mm.Subject = user.FirstName + " " + user.LastName + " sent his note for review.";
            mm.Body = "Hello Admins, \n\n We want to inform you that, " + user.FirstName + " " + user.LastName + " sent his note " + sellerNote.Title + " for review. Please look at the notes and take required actions. \n\n\n Regards, \n Notes Marketplace";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;

            NetworkCredential nc = new NetworkCredential("yunadkat1712@gmail.com", "yash@143");
            smtp.EnableSsl = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            return RedirectToAction("Dashboard");
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            if(Session["email"] != null)
            {
                int userid = (int)Session["email"];
                User user = db.Users.FirstOrDefault(m => m.ID == userid);
                ContactUs contactUs = new ContactUs();
                contactUs.EmailID = user.EmailID;
                contactUs.FullName = user.FirstName + " " + user.LastName;
                return View(contactUs);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ContactUs(Models.ContactUs contact)
        {
            MailMessage mm = new MailMessage("yunadkat1712@gmail.com", "yunadkat1712@gmail.com");
            mm.Subject = contact.Subject;
            mm.Body = "Hello " +contact.FullName + ",\n\n "+ contact.Comment +" \n\n \n\nRegards,\n"+contact.FullName;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;

            NetworkCredential nc = new NetworkCredential("yunadkat1712@gmail.com", "yash@143");
            smtp.EnableSsl = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            return View();
        }

        public ActionResult LogIn()
        {
            HttpCookie cookieLogin = Request.Cookies["RememberMe"];
            User user = new User();

            if(cookieLogin != null)
            {
                user.EmailID = cookieLogin["email"];
                user.Password = cookieLogin["pass"];

                if (cookieLogin["email"].Length != 0)
                {
                    user.RememberMe = true;
                }
                else
                {
                    user.RememberMe = false;
                }
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult LogIn(User user)
        {
            HttpCookie cookie = new HttpCookie("RememberMe");
            if (user.RememberMe == true)
            {
                cookie["email"] = user.EmailID;
                cookie["pass"] = user.Password;
                cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookie);
            }
            else
            {
                cookie["email"] = null;
                cookie["pass"] = null;
                Response.Cookies.Add(cookie);
            }

            var auth = db.Users.Where(a => a.EmailID.Equals(user.EmailID) && a.Password.Equals(user.Password)).FirstOrDefault();
            if (auth != null)
            {
                if (auth.IsActive == true)
                {
                    if (auth.IsEmailVerified == true)

                    {
                        ViewBag.IsValid = "true";
                        Session["email"] = auth.ID;
                        Session["fname"] = auth.FirstName;
                        Session["lname"] = auth.LastName;
                        //return Content(user.ID + " = " + Session["email"]);
                        if (auth.IsDetailsSubmitted == true)
                        {
                            
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "pages");
                        }
                    }
                    else
                    {
                        return View(user);
                    }
                }
            }
            
            return View(user);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        //Post Method
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            user.RoleID = 1;
            user.IsEmailVerified = false;
            db.Users.Add(user);
            try
            {
                db.SaveChanges();
                MailMessage mm = new MailMessage("yunadkat1712@gmail.com", "yunadkat1712@gmail.com");
                mm.Subject = "Notes Marketplace Email-Verification";
                mm.Body = "Hello " + user.FirstName + " " + user.LastName + ",\n\nThank you for signing up with us. Please click on below link to verify your email address and to do login. \n\n https://localhost:44369/pages/EmailVerification/?id=" + user.ID + "\n\nRegards,\nNotes Marketplace";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;

                NetworkCredential nc = new NetworkCredential("yunadkat1712@gmail.com", "yash@143");
                smtp.EnableSsl = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                return RedirectToAction("LogIn", "Pages");
            }
            catch (Exception e)
            {
                return View(user);
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(String EmailID)
        {
            var auth = db.Users.Where(a => a.EmailID.Equals(EmailID)).FirstOrDefault();

            if (auth != null)
            {
                String getranpass = NotesMarketplace.RandomPassword.Generate(8, 10);
                //String getranpass = "demo@123";
                MailMessage mail = new MailMessage("yunadkat1712@gmail.com", "yunadkat1712@gmail.com");
                mail.Subject = "New Temporary Password has been created for you";
                string Body = "Hello,\n\nWe have generated a new password for you \nPassword:" + getranpass + "\n\nRegards,\nNotes Marketplace";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;

                NetworkCredential nc = new NetworkCredential("yunadkat1712@gmail.com", "yash@143");
                smtp.EnableSsl = true;
                smtp.Credentials = nc;
                smtp.Send(mail);

                User user = db.Users.FirstOrDefault(x => x.EmailID.Equals(EmailID.ToString()));
                user.Password = getranpass;
                user.ConfirmPassword = getranpass;
                db.SaveChanges();

                return RedirectToAction("LogIn", "pages");
            }
            return RedirectToAction("LogIn", "Home");
        }
        public ActionResult BuyerRequest()
        {
            return View();
        }

        public ActionResult MyDownload()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["email"] != null)
            {
                int userid = (int)Session["email"];
                var userprofile = db.UserProfiles.FirstOrDefault(m => m.User.ID == userid);
                //dashboard dash = db.dashboard.first
                var processnote = db.SellerNotes.ToList();
                return View(processnote);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        public ActionResult EmailVerification(int id)
        {
            User user = db.Users.FirstOrDefault(m => m.ID == id);
            user.ConfirmPassword = user.Password;
            user.IsEmailVerified = true;
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult EmailVerification()
        {
            ViewBag.IsValid = "true";
            return View("LogIn");
        }

        public ActionResult MyRejectedNotes()
        {
            return View();
        }

        public ActionResult MySoldNotes()
        {
            return View();
        }

        public ActionResult NoteDetails(int id, SellerNote sellerNote)
        { 
            if(Session["email"] != null)
            {
                var data = db.SellerNotes.FirstOrDefault(x => x.SellerID == x.User.ID);
                var data1 = db.SellerNotes.Single(x => x.ID == id);
                return View(data1);
            }
            else
            {
                return RedirectToAction("LogIn"); 
            }
        }
        //[HttpPost]
        //public ActionResult NoteDetails(string download)
        //{
        //    if (!string.IsNullOrEmpty(download))
        //    {
        //        if(Session["email"] != null)
        //        {
        //            return View();
        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            int userid = (int)Session["email"];
            User user = db.Users.FirstOrDefault(m => m.ID == userid);
            if (user != null)
            {
                if(user.Password == reset.OldPassword)
                {
                    user.Password = reset.ConfirmNewPassword;
                    user.ConfirmPassword = reset.ConfirmNewPassword;
                    db.SaveChanges();
                    Session["email"] = null;
                    return RedirectToAction("LogIn");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        public ActionResult UserProfile()
        {
            int userid = (int)Session["email"];
            User user = db.Users.FirstOrDefault(m => m.ID == userid);
            if(user!=null)
            {
                UserProfile userProfile = new UserProfile();
                userProfile.User = user;
                return View(userProfile);
            }
            else
            {
                return RedirectToAction("LogIn", "pages");
            }
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfile userprofile)
        {
            int userid = (int)Session["email"];
            User user = db.Users.FirstOrDefault(m => m.ID == userid);
            user.IsDetailsSubmitted = true;
            user.ConfirmPassword = user.Password;
            user.FirstName = userprofile.User.FirstName;
            user.LastName = userprofile.User.LastName;
            user.IsActive = true;
            userprofile.CreatedDate = userprofile.User.CreatedDate;
            userprofile.ModifiedDate = DateTime.Now;
            userprofile.ID = user.P_K_User;
            userprofile.User = user;

            if (userprofile.Profile_Picture != null)
            {
                String path = Server.MapPath("~/Uploads/");
                String Filename = userprofile.Profile_Picture.FileName;
                String finalpath = System.IO.Path.Combine(path + Filename);
                userprofile.Profile_Picture.SaveAs(finalpath);
                userprofile.ProfilePicture = Filename;
                
            }
            db.UserProfiles.Add(userprofile);
            db.SaveChanges();
            return RedirectToAction("SearchNotes", "pages");
        }
        public ActionResult LogOut()
        {
            Session["email"] = null;
            Session["fname"] = null;
            Session["lname"] = null;
            return RedirectToAction("LogIn","pages");
        }

    }
}