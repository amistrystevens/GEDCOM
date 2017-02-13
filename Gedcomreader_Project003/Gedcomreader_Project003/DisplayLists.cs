using System.Collections.Generic;
using System.Windows.Forms;
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
            foreach (INDI l in IndividualsList)
            {
                string info = l.ID + "," + l.Name + "," + l.Sex + "," + l.BirthDay + "," + l.age + "," + l.Alive + "," + l.death + "," + l.Child + "," + l.spouse;
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
