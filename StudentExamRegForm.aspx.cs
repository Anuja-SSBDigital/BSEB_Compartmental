using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentExamRegForm : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Session["CollegeId"] != null)
                {

                    Binddropdown();
                    BindFacultydropdown();
                    string encryptedStudentId = Request.QueryString["studentId"];
                    string StudentId = CryptoHelper.Decrypt(encryptedStudentId);
                    // Page_Load
                    hfStudentIdEncrypted.Value = CryptoHelper.Encrypt(StudentId);

                    //string registrationType = Request.QueryString["registrationType"];
                    string examTypeId = Request.QueryString["examTypeId"];
                    //if (registrationType == "Private")
                    //{
                    //    div_faculty.Visible = true;
                    //}
                    //else
                    //{
                    //    div_faculty.Visible = false;
                    //}
                    //  if (!string.IsNullOrEmpty(StudentId) && !string.IsNullOrEmpty(registrationType))
                    if (!string.IsNullOrEmpty(StudentId))
                    {
                        // string decodedStudentId = Server.UrlDecode(encodedStudentId);
                        hfStudentId.Value = StudentId;
                        LoadStudentData(StudentId, examTypeId);
                        // LoadStudentData(StudentId, registrationType, examTypeId);
                    }
                    //else
                    //{
                    //    registrationType = Request.QueryString["registrationType"];
                    //    LoadStudentData("", registrationType, examTypeId);
                    //}
                    string aadharValue = txtAadharNumber.Text.Replace("'", "\\'").Trim();

                    // Use string concatenation instead of interpolated strings
                    string script = "<script type='text/javascript'>\n" +
                                    "window.aadharValue = '" + aadharValue + "';\n" +
                                    "</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "aadharScript", script);
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
    public void Binddropdown()
    {
        try
        {
            //DataTable dtExamcat = dl.getExamCatfordropdown();
            //if (dtExamcat.Rows.Count > 0)
            //{
            //    ddlExamcat.DataSource = dtExamcat;
            //    ddlExamcat.DataTextField = "ExamTypeName";
            //    ddlExamcat.DataValueField = "Pk_ExamTypeId";
            //    ddlExamcat.DataBind();
            //    ddlExamcat.Items.Insert(0, new ListItem("Select Exam Category", "0"));
            //}
            //else
            //{
            //    ddlExamcat.Items.Clear();
            //    ddlExamcat.Items.Insert(0, new ListItem("Select Exam Category", "0"));
            //}
            DataTable dtNationality = dl.getNationalityfordropdown();
            if (dtNationality.Rows.Count > 0)
            {
                ddlNationality.DataSource = dtNationality;
                ddlNationality.DataTextField = "Nationality";
                ddlNationality.DataValueField = "Pk_NationalityId";
                ddlNationality.DataBind();
                ddlNationality.Items.Insert(0, new ListItem("Select Nationality", "0"));
            }
            else
            {
                ddlNationality.Items.Clear();
                ddlNationality.Items.Insert(0, new ListItem("Select Nationality", "0"));
            }

            DataTable dtReligion = dl.getReligionfordropdown();
            if (dtReligion.Rows.Count > 0)
            {
                ddlReligion.DataSource = dtReligion;
                ddlReligion.DataTextField = "Religion";
                ddlReligion.DataValueField = "Pk_ReligionId";
                ddlReligion.DataBind();
                ddlReligion.Items.Insert(0, new ListItem("Select Religion", "0"));
            }
            else
            {
                ddlReligion.Items.Clear();
                ddlReligion.Items.Insert(0, new ListItem("Select Religion", "0"));
            }


            DataTable dtCasteCategory = dl.getCasteCategory_Mstfordropdown();
            if (dtCasteCategory.Rows.Count > 0)
            {
                ddlCasteCategory.DataSource = dtCasteCategory;
                ddlCasteCategory.DataTextField = "CasteCategoryCode";
                ddlCasteCategory.DataValueField = "PK_CasteCategoryId";
                ddlCasteCategory.DataBind();
                ddlCasteCategory.Items.Insert(0, new ListItem("Select Caste Category", "0"));
            }
            else
            {
                ddlCasteCategory.Items.Clear();
                ddlCasteCategory.Items.Insert(0, new ListItem("Select Caste Category", "0"));
            }


            DataTable dtGender = dl.getGenderfordropdown();
            if (dtGender.Rows.Count > 0)
            {
                ddlGender.DataSource = dtGender;
                ddlGender.DataTextField = "GenderName";
                ddlGender.DataValueField = "Pk_GenderId";
                ddlGender.DataBind();
                ddlGender.Items.Insert(0, new ListItem("Select Gender", "0"));
            }
            else
            {
                ddlGender.Items.Clear();
                ddlGender.Items.Insert(0, new ListItem("Select Gender", "0"));
            }

            DataTable dtArea = dl.getArea_Mstfordropdown();
            if (dtArea.Rows.Count > 0)
            {
                ddlArea.DataSource = dtArea;
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataValueField = "Pk_AreaId";
                ddlArea.DataBind();
                ddlArea.Items.Insert(0, new ListItem("Select Area", "0"));
            }
            else
            {
                ddlArea.Items.Clear();
                ddlArea.Items.Insert(0, new ListItem("Select Area", "0"));
            }

            DataTable dtMatrixBoard = dl.getBoardMasterfordropdown();
            if (dtMatrixBoard.Rows.Count > 0)
            {
                ddlMatrixBoard.DataSource = dtMatrixBoard;
                ddlMatrixBoard.DataTextField = "BoardName";
                ddlMatrixBoard.DataValueField = "Pk_BoardId";
                ddlMatrixBoard.DataBind();
                ddlMatrixBoard.Items.Insert(0, new ListItem("Select Matrix Board", "0"));
            }
            else
            {
                ddlMatrixBoard.Items.Clear();
                ddlMatrixBoard.Items.Insert(0, new ListItem("Select Matrix Board", "0"));
            }
            DataTable dtMarital_Ms = dl.getMarital_Mstfordropdown();
            if (dtMarital_Ms.Rows.Count > 0)
            {
                ddlMaritalStatus.DataSource = dtMarital_Ms;
                ddlMaritalStatus.DataTextField = "MaritalStatus";
                ddlMaritalStatus.DataValueField = "Pk_MaritalStatusId";
                ddlMaritalStatus.DataBind();
                ddlMaritalStatus.Items.Insert(0, new ListItem("Select Marital Status", "0"));
            }
            else
            {
                ddlMaritalStatus.Items.Clear();
                ddlMaritalStatus.Items.Insert(0, new ListItem("Select Marital Status", "0"));
            }

            DataTable dtMedium_Mst = dl.getMedium_Mstfordropdown();
            if (dtMedium_Mst.Rows.Count > 0)
            {
                ddlMedium.DataSource = dtMedium_Mst;
                ddlMedium.DataTextField = "ExamMediumName";
                ddlMedium.DataValueField = "Pk_ExamMediumId";
                ddlMedium.DataBind();
                ddlMedium.Items.Insert(0, new ListItem("Select Medium", "0"));
            }
            else
            {
                ddlMedium.Items.Clear();
                ddlMedium.Items.Insert(0, new ListItem("Select Medium", "0"));
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    // private void LoadStudentData(string decodedStudentId, string registrationType,string examTypeId)
    private void LoadStudentData(string decodedStudentId, string examTypeId)
    {
        try
        {
            DataTable dt = dl.GetStudentExamRegDetails(Convert.ToInt32(decodedStudentId), Convert.ToInt32(examTypeId));
            // DataTable dt = dl.GetStudentExamRegDetails(Convert.ToInt32(decodedStudentId), registrationType,Convert.ToInt32(examTypeId));
            // DataTable dt = dl.ViewStudentRegDetails(decodedStudentId,"Regular");

            if (dt != null && dt.Rows.Count > 0)
            {

                //pnlDisplayData.Visible = true;
                // pnlUpdateData.Visible = false;

                DataRow row = dt.Rows[0];
         
                string ExamTypeId = row["ExamTypeId"].ToString().Trim();
                hnd_extype.Value = ExamTypeId;
                //if (ExamTypeId == "3")
                //{
                //    btnAddStudentReg.Visible = true;
                //   btnUpdate.Visible = false;
                //}
                

                txtStudentName.Text = row["StudentName"].ToString();
                txtmotherName.Text = row["MotherName"].ToString();
                txtfatherName.Text = row["FatherName"].ToString();
                //DateTime dob = Convert.ToDateTime(row["DOB"]);
                //txtDOB.Text = dob.ToString("dd-MM-yyyy");
                if (row["DOB"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["DOB"].ToString()))
                {
                    DateTime dob;
                    if (DateTime.TryParse(row["DOB"].ToString(), out dob))
                    {
                        // Convert to yyyy-MM-dd format required by HTML5 date input
                        txtDOB.Text = dob.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtDOB.Text = string.Empty;
                    }
                }
                else
                {
                    txtDOB.Text = string.Empty;
                }


                txtsubDivision.Text = row["SubDivisionName"].ToString();
                txtdistrict.Text = row["DistrictName"].ToString();

                txtboardRollCode.Text = row["MatricRollCode"].ToString();


                txtrollNumber.Text = row["MatricRollNumber"].ToString();
                txtpassingYear.Text = row["MatricPassingYear"].ToString();
                string DifferentlyAbled = row["DifferentlyAbled"].ToString().Trim().ToLower();
                if (DifferentlyAbled == "true")
                {
                    rdoAbledYes.Checked = true;
                }
                else if (DifferentlyAbled == "false")
                {
                    rdoAbledNo.Checked = true;
                }

                txtAadharNumber.Text = row["AadharNumber"].ToString();
                txtAdress.Text = row["StudentAddress"].ToString();
                txtMobile.Text = row["MobileNo"].ToString();
                txtEmail.Text = row["EmailId"].ToString();

                txtpincode.Text = row["PinCode"].ToString();

                txtBankACNo.Text = row["StudentBankAccountNo"].ToString();
                txtBranchName.Text = row["BankBranchName"].ToString();
                txtIFSCCode.Text = row["IFSCCode"].ToString();
                txtIdentification1.Text = row["IdentificationMark1"].ToString();
                txtIdentification2.Text = row["IdentificationMark2"].ToString();
                //txtparentno.Text = row["ParentGuardianMobileNo"].ToString();
                //txtApaarId.Text = row["ApaarId"].ToString();

                //  StudentPhotoPath.Text = row["StudentPhotoPath"].ToString();
                //   StudentSignaturePath.Text = row["StudentSignaturePath"].ToString();

                if (ddlCasteCategory.Items.FindByValue(row["Fk_CasteCategoryId"].ToString()) != null)
                {
                    ddlCasteCategory.SelectedValue = row["Fk_CasteCategoryId"].ToString();
                }
                if (ddlNationality.Items.FindByValue(row["Fk_NationalityId"].ToString()) != null)
                {
                    ddlNationality.SelectedValue = row["Fk_NationalityId"].ToString();
                }
                if (ddlGender.Items.FindByValue(row["Fk_GenderId"].ToString()) != null)
                {
                    ddlGender.SelectedValue = row["Fk_GenderId"].ToString();
                }

                if (ddlReligion.Items.FindByValue(row["Fk_ReligionId"].ToString()) != null)
                {
                    ddlReligion.SelectedValue = row["Fk_ReligionId"].ToString();
                }

                if (ddlMatrixBoard.Items.FindByValue(row["Fk_BoardId"].ToString()) != null)
                {
                    ddlMatrixBoard.SelectedValue = row["Fk_BoardId"].ToString();
                }

                if (ddlArea.Items.FindByValue(row["FK_AreaId"].ToString()) != null)
                {
                    ddlArea.SelectedValue = row["FK_AreaId"].ToString();
                }

                if (ddlMaritalStatus.Items.FindByValue(row["Fk_MaritalStatusId"].ToString()) != null)
                {
                    ddlMaritalStatus.SelectedValue = row["Fk_MaritalStatusId"].ToString();
                }

                if (ddlMedium.Items.FindByValue(row["Fk_ExamMediumId"].ToString()) != null)
                {
                    ddlMedium.SelectedValue = row["Fk_ExamMediumId"].ToString();
                }
                if (ddlFaculty.Items.FindByValue(row["FacultyId"].ToString()) != null)
                {
                    ddlFaculty.SelectedValue = row["FacultyId"].ToString();
                }

                string studentPhotoPath = row["StudentPhotoPath"].ToString();
                string studentSignaturePath = row["StudentSignaturePath"].ToString();


                //signaturePreview.ImageUrl = !string.IsNullOrEmpty(studentSignaturePath) ? studentSignaturePath : "";
               
                hfFaculty.Value = row["FacultyId"].ToString();

                txtcollegeName.Text = row["CollegeName"].ToString();
                txtcollegeCode.Text = row["CollegeCode"].ToString();
              //  hfCollegeCode.Value = txtcollegeCode.Text;
            }
          
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    //[System.Web.Services.WebMethod]
    //public static object UpdateStudent(string studentId, string facultyId, string mobile, string email, string Adress, string ddlMaritalStatus, string pincode, string BranchName, string IFSCCode, string BankACNo, string Identification1, string Identification2, string ddlMedium, string ExamTypeId, string CollegeCode)
    //{
    //    try
    //    {
    //        DBHelper dl = new DBHelper();
    //        int result = dl.UpdateStudentExamRegForm(Convert.ToInt32(studentId), mobile, email, Adress, ddlMaritalStatus, pincode, BranchName, IFSCCode, BankACNo, Identification1, Identification2, ddlMedium, Convert.ToInt32(ExamTypeId));

    //        if (result > 0)
    //        {
    //            string redirectUrl = "ExamStudentSubjectgrps.aspx?studentId=" + HttpUtility.UrlEncode(studentId) + "&FacultyId=" + HttpUtility.UrlEncode(facultyId) + "&ExamTypeId=" + HttpUtility.UrlEncode(ExamTypeId) + "&collegeCode=" + HttpUtility.UrlEncode(CollegeCode);
    //            return new { status = "success", message = "Student updated successfully.", redirectUrl = redirectUrl };
    //        }
    //        else
    //        {
    //            return new { status = "error", message = "Update failed. Please try again." };
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return new { status = "error", message = "Exception: " + ex.Message };
    //    }
    //}



    [System.Web.Services.WebMethod]
    public static object UpdateStudent(
     string studentId, string facultyId, string mobile, string email, string address,
     string maritalStatus, string pincode, string branchName, string ifscCode, string bankACNo,
     string identification1, string identification2, string medium, string examTypeId,
     string collegeCode, string AadharNumber, string aadharFileName, string aadharFileExtension, string district, string subdivision)
    {
        try
        {
            // Default file name if no file uploaded
            string adharfileFileName = string.Empty;

            // If there's an uploaded file, process it
            if (!string.IsNullOrEmpty(aadharFileName) && !string.IsNullOrEmpty(aadharFileExtension))
            {
                string photoBaseVirtual = "~/Uploads/Aadhar/Photos";
                string photoBasePath = HttpContext.Current.Server.MapPath(photoBaseVirtual);

                if (!System.IO.Directory.Exists(photoBasePath))
                    System.IO.Directory.CreateDirectory(photoBasePath);

                // Check the file extension to validate it
                if (aadharFileExtension != "jpg" && aadharFileExtension != "jpeg"&& aadharFileExtension != "png")
                    return new { status = "error", message = "Invalid file type" };

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string randomNum = new Random().Next(10000, 99999).ToString();
                adharfileFileName = randomNum + "_" + timestamp + "." + aadharFileExtension;

                // Save the file name (file content would need to be saved as well, if required)
                string fullPath = System.IO.Path.Combine(photoBasePath, adharfileFileName);
                // You will need to save the actual file content here, if required
                // e.g., you can save the file content to the server using some method.
            }

            // Call the update method in DB or other logic as required
            DBHelper dl = new DBHelper();
            int result = dl.UpdateStudentExamRegForm(Convert.ToInt32(studentId), mobile, email, address, maritalStatus, pincode, branchName, ifscCode, bankACNo, identification1, identification2, medium, Convert.ToInt32(examTypeId), AadharNumber, adharfileFileName, district, subdivision, "", "", "", "", "", "", "", "","");


            if (result > 0)
            {
                string encryptedStudentId = CryptoHelper.Encrypt(studentId);
                string redirectUrl = "ExamStudentSubjectgrps.aspx?studentId=" + HttpUtility.UrlEncode(encryptedStudentId) + "&FacultyId=" + HttpUtility.UrlEncode(facultyId) + "&ExamTypeId=" + HttpUtility.UrlEncode(examTypeId) + "&collegeCode=" + HttpUtility.UrlEncode(collegeCode);
                return new { status = "success", message = "Student updated successfully.", redirectUrl = redirectUrl };
            }
            else
            {
                return new { status = "error", message = "Update failed. Please try again." };
            }
        }
        catch (Exception ex)
        {
            return new { status = "error", message = "Exception: " + ex.Message };
        }
    }

    [System.Web.Services.WebMethod]

    public static object UpdateStudentDetails(
    string studentId,
    string facultyId,
    string mobile,
    string email,
    string address,
    string maritalStatus,
    string pincode,
    string branchName,
    string ifscCode,
    string bankACNo,
    string identification1,
    string identification2,
    string medium,
    string examTypeId,
    string collegeCode,
    string AadharNumber,
    string aadharFileName,
    string aadharFileExtension,
    string district,
    string subdivision,
    string MatrixBoard,
    string RollCode,
    string RollNumber,
    string PassingYear,
    string Gender,
    string CasteCategory,
    string Nationality,
    string Religion,
    string DOB)
    {
        try
        {
            string adharfileFileName = string.Empty;

            if (!string.IsNullOrEmpty(aadharFileName) && !string.IsNullOrEmpty(aadharFileExtension))
            {
                string photoBaseVirtual = "~/Uploads/Aadhar/Photos";
                string photoBasePath = HttpContext.Current.Server.MapPath(photoBaseVirtual);

                if (!System.IO.Directory.Exists(photoBasePath))
                    System.IO.Directory.CreateDirectory(photoBasePath);

                if (aadharFileExtension != "jpg" && aadharFileExtension != "jpeg" && aadharFileExtension != "png")
                    return new { status = "error", message = "Invalid file type" };

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string randomNum = new Random().Next(10000, 99999).ToString();
                adharfileFileName = randomNum + "_" + timestamp + "." + aadharFileExtension;
                // Save file content if needed
            }

            DBHelper dl = new DBHelper();
            int result = dl.UpdateStudentExamRegForm(Convert.ToInt32(studentId), mobile, email, address, maritalStatus, pincode, branchName, ifscCode, bankACNo, identification1, identification2, medium, Convert.ToInt32(examTypeId), AadharNumber, adharfileFileName, district, subdivision, MatrixBoard, RollCode, RollNumber, PassingYear, Gender, CasteCategory, Nationality, Religion, DOB);


            if (result > 0)
            {
                string encryptedStudentId = CryptoHelper.Encrypt(studentId);
                string redirectUrl = "ExamStudentSubjectgrps.aspx?studentId=" + HttpUtility.UrlEncode(encryptedStudentId) + "&FacultyId=" + HttpUtility.UrlEncode(facultyId) + "&ExamTypeId=" + HttpUtility.UrlEncode(examTypeId) + "&collegeCode=" + HttpUtility.UrlEncode(collegeCode);
                return new { status = "success", message = "Student updated successfully.", redirectUrl = redirectUrl };
            }
            else
            {
                return new { status = "error", message = "Update failed. Please try again." };
            }
        }
        catch (Exception ex)
        {
            return new { status = "error", message = "Exception: " + ex.Message };
        }
    }



    //[System.Web.Services.WebMethod]
    //public static object GoToNextStep(string studentId, string facultyId, string collegeCode, string examTypeId)
    //{
    //    try
    //    {
    //        //string redirectUrl = "StudentSubjectgrps.aspx?studentId=" + HttpUtility.UrlEncode(studentId) +
    //        //                     "&FacultyId=" + HttpUtility.UrlEncode(facultyId);

    //        //if (HttpContext.Current.Session["CollegeName"].ToString() == "Admin")
    //        //{
    //        //    redirectUrl += "&collegeCode=" + HttpUtility.UrlEncode(collegeCode);
    //        //}

    //        //return new { status = "success", redirectUrl = redirectUrl };
    //        bool isAdmin = false;

    //        if (HttpContext.Current.Session != null && HttpContext.Current.Session["CollegeName"] != null && HttpContext.Current.Session["CollegeName"].ToString() == "Admin")
    //        {
    //            isAdmin = true;
    //        }

    //        collegeCode = HttpContext.Current.Session != null && HttpContext.Current.Session["CollegeCode"] != null ? HttpContext.Current.Session["CollegeCode"].ToString() : "";
    //        //string examTypeId = HttpContext.Current.Request.QueryString["examTypeId"];
    //        string redirectUrl = "ExamStudentSubjectgrps.aspx?studentId=" + HttpUtility.UrlEncode(studentId) + "&FacultyId=" + HttpUtility.UrlEncode(facultyId) + "&examTypeId=" + HttpUtility.UrlEncode(examTypeId) + "&collegeCode=" + HttpUtility.UrlEncode(collegeCode); ;

    //        //if (isAdmin)
    //        //{
    //        //    redirectUrl += "&collegeCode=" + HttpUtility.UrlEncode(collegeCode);
    //        //}

    //        return new { status = "success", message = "Student updated successfully.", redirectUrl = redirectUrl };
    //    }
    //    catch (Exception ex)
    //    {
    //        return new { status = "error", message = ex.Message };
    //    }
    //}


    public void BindFacultydropdown()
    {
        try
        {
             DataTable dtfaculty = dl.getFacultyfordropdown();
             //DataTable dtfaculty = dl.getPrivateRegFacultyfordropdown();
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
        catch (Exception ex)
        {

            throw ex;
        }

    }
}