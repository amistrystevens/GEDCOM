//ANTHONY SETOLA SUBMISSION
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <typeinfo>
#include <cppunit/CompilerOutputter.h>
#include <cppunit/extensions/TestFactoryRegistry.h>
#include <cppunit/ui/text/TestRunner.h> 
using namespace std;

void read()
{
	vector <string> words;
	string line;
	vector <string> validtags = {"INDI","NAME","SEX","BIRT","DEAT","FAMC","FAMS","FAM","MARR","HUSB","WIFE","CHIL","DIV","DATE","HEAD","TRLR","NOTE"};
	ifstream infile("gedcominput.ged");
	ofstream outfile;
	outfile.open("gedcomoutput.txt");
	while(getline(infile,line))
	{
		words.clear();
		istringstream iss(line);
	    string word;
	    bool valid = false;
	    while(iss >> word) 
	    {
	    	words.push_back(word);
	    }
		cout << line << endl;
		outfile << line << endl;
		cout << "Level is " << line[0] << endl;
		outfile << "Level is " << line[0] << endl;
		for(int i=0; i<validtags.size();i++)
		{
			if(words[1] == validtags[i])
			{
				cout << words[1] << endl;
				outfile << words[1] << endl;
				valid=true;
			}
		}
		if (!valid)
		{
			if(words[2] == "FAM" || words[2] == "INDI")
			{
				cout << words[2] << endl;
				outfile << words[2] << endl;
			}
			else
			{
				cout << "INVALID ID" << endl;
				outfile << "INVALID ID" << endl;
			}
		}
		cout << "--------------------------------" << endl;
		outfile << "--------------------------------" << endl;
	}
}

