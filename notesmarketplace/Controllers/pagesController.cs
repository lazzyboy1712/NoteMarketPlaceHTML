using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using notesmarketplace.Models;

namespace notesmarketplace.Controllers
{
    public class pagesController : Controller
    {
        NotesMarketPlaceEntities db = new NotesMarketPlaceEntities();

        // GET: pages
        public ActionResult SearchNotes()
        {
            return View();
        }

        public ActionResult AddNotes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNotes(SellerNote sellerNote)
        {
            sellerNote.IsActive = true;
            sellerNote.CreatedDate = DateTime.Now;
            sellerNote.Title = sellerNote.Title;
            sellerNote.Category = 1;
            sellerNote.NoteType = 1;
            sellerNote.NumberOfPage = sellerNote.NumberOfPage;
            sellerNote.Description = sellerNote.Description;
            sellerNote.Country = sellerNote.Country;
            sellerNote.InstitutionName = sellerNote.InstitutionName;
            sellerNote.Course = sellerNote.Course;
            sellerNote.CourseCode = sellerNote.CourseCode;
            sellerNote.Professor = sellerNote.Professor;
            sellerNote.SellingPrice = sellerNote.SellingPrice;
            sellerNote.SellerID = 10;
            sellerNote.Status = 1;
            sellerNote.IsPaid = true;
            db.SellerNotes.Add(sellerNote);
            if (sellerNote.Display_Picture != null)
            {
                String path = Server.MapPath("~/Notes/DisplayPicture/");
                String Filename = "DP"+DateTime.Now.ToString("yyyyMMddmmss")+System.IO.Path.GetExtension(sellerNote.Display_Picture.FileName);
                String finalpath = System.IO.Path.Combine(path + Filename);
                sellerNote.Display_Picture.SaveAs(finalpath);
                sellerNote.DisplayPicture = Filename;

            }
            if (sellerNote.Upload_Note != null)
            {
                String path = Server.MapPath("~/Notes/");
                String Filename = "Note" + DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(sellerNote.Upload_Note.FileName);
                String finalpath1 = System.IO.Path.Combine(path + Filename);
                sellerNote.Upload_Note.SaveAs(finalpath1);
                sellerNote.UploadNote = Filename;

            }
            if (sellerNote.Note_Preview != null)
            {
                String path = Server.MapPath("~/Notes/NotePreview/");
                String Filename = "NotePreview"+DateTime.Now.ToString("yyyyMMddhhmmss")+System.IO.Path.GetExtension(sellerNote.Note_Preview.FileName);
                String finalpath2 = System.IO.Path.Combine(path + Filename);
                sellerNote.Note_Preview.SaveAs(finalpath2);
                sellerNote.NotePreview = Filename;

            }

            db.SaveChanges();

            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
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
                        if (auth.IsDetailsSubmitted == true)
                        {
                            Session["email"] = user.ID;
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
            return View();
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

        public ActionResult NoteDetails()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            User user = db.Users.FirstOrDefault(m=>m.ID==10);
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
            User user = db.Users.FirstOrDefault(m => m.ID == 10);
            user.IsDetailsSubmitted = true;
            user.ConfirmPassword = user.Password;
            user.FirstName = userprofile.User.FirstName;
            user.LastName = userprofile.User.LastName;
            user.IsActive = true;
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
    }
}