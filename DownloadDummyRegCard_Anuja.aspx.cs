using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DownloadDummyRegCard : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
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
                    pnlNoRecords.Visible = false;
                    pnlStudentTable.Visible = false;
                    btnDownloadPDF.Visible = false;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error in Page_Load: " + ex.Message);

                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "swal({title: 'Error', text: 'An unexpected error occurred during page load.', icon: 'error', button: 'OK'});", true);
            }
        }
    }

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

    protected void btnGetStudentDummyRegData(object sender, EventArgs e)
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
            else
            {

                Response.Redirect("Login.aspx");
                return;
            }


            DataTable result = dl.GetStudentDummyRegData(CollegeId, facultyId);
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

            chkSelectAll.Checked = false;

        }
        catch (Exception ex)
        {

            Console.WriteLine("Error in btnGetStudentDummyRegData: " + ex.Message);
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

    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {
        // Re-bind the Repeater to restore its items.
        btnGetStudentDummyRegData(null, null);

        string selectedIds = hfSelectedIds.Value;
        if (string.IsNullOrEmpty(selectedIds))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one student to download PDF', icon: 'error', button: 'Retry' });", true);
            return;
        }

        string[] ids = selectedIds.Split(',');
        List<string> selectedStudentData = new List<string>();

        foreach (RepeaterItem item in rptStudents.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
                HiddenField hfCollege = (HiddenField)item.FindControl("hfCollege");
                HiddenField hfFaculty = (HiddenField)item.FindControl("hfFaculty");

                if (hfStudentID != null && hfCollege != null && hfFaculty != null)
                {
                    string studentid = hfStudentID.Value;
                    string rawCollegeId = hfCollege.Value;
                    string faculty = hfFaculty.Value;

                    if (!string.IsNullOrEmpty(studentid) && ids.Contains(studentid))
                    {
                        string CollegeId = rawCollegeId;

                        if (!string.IsNullOrEmpty(rawCollegeId) && rawCollegeId.Contains("|"))
                        {
                            var parts = rawCollegeId.Split('|');
                            if (parts.Length > 1)
                                CollegeId = parts[1].Trim();
                        }

                        if (!string.IsNullOrEmpty(CollegeId) && !string.IsNullOrEmpty(faculty))
                        {
                            string combinedData = string.Format("{0}|{1}|{2}", studentid, CollegeId, faculty);
                            selectedStudentData.Add(combinedData);
                        }
                    }
                }
            }
        }

        // Redirect after loop
        if (selectedStudentData.Count > 0)
        {
            string encodedStudentData = Server.UrlEncode(string.Join(",|", selectedStudentData));
            Response.Redirect("DummyRegCertificate.aspx?studentData=" + encodedStudentData);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one student to download PDF', icon: 'error', button: 'Retry' });", true);
        }
    }

    //protected void btnDownloadPDF_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        List<string> selectedStudentData = new List<string>();

    //        foreach (RepeaterItem item in rptStudents.Items)
    //        {
    //            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
    //            {
    //                CheckBox chk = (CheckBox)item.FindControl("chkRowSelect");
    //                HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
    //                HiddenField hfCollege = (HiddenField)item.FindControl("hfCollege");
    //                HiddenField hfFaculty = (HiddenField)item.FindControl("hfFaculty");

    //                if (chk != null && chk.Checked && hfStudentID != null && hfCollege != null && hfFaculty != null)
    //                {
    //                    string rawCollegeId = hfCollege.Value;
    //                    string CollegeId = rawCollegeId;
    //                    string faculty = hfFaculty.Value;

    //                    if (rawCollegeId.Contains("|"))
    //                    {
    //                        var parts = rawCollegeId.Split('|');
    //                        if (parts.Length > 1)
    //                            CollegeId = parts[1].Trim();
    //                    }

    //                    string combinedData = string.Format("{0}|{1}|{2}", hfStudentID.Value, CollegeId, faculty);
    //                    selectedStudentData.Add(combinedData);
    //                }
    //            }
    //        }

    //        if (selectedStudentData.Count == 0)
    //        {
    //            string script = @"
    //                swal({
    //                    title: 'Failed',
    //                    text: 'Please select at least one student to Download Dummy Reg Certificate',
    //                    icon: 'error',
    //                    button: 'Retry'
    //                });";
    //            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
    //            return;
    //        }

    //        string encodedStudentData = Server.UrlEncode(string.Join(",|", selectedStudentData));
    //        Response.Redirect("DummyRegCertificate.aspx?studentData=" + encodedStudentData);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            Button btnUploadRow = (Button)sender;
            RepeaterItem item = (RepeaterItem)btnUploadRow.NamingContainer;
            FileUpload fuRowUpload = (FileUpload)item.FindControl("fuRowUpload");
            HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");

            if (fuRowUpload != null && fuRowUpload.HasFile && hfStudentID != null)
            {
                string studentId = hfStudentID.Value;
                string uploadFolder = Server.MapPath("~/Uploads/StudentsReg/DummyRegCard/");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string extension = Path.GetExtension(fuRowUpload.FileName);
                string fileName = string.Format("{0}_{1}{2}", studentId, timestamp, extension);
                string fullPath = Path.Combine(uploadFolder, fileName);
                fuRowUpload.SaveAs(fullPath);

                string dbPath = string.Format("~/Uploads/StudentsReg/DummyRegCard/{0}", fileName);

                bool updated = dl.UpdateStudentRegCard(studentId, dbPath);

                if (updated)
                {
                    string script = @"
                        swal({
                            title: 'Success',
                            text: 'File uploaded and saved successfully.',
                            icon: 'success',
                            button: 'OK'
                        });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessUpload", script, true);

                    btnGetStudentDummyRegData(null, null);
                }
                else
                {
                    string script = @"
                        swal({
                            title: 'Database Error',
                            text: 'Failed to update database.',
                            icon: 'error',
                            button: 'OK'
                        });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "DBErrorUpload", script, true);
                }
            }
            else
            {
                string script = @"
                    swal({
                        title: 'Warning',
                        text: 'Please select a file to upload.',
                        icon: 'warning',
                        button: 'OK'
                    });";
                ScriptManager.RegisterStartupScript(this, GetType(), "NoFileSelected", script, true);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error in btnUpload_Click: " + ex.Message);
            string safeMessage = ex.Message.Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
            string script = string.Format(@"
                swal({{
                    title: 'Error',
                    text: '{0}',
                    icon: 'error',
                    button: 'OK'
                }});", safeMessage);
            ScriptManager.RegisterStartupScript(this, GetType(), "ErrorUpload", script, true);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Button btnDelete = (Button)sender;
        string studentIdToDelete = btnDelete.CommandArgument;

        try
        {
            bool success = dl.ClearStudentRegCard(studentIdToDelete);

            if (success)
            {
                string script = @"
                swal({
                    title: 'Deleted!',
                    text: 'Registration card data cleared successfully.',
                    icon: 'success',
                    button: 'OK'
                });";
                ScriptManager.RegisterStartupScript(this, GetType(), "DeleteSuccess", script, true);

                btnGetStudentDummyRegData(null, null);
            }
            else
            {
                string script = @"
                swal({
                    title: 'Error!',
                    text: 'Failed to clear registration card data. Please try again.',
                    icon: 'error',
                    button: 'OK'
                });";
                ScriptManager.RegisterStartupScript(this, GetType(), "DeleteFailed", script, true);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error deleting registration card data for Student ID " + studentIdToDelete + ": " + ex.Message);

            string script = @"
            swal({
                title: 'Error!',
                text: 'An unexpected error occurred: " + ex.Message.Replace("'", "\\'") + @"',
                icon: 'error',
                button: 'OK'
            });";
            ScriptManager.RegisterStartupScript(this, GetType(), "DeleteError", script, true);
        }
    }

    //protected void rptStudents_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {

    //        HtmlAnchor divpath = (HtmlAnchor)e.Item.FindControl("divpath");
    //        HiddenField hf_IsRegCardUploaded = (HiddenField)e.Item.FindControl("hf_IsRegCardUploaded");

    //        // Check if controls were found
    //        if (divpath != null && hf_IsRegCardUploaded != null)
    //        {
    //            if (!string.IsNullOrEmpty(hf_IsRegCardUploaded.Value) && Convert.ToBoolean(hf_IsRegCardUploaded.Value))
    //            {
    //                divpath.Visible = true;
    //            }
    //            else
    //            {
    //                divpath.Visible = false;
    //            }
    //        }


    //    }
    //}


  
}