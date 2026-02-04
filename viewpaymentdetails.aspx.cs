using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;
using System.Web.UI.HtmlControls;
using Image = iTextSharp.text.Image;
using System.Text;
using System.Threading;

public partial class viewpaymentdetails : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    public void gettrsactiondetails()
    {
        string clientTxnId = Request.QueryString["id"].ToString();
        if(clientTxnId.StartsWith("EXM"))
        {
            DataSet result = db.GetExmPaymntDetailsTxnIdwise(clientTxnId,0);
            if (result != null && result.Tables.Count > 0)
            {
                rpt_getpayemnt.DataSource = result.Tables[1];
                rpt_getpayemnt.DataBind();
            }
            else
            {
                rpt_getpayemnt.DataSource = null;
                rpt_getpayemnt.DataBind(); // Optional: clears previous data

            }
        }
        else
        {
            DataSet result = db.GetStdntPaymntDetailsTxnIdwise(clientTxnId);
            if (result != null && result.Tables.Count > 0)
            {
                rpt_getpayemnt.DataSource = result.Tables[1];
                rpt_getpayemnt.DataBind();
            }
            else
            {
                rpt_getpayemnt.DataSource = null;
                rpt_getpayemnt.DataBind(); // Optional: clears previous data

            }
        }




       
       
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            if (!IsPostBack)
            {
                gettrsactiondetails();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void rpt_getpayemnt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblTxn = (Label)e.Item.FindControl("lblTxn");
            if (lblTxn != null)
            {
                lblTxn.Text = Request.QueryString["id"];
            }
        }
    }

    protected void DwnTransaction_PDF_Click(object sender, EventArgs e)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 30f, 30f, 20f, 20f);
                PdfWriter.GetInstance(pdfDoc, ms);

                pdfDoc.Open();

                // Load and Add Logo
                //string logoPath = Server.MapPath("~/assets/img/bsebimage2.png");
                //if (File.Exists(logoPath))
                //{
                //    Image logo = Image.GetInstance(logoPath);
                //    logo.ScaleToFit(90f, 90f);
                //    logo.Alignment = Image.ALIGN_CENTER;
                //    pdfDoc.Add(logo);
                //}

                // Title
                Font companyFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK);
                Paragraph companyName = new Paragraph("Transaction Details", companyFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                pdfDoc.Add(companyName);

                // Table with Correct Column Count
                PdfPTable table = new PdfPTable(7); // Fix column count
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 10f;
                table.SetWidths(new float[] { 1f, 2.5f, 2.5f , 2.5f , 2.5f , 2.5f ,2.5f });

                // Header Styling
                Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
                BaseColor headerBgColor = new BaseColor(60, 60, 60);

                string[] headers = { "No.", "Transaction Id", "Student Name", "Father Name", "Faculty", "Board Name", "DOB" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = headerBgColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 8,
                        BorderColor = BaseColor.BLACK
                    };
                    table.AddCell(cell);
                }

                // Data Styling
                Font dataFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
                int rowIndex = 1;

                foreach (RepeaterItem item in rpt_getpayemnt.Items)
                 {
                    Label lblTxn = (Label)item.FindControl("lblTxn");
                    Label StudentFullName = (Label)item.FindControl("StudentFullName");
                    Label FatherName = (Label)item.FindControl("FatherName");
                    Label lblft = (Label)item.FindControl("FacultyName");
                    Label BoardName = (Label)item.FindControl("BoardName");
                    Label dob = (Label)item.FindControl("dob");

                    // Ensure labels exist before accessing .Text
                    string lblTxnin = lblTxn != null ? lblTxn.Text : "N/A";
                    string STname = lblTxn != null ? StudentFullName.Text : "N/A";
                    string fname = lblTxn != null ? FatherName.Text : "N/A";
                    string  faculty= lblTxn != null ? lblft.Text : "N/A";
                    string brdname = lblTxn != null ? BoardName.Text : "N/A";
                    string dobin = lblTxn != null ? dob.Text : "N/A";

                    PdfPCell[] cells = {
                    new PdfPCell(new Phrase(rowIndex.ToString(), dataFont)),
                    new PdfPCell(new Phrase(lblTxnin, dataFont)),
                    new PdfPCell(new Phrase(STname, dataFont)),
                    new PdfPCell(new Phrase(fname, dataFont)),
                    new PdfPCell(new Phrase(faculty, dataFont)),
                    new PdfPCell(new Phrase(brdname, dataFont)),
                    new PdfPCell(new Phrase(dobin, dataFont)),

                };

                    // Apply Styling
                    foreach (PdfPCell cell in cells)
                    {
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 6;
                        cell.BorderColor = BaseColor.GRAY;
                    }

                    // Alternate Row Color
                    if (rowIndex % 2 == 0)
                    {
                        foreach (PdfPCell cell in cells)
                        {
                            cell.BackgroundColor = new BaseColor(240, 240, 240); // Light Gray
                        }
                    }

                    // Add Cells to Table
                    foreach (PdfPCell cell in cells)
                    {
                        table.AddCell(cell);
                    }

                    rowIndex++;
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                // File Download
                byte[] pdfBytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TransactionReport.pdf");
                Response.Buffer = true;
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error generating PDF: " + ex.Message + "');</script>");
        }
    }
}