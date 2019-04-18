using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Utilities
{
    public class SecurityManager
    {
        public static GreyFoxUser GetUser(string username)
        {
            GreyFoxUserManager userManager;
            GreyFoxUserCollection users;
            userManager = new GreyFoxUserManager();
            users = userManager.GetCollection(
                "UserName='" + username.Replace("'", "''") + "'", string.Empty);
            if (users.Count > 0)
                return users[0];
            return null;
        }

        public static GreyFoxUser NewUser(string username)
        {
            GreyFoxUser user = new GreyFoxUser();
            user.UserName = username;
            user.LoginPassword = GreyFoxPassword.CreateRandomPassword(15);
            user.Contact = new Amns.GreyFox.People.GreyFoxContact(GreyFoxUserManager.ContactTable);
            user.Encrypt();
            return user;
        }

        public static GreyFoxRole GetRole(string rolename)
        {
            GreyFoxRoleManager roleManager;
            GreyFoxRoleCollection roles;
            roleManager = new GreyFoxRoleManager();
            roles = roleManager.GetCollection(
                "Name='" + rolename.Replace("'", "''") + "'", string.Empty);
            if (roles.Count > 0)
                return roles[0];
            return null;
        }
    }
}
