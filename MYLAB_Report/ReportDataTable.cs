using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MYLAB_Report
{
    public class ReportDataTable
    {


        public DataTable GetReportBySPNID(string ID,string SP)
        {
            try
            {
                string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(conString);
                SqlCommand sqlcmd = new SqlCommand(SP, sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ID", ID);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetPatientTestReport(string ID)
        {
            try
            {
                string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(conString);
                SqlCommand sqlcmd = new SqlCommand("USP_GETTESTDETAIL_Report", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@TestId", ID);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                sqlcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}