void check()
{
	vector <string> words;
	vector <string> name;
	vector <string> born;
	vector <string> marr;
	vector <string> deat;
	vector <string> mpp;
	vector< vector<string> > people;
	vector <string> person;
	vector< vector<string> > families;
	vector <string> family;
	vector< vector<string> > genders;
	vector <string> gender;
	string line;
	string last="";
	string word;
	ifstream infile("gedcominput.ged");
	ofstream outfile;
	outfile.open("gedcomoutput.txt", ios::app);
	string a;
	cout << "##############ERRORS##############"<< endl;
	outfile << "##############ERRORS##############"<< endl;
	while(getline(infile,line))
	{
		words.clear();
		istringstream iss(line);
	    string word="";
	    bool valid = false;
	    while(iss >> word) 
	    {
	    	words.push_back(word);
	    }
		if(words[2] == "INDI")
			{
				people.push_back(person);
				person.clear();
				name.push_back(words[1]);
				person.push_back(words[1]);
				last="INDI";
			}
		if(words[1] == "NAME")
		{
			person.push_back(words[2]+words[3]);	
		}
		if(words[1] == "SEX")
		{
			person.push_back(words[2]);	
		}
		if(words[1] == "MARR")
			{
				last="MARR";
			}
		if(words[1] == "DEAT")
			{
				last="DEAT";
			}
		if(words[1] == "DATE")
		{
			a="";
			if(last =="INDI")
			{
				a = words[2]+words[3]+words[4];
				born.push_back(a);
				person.push_back("BORN "+a);
			}
			if(last =="MARR")
			{
				a = words[2]+words[3]+words[4];
				marr.push_back(a);
				family.push_back("MARR "+a);
			}
			if(last =="DEAT")
			{
				a = words[2]+words[3]+words[4];
				deat.push_back(a);
				person.push_back("DEAT "+a);
			}
			last="";
		}
		if(words[1] == "HUSB" || words[1] == "WIFE")
		{
			mpp.push_back(words[2]);
		}
		if(words[2] == "FAM")
		{
			families.push_back(family);
			family.clear();
			family.push_back(words[1]);
		}
		if(words[1] == "HUSB" || words[1] == "WIFE" || words[1] == "CHIL")
		{
			family.push_back(words[2]);
			gender.push_back(words[2]);
			gender.push_back(words[1]);
			genders.push_back(gender);
			gender.clear();
		}
	}
	people.push_back(person);
	families.push_back(family);




/*
//TESTING PEOPLE
	for(int i=0;i<people.size();i++)
	{
		for(int j=0;j<people[i].size();j++)
		{
			cout << people[i][j] << " ";
		}
		cout << endl;
	}
//TESTING FAMILIES
	for(int i=0;i<families.size();i++)
	{
		for(int j=0;j<families[i].size();j++)
		{
			cout << families[i][j] << " ";
		}
		cout << endl;
	}
*/

	for(int i=0; i<deat.size();i++)
	{
		if(born[i] > deat[i])
		{
			cout << name[i] << "is not correct <DEATH>" << endl;
			outfile << name[i] << "is not correct <DEATH>" << endl;
		}
	}
	marr.empty();
	for(int i=1;i<families.size();i++)
	{
		for(int j=2;j<4;j++)
		{
			marr.push_back(families[i][j]);
		}
	}
	for(int i=0; i<marr.size();i++)
		for(int j=i+1;j<marr.size();j++)
			if(marr[i] == marr[j])
			{
				cout << "A person is married more than once" << endl;
				outfile << "A person is married more than once" << endl;
			}
	string famname="";
	for(int i=0;i<families.size();i++)
	{
		for(int j=2;j<families[i].size();j++)
		{
			for(int k=1; k<people.size();k++)
			{
				if(families[i][j] == people[k][0] && people[k][people[k].size()-1] == "M")
				{
					if(famname == "")
					{
						famname=people[k][1];
						famname=famname.substr(famname.find('/')+1,famname.size());
					}
					if(people[k][1].substr(people[k][1].find('/')+1,people[k][1].size()) != famname)
					{
						cout << "Not every male has the same name"<<endl;
						outfile << "Not every male has the same name"<<endl;
					}
				}
			}
		}
		famname="";
	}
	for(int i=0;i<genders.size();i++)
	{
		for(int j=1;j<people.size();j++)
		{
			if(genders[i][0] == people[j][0])
			{
				if(people[j][people[j].size()-1] == "M" && genders[i][1] == "WIFE")
				{
					cout << "A Male is a Wife" << endl;
					outfile << "A Male is a Wife" << endl;
				}
				if(people[j][people[j].size()-1] == "F" && genders[i][1] == "HUSB")
				{
					cout << "A Women is a Husband" << endl;
					outfile << "A Women is a Husband" << endl;
				}
			}
			
		}
	}
	int sib=0;
	for(int a=1;a<families.size();a++)
	{
		for(int b=1;b<families.size();b++)
		{
			for(int c=2;c<families[b].size();c++)
			{
				if(c>3 && families[a][2] == families[b][c])
				{
					for(int d=1;d<families.size();d++)
					{
						sib=0;
						for(int e=1;e<families[d].size();e++)
						{
							if(e>3 && families[d][e] == families[a][3])
								sib++;
							if(e>3 && families[d][e] == families[b][2])
								sib++;
							if(e>3 && families[d][e] == families[b][3])
								sib++;
						}
						if(sib>1)
						{
							cout << "An uncle or Aunt is married to a niece or nephew"<<endl;
							outfile << "An uncle or Aunt is married to a niece or nephew"<<endl;
						}
					}
				}
				if(c>3 && families[a][3] == families[b][c])
				{
					for(int d=1;d<families.size();d++)
					{
						sib=0;
						for(int e=1;e<families[d].size();e++)
						{
							if(e>3 && families[d][e] == families[a][3])
								sib++;
							if(e>3 && families[d][e] == families[b][2])
								sib++;
							if(e>3 && families[d][e] == families[b][3])
								sib++;
						}
						if(sib>1)
						{
							cout << "An uncle or Aunt is married to a niece or nephew"<<endl;
							outfile << "An uncle or Aunt is married to a niece or nephew"<<endl;
						}
					}	
				}
			}
		}
	}

	int myear;
	int age1;
	int age2;
	int temp;
	for(int a=1;a<families.size();a++)
	{
		myear=stoi(families[a][1].substr(families[a][1].length()-4));
		for(int b=1;b<people.size();b++)
		{
			if(people[b][0] == families[a][2])
				age1=stoi(people[b][2].substr(people[b][2].length()-4));
			if(people[b][0] == families[a][3])
				age2=stoi(people[b][2].substr(people[b][2].length()-4));
		}
		age1=myear-age1;
		age2=myear-age2;
		if(age1>age2) //Age 2 is bigger
		{
			temp=age1;
			age1=age2;
			age2=temp;
		}
		if(age1<0 || age2<0)
		{
			cout << "Someone was married before they were born" << endl;
			outfile << "Someone was married before they were born" << endl;
		}
		if(age1<14 || age2<14)
		{
			cout << "Someone was married before they turned 14" << endl;
			outfile << "Someone was married before they turned 14" << endl;
		}
		if(2*age1 < age2)
		{
			cout << "One person in a marraige is more than twice the age of the other";
			outfile << "One person in a marraige is more than twice the age of the other";
		}
	}
}
int main(int argc, char const *argv[])
{
	read();
	check();
	return 0;
}