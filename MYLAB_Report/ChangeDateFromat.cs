using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYLAB_Report
{
    public class ChangeDateFromat
    {

        public string changeDateTtype(string FDate)
        {


            string[] FDate1 = FDate.Split('-');
            string newdate = FDate1[2]+"-"+ FDate1[0]+"-"+FDate1[1];
            return newdate;
        }
    }
}