using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;

namespace GedComUnitTest
{
    public class ListDeceasedTest
    {
        [Fact]
        public void testNullData()
        {
            //Console.WriteLine("test");
            INDI i = new INDI();

            //case1
            i.Name = "tester1";
            i.age= 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = false;
            i.death = null;
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);
            //Assert.Equal(true, actual);

            List<INDI> list = new List<INDI>();
            list.Add(i);
            //
            Assert.NotNull(i);

            bool actual = Gedcomreader_Project003.Program.isDeadBeforeDay(i, day);

            Assert.Equal(false, actual);


            Assert.False(Gedcomreader_Project003.Program.isDeadBeforeDay(i, day));

            Assert.IsType<INDI>(i);
        }

        [Fact]
        public void testNotDeadBeforeDay()
        {
            INDI i = new INDI();

            //case2
            i.Name = "tester2";
            i.age = 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = false;
            i.death = "12 MAR 1997";
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);

            //Test if the user is actually dead (he isn't
            bool actual = Gedcomreader_Project003.Program.isDeadBeforeDay(i, day);

            Assert.Equal(false, actual);
        }

        [Fact]
        public void testUserDead()
        {
            INDI i = new INDI();

            //case3
            i.Name = "tester2";
            i.age = 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = true;
            i.death = "13 MAR 1995";
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDeadBeforeDay(i, day);

            Assert.Equal(true, actual);




        }

        //case4
        [Fact]
        public void testUserDeadAfterDate()
        {
            INDI i = new INDI();

            //case4
            i.Name = "tester2";
            i.age = 18;
            i.BirthDay = "12 MAR 1995";
            i.Dead = true;
            i.death = "13 MAR 1997";
            i.Sex = "M";

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDeadBeforeDay(i, day);

            Assert.Equal(false, actual);
        }

        //case5
        [Fact]
        public void testUserEmpty()
        {
            INDI i = new INDI();

            //case4
            i.Name = "" ;
            i.age = -1;
            i.BirthDay = "";
            i.Dead = false;
            i.death = "";
            i.Sex = "" ;

            DateTime day = new DateTime(1996, 3, 24);

            bool actual = Gedcomreader_Project003.Program.isDeadBeforeDay(i, day);

            Assert.Equal(false, actual);

        }

    }
}
