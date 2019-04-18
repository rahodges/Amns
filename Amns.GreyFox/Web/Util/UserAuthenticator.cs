using System;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Web.Util
{
    public sealed class UserAuthenticator
    {
        /// <summary>
        /// Authenticates a user by creating a cookie for FormsAuthentication
        /// and setting the Principal and roles for immediate use.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="persist"></param>
        public static void Authenticate(GreyFoxUser user, bool persist)
        {
            string hash;
            DateTime expireDate;
            FormsAuthenticationTicket authTicket;
            HttpCookie authCookie;

            // Initialize Forms Authentication
            FormsAuthentication.Initialize();

            // Set Persistence Cookie Expirations
            if (persist)
                expireDate = DateTime.Now.AddYears(1);
            else
                expireDate = DateTime.Now.AddDays(2);

            // Create Ticket
            authTicket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.Now,
                expireDate,
                persist,
                String.Join(",", user.Roles.RolesNamesToArray()),
                FormsAuthentication.FormsCookiePath);

            // Encrypt Ticket
            hash = FormsAuthentication.Encrypt(authTicket);

            // Set Cookie
            authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                hash);

            if (persist)
            {
                authCookie.Expires = authTicket.Expiration;
            }

            // Add Cookie To Response
            HttpContext.Current.Response.Cookies.Add(authCookie);

            // Set Principal
            FormsIdentity ident;
            ident = new FormsIdentity(authTicket);
            GenericPrincipal principal;
            principal = new GenericPrincipal(ident, 
                user.Roles.RolesNamesToArray());
            HttpContext.Current.User = principal;
        }

        /// <summary>
        /// Reauthenticates a user for a page request by setting the current
        /// user context's roles to the proper roles from the cookie, otherwise 
        /// the roles will be left blank. This should only be used in the
        /// Global.asax AuthenticateRequest method.
        /// </summary>
        public static void ReAuthenticate()
        {
            HttpContext context = HttpContext.Current;

            if (context.User == null)
            {
                return;
            }

            HttpCookie authCookie =
                context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null)
            {
                return;
            }

            FormsAuthenticationTicket authTicket = null;

            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            if (authTicket == null)
            {
                return;
            }

            string[] roles = authTicket.UserData.Split(new char[] { ',' });

            FormsIdentity id;
            id = new FormsIdentity(authTicket);
            GenericPrincipal principal;
            principal = new GenericPrincipal(id, roles);
            context.User = principal;
        }

        /// <summary>
        /// Deauthenticates the user immediately. This method will force a 
        /// redirect to the login page for security.
        /// </summary>
        public static void DeAuthenticate()
        {
            FormsAuthentication.SignOut();

            FormsAuthentication.RedirectToLoginPage();
        }
    }
}
