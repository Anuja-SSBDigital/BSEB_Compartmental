using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class StudentSubjectgrps : System.Web.UI.Page
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
                    string studentId = Request.QueryString["studentId"];
                    string FacultyId = Request.QueryString["FacultyId"];
                    string fromPage = Request.QueryString["from"];
                    string collegeCode = Request.QueryString["collegeCode"];

                    DataTable facultyList = dl.getFacultyfordropdown();
                    ddlFaculty.DataSource = facultyList;
                    ddlFaculty.DataTextField = "FacultyName";
                    ddlFaculty.DataValueField = "Pk_FacultyId";
                    ddlFaculty.DataBind();
                    ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));

                    if (!string.IsNullOrEmpty(FacultyId))
                    {
                        ListItem selectedItem = ddlFaculty.Items.FindByValue(FacultyId);
                        if (selectedItem != null)
                        {
                            ddlFaculty.ClearSelection();
                            selectedItem.Selected = true;
                        }
                    }


                    if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(FacultyId))
                    {

                        string CollegeId="";
                        //if (Session["CollegeName"].ToString() == "Admin")
                        //{
                        //    DataTable collegeid = dl.GetCollegeIdFromAdmin(collegeCode);
                        //    if (collegeid != null && collegeid.Rows.Count > 0)
                        //    {
                        //        CollegeId = collegeid.Rows[0]["Collegeid"].ToString();
                        //    }
                        //}
                        if (Session["CollegeName"].ToString() == "Admin")
                        {
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
                        DataTable AllSubjects = dl.GetSubjectsByGroup(FacultyId, CollegeId);

                        DataView dvCompulsoryHindiEng = new DataView(AllSubjects);
                        dvCompulsoryHindiEng.RowFilter = "GroupName = 'Compulsory' AND (SubjectPaperName = 'Hindi' OR SubjectPaperName = 'English')";
                        rptCompulsorySubjects.DataSource = dvCompulsoryHindiEng;
                        rptCompulsorySubjects.DataBind();


                        if (FacultyId == "4")
                        {
                            var vocElective = AllSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Elective")
                             .GroupBy(r =>
                             {
                                 var paperType = r["PaperType"] == DBNull.Value ? "" : r["PaperType"].ToString();
                                 return string.IsNullOrWhiteSpace(paperType) ? "Paper-1" : paperType;
                             }).ToDictionary(g => g.Key, g => g.ToList());

                            DataTable vocElectiveTable = new DataTable();
                            vocElectiveTable.Columns.Add("Name1"); // Paper-1 Name
                            vocElectiveTable.Columns.Add("Code1"); // Paper-1 Code
                            vocElectiveTable.Columns.Add("Name2"); // Paper-2 Name
                            vocElectiveTable.Columns.Add("Code2"); // Paper-2 Code

                            // Get lists for Paper-1 and Paper-2
                            var paper1 = vocElective.ContainsKey("Paper-1") ? vocElective["Paper-1"] : new List<DataRow>();
                            var paper2 = vocElective.ContainsKey("Paper-2") ? vocElective["Paper-2"] : new List<DataRow>();

                            int maxCount = Math.Max(paper1.Count, paper2.Count);

                            for (int i = 0; i < maxCount; i++)
                            {
                                DataRow newRow = vocElectiveTable.NewRow();

                                if (i < paper1.Count)
                                {
                                    newRow["Name1"] = paper1[i]["SubjectPaperName"];
                                    newRow["Code1"] = paper1[i]["SubjectPaperCode"];
                                }

                                if (i < paper2.Count)
                                {
                                    newRow["Name2"] = paper2[i]["SubjectPaperName"];
                                    newRow["Code2"] = paper2[i]["SubjectPaperCode"];
                                }

                                vocElectiveTable.Rows.Add(newRow);
                            }

                            rptVocElectiveSubjects.DataSource = vocElectiveTable;
                            rptVocElectiveSubjects.DataBind();

                            ElectiveSection.Visible = false;
                            VocElectiveSection.Visible = true;

                        }
                        else
                        {
                            var elective = AllSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Elective").ToList();

                            DataTable electiveReshaped = new DataTable();
                            electiveReshaped.Columns.Add("Name1");
                            electiveReshaped.Columns.Add("Code1");
                            electiveReshaped.Columns.Add("Name2");
                            electiveReshaped.Columns.Add("Code2");

                            for (int i = 0; i < elective.Count; i += 2)
                            {
                                DataRow newRow = electiveReshaped.NewRow();
                                newRow["Name1"] = elective[i]["SubjectPaperName"];
                                newRow["Code1"] = elective[i]["SubjectPaperCode"];
                                if (i + 1 < elective.Count)
                                {
                                    newRow["Name2"] = elective[i + 1]["SubjectPaperName"];
                                    newRow["Code2"] = elective[i + 1]["SubjectPaperCode"];
                                }
                                electiveReshaped.Rows.Add(newRow);
                            }

                            rptElectiveSubjects.DataSource = electiveReshaped;
                            rptElectiveSubjects.DataBind();
                            ViewState["ElectiveSubjects"] = electiveReshaped;
                            VocElectiveSection.Visible = false;
                        }
                        // =========================
                        // 3-column layout for "Additional"
                        // =========================
                        var additional = AllSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Additional").CopyToDataTable();

                        DataTable reshaped = new DataTable();
                        for (int i = 1; i <= 3; i++)
                        {
                            reshaped.Columns.Add("Name" + i);
                            reshaped.Columns.Add("Code" + i);
                            reshaped.Columns.Add("PaperId" + i);
                        }

                        for (int i = 0; i < additional.Rows.Count; i += 3)
                        {
                            DataRow newRow = reshaped.NewRow();
                            for (int j = 0; j < 3; j++)
                            {
                                if (i + j < additional.Rows.Count)
                                {
                                    newRow["Name" + (j + 1)] = additional.Rows[i + j]["SubjectPaperName"];
                                    newRow["Code" + (j + 1)] = additional.Rows[i + j]["SubjectPaperCode"];
                                    newRow["PaperId" + (j + 1)] = additional.Rows[i + j]["Pk_SubjectPaperId"];
                                }
                            }
                            reshaped.Rows.Add(newRow);
                        }

                        rptAdditionalSubjects.DataSource = reshaped;
                        rptAdditionalSubjects.DataBind();





                        BindRepeater(AllSubjects, "Compulsory", rptCompulsorySubjects2);
                        BindRepeater(AllSubjects, "Vocational Additional", rptVocationalAdditionalSubjects);
                        
                        ApplyPreviouslySelectedSubjects(studentId);
                       
                    }
               
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {

                string errorMessage = "An error occurred during page load: " + ex.Message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + errorMessage + "');", true);
            }
        }
       
    }
    private void RestoreElectiveCheckboxes(Repeater repeater, string[] selectedCodes)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            for (int i = 1; i <= 2; i++)
            {
                string chkId = "chkElective" + i;
                CheckBox chk = item.FindControl(chkId) as CheckBox;

                if (chk != null && chk.Attributes["Value"] != null)
                {
                    string code = chk.Attributes["Value"].ToString();
                    if (selectedCodes.Contains(code))
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
    }
    private void BindRepeater(DataTable dt, string groupName, System.Web.UI.WebControls.Repeater repeater)
    {
        try
        {
            DataView dvVocational = new DataView(dt);
            dvVocational.RowFilter = "GroupName = 'Vocational Additional'";
            if (dvVocational.Count > 0)
            {
                divVocationalSubjects.Visible = true;
                div_vocational.Visible = true;
                Borderline_vocational.Visible = true;
                rptVocationalAdditionalSubjects.DataSource = dvVocational;
                rptVocationalAdditionalSubjects.DataBind();
            }
           
         
                
                DataView dv = new DataView(dt);
                dv.RowFilter = string.Format("GroupName = '{0}'", groupName);
                repeater.DataSource = dv;
                repeater.DataBind();
            
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void btnSubmitSubjects_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = Request.QueryString["studentId"];
            string modifiedBy = Session["CollegeId"].ToString();
            string FacultyId = Request.QueryString["FacultyId"];
            if (string.IsNullOrEmpty(studentId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Student ID is missing.');", true);
                return;
            }
            LoadComGrpMap(studentId);
            if (!SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy, 5))
                return;

            if (FacultyId == "4")
            {
                SaveSelectedSubjects(rptVocElectiveSubjects, studentId, modifiedBy, 3);
            }
            // Process each Repeater
            SaveSelectedSubjects(rptCompulsorySubjects, studentId, modifiedBy, 1);
            SaveSelectedSubjects(rptCompulsorySubjects2, studentId, modifiedBy, 2);
            // SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy,-1);
            SaveSelectedSubjects(rptElectiveSubjects, studentId, modifiedBy, 3);
            SaveSelectedSubjects(rptAdditionalSubjects, studentId, modifiedBy, 4);

           
            string script = @"
                    <script>
                        Swal.fire({
                            icon: 'success',
                            title: 'Subjects Submitted',
                            text: 'Subjects submitted successfully!',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = 'StudentPhotoAndsignatureDetails.aspx?studentId=" + Server.UrlEncode(studentId.ToString()) + @"';
                            }
                        });
                    </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "sweetAlertSuccess", script);

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    private void LoadComGrpMap(string studentId)
    {
        var map = new Dictionary<string, string>();
        DataTable appliedSubjects = dl.GetAppliedSubjects(studentId);
        foreach (DataRow row in appliedSubjects.Rows)
        {
            string comgrp = row["ComGrp"].ToString();
            string pkId = row["Pk_StudentPaperAppliedId"].ToString();
            map[comgrp] = pkId;
        }
        ViewState["ComGrpAppliedMap"] = map;
    }
    private bool SaveSelectedSubjects(Repeater repeater, string studentId, string modifiedBy, int comgrp)
    {
        try
        {
            if (repeater.ID == "rptVocationalAdditionalSubjects")
            {
                HashSet<string> selectedCodes = new HashSet<string>();
                bool vocationalSubjectSelected = false;
                DataTable existingSubjects = new DataTable();
                existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
                foreach (RepeaterItem item in repeater.Items)
                {
                    CheckBox chkVocational = (CheckBox)item.FindControl("chkSubject");
                    if (chkVocational != null && chkVocational.Checked)
                    {
                        string code = chkVocational.Attributes["Value"];
                        if (!string.IsNullOrEmpty(code))
                        {
                            selectedCodes.Add(code);
                            vocationalSubjectSelected = true;
                        }
                    }
                }

                if (vocationalSubjectSelected)
                {
                    string collegeId = "";
                    if (Session["CollegeName"].ToString() == "Admin")
                    {
                        string collegeCode = Request.QueryString["collegeCode"];
                        DataTable dtres = dl.getcollegeidbasedonCollegecode(collegeCode);

                        if (dtres.Rows.Count > 0)
                        {
                            collegeId = dtres.Rows[0]["Pk_CollegeId"].ToString();

                        }

                    }
                    else
                    {
                         collegeId = Session["CollegeId"].ToString();
                    }
                    
                    string facultyId = Request.QueryString["FacultyId"];
                    const int vocationalLimit = 25;

                    int currentCount = dl.GetVocationalSubjectCount(facultyId, collegeId);
                    hfVocationalSubjectCount.Value = currentCount.ToString();


                     //existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
                  

                    if (currentCount >= vocationalLimit)
                    {
                        bool studentAlreadyHasVocational = existingSubjects.Rows.Count > 0;


                        if (studentAlreadyHasVocational && currentCount >= vocationalLimit)
                        {
                            string SubjectPaperCode = existingSubjects.Rows[0]["SubjectPaperCode"].ToString();
                            string pkId = existingSubjects.Rows[0]["Pk_StudentPaperAppliedId"].ToString();
                            DataTable subjectInfo = dl.GetSubjectDetailsByCode(SubjectPaperCode);
                            if (subjectInfo.Rows.Count > 0)
                            {
                                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();
                                dl.UpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                            }
                        }
                        else
                        {
                            string script = @"
                                    alert('Only " + vocationalLimit + @" students can apply for vocational subjects in this faculty and college.');
                                    document.querySelectorAll('.VocationalSubjects input[type=""checkbox""]').forEach(cb => {
                                        cb.checked = false;
                                    });";
                            ClientScript.RegisterStartupScript(this.GetType(), "VocationalLimitAlertAndUncheck", script, true);
                            return false;
                        }
                         
                    }
                }

               // DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

                
               // bool isChecked = vocationalSubjectSelected;
                if (vocationalSubjectSelected)
                {
                    foreach (string code in selectedCodes)
                    {
                        DataRow existingRow = existingSubjects.AsEnumerable()
                            .FirstOrDefault(r => r["SubjectPaperCode"].ToString() == code);

                        if (existingRow != null)
                        {
                            int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);
                            DataTable subjectInfo = dl.GetSubjectDetailsByCode(code);
                            if (subjectInfo.Rows.Count > 0)
                            {
                                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();
                                dl.UpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                            }
                        }
                        else
                        {
                            SaveSubjectByCode(code, studentId, modifiedBy, comgrp);
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in existingSubjects.Rows)
                    {
                        string savedCode = row["SubjectPaperCode"].ToString();
                        int pkId = Convert.ToInt32(row["Pk_StudentPaperAppliedId"]);

                        if (!selectedCodes.Contains(savedCode))
                        {
                            dl.DeleteStudentPaperAppliedById(pkId);
                        }
                    }
                }

                return true;
            }

            foreach (RepeaterItem item in repeater.Items)
            {
                if (repeater.ID == "rptElectiveSubjects")
                {
                    ProcessElectiveSubjects(studentId, modifiedBy, comgrp);
                    return true;
                }
                else if (repeater.ID == "rptVocElectiveSubjects")
                {
                    ProcessVocElectiveSubjects(studentId, modifiedBy, comgrp);
                    return true;
                }
                else if (repeater.ID == "rptAdditionalSubjects")
                {
                    ProcessAdditionalSubjects(item, studentId, modifiedBy, comgrp);
                }
                else
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSubject");
                    if (chk != null && chk.Checked)
                    {
                        string subjectPaperCode = chk.Attributes["Value"];
                        SaveSubjectByCode(subjectPaperCode, studentId, modifiedBy, comgrp);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    private void ProcessElectiveSubjects(string studentId, string modifiedBy, int comgrp)
    {
        var selected1 = Request.Form.GetValues("chkElectiveSubjects") ?? new string[0];
        var selected2 = Request.Form.GetValues("chkGroup2Subjects") ?? new string[0];

       
        List<string> selectedCodes = new List<string>(selected1);
        selectedCodes.AddRange(selected2);

        if (selectedCodes.Count != 3)
        {
            throw new Exception("Exactly 3 elective subjects must be selected.");
        }

        
        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

       
        while (existingSubjects.Rows.Count < 3)
        {
           
            SaveSubjectByCode(selectedCodes[existingSubjects.Rows.Count], studentId, modifiedBy, comgrp);
            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }

        
        while (existingSubjects.Rows.Count > 3)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[0]["Pk_StudentPaperAppliedId"]);
            dl.DeleteStudentPaperAppliedById(pkId);
            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }

       
        for (int i = 0; i < 3; i++)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[i]["Pk_StudentPaperAppliedId"]);
            string subjectCode = selectedCodes[i];

            DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectCode);
            if (subjectInfo.Rows.Count > 0)
            {
                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                dl.UpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
            }
        }
    }

    private void ProcessAdditionalSubjects(RepeaterItem item, string studentId, string modifiedBy, int comgrp)
    {
        // Get all existing applied subjects for the student in this group
        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

        for (int i = 1; i <= 3; i++)
        {
            string chkId = "chkAdditional" + i;
            string hfCodeId = "hfCode" + i;
            string hfPaperIdId = "hfPaperId" + i;

            CheckBox chk = item.FindControl(chkId) as CheckBox;
            HiddenField hfCode = item.FindControl(hfCodeId) as HiddenField;
            HiddenField hfPaperId = item.FindControl(hfPaperIdId) as HiddenField;

            if (chk != null && hfCode != null && hfPaperId != null)
            {
                string code = hfCode.Value;
                string paperId = hfPaperId.Value;
                bool isChecked = chk.Checked;

                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(paperId))
                    continue;

                // SubjectGroupId for "Additional" group is always 3
                string subjectGroupId = "3";

                // Check if subject already exists
                DataRow existingRow = existingSubjects.AsEnumerable()
                    .FirstOrDefault(r => r["SubjectPaperCode"].ToString() == code);

                if (isChecked)
                {
                    if (existingRow != null)
                    {
                        // Update existing selection
                        int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);
                        dl.UpdateStudentSubjectByPkId(pkId.ToString(), paperId, subjectGroupId, modifiedBy);
                    }
                    else
                    {
                        // Insert new selection
                        dl.InsertStudentSubjectUsingSP(studentId, paperId, subjectGroupId, modifiedBy, comgrp);
                    }
                }
                else
                {
                    if (existingRow != null)
                    {
                        // Delete unselected subject
                        int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);
                        dl.DeleteStudentPaperAppliedById(pkId);
                    }
                }
            }
        }
    }

    private string GetPreviouslySelectedCode(string studentId, int comgrp, int slotIndex)
    {
        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

        // You’ll need a way to map slots (1/2/3) to subjects – maybe by index or database field
        if (slotIndex <= existingSubjects.Rows.Count)
        {
            return existingSubjects.Rows[slotIndex - 1]["SubjectPaperCode"].ToString();
        }

        return null;
    }

    private void ProcessVocElectiveSubjects(string studentId, string modifiedBy, int comgrp)
    {
        var selected1 = Request.Form.GetValues("chkVocElectiveSubjects") ?? new string[0];
        var selected2 = Request.Form.GetValues("chkGroup2Subjects") ?? new string[0];


        List<string> selectedCodes = new List<string>(selected1);
        selectedCodes.AddRange(selected2);

        if (selectedCodes.Count != 3)
        {
            throw new Exception("Exactly 3 elective subjects must be selected.");
        }


        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);


        while (existingSubjects.Rows.Count < 3)
        {

            SaveSubjectByCode(selectedCodes[existingSubjects.Rows.Count], studentId, modifiedBy, comgrp);
            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }


        while (existingSubjects.Rows.Count > 3)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[0]["Pk_StudentPaperAppliedId"]);
            dl.DeleteStudentPaperAppliedById(pkId);
            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }


        for (int i = 0; i < 3; i++)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[i]["Pk_StudentPaperAppliedId"]);
            string subjectCode = selectedCodes[i];

            DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectCode);
            if (subjectInfo.Rows.Count > 0)
            {
                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                dl.UpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
            }
        }
    }

    private void SaveSubjectByCode(string subjectPaperCode, string studentId, string modifiedBy, int comgrp)
    {
        try
        {
            DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectPaperCode);
            if (subjectInfo.Rows.Count > 0)
            {
                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                string pkId = null;
                var map = ViewState["ComGrpAppliedMap"] as Dictionary<string, string>;
                if (map != null)
                {
                    map.TryGetValue(comgrp.ToString(), out pkId);
                }

                if (!string.IsNullOrEmpty(pkId))
                {
                    dl.UpdateStudentSubjectByPkId(pkId, subjectPaperId, subjectGroupId, modifiedBy);
                }
                else
                {
                    dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex; 
        }
    }



    //Selectedchekbox logic 
    private void ApplyPreviouslySelectedSubjects(string studentId)
    {
        try
        {
            DataTable appliedSubjects = dl.GetAppliedSubjects(studentId);

            
            Dictionary<string, string> comGrpToAppliedId = new Dictionary<string, string>();
            foreach (DataRow row in appliedSubjects.Rows)
            {
                string comGrp = row["ComGrp"].ToString();
                string pkId = row["Pk_StudentPaperAppliedId"].ToString();
                string SubjectPaperId = row["SubjectPaperId"].ToString();
                hfStudentPaperAppliedId.Value = pkId;
                hfSubjectPaperId.Value = SubjectPaperId;
                if (!comGrpToAppliedId.ContainsKey(comGrp))
                    comGrpToAppliedId[comGrp] = pkId;
            }

            ViewState["ComGrpAppliedMap"] = comGrpToAppliedId;

           
            Dictionary<string, HashSet<string>> groupWiseCodes = new Dictionary<string, HashSet<string>>();
            foreach (DataRow row in appliedSubjects.Rows)
            {
                string code = row["SubjectPaperCode"].ToString();
                string group = row["ComGrp"].ToString();

                if (!groupWiseCodes.ContainsKey(group))
                    groupWiseCodes[group] = new HashSet<string>();

                groupWiseCodes[group].Add(code);
            }
            string FacultyId = Request.QueryString["FacultyId"];
            if (FacultyId == "4")
            {
                
                ApplyToVocElectiveSubjects(rptVocElectiveSubjects, groupWiseCodes, "3");
            }
            ApplyToCompulsorySubjects(rptCompulsorySubjects, groupWiseCodes, "1");
            ApplyToCompulsorySubjects(rptCompulsorySubjects2, groupWiseCodes, "2");
            ApplyToElectiveSubjects(rptElectiveSubjects, groupWiseCodes, "3");
            ApplyToAdditionalSubjects(rptAdditionalSubjects, groupWiseCodes, "4");
            ApplyToVocationalSubjects(rptVocationalAdditionalSubjects, groupWiseCodes, "5");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void ApplyToCompulsorySubjects(Repeater repeater, Dictionary<string, HashSet<string>> groupWiseCodes, string comGrp)
    {
        try
        {
            if (!groupWiseCodes.ContainsKey(comGrp)) return;

            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox chk = item.FindControl("chkSubject") as CheckBox;
                if (chk != null && groupWiseCodes[comGrp].Contains(chk.Attributes["value"]))
                {
                    chk.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
      
    }

    private void ApplyToVocationalSubjects(Repeater repeater, Dictionary<string, HashSet<string>> groupWiseCodes, string comGrp)
    {
        try
        {
            if (!groupWiseCodes.ContainsKey(comGrp)) return;

            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox chk = item.FindControl("chkSubject") as CheckBox;
                if (chk != null && groupWiseCodes[comGrp].Contains(chk.Attributes["value"]))
                {
                    chk.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
     
    }


    private HashSet<string> selectedElectiveCodes = new HashSet<string>();

    private void ApplyToElectiveSubjects(Repeater repeater, Dictionary<string, HashSet<string>> groupWiseCodes, string comGrp)
    {
        try
        {
            if (!groupWiseCodes.ContainsKey(comGrp)) return;

            selectedElectiveCodes = groupWiseCodes[comGrp];

            repeater.DataBind();
        }
        catch (Exception ex)
        {

            throw ex;
        }
      
    }
    private void ApplyToVocElectiveSubjects(Repeater repeater, Dictionary<string, HashSet<string>> groupWiseCodes, string comGrp)
    {
        try
        {
            if (!groupWiseCodes.ContainsKey(comGrp)) return;

            selectedElectiveCodes = groupWiseCodes[comGrp];

            repeater.DataBind();
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public string GetCheckedAttr(object code)
    {
        try
        {
            if (code == null) return "";
            return selectedElectiveCodes.Contains(code.ToString()) ? "checked='checked'" : "";
        }
        catch (Exception ex)
        {

            throw ex;
        }
      
    }


    private void ApplyToAdditionalSubjects(Repeater repeater, Dictionary<string, HashSet<string>> groupWiseCodes, string comGrp)
    {
        try
        {
            if (!groupWiseCodes.ContainsKey(comGrp)) return;

            foreach (RepeaterItem item in repeater.Items)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var chk = item.FindControl("chkAdditional" + i) as CheckBox;
                    var hf = item.FindControl("hfCode" + i) as HiddenField;

                    if (chk != null && hf != null && chk.Enabled && groupWiseCodes[comGrp].Contains(hf.Value))
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }



}
