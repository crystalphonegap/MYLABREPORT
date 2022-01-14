using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MYLAB_Report
{
    public partial class RateListReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string Id = Request.QueryString["ID"].ToString();
                    string Type = Request.QueryString["Type"].ToString();

                    GetRateListReport(Id, Type);
                }

            }
            catch (Exception)
            {

            }
        }

        private void GetRateListReport(string Id, string Type)
        {
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            try
            {

                try
                {

                    SqlCommand sqlcmd = new SqlCommand("PRC_RP_GETRATELIST", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_ID", Id);
                    sqlcmd.Parameters.AddWithValue("@P_Type", Type);
                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    ReportDocument CrystalReport = new ReportDocument();
                    Label.Enabled = false;
                    Label.Text = " ";
                    try
                    {
                        CrystalReport.Load(Server.MapPath("~/Reports/RateListReports.rpt"));
                    }
                    catch
                    {
                        Label.Enabled = true;
                        Label.Text = "Crystal Report File Does Not Exist ";
                        return;
                    }
                    CrystalReport.SetDataSource(dt);
                    CrvDoctor.Enabled = true;
                    CrvDoctor.ReportSource = CrystalReport;
                    CrvDoctor.DataBind();
                    CrvDoctor.RefreshReport();
                    CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "RateListReport-" + DateTime.Now);
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