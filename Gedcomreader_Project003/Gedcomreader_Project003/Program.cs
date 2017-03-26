using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gedcomreader_Project003.GedcomCls;
using System.Collections;
using System.Text.RegularExpressions;
using Datelib;

namespace Gedcomreader_Project003
{
    
    public class Program
    {
        static int tableWidth = 100;

        static void Main(string[] args)
        {

            List<INDI> Individuals = new List<INDI>();
            List<FAM> Family = new List<FAM>();

            List<FAM> customfam = new List<FAM>();


            // string path = "C:\\Users\\kumara\\Downloads\\TestGED\\TGC55C.ged";

            double days, months, years;

            // string path = "C:\\test\\sample_family.ged";

            string path = "C:\\Users\\Amit\\Desktop\\GED\\sample_family.ged";

            string outputpath = "C:\\Users\\Amit\\Desktop\\GED\\familytree.txt";

          //  File.Create(outputpath);

            StreamWriter fileout   = new StreamWriter(outputpath,true);



            // string path = "C:\\Users\\Class2017\\Documents\\A Stuff\\SSW 555\Project06\\sample_family.ged";

            string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };


            string[] columnsfamily = { "FAMILYID", "MARRID", "DIVORCED", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };


            //Open path to GED file
            StreamReader SR = new StreamReader(path);

            //Read entire block and then plit on 0 @ for individuals and familys (no other info is needed for this instance)
            string[] Holder = SR.ReadToEnd().Replace("0 @", "\u0646").Split('\u0646');

            //For each new cell in the holder array look for Individuals and familys
            foreach (string Node in Holder)
            {

                //Sub Split the string on the returns to get a true block of info
                //  string[] SubNode = Node.Replace("\r\n", "\r").Split('\r');

                string[] SubNode = Node.Split('\n');


                for (int i = 0; i < SubNode.Length; i++)
                //  int i = 0;
                //   while(i<SubNode.Length)
                {

                    bool contains = Regex.IsMatch(SubNode[i], @"(^|\s)INDI(\s|$)"); // yields true

                    if (Regex.IsMatch(SubNode[i], @"(^|\s)INDI(\s|$)"))
                    {

                        INDI indiObj = new INDI();
                        // SubNode[FindIndexinArray(SubNode, "1 NAME ") + 1].Replace("1 NAME", "").Trim();

                        //string strsub=  SubNode[i].Substring(0, SubNode[i].Length - 4).Trim();
                        string[] moresub = SubNode[i].Split(' ');

                        indiObj.ID = moresub[1].Trim();  //SubNode[i].Replace("1 NAME", "").Replace("/", "").Trim();

                        indiObj.Name = SubNode[++i].Replace("1 NAME", "").Replace("/", "").Trim();


                        if (SubNode[++i].Contains("BIRT"))
                        {
                            // add birthday to Struct 
                            indiObj.BirthDay = SubNode[++i].Replace("2 DATE ", "").Trim();


                        }

                        if (SubNode[++i].Contains("SEX"))
                        {

                            indiObj.Sex = SubNode[i++].Replace("1 SEX", "").Trim();

                        }

                        if (SubNode[i].Contains("FAMS"))
                        {
                            //var FAMC = SubNode[i++].Replace("1 FAMS", "").Trim();
                            indiObj.Child = SubNode[i++].Replace("1 FAMS", "").Trim();
                        }

                        if (SubNode[i++].Contains("1 DEAT"))
                        {
                            //convert Y or N to true or false ( defaults to False so no need to change unless Y is found.
                            // var deathdt = SubNode[i].Replace("2 DEAT ", "").Trim();
                            indiObj.death = SubNode[i].Replace("2 DATE ", "").Trim();
                            indiObj.Dead = false;

                            CustomDates.calcuateDayMonthsYears(Convert.ToDateTime(indiObj.BirthDay), (string.IsNullOrEmpty(indiObj.death) ? System.DateTime.Now : Convert.ToDateTime(indiObj.death)), out days, out months, out years);

                            indiObj.age = Convert.ToInt16(years);
                            //set death
                            //var Dead = true;

                        }else
                        {
                            i=i-2;
                            CustomDates.calcuateDayMonthsYears(Convert.ToDateTime(indiObj.BirthDay), (string.IsNullOrEmpty(indiObj.death) ? System.DateTime.Now : Convert.ToDateTime(indiObj.death)), out days, out months, out years);

                            indiObj.age = Convert.ToInt16(years);


                        }


                        Individuals.Add(indiObj);

                        indiObj = null;
                    }



                    /*
                       0 F23 FAM
                       1 MARR
                       2 DATE 14 FEB 1980
                       1 HUSB I01
                       1 WIFE I07
                       1 CHIL I19
                       1 CHIL I26

                       famObj.Divorced=
                       famObj.HusbandID=
                       famObj.Wifeid=
                       famObj.childeren=

                       */

                    //1 FAMS F23

                    //0 F23 FAM



                    if ((Regex.IsMatch(SubNode[i], @"(^|\s)FAM(\s|$)")))
                    {
                        FAM famObj = new FAM();

                        string[] morefam = SubNode[i].Split(' ');

                        famObj.FamID = morefam[1].Trim();

                        if (SubNode[++i].Contains("MARR"))
                        {
                            // add birthday to Struct 
                            famObj.Married = SubNode[++i].Replace("2 DATE ", "").Trim();

                        }

                        if (SubNode[++i].Contains("DIV"))
                        {
                            // add birthday to Struct 
                            famObj.Divorced = SubNode[++i].Replace("2 DATE ", "").Trim();

                        }


                        if (SubNode[++i].Contains("HUSB"))
                        {
                            famObj.HusbandID = SubNode[i].Replace("1 HUSB", "").Trim();
                        }


                        if (SubNode[++i].Contains("WIFE"))
                        {
                            famObj.Wifeid = SubNode[i].Replace("1 WIFE", "").Trim();
                        }
                        i++;
                        string str = string.Empty;
                        while (i!= SubNode.Length)
                        {
                            if (SubNode[i].Contains("CHIL") )
                            {
                                str = str + "," + SubNode[i].Replace("1 CHIL", "").Trim();
                                i++;
                            }
                            else
                            {
                                break;
                            }

                        }

                        if (str != string.Empty)
                        {
                            famObj.childeren = str;
                        }

                        Family.Add(famObj);

                        famObj = null;

                        i = i - 2;
                    }

                }

            }


