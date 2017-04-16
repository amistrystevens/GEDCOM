using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;

namespace GedComUnitTest
{
    public class US22_23Tests
    {
        [Fact]
        public void testSameIDIndi()
        {
            INDI i1 = new INDI();
            INDI i2 = new INDI();

            List<INDI> indi = new List<INDI>();
            List<string> results = new List<string>();

            //case2
            i1.Name = "tester1";
            i1.ID = "@I1";
            i2.Name = "tester2";
            i2.ID = "@I1";

            indi.Add(i1);
            indi.Add(i2);

            bool actual = Gedcomreader_Project003.Program.isIDUniqueIndi(indi, ref results);

            Assert.Equal(false, actual);

            indi[0].ID = "@I2";

            actual = Gedcomreader_Project003.Program.isIDUniqueIndi(indi, ref results);

            Assert.Equal(true, actual);
        }

        [Fact]
        public void testSameIDFam()
        {
            FAM f1 = new FAM();
            FAM f2 = new FAM();

            List<FAM> fam = new List<FAM>();
            List<string> results = new List<string>();
            //case2
           
            f1.FamID = "@F1";
            f2.FamID = "@F1";

            fam.Add(f1);
            fam.Add(f2);

            bool actual = Gedcomreader_Project003.Program.isIDUniqueFam(fam, ref results);

            Assert.Equal(false, actual);

            fam[0].FamID = "@F2";

            actual = Gedcomreader_Project003.Program.isIDUniqueFam(fam, ref results);

            Assert.Equal(true, actual);
        }

        [Fact]
        public void testNameBirthdayUnique()
        {
            INDI i1 = new INDI();
            INDI i2 = new INDI();

            List<INDI> indi = new List<INDI>();
            List<string> namesResults = new List<string>();
            List<string> birthdaysResults = new List<string>();

            //case2
            i1.Name = "tester1";
            i1.ID = "@I1";
            i1.BirthDay = "19 MAR 2016";
            i2.Name = "tester1";
            i2.ID = "@I2";
            i2.BirthDay = "19 MAR 2016";

            indi.Add(i1);
            indi.Add(i2);

            bool actual = Gedcomreader_Project003.Program.isNameBirthdayUnique(indi, ref namesResults, ref birthdaysResults);

            Assert.Equal(false, actual);

            indi[0].BirthDay = "20 MAR 2016";

            actual = Gedcomreader_Project003.Program.isNameBirthdayUnique(indi, ref namesResults, ref birthdaysResults);

            Assert.Equal(true, actual);
        }
    }
}
