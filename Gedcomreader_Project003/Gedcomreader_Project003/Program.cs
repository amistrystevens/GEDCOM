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

            //string path = "C:\\test\\sample_family.ged";

            //string path ="C:\\Users\\Amit\\Desktop\\myged\\sample_family.ged";

            string path = "c:\\users\\class2017\\ssw555\\gedcom\\gedcomreader_project003\\gedcomreader_project003\\sample_family.ged";

            string outputpath = "c:\\users\\class2017\\ssw555\\gedcom\\gedcomreader_project003\\gedcomreader_project003\\output.txt";


            //string outputpath = "C:\\Users\\Amit\\Desktop\\myged\\output.txt";

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

                            //amit mistry US27:Include individual ages
                            CustomDates.calcuateDayMonthsYears(Convert.ToDateTime(indiObj.BirthDay), (string.IsNullOrEmpty(indiObj.death) ? System.DateTime.Now : Convert.ToDateTime(indiObj.death)), out days, out months, out years);

                            indiObj.age = Convert.ToInt16(years);
                            //set death
                            //var Dead = true;

                        }else
                        {
                            i=i-2;
                            //amit mistry US27:Include individual ages
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


            foreach (var mem in Family)
            {
                foreach (var ind in Individuals)
                {
                    if (mem.HusbandID == ind.ID)
                    {
                        mem.Husbandname = ind.Name;
                        ind.spouse = mem.Wifeid;
                    }

                    if (mem.Wifeid == ind.ID)
                    {
                        mem.Wifename = ind.Name;
                        ind.spouse = mem.HusbandID;
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

            Console.WriteLine("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\t Amitkumar Mistry Users stories 4,5  ");
            Console.WriteLine("\t\t\t\t\tSprint1 : Users stories 4,5  ");
            fileout.WriteLine("\t\t\t\t\tSprint1 : Users stories 4,5  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n ");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n ");

            foreach (FAM fam in Family)
            {
                var fil = Individuals.Where(i => i.ID == fam.HusbandID || i.ID == fam.Wifeid).ToList();

                foreach (INDI ind in fil)
                {
                    if (!IsValidDateforMarriageBeforeDeath(ind, fam))
                    {
                        Console.WriteLine("ERROR: INDIVIDUAL : US4 : " + ind.ID + " : " + " Married " + fam.Married + " after death on " + ind.death);
                        fileout.WriteLine("ERROR: INDIVIDUAL : US4 : " + ind.ID + " : " + " Married " + fam.Married + " after death on " + ind.death);
                    }
                }

            }

            
            foreach (FAM fam in Family)
            {
                if (!IsValidDateforMarriageBeforeDivorce(fam))
                {
                    Console.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Divorced on: " + fam.Divorced + " before married " + fam.Married);
                    fileout.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Divorced on: " + fam.Divorced + " before married " + fam.Married);
                }
            }
            Console.WriteLine("\n");
            fileout.WriteLine("\n");
            //------------- US 1 and US 29

            Console.WriteLine("\t\t\t\t\tSprint1 : Users stories 1,29  ");
            fileout.WriteLine("\t\t\t\t\tSprint1 : Users stories 1,29  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n ");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n ");

            datesBeforeToday(Individuals, Family, fileout);
            DateTime day = new DateTime(1996, 3, 24);

            deadBeforeDay(Individuals, day, fileout);


            //deadBeforeDay(Individuals, day);

            //------------- US 12 and US 14
            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\t Amitkumar Mistry Users stories 12,14  ");
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
                        sonAge = Individuals.Where(ind => ind.ID == child[i]).FirstOrDefault().age;
                        
                        if (CompareAge(sonAge, fatherAge, motherAge))
                        {
                            Console.WriteLine("ERROR: FAMILY : US12 : " + fam.FamID + " father(" + fam.HusbandID + ") age (" + fatherAge + ") or mother(" + fam.Wifeid + ") age (" + motherAge + ") is  older than child (" + child[i] + ")");
                            fileout.WriteLine("ERROR: FAMILY : US12 : " + fam.FamID + " father(" + fam.HusbandID + ") age (" + fatherAge + ") or mother(" + fam.Wifeid + ") age (" + motherAge + ") is  older than child (" + child[i] + ")");
                            break;
                        }

                    }


                    for (int i = 0; i < child.Length; i++)
                    {
                        birthday.Add(Convert.ToDateTime(Individuals.Where(ind => ind.ID == child[i]).FirstOrDefault().BirthDay).Day);
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
            Console.WriteLine("\t\t\t\t\tSprint2 : Users stories 06,07  ");
            fileout.WriteLine("\t\t\t\t\tSprint2 : Users stories 06,07  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");

            foreach (FAM fam in Family)
            {
                if (!IsValidDateforDivorceBeforeDeath(fam))
                {
                    Console.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Died on " + fam.Death + " before Divorced " + fam.Divorced);
                    fileout.WriteLine("ERROR: FAMILY : US5 : " + fam.FamID + ": Died on " + fam.Death + " before Divorced " + fam.Divorced);
                }
            }

            foreach (INDI indi in Individuals)
            {
                if(isTooOld(indi))
                {
                    Console.WriteLine("ERROR: INDIVIDUAL : US07 : Family " + indi.ID + " has Birth " + indi.BirthDay + " and Death " + indi.death);
                    fileout.WriteLine("ERROR: INDIVIDUAL : US07 : Family " + indi.ID + " has Birth " + indi.BirthDay + " and Death " + indi.death);
                }
            }




            /////User Stroies 18,19           

            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\t Amitkumar Mistry Users stories 18,19  ");
            Console.WriteLine("\t\t\t\t\tSprint3 : Users stories 18,19  ");
            fileout.WriteLine("\t\t\t\t\tSprint3 : Users stories 18,19  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");


            foreach (FAM fam in Family)
            {
                if (fam.HusbandID != null && fam.Wifeid != null)
                {
                    if (IsSiblingsMarryoneanother(fam, Family))
                    {
                        Console.WriteLine("ERROR: FAMILY : US18 : " + fam.FamID + ": " + fam.HusbandID + " cannot marry to siblig " + fam.Wifeid);
                        fileout.WriteLine("ERROR: FAMILY : US18 : " + fam.FamID + ": " + fam.HusbandID + " cannot marry to siblings " + fam.Wifeid);
                    }
                }

            }



            //string[] child1;
            List<KeyValuePair<string, string[]>> grp = new List<KeyValuePair<string, string[]>>();
            foreach (FAM fam in Family)
            {

                if (fam.HusbandID != null && fam.Wifeid != null)
                {

                    if (fam.childeren != null)
                    {
                        var childeren = fam.childeren;
                        string mychildern = childeren.Trim(new Char[] { ' ', ',' });
                        string[] child1 = mychildern.Split(',');
                        grp.Add(new KeyValuePair<string, string[]>(fam.FamID, child1));
                    }
                }
            }

            string wifeid;
            foreach (var v in grp)
            {
                foreach (var j in v.Value)
                {

                    if (IsFirstCousinMarriedOneAnother(Individuals, j, v, out wifeid))
                    {
                        Console.WriteLine("ERROR: FAMILY : US19 : " + v.Key + ": " + j + " First cousins should not marry one another " + wifeid);
                        fileout.WriteLine("ERROR: FAMILY : US19 : " + v.Key + ": " + j + " First cousins should not marry one another  " + wifeid);
                    }

                }
            }


            /////User Stroies 22,23


            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\tSprint3 : Users stories 22,23  ");
            fileout.WriteLine("\t\t\t\t\tSprint3 : Users stories 22,23  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");

            List<string> resultsFam = new List<string>();
            if (!isIDUniqueFam(Family, ref resultsFam))
            {
                foreach (string s in resultsFam)
                {
                    Console.WriteLine("ERROR: FAMILY : US22 : " + s + " Family ID's Match");
                    fileout.WriteLine("ERROR: FAMILY : US22 : " + s + " Family ID's Match");
                }
            }
            List<string> resultsIndi = new List<string>();
            if (!isIDUniqueIndi(Individuals, ref resultsIndi))
            {
                for (int i = 0; i < resultsIndi.Count(); i += 3)
                {
                    Console.WriteLine("ERROR: INDIVIDUAL : US22 : " + "ID: " + resultsIndi[i] + " Name: " + resultsIndi[i + 1] + " Name: " + resultsIndi[i + 2] + " " + " Individual ID's Match");
                    fileout.WriteLine("ERROR: INDIVIDUAL : US22 : " + resultsIndi[i] + "Name: " + resultsIndi[i + 1] + "Name: " + resultsIndi[i + 2] + " " + " Individual ID's Match");
                }
            }

            List<string> resultsNames = new List<string>();
            List<string> resultsBirthday = new List<string>();
            if (!isNameBirthdayUnique(Individuals, ref resultsNames, ref resultsBirthday))
            {
                int counter = 0;
                foreach (string s in resultsNames)
                {
                    Console.WriteLine("ERROR: INDIVIDUAL : US23 : " + s + " " + resultsBirthday[counter] + " Individual Names and Birthdays Match");
                    fileout.WriteLine("ERROR: INDIVIDUAL : US23 : " + s + " " + resultsBirthday[counter] + " Individual Names and Birthdays Match");
                    counter++;
                }
            }

            Console.Write("\n");
            fileout.WriteLine("\n");
            Console.WriteLine("\t\t\t\t\tSprint4 : Users stories 24,25  ");
            fileout.WriteLine("\t\t\t\t\tSprint4 : Users stories 24,25  ");
            Console.WriteLine("\t\t\t\t\t------------------------------\n");
            fileout.WriteLine("\t\t\t\t\t------------------------------\n");

            List<FAM> resultsFamily = new List<FAM>();
            List<INDI> matches = new List<INDI>();

            if(!isUniqueFamily(Family, ref resultsFamily))
            {
                foreach (FAM f in resultsFamily)
                {
                    Console.WriteLine("ERROR: FAMILY : US24 : Husband: " + f.Husbandname + ", Wife: " + f.Wifename +  " Marriage date: " + f.Married + ", Duplicate Entry Exists");
                    fileout.WriteLine("ERROR: FAMILY : US24 : Husband: " + f.Husbandname + ", Wife: " + f.Wifename + " Marriage date: " + f.Married + ", Duplicate Entry Exists");
                }
            }
             
            foreach (FAM f in Family)
            {
                if(!isUniqueFamilyNames(f, Individuals, ref matches))
                {
                    for (int i = 0; i < matches.Count; i ++)
                    {
                        Console.WriteLine("ERROR: FAMILY : US25 : Name: " + matches[i].Name + ", Birthday: " + matches[i].BirthDay + ", Duplicate Entry Exists");
                        fileout.WriteLine("ERROR: FAMILY : US25 : Name: " + matches[i].Name + ", Birthday: " + matches[i].BirthDay + ", Duplicate Entry Exists");
                    }
                }
            }


            // Keep the console window open in debug mode.
            Console.WriteLine("\n\n\n\nPress any key to exit.");
            fileout.Flush();
            fileout.Close();
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

        public static bool IsSiblingsMarryoneanother(FAM fam, List<FAM> Family)
        {
            foreach (var innerfam in Family)
            {
                if (innerfam.childeren != null)
                {
                    var childeren = innerfam.childeren;
                    string mychildern = childeren.Trim(new Char[] { ' ', ',' });
                    string[] child = mychildern.Split(',');

                    if (child.Contains(fam.HusbandID) && child.Contains(fam.Wifeid))
                    {
                        return true;
                    }

                }

            }
            return false;
        }




        //US19    First cousins should not marry : First cousins should not marry one another

        public static bool IsFirstCousinMarriedOneAnother(List<INDI> Individuals, string j, KeyValuePair<string, string[]> v, out string wifeid)
        {
            // bool IsmarriedOneAnother =false ;
            foreach (var ind in Individuals)
            {
                if (ind.ID == j)
                {
                    if (v.Value.Contains(ind.spouse))
                    {

                        wifeid = ind.spouse;
                        return true;
                    }
                }

            }

            wifeid = null;
            return false;
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
        public static void deadBeforeDay(List<INDI> Individuals, DateTime when,StreamWriter fileout)
        {
            //string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };

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

            Console.WriteLine("DEAD INDIVIDUALS BEFORE GIVEN DATE: " + when.ToString());
            fileout.WriteLine("DEAD INDIVIDUALS BEFORE GIVEN DATE: " + when.ToString());

            foreach (INDI i in deadIndi)
            {

                Console.WriteLine("ERROR: INDIVIDUAL : US29 :" + i.Name + " Died on: " + i.death);
                fileout.WriteLine("ERROR: INDIVIDUAL : US29 :" + i.Name + " Died on: " + i.death);
            }

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
        public static void datesBeforeToday(List<INDI> Individuals, List<FAM> Families, StreamWriter fileout)
        {
            //string[] columnsIndviduals = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };
            //string[] columnsFamily = { "FAMILYID", "MARRID", "DIVORCED", "DEATH", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };

            List<INDI> IndiWithDates = new List<INDI>();
            List<FAM> FamilyWithDates = new List<FAM>();

            INDI currentIndi = new INDI();
            bool changed = false;
            //Loop through and check all the death days on individuals

            Console.WriteLine("INDIVIDUALS WITH DATES AFTER CURRENT DATE: " + DateTime.Today.ToString());
            fileout.WriteLine("INDIVIDUALS WITH DATES AFTER CURRENT DATE: " + DateTime.Today.ToString());

            for (int i = 0; i < Individuals.Count(); i++)
            {
                currentIndi = Individuals[i];
                changed = isDateAfterTodayIndi(ref currentIndi, fileout);
                if (changed)
                    IndiWithDates.Add(currentIndi);
            }

            //Print the list of changed individuals
 
           

            Console.WriteLine("\n");
            fileout.WriteLine("\n");
            //Loop through all Families now
            changed = false;
            FAM currentFam = new FAM();
            Console.WriteLine("FAMILIES WITH DATES BEFORE CURRENT DATE: " + DateTime.Today.ToString());
            fileout.WriteLine("FAMILIES WITH DATES BEFORE CURRENT DATE: " + DateTime.Today.ToString());

            for (int i = 0; i < Families.Count(); i++)
            {
                currentFam = Families[i];
                changed = isDateAfterTodayFam(ref currentFam, fileout);
                if (changed)
                    FamilyWithDates.Add(currentFam);
            }

            //Print the list of changed families
           

            Console.WriteLine("\n");
            fileout.WriteLine("\n");
        }

        /// <summary>
        /// US01 List Date before Current Day
        /// Helper Method in Boolean Form
        /// </summary>
        /// <param name="IndiList"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static bool isDateAfterTodayIndi(ref INDI i, StreamWriter fileout)
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
                changed = true;
                Console.WriteLine("ERROR: INDIVIDUAL : US01 : " + i.Name + "Has Death Date: " + i.death);
                if(fileout != null)
                    fileout.WriteLine("ERROR: INDIVIDUAL : US01 : " + i.Name + "Has Death Date: " + i.death);
            }
            if (i.BirthDay == null || i.BirthDay.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(i.BirthDay))
            {
                changed = true;
                Console.WriteLine("ERROR: INDIVIDUAL : US01 : " + i.Name + "Has Birth Date: " + i.death);
                if (fileout != null)
                    fileout.WriteLine("ERROR: INDIVIDUAL : US01 : " + i.Name + "Has Birth Date: " + i.death);
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
        public static bool isDateAfterTodayFam(ref FAM f, StreamWriter fileout)
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
                changed = true;
                Console.WriteLine("ERROR: FAMILY : US01 : " + f.FamID + "Has Marriage Date: " + f.Married);
                if (fileout != null)
                    fileout.WriteLine("ERROR: FAMILY : US01 : " + f.FamID + "Has Marriage Date: " + f.Married);
            }
            if (f.Divorced == null || f.Divorced.Equals(""))
            {
                changed = false;
            }
            else if (today < Convert.ToDateTime(f.Divorced))
            {
                changed = true;
                Console.WriteLine("ERROR: FAMILY : US01 : " + f.FamID + "Has Divorce Date: " + f.Married);
                if (fileout != null)
                    fileout.WriteLine("ERROR: FAMILY : US01 : " + f.FamID + "Has Divorce Date: " + f.Married);
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

        /// <summary>
        /// US022	Unique ID's
        /// </summary>
        /// <param name="indiObj"></param>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>
        public static bool isIDUnique(List<INDI> indi, List<FAM> fam)
        {
            List<string> results = new List<string>();
            if (!isIDUniqueIndi(indi, ref results))
                return false;
            else if (!isIDUniqueFam(fam, ref results))
                return false;
            else return true;
        }
        public static bool isIDUniqueIndi(List<INDI> indi, ref List<string> results)
        {
            List<string> ids = new List<string>();
            bool endResult = true;
            //Loop through each individual
            foreach (INDI i in indi)
            {
                //Ensure individual ID's are not null
                if (i.ID != null)
                {
                    //if the id is not in the list, add it.  Otherwise, it's a copy and invalid
                    if (!ids.Contains(i.ID))
                    {
                        ids.Add(i.ID);
                        ids.Add(i.Name);
                    }
                    else
                    {
                        endResult = false;
                        results.Add(i.ID);
                        results.Add(i.Name);
                        int count = 0;
                        foreach(string s in ids)
                        {   
                            if (s.Equals(i.ID))
                            {
                                results.Add(ids[count + 1]);
                                break;
                            }
                            count++;
                        }
                    }

                }   
            }
            return endResult;
        }
        public static bool isIDUniqueFam(List<FAM> fam, ref List<string> results)
        {
            List<string> ids = new List<string>();
            bool endResult = true;
            //Loop through each family
            foreach (FAM f in fam)
            {
                //Ensure family ID's are not null
                if (f.FamID != null)
                {
                    //if the id is not in the list, add it.  Otherwise, it's a copy and invalid
                    if (!ids.Contains(f.FamID))
                        ids.Add(f.FamID);
                    else
                    {
                        endResult = false;
                        results.Add(f.FamID);
                    }

                }
            }
            return endResult;
        }

        /// <summary>
        /// US023	Unique names and birthdates
        /// </summary>
        /// <param name="indiObj"></param>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>

        public static bool isNameBirthdayUnique(List<INDI> indi, ref List<string> namesResult, ref List<string> birthdaysResult)
        {
            List<string>names = new List<string>();
            List<string>birthdays = new List<string>();
            bool endResult = true;

            //Loop through each individual
            foreach (INDI i in indi)
            {
                //Ensure parameters aren't null
                if (i.Name != null && i.BirthDay != null)
                {
                    //If one or the other record doesn't exist, it doesn't matter, add it
                    if (!names.Contains(i.Name) || !birthdays.Contains(i.BirthDay))
                    {
                        names.Add(i.Name);
                        birthdays.Add(i.BirthDay);
                    }
                    //If the name exists, check for birthday
                    else if (names.Contains(i.Name))
                    {
                        int counter = 0;
                        foreach(String s in names)
                        {
                            if(s.Equals(i.Name))
                            {
                                //If both matched, return false
                                if(birthdays[counter].Equals(i.BirthDay))
                                {
                                    endResult = false;
                                    namesResult.Add(i.Name);
                                    birthdaysResult.Add(i.BirthDay);
                                }
                            }

                            //Increment the counte because name matched, but not birthday
                            counter++;
                        }
                    }
                    //Name or birthday existed, but not for the same person.  Add it.  
                    else
                    {
                        names.Add(i.Name);
                        birthdays.Add(i.BirthDay);
                    }    
                }
            }
            return endResult;
        }

        /// <summary>
        /// US024	Unique families by spouses
        /// </summary>
        /// <param name="fam"></param>
        /// <returns> bool </returns>
        
        public static bool isUniqueFamily (List<FAM> fam, ref List<FAM> results)
        {
            bool unique = true;

            List<string> familyDetails = new List<string>();
            //Store in order husband, wife, marriage date

            foreach (FAM f in fam)
            {
                for (int i = 0; i < familyDetails.Count(); i+=3)
                {
                    if (f.Husbandname != null && f.Husbandname.Equals(familyDetails[i]))
                    {
                        if (f.Wifename != null && f.Wifename.Equals(familyDetails[i+1]))
                        {
                            if (f.Married != null && f.Married.Equals(familyDetails[i+2]))
                            {
                                unique = false;
                                results.Add(f);
                            }
                        }
                    }
                }
                familyDetails.Add(f.Husbandname);
                familyDetails.Add(f.Wifename);
                familyDetails.Add(f.Married);
            }

            return unique;
        }

        /// <summary>
        /// US025	Unique names in family
        /// </summary>
        /// <param name="famObj"></param>
        /// <returns> bool </returns>

        public static bool isUniqueFamilyNames(FAM fam, List<INDI> indi, ref List<INDI> matches)
        {
            bool unique = true;
            
            List<INDI> children = new List<INDI>();
            List<INDI> parsedChildren = new List<INDI>();

            if (fam.childeren == null)
                return true;
            char[] delimeter = { ',' };
            string[] childrenIDs = fam.childeren.Split(delimeter);

            for (int i = 0; i < childrenIDs.Length; i++)
            {
                foreach (INDI ind in indi)
                {
                    if (childrenIDs[i].Equals(ind.ID))
                    {
                        children.Add(ind);
                    }

                }
            }

            for (int i = 0; i < children.Count; i++)
            {
                for(int j = 0; j < parsedChildren.Count; j++)
                {
                    if ((children[i].Name != null) && parsedChildren[j].Name.Equals(children[i].Name))
                    {
                        if((children[i].BirthDay != null ) && children[j].BirthDay.Equals(children[i].BirthDay))
                        //Ensure duplicate entries aren't added
                        if (!matches.Contains(children[j]))
                            matches.Add(children[j]);
                        unique = false;
                    }
                }
            
                parsedChildren.Add(children[i]);
            }
            
            return unique;
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
