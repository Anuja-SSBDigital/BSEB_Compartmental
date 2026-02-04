using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewExamDetalis : System.Web.UI.Page
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
                    string encryptedStudentId = Request.QueryString["studentId"];
                    string StudentId = CryptoHelper.Decrypt(encryptedStudentId);
                    //string StudentId = Request.QueryString["studentId"];
                    //string CategoryType = Request.QueryString["CategoryType"];
                    //string RegistrationType = Request.QueryString["RegistrationType"];
                    string ExamTypeId = Request.QueryString["ExamTypeId"];
                    string fromPage = Request.QueryString["from"];
                    if (!string.IsNullOrEmpty(StudentId))
                    {
                        if (!string.IsNullOrEmpty(ExamTypeId))
                        {
                            //hfCategoryType.Value = CategoryType;
                            LoadStudentExamData(Convert.ToInt32(StudentId), Convert.ToInt32(ExamTypeId));
                            LoadSubjects(Convert.ToInt32(StudentId),Convert.ToInt32(ExamTypeId));
                        }
                      
                    }
                    if (fromPage == "ExamForm")
                    {
                        declarationContainer.Visible = false;
                        btnUpdate.Visible = false;
                        btndownloadpdf.Visible = true;
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

    private void LoadSubjects(int studentId,int ExamTypeId)
    {
        try
        {
            DataTable dt = dl.ExamGetStudentSubjectsListByStudentId(studentId, ExamTypeId);
            //DataTable dt = dl.GetStudentSubjectsListByStudentId(studentId);

            if (dt != null && dt.Rows.Count > 0)
            {
                rptSubjects.DataSource = dt;
                rptSubjects.DataBind();
            }
            else
            {
                rptSubjects.DataSource = null;
                rptSubjects.DataBind();
            }
        }
        catch (Exception ex)
        {
            
            string script = string.Format("swal('Error', 'Failed to load subjects: {0}', 'error');", ex.Message.Replace("'", "\\'"));
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
        }
    }

    
    //Exam Form 

   
    private void LoadStudentExamData(int studentId, int examTypeId)
    {
        try
        {
            DataTable dt = dl.GetStudentExamRegDetails(Convert.ToInt32(studentId), Convert.ToInt32(examTypeId));

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lblStudentName.Text = dr["StudentName"].ToString().ToUpper();
                lblExamTypeName.Text = dr["ExamTypeName"].ToString().ToUpper();
                //lblCategory.Text = dr["CategoryName"].ToString().ToUpper();
                lblFatherName.Text = dr["FatherName"].ToString().ToUpper();
                lblMotherName.Text = dr["MotherName"].ToString().ToUpper();
                lblCollege.Text = dr["College"].ToString().ToUpper();
                lblDOB.Text = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]).ToString("dd/MM/yyyy") : "";

                lblMatricBoardName.Text = dr["MatricBoardName"].ToString().ToUpper();
                lblRollCode.Text = dr["MatricRollCode"].ToString().ToUpper();
                lblRollNumber.Text = dr["MatricRollNumber"].ToString().ToUpper();
                lblPassingYear.Text = dr["MatricPassingYear"].ToString().ToUpper();
                lblGender.Text = dr["GenderName"].ToString().ToUpper();
                lblCaste.Text = dr["CasteCategoryCode"].ToString().ToUpper();
                lblDifferentlyAbled.Text = dr["DifferentlyAbled"] != DBNull.Value && Convert.ToBoolean(dr["DifferentlyAbled"]) ? "YES" : "NO";

                //lblDisabledNote.Text = "";
                lblNationality.Text = dr["Nationality"].ToString().ToUpper();
                lblReligion.Text = dr["Religion"].ToString().ToUpper();
                lblArea.Text = dr["AreaName"].ToString().ToUpper();
                lblMobileNo.Text = dr["MobileNo"].ToString();
                lblEmailId.Text = dr["EmailId"].ToString().ToUpper();
                //lblParentMobileNo.Text = dr["ParentGuardianMobileNo"].ToString();
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

                //imgPhoto.ImageUrl = dr["StudentPhotoPath"].ToString();
                //imgSignature.ImageUrl = dr["StudentSignaturePath"].ToString();
                string dirPath = "~/Uploads/StudentsReg/Signatures/";
                string dirPHOTOSPath = "~/Uploads/StudentsReg/Photos/";
                string imgepath = dr["StudentPhotoPath"].ToString();
                string imgesignpath = dr["StudentSignaturePath"].ToString();
                imgPhoto.ImageUrl = dirPHOTOSPath + imgepath;
                imgSignature.ImageUrl = dirPath + imgesignpath;
                //lblimgPhoto.Text = dr["StudentName"].ToString();
                //lblimgSignature.Text = dr["StudentName"].ToString();
                // lblStuFees.Text = dr["StuFees"].ToString();
                decimal baseFee = dr["BaseFee"] != DBNull.Value ? Convert.ToDecimal(dr["BaseFee"]) : 0;
                decimal concessionFee = dr["ConcessionFee"] != DBNull.Value ? Convert.ToDecimal(dr["ConcessionFee"]) : 0;

                // Use concessionFee if it's not zero, otherwise use baseFee
                decimal feeToDisplay = concessionFee > 0 ? concessionFee : baseFee;

                // Display the fee in currency format
               // lblStuFees.Text = feeToDisplay.ToString();
                //lbl_aparid.Text = dr["ApaarId"].ToString();
                //lblStuFees.Text = feeToDisplay.ToString("C");
            }
            else
            {
                string script = "swal('No Data', 'No student data found for the given ID.', 'info');";
                ClientScript.RegisterStartupScript(this.GetType(), "NoDataAlert", script, true);
            }
        }
        catch (Exception ex)
        {
            // Corrected line:
            string script = string.Format("swal('Error', 'Failed to load student data: {0}', 'error');", ex.Message.Replace("'", "\\'"));
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            //string studentId = Request.QueryString["studentId"];
            string encryptedStudentId = Request.QueryString["studentId"];
            string studentId = CryptoHelper.Decrypt(encryptedStudentId);
            string CategoryType = Request.QueryString["CategoryType"];
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
            bool updated = dl.UpdateStudentExamForm(studentId);
            if (updated)
            {
              //  string categoryType = hfCategoryType.Value;
                string redirectUrl = "ExamForm.aspx";

                string script = @"
                    swal({
                        title: 'Success!',
                        text: 'Exam Form Submitted Successfully.',
                        icon: 'success',
                        button: 'OK'
                    }).then(function() {
                        window.location.href = '" + redirectUrl + @"';
                    });";
                ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script, true);
            }
            else
            {
                string script = @"
                    swal({
                        title: 'Failed!',
                        text: 'Update failed.',
                        icon: 'error',
                        button: 'OK'
                    });";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
            }
        }
        catch (Exception ex)
        {
            // Corrected line:
            string script = string.Format("swal('Error', 'An error occurred during update: {0}', 'error');", ex.Message.Replace("'", "\\'"));
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
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
                dl.UpdateStudentExamForm(studentId);

                string printScript = "setTimeout(function() { window.print(); }, 500);";
                ClientScript.RegisterStartupScript(this.GetType(), "DelayedPrintScript", printScript, true);
            }
            else
            {
                // Corrected line:
                string script = "swal('Invalid ID', 'The student ID provided is not valid.', 'error');";
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidIDAlert", script, true);
            }
        }
        else
        {
            // Corrected line:
            string script = "swal('Missing ID', 'No student ID found to print.', 'warning');";
            ClientScript.RegisterStartupScript(this.GetType(), "MissingIDAlert", script, true);
        }
    }
}