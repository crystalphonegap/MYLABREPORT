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
                    string TESTREPORT= Request.QueryString["TESTREPORT"].ToString();
                    string ID = Request.QueryString["ID"].ToString();
                    string Patient_Name = Request.QueryString["Patient_Name"].ToString();
                    string Contact = Request.QueryString["Contact"].ToString();
                    string EmailId = Request.QueryString["EmailId"].ToString();
                    string testId = Request.QueryString["testId"].ToString();
                    string reportDate = Request.QueryString["reportDate"].ToString();
                   
                    GetPatientTestReport(TESTREPORT, ID,Patient_Name, Contact, EmailId,testId,reportDate);

                }

            }
            catch (Exception)
            {

            }
        }

        private void GetPatientTestReport(string TESTREPORT, string ID,string Patient_Name, string Contact, string EmailId, string testId, string reportDate)
        {
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            try
            {

                try
                {

                    SqlCommand sqlcmd = new SqlCommand("PRC_RP_PATIENT_TEST", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_ACTION", TESTREPORT);
                    sqlcmd.Parameters.AddWithValue("@P_ID", ID);
                    sqlcmd.Parameters.AddWithValue("@P_Patient_Name", Patient_Name);
                    sqlcmd.Parameters.AddWithValue("@P_contact_no", Contact);
                    sqlcmd.Parameters.AddWithValue("@P_EmailId", EmailId);
                    sqlcmd.Parameters.AddWithValue("@P_testId", testId.TrimEnd(','));
                    sqlcmd.Parameters.AddWithValue("@P_reportDate", reportDate);
                   
                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    
                    da.Fill(dt);
                    string format = dt.Rows[0]["TESTMST_PrintFormat"].ToString();




                    ReportDocument CrystalReport = new ReportDocument();
                    try
                    {

                         if (TESTREPORT == "rptTOG")
                        {

                            CrystalReport.Load(Server.MapPath("~/Reports/rptTOG.rpt"));

                        }
                        else if (TESTREPORT == "rptPrintAll")
                        {

                            CrystalReport.Load(Server.MapPath("~/Reports/rptPrintAll.rpt"));

                        }
                        else
                        {
                            //CrystalReport.Load(Server.MapPath("~/Reports/FORMAT01.rpt"));
                            CrystalReport.Load(Server.MapPath("~/Reports/"+format+".rpt"));
                        }


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
                catch (Exception ex)
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
            CrvTestRep.Dispose();
           // CrystalReport.Close();
        }
    }
}