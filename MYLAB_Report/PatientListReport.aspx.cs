using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace MYLAB_Report
{
    public partial class PatientListReport : System.Web.UI.Page
    {

        ReportController _ReportController = new ReportController();
        ReportNameController _ReportNameController = new ReportNameController();
        Encryption _Encryption = new Encryption();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string Id = Request.QueryString["ID"].ToString();
                    string FDate = Request.QueryString["FromDate"].ToString();
                    string TDate = Request.QueryString["ToDate"].ToString();
                    GetPatientListReport(Id, FDate, TDate);
                }

            }
            catch (Exception)
            {

            }
        }

        private void GetPatientListReport(string Id, string FDate, string TDate)
        {

            ChangeDateFromat cs = new ChangeDateFromat();
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            try
            {

                try
                {

                    SqlCommand sqlcmd = new SqlCommand("PRC_RP_GETPATIENTLIST", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_ID", Id);
                    if (FDate == "" || TDate == "")
                    {
                        sqlcmd.Parameters.AddWithValue("@P_FDate", DateTime.Now);
                        sqlcmd.Parameters.AddWithValue("@P_TDate", DateTime.Now);
                    }
                    else
                    {
                        string d1 = cs.changeDateTtype(FDate);
                        string d2 = cs.changeDateTtype(TDate);

                        sqlcmd.Parameters.AddWithValue("@P_FDate", Convert.ToDateTime(FDate));
                        sqlcmd.Parameters.AddWithValue("@P_TDate", Convert.ToDateTime(TDate));
                    }
                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    ReportDocument CrystalReport = new ReportDocument();
                    Label.Enabled = false;
                    Label.Text = " ";
                    try
                    {
                        CrystalReport.Load(Server.MapPath("~/Reports/PatientListReport.rpt"));
                    }
                    catch
                    {
                        Label.Enabled = true;
                        Label.Text = "Crystal Report File Does Not Exist ";
                        return;
                    }
                    CrystalReport.SetDataSource(dt);
                    CrystalReportViewer1.Enabled = true;
                    CrystalReportViewer1.ReportSource = CrystalReport;
                    CrystalReportViewer1.DataBind();
                    CrystalReportViewer1.RefreshReport();
                    CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "PatientListReport-" + DateTime.Now);
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