            PrintLine(fileout);
            PrintRow(fileout,columns);
            PrintLine(fileout);
            

            printIndividual(Individuals,fileout);

            Console.WriteLine("\n");
            fileout.WriteLine("\n");

            PrintLine(fileout);
            PrintRow(fileout,columnsfamily);
            PrintLine(fileout);

            printFamily(Family, fileout);

            Console.WriteLine("\t\t List of Errors");
            fileout.WriteLine("\t\t List of Errors");
            Console.WriteLine("\t\t\t\t\tsprint1 : Users stories 3,5  ");
            fileout.WriteLine("\t\t\t\t\tsprint1 : Users stories 3,5  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n ");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n ");

            foreach (FAM fam in Family)
            {
                var fil = Individuals.Where(i => i.ID == fam.HusbandID || i.ID == fam.Wifeid).ToList();

                foreach (INDI ind in fil)
                {
                    if (!IsValidDateforMarriageBeforeDeath(ind, fam))
                    {
                        Console.WriteLine("ERROR: INDIVIDUAL : US3 : " + ind.ID + " : " + " Married " + fam.Married + " after death on " + ind.death);
                        fileout.WriteLine("ERROR: INDIVIDUAL : US3 : " + ind.ID + " : " + " Married " + fam.Married + " after death on " + ind.death);
                    }
                }

            }

            
            foreach (FAM fam in Family)
            {
                if (!IsValidDateforMarriageBeforeDivorce(fam))
                {
                    Console.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Divorced on" + fam.Divorced + " before married " + fam.Married);
                    fileout.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Divorced on" + fam.Divorced + " before married " + fam.Married);
                }
            }

            //------------- US 1 and US 29

