using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExamFormStatus : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

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
                    hfCollegeId.Value = CollegeId.ToString();
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
        int CollegeId = 0;
        if (Session["CollegeName"].ToString() == "Admin")
        {
            DataTable dt = dl.getcollegeidbasedonCollegecode(txt_CollegeName.Text);

            if (dt.Rows.Count > 0)
            {
                CollegeId = Convert.ToInt32(dt.Rows[0]["Pk_CollegeId"].ToString());

            }

        }
        else
        {
            CollegeId = Convert.ToInt32(hfCollegeId.Value);
        }


        DataTable result = dl.GetExamFormStatus(CollegeId,Convert.ToInt32(ddlFaculty.SelectedValue), ddl_status.SelectedValue);
        if (result != null && result.Rows.Count > 0)
        {
            rptStudents.DataSource = result;
            rptStudents.DataBind();
            pnlNoRecords.Visible = false;
            pnlStudentTable.Visible = true;

        }
        else
        {
            rptStudents.DataSource = null;
            rptStudents.DataBind();
            pnlStudentTable.Visible = false;
            pnlNoRecords.Visible = true;

        }
    }


}