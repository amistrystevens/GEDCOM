using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;

namespace GedComUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            //Console.WriteLine("test");
            FAM f = new FAM();

           //case1
            f.childeren = "child1 child2";
            f.Divorced = "12 MAR 1995";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "13 MAR 1995";


            //Assert.Equal(true, actual);

            
            //
            Assert.NotNull(f);

            bool actual = Gedcomreader_Project003.Program.IsValidDate(f);


            Assert.Equal(false, actual);


            Assert.False(Gedcomreader_Project003.Program.IsValidDate(f));

            Assert.IsType<FAM>(f);

            f = null;

            Assert.Null(f);





        }

        [Fact]
        public void TestMethod2()
        {
            FAM f = new FAM();
            //case2
            f.childeren = "child1 child2";
            f.Divorced = "12 MAR 1995";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "12 MAR 1995";
            bool actual = Gedcomreader_Project003.Program.IsValidDate(f);

            Assert.Equal(false, actual);

        }

        [Fact]
        public void TestMethod3()
        {
            //case3
            FAM f = new FAM();
            f.childeren = "child1 child2";
            f.Divorced = "12 MAR 1995";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "11 MAR 1995";


            Assert.NotNull(f);

            bool actual = Gedcomreader_Project003.Program.IsValidDate(f);


            Assert.Equal(true, actual);

            Assert.True(Gedcomreader_Project003.Program.IsValidDate(f));

            Assert.IsType<FAM>(f);

            f = null;
            
            Assert.Null(f);

            


        }
        
        //case4
        [Fact]
        public void TestMethod4()
        {
            FAM f = new FAM();
            f.childeren = "child1 child2";
            f.Divorced = "12 MAR 1995";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = null;
            bool actual = Gedcomreader_Project003.Program.IsValidDate(f);
            Assert.Equal(false, actual);
        }

        //case5
        [Fact]
        public void TestMethod5()
        {
            FAM f = new FAM();
            f.childeren = "child1 child2";
            f.Divorced = null;
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "12 MAR 1995";
            bool actual = Gedcomreader_Project003.Program.IsValidDate(f);
            Assert.Equal(false, actual);
            
        }        

    }
}
