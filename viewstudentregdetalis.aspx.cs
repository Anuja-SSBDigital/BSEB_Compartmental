using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class viewstudentregdetalis : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["CollegeId"] != null)
            {
                try
                {
                    string StudentId = Request.QueryString["studentId"];
                    string CategoryType = Request.QueryString["CategoryType"];
                    string fromPage = Request.QueryString["from"];

                    if (!string.IsNullOrEmpty(StudentId))
                    {
                        hfCategoryType.Value = CategoryType;
                        LoadStudentData(StudentId, CategoryType);
                        LoadSubjects(Convert.ToInt32(StudentId));
                    }

                    if (fromPage == "Register27Page" || fromPage == "registerPrivatePage")
                    {
                        declarationContainer.Visible = false;
                        btnUpdate.Visible = false;
                        btndownloadpdf.Visible = true;
                        btnUpdateReg.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    string script = string.Format("swal('Error', 'An error occurred during page load: {0}', 'error');", ex.Message.Replace("'", "\\'"));
                    ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }

    private void LoadSubjects(int studentId)
    {
        try
        {
            DataTable dt = dl.GetStudentSubjectsListByStudentId(studentId);

            rptSubjects.DataSource = dt;
            rptSubjects.DataBind();
        }
        catch (Exception ex)
        {
            string script = string.Format("swal('Error', 'Failed to load subjects: {0}', 'error');", ex.Message.Replace("'", "\\'"));
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
        }
    }

    private void LoadStudentData(string studentId, string CategoryType)
    {
        try
        {
            DataTable dt = dl.ViewStudentRegDetails(studentId, CategoryType);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string CollegeId = "";

                lblStudentName.Text = dr["StudentName"].ToString().ToUpper();
                lblCategory.Text = dr["CategoryName"].ToString().ToUpper();
                lblFatherName.Text = dr["FatherName"].ToString().ToUpper();
                lblMotherName.Text = dr["MotherName"].ToString().ToUpper();
                lblCollege.Text = dr["College"].ToString().ToUpper();
                lblDOB.Text = Convert.ToDateTime(dr["DOB"]).ToString("dd/MM/yyyy");
                lblMatricBoardName.Text = dr["MatricBoardName"].ToString().ToUpper();
                lblRollCode.Text = dr["MatricRollCode"].ToString();
                lblRollNumber.Text = dr["MatricRollNumber"].ToString();
                lblPassingYear.Text = dr["MatricPassingYear"].ToString();
                lblGender.Text = dr["GenderName"].ToString().ToUpper();
                lblCaste.Text = dr["CasteCategoryName"].ToString().ToUpper();
                lblDifferentlyAbled.Text = Convert.ToBoolean(dr["DifferentlyAbled"]) ? "yes" : "no";
                lblNationality.Text = dr["Nationality"].ToString().ToUpper();
                lblReligion.Text = dr["Religion"].ToString().ToUpper();
                lblArea.Text = dr["AreaName"].ToString().ToUpper();
                lblMobileNo.Text = dr["MobileNo"].ToString();
                lblEmailId.Text = dr["EmailId"].ToString().ToUpper();
                lbluniqueno.Text = dr["StudentUniqueId"].ToString();
                lblAadharNo.Text = dr["AadharNumber"].ToString();
                lblAddress.Text = dr["StudentAddress"].ToString().ToUpper();
                lblPinCode.Text = dr["PinCode"].ToString();
                lblMaritalStatus.Text = dr["MaritalStatus"].ToString().ToUpper();
                lblAccountNo.Text = dr["StudentBankAccountNo"].ToString();
                lblBankBranch.Text = dr["BankBranchName"].ToString().ToUpper();
                lblIFSC.Text = dr["IFSCCode"].ToString().ToUpper();
                lblIdent1.Text = dr["IdentificationMark1"].ToString().ToUpper();
                lblIdent2.Text = dr["IdentificationMark2"].ToString().ToUpper();
                lblMedium.Text = dr["ExamMediumName"].ToString().ToUpper();

                string dirPath = "~/Uploads/StudentsReg/Signatures/";
                string dirPHOTOSPath = "~/Uploads/StudentsReg/Photos/";
                string imgepath= dr["StudentPhotoPath"].ToString();
                string imgesignpath= dr["StudentSignaturePath"].ToString();
                imgPhoto.ImageUrl = dirPHOTOSPath+ imgepath;
                imgSignature.ImageUrl = dirPath + imgesignpath;
                //imgPhoto.ImageUrl = dr["StudentPhotoPath"].ToString();
                //imgSignature.ImageUrl = dr["StudentSignaturePath"].ToString();
               // lblStuFees.Text = dr["StuFees"].ToString();
                lbl_aparid.Text = dr["ApaarId"].ToString();
                hfCollegeId.Value = dr["CollegeId"].ToString();
            }
            else
            {
                string script = "swal('No Data', 'No student data found for the given ID.', 'info');";
                ClientScript.RegisterStartupScript(this.GetType(), "NoDataAlert", script, true);
            }
        }
        catch (Exception ex)
        {
            string script = string.Format("swal('Error', 'Failed to load student data: {0}', 'error');", ex.Message.Replace("'", "\\'"));
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = Request.QueryString["studentId"];
            string categoryType = Request.QueryString["CategoryType"];
            string CollegeId = hfCollegeId.Value;

            if (string.IsNullOrEmpty(studentId))
            {
                string script = @"
                swal({
                    title: 'Missing Information',
                    text: 'Student ID missing.',
                    icon: 'warning',
                    button: 'OK'
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
                return;
            }

            // Redirect to DECLARATIONFORM.aspx with the necessary parameters
            Response.Redirect("~/DECLARATIONFORM.aspx?studentIds=" + studentId + "&CategoryType=" + categoryType + "&CollegeId=" + CollegeId);
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            string script = @"
            swal({
                title: 'Error',
                text: 'An error occurred while processing the request.',
                icon: 'error',
                button: 'OK'
            });";
            ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", script, true);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string studentId = Request.QueryString["studentId"];
        if (!string.IsNullOrEmpty(studentId))
        {
            int stuId;
            if (int.TryParse(studentId, out stuId))
            {
                dl.UpdateStudentsDownloaded(stuId);
                string printScript = "setTimeout(function() { window.print(); }, 500);";
                ClientScript.RegisterStartupScript(this.GetType(), "DelayedPrintScript", printScript, true);
            }
            else
            {
                string script = "swal('Invalid ID', 'The student ID provided is not valid.', 'error');";
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidIDAlert", script, true);
            }
        }
        else
        {
            string script = "swal('Missing ID', 'No student ID found to print.', 'warning');";
            ClientScript.RegisterStartupScript(this.GetType(), "MissingIDAlert", script, true);
        }
    }

    protected void btnUpdateRegistration_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = Request.QueryString["studentId"];
            string categoryType = Request.QueryString["CategoryType"];

            if (string.IsNullOrEmpty(studentId))
            {
                string script = @"
                swal({
                    title: 'Missing Information',
                    text: 'Student ID missing.',
                    icon: 'warning',
                    button: 'OK'
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
                return;
            }

          
            Response.Redirect("~/studentregform.aspx?studentId=" + studentId + "&categoryType=" + categoryType);
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            string script = @"
            swal({
                title: 'Error',
                text: 'An error occurred while processing the request.',
                icon: 'error',
                button: 'OK'
            });";
            ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", script, true);
        }
    }
}
