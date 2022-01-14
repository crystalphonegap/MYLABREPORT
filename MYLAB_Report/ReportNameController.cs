using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYLAB_Report
{
    public class ReportNameController
    {
        Dictionary<string, string> Name = new Dictionary<string, string>();
        public string ReportName(string Report)
        {
            SetReportName();
            try
            {
                string ReportName;
                ReportName = Name[Report];
                return ReportName;
            }
            catch
            {
                return Report;
            }
        }


        private void SetReportName()
        {
            Name.Add("GetPatientTestReport", "PatientTestReport");
            Name.Add("FORMAT10", "PatientTestReport");

        }


    }
}