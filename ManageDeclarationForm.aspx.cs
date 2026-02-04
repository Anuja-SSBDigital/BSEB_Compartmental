using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ManageDeclarationForm : System.Web.UI.Page
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
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine("Error in BindFacultydropdown: " + ex.Message);
            throw ex;
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
            DataTable result = dl.GetDeclarationFormData(CollegeId, facultyId, ddl_category.SelectedValue);
            if (result != null && result.Rows.Count > 0)
            {
                rptStudents.DataSource = result;
                rptStudents.DataBind();
                pnlNoRecords.Visible = false;
                pnlStudentTable.Visible = true;
                btnDownloadPDF.Visible = true;
                //SpSearchresult.Visible = true;
                pnlPager.Visible = true;
                searchInputDIV.Visible = true;

            }
            else
            {
                rptStudents.DataSource = null;
                rptStudents.DataBind();
                pnlStudentTable.Visible = false;
                pnlNoRecords.Visible = true;
                btnDownloadPDF.Visible = false;
                //SpSearchresult.Visible = false;
                pnlPager.Visible = false;
                searchInputDIV.Visible = false;

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

    protected void rptStudents_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            string studentId = e.CommandArgument.ToString();
            HiddenField hfCategory = (HiddenField)e.Item.FindControl("hfCategory");
            string rawCategory = hfCategory != null ? hfCategory.Value : "";
            string categoryType = rawCategory;
            string collegeId = hfCollegeId.Value;

            if (!string.IsNullOrEmpty(rawCategory) && rawCategory.Contains("|"))
            {
                var parts = rawCategory.Split('|');
                if (parts.Length > 1)
                    categoryType = parts[1].Trim();
            }

            switch (e.CommandName.ToLower())
            {
                case "btnupload":
                    FileUpload fuRowUpload = (FileUpload)e.Item.FindControl("fuRowUpload");
                    LinkButton btnUploadRow = (LinkButton)e.Item.FindControl("btnUploadRow");
                    LinkButton btn_Delete = (LinkButton)e.Item.FindControl("btn_Delete");
                    LinkButton btn_correction = (LinkButton)e.Item.FindControl("btn_correction");
                    Label lblfilesuccess1 = (Label)e.Item.FindControl("lblfilesuccess");

                    if (fuRowUpload != null && fuRowUpload.HasFile)
                    {
                        string uploadFolder = Server.MapPath("~/Uploads/StudentsReg/DeclarationForm/");
                        if (!Directory.Exists(uploadFolder))
                            Directory.CreateDirectory(uploadFolder);

                        // Generate random 5-digit number
                        Random rnd = new Random();
                        string uniqueNum = rnd.Next(10000, 99999).ToString();

                        // Timestamp
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                        // File extension
                        string extension = Path.GetExtension(fuRowUpload.FileName);

                        // Create file name (only file name for DB)
                        string fileName = string.Format("{0}_{1}{2}", uniqueNum, timestamp, extension);

                        // Full physical path to save
                        string fullPath = Path.Combine(uploadFolder, fileName);

                        // Save file
                        fuRowUpload.SaveAs(fullPath);

                        // DB path (only file name, not full path)
                        string dbFileName = fileName;

                        // Save into DB
                        bool updated = dl.UploadDeclarationForm(studentId, dbFileName);

                        if (updated)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "UploadSuccess",
                                "swal({ title: 'Success', text: 'File uploaded successfully!', icon: 'success', button: 'OK' });", true);
                            fuRowUpload.Visible = false;
                            btnUploadRow.Visible = false;
                            btn_correction.Visible = false;
                            lblfilesuccess1.Visible = true;
                            btn_Delete.Visible = false;
                            if (Session["CollegeName"].ToString() == "Admin")
                            {
                                btn_Delete.Visible = true;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "UploadFail",
                                "swal({ title: 'Error', text: 'Database update failed!', icon: 'error', button: 'OK' });", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoFile",
                            "swal({ title: 'Warning', text: 'Please select a file to upload.', icon: 'warning', button: 'OK' });", true);
                    }
                    break;



                case "btnedit":
                    string editUrl = string.Format(
                        "studentregform.aspx?studentId={0}&categoryType={1}",
                        Server.UrlEncode(studentId),
                        Server.UrlEncode(categoryType)
                    );
                    Response.Redirect(editUrl, false);
                    break;

                case "btndownloadpdf":
                    string downloadUrl = string.Format(
                        "DECLARATIONFORM.aspx?studentIds={0}&categoryType={1}&fromPage={2}&CollegeId={3}",
                        Server.UrlEncode(studentId),
                        Server.UrlEncode(categoryType),
                        "ManageDeclarationForm",
            Server.UrlEncode(collegeId)
                    );
                    Response.Redirect(downloadUrl, false);
                    break;

                case "btndelete":
                    LinkButton btnDelete = (LinkButton)e.Item.FindControl("btn_Delete");
                    FileUpload fuUpload = (FileUpload)e.Item.FindControl("fuRowUpload");
                    LinkButton btnUpload = (LinkButton)e.Item.FindControl("btnUploadRow");
                    LinkButton btn_corrections = (LinkButton)e.Item.FindControl("btn_correction");
                    Label lblfilesuccess = (Label)e.Item.FindControl("lblfilesuccess");
                    bool removed = dl.DeleteDeclarationForm(studentId,""); // Implement this in DAL

                    if (removed)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteSuccess",
                            "swal({ title: 'Deleted', text: 'File deleted successfully!', icon: 'success', button: 'OK' });", true);

                        // Reset UI
                        fuUpload.Visible = true;
                        btnUpload.Visible = true;
                        btnDelete.Visible = false;
                        btn_corrections.Visible = true;
                        lblfilesuccess.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteFail",
                            "swal({ title: 'Error', text: 'Unable to delete file!', icon: 'error', button: 'OK' });", true);
                    }
                    break;

            }
        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message.Replace("'", "\\'");
            string script = string.Format(
                "swal({{ title: 'Error', text: '{0}', icon: 'error', button: 'OK' }});",
                safeMessage
            );
            ScriptManager.RegisterStartupScript(this, GetType(), "Error", script, true);
        }
    }



    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {
        // Re-bind to ensure fresh data
        btnviewrecord_Click(null, null);

        string selectedIds = hfSelectedIds.Value;
        if (string.IsNullOrEmpty(selectedIds))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one student to download PDF', icon: 'error', button: 'Retry' });", true);
            return;
        }

        // College + Faculty + Category
        string facultyId = ddlFaculty.SelectedValue;
        string categoryType = ddl_category.SelectedValue;

        // Build redirect URL with multiple IDs
        string redirectUrl = string.Format(
            "~/DECLARATIONFORM.aspx?studentIds={0}&facultyId={1}&categoryType={2}&fromPage={3}&CollegeId={4}",
            Server.UrlEncode(selectedIds),     // Example: "123,456,789"
            Server.UrlEncode(facultyId),
            Server.UrlEncode(categoryType),
            "ManageDeclarationForm",
            Server.UrlEncode(hfCollegeId.Value)
        );

        Response.Redirect(redirectUrl, false);
    }


}