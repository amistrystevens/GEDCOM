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

            //string path = "C:\\Users\\kumara\\Downloads\\TestGED\\TGC55C.ged";
            string path = "C:\\Users\\Amit\\Desktop\\test\\TGC55C.ged";

            string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };


            string[] columnsfamily = { "FAMILYID", "MARRID", "DIVORCED", "DEATH", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };


            //Open path to GED file
            StreamReader SR = new StreamReader(path);

            //Read entire block and then plit on 0 @ for individuals and familys (no other info is needed for this instance)
            string[] Holder = SR.ReadToEnd().Replace("0 @", "\u0646").Split('\u0646');

            //For each new cell in the holder array look for Individuals and familys
            foreach (string Node in Holder)
            {

                //Sub Split the string on the returns to get a true block of info
                string[] SubNode = Node.Replace("\r\n", "\r").Split('\r');
                //If a individual is found
                if (SubNode[0].Contains("INDI"))
                {
                    //Create new Structure
                    INDI I = new INDI();
                    //Add the ID number and remove extra formating
                    I.ID = SubNode[0].Replace("@", "").Replace(" INDI", "").Trim();
                    //Find the name remove extra formating for last name
                    I.Name = SubNode[FindIndexinArray(SubNode, "NAME")].Replace("1 NAME", "").Replace("/", "").Trim();
                    //Find Sex and remove extra formating
                    // I.Sex = SubNode[FindIndexinArray(SubNode, "SEX")].Replace("1 SEX ", "").Trim();

                    if (FindIndexinArray(SubNode, "SEX") != -1)
                    {
                        //Find Sex and remove extra formating
                        I.Sex = SubNode[FindIndexinArray(SubNode, "SEX")].Replace("1 SEX", "").Trim();

                    }



                    //Deterine if there is a brithday -1 means no
                    if (FindIndexinArray(SubNode, "1 BIRT ") != -1)
                    {
                        // add birthday to Struct 
                        I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 BIRT ") + 1].Replace("2 DATE ", "").Trim();
                    }



                    // deterimin if there is a death tag will return -1 if not found
                    if (FindIndexinArray(SubNode, "1 DEAT ") != -1)
                    {
                        //convert Y or N to true or false ( defaults to False so no need to change unless Y is found.
                        if (SubNode[FindIndexinArray(SubNode, "1 DEAT ")].Replace("1 DEAT ", "").Trim() == "Y")
                        {
                            //set death
                            I.Dead = true;
                        }

                    }
                    //add the Struct to the list for later use
                    Individuals.Add(I);
                }

                // Start Family section
                else if (SubNode[0].Contains("FAM"))
                {
                    //grab Fam id from node early on to keep from doing it over and over
                    string FamID = SubNode[0].Replace("@ FAM", "");

                    bool checkparent = false;
                    // Multiple children can exist for each family so this section had to be a bit more dynaimic

                    // Look at each line of node
                    foreach (string Line in SubNode)
                    {
                        // If node is HUSB
                        if (Line.Contains("1 HUSB "))
                        {

                            FAM F = new FAM();
                            F.FamID = FamID;
                            F.type = "PAR";
                            F.HusbandID = Line.Replace("1 HUSB ", "").Replace("@", "").Trim();
                            Family.Add(F);
                            checkparent = true;
                        }
                        //If node for Wife
                        else if (Line.Contains("1 WIFE "))
                        {
                            FAM F = new FAM();
                            F.FamID = FamID;
                            F.type = "PAR";
                            F.Wifeid = Line.Replace("1 WIFE ", "").Replace("@", "").Trim();
                            Family.Add(F);
                        }
                        else if (Line.Contains("1 MARR"))
                        {
                            FAM F = new FAM();
                            //I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 MARR") + 1].Replace("2 DATE ", "").Trim();
                            F.FamID = FamID;
                            F.type = "MarrageDate";
                            //F.Married = Line.Replace("2 DATE", "").Trim();
                            F.Married = SubNode[FindIndexinArray(SubNode, "1 MARR") + 1].Replace("2 DATE ", "").Trim();
                            Family.Add(F);
                            checkparent = false;
                        }
                        else if (Line.Contains("1 DEAT"))
                        {
                            FAM F = new FAM();
                            //I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 MARR") + 1].Replace("2 DATE ", "").Trim();
                            F.FamID = FamID;
                            F.type = "DeathDate";
                            //F.Married = Line.Replace("2 DATE", "").Trim();
                            F.Death = SubNode[FindIndexinArray(SubNode, "1 DEAT") + 1].Replace("2 DATE ", "").Trim();
                            Family.Add(F);
                            // checkparent = false;
                        }
                        else if (Line.Contains("1 DIV"))
                        {
                            FAM F = new FAM();
                            //I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 MARR") + 1].Replace("2 DATE ", "").Trim();
                            F.FamID = FamID;
                            F.type = "DivorceDate";
                            //F.Married = Line.Replace("2 DATE", "").Trim();
                            F.Divorced = SubNode[FindIndexinArray(SubNode, "1 DIV") + 1].Replace("2 DATE ", "").Trim();
                            Family.Add(F);
                            // checkparent = false;
                        }
                        //if node for multi children
                        else if (Line.Contains("1 CHIL "))
                        {
                            FAM F = new FAM();
                            F.FamID = FamID;
                            F.type = "CHIL";
                            F.childeren = Line.Replace("1 CHIL ", "").Replace("@", "");
                            F.IndiID = Line.Replace("1 CHIL ", "").Replace("@", "");
                            Family.Add(F);
                        }
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

            var filered = Family.Select(x => x.FamID).Distinct();

            foreach (string s in filered)
            {
                List<FAM> filteredList = Family.Where(x => x.FamID == s).ToList();
                FAM fill = new FAM();
                string childstr = string.Empty;
                Dictionary<string, string> lst = new Dictionary<string, string>();
                lst.Add("Familys", s);
                foreach (FAM obj in filteredList)
                {
                    if (obj.Married != null)
                    {
                        //fill.Married = obj.Married;
                        lst.Add("MARRIAGE", obj.Married);
                    }
                    else if (obj.HusbandID != null)
                    {
                        //fill.HusbandID = obj.HusbandID;
                        lst.Add("HUSBANDID", obj.HusbandID);
                    }
                    else if (obj.Wifeid != null)
                    {
                        //fill.Wifeid = obj.Wifeid;
                        lst.Add("WifeID", obj.Wifeid);
                    }
                    else if (obj.childeren != null)
                    {
                        childstr += obj.childeren;
                    }
                    else if (obj.Divorced != null)
                    {
                        if (!lst.ContainsKey("Divorced"))
                        {
                            lst.Add("Divorced", obj.Divorced);
                        }
                    }
                    else if (obj.Death != null)
                    {
                        // if (!lst.ContainsKey("Divorced"))
                        {
                            lst.Add("Death", obj.Death);
                        }

                    }

                }
                lst.Add("childeren", childstr);

                FAM newfam = new FAM();
                if (lst.Count > 0)
                {

                    foreach (KeyValuePair<string, string> pair in lst)
                    {
                        //    MessageBox.Show(pair.Key.ToString() + "  -  " + pair.Value.ToString());
                        if (pair.Key == "Familys")
                        {
                            newfam.FamID = pair.Value;
                        }
                        else if (pair.Key == "HUSBANDID")
                        {
                            newfam.HusbandID = pair.Value;
                        }
                        else if (pair.Key == "WifeID")
                        {
                            newfam.Wifeid = pair.Value;
                        }
                        else if (pair.Key == "MARRIAGE")
                        {
                            newfam.Married = pair.Value;
                        }
                        else if (pair.Key == "childeren")
                        {
                            newfam.childeren = pair.Value;
                        }
                        else if (pair.Key == "Divorced")
                        {
                            newfam.Divorced = pair.Value;
                        }
                        else if (pair.Key == "Death")
                        {
                            newfam.Death = pair.Value;
                        }
                    }

                    if (IsValidDateforMarriageDeath(newfam))
                    {
                        customfam.Add(newfam);
                    }

                    /*
                    if (IsValidDate(newfam))
                     {
                         customfam.Add(newfam);
                     }
                     */

                }
                newfam = null;
                lst = null;
            }


            printFamily(customfam);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();


            //  List<FAM> child = Family.Where(x => x.FamID == s && x.type == "CHIL").ToList();
            //   string test = string.Empty;
            //foreach (FAM c in child)
            //{
            //    test = test + " " + c.IndiID;
            //}
            //fill.childeren = test;

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
                Console.WriteLine("{0,-5}{1,-10}{2,-30}{3,-34}{4,-40}{5,-50}{6,-60}{7,-70}{8,-80}", l.ID, l.Name, l.Sex, l.BirthDay, l.age, l.Alive, l.death, l.Child, l.spouse);
                PrintLine();
            }
        }

        static private void printFamily(List<FAM> lst)
        {
            foreach (FAM f in lst)
            {
                Console.WriteLine("{0,-15}{1,-20}{2,-30}{3,-35}{4,-40}{5,-42}{6,-43}{7,-45}{8,-47}",
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



    #region COMMENTED CODE
    //public class Program
    //{
    //    static int tableWidth = 177;

    //    static void Main(string[] args)
    //    {
    //        // The files used in this example are created in the topic
    //        // How to: Write to a Text File. You can change the path and
    //        // file name to substitute text files of your own.

    //        string path = "C:\\Users\\Amit\\Desktop\\test\\TGC551.ged";
    //        //string path = "C:\\test\\sample_family.ged";
    //        // string path = "C:\\Users\\Amit\\Desktop\\Agile\\Amitkumar_mistry_Project002\\GED\\Amit_Mistry_Project001-BloodTree.ged";

    //        // Example #1
    //        // Read the file as one string.
    //        string text = System.IO.File.ReadAllText(@"C:\\test\\data.txt");

    //        Hashtable hs = new Hashtable();

    //        List<INDI> Individuals = new List<INDI>();
    //        List<FAM> Family = new List<FAM>();



    //        // Display the file contents to the console. Variable text is a string.
    //        //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

    //        // Example #2
    //        // Read each line of the file into a string array. Each element
    //        // of the array is one line of the file.
    //        //string[] lines = System.IO.File.ReadAllLines(@"C:\\Users\\Amit\\Desktop\\test\\TGC551.ged");

    //        string[] lines = System.IO.File.ReadAllLines(path);

    //        //// Display the file contents by using a foreach loop.
    //        //System.Console.WriteLine("Contents of WriteLines2.txt = ");

    //        //foreach (string line in lines)
    //        //{
    //        //    // Use a tab to indent each line of the file.
    //        //    Console.WriteLine("\t" + line);
    //        //}

    //        //Open path to GED file
    //        StreamReader SR = new StreamReader(path);

    //        //Read entire block and then plit on 0 @ for individuals and familys (no other info is needed for this instance)
    //        string[] Holder = SR.ReadToEnd().Replace("0 @", "\u0646").Split('\u0646');

    //        string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };


    //        string[] columnsfamily = { "FAMILYID", "MARRID", "DIVORCED", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };


    //        foreach (string Node in Holder)
    //        {

    //            //Sub Split the string on the returns to get a true block of info
    //            string[] SubNode = Node.Replace("\r\n", "\r").Split('\r');
    //            //If a individual is found

    //            for (int i=0;i<SubNode.Length;i++)
    //            {
    //                if (SubNode[i].Contains("INDI"))
    //                {
    //                    //Create new Structure
    //                    INDI I = new INDI();
    //                    //Add the ID number and remove extra formating
    //                    I.ID = SubNode[i].Replace("@", "").Replace(" INDI", "").Trim();
    //                    //Find the name remove extra formating for last name
    //                    I.Name = SubNode[FindIndexinArray(SubNode, "NAME")].Replace("1 NAME", "").Replace("/", "").Trim();

    //          //          I.Name= SubNode[++i].Replace("1 NAME", "").Replace("/", "").Trim();

    //                    if (FindIndexinArray(SubNode, "SEX") != -1)
    //                    {
    //                        //Find Sex and remove extra formating
    //                        I.Sex = SubNode[FindIndexinArray(SubNode, "SEX")].Replace("1 SEX", "").Trim();

    //                    }


    //                    //Deterine if there is a brithday -1 means no
    //                    if (FindIndexinArray(SubNode, "1 BIRT") != -1)
    //                    {
    //                        // add birthday to Struct 
    //                        I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 BIRT") + 1].Replace("2 DATE ", "").Trim();
    //                    }

    //                    // deterimin if there is a death tag will return -1 if not found
    //                    if (FindIndexinArray(SubNode, "1 DEAT") != -1)
    //                    {
    //                        //convert Y or N to true or false ( defaults to False so no need to change unless Y is found.
    //                        if (SubNode[FindIndexinArray(SubNode, "1 DEAT")].Replace("1 DEAT ", "").Trim() == "Y")
    //                        {
    //                            //set death
    //                            I.death = SubNode[FindIndexinArray(SubNode, "1 DEAT")].Replace("1 DEAT ", "").Trim();
    //                            I.Dead = true;
    //                        }
    //                    }   




    //                    //add the Struct to the list for later use
    //                    Individuals.Add(I);

    //                    hs.Add(I.ID, I);

    //                }

    //                // Start Family section
    //                else if (SubNode[i].Contains("FAM")) //(Regex.IsMatch(SubNode[i], @"\bFAM\b") 
    //                {
    //                    //grab Fam id from node early on to keep from doing it over and over
    //                    string FamID = SubNode[i].Replace("@ FAM", "");

    //                    // Multiple children can exist for each family so this section had to be a bit more dynaimic
    //                    FAM F = new FAM();
    //                    List<string> str = new List<string>();
    //                    // Look at each line of node
    //                    foreach (string Line in SubNode)
    //                    {
    //                        // If node is HUSB

    //                        if (Line.Contains("1 HUSB"))
    //                        {


    //                            //        FAM F = new FAM();
    //                            F.FamID = FamID;
    //                            // F.type = "PAR";
    //                            // F.HusbandID = Line.Replace("1 HUSB", "").Replace("@", "").Trim();
    //                             string husbandid = Line.Replace("1 HUSB", "").Replace("@", "").Trim();
    //                            //Family.Add(F);
    //                            str.Add(FamID);
    //                            str.Add(husbandid);
    //                        }
    //                        //If node for Wife
    //                        else if (Line.Contains("1 WIFE"))
    //                        {
    //                            //  FAM F = new FAM();
    //                            //  F.FamID = FamID;
    //                            //  F.type = "PAR";
    //                            // F.Wifeid = Line.Replace("1 WIFE", "").Replace("@", "").Trim();

    //                            string wifeid = Line.Replace("1 WIFE", "").Replace("@", "").Trim();

    //                            str.Add(wifeid);   

    //                           // Family.Add(F);
    //                        }
    //                        else if(Line.Contains("1 MARR"))
    //                        {

    //                        }
    //                        else if(SubNode[i].Contains("FAM") && Line.Contains("2 DATE"))
    //                        {
    //                            if (str.Count > 0)
    //                            {
    //                                string marriagedate = Line.Replace("2 DATE", "").Trim();
    //                                str.Add(marriagedate);
    //                            }
    //                        }
    //                        //if node for multi children
    //                        else if (Line.Contains("1 CHIL"))
    //                        {
    //                          //  FAM F = new FAM();
    //                           // F.FamID = FamID;
    //                           // F.type = "CHIL";
    //                           // F.IndiID = Line.Replace("1 CHIL", "").Replace("@", "");
    //                            str.Add(Line.Replace("1 CHIL", "").Replace("@", "").Trim());
    //                            //F.childeren= new string[] { Line.Replace("1 CHIL", "").Replace("@", "") };
    //                        }

    //                       if(str.Count>=6)
    //                        {
    //                            FAM F1 = new FAM();
    //                             // Loop with for and use string interpolation to print values.
    //                            for (int j = 0; j < str.Count; j++)
    //                            {
    //                                switch(j)
    //                                {
    //                                    case 0:
    //                                        F1.FamID = str[j];
    //                                        break;
    //                                    case 1:
    //                                        F1.Married = str[j];
    //                                        break;
    //                                    case 2:
    //                                        F1.HusbandID = str[j];
    //                                        break;
    //                                    case 3:
    //                                        F1.Wifeid = str[j];
    //                                        break;
    //                                    case 4:
    //                                        F1.childeren = str[j];
    //                                        break;
    //                                    case 5:
    //                                        F1.childeren= F1.childeren+ F1.childeren;
    //                                        break;
    //                                    case 6:
    //                                        F1.childeren = F1.childeren + F1.childeren;
    //                                        break;
    //                                }
    //                            }
    //                            Family.Add(F1);

    //                            str = null;

    //                        }
    //                        /*
    //                        if (F.FamID !=null )
    //                        {
    //                            Family.Add(F);
    //                        }*/

    //                    }
    //                }
    //            }
    //        }
    //        PrintLine();
    //        PrintRow(columns);
    //        PrintLine();

    //        printIndividual(Individuals);

    //        Console.WriteLine("\n");

    //        PrintLine();
    //        PrintRow(columnsfamily);
    //        PrintLine();
    //        printFamily(Family);

    //        // Keep the console window open in debug mode.
    //        Console.WriteLine("Press any key to exit.");
    //        System.Console.ReadKey();
    //    }

    //   static private int FindIndexinArray(string[] Arr, string search)
    //    {
    //        int Val = -1;
    //        for (int i = 0; i < Arr.Length; i++)
    //        {
    //            if (Arr[i].Contains(search.Trim()))
    //            {
    //                Val = i;
    //            }
    //        }
    //        return Val;
    //    }



    //    static private void PrintLine()
    //    {
    //        Console.WriteLine(new string('-', tableWidth));
    //    }

    //    static private void PrintRow(params string[] columns)
    //    {
    //        int width = (tableWidth - columns.Length) / columns.Length;
    //        string row = "|";

    //        foreach (string column in columns)
    //        {
    //            row += AlignCentre(column, width) + "|";
    //        }

    //        Console.WriteLine(row);
    //    }

    //    static private void printIndividual(List<INDI> lst)
    //    {
    //        foreach(INDI l in lst)
    //        {
    //            Console.WriteLine("{0,-10} {1,-35} {2,-40} {3,-50} {4,-60} {5,-70} {6,-75} {7,-80} {8,-90}", l.ID,l.Name,l.Sex,l.BirthDay,l.age,l.Alive,l.death,l.Child,l.spouse);
    //            PrintLine();
    //        }
    //    }

    //    static private void printFamily(List<FAM> lst)
    //    {
    //        foreach ( FAM f in lst)
    //        {
    //            Console.WriteLine("{0,-10},{1,-30},{2,-40},{3,-50},{4,-60},{5,-70},{6,-75},{7,-80}",
    //                f.FamID, f.Married, f.Divorced,f.HusbandID,f.Husbandname,f.Wifeid,f.Wifename,f.childeren);
    //        }
    //    }


    //    static private string AlignCentre(string text, int width)
    //    {
    //        text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

    //        if (string.IsNullOrEmpty(text))
    //        {
    //            return new string(' ', width);
    //        }
    //        else
    //        {
    //            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
    //        }
    //    }





    //}
    #endregion
}
