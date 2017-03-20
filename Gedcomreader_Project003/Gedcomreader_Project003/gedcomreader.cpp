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
	ifstream infile("gedcominput.txt");
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

void checkBirthMarraige()
{
	vector <string> words;
	vector <string> name;
	vector <string> born;
	vector <string> marr;
	vector <string> deat;
	vector <string> mpp;
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
				name.push_back(words[1]);
				last="INDI";
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
			}
			if(last =="MARR")
			{
				a = words[2]+words[3]+words[4];
				marr.push_back(a);
			}
			if(last =="DEAT")
			{
				a = words[2]+words[3]+words[4];
				deat.push_back(a);
			}
			last="";
		}
		if(words[1] == "HUSB" || words[1] == "WIFE")
		{
			mpp.push_back(words[2]);
		}
	}
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

}
int main(int argc, char const *argv[])
{
	read();
	checkBirthMarraige();
	return 0;
}