using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Web.UI.WebControls;



namespace MYLAB_Report
{
    public partial class Barcode : System.Web.UI.Page
    {
        string IPAddress = "";
        byte[] byteImage;
        ReportDocument crystalReport = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            BtnPrint();
        }



        protected void BtnPrint()
        {
            crystalReport = new ReportDocument();
            string conString = ConfigurationManager.ConnectionStrings["MYLABWEBConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(conString);
                    PrinterSettings printerName = new PrinterSettings();
                    //string defaultPrinter;
                    string barcode = "12345678";
                    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

                    string BarCodeU = Server.MapPath("~/Barcode/CODE39.TTF");
                    //string BarCodeU = Server.MapPath("~/Content/BarcodeFont/CODE39.TTF");
                    using (Bitmap bitMap = new Bitmap(580, 80))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            var myFonts = new System.Drawing.Text.PrivateFontCollection();
                            myFonts.AddFontFile(BarCodeU);
                            //var oFont = new System.Drawing.Font(myFonts.Families[0], 16);
                            var oFont = new System.Drawing.Font("CODE39.TTF", 16);//BarcodeFont In your Folder
                            PointF point = new PointF(2f, 2f);

                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                            graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byteImage = ms.ToArray();

                            Convert.ToBase64String(byteImage);
                            imgBarCode.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(byteImage);
                        }
                    }

                    SqlCommand cmd = new SqlCommand("PRC_MS_PRINTBARCODE", sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_Barcodes", barcode);//barcode);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable datatable = new DataTable();
                    da.Fill(datatable); // getting value according to imageID and fill dataset

                    DataTable dt1 = new DataTable();

                    // creating object of crystal report
                    foreach (DataRow dr in datatable.Rows)
                    {

                        dr["Barcode"] = dr["Barcode"].ToString();
                    }

                    crystalReport.Load(Server.MapPath("~/Reports/Barcode.rpt")); // path of report
                                                                                  // ReportDataSource datasource = new ReportDataSource("RDLC", datatable); 
                    crystalReport.SetDataSource(datatable);
                    //CrystalReportViewer1.ReportSource = crystalReport;

                    crystalReport.ExportToHttpResponse
                                 (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Barcode");

        }
    }
}