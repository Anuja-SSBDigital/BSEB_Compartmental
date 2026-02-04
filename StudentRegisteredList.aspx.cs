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
using ListItem = System.Web.UI.WebControls.ListItem;


public partial class StudentRegisteredList : System.Web.UI.Page
{

    DBHelper dl = new DBHelper();

    public void BindFacultydropdown()
    {
        try
        {
            DataTable dtfaculty = dl.getFacultyfordropdown();
            if (dtfaculty.Rows.Count > 0)
            {
                ddlFaculty.DataSource = dtfaculty;
                ddlFaculty.DataTextField = "FacultyName";
                ddlFaculty.DataValueField = "Pk_FacultyId";
                ddlFaculty.DataBind();
                ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
            }
            else
            {
                ddlFaculty.Items.Clear();
                ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
            }

            DataTable dtExamcat = dl.getExamCatfordropdown();
            var filtered = dtExamcat.AsEnumerable().Where(row => row.Field<int>("Pk_ExamTypeId") != 5);

            if (filtered.Any())
            {
                ddlExamcat.DataSource = filtered.CopyToDataTable();
                ddlExamcat.DataTextField = "ExamTypeName";
                ddlExamcat.DataValueField = "Pk_ExamTypeId";
                ddlExamcat.DataBind();
                ddlExamcat.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Exam Category", "0"));
            }
            else
            {
                ddlExamcat.Items.Clear();
            }

        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "DropdownError", @"
        swal({
            title: 'Error',
            text: 'An error occurred while binding dropdowns: " + safeMessage + @"',
            icon: 'error',
            button: 'Close'
        });
    ", true);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["CollegeId"] != null)
            {
                if (Session["CollegeName"].ToString() == "Admin")
                {
                    txt_CollegeName.Text = "";
                }
                else
                {
                    txt_CollegeName.Text = Session["CollegeCode"].ToString() + " | " + Session["CollegeName"].ToString();
                    txt_CollegeName.ReadOnly = true;
                    string CollegeId = Session["CollegeId"].ToString();
                }
                BindFacultydropdown();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }

    protected void btnviewrecord_Click(object sender, EventArgs e)
    {
        try
        {
            string facultyId = ddlFaculty.SelectedValue;
            string CollegeId = "";
            if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
            {
                DataTable dt = dl.getcollegeidbasedonCollegecode(txt_CollegeName.Text);

                if (dt.Rows.Count > 0)
                {
                    CollegeId = dt.Rows[0]["Pk_CollegeId"].ToString();
                }
            }
            else if (Session["CollegeId"] != null)
            {
                CollegeId = Session["CollegeId"].ToString();
            }
            hfCollegeId.Value = CollegeId;
            DataTable result = dl.GetStudentExaminationListData(Convert.ToInt32(CollegeId), Convert.ToInt32(facultyId), ddlExamcat.SelectedValue);
            bool hasRecords = result != null && result.Rows.Count > 0;
            if (result != null && result.Rows.Count > 0)
            {

                pnlNoRecords.Visible = !hasRecords;
                pnlStudentTable.Visible = hasRecords;
                rptStudentList.DataSource = hasRecords ? result : null;
                rptStudentList.DataBind();
                pnlNoRecords.Visible = false;

            }
            else
            {

                pnlStudentTable.Visible = false;
                pnlNoRecords.Visible = true;




            }



        }
        catch (Exception ex)
        {

            Console.WriteLine("Error in btnviewrecord_Click: " + ex.Message);
            string safeMessage = ex.Message.Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
            string script = string.Format(@"
                swal({{
                    title: 'Error',
                    text: '{0}',
                    icon: 'error',
                    button: 'OK'
                }});", safeMessage);
            ScriptManager.RegisterStartupScript(this, GetType(), "ErrorGetData", script, true);
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
                Paragraph companyName = new Paragraph("Registered Student List", companyFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                pdfDoc.Add(companyName);

                // Table with Correct Column Count
                PdfPTable table = new PdfPTable(6); // Fix column count
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 10f;
                table.SetWidths(new float[] { 1f, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f });

                // Header Styling
                Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
                BaseColor headerBgColor = new BaseColor(60, 60, 60);

                string[] headers = { "No.", "+2 School/College Code", "Student Name", "Father Name", "Mother Name", "Year Of Passing" };
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

                foreach (RepeaterItem item in rptStudentList.Items)
                {
                    Label CollegeCode = (Label)item.FindControl("lblCollegeCode");
                    Label StudentFullName = (Label)item.FindControl("lblStudentName");
                    Label FatherName = (Label)item.FindControl("lblFatherName");
                    Label MotherName = (Label)item.FindControl("lblMotherName");
                    Label YearOfPassing = (Label)item.FindControl("lblYearOfPassing");


                    // Ensure labels exist before accessing .Text
                    string lblTxnin = CollegeCode != null ? CollegeCode.Text : "N/A";
                    string STname = StudentFullName != null ? StudentFullName.Text : "N/A";
                    string fname = FatherName != null ? FatherName.Text : "N/A";
                    string faculty = MotherName != null ? MotherName.Text : "N/A";
                    string brdname = YearOfPassing != null ? YearOfPassing.Text : "N/A";

                    PdfPCell[] cells = {
                    new PdfPCell(new Phrase(rowIndex.ToString(), dataFont)),
                    new PdfPCell(new Phrase(lblTxnin, dataFont)),
                    new PdfPCell(new Phrase(STname, dataFont)),
                    new PdfPCell(new Phrase(fname, dataFont)),
                    new PdfPCell(new Phrase(faculty, dataFont)),
                    new PdfPCell(new Phrase(brdname, dataFont)),


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
                Response.AddHeader("content-disposition", "attachment;filename=RegisteredStudentList.pdf");
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