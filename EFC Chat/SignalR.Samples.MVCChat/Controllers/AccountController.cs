/* 
 * AccountController.cs- main class of the main uses of the users functionality and usability. 
 * Conatains the main methods to work with users signing in.
 * Written by: ALL EFC Team, as each has a part of specific methods.
 * Used for login screen.
 * Some methods are built by default when creating a MVC3 project.
 *  
 */
using System;
using System.Web.Mvc;
using System.Web.Security;
using SignalR.Samples.MVCChat.Models;
using System.IO;
using SignalR.Samples.MVCChat.Hubs.Chat;


namespace SignalR.Samples.MVCChat.Controllers

{
   
    public class AccountController : Controller
    {
        public int LoggedUser = 0;
        private String file_path = @"d:\users.txt";  //The path of which the users file will be saved.
                                                    //file format:  username \t\t password.
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //This method varifies if a user exists in a the users file. if exists return -1, else retunr 0.
        private int user_exist(string user, string pass)
        {
            string[] lines = System.IO.File.ReadAllLines(file_path);
            string text = System.IO.File.ReadAllText(file_path);

            StreamReader SR;
            string S;
            SR = System.IO.File.OpenText(file_path);
            S = SR.ReadLine();
            while (S != null)
            {

                if (S == (user + "\t\t" + pass)) // User exists, return -1.
                {
                    SR.Close();
                    return -1;
                }
                S = SR.ReadLine();
            }
            SR.Close();
            return 0;
        }
        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl) //
        {
            if (ModelState.IsValid)
            {
                if (user_exist(model.UserName.ToString(), model.Password.ToString()) == (-1)) // If user doesn't exist, insert to the users file and direct it to the chat page.
                {                   
               
                    //using SignalR.Samples.MVCChat.Hubs.Chat.Chat("/nick" + model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        LoggedUser = 1;                        
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        LoggedUser = 1;
                        
                        return RedirectToAction("Index", "Chat");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
         
                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    if (user_exist(model.UserName.ToString(), model.Password.ToString()) == 0)
                    {
                        System.IO.File.AppendAllText(file_path, model.UserName.ToString() + "\t\t" + model.Password.ToString() + "\r\n");
                        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User");
                    }
                }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
