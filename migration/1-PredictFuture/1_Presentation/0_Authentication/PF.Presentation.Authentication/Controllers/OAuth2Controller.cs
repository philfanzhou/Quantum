using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Core.Infrastructure.Crosscutting.Service;
using PF.DistributedService.Contracts.Crosscutting;
using PF.DistributedService.Contracts.UserContext;
using Core.DistributedServices.OAuth2;
using PF.Presentation.Authentication.Models;

namespace PF.Presentation.Authentication.Controllers
{
    [Authorize]
    public class OAuth2Controller : Controller
    {
        private ServiceClient<IOAuth2AuthorizationServer> OAuthService;
        private ServiceClient<IAuthenticationService> AuthenticationService;
        private ServiceClient<ICrosscuttingService> CrosscuttingService;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public OAuth2Controller()
        {
            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:9192/PredictFuture/OAuth2AuthorizationServer/");
            OAuthService = new ServiceClient<IOAuth2AuthorizationServer>(binding, address);

            binding = new NetTcpBinding();
            address = new EndpointAddress("net.tcp://localhost:9193/PredictFuture/AuthenticationService/");
            AuthenticationService = new ServiceClient<IAuthenticationService>(binding, address);

            binding = new NetTcpBinding();
            address = new EndpointAddress("net.tcp://localhost:9193/PredictFuture/CrosscuttingService/");
            CrosscuttingService = new ServiceClient<ICrosscuttingService>(binding, address);
        }

        //
        // GET: /OAuth2/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        #region Login

        // GET: /OAuth2/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /OAuth2/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // To do: should encrypt at client side
                if (AuthenticationService.Invoke<bool>(p => p.validate(model.UserName, Encrypt(model.Password))))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.UserName));
                    identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
                    identity.AddClaim(new Claim("DisplayName", model.UserName));
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        #endregion

        #region Register

        //
        // GET: /OAuth2/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // GET: /OAuth2/VerifyMail
        [AllowAnonymous]
        public ActionResult VerifyMail()
        {
            string id = Request.Params["id"];
            string code = Request.Params["code"];
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Verified = false;
            if (CrosscuttingService.Invoke<bool>(c => c.VerifyCaptcha("MailVerify_" + id, code)))
            {
                ViewBag.Verified = true;
            }
            return View();
        }

        //
        // POST: /OAuth2/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (AuthenticationService.Invoke<bool>(p => p.register(model.UserMail, Encrypt(model.Password))))
                {
                    string key = "MailVerify_" + model.UserMail;
                    string code = CrosscuttingService.Invoke<string>(c => c.GenerateCaptcha(key));
                    string body = string.Format(@"Open below link to complete verification: {0} http://{1}{2}?id={3}&code={4}", Environment.NewLine, Request.Url.Authority, Url.Action("VerifyMail"), model.UserMail, code);
                    ViewBag.UserMail = model.UserMail;
                    ViewBag.MailSent = CrosscuttingService.Invoke<bool>(c => c.SendMail(model.UserMail, "Mail Verification", body));
                    return View("RegisterSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Register failed.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        //
        // POST: /OAuth2/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #region OAuth2

        // GET: /OAuth2/authorize
        public ActionResult authorize()
        {
            //string response_type = "code";
            string client_id = Request.Params.Get("client_id");
            string redirect_uri = Request.Params.Get("redirect_uri");
            string scope = Request.Params.Get("scope");
            string state = Request.Params.Get("state");

            AuthorizationRequest request = new AuthorizationRequest()
            {
                ClientIdentifier = client_id,
                Scope = scope,
                RedirectURI = redirect_uri
            };

            string code = OAuthService.Invoke<string>(o => o.GetAuthorizationCode(request));

            return Redirect(string.Format("{0}?code={1}&state={2}", redirect_uri, code, state));
        }

        // GET: /OAuth2/access_token
        [AllowAnonymous]
        public JsonResult access_token()
        {
            //string grant_type = "authorization_code";
            string client_id = Request.Params.Get("client_id");
            string client_secret = Request.Params.Get("client_secret");
            string redirect_uri = Request.Params.Get("redirect_uri");
            string code = Request.Params.Get("code");
            if (code.Contains(","))
            {
                code = code.Split(',')[0];
            }

            AccessToken token = OAuthService.Invoke<AccessToken>(o => o.GetAccessToken(code, client_secret, User.Identity.GetUserName()));

            return Json(new { access_token = token.Token, token_type = "PF" });
        }
        
        #endregion

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString().ToUpper();
            }
        }

        #endregion
    }
}