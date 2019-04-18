using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amns.GreyFox.People;
using Amns.GreyFox.Security;
using Test.Data;

namespace Amns.GreyFox.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SecurityTest
    {
        public SecurityTest()
        {
            try
            {
                Amns.GreyFox.Data.MsJetUtility.CreateDB("Test.mdb");
            }
            catch { }
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateTable()
        {

        }

        [TestMethod]
        public void TestUserAccounts()
        {
            GreyFoxUserManager um = new GreyFoxUserManager();
            um.CreateTable();
            GreyFoxRoleManager rm = new GreyFoxRoleManager();
            rm.CreateTable();
            um.CreateReferences();

            int userAID;
            int userBID;
            int userCID;

            bool adminRoleAdded;

            SecurityTestData.GetInstance().RoleAdmin.Save();
            SecurityTestData.GetInstance().RoleUser.Save();
            SecurityTestData.GetInstance().RoleGuest.Save();

            userAID = SecurityTestData.GetInstance().UserA.Save();
            userBID = SecurityTestData.GetInstance().UserB.Save();
            userCID = SecurityTestData.GetInstance().UserC.Save();

            GreyFoxUser user = new GreyFoxUser(userBID);
            user.Roles.Add(SecurityTestData.GetInstance().RoleAdmin);
            user.Save();

            user = null;

            user = new GreyFoxUser(userBID);

            adminRoleAdded = false;

            foreach (GreyFoxRole role in user.Roles)
            {
                if (role.Name == SecurityTestData.GetInstance().RoleAdmin.Name)
                {
                    adminRoleAdded = true;
                }
            }

            if (!adminRoleAdded)
                throw new Exception("Admin role was not correctly added.");

            // Delete Users - 'User Roles should be clear
            SecurityTestData.GetInstance().UserA.Delete();
            SecurityTestData.GetInstance().UserB.Delete();
            SecurityTestData.GetInstance().UserC.Delete();

            // Delete Roles - 'User Roles should be clear
            SecurityTestData.GetInstance().RoleAdmin.Delete();
            SecurityTestData.GetInstance().RoleUser.Delete();
            SecurityTestData.GetInstance().RoleGuest.Delete();
        }
    }
}