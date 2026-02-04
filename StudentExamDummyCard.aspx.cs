using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentExamDummyCard : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["msg"] == "nodata")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No Data Found! No records available.');", true);

            }

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Session["CollegeId"] = null;
            BindFacultydropdown();
           
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
            string RegistrationNo = txt_RegistrationNo.Text.Trim();
            string Collegecode = txt_collegecode.Text.Trim();
            string FacultyId = ddlFaculty.SelectedValue;
            string Dob = txt_dob.Text.Trim();

            string url = "DummyExamAdmitCertificate.aspx?RegistrationNo=" + Server.UrlEncode(RegistrationNo.ToString()) +
            "&Collegecode=" + Server.UrlEncode(Collegecode) + "&faculty=" + Server.UrlEncode(FacultyId) + "&Dob=" + Server.UrlEncode(Dob) +
            "&from=StudentExamDummyCard";


            Response.Redirect(url, false);
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }
}
