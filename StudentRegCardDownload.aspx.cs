using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentRegCardDownload : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Session["CollegeId"] = null;
            BindFacultydropdown();
            if (Request.QueryString["alert"] == "noData")
            {
                string script = "<script type='text/javascript'>swal('No Matching Data', 'There is no matching data for the provided search criteria.', 'warning');</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showalert", script);
            }
        }
    }

    public void BindFacultydropdown()
    {
        DataTable dtfaculty = dl.getFacultyfordropdown();
        ddlFaculty.Items.Clear();
        if (dtfaculty.Rows.Count > 0)
        {
            ddlFaculty.DataSource = dtfaculty;
            ddlFaculty.DataTextField = "FacultyName";
            ddlFaculty.DataValueField = "Pk_FacultyId";
            ddlFaculty.DataBind();
        }
        ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
    }

    protected void DwnlDummyCard(object sender, EventArgs e)
    {
        try
        {
            string Studentname = txt_studentname.Text.Trim();
            string Collegecode = txt_collegecode.Text.Trim();
            string FacultyId = ddlFaculty.SelectedValue;
            string Dob = txt_dob.Text.Trim();

            string url = "DummyRegCertificate.aspx?Studentname=" + Server.UrlEncode(Studentname.ToString()) +
            "&Collegecode=" + Server.UrlEncode(Collegecode) + "&FacultyId=" + Server.UrlEncode(FacultyId) + "&Dob=" + Server.UrlEncode(Dob) +
            "&from=StudentRegCardDownload";
            Response.Redirect(url, false);
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }
}