            Console.WriteLine("\t\t List of Errors");
            fileout.WriteLine("\t\t List of Errors");
            Console.WriteLine("\t\t\t\t\tsprint1 : Users stories 1,29  ");
            fileout.WriteLine("\t\t\t\t\tsprint1 : Users stories 1,29  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n ");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n ");
            //datesBeforeToday(Individuals, Family);

            DateTime day = new DateTime(1996, 3, 24);

            //deadBeforeDay(Individuals, day);

            //------------- US 12 and US 14
            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\tSprint2 : Users stories 12,14  ");
            fileout.WriteLine("\t\t\t\t\tSprint2 : Users stories 12,14  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");

            int sonAge;
            int fatherAge;
            int motherAge;
            List<int> birthday = new List<int>();
            foreach (FAM fam in Family)
            {
                if (fam.childeren != null)
                {
                    var childeren = fam.childeren;
                    string mychildern = childeren.Trim(new Char[] { ' ', ',' });
                    string[] child = mychildern.Split(',');

                    fatherAge = Individuals.Where(ind => ind.ID == fam.HusbandID).SingleOrDefault().age;
                    motherAge = Individuals.Where(ind => ind.ID == fam.Wifeid).SingleOrDefault().age;

                    for (int i = 0; i < child.Length; i++)
                    {
                        sonAge = Individuals.Where(ind => ind.ID == child[i]).SingleOrDefault().age;

                        if (CompareAge(sonAge, fatherAge, motherAge))
                        {
                            Console.WriteLine("ERROR: FAMILY : US12 : " + fam.FamID + " father(" + fam.HusbandID + ") age (" + fatherAge + ") or mother(" + fam.Wifeid + ") age (" + motherAge + ") is  older than child (" + child[i] + ")");
                            fileout.WriteLine("ERROR: FAMILY : US12 : " + fam.FamID + " father(" + fam.HusbandID + ") age (" + fatherAge + ") or mother(" + fam.Wifeid + ") age (" + motherAge + ") is  older than child (" + child[i] + ")");
                            break;
                        }

                    }


                    for (int i = 0; i < child.Length; i++)
                    {
                        birthday.Add(Convert.ToDateTime(Individuals.Where(ind => ind.ID == child[i]).SingleOrDefault().BirthDay).Day);
                    }


                    if (IsMultipleBirth(birthday))
                    {
                        Console.WriteLine("ERROR: FAMILY : US14 : Family " + fam.FamID + " has " + birthday.Count + " siblings ");
                        fileout.WriteLine("ERROR: FAMILY : US14 : Family " + fam.FamID + " has " + birthday.Count + " siblings ");
                    }
                }

            }

            //------------- US 06 and US 07
            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\tSprint2 : Users stories 12,15  ");
            fileout.WriteLine("\t\t\t\t\tSprint2 : Users stories 12,15  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");

            foreach (FAM fam in Family)
            {
                if (!IsValidDateforMarriageBeforeDivorce(fam))
                {
                    Console.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Died on" + fam.Death + " before Divorced " + fam.Divorced);
                    fileout.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Died on" + fam.Death + " before Divorced " + fam.Divorced);
                }
            }

            foreach (INDI indi in Individuals)
            {
                if(isTooOld(indi))
                {
                    Console.WriteLine("ERROR: INDIVIDUAL : US07 : Family " + indi.ID + " has Birth" + indi.BirthDay + " and Death " + indi.death);
                    fileout.WriteLine("ERROR: INDIVIDUAL : US07 : Family " + indi.ID + " has Birth" + indi.BirthDay + " and Death " + indi.death);
                }
            }



            /////User Stroies 18,19

            
            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\tSprint3 : Users stories 18,19  ");
            fileout.WriteLine("\t\t\t\t\tSprint3 : Users stories 18,19  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");


           // Family.ForEach( i=>i.HusbandID  Family.ForEach( i=>i.HusbandID)

