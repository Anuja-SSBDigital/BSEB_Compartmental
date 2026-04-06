using System;
using System.Data;

public partial class CollegeProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["CollegeId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                int collegeId = Convert.ToInt32(Session["CollegeId"]);

                cstCollegeCode.Value = Session["CollegeCode"] != null ? Session["CollegeCode"].ToString() : "";
                cstCollegeName.Value = Session["CollegeName"] != null ? Session["CollegeName"].ToString() : "";
                cstDistrictName.Value = Session["DistrictName"] != null ? Session["DistrictName"].ToString() : "";

                DBHelper db = new DBHelper();
                DataTable dt = db.GetCollegeProfile(collegeId);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    cstUDISECode.Value = row["UDISECode"].ToString();
                    cstPrincipalName.Value = row["PrincipalName"].ToString();
                    cstPrincipalMobile.Value = row["PrincipalMobile"].ToString();
                    cstPrincipalEmail.Value = row["PrincipalEmail"].ToString();
                    cstSubDivision.Value = row["SubDivisionName"].ToString();
                    cstBlockName.Value = row["BlockName"].ToString();
                    cstPinCode.Value = row["PinCode"].ToString();
                    cstFullAddress.Value = row["FullAddress"].ToString();
                }

                bool isAdmin = Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin";

                cstCollegeCode.Disabled = !isAdmin;
                cstCollegeName.Disabled = !isAdmin;
                cstDistrictName.Disabled = !isAdmin;
            }
        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message.Replace("'", "").Replace("\n", " ");

            string errorScript = @"
                Swal.fire({
                    icon: 'error',
                    title: 'Error!',
                    text: '" + safeMessage + @"'
                     });
                     ";

            ClientScript.RegisterStartupScript(this.GetType(), "alertError", errorScript, true);
        }
    }
    // ── Called by the hidden button that JS triggers after validation passes ──
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["CollegeId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int collegeId = Convert.ToInt32(Session["CollegeId"]);
            DBHelper db = new DBHelper();

            db.SaveCollegeProfile(collegeId, cstUDISECode.Value.Trim(),cstPrincipalName.Value.Trim(), cstPrincipalMobile.Value.Trim(),cstPrincipalEmail.Value.Trim(),cstSubDivision.Value.Trim(),cstBlockName.Value.Trim(),cstFullAddress.Value.Trim(), cstPinCode.Value.Trim());

            string script = @"
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: 'Profile updated successfully.',
                confirmButtonColor: '#4f6ef7'
            }).then(() => {
                window.location = 'CollegeProfile.aspx';
            });
        ";

            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
            //Response.Redirect("CollegeProfile.aspx?saved=1", false);
            //Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message.Replace("'", "").Replace("\n", " ");

            string errorScript = @"
        Swal.fire({
            icon: 'error',
            title: 'Error!',
                text: '" + safeMessage + @"'
             });
         ";

            ClientScript.RegisterStartupScript(this.GetType(), "alertError", errorScript, true);
        }
    }
}
