using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CollegeReport : System.Web.UI.Page
{
    DBHelper db = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["CollegeId"] != null)
            {
                BindReport();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }

    private void BindReport()
    {
        try
        {
            // Fetch data without updating fees
            DataSet ds = db.GetStudentStats(updateFee: false);

            // Labels
            lblExamFormDownloaded.Text = ds.Tables[0].Rows[0][0].ToString();
            lblExamFormSubmitted.Text = ds.Tables[1].Rows[0][0].ToString();
            lblFeeSubmitted.Text = ds.Tables[2].Rows[0][0].ToString();

            // GridViews
            gvFeePerExam.DataSource = ds.Tables[3];
            gvFeePerExam.DataBind();

            gvFormPerExam.DataSource = ds.Tables[4];
            gvFormPerExam.DataBind();

           
        }
        catch (Exception ex)
        {
            // Error alert
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
        }



    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Call your DB method
            DataSet ds = db.GetStudentStats(updateFee: true);
            // Optional: You can check if update was successful based on your DB method
            if (ds != null && ds.Tables.Count > 0)

            {
                // Show success alert
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
           "alert('Update successful!'); window.location.href=window.location.href;", true);

            }
            else
            {
                // Show warning if no data returned
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Update completed, but no data returned.');", true);
            }
        }
        catch (Exception ex)
        {
            // Show error alert
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);

        }
    }
}
