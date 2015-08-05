using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace PF.Presentation.WebClient.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private OAuthe2Client oauth;

        public AccountController()
        {
            oauth = new OAuthe2Client("Any", "Secret", "http://localhost:1538/Account/Callback");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return ReturnToURL();
            }

            Session["returnUrl"] = returnUrl;

            return Redirect(oauth.GetAuthorizeUrl());
        }

        //
        // GET: /Account/Callback
        [AllowAnonymous]
        public ActionResult Callback()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return ReturnToURL();
            }
            string code = Request.Params.Get("code");
            string token = oauth.GetAccessTokenByAuthorizationCode(code);

            if (!string.IsNullOrEmpty(token))
            {
                ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim(ClaimTypes.Name, "Authorized User"));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Authorized User"));
                identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
                identity.AddClaim(new Claim("DisplayName", "Authorized User"));
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                return ReturnToURL();
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult ReturnToURL()
        {
            if (Session["returnUrl"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(Session["returnUrl"].ToString());
            }
        }


        #endregion
    }
}