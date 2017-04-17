using System;
using System.Linq;
using Xunit;

using Gedcomreader_Project003.GedcomCls;
using Gedcomreader_Project003.Utilities;
using System.Collections.Generic;
using System.Collections;
using Datelib;

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

            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f);


            Assert.Equal(false, actual);


            Assert.False(Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f));

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
            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f);

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

            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f);


            Assert.Equal(true, actual);

            Assert.True(Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f));

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
            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f);
            Assert.NotEqual(false, actual);
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
            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDivorce(f);
            Assert.NotEqual(false, actual);

        }

    }

    public class UnitTest2
    {
        [Fact]
        public void TestMethod1()
        {
            //Console.WriteLine("test");
            FAM f = new FAM();

            //case1
            f.childeren = "child1 child2";

            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "13 JAN 2014";
            f.Wifeid = "ID2";

            //Assert.Equal(true, actual);

            INDI i = new INDI();

            i.BirthDay = "12 JAN 1960";
            i.age = 12;
            i.Alive = false;
            i.death = "13 JAN 2014";
            i.ID = "ID1";


            Assert.NotNull(f);

            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDeath(i, f);


            Assert.Equal(false, actual);


            Assert.False(Gedcomreader_Project003.Program.IsValidDateforMarriageBeforeDeath(i, f));

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
            f.Death = "13 JAN 2014";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "13 JAN 2014";

            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageDeath(f);

            Assert.Equal(false, actual);

        }

        [Fact]
        public void TestMethod3()
        {
            //case3
            FAM f = new FAM();

            f.childeren = "child1 child2";
            f.Death = "13 JAN 2014";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "12 JAN 2014";



            Assert.NotNull(f);

            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageDeath(f);


            Assert.Equal(true, actual);

            Assert.True(Gedcomreader_Project003.Program.IsValidDateforMarriageDeath(f));

            Assert.IsType<FAM>(f);

            f = null;

            Assert.Null(f);




        }

        //case4
        [Fact]
        public void TestMethod4()
        {
            FAM f = new FAM();

            /*
            f.childeren = "child1 child2";
            f.Divorced = "12 MAR 1995";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = null;
            */

            f.childeren = "child1 child2";
            f.Death = "13 JAN 2014";
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = null;





            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageDeath(f);
            Assert.Equal(false, actual);
        }

        //case5
        [Fact]
        public void TestMethod5()
        {
            FAM f = new FAM();
            /*
            f.childeren = "child1 child2";
            f.Divorced = null;
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "12 MAR 1995";
            */

            f.childeren = "child1 child2";
            f.Death = null;
            f.FamID = "Family1";
            f.HusbandID = "ID1";
            f.Husbandname = "test";
            f.Married = "13 JAN 2014";



            bool actual = Gedcomreader_Project003.Program.IsValidDateforMarriageDeath(f);
            Assert.Equal(false, actual);

        }

    }

    public class UnitTestUsersStory_12_15
    {

        [Fact]
        public void compageAgeTest()
        {
            bool actual = Gedcomreader_Project003.Program.CompareAge(18, 100, 80);
            Assert.Equal(true, actual);
        }

        [Fact]
        public void IsMultipleBirthTest()
        {
            List<int> birthday = new List<int>();
            birthday = Enumerable.Repeat(5, 7).ToList();
            bool actual = Gedcomreader_Project003.Program.IsMultipleBirth(birthday);
            Assert.Equal(true, actual);
        }

    }


    public class UnitTestUsersStory_18_19
    {

        [Fact]

        public void IsSiblingsMarryoneanotherTest()
        {
            FAM f1 = new FAM();
            f1.childeren = "ID1,ID2";
            f1.Divorced = "12 MAR 1995";
            f1.FamID = "Family1";
            f1.HusbandID = "I50";
            f1.Husbandname = "test123";
            f1.Married = "12 JAN 2013";
            f1.Wifeid = "ID";

            FAM f2 = new FAM();
            f2.childeren = "child1,child2";
            f2.Death = null;
            f2.FamID = "Family1";
            f2.HusbandID = "ID1";
            f2.Husbandname = "test";
            f2.Married = "13 JAN 2014";
            f2.Wifeid = "ID2";

            FAM f3 = new FAM();
            f3.childeren = null;
            f3.Death = null;
            f3.FamID = "family1";
            f3.HusbandID = "ID1";
            f3.Wifeid = "ID2";
            f3.Married = "25 jun 2016";




            List<FAM> Family = new List<FAM>();
            Family.Add(f1);
            Family.Add(f2);


            bool actual = Gedcomreader_Project003.Program.IsSiblingsMarryoneanother(f3, Family);

            Assert.Equal(true, actual);

        }



        [Fact]
        public void IsFirstCousinMarriedOneAnotherTest()
        {
            string[] arra = { "ID22", "test2" };
            string[] arra1 = { "test3", "test4" };
            string[] arra2 = { "test5", "test6" };


            List<INDI> lst = new List<INDI>();
            lst.Add(new INDI { age = 25, Alive = true, BirthDay = "22 JAN 1984", Child = "ID1", death = null, ID = "ID21", Name = "TEST123", Dead = false, Sex = "M", spouse = "ID22" });

            List<KeyValuePair<string, string[]>> kvpList = new List<KeyValuePair<string, string[]>>()
                {
                    new KeyValuePair<string, string[]>("Key1", arra),
                    new KeyValuePair<string, string[]>("Key2", arra1),
                    new KeyValuePair<string, string[]>("Key3", arra2),
                };


            string wifeid;

            bool actual = Gedcomreader_Project003.Program.IsFirstCousinMarriedOneAnother(lst, "ID21", kvpList.FirstOrDefault(), out wifeid);

            Assert.Equal(true, actual);


        }



        public class UnitTestUserStrory_27_28
        {

            //amit mistry US27:Include individual ages

            [Fact]

            public void calcuateDayMonthsYearsTest()
            {

                INDI indiObj =

                    new INDI
                    {
                        age = 33,
                        Alive = true,
                        BirthDay = "22 JAN 1984",
                        Child = "ID1",
                        death = null,
                        ID = "ID21",
                        Name = "TEST123",
                        Dead = false,
                        Sex = "M",
                        spouse = "ID22"
                    };

                double days, months, years;

                CustomDates.calcuateDayMonthsYears(Convert.ToDateTime(indiObj.BirthDay), (string.IsNullOrEmpty(indiObj.death) ? System.DateTime.Now : Convert.ToDateTime(indiObj.death)), out days, out months, out years);

                Assert.Equal(indiObj.age, years);

                indiObj =

                   new INDI
                   {
                       age = 16,
                       Alive = false,
                       BirthDay = "22 JAN 1984",
                       Child = "ID1",
                       death = "22 JAN 2000",
                       ID = "ID21",
                       Name = "TEST123",
                       Dead = true,
                       Sex = "M",
                       spouse = "ID22"
                   };

                CustomDates.calcuateDayMonthsYears(Convert.ToDateTime(indiObj.BirthDay), (string.IsNullOrEmpty(indiObj.death) ? System.DateTime.Now : Convert.ToDateTime(indiObj.death)), out days, out months, out years);

                Assert.Equal(indiObj.age, years);

            }


            //US28 Order siblings by age List siblings in families by decreasing age, i.e.oldest siblings first

            [Fact]

            public void OrderedSiblingsTest()
            {
                FAM f = new FAM();
                f.childeren = "ID1,ID2,ID3,ID4";
                f.Divorced = null;
                f.FamID = "Family1";
                f.HusbandID = "ID11";
                f.Husbandname = "test";
                f.Married = "12 MAR 1995";

                List<INDI> lst = new List<INDI>();
                lst.Add(new INDI
                {
                    age = 27,
                    Alive = true,
                    BirthDay = "22 JAN 1990",
                    Child = "ID12",
                    death = null,
                    ID = "ID1",
                    Name = "Jone Doe",
                    Dead = false,
                    Sex = "M",
                    spouse = "ID21"
                }
                );

                lst.Add(new INDI
                {
                    age = 35,
                    Alive = true,
                    BirthDay = "22 JAN 1982",
                    Child = "ID112",
                    death = "22 JAN 2017",
                    ID = "ID2",
                    Name = "Jone Doe2",
                    Dead = false,
                    Sex = "M",
                    spouse = "ID22"
                });


                lst.Add(new INDI
                {
                    age = 32,
                    Alive = true,
                    BirthDay = "22 JAN 1984",
                    Child = "ID113",
                    death = "12 JAN 2016",
                    ID = "ID3",
                    Name = "Jone Doe3",
                    Dead = true,
                    Sex = "M",
                    spouse = "ID23"
                });


                lst.Add(new INDI
                {
                    age = 20,
                    Alive = false,
                    BirthDay = "22 JAN 1997",
                    Child = null,
                    death = null,
                    ID = "ID4",
                    Name = "Jone Doe4",
                    Dead = false,
                    Sex = "M",
                    spouse = "ID24"
                });



                string str = Gedcomreader_Project003.Program.OrderedSiblings(f, lst);

                Assert.NotSame(f.childeren, str);

            }


        }








    }







}


