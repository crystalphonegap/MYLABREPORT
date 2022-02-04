using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MYLAB_Report
{
    public partial class PatientBillReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
                if (!IsPostBack)
                {
                    string Id = Request.QueryString["ID"].ToString();
                    string Type = Request.QueryString["Type"].ToString();
                    if(Type== "Barcode")
                    {
                        GetPatientTestReportBarcode(Id);
                    }
                    else
                    {
                        GetPatientTestReport(Id);
                    }
                    
                }

            //}
            //catch (Exception)
            //{

            //}
        }

        private void GetPatientTestReport(string Id)
        {
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            //try
            //{

            //    try
            //    {

                    SqlCommand sqlcmd = new SqlCommand("PRC_MS_GETPATIENTBILL", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_PATIENTID", Id);

                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ReportDocument CrystalReport = new ReportDocument();
                    //try
                    //{
                        CrystalReport.Load(Server.MapPath("~/Reports/PatientBillReport.rpt"));
                    //}
                    //catch(Exception e)
                    //{
                    //    Label.Enabled = true;
                    //    Label.Text = "Crystal Report File Does Not Exist ";
                    //    return;
                    //}
                    CrystalReport.SetDataSource(dt);
                    CrvPatientBill.Enabled = true;
                    CrvPatientBill.ReportSource = CrystalReport;
                    CrvPatientBill.DataBind();
                    CrvPatientBill.RefreshReport();
                    string rptname = "PatientBill-" + DateTime.Now;
                    CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, rptname);

                    Response.Write("<script>");
                    Response.Write("window.open("+ rptname+", '_newtab');");
                    Response.Write("</script>");
                    Response.End();
                //}
                //catch (Exception ex)
                //{ 

                //}

            //}
            //catch (Exception ex)
            //{
            //    Label.Enabled = true;
            //    Label.Text = "Invalid URL OR " + ex.Message;
            //    throw ex;
            //}
            //finally
            //{
            //    sqlcon.Close();
            //}
        }

        private void GetPatientTestReportBarcode(string Id)
        {
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
            try
            {

                try
                {

                    SqlCommand sqlcmd = new SqlCommand("PRC_MS_GETPATIENTBILL", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@P_PATIENTID", Id);

                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ReportDocument CrystalReport = new ReportDocument();
                    try
                    {
                        CrystalReport.Load(Server.MapPath("~/Reports/PatientBillBarcodeReport.rpt"));
                    }
                    catch
                    {
                        Label.Enabled = true;
                        Label.Text = "Crystal Report File Does Not Exist ";
                        return;
                    }
                    CrystalReport.SetDataSource(dt);
                    CrvPatientBill.Enabled = true;
                    CrvPatientBill.ReportSource = CrystalReport;
                    CrvPatientBill.DataBind();
                    CrvPatientBill.RefreshReport();
                    CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "PatientBarcodeBill-" + DateTime.Now);
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