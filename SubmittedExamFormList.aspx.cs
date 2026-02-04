using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SubmittedExamFormList : System.Web.UI.Page
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

        DataTable result = dl.GetSubmittedExamFormList(CollegeId, CollegeCode, StudentName, facultyId);
        if (result != null && result.Rows.Count > 0)
        {
            rptStudents.DataSource = result;
            rptStudents.DataBind();
            pnlNoRecords.Visible = false;
            pnlStudentTable.Visible = true;
            btnDownloadPDF.Visible = true;
        }
        else
        {
            rptStudents.DataSource = null;
            rptStudents.DataBind();
            pnlStudentTable.Visible = false;
            pnlNoRecords.Visible = true;
            btnDownloadPDF.Visible = false;
        }

        chkSelectAll.Checked = false;


    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {

        bool isChecked = ((CheckBox)sender).Checked;

        foreach (RepeaterItem item in rptStudents.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox rowCheckbox = (CheckBox)item.FindControl("chkRowSelect");
                if (rowCheckbox != null)
                {
                    rowCheckbox.Checked = isChecked;
                }
            }
        }
        //btnDownloadPDF.Visible = isChecked;


    }

    protected void RowCheckbox_CheckedChanged(object sender, EventArgs e)
    {

        bool anyChecked = false;

        foreach (RepeaterItem item in rptStudents.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox rowCheckbox = (CheckBox)item.FindControl("chkRowSelect");
                if (rowCheckbox != null && rowCheckbox.Checked)
                {
                    anyChecked = true;
                    break;
                }
            }
        }
        // btnDownloadPDF.Visible = anyChecked;


        chkSelectAll.Checked = !rptStudents.Items.Cast<RepeaterItem>().Any(i =>
        {
            var cb = (CheckBox)i.FindControl("chkRowSelect");
            return cb != null && !cb.Checked;
        });


    }

    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {

        List<string> selectedStudentData = new List<string>();

        foreach (RepeaterItem item in rptStudents.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkRowSelect");
                HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
                HiddenField hfCollege = (HiddenField)item.FindControl("hfCollege");
                HiddenField hfFaculty = (HiddenField)item.FindControl("hfFaculty");

                if (chk != null && chk.Checked && hfStudentID != null && hfCollege != null && hfFaculty != null)
                {
                    string rawCollegeId = hfCollege.Value;
                    string CollegeId = rawCollegeId;
                    string faculty = hfFaculty.Value;


                    if (rawCollegeId.Contains("|"))
                    {
                        var parts = rawCollegeId.Split('|');
                        if (parts.Length > 1)
                            CollegeId = parts[1].Trim();
                    }

                    string combinedData = string.Format("{0}|{1}|{2}", hfStudentID.Value, CollegeId, faculty);
                    selectedStudentData.Add(combinedData);
                }
            }
        }

        if (selectedStudentData.Count == 0)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select at least one student to download PDF.');", true);
            string script = @"
                                swal({
                                    title: 'Failed',
                                    text: 'Please select at least one student to download PDF',
                                    icon: 'error',
                                    button: 'Retry'
                                });";
            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);


            return;
        }

        string encodedStudentData = Server.UrlEncode(string.Join(",|", selectedStudentData));
        Response.Redirect("InterRegistrationForm.aspx?studentData=" + encodedStudentData);


    }


}