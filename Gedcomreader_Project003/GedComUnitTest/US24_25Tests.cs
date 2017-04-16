using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;

namespace GedComUnitTest
{
    public class US24_25Tests
    {
        [Fact]
        public void testUniqueFamilies()
        {
            FAM f1 = new FAM();
            f1.Husbandname = "George Smith";
            f1.Wifename = "Kelly Smith";
            f1.Married = "13 MAR 1934";
            FAM f2 = new FAM();
            f2.Husbandname = "George Smith";
            f2.Wifename = "Kelly Smith";
            f2.Married = "13 MAR 1934";

            List<FAM> family = new List<FAM>();
            family.Add(f1);
            family.Add(f2);

            List<FAM> familyResults = new List<FAM>();

            bool actual = Gedcomreader_Project003.Program.isUniqueFamily(family, ref familyResults);

            Assert.Equal(false, actual);

            family[1].Married = "13 MAR 1935";

            actual = Gedcomreader_Project003.Program.isUniqueFamily(family, ref familyResults);

            Assert.Equal(true, actual);
        }

        [Fact]
        public void testUniqueNames()
        {
            INDI i1 = new INDI();
            i1.ID = "107";
            i1.Name = "Bobby Joe";
            i1.BirthDay = "Birthday";

            INDI i2 = new INDI();
            i2.ID = "108";
            i2.Name = "Bobby Joe";
            i2.BirthDay = "Birthday";

            List <INDI> indi = new List<INDI>();
            indi.Add(i1);
            indi.Add(i2);

            FAM fam = new FAM();
            fam.childeren = "107,108";

            List<INDI> results = new List<INDI>();

            bool actual = Gedcomreader_Project003.Program.isUniqueFamilyNames(fam, indi, ref results);

            Assert.Equal(false, actual);

            indi[1].Name = "Billy Joe";

            actual = Gedcomreader_Project003.Program.isUniqueFamilyNames(fam, indi, ref results);

            Assert.Equal(true, actual);
        }   
    }
}