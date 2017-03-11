using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gedcomreader_Project003.GedcomCls;
using System.Collections;
using System.Text.RegularExpressions;

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



            // string path = "C:\\test\\sample_family.ged";

            string path = "C:\\Users\\Amit\\Desktop\\test\\sample_family.ged";

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
                            indiObj.Dead = true;

                            //set death
                            //var Dead = true;

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

                        string str = string.Empty;
                        while (SubNode[++i].Contains("CHIL"))
                        {
                            if (SubNode[i].Contains("CHIL"))
                            {
                                str = str + "," + SubNode[i].Replace("1 CHIL", "").Trim();
                            }
                        }

                        if (str != string.Empty)
                        {
                            famObj.childeren = str;
                        }

                        Family.Add(famObj);

                        famObj = null;


                    }

                }

            }


            PrintLine();
            PrintRow(columns);
            PrintLine();

            printIndividual(Individuals);

            Console.WriteLine("\n");

            PrintLine();
            PrintRow(columnsfamily);
            PrintLine();

            printFamily(Family);

            Console.WriteLine("\n\n\n\n List of Errors");


            foreach (FAM fam in Family)
            {
                var fil = Individuals.Where(i => i.ID == fam.HusbandID || i.ID == fam.Wifeid).ToList();

                foreach (INDI ind in fil)
                {
                    if (!IsValidDateforMarriageBeforeDeath(ind, fam))
                    {
                        Console.WriteLine("ERROR: INDIVIDUAL : US3 : " + ind.ID + " : " + " Married " + fam.Married + " after death on " + ind.death);
                    }
                }

            }


            Console.Write("\n");

            foreach (FAM fam in Family)
            {
                if (!IsValidDateforMarriageBeforeDivorce(fam))
                {
                    Console.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Divorced on" + fam.Divorced + " before married " + fam.Married);
                }
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("\n\n\n\n\n\n\nPress any key to exit.");
            System.Console.ReadKey();


        }

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
        public void deadBeforeDay(List<INDI> Individuals, DateTime when)
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
            PrintLine();
            PrintRow(columns);
            PrintLine();

            printIndividual(deadIndi);

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
        public void DatesBeforeToday(List<INDI> Individuals, List<FAM> Families)
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
            PrintLine();
            PrintRow(columnsIndviduals);
            PrintLine();

            printIndividual(IndiWithDates);

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
            PrintLine();
            PrintRow(columnsFamily);
            PrintLine();

            printFamily(FamilyWithDates);

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

        static private void printIndividual(List<INDI> lst)
        {
            foreach (INDI l in lst)
            {
                Console.WriteLine("{0,7}{1,14}{2,7}{3,17}{4,5}{5,10}{6,15}{7,17}{8,8}",
                                   l.ID, l.Name, l.Sex, l.BirthDay, l.age, l.Alive, l.death, l.Child, l.spouse);
                PrintLine();
            }
        }

        static private void printFamily(List<FAM> lst)
        {
            foreach (FAM f in lst)
            {
                Console.WriteLine("{0,7}{1,15}{2,15}{3,10}{4,5}{5,10}{6,5}{7,10}{8,15}",
                    f.FamID, f.Married, f.Divorced, f.Death, f.HusbandID, f.Husbandname, f.Wifeid, f.Wifename, f.childeren);
            }
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
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
