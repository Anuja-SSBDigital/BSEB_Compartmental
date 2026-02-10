using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CompExaminationForm : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CompExaminationForm));
    DBHelper dl = new DBHelper();
    protected DataTable mergedTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        log.Info("Page_Load started.");

        if (!IsPostBack)
        {
            try
            {
                string encodedStudentData = Request.QueryString["studentData"];
                if (!string.IsNullOrEmpty(encodedStudentData))
                {
                    string decodedStudentData = Server.UrlDecode(encodedStudentData);
                    log.Debug("Decoded student data: " + decodedStudentData);

                    List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    mergedTable = CreateStudentDataTable();
                    string FacultyId = string.Empty;
                    string CollegeId = string.Empty;
                    string ExamTypeId = string.Empty;
                    string CollegeCode = string.Empty;

                    foreach (string studentEntry in selectedStudents)
                    {
                        string[] parts = studentEntry.Split('|');
                        if (parts.Length == 5)
                        {
                            string studentIdStr = parts[0];
                            CollegeId = parts[1];
                            FacultyId = parts[2];
                            ExamTypeId = parts[3];
                            CollegeCode = parts[4];
                            Session["CollegeCode"] = CollegeCode.ToString();
                            int studentId;
                            if (int.TryParse(studentIdStr, out studentId))
                            {
                                // Replaced interpolated string with string.Format
                                log.Info(string.Format("Fetching data for StudentID: {0}, College: {1}, Faculty: {2}, ExamTypeId:{3}", studentId, CollegeId, FacultyId, ExamTypeId));
                                DataTable dt = dl.GetDwnldExaminationFormData(Convert.ToInt32(studentId), Convert.ToInt32(CollegeId), Convert.ToInt32(FacultyId), Convert.ToInt32(ExamTypeId));
                                Session["StudentId"] = studentId;
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow sourceRow in dt.Rows)
                                    {
                                        DataRow newRow = mergedTable.NewRow();
                                        //newRow["StudentPhotoPath"] = sourceRow["StudentPhotoPath"];
                                        //newRow["StudentSignaturePath"] = sourceRow["StudentSignaturePath"];
                                        newRow["FacultyName"] = sourceRow["FacultyName"];
                                        newRow["OFSSCAFNo"] = sourceRow["OFSSCAFNo"];
                                        newRow["CategoryName"] = sourceRow["CategoryName"];
                                        newRow["CollegeCode"] = sourceRow["CollegeCode"];
                                        newRow["CollegeName"] = sourceRow["CollegeName"];
                                        newRow["DistrictName"] = sourceRow["DistrictName"];
                                        newRow["StudentName"] = sourceRow["StudentName"];
                                        newRow["MotherName"] = sourceRow["MotherName"];
                                        newRow["FatherName"] = sourceRow["FatherName"];
                                        newRow["DOB"] = sourceRow["DOB"] != DBNull.Value && sourceRow["DOB"] != null ? ParseDateOfBirth(sourceRow["DOB"]) : "";
                                        newRow["MatricBoardName"] = sourceRow["MatricBoardName"];
                                        newRow["MatricRollCode"] = sourceRow["MatricRollCode"];
                                        newRow["MatricRollNumber"] = sourceRow["MatricRollNumber"];
                                        newRow["MatricPassingYear"] = sourceRow["MatricPassingYear"];
                                        newRow["Gender"] = sourceRow["Gender"];
                                        newRow["CasteCategory"] = sourceRow["CasteCategoryCode"];
                                        newRow["Nationality"] = sourceRow["Nationality"];
                                        newRow["Religion"] = sourceRow["Religion"];
                                        newRow["AadharNumber"] = sourceRow["AadharNumber"];
                                        newRow["SubDivisionName"] = sourceRow["SubDivisionName"];
                                        newRow["MobileNo"] = sourceRow["MobileNo"];
                                        newRow["EmailId"] = sourceRow["EmailId"];
                                        newRow["StudentAddress"] = sourceRow["StudentAddress"];
                                        newRow["AreaName"] = sourceRow["AreaName"];
                                        newRow["MaritalStatus"] = sourceRow["MaritalStatus"];
                                        newRow["StudentBankAccountNo"] = sourceRow["StudentBankAccountNo"];
                                        newRow["BankBranchName"] = sourceRow["BankBranchName"];
                                        newRow["IFSCCode"] = sourceRow["IFSCCode"];
                                        newRow["IdentificationMark1"] = sourceRow["IdentificationMark1"];
                                        newRow["IdentificationMark2"] = sourceRow["IdentificationMark2"];
                                        newRow["MediumName"] = sourceRow["MediumName"];
                                        newRow["RegistrationNo"] = sourceRow["RegistrationNo"];
                                        newRow["UniqueNo"] = sourceRow["UniqueNo"];

                                        newRow["StudentPhotoPath"] = "~/Uploads/StudentsReg/Photos/" + sourceRow["StudentPhotoPath"].ToString();
                                        newRow["StudentSignaturePath"] = "~/Uploads/StudentsReg/Signatures/" + sourceRow["StudentSignaturePath"].ToString();

                                        object isDifferently = sourceRow["DifferentlyAbled"];

                                        if (isDifferently != DBNull.Value && Convert.ToInt32(isDifferently) == 1)
                                        {
                                            newRow["DifferentlyAbled"] = "Yes";
                                        }
                                        else
                                        {
                                            newRow["DifferentlyAbled"] = "No";
                                        }
                                        newRow["FacultyId"] = FacultyId;
                                        //newRow["ExamTypeId"] = ExamTypeId;
                                        mergedTable.Rows.Add(newRow);
                                    }
                                }
                            }
                        }
                        else
                        {
                            log.Warn("Invalid student entry format: " + studentEntry);
                        }
                    }

                    if (mergedTable.Rows.Count > 0)
                    {
                        rptStudents.DataSource = mergedTable;
                        rptStudents.DataBind();
                        log.Info("Merged table bound to rptStudents.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Page_Load: " + ex.Message, ex);
                // Optionally, display an error message to the user
                // lblErrorMessage.Text = "An error occurred while loading student data.";
            }
        }
    }

    private DataTable CreateStudentDataTable()
    {
        log.Debug("Creating empty student data table.");
        DataTable dt = new DataTable();
        dt.Columns.Add("FacultyName");
        dt.Columns.Add("OFSSCAFNo");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("CategoryName");
        dt.Columns.Add("CollegeCode");
        dt.Columns.Add("CollegeName");
        dt.Columns.Add("DistrictName");
        dt.Columns.Add("StudentName");
        dt.Columns.Add("MotherName");
        dt.Columns.Add("FatherName");
        dt.Columns.Add("DOB");
        dt.Columns.Add("MatricBoardName");
        dt.Columns.Add("MatricRollCode");
        dt.Columns.Add("MatricRollNumber");
        dt.Columns.Add("MatricPassingYear");
        dt.Columns.Add("Gender");
        dt.Columns.Add("CasteCategory");
        dt.Columns.Add("DifferentlyAbled");
        dt.Columns.Add("Nationality");
        dt.Columns.Add("Religion");
        dt.Columns.Add("StudentPhotoPath");
        dt.Columns.Add("StudentSignaturePath");
        dt.Columns.Add("AadharNumber");
        dt.Columns.Add("SubDivisionName");
        dt.Columns.Add("MobileNo");
        dt.Columns.Add("EmailId");
        dt.Columns.Add("StudentAddress");
        dt.Columns.Add("AreaName");
        dt.Columns.Add("MaritalStatus");
        dt.Columns.Add("StudentBankAccountNo");
        dt.Columns.Add("BankBranchName");
        dt.Columns.Add("IFSCCode");
        dt.Columns.Add("IdentificationMark1");
        dt.Columns.Add("IdentificationMark2");
        dt.Columns.Add("MediumName");
        dt.Columns.Add("FacultyId");
        //dt.Columns.Add("ExamTypeId");
        dt.Columns.Add("UniqueNo");
        return dt;
    }

    public StudentSubjectData LoadSubjects(string FacultyId, string CollegeId, DataTable appliedSubjects)
    {
        StudentSubjectData subjectData = new StudentSubjectData();

        try
        {
            if (Session["CollegeName"].ToString() == "Admin")
            {
                string collegeCode = Session["CollegeCode"].ToString();
                DataTable dtres = dl.getcollegeidbasedonCollegecode(collegeCode);

                if (dtres.Rows.Count > 0)
                {
                    CollegeId = dtres.Rows[0]["Pk_CollegeId"].ToString();
                }
            }
            else
            {
                CollegeId = Session["CollegeId"].ToString();
            }

            DataTable allSubjects = dl.GetSubjectsByGroup(FacultyId, CollegeId);

            Func<string, int, bool> isApplied = (subjectCode, comGrp) =>
            {
                if (appliedSubjects == null || appliedSubjects.Rows.Count == 0)
                    return false;

                return appliedSubjects.AsEnumerable().Any(appliedRow =>
                {
                    string appliedCode = appliedRow["SubjectPaperCode"].ToString();
                    int appliedComGrp = Convert.ToInt32(appliedRow["ComGrp"]);
                    return appliedCode == subjectCode && appliedComGrp == comGrp;
                });
            };

            var allCompulsorySubjects = allSubjects.AsEnumerable()
      .Where(r => r["GroupName"].ToString() == "Compulsory")
      .OrderBy(r => r["SubjectPaperCode"].ToString())
      .ToList();

            // Group 1: Only English & Hindi
            var group1Subjects = allCompulsorySubjects
                .Where(r => r["SubjectPaperName"].ToString().Trim().Equals("Hindi", StringComparison.OrdinalIgnoreCase)
                         || r["SubjectPaperName"].ToString().Trim().Equals("English", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Group 2: Include all subjects (including English & Hindi again)
            var group2Subjects = allCompulsorySubjects;  // No filtering

            var combined = new List<CombinedSubject>();
            int maxRows = Math.Max(group1Subjects.Count, group2Subjects.Count);
            for (int i = 0; i < maxRows; i++)
            {
                CombinedSubject cs = new CombinedSubject();

                if (i < group1Subjects.Count)
                {
                    string code1 = group1Subjects[i]["SubjectPaperCode"].ToString();
                    cs.Group1SubjectName = group1Subjects[i]["SubjectPaperName"].ToString();
                    cs.Group1SubjectCode = code1;
                    cs.Group1CheckboxHtml = "<input type='checkbox' name='compgrp1' value='" + code1 + "' "
                        + (isApplied(code1, 0) ? "checked='checked'" : "") + " />";
                        //+ (isApplied(code1, 1) ? "checked='checked'" : "") + " />";
                }

                if (i < group2Subjects.Count)
                {
                    string code2 = group2Subjects[i]["SubjectPaperCode"].ToString();
                    cs.Group2SubjectName = group2Subjects[i]["SubjectPaperName"].ToString();
                    cs.Group2SubjectCode = code2;
                    cs.Group2CheckboxHtml = "<input type='checkbox' name='compgrp2' value='" + code2 + "' "
                        + (isApplied(code2, 0) ? "checked='checked'" : "") + " />";
                        //+ (isApplied(code2, 2) ? "checked='checked'" : "") + " />";
                }

                combined.Add(cs);
            }
            subjectData.CompulsorySubjectsCombined = combined;



            // Elective Subjects
            var electives = allSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Elective").ToList();
            DataTable dtElective = new DataTable();
            dtElective.Columns.Add("Name1"); dtElective.Columns.Add("Code1"); dtElective.Columns.Add("Checkbox1Html");
            dtElective.Columns.Add("Name2"); dtElective.Columns.Add("Code2"); dtElective.Columns.Add("Checkbox2Html");

            for (int i = 0; i < electives.Count; i += 2)
            {
                DataRow dr = dtElective.NewRow();
                string code1 = electives[i]["SubjectPaperCode"].ToString();
                dr["Name1"] = electives[i]["SubjectPaperName"];
                dr["Code1"] = code1;
                dr["Checkbox1Html"] = code1 + " <input type='checkbox' name='elective' value='" + code1 + "' "
     + (isApplied(code1, 0) ? "checked='checked'" : "") + " />";
     //+ (isApplied(code1, 3) ? "checked='checked'" : "") + " />";


                if (i + 1 < electives.Count)
                {
                    string code2 = electives[i + 1]["SubjectPaperCode"].ToString();
                    dr["Name2"] = electives[i + 1]["SubjectPaperName"];
                    dr["Code2"] = code2;
                    dr["Checkbox2Html"] = code2 + " <input type='checkbox' name='elective' value='" + code2 + "' "
        + (isApplied(code2, 0) ? "checked='checked'" : "") + " />";
        //+ (isApplied(code2, 3) ? "checked='checked'" : "") + " />";

                }
                dtElective.Rows.Add(dr);
            }
            subjectData.ElectiveSubjects = dtElective;

            var vocElectives = allSubjects.AsEnumerable()
                 .Where(r => r["GroupName"].ToString().Equals("Elective", StringComparison.OrdinalIgnoreCase))
                 .ToList();

            // Group and merge codes for duplicate SubjectPaperName
            var grouped = vocElectives
                .GroupBy(r => r["SubjectPaperName"].ToString())
                .Select(g => new
                {
                    SubjectName = g.Key,
                    Code1 = string.Join(",", g.Select(r => r["SubjectPaperCode"].ToString()).Distinct()),
                    Code2 = string.Join(",", g.Select(r => r.Table.Columns.Contains("SecondCode")
                         ? r["SecondCode"].ToString() : "")
                         .Where(c => !string.IsNullOrEmpty(c)).Distinct())
                })
                .ToList();

            // Create DataTable for Repeater
            DataTable dtVocElective = new DataTable();
            dtVocElective.Columns.Add("Name1");
            dtVocElective.Columns.Add("Code1");
            dtVocElective.Columns.Add("Checkbox1Html");
            dtVocElective.Columns.Add("Name2");
            dtVocElective.Columns.Add("Code2");
            dtVocElective.Columns.Add("Checkbox2Html");

            for (int i = 0; i < grouped.Count; i += 2)
            {
                DataRow dr = dtVocElective.NewRow();

                // Column 1
                string code1 = grouped[i].Code1.Split(',').First(); // Get first code for checkbox value
                dr["Name1"] = grouped[i].SubjectName;
                dr["Code1"] = string.IsNullOrEmpty(grouped[i].Code2)
                    ? grouped[i].Code1
                    : string.Format("{0}, {1}", grouped[i].Code1, grouped[i].Code2);
                dr["Checkbox1Html"] = "<input type='checkbox' name='chkElective' value='"
                    + code1 + "' " + (isApplied(code1, 0) ? "checked='checked'" : "") + " />";
                    //+ code1 + "' " + (isApplied(code1, 3) ? "checked='checked'" : "") + " />";

                // Column 2 (if available)
                if (i + 1 < grouped.Count)
                {
                    string code2 = grouped[i + 1].Code1.Split(',').First(); // Get first code for checkbox value
                    dr["Name2"] = grouped[i + 1].SubjectName;
                    dr["Code2"] = string.IsNullOrEmpty(grouped[i + 1].Code2)
                        ? grouped[i + 1].Code1
                        : string.Format("{0}, {1}", grouped[i + 1].Code1, grouped[i + 1].Code2);
                    dr["Checkbox2Html"] = "<input type='checkbox' name='chkElective' value='"
                        + code2 + "' " + (isApplied(code2, 0) ? "checked='checked'" : "") + " />";
                        //+ code2 + "' " + (isApplied(code2, 3) ? "checked='checked'" : "") + " />";
                }

                dtVocElective.Rows.Add(dr);
            }
            subjectData.VocationalElectiveSubjects = dtVocElective;

            // Additional Subjects
            var additionals = allSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Additional").ToList();
            DataTable dtAdditional = new DataTable();
            dtAdditional.Columns.Add("Name1"); dtAdditional.Columns.Add("Code1"); dtAdditional.Columns.Add("Checkbox1Html");
            dtAdditional.Columns.Add("Name2"); dtAdditional.Columns.Add("Code2"); dtAdditional.Columns.Add("Checkbox2Html");
            dtAdditional.Columns.Add("Name3"); dtAdditional.Columns.Add("Code3"); dtAdditional.Columns.Add("Checkbox3Html");

            for (int i = 0; i < additionals.Count; i += 3)
            {
                DataRow dr = dtAdditional.NewRow();
                for (int j = 0; j < 3 && (i + j) < additionals.Count; j++)
                {
                    string code = additionals[i + j]["SubjectPaperCode"].ToString();
                    dr["Name" + (j + 1)] = additionals[i + j]["SubjectPaperName"];
                    dr["Code" + (j + 1)] = code;
                    dr["Checkbox" + (j + 1) + "Html"] = code + " <input type='checkbox' name='addl' value='" + code + "' "
     + (isApplied(code, 0) ? "checked='checked'" : "") + " />";
     //+ (isApplied(code, 4) ? "checked='checked'" : "") + " />";


                }
                dtAdditional.Rows.Add(dr);
            }
            subjectData.AdditionalSubjects = dtAdditional;

            // Vocational
            var vocationalList = allSubjects.AsEnumerable().Where(r => r["GroupName"].ToString().Contains("Vocational Additional")).ToList();
            if (vocationalList.Any())
            {
                DataTable vocationalDt = new DataTable();
                vocationalDt.Columns.Add("GroupName");
                vocationalDt.Columns.Add("GroupNameHindi");
                vocationalDt.Columns.Add("SubjectPaperCode");
                vocationalDt.Columns.Add("SubjectPaperName");
                vocationalDt.Columns.Add("Fk_SubjectGroupId", typeof(int));
                vocationalDt.Columns.Add("Pk_SubjectPaperId", typeof(int));
                vocationalDt.Columns.Add("CheckboxHtml");

                foreach (DataRow vocationalRow in vocationalList)
                {
                    DataRow newVocationalRow = vocationalDt.NewRow();
                    newVocationalRow["GroupName"] = vocationalRow["GroupName"];
                    newVocationalRow["GroupNameHindi"] = vocationalRow["GroupNameHindi"];
                    newVocationalRow["SubjectPaperCode"] = vocationalRow["SubjectPaperCode"];
                    newVocationalRow["SubjectPaperName"] = vocationalRow["SubjectPaperName"];
                    newVocationalRow["Fk_SubjectGroupId"] = vocationalRow["Fk_SubjectGroupId"];
                    newVocationalRow["Pk_SubjectPaperId"] = vocationalRow["Pk_SubjectPaperId"];

                    string code = vocationalRow["SubjectPaperCode"].ToString();
                    string displayCode = code.Replace("(", "").Replace(")", ""); // This removes brackets

                    newVocationalRow["CheckboxHtml"] = "<input type='checkbox' name='vocational' value='" + displayCode + "' "
                        + (isApplied(code, 0) ? "checked='checked'" : "") + " />";
                        //+ (isApplied(code, 5) ? "checked='checked'" : "") + " />";

                    vocationalDt.Rows.Add(newVocationalRow);
                }


                subjectData.VocationalAdditionalSubjects = vocationalDt;
            }
            else
            {
                subjectData.VocationalAdditionalSubjects = new DataTable();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in LoadSubjects: " + ex.Message, ex);
            throw;
        }

        return subjectData;
    }

    protected string GetDobDigit(object dobValue, int index)
    {
        if (dobValue == null || dobValue == DBNull.Value)
            return "";

        DateTime dob;
        string dobString = dobValue.ToString().Trim();

        // Support multiple formats including dd-MM-yyyy
        string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "dd-MM-yyyy" };

        if (DateTime.TryParseExact(dobString, formats,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out dob))
        {
            // Convert to ddMMyyyy format (only digits)
            string dobStr = dob.ToString("ddMMyyyy");
            if (index >= 0 && index < dobStr.Length)
            {
                return dobStr[index].ToString();
            }
        }
        else
        {
            // Last resort – just strip non-digits from the string
            string onlyDigits = System.Text.RegularExpressions.Regex.Replace(dobString, @"\D", "");
            if (onlyDigits.Length >= 8 && index >= 0 && index < onlyDigits.Length)
            {
                return onlyDigits[index].ToString();
            }
        }

        return "";
    }

    private string ParseDateOfBirth(object dobValue)
    {
        string dobString = dobValue.ToString();
        DateTime parsedDate;

        // Try parsing with multiple formats (yyyy-MM-dd and dd/MM/yyyy)
        if (DateTime.TryParseExact(dobString, new string[] { "yyyy-MM-dd", "dd/MM/yyyy" },
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return parsedDate.ToString("dd/MM/yyyy");  // Use a standard format
        }
        else if (DateTime.TryParse(dobString, out parsedDate))  // Fallback if other formats work
        {
            return parsedDate.ToString("dd/MM/yyyy");
        }
        else
        {
            log.Warn("Invalid DOB format: " + dobString);  // Log warning if it's invalid
            return "";  // Return empty if parsing fails
        }
    }



    protected void rptStudents_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            //HiddenField hfExamTypeId = (HiddenField)e.Item.FindControl("hfExamTypeId");
            //Label lblExamTitle = (Label)e.Item.FindControl("lblExamTitle");
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                //hfExamTypeId.Value = drv["ExamTypeId"].ToString();
                //string examTypeId = hfExamTypeId.Value;
                //if (examTypeId == "2")
                //{
                //    lblExamTitle.Text = "Compartmental";
                //}
                //else
                //{
                    //lblExamTitle.Text = "Special";
                //}

                string photoPath = string.Empty;
                if (drv["StudentPhotoPath"] != DBNull.Value)
                {
                    photoPath = drv["StudentPhotoPath"].ToString();
                }

                string signaturePath = string.Empty;
                if (drv["StudentSignaturePath"] != DBNull.Value)
                {
                    signaturePath = drv["StudentSignaturePath"].ToString();
                }

                //string studentIdStr = string.Empty;
                //if (drv["StudentId"] != DBNull.Value) 
                //{
                //    studentIdStr = drv["StudentId"].ToString();
                //}
                DataRowView drv1 = (DataRowView)e.Item.DataItem;
                string facultyId = drv["FacultyId"] != null ? drv1["FacultyId"].ToString() : "";
                string collegeId = Session["CollegeId"] != null ? Session["CollegeId"].ToString() : "";
                string StudentId = Session["StudentId"] != null ? Session["StudentId"].ToString() : "";

                Image imgPhoto = (Image)e.Item.FindControl("imgPhoto");
                Image imgSign = (Image)e.Item.FindControl("imgSign");
                if (imgPhoto != null && !string.IsNullOrEmpty(photoPath))
                {
                    imgPhoto.ImageUrl = ResolveUrl(photoPath);
                }
                if (imgSign != null && !string.IsNullOrEmpty(signaturePath))
                {
                    imgSign.ImageUrl = ResolveUrl(signaturePath);
                }

                //string facultyId = string.Empty;
                //if (drv["FacultyId"] != DBNull.Value)
                //{
                //    facultyId = drv["FacultyId"].ToString();
                //}

                //string collegeId = string.Empty;
                //if (drv["CollegeId"] != DBNull.Value)
                //{
                //    collegeId = drv["CollegeId"].ToString();
                //}
                //else if (Session["CollegeId"] != null)
                //{
                //    collegeId = Session["CollegeId"].ToString();
                //}

                //if (string.IsNullOrEmpty(facultyId) || string.IsNullOrEmpty(collegeId))
                //{
                //    System.Diagnostics.Debug.WriteLine("Missing FacultyId or CollegeId for student in rptStudents_ItemDataBound.");
                //    return;
                //}


                DataTable appliedSubjects = dl.GetAppliedSubjects(StudentId);

                StudentSubjectData ssd = LoadSubjects(facultyId, collegeId, appliedSubjects);
                //StudentSubjectData ssd = LoadSubjects(facultyId, collegeId);

                Repeater rptCompulsorySubjectsCombined = e.Item.FindControl("rptCompulsorySubjectsCombined") as Repeater;
                if (rptCompulsorySubjectsCombined != null && ssd.CompulsorySubjectsCombined != null)
                {
                    rptCompulsorySubjectsCombined.DataSource = ssd.CompulsorySubjectsCombined;
                    rptCompulsorySubjectsCombined.DataBind();
                }

                Repeater rptElectiveSubjects = e.Item.FindControl("rptElectiveSubjects") as Repeater;
                //if (rptElectiveSubjects != null && ssd.ElectiveSubjects != null && ssd.ElectiveSubjects.Rows.Count > 0)
                //{
                //    rptElectiveSubjects.DataSource = ssd.ElectiveSubjects;
                //    rptElectiveSubjects.DataBind();
                //}
                Repeater rptVocElective = e.Item.FindControl("rptVocElectiveSubjects") as Repeater;

                if (facultyId.Equals("4", StringComparison.OrdinalIgnoreCase))
                {
                    rptElectiveSubjects.Visible = false;
                    rptVocElective.Visible = true;

                    // Use vocational electives from LoadSubjects (You must update LoadSubjects to provide this)
                    if (ssd.VocationalElectiveSubjects != null && ssd.VocationalElectiveSubjects.Rows.Count > 0)
                    {
                        rptVocElective.DataSource = ssd.VocationalElectiveSubjects;
                        rptVocElective.DataBind();
                    }
                }
                else if (rptElectiveSubjects != null && ssd.ElectiveSubjects != null && ssd.ElectiveSubjects.Rows.Count > 0)
                {
                    rptElectiveSubjects.DataSource = ssd.ElectiveSubjects;
                    rptElectiveSubjects.DataBind();
                }
                Repeater rptAdditionalSubjects = e.Item.FindControl("rptAdditionalSubjects") as Repeater;
                if (rptAdditionalSubjects != null && ssd.AdditionalSubjects != null && ssd.AdditionalSubjects.Rows.Count > 0)
                {
                    rptAdditionalSubjects.DataSource = ssd.AdditionalSubjects;
                    rptAdditionalSubjects.DataBind();
                }

                Repeater rptVocationalAdditionalSubjects = e.Item.FindControl("rptVocationalAdditionalSubjects") as Repeater;
                Panel pnlVocational = e.Item.FindControl("pnlVocational") as Panel;

                if (rptVocationalAdditionalSubjects != null && pnlVocational != null)
                {
                    if (ssd.VocationalAdditionalSubjects != null && ssd.VocationalAdditionalSubjects.Rows.Count > 0)
                    {
                        rptVocationalAdditionalSubjects.DataSource = ssd.VocationalAdditionalSubjects;
                        rptVocationalAdditionalSubjects.DataBind();
                        pnlVocational.Visible = true;
                    }
                    else
                    {
                        pnlVocational.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in rptStudents_ItemDataBound: " + ex.Message, ex);
            // Optionally, log the error or display a message
        }
    }

    // Dummy class for subject data, replace with your actual data structure if it's different
    public class StudentSubjectData
    {
        public List<CombinedSubject> CompulsorySubjectsCombined { get; set; }
        public DataTable ElectiveSubjects { get; set; }
        public DataTable AdditionalSubjects { get; set; }
        public DataTable VocationalAdditionalSubjects { get; set; }

        public DataTable VocationalElectiveSubjects { get; set; }
    }

    // Dummy class for combined subjects, replace with your actual data structure if it's different
    public class CombinedSubject
    {
        public string Group1SubjectName { get; set; }
        public string Group1SubjectCode { get; set; }
        public string Group1CheckboxHtml { get; set; }
        public string Group2SubjectName { get; set; }
        public string Group2SubjectCode { get; set; }
        public string Group2CheckboxHtml { get; set; }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string encodedStudentData = Request.QueryString["studentData"];
            if (!string.IsNullOrEmpty(encodedStudentData))
            {
                string decodedStudentData = Server.UrlDecode(encodedStudentData);
                log.Debug("Decoded student data: " + decodedStudentData);

                List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();



                foreach (var StuIds in selectedStudents)
                {
                    var Parts = StuIds.Split('|');
                    // int id; // This variable is declared but never used. Can be removed or used if needed.
                    if (Parts.Length == 5)
                    {
                        int studentId;
                        if (int.TryParse(Parts[0], out studentId))
                        {
                            dl.UpdateExamStudentsFormDownloaded(studentId);
                        }

                    }
                }


            }
            ClientScript.RegisterStartupScript(this.GetType(), "printScript", "window.print();", true);
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }


    //[System.Web.Services.WebMethod]
    //public static string UpdateDownloaded(string studentData)
    //{
    //    try
    //    {
    //        string decodedStudentData = HttpContext.Current.Server.UrlDecode(studentData);
    //        List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

    //        foreach (var StuIds in selectedStudents)
    //        {
    //            var Parts = StuIds.Split('|');
    //            if (Parts.Length == 3)
    //            {
    //                int studentId;
    //                if (int.TryParse(Parts[0], out studentId))
    //                {
    //                    // Call your existing DL method
    //                    DBHelper db = new DBHelper();
    //                    db.UpdateExamStudentsFormDownloaded(studentId);
    //                }
    //            }
    //        }
    //        return "success";
    //    }
    //    catch (Exception ex)
    //    {
    //        return "error: " + "Load Page Again";
    //    }
    //}

}