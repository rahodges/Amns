using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amns.GreyFox.People;
using Test.Data;

namespace Amns.GreyFox.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ContactTest
    {
        private GreyFoxContact contactA;
        private GreyFoxContact contactB;
        private GreyFoxContact contactC;

        public ContactTest()
        {
            try
            {
                Amns.GreyFox.Data.MsJetUtility.CreateDB("Test.mdb");
            }
            catch {}

            try
            {
                GreyFoxContactManager manager = new GreyFoxContactManager("TestContacts");
                manager.CreateTable();
            }
            catch { }

            // For load testing please make sure "Run unit tests in application domain" is set to
            // true in the Run Settings!

            contactA = PeopleTestData.GetInstance().ContactA;
            contactB = PeopleTestData.GetInstance().ContactB;
            contactC = PeopleTestData.GetInstance().ContactC;                
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
                
        private void parseTest(string name,
            string prefix, string first, string middle, string last, string suffix)
        {
            GreyFoxContact contact;
            bool pass;

            contact = new GreyFoxContact();
            contact.ParseName(name);
            
            pass = contact.Prefix == prefix &&
                contact.FirstName == first &&
                contact.MiddleName == middle &&
                contact.LastName == last &&
                contact.Suffix == suffix;

            if (!pass)
            {
                throw new Exception("Failed name parse.");
            }
        }

        [TestMethod]
        public void OrderTestA()
        {
            parseTest("Robert Lee", "", "Robert", "", "Lee", "");
            parseTest("Lee,Robert", "", "Robert", "", "Lee", "");
            parseTest("Robert E Lee", "", "Robert", "E", "Lee", "");
            parseTest("Robert E. Lee", "", "Robert", "E.", "Lee", "");
            parseTest("Lee, Robert E.", "", "Robert", "E.", "Lee", "");
            parseTest("Lee,Robert E.", "", "Robert", "E.", "Lee", "");

            parseTest("robert lee", "", "Robert", "", "Lee", "");
            parseTest("lee,robert", "", "Robert", "", "Lee", "");
            parseTest("robert e lee", "", "Robert", "E", "Lee", "");
            parseTest("robert e. lee", "", "Robert", "E.", "Lee", "");
            parseTest("lee, robert e.", "", "Robert", "E.", "Lee", "");
            parseTest("lee,robert e.", "", "Robert", "E.", "Lee", "");

            parseTest("robErt leE", "", "Robert", "", "Lee", "");
            parseTest("lee,roberT", "", "Robert", "", "Lee", "");
            parseTest("roBErt e lEe", "", "Robert", "E", "Lee", "");
            parseTest("Robert E. lee", "", "Robert", "E.", "Lee", "");
            parseTest("lee, robert e.", "", "Robert", "E.", "Lee", "");
            parseTest("lee,RobErt e.", "", "Robert", "E.", "Lee", "");
        }

        [TestMethod]
        public void CreateContactA()
        {
            contactA.Save();
        }

        [TestMethod]
        public void CreateContactB()
        {
            contactB.Save();
        }

        [TestMethod]
        public void CreateContactC()
        {
            contactC.Save();
        }

        [TestMethod]
        public void SaveLoadContactA()
        {
            GreyFoxContact loadContact;
            contactA.Save();
            loadContact = new GreyFoxContact(contactA.TableName, contactA.ID);
            if (contactA.FirstName != loadContact.FirstName ||
                contactA.MiddleName != loadContact.MiddleName ||
                contactA.LastName != loadContact.LastName ||
                contactA.DisplayName != loadContact.DisplayName ||
                contactA.Address1 != loadContact.Address1 ||
                contactA.Address2 != loadContact.Address2 ||
                contactA.City != loadContact.City ||
                contactA.StateProvince != loadContact.StateProvince ||
                contactA.Country != loadContact.Country ||
                contactA.HomePhone != loadContact.HomePhone ||
                contactA.WorkPhone != loadContact.WorkPhone ||
                contactA.BirthDate != loadContact.BirthDate ||
                contactA.ContactMethod != loadContact.ContactMethod)
            {
                throw new Exception("Contact saved incorrectly.");
            }
        }

        [TestMethod]
        public void DeleteContactA()
        {
            contactA.Save();
            contactA.Delete();
        }

        [TestMethod]
        public void DeleteContactB()
        {
            contactB.Save();
            contactB.Delete();
        }

        [TestMethod]
        public void DeleteContactC()
        {
            contactC.Save();
            contactC.Delete();
        }

        [TestMethod]
        public void ModifyContactA()
        {
            contactA.Save();
            contactA.ParseName("Alexander E. Smithy");
            contactA.Save();
        }

        [TestMethod]
        public void ModifyContactB()
        {
            contactB.Save();
            contactB.ParseName("Alexander E. Smithy");
            contactB.Save();
        }

        [TestMethod]
        public void ModifyContactC()
        {
            contactC.Save();
            contactC.ParseName("Alexander E. Smithy");
            contactC.Save();
        }

        private GreyFoxContactCollection generateContacts()
        {
            GreyFoxContactCollection contacts = new GreyFoxContactCollection();
            for (int i = 0; i < 100; i++)
            {
                contacts.Add(contactA.Copy());
                contacts.Add(contactB.Copy());
                contacts.Add(contactC.Copy());
            }
            return contacts;
        }

        private void saveContacts(GreyFoxContactCollection contacts)
        {
            foreach (GreyFoxContact contact in contacts)
            {
                contact.Save();
            }
        }

        [TestMethod]
        public void Save300Contacts()
        {
            DateTime start;
            GreyFoxContactCollection contacts;    

            start = DateTime.Now;
            contacts = generateContacts();
            saveContacts(contacts);            
            TestContext.WriteLine((DateTime.Now - start).ToString());

            start = DateTime.Now;
            contacts = generateContacts();
            saveContacts(contacts);
            TestContext.WriteLine((DateTime.Now - start).ToString());

            start = DateTime.Now;
            contacts = generateContacts();
            saveContacts(contacts);
            TestContext.WriteLine((DateTime.Now - start).ToString());

            start = DateTime.Now;
            contacts = generateContacts();
            saveContacts(contacts);
            TestContext.WriteLine((DateTime.Now - start).ToString());

            start = DateTime.Now;
            contacts = generateContacts();
            saveContacts(contacts);
            TestContext.WriteLine((DateTime.Now - start).ToString());
        }

        [TestMethod]
        public void GetContacts()
        {
            GreyFoxContactManager cm = new GreyFoxContactManager("TestContacts");
            GreyFoxContactCollection contacts = cm.GetCollection(100, string.Empty, string.Empty);
        }

        [TestMethod]
        public void GetContactsSortName()
        {
            GreyFoxContactManager cm;
            GreyFoxContactCollection contacts;
            DateTime time;
            
            cm = new GreyFoxContactManager("TestContacts");

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, FirstName, MiddleName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName, Suffix");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName");
            TestContext.WriteLine((DateTime.Now - time).ToString());

            time = DateTime.Now;
            contacts = cm.GetCollection(100, string.Empty, "LastName, MiddleName, FirstName");
            TestContext.WriteLine((DateTime.Now - time).ToString());
        }
    }
}
