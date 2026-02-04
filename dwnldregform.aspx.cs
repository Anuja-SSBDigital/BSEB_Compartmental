using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dwnldregform : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

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
    }

    public void BindFacultydropdown()
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

    protected void btnGetStudentData(object sender, EventArgs e)
    {

        string facultyId = ddlFaculty.SelectedValue;
        string CollegeNameAndCode = txt_CollegeName.Text.Trim();
        string CollegeCode = "";
        string CollegeId = "";
        if (Session["CollegeName"].ToString() == "Admin")
        {
            CollegeCode = txt_CollegeName.Text;
            CollegeId = "";
        }
        else
        {
            CollegeCode = "";
            CollegeId = Session["CollegeId"].ToString();
        }

        string StudentName = txtStudentName.Text.Trim();

        DataTable result = dl.getStudentData(CollegeId, CollegeCode, StudentName, facultyId, "");
        if (result != null && result.Rows.Count > 0)
        {
            rptStudents.DataSource = result;
            rptStudents.DataBind();
            pnlNoRecords.Visible = false;
            pnlStudentTable.Visible = true;
            btnDownloadPDF.Visible = true;
            lblCollege.Text = result.Rows[0]["College"].ToString();
            SpSearchresult.Visible = true;
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
            SpSearchresult.Visible = false;
            pnlPager.Visible = false;
            searchInputDIV.Visible = false;
        }

        chkSelectAll.Checked = false;


    }
    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {
        // Re-bind the Repeater to restore its items.
        btnGetStudentData(null, null);

        string selectedIds = hfSelectedIds.Value;
        if (string.IsNullOrEmpty(selectedIds))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one student to download PDF', icon: 'error', button: 'Retry' });", true);
            return;
        }

        string[] ids = selectedIds.Split(',');
        if (ids.Length > 25)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Limit Exceeded', text: 'You can select a maximum of 25 students at a time.', icon: 'warning', button: 'OK' });", true);
            return;
        }

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
            Response.Redirect("InterRegistrationForm.aspx?studentData=" + encodedStudentData);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one student to download PDF', icon: 'error', button: 'Retry' });", true);
        }
    }


}
