using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System;

namespace MYLAB_Report
{
    public class ReportController
    {
        ReportDataTable _ReportDataTable = new ReportDataTable();

        public DataTable Report(string Report, string ID)
        {
            if (Report == "GetPatientTestReport")
            {
                return _ReportDataTable.GetPatientTestReport(ID);
            }
            else if (Report == "Other")
            {
                return _ReportDataTable.GetPatientTestReport(ID);
            }
            else
            {
                return _ReportDataTable.GetReportBySPNID(ID, Report);
            }
        }


    }
}