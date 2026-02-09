using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    private static readonly ILog log = LogManager.GetLogger(typeof(login));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }

    protected void btn_login_Click(object sender, EventArgs e)
    {
        try
        {
            string message;
            bool IsAdmin = txt_username.Text.Trim().Equals("Admin@bseb.com", StringComparison.OrdinalIgnoreCase);
            DataTable dt = db.LoginUser(txt_username.Text.Trim(), txt_password.Text.Trim(), out message, IsAdmin);

            if (message == "Login successful." && dt.Rows.Count > 0)
            {
                Session["CollegeId"] = dt.Rows[0]["Id"];
                Session["UserName"] = dt.Rows[0]["UserName"];
                Session["CollegeName"] = dt.Rows[0]["CollegeName"];
                Session["CollegeCode"] = dt.Rows[0]["CollegeCode"];
                Session["DistrictName"] = dt.Rows[0]["DistrictName"];
                Session["DistrictCode"] = dt.Rows[0]["DistrictCode"];
                Session["PrincipalMobileNo"] = dt.Rows[0]["PrincipalMobileNo"];
                Session["EmailId"] = dt.Rows[0]["EmailId"];
                if (!IsAdmin)
                {
                    Session["IsProfileCompleted"] = dt.Rows[0]["IsProfileCompleted"];
                }
                else
                {
                   
                    Session["IsProfileCompleted"] = null;
                }
                // Response.Write("<script>alert('Site is under maintenance. Please try again later.');</script>");
                Response.Redirect("AppModule.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Response.Write("<script>alert('" + message + "');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('An error occurred during login: " + ex.Message.Replace("'", "") + "');</script>");
        }
    }
}