            foreach (FAM fam in Family)
            {
                if(fam.HusbandID!=null && fam.Wifeid!=null)
                {
                    foreach(var innerfam in Family)
                    {
                        if(innerfam.childeren !=null)
                        {
                            var childeren = innerfam.childeren;
                            string mychildern = childeren.Trim(new Char[] { ' ', ',' });
                            string[] child = mychildern.Split(',');

                            if(child.Contains(fam.HusbandID) && child.Contains(fam.Wifeid))
                            {
                                Console.WriteLine("ERROR: FAMILY : US18 : " + fam.FamID + ": " + fam.HusbandID + " cannot marry to siblig " + fam.Wifeid);
                                fileout.WriteLine("ERROR: FAMILY : US18 : " + fam.FamID + ": " + fam.HusbandID + " cannot marry to siblings " + fam.Wifeid);
                            }
                        }
                    }
                } 
            }




                // Keep the console window open in debug mode.
                Console.WriteLine("\n\n\n\nPress any key to exit.");
            System.Console.ReadKey();


        }


        //US12: The description in column C explains that US12, parents too old, 
        //occurs when the father’s age – child’s age >= 80 or mother’s age – child’s age >= 60 


        public static bool CompareAge(int sage,int fage,int mage)
        {
            return (fage - sage >= 80 || mage - sage >= 60) ? true : false; 
        }

        /*
        US14: US14 occurs when the number of children born on a single day > 6. 
        You can ignore the boundary condition where siblings are born across two consecutive days, 
        e.g.sibling 1 born at 11:59PM and sibling 2 born at 12:01AM the next day.Just count the number of children born each birthday within 
        a family and generate a warning if > 5 
        */

        public static bool IsMultipleBirth( List<int> birthday)
        {
            bool bmultibirth=false;

            Dictionary<int, int> counts = birthday.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            if (counts.Count > 0)
            {

                foreach (var c in counts)
                {
                    if (c.Value > 6)
                    {
                        bmultibirth = true;                     
                        break;
                    }
                }
            }

            return bmultibirth;
        }


        //US18 Siblings should not marry : Siblings should not marry one another

        public static bool IsSiblingsMarryoneanother()
        {

            return false;
        }


        //US19    First cousins should not marry : First cousins should not marry one another






        public static bool IsValidDateforMarriageBeforeDeath(INDI ind, FAM fil)
        {
            if (ind.death != null && fil.Married != null)
            {
                DateTime mgdt = Convert.ToDateTime(fil.Married);
                DateTime deathdt = Convert.ToDateTime(ind.death);
                if (mgdt > deathdt)
                {
                    return false;
                }
                else if (mgdt < deathdt)
                {
                    return true;
                }
                else if (mgdt == deathdt)
                {
                    return false;
                }

            }
            return true;

        }

        public static bool IsValidDateforMarriageBeforeDivorce(FAM famObj)
        {
            if (famObj.Divorced != null && famObj.Married != null)
            {
                DateTime mgdt = Convert.ToDateTime(famObj.Married);
                DateTime divdt = Convert.ToDateTime(famObj.Divorced);
                if (mgdt > divdt)
                {
                    return false;
                }
                else if (mgdt < divdt)
                {
                    return true;
                }
                else if (mgdt == divdt)
                {
                    return false;
                }

            }
            return true;
        }


        /// <summary>
        /// US05	Marriage before death	am
        /// </summary>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>
        /*
         marriage date > death date 

        - marriage date == death date 

        - marriage date<death date

        - marriage date but no death date 

        - no marriage date with death date
        */


        public static bool IsValidDateforMarriageDeath(FAM famObj)
        {
            if (famObj.Death != null && famObj.Married != null)
            {
                DateTime mgdt = Convert.ToDateTime(famObj.Married);
                DateTime deathdt = Convert.ToDateTime(famObj.Death);
                if (mgdt > deathdt)
                {
                    return false;
                }
                else if (mgdt < deathdt)
                {
                    return true;
                }
                else if (mgdt == deathdt)
                {
                    return false;
                }

            }
            return false;
        }


        /// <summary>
        /// US04	Marriage before divorce	
        /// </summary>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>
        /*
         marriage date > divorce data 

        - marriage date == divorce date 

        - marriage date<divorce date

        - marriage date but no divorce date 

        - no marriage date with divorce date
        */


