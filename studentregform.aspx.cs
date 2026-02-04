using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class studentregform : System.Web.UI.Page
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
                    string StudentId = Request.QueryString["studentId"];
                    string CategoryType = Request.QueryString["categoryType"];
                    string correctionMode = Request.QueryString["correctionMode"]; 
                    if (CategoryType == "Private")
                    {
                        div_faculty.Visible = true;
                    }
                    else
                    {
                        div_faculty.Visible = false;
                    }
                    if (!string.IsNullOrEmpty(StudentId) && !string.IsNullOrEmpty(CategoryType))
                    {
                        // string decodedStudentId = Server.UrlDecode(encodedStudentId);
                        hfStudentId.Value = StudentId;
                        LoadStudentData(StudentId, CategoryType);
                    }
                    else
                    {
                        CategoryType = Request.QueryString["categoryType"];
                        LoadStudentData("", CategoryType);
                    }
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
                ddlMatrixBoard.Items.Insert(0, new ListItem("Select Matric Board", "0"));
            }
            else
            {
                ddlMatrixBoard.Items.Clear();
                ddlMatrixBoard.Items.Insert(0, new ListItem("Select Matric Board", "0"));
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

    private void LoadStudentData(string decodedStudentId, string CategoryType)
    {
        try
        {
            DataTable dt = dl.ViewStudentRegDetails(decodedStudentId, CategoryType);

            if (dt != null && dt.Rows.Count > 0)
            {

                //pnlDisplayData.Visible = true;
                // pnlUpdateData.Visible = false;

                DataRow row = dt.Rows[0];
                string category = row["CategoryName"].ToString().Trim().ToLower();
                string correctionMode = Request.QueryString["correctionMode"];
                if(correctionMode == "Correctionmode")
                {
                    txtcollegeName.ReadOnly = true;
                    txtcollegeCode.ReadOnly = true;
                    ddlMatrixBoard.Enabled = false;
                    txtStudentName.ReadOnly = false;
                    txtmotherName.ReadOnly = false;
                    txtfatherName.ReadOnly = false;
                    txtDOB.ReadOnly = false;
                    txtdistrict.ReadOnly = false;
                    txtboardRollCode.ReadOnly = true;
                    txtsubDivision.ReadOnly = false;
                    txtrollNumber.ReadOnly = true;
                    txtpassingYear.ReadOnly = true;
                    ddlArea.Enabled = false;
                    txtAdress.ReadOnly = true;
                    txtuniqueid.ReadOnly = true;
                    txtpincode.ReadOnly = true;
                    txtBankACNo.ReadOnly = true;
                    txtBranchName.ReadOnly = true;
                   txtIFSCCode.ReadOnly = true;
                    txtIdentification1.ReadOnly = true;
                    txtIdentification2.ReadOnly = true;

                }
                if (category == "regular")
                {
                    rdoRegular.Checked = true;
                    rdoPrivate.Enabled = false;
                    rdoRegular.Enabled = true;
                    txtrollNumber.ReadOnly = true;
                    txtboardRollCode.ReadOnly = true;
                    ddlMatrixBoard.Enabled = false;
                    txtDOB.ReadOnly = true;
                }
                else if (category == "private")
                {
                    rdoPrivate.Checked = true;
                    rdoRegular.Enabled = false;
                    rdoPrivate.Enabled = true;
                    if (ddlFaculty.Items.FindByValue(row["FacultyId"].ToString()) != null)
                    {
                        ddlFaculty.SelectedValue = row["FacultyId"].ToString();                 
                    }
                }
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
                 txtuniqueid.Text = row["StudentUniqueId"].ToString();

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

                string studentPhotoPath = row["StudentPhotoPath"].ToString();
                string studentSignaturePath = row["StudentSignaturePath"].ToString();


                signaturePreview.ImageUrl = !string.IsNullOrEmpty(studentSignaturePath) ? studentSignaturePath : "";
                //}
                //else if (CategoryType == "Private")
                //{

                //    rdoPrivate.Checked = true;
                //    btnAddStudentReg.Visible = true;
                //    btnSave.Visible = false;
                //    txtmotherName.ReadOnly = false;
                //    txtStudentName.ReadOnly = false;
                //    txtfatherName.ReadOnly = false;
                //    txtDOB.ReadOnly = false;
                //    txtrollNumber.ReadOnly = false;
                //    txtpassingYear.ReadOnly = false;
                //}
                //hfFacultyName.Value = row["Faculty"].ToString();
                hfFaculty.Value = row["FacultyId"].ToString();
               
                    txtcollegeName.Text = row["College"].ToString();
                    txtcollegeCode.Text = row["CollegeCode"].ToString();
                    txtcollegeName.ReadOnly = true;
                    txtcollegeCode.ReadOnly = true;
                txtApaarId.Text = row["ApaarId"].ToString();


            }
            else if (CategoryType == "Private")
            {
                rdoRegular.Enabled = false;
                rdoPrivate.Checked = true;
                btnAddStudentReg.Visible = true;
                btnUpdate.Visible = false;
                txtmotherName.ReadOnly = false;
                txtStudentName.ReadOnly = false;
                txtfatherName.ReadOnly = false;
                txtDOB.ReadOnly = false;
                txtrollNumber.ReadOnly = false;
                txtboardRollCode.ReadOnly = false;
                ddlMatrixBoard.Enabled = true;
                txtpassingYear.ReadOnly = false;
                ddlFaculty.Enabled = true;
                string CollegeId = "";
                if (Session["CollegeName"].ToString() == "Admin")
                {
                    DataTable dt1 = dl.getcollegeidbasedonCollegecode(txtcollegeName.Text);

                    if (dt1.Rows.Count > 0)
                    {
                        CollegeId = dt1.Rows[0]["Pk_CollegeId"].ToString();

                    }

                }
                else
                {
                    CollegeId = Session["CollegeId"].ToString();
                }

                DataTable PRdt = dl.ViewPrivateStudentRegDetails(CollegeId);

                if (PRdt != null && PRdt.Rows.Count > 0)
                {
                    DataRow row = PRdt.Rows[0];
                    hfFaculty.Value = row["FacultyId"].ToString();
                    if (Session["CollegeName"].ToString() != "Admin")
                    {
                        txtcollegeName.Text = row["College"].ToString();
                        txtcollegeCode.Text = row["CollegeCode"].ToString();
                        txtcollegeName.ReadOnly = true;
                        txtcollegeCode.ReadOnly = true;
                    }
                    else
                    {
                        txtcollegeName.Text = "";
                        txtcollegeCode.Text = "";
                    }
                    //if (ddlFaculty.Items.FindByValue(row["FacultyId"].ToString()) != null)
                    //{
                    //    ddlFaculty.SelectedValue = row["FacultyId"].ToString();
                    //}
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = hfStudentId.Value;
            string FacultyId = hfFaculty.Value;
            int CollegeId = 0;
            if (Session["CollegeName"].ToString() == "Admin")
            {
                DataTable dtres = dl.getcollegeidbasedonCollegecode(txtcollegeCode.Text);

                if (dtres.Rows.Count > 0)
                {
                    CollegeId = Convert.ToInt32(dtres.Rows[0]["Pk_CollegeId"].ToString());

                }

            }
            else
            {
                CollegeId = Convert.ToInt32(Session["CollegeId"].ToString());
            }

            string name = txtStudentName.Text.Trim();
            string father = txtfatherName.Text.Trim();
            string mother = txtmotherName.Text.Trim();
            string dob = txtDOB.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string district = txtdistrict.Text.Trim();
            string subDivision = txtsubDivision.Text.Trim();
            //string collegeCode = txtcollegeCode.Text.Trim();
            string collegeName = txtcollegeName.Text.Trim();
            string boardRollCode = txtboardRollCode.Text.Trim();
            string boardRollNumber = txtrollNumber.Text.Trim();
            string passingYear = txtpassingYear.Text.Trim();
            string aadharNumber = txtAadharNumber.Text.Trim();
            string address = txtAdress.Text.Trim();
            string pincode = txtpincode.Text.Trim();
            string bankAccount = txtBankACNo.Text.Trim();
            string bankBranch = txtBranchName.Text.Trim();
            string ifscCode = txtIFSCCode.Text.Trim();
            string idMark1 = txtIdentification1.Text.Trim();
            string idMark2 = txtIdentification2.Text.Trim();

            string nationalityId = ddlNationality.SelectedValue;
            string religionId = ddlReligion.SelectedValue;
            string casteCategoryId = ddlCasteCategory.SelectedValue;
            string genderId = ddlGender.SelectedValue;
            string areaId = ddlArea.SelectedValue;
            string maritalStatusId = ddlMaritalStatus.SelectedValue;
            string mediumId = ddlMedium.SelectedValue;
            string matrixBoardId = ddlMatrixBoard.SelectedValue;
            string ApaarId = txtApaarId.Text.Trim();
            string Uniqueid = txtuniqueid.Text.Trim();

            string category = rdoRegular.Checked ? "Regular" : rdoPrivate.Checked ? "Private" : "";
            string differentlyAbled = rdoAbledYes.Checked ? "true" : "false";

            int success = dl.AddUpdateStudent(Convert.ToInt32(studentId), Convert.ToInt32(FacultyId), name.Trim().ToUpper(), father.Trim().ToUpper(), mother.Trim().ToUpper(), dob, mobile, email, district.Trim().ToUpper(), subDivision.Trim().ToUpper(),
              CollegeId, boardRollCode, boardRollNumber, passingYear, aadharNumber, address.ToUpper(), pincode,
              bankAccount, bankBranch.Trim().ToUpper(), ifscCode, idMark1.Trim().ToUpper(), idMark2.Trim().ToUpper(), Convert.ToInt32(nationalityId), Convert.ToInt32(religionId), Convert.ToInt32(casteCategoryId),
              Convert.ToInt32(genderId), Convert.ToInt32(areaId), Convert.ToInt32(maritalStatusId), Convert.ToInt32(mediumId), matrixBoardId, category, differentlyAbled, ApaarId, Uniqueid);

            //lblMessage.Text = success ? "Update successful!" : "Update failed.";

            if (success > 0)
            {
                string redirectUrl = "";

                if (Session["CollegeName"].ToString() == "Admin")
                {
                    string collegeCode = txtcollegeCode.Text;

                    redirectUrl = "StudentSubjectgrps.aspx?studentId=" + Server.UrlEncode(studentId) +
                                  "&FacultyId=" + Server.UrlEncode(FacultyId) +
                                  "&collegeCode=" + HttpUtility.UrlEncode(collegeCode);
                }
                else
                {
                    redirectUrl = "StudentSubjectgrps.aspx?studentId=" + Server.UrlEncode(studentId) +
                                  "&FacultyId=" + Server.UrlEncode(FacultyId);
                }

                string script = @"
                swal({
                    title: 'Success!',
                    text: 'Student record updated successfully.',
                    icon: 'success',
                    button: 'OK'
                }).then(function() {
                    window.location.href = '" + redirectUrl + @"';
                });";

                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSuccess", script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateFailed", "alert('Update failed. Please try again.');", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }

    public void BindFacultydropdown()
    {
        try
        {
            DataTable dtfaculty = dl.getPrivateRegFacultyfordropdown();
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

    protected void btnAddStudentReg_Click(object sender, EventArgs e)
    {
        try
        {
            string FacultyId = ddlFaculty.SelectedValue;
           
            string name = txtStudentName.Text.Trim();
            string father = txtfatherName.Text.Trim();
            string mother = txtmotherName.Text.Trim();
            string dob = txtDOB.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string district = txtdistrict.Text.Trim();
            string subDivision = txtsubDivision.Text.Trim();
            //string collegeCode = txtcollegeCode.Text.Trim();
            string collegeName = txtcollegeName.Text.Trim();
            string boardRollCode = txtboardRollCode.Text.Trim();
            string boardRollNumber = txtrollNumber.Text.Trim();
            string passingYear = txtpassingYear.Text.Trim();
            string aadharNumber = txtAadharNumber.Text.Trim();
            string address = txtAdress.Text.Trim();
            string pincode = txtpincode.Text.Trim();
            string bankAccount = txtBankACNo.Text.Trim();
            string bankBranch = txtBranchName.Text.Trim();
            string ifscCode = txtIFSCCode.Text.Trim();
            string idMark1 = txtIdentification1.Text.Trim();
            string idMark2 = txtIdentification2.Text.Trim();

            string nationalityId = ddlNationality.SelectedValue;
            string religionId = ddlReligion.SelectedValue;
            string casteCategoryId = ddlCasteCategory.SelectedValue;
            string genderId = ddlGender.SelectedValue;
            string areaId = ddlArea.SelectedValue;
            string maritalStatusId = ddlMaritalStatus.SelectedValue;
            string mediumId = ddlMedium.SelectedValue;
            string matrixBoardId = ddlMatrixBoard.SelectedValue;
            string ApaarId = txtApaarId.Text.Trim();
            string Uniqueid = txtuniqueid.Text.Trim();


            int CollegeId = 0;
            if (Session["CollegeName"].ToString() == "Admin")
            {
                DataTable dtres = dl.getcollegeidbasedonCollegecode(txtcollegeCode.Text);

                if (dtres.Rows.Count > 0)
                {
                    CollegeId = Convert.ToInt32(dtres.Rows[0]["Pk_CollegeId"].ToString());

                }

            }
            else
            {
                CollegeId = Convert.ToInt32(Session["CollegeId"].ToString());
            }

            string category = rdoRegular.Checked ? "Regular" : rdoPrivate.Checked ? "Private" : "";
            string differentlyAbled = rdoAbledYes.Checked ? "true" : "false";

            int success = dl.AddUpdateStudent(0, Convert.ToInt32(FacultyId), name.Trim().ToUpper(), father.Trim().ToUpper(), mother.Trim().ToUpper(), dob, mobile, email, district.Trim().ToUpper(), subDivision.Trim().ToUpper(),
              Convert.ToInt32(CollegeId), boardRollCode, boardRollNumber, passingYear, aadharNumber, address, pincode,
              bankAccount, bankBranch.Trim().ToUpper(), ifscCode, idMark1.Trim().ToUpper(), idMark2.Trim().ToUpper(), Convert.ToInt32(nationalityId), Convert.ToInt32(religionId), Convert.ToInt32(casteCategoryId),
              Convert.ToInt32(genderId), Convert.ToInt32(areaId), Convert.ToInt32(maritalStatusId), Convert.ToInt32(mediumId), matrixBoardId, "Private", differentlyAbled, ApaarId, Uniqueid);

            if (success > 0)
            {
                string alertScript = "alert('Student details have been added successfully. Please complete the payment before proceeding to enter additional information.'); window.location='registerPrivate.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "InsertSuccess", alertScript, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "InsertFailed", "alert('Insert failed. Please try again.');", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }

}