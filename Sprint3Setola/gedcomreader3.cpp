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
	ifstream infile("gedcomtestfile.txt");
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
	ifstream infile("gedcominput.txt");
	ofstream outfile;
	string a;
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


	for(int i=0; i<marr.size();i++)
	{
		if(born[i] > marr[i])
			cout << name[i] << "is not correct <MARRAIGE>" << endl;
		if(stoi(born[i].substr(born[i].length()-4,born[i].length()-1))+14 < stoi(marr[i].substr(marr[i].length()-4,marr[i].length()-1)))
			cout << name[i] << "is not correct <Not old enough>" << endl;
	}
	for(int i=0; i<deat.size();i++)
	{
		if(born[i] > deat[i])
			cout << name[i] << "is not correct <DEATH>" << endl;
	}
	for(int i=0; i<marr.size();i++)
	{
		for(int j=i; j<marr.size();j++)
		{
			if(name[i] == name[j])
				cout << name[i] << " has married more than once" << endl;
		}
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
					cout << "A Male is a Wife" << endl;
				if(people[j][people[j].size()-1] == "F" && genders[i][1] == "HUSB")
					cout << "A Women is a Husband" << endl;
			}
			
		}
	}

}
int main(int argc, char const *argv[])
{
	read();
	check();
	return 0;
}