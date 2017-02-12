using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gedcomreader_Project003.Utilities
{
public  class Utillity
    {

        public int calculateage(DateTime birthdate,DateTime death )
        {
            int currentage;

            if (death.Equals(null))
            {
                var age = System.DateTime.Now - birthdate;

                currentage =  Convert.ToInt16(age);
              //  return currentage;
            }else
            {
                var age = death - birthdate;

                 currentage = Convert.ToInt16(age);
               // return currentage;


            }
            return currentage;
        }







    }
}
