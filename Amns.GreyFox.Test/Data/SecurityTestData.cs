using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amns.GreyFox.People;
using Amns.GreyFox.Security;

namespace Test.Data
{
    public class SecurityTestData
    {
        private GreyFoxUser userA;
        private GreyFoxUser userB;
        private GreyFoxUser userC;

        private GreyFoxRole roleAdmin;
        private GreyFoxRole roleUser;
        private GreyFoxRole roleGuest;

        public GreyFoxUser UserA { get { return userA; } }
        public GreyFoxUser UserB { get { return userB; } }
        public GreyFoxUser UserC { get { return userC; } }

        public GreyFoxRole RoleAdmin { get { return roleAdmin; } }
        public GreyFoxRole RoleUser { get { return roleUser; } }
        public GreyFoxRole RoleGuest { get { return roleGuest; } }

        public SecurityTestData()
        {   
            roleAdmin = new GreyFoxRole();
            roleAdmin.Name = "Administrator";
            roleAdmin.Description = "System Administrators";
            roleAdmin.IsDisabled = false;

            roleUser = new GreyFoxRole();
            roleUser.Name = "User";
            roleUser.Description = "System Users";
            roleUser.IsDisabled = false;

            roleGuest = new GreyFoxRole();
            roleGuest.Name = "Guest";
            roleGuest.Description = "System Guest";
            roleGuest.IsDisabled = true;

            userA = new GreyFoxUser();
            userA.Contact = new GreyFoxContact(GreyFoxUserManager.ContactTable);
            PeopleTestData.GetInstance().ContactA.CopyValuesTo(userA.Contact, true);
            userA.IsDisabled = false;
            userA.LoginPassword = "ESPLATIO324!";
            userA.UserName = userA.CreateUserName();
            userA.Roles = new GreyFoxRoleCollection();
            userA.Roles.Add(roleAdmin);            
            userA.LoginDate = DateTime.Now;
            userA.Encrypt();

            userB = new GreyFoxUser();
            userB.Contact = new GreyFoxContact(GreyFoxUserManager.ContactTable);
            PeopleTestData.GetInstance().ContactB.CopyValuesTo(userB.Contact, true);
            userB.IsDisabled = true;
            userB.LoginPassword = "IDONTKN0W!!!THIS";
            userB.UserName = userB.CreateUserName();
            userB.Roles = new GreyFoxRoleCollection();
            userB.Roles.Add(roleUser);
            userB.LoginDate = DateTime.Now;
            userB.Encrypt();

            userC = new GreyFoxUser();
            userC.Contact = new GreyFoxContact(GreyFoxUserManager.ContactTable);
            PeopleTestData.GetInstance().ContactC.CopyValuesTo(userC.Contact, true);
            userC.IsDisabled = false;
            userC.LoginPassword = "SILLYPASSWRD!$12!";
            userC.UserName = userC.CreateUserName();
            userC.Roles = new GreyFoxRoleCollection();
            userC.Roles.Add(roleUser);
            userC.LoginDate = DateTime.Now;
            userC.Encrypt();
        }

        public static SecurityTestData GetInstance()
        {
            return Nested.instance;
        }

        class Nested
        {
            static Nested() { }
            internal static readonly SecurityTestData instance =
                new SecurityTestData();
        }
    }
}
