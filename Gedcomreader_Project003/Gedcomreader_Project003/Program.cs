using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gedcomreader_Project003.GedcomCls;
using System.Collections;
using System.Text.RegularExpressions;

using System.Windows.Forms;

namespace Gedcomreader_Project003
{
  public class Program
    {
        static int tableWidth = 177;

        [STAThread]
        static void Main(string[] args)
        {
            //--------------------------
            // Read Files relative to user
            // directory in debug
            //--------------------------

            string path = (Environment.CurrentDirectory).Substring(0, Environment.CurrentDirectory.Length-9) + "TGC551.ged";
            
            // The files used in this example are created in the topic
            // How to: Write to a Text File. You can change the path and
            // file name to substitute text files of your own.

            //string path = "C:\\Users\\Amit\\Desktop\\test\\TGC551.ged";
            //string path = "C:\\test\\sample_family.ged";
            // string path = "C:\\Users\\Amit\\Desktop\\Agile\\Amitkumar_mistry_Project002\\GED\\Amit_Mistry_Project001-BloodTree.ged";

            // Example #1
            // Read the file as one string.
            //string text = System.IO.File.ReadAllText(@"C:\\test\\data.txt");

            Hashtable hs = new Hashtable();

            List<INDI> Individuals = new List<INDI>();
            List<FAM> Family = new List<FAM>();



            // Display the file contents to the console. Variable text is a string.
            //System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

            // Example #2
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            //string[] lines = System.IO.File.ReadAllLines(@"C:\\Users\\Amit\\Desktop\\test\\TGC551.ged");

            string[] lines = System.IO.File.ReadAllLines(path);

            //// Display the file contents by using a foreach loop.
            //System.Console.WriteLine("Contents of WriteLines2.txt = ");

            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    Console.WriteLine("\t" + line);
            //}

            //Open path to GED file
            StreamReader SR = new StreamReader(path);

            //Read entire block and then plit on 0 @ for individuals and familys (no other info is needed for this instance)
            string[] Holder = SR.ReadToEnd().Replace("0 @", "\u0646").Split('\u0646');
            
            string[] columns = { "ID", "NAME", "Gender", "Birthday", "Age", "Alive", "Death", "child", "spouse" };

            
            string[] columnsfamily = { "FAMILYID", "MARRID", "DIVORCED", "HUSBANDID", "HUSBANDNAME", "WIFEID", "WIFENAME", "CHILDREN" };


            foreach (string Node in Holder)
            {

                //Sub Split the string on the returns to get a true block of info
                string[] SubNode = Node.Replace("\r\n", "\r").Split('\r');
                //If a individual is found

                for (int i=0;i<SubNode.Length;i++)
                {
                    if (SubNode[i].Contains("INDI"))
                    {
                        //Create new Structure
                        INDI I = new INDI();
                        //Add the ID number and remove extra formating
                        I.ID = SubNode[i].Replace("@", "").Replace(" INDI", "").Trim();
                        //Find the name remove extra formating for last name
                        I.Name = SubNode[FindIndexinArray(SubNode, "NAME")].Replace("1 NAME", "").Replace("/", "").Trim();

              //          I.Name= SubNode[++i].Replace("1 NAME", "").Replace("/", "").Trim();

                        if (FindIndexinArray(SubNode, "SEX") != -1)
                        {
                            //Find Sex and remove extra formating
                            I.Sex = SubNode[FindIndexinArray(SubNode, "SEX")].Replace("1 SEX", "").Trim();

                        }


                        //Deterine if there is a brithday -1 means no
                        if (FindIndexinArray(SubNode, "1 BIRT") != -1)
                        {
                            // add birthday to Struct 
                            I.BirthDay = SubNode[FindIndexinArray(SubNode, "1 BIRT") + 1].Replace("2 DATE ", "").Trim();
                        }

                        // deterimin if there is a death tag will return -1 if not found
                        if (FindIndexinArray(SubNode, "1 DEAT") != -1)
                        {
                            //convert Y or N to true or false ( defaults to False so no need to change unless Y is found.
                            if (SubNode[FindIndexinArray(SubNode, "1 DEAT")].Replace("1 DEAT ", "").Trim() == "Y")
                            {
                                //set death
                                I.death = SubNode[FindIndexinArray(SubNode, "1 DEAT")].Replace("1 DEAT ", "").Trim();
                                I.Dead = true;
                            }
                        }




                        //add the Struct to the list for later use
                        Individuals.Add(I);

                        hs.Add(I.ID, I);

                    }

                    // Start Family section
                    else if (SubNode[i].Contains("FAM")) //(Regex.IsMatch(SubNode[i], @"\bFAM\b") 
                    {
                        //grab Fam id from node early on to keep from doing it over and over
                        string FamID = SubNode[i].Replace("@ FAM", "");

                        // Multiple children can exist for each family so this section had to be a bit more dynaimic

                        // Look at each line of node
                        foreach (string Line in SubNode)
                        {
                            // If node is HUSB
                            if (Line.Contains("1 HUSB"))
                            {

                                FAM F = new FAM();
                                F.FamID = FamID;
                                F.type = "PAR";
                                F.IndiID = Line.Replace("1 HUSB", "").Replace("@", "").Trim();
                                Family.Add(F);
                            }
                            //If node for Wife
                            else if (Line.Contains("1 WIFE"))
                            {
                                FAM F = new FAM();
                                F.FamID = FamID;
                                F.type = "PAR";
                                F.IndiID = Line.Replace("1 WIFE", "").Replace("@", "").Trim();
                                Family.Add(F);
                            }
                            //if node for multi children
                            else if (Line.Contains("1 CHIL"))
                            {
                                FAM F = new FAM();
                                F.FamID = FamID;
                                F.type = "CHIL";
                                F.IndiID = Line.Replace("1 CHIL", "").Replace("@", "");
                                Family.Add(F);
                            }
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
            printFamily(Family);

            Application.EnableVisualStyles();

            DisplayLists form1 = new DisplayLists();
            form1.addIndividuals(Individuals);
            form1.addFamilies(Family);
            Application.Run(form1);
            
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

       static private int FindIndexinArray(string[] Arr, string search)
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

        

        static private void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static private void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static private void printIndividual(List<INDI> lst)
        {
            foreach(INDI l in lst)
            {
                Console.WriteLine("{0,-10} {1,-35} {2,-40} {3,-50} {4,-60} {5,-70} {6,-75} {7,-80} {8,-90}", l.ID,l.Name,l.Sex,l.BirthDay,l.age,l.Alive,l.death,l.Child,l.spouse);
                PrintLine();
            }
        }

        static private void printFamily(List<FAM> lst)
        {
            foreach ( FAM f in lst)
            {
                Console.WriteLine("{0,-10},{1,-30},{2,-40},{3,-50},{4,-60},{5,-70},{6,-75},{7,-80}",
                    f.FamID, f.Married, f.Divorced,f.HusbandID,f.Husbandname,f.Wifeid,f.Wifename,f.childeren);
            }
        }


        static private string AlignCentre(string text, int width)
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
}
