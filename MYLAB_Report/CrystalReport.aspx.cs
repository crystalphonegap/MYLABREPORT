using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Web.UI.WebControls;

namespace MYLAB_Report
{
    public partial class CrystalReport : System.Web.UI.Page
    {

        ReportController _ReportController = new ReportController();
        ReportNameController _ReportNameController = new ReportNameController();
        Encryption _Encryption = new Encryption();
        protected void Page_Load(object sender, EventArgs e)
        {
            SetCrystalReport();
        }

        private void SetCrystalReport()
        {
            try
            {
                
                if (!string.IsNullOrEmpty(Request["Report"]) && !string.IsNullOrEmpty(Request["ID"]))
                {

                    string ReportName = _Encryption.DecryptAngulartxtAES(Request["Report"].ToString());
                    string ID = "1";// _Encryption.DecryptAngulartxtAES(Request["ID"].ToString());
                    //ID = ID.Remove(ID.Length-1,1);
                    var Data = _ReportController.Report(ReportName, ID);
                    ReportName = _ReportNameController.ReportName(ReportName);
                    if (Data != null && !string.IsNullOrEmpty(ReportName))
                    {
                        ReportDocument CrystalReport = new ReportDocument();
                        Label.Enabled = false;
                        Label.Text = " ";
                        try
                        {
                            CrystalReport.Load(Server.MapPath("~/Reports/" + ReportName + ".rpt"));
                        }
                        catch
                        {
                            Label.Enabled = true;
                            Label.Text = "Crystal Report File Does Not Exist ";
                            return;
                        }
                        CrystalReport.SetDataSource(Data);
                        CrystalReportViewer1.Enabled = true;
                        CrystalReportViewer1.ReportSource = CrystalReport;
                        CrystalReportViewer1.DataBind();
                        CrystalReportViewer1.RefreshReport();

                        CrystalReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "PatientTestReport");
                        Response.End();
                        

                    }
                    else
                    {
                        Label.Enabled = true;
                        Label.Text = "No Data Available";
                    }
                }
                else
                {
                    Label.Enabled = true;
                    Label.Text = "Crystal Report Working ";
                }

            }
            catch (Exception ex)
            {
                Label.Enabled = true;
                Label.Text = "Invalid URL OR " + ex.Message;
                throw ex;
            }
        }



    }
}
