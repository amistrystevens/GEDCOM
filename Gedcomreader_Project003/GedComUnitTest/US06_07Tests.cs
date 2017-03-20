using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;

namespace GedComUnitTest
{
    public class US06_07Tests
    {
        [Fact]
        public void testTooOld()
        {
            INDI i = new INDI();

            //case2
            i.Name = "tester1";
            i.age = 18;
            i.BirthDay = "12 MAR 1900";
            i.Dead = false;
            i.death = "12 MAR 2051";
            i.Sex = "M";


            //Test if the user is actually dead (he isn't
            bool actual = Gedcomreader_Project003.Program.isTooOld(i);

            Assert.Equal(true, actual);

            i.death = "12 MAR 2030";

            //Test if the user is actually dead (he isn't
            actual = Gedcomreader_Project003.Program.isTooOld(i);

            Assert.Equal(false, actual);

        }

        [Fact]
        public void testDivorceBeforeDeath()
        {
            FAM f = new FAM();

            //case2
            f.Death = "12 MAR 1900";
            f.Divorced = "12 MAR 1901";


            //Test if the user is actually dead (he isn't
            bool actual = Gedcomreader_Project003.Program.IsValidDateforDivorceBeforeDeath(f);

            Assert.Equal(false, actual);

            f.Divorced = "12 MAR 1899";

            //Test if the user is actually dead (he isn't
            actual = Gedcomreader_Project003.Program.IsValidDateforDivorceBeforeDeath(f);

            Assert.Equal(true, actual);

        }
    }
}
