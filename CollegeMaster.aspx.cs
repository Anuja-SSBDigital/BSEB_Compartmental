using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CollegeMaster : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["CollegeId"] != null)
            {
                // ClearControls();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
    }

    protected void btnGetCollegeMasterByCollegeCode(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = txtcollegeCode.Text.Trim();
            DataTable dt = dl.GetCollegeMasterByCollegeCode(collegeCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                pnlcollegeCodeTable.Visible = true;
                pnlNoRecords.Visible = false;
                rptcollegeCode.DataSource = dt;
                rptcollegeCode.DataBind();
            }
            else
            {
                rptcollegeCode.DataSource = null;
                rptcollegeCode.DataBind();

                pnlcollegeCodeTable.Visible = false;
                pnlNoRecords.Visible = true;
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}