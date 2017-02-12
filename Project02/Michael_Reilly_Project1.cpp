//@author: Michael Reilly
//SSW 555
//February 5th, 2017

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>

using namespace std;

int main()
{
	vector <string> tags;
	tags = {"INDI", "NAME", "SEX", "BIRT", "DEAT", "FAMC", "FAMS", "FAM", "MARR", "HUSB", "WIFE", "CHIL", "DIV", "DATE", "HEAD", "TRLR", "NOTE"};
	ifstream file;
	ofstream file2;
	file.open("family.ged");
	file2.open("outfamily.txt");
	string line;

	vector<string> words;
	while(getline(file, line))
	{

		words.clear();

		istringstream is(line);
		string word;

		while (is >> word)
		{
			words.push_back(word);
		}

		cout <<line<< endl;
		file2 <<line << endl;
		cout <<line[0]<<endl;
		file2 <<line[0]<<endl;

		bool validTag = false;
		//Make sure there is at least more than a level info i.e. correct file
		if(words.size() == 1)
		{
			cout <<"INVALID TAG"<<endl;
			file2 <<"INVALID TAG"<<endl;
			continue;		
		}
		for(int i = 0; i<tags.size(); i++)
		{
			if(words[1].compare(tags[i]) ==0)
			{
				validTag = true;
				cout <<tags[i]<<endl;
				file2 <<tags[i]<<endl;
				break;
			}
		}
		if (!validTag)
		{
			//If it made it this far, no third word is automatically invalid
			if(words.size() <= 2)
			{
				cout <<"INVALID TAG"<<endl;
				file2 <<"INVALID TAG"<<endl;
				continue;
			}
			if(words[2] == "FAM" || words[2] == "INDI")
			{
				cout <<words[2]<<endl;
				file2 <<words[2]<<endl;
			}
			else
			{
				cout <<"INVALID TAG"<<endl;
				file2 <<"INVALID TAG"<<endl;
			}		
		}
	}

	file.close();
	file2.close();

	return 0;
}


