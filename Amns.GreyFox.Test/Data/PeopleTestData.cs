using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amns.GreyFox.People;

namespace Test.Data
{
    public class PeopleTestData
    {
        private GreyFoxContact contactA;
        private GreyFoxContact contactB;
        private GreyFoxContact contactC;

        public GreyFoxContact ContactA { get { return contactA; } }
        public GreyFoxContact ContactB { get { return contactB; } }
        public GreyFoxContact ContactC { get { return contactC; } }

        public PeopleTestData()
        {
            contactA = new GreyFoxContact("TestContacts");
            contactA.ParseName("Alexander E. Smith");
            contactA.DisplayName = "Alexander E. Smith";
            contactA.Address1 = "5555 Somewhere St.";
            contactA.City = "Anytown";
            contactA.StateProvince = "VA";
            contactA.PostalCode = "55555";
            contactA.Country = "USA";
            contactA.HomePhone = "(555)555-5555";
            contactA.WorkPhone = "(555)555-5555";
            contactA.BirthDate = new DateTime(1954, 3, 20);
            contactA.ContactMethod = GreyFoxContactMethod.WorkPhone;

            contactB = new GreyFoxContact("TestContacts");
            contactB.ParseName("Frank Corndog");
            contactB.DisplayName = "Frank Corndog";
            contactB.Address1 = "5555 Somewhere St.";
            contactB.City = "Anytown";
            contactB.StateProvince = "VA";
            contactB.PostalCode = "55555";
            contactB.Country = "USA";
            contactB.HomePhone = "(555)555-5555";
            contactB.WorkPhone = "(555)555-5555";
            contactB.BirthDate = new DateTime(1978, 6, 14);
            contactB.ContactMethod = GreyFoxContactMethod.Email;

            contactC = new GreyFoxContact("TestContacts");
            contactC.ParseName("Professor Martin E. Shorty III");
            contactC.DisplayName = "Professor Martin E. Shorty III";
            contactC.Address1 = "5555 Somewhere St.";
            contactC.City = "Anytown";
            contactC.StateProvince = "VA";
            contactC.PostalCode = "55555";
            contactC.Country = "USA";
            contactC.HomePhone = "(555)555-5555";
            contactC.WorkPhone = "(555)555-5555";
            contactC.ContactMethod = GreyFoxContactMethod.Email;
            contactC.BirthDate = new DateTime(1952, 12, 10);
        }

        public static PeopleTestData GetInstance()
        {
            return Nested.instance;
        }

        class Nested
        {
            static Nested() { }
            internal static readonly PeopleTestData instance =
                new PeopleTestData();
        }
    }
}
