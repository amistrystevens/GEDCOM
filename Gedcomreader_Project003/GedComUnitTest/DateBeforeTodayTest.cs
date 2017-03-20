using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;

namespace GedComUnitTest
{
    public class DateBeforeTodayTest
    {
        [Fact]
        public void testNullData()
        {
            //Console.WriteLine("test");
            INDI i = new INDI();
            FAM f = new FAM();
            //case1
            i.Name = "tester1";
            i.age = 18;
            i.BirthDay = null;
            i.Dead = false;
            i.death = null;
            i.Sex = "M";

            f.FamID = "test";
            f.Divorced = null;
            f.Married = null;
            //Assert.Equal(true, actual);

            List<INDI> listIndi = new List<INDI>();
            listIndi.Add(i);

            List<FAM> listFam = new List<FAM>();
            listFam.Add(f);

            //
            Assert.NotNull(i);
            Assert.NotNull(f);

            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayIndi(ref i);

            Assert.Equal(false, actual);

            actual = Gedcomreader_Project003.Program.isDateAfterTodayFam(ref f);

            Assert.Equal(false, actual);


            Assert.False(Gedcomreader_Project003.Program.isDateAfterTodayIndi(ref i));
            Assert.False(Gedcomreader_Project003.Program.isDateAfterTodayFam(ref f));

            Assert.IsType<INDI>(i);
            Assert.IsType<FAM>(f);
        }

        [Fact]
        public void testNotBeforeToday()
        {
            //Console.WriteLine("test");
            INDI i = new INDI();
            FAM f = new FAM();
            //case1
            i.Name = "tester1";
            i.age = 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = true;
            i.death = "12 MAR 1997";
            i.Sex = "M";

            f.FamID = "test";
            f.Divorced = "12 MAR 1995";
            f.Married = "12 MAR 1997";
            //Assert.Equal(true, actual);

            List<INDI> listIndi = new List<INDI>();
            listIndi.Add(i);

            List<FAM> listFam = new List<FAM>();
            listFam.Add(f);

            DateTime day = new DateTime(1996, 3, 24);

            //Test if the user is actually dead (he isn't
            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayIndi(ref i);

            Assert.Equal(false, actual);

            actual = Gedcomreader_Project003.Program.isDateAfterTodayFam(ref f);

            Assert.Equal(false, actual);
        }

        [Fact]
        public void testUserAfterTodayDeath()
        {
            INDI i = new INDI();

            //case3
            i.Name = "tester2";
            i.age = 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = true;
            i.death = "13 MAR 2022";
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayIndi(ref i);

            Assert.Equal(true, actual);




        }

        //case4
        [Fact]
        public void testUserAfterTodayBirth()
        {
            INDI i = new INDI();

            //case4
            i.Name = "tester2";
            i.age = 18;
            i.BirthDay = "12 MAR 2025";
            i.Dead = true;
            i.death = "13 MAR 1997";
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayIndi(ref i);

            Assert.Equal(true, actual);
        }

        //case5
        [Fact]
        public void testUserAfterTodayMarriage()
        {
            FAM f = new FAM();
            f.FamID = "test";
            f.Divorced = "12 MAR 1992";
            f.Married = "12 MAR 2022";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayFam(ref f);

            Assert.Equal(true, actual);

        }

        //case6
        [Fact]
        public void testUserAfterTodayDivorce()
        {
            FAM f = new FAM();
            f.FamID = "test";
            f.Divorced = "12 MAR 2022";
            f.Married = "12 MAR 1995";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDateAfterTodayFam(ref f);

            Assert.Equal(true, actual);

        }

    }
}
