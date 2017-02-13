using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using Gedcomreader_Project003.GedcomCls;


namespace Gedcomreader_Project003
{
    public partial class DisplayLists : Form
    {
        public DisplayLists()
        {
            InitializeComponent();
        }
        public void addIndividuals(List<INDI> IndividualsList)
        {
            string title = "ID, Name, Gender, Birthday, Age, Alive, Death, Child, Spouse";
            Individuals.Items.Add(title);
            foreach (INDI l in IndividualsList)
            {
                string id = "" + l.ID;
                //IID.Items.Add(id);
                string name = "" + l.Name;
                //IName.Items.Add(name);
                string sex = "" + l.Sex;
                //ISex.Items.Add(sex);
                string birthDay = "" + l.BirthDay;
                //IBirthday.Items.Add(birthDay);
                string age = "" + l.age.ToString();
                //IAge.Items.Add(age);
                string alive = "" + l.Alive.ToString();
                //IAlive.Items.Add(alive);
                string death = "" + l.death;
                //IDeath.Items.Add(death);
                string child = "" + l.Child;
                //IChild.Items.Add(child);
                string spouse = "" + l.spouse;
                //ISpouse.Items.Add(spouse);

                string info = id + "," + name + "," + sex + "," + birthDay + "," + age + "," + alive + "," + death + "," + child + "," + spouse;
                Individuals.Items.Add(info);
            }
        }
        public void addFamilies(List<FAM> FamiliesList)
        {
            foreach (FAM f in FamiliesList)
            {
                string info = f.FamID + "," + f.Married + "," + f.Divorced + "," + f.HusbandID + "," + f.Husbandname + "," + f.Wifeid + "," + f.Wifename + "," + f.childeren;
                Families.Items.Add(info);
            }
        }
    }
}