        public static bool IsValidDate(FAM famObj)
        {
            if (famObj.Divorced != null && famObj.Married != null)
            {
                DateTime mgdt = Convert.ToDateTime(famObj.Married);
                DateTime divdt = Convert.ToDateTime(famObj.Divorced);
                if (mgdt > divdt)
                {
                    return false;
                }
                else if (mgdt < divdt)
                {
                    return true;
                }
                else if (mgdt == divdt)
                {
                    return false;
                }

            }
            return false;
        }


        /// <summary>
        /// US29 List Deceased
        /// List Displays all deceased members before the current date
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static void deadBeforeDay(List<INDI> Individuals, DateTime when,StreamWriter fileout)
        {
            string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };

            List<INDI> deadIndi = new List<INDI>();

            //Loop through and check all the death days on individuals
            foreach (INDI i in Individuals)
            {
                //Skip anyone without a death day
                if (isDeadBeforeDay(i, when) == false)
                {
                    continue;
                }
                else
                {
                    deadIndi.Add(i);
                }
            }

            //Print the list of dead individuals
            PrintLine(fileout);
            Console.WriteLine("DEAD INDIVIDUALS BEFORE GIVEN DATE: " + when.ToString());
            fileout.WriteLine("DEAD INDIVIDUALS BEFORE GIVEN DATE: " + when.ToString());
            PrintLine(fileout);
            PrintRow(fileout, columns);
            PrintLine(fileout);

            printIndividual(deadIndi,fileout);

