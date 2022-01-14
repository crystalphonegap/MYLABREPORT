using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MYLAB_Report
{
    public partial class PatientTestReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string Id = Request.QueryString["ID"].ToString();
                    GetPatientTestReport(Id);
                }

            }
            catch (Exception)
            {

            }
        }

        private void GetPatientTestReport(string Id)
        {
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            try
            {

                try
                {

                    SqlCommand sqlcmd = new SqlCommand("PRC_RP_QUR_TEST", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_PATIENTID", Id);
                 
                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ReportDocument CrystalReport = new ReportDocument();
                    try
                    {
                        CrystalReport.Load(Server.MapPath("~/Reports/PatientTestReport.rpt"));
                    }
                    catch
                    {
                        Label.Enabled = true;
                        Label.Text = "Crystal Report File Does Not Exist ";
                        return;
                    }
                    CrystalReport.SetDataSource(dt);
                    CrvTestRep.Enabled = true;
                    CrvTestRep.ReportSource = CrystalReport;
                    CrvTestRep.DataBind();
                    CrvTestRep.RefreshReport();
                    CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "TestReport-" + DateTime.Now);
                    Response.End();
                }
                catch (Exception)
                {

                }

            }
            catch (Exception ex)
            {
                Label.Enabled = true;
                Label.Text = "Invalid URL OR " + ex.Message;
                throw ex;
            }
            finally
            {
                sqlcon.Close();
            }
        }
    }
}