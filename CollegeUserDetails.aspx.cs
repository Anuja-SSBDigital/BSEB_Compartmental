using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CollegeUserDetails : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int sddf = Convert.ToInt32(Session["CollegeId"]);

            if (Session["CollegeId"] != null)
            {

                string CollegeCode = Request.QueryString["collegeCode"].ToString();
                txtcollegeCode.Text = CollegeCode;
                LoadCollegeDetails(CollegeCode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }



        }
    }

    private void LoadCollegeDetails(string collegeCode)
    {
        try
        {
            DataTable dt = dl.GetCollegeUserDetailsByCollegeCode(collegeCode);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtcollegeName.Text = row["CollegeName"].ToString();
                txtPrincipalName.Text = row["PrincipalName"].ToString();
                txtPrincipalConNo.Text = row["PrincipalMobileNo"].ToString();
                txtEmailId.Text = row["EmailId"].ToString();
                txtUserName.Text = row["UserName"].ToString();
                hdnCollegeId.Value = row["CollegeId"].ToString();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected void btnUpdateCollegeDetailsByCollegeCode(object sender, EventArgs e)
    {
        try
        {
            string CollegeName = txtcollegeName.Text.Trim();
            int collegeId = int.Parse(hdnCollegeId.Value);
            string principalName = txtPrincipalName.Text.Trim();
            string principalMobile = txtPrincipalConNo.Text.Trim();
            string email = txtEmailId.Text.Trim();


            bool isUpdated = dl.UpdateCollegeDetails(collegeId, principalName, principalMobile, email, CollegeName);

            if (isUpdated)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('College details updated successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Update failed.');", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        try
        {
            pnlChangePassword.Visible = true;
            txtChangeUserName.Text = txtUserName.Text;
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void btnSubmitNewPassword_Click(object sender, EventArgs e)
    {
        try
        {
            string userName = txtChangeUserName.Text.Trim();

            //string newPassword = "JAMGVTJNKT";
            string newPassword = txtNewPassword.Text.Trim();
            string PlainPassword = txtNewPassword.Text.Trim();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(newPassword))
            {
                string hashedPassword = HashPasswordSHA256(newPassword);
                bool result = dl.UpdateUserPassword(userName, hashedPassword, PlainPassword);
                if (result)
                {
                    pnlChangePassword.Visible = false;
                    txtNewPassword.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Password updated successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Password update failed.');", true);
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    public string HashPasswordSHA256(string password)
    {
        try
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


}