            Console.WriteLine("\n");
        }

        /// <summary>
        /// US29 List Deceased
        /// Helper Method in Boolean Form
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static bool isDeadBeforeDay(INDI i, DateTime when)
        {

            //Skip anyone without a death day
            if (i.death == null || i.death.Equals(""))
            {
                return false;
            }
            else if (when > Convert.ToDateTime(i.death))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// US01 List Dates before date
        /// List all Individuals and Families who have a date before this date
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static void datesBeforeToday(List<INDI> Individuals, List<FAM> Families,StreamWriter fileout)
        {
            string[] columnsIndviduals = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };
            string[] columnsFamily = { "FAMILYID", "MARRID", "DIVORCED", "DEATH", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };

            List<INDI> IndiWithDates = new List<INDI>();
            List<FAM> FamilyWithDates = new List<FAM>();

            INDI currentIndi = new INDI();
            bool changed = false;
            //Loop through and check all the death days on individuals
            for (int i = 0; i < Individuals.Count(); i++)
            {
                currentIndi = Individuals[i];
                changed = isDateAfterTodayIndi(ref currentIndi);
                if (changed)
                    IndiWithDates.Add(currentIndi);
            }

            //Print the list of changed individuals
            PrintLine(fileout);
            Console.WriteLine("INDIVIDUALS WITH DATES BEFORE CURRENT DATE: " +DateTime.Today.ToString());
            fileout.WriteLine("INDIVIDUALS WITH DATES BEFORE CURRENT DATE: " + DateTime.Today.ToString());
            PrintLine(fileout);
            PrintLine(fileout);
            PrintRow(fileout, columnsIndviduals);
            PrintLine(fileout);

            printIndividual(IndiWithDates,fileout);

            Console.WriteLine("\n");

            //Loop through all Families now
            changed = false;
            FAM currentFam = new FAM();
            for (int i = 0; i < Families.Count(); i++)
            {
                currentFam = Families[i];
                changed = isDateAfterTodayFam(ref currentFam);
                if (changed)
                    FamilyWithDates.Add(currentFam);
            }

            //Print the list of changed families
            PrintLine(fileout);
            Console.WriteLine("FAMILIES WITH DATES BEFORE CURRENT DATE: " + DateTime.Today.ToString());
            PrintLine(fileout);
            PrintLine(fileout);
            PrintRow(fileout,columnsFamily);
            PrintLine(fileout);

            printFamily(FamilyWithDates,fileout);

            Console.WriteLine("\n");
        }

        /// <summary>
        /// US01 List Date before Current Day
        /// Helper Method in Boolean Form
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static bool isDateAfterTodayIndi(ref INDI i)
        {
            bool changed = false;
            DateTime today = DateTime.Today;

            //Skip anyone without a death day
            if (i.death == null || i.death.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(i.death))
            {
                i.death = "";
                changed = true;
                i.Dead = false;
            }
            if (i.BirthDay == null || i.BirthDay.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(i.BirthDay))
            {
                i.BirthDay = "";
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// US01 List Date before Current Day
        /// Helper Method in Boolean Form
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static bool isDateAfterTodayFam(ref FAM f)
        {
            bool changed = false;
            DateTime today = DateTime.Today;

            //Skip anyone without a death day
            if (f.Married == null || f.Married.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(f.Married))
            {
                f.Married = "";
                changed = true;
            }
            if (f.Divorced == null || f.Divorced.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(f.Divorced))
            {
                f.Divorced = "";
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// US06	Divorce before death	
        /// </summary>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>
        public static bool IsValidDateforDivorceBeforeDeath(FAM famObj)
        {
            if (famObj.Divorced != null && famObj.Death != null)
            {
                DateTime divorceddt = Convert.ToDateTime(famObj.Divorced);
                DateTime deathdt = Convert.ToDateTime(famObj.Death);
                if (deathdt <= divorceddt)
                {
                    return false;
                }
                else if (deathdt > divorceddt)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// US07	Less than 150 Years Old
        /// </summary>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>
        public static bool isTooOld(INDI indi)
        {
            //Check first if the user is dead
            if (indi.BirthDay != null && indi.death != null)
            {
                DateTime birthdt = Convert.ToDateTime(indi.BirthDay);
                DateTime deathdt = Convert.ToDateTime(indi.death);
                //Check if there's 150 years between death (later date) and birth (smaller date)
                if (deathdt.AddYears(-150) >= birthdt)
                    //yes they are too old
                    return true;
                else
                    //No they are not
                    return false;
            }
            //Not dead
            else
                return false;
        }

        #region Printer Methods
        private static int FindIndexinArray(string[] Arr, string search)
        {
            int Val = -1;
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i].Contains(search.Trim()))
                {
                    Val = i;
                }
            }
            return Val;
        }

        static private void printIndividual(List<INDI> lst, StreamWriter fileout)
        {
            //double days = 0, months = 0, years = 0;
            foreach (INDI l in lst)
            {
                //CustomDates.calcuateDayMonthsYears( Convert.ToDateTime(l.BirthDay), ( string.IsNullOrEmpty(l.death)?System.DateTime.Now : Convert.ToDateTime(l.death)), out days, out months,out years);
                Console.WriteLine("{0,7}{1,14}{2,7}{3,17}{4,5}{5,10}{6,15}{7,17}{8,8}",
                                   l.ID, l.Name, l.Sex, l.BirthDay, l.age , l.Dead, l.death, l.Child, l.spouse);
                fileout.WriteLine("{0,7}{1,14}{2,7}{3,17}{4,5}{5,10}{6,15}{7,17}{8,8}",
                                   l.ID, l.Name, l.Sex, l.BirthDay, l.age, l.Dead, l.death, l.Child, l.spouse);
                PrintLine(fileout);
            }
        }

        static private void printFamily(List<FAM> lst, StreamWriter fileout)
        {
            foreach (FAM f in lst)
            {
                Console.WriteLine("{0,7}{1,15}{2,15}{3,10}{4,5}{5,10}{6,5}{7,10}{8,15}",
                    f.FamID, f.Married, f.Divorced, f.Death, f.HusbandID, f.Husbandname, f.Wifeid, f.Wifename, f.childeren);
                fileout.WriteLine("{0,7}{1,15}{2,15}{3,10}{4,5}{5,10}{6,5}{7,10}{8,15}",
                    f.FamID, f.Married, f.Divorced, f.Death, f.HusbandID, f.Husbandname, f.Wifeid, f.Wifename, f.childeren);
            }
        }

        static void PrintLine(StreamWriter fileout)
        {
            Console.WriteLine(new string('-', tableWidth));

            fileout.WriteLine(new string('-', tableWidth));

        }

        static void PrintRow(StreamWriter fileout, params string[] columns )
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            
            Console.WriteLine(row);
            fileout.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

    }
    #endregion
}
