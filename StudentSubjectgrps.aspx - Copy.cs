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
                        string CollegeId = Session["CollegeId"].ToString();
                        DataTable AllSubjects = dl.GetSubjectsByGroup(FacultyId, CollegeId);

                        DataView dvCompulsoryHindiEng = new DataView(AllSubjects);
                        dvCompulsoryHindiEng.RowFilter = "GroupName = 'Compulsory' AND (SubjectPaperName = 'Hindi' OR SubjectPaperName = 'English')";
                        rptCompulsorySubjects.DataSource = dvCompulsoryHindiEng;
                        rptCompulsorySubjects.DataBind();



                        var elective = AllSubjects.AsEnumerable()
                  .Where(r => r["GroupName"].ToString() == "Elective")
                  .ToList();

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
                        //if (ViewState["SelectedElectives"] != null)
                        //{
                        //    RestoreElectiveCheckboxes(rptElectiveSubjects, (string[])ViewState["SelectedElectives"]);
                        //}


                        // =========================
                        // 3-column layout for "Additional"
                        // =========================
                        var additional = AllSubjects.AsEnumerable()
                            .Where(r => r["GroupName"].ToString() == "Additional")
                            .CopyToDataTable();

                        DataTable reshaped = new DataTable();
                        for (int i = 1; i <= 3; i++)
                        {
                            reshaped.Columns.Add("Name" + i);
                            reshaped.Columns.Add("Code" + i);
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
                                }
                            }
                            reshaped.Rows.Add(newRow);
                        }

                        rptAdditionalSubjects.DataSource = reshaped;
                        rptAdditionalSubjects.DataBind();





                        BindRepeater(AllSubjects, "Compulsory", rptCompulsorySubjects2);
                        BindRepeater(AllSubjects, "Voccational Additional", rptVocationalAdditionalSubjects);
                        //BindRepeater(AllSubjects, "Elective", rptElectiveSubjects);
                        //BindRepeater(AllSubjects, "Additional", rptAdditionalSubjects);

                        if (fromPage == "photoPage")
                        {
                            ApplyPreviouslySelectedSubjects(studentId);
                        }
                    }
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Student ID or Faculty Name is missing in the URL.');", true);
                    //}
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
        //else
        //{
        //    if (Session["CollegeId"] != null)
        //    {
        //        string FacultyId = Request.QueryString["FacultyId"];
        //        string CollegeId = Session["CollegeId"].ToString();

        //        DataTable AllSubjects = dl.GetSubjectsByGroup(FacultyId, CollegeId);

        //        // Rebuild the electiveReshaped DataTable again
        //        var elective = AllSubjects.AsEnumerable()
        //            .Where(r => r["GroupName"].ToString() == "Elective")
        //            .ToList();

        //        DataTable electiveReshaped = new DataTable();
        //        electiveReshaped.Columns.Add("Name1");
        //        electiveReshaped.Columns.Add("Code1");
        //        electiveReshaped.Columns.Add("Name2");
        //        electiveReshaped.Columns.Add("Code2");

        //        for (int i = 0; i < elective.Count; i += 2)
        //        {
        //            DataRow newRow = electiveReshaped.NewRow();
        //            newRow["Name1"] = elective[i]["SubjectPaperName"];
        //            newRow["Code1"] = elective[i]["SubjectPaperCode"];
        //            if (i + 1 < elective.Count)
        //            {
        //                newRow["Name2"] = elective[i + 1]["SubjectPaperName"];
        //                newRow["Code2"] = elective[i + 1]["SubjectPaperCode"];
        //            }
        //            electiveReshaped.Rows.Add(newRow);
        //        }

        //        rptElectiveSubjects.DataSource = electiveReshaped;
        //        rptElectiveSubjects.DataBind();  // 🟢 Rebind data here

        //        // Now you can safely set checkbox selections
        //        string selectedElectives = hfElectiveSubjects.Value;
        //        if (!string.IsNullOrEmpty(selectedElectives))
        //        {
        //            string[] selectedCodes = selectedElectives.Split(',');
        //            foreach (RepeaterItem item in rptElectiveSubjects.Items)
        //            {
        //                HtmlInputCheckBox cb1 = (HtmlInputCheckBox)item.FindControl("chkSubject_1");
        //                HtmlInputCheckBox cb2 = (HtmlInputCheckBox)item.FindControl("chkGroup2_2");

        //                if (cb1 != null && selectedCodes.Contains(cb1.Value))
        //                    cb1.Checked = true;

        //                if (cb2 != null && selectedCodes.Contains(cb2.Value))
        //                    cb2.Checked = true;
        //            }
        //        }
        //    }
        //}

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
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("GroupName = '{0}'", groupName);
            repeater.DataSource = dv;
            repeater.DataBind();
        }
        catch (Exception ex)
        {

            throw;
        }
     
    }
    protected void btnSubmitSubjects_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = Request.QueryString["studentId"];
            string modifiedBy = "admin"; // or fetch the current user/session

            if (string.IsNullOrEmpty(studentId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Student ID is missing.');", true);
                return;
            }
            if (!SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy, -1))
                return;
            
            // Process each Repeater
            SaveSelectedSubjects(rptCompulsorySubjects, studentId, modifiedBy,1);
            SaveSelectedSubjects(rptCompulsorySubjects2, studentId, modifiedBy,2);
           // SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy,-1);
            SaveSelectedSubjects(rptElectiveSubjects, studentId, modifiedBy,-1);
            SaveSelectedSubjects(rptAdditionalSubjects, studentId, modifiedBy,-1);
            
            string url = "StudentPhotoAndsignatureDetails.aspx?studentId=" + Server.UrlEncode(studentId.ToString());
            Response.Redirect(url, false);  // 'false' allows code to continue after redirect
            Context.ApplicationInstance.CompleteRequest();
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Subjects submitted successfully.');", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    private bool SaveSelectedSubjects(Repeater repeater, string studentId, string modifiedBy, int comgrp)
    {
        try
        {
            if (repeater.ID == "rptVocationalAdditionalSubjects")
            {
                bool vocationalSubjectSelected = false;

                
                foreach (RepeaterItem item in repeater.Items)
                {
                    CheckBox chkVocational = (CheckBox)item.FindControl("chkSubject");
                    if (chkVocational != null && chkVocational.Checked)
                    {
                        vocationalSubjectSelected = true;
                        break; // Exit the loop once a checkbox is selected
                    }
                }

                if (vocationalSubjectSelected)
                {
                    string collegeId = Session["CollegeId"].ToString();
                    string facultyId = Request.QueryString["FacultyId"];
                    const int vocationalLimit = 25;

                    // Count current students who opted for vocational subjects
                    int currentCount = dl.GetVocationalSubjectCount(facultyId, collegeId);
                    hfVocationalSubjectCount.Value = currentCount.ToString();

                    // Check if the limit is reached
                    if (currentCount >= vocationalLimit)
                    {
                        
                        string script = @"
                    alert('Only " + vocationalLimit + @" students can apply for vocational subjects in this faculty and college.');
                    document.querySelectorAll('.VocationalSubjects input[type=""checkbox""]').forEach(cb => {
                        cb.checked = false;
                    });
                    ";
                        ClientScript.RegisterStartupScript(this.GetType(), "VocationalLimitAlertAndUncheck", script, true);
                        return false;
                    }
                }
            }

            foreach (RepeaterItem item in repeater.Items)
            {
                if (repeater.ID == "rptElectiveSubjects")
                {
                    ProcessElectiveSubjects(studentId, modifiedBy, comgrp); // Remove item parameter
                    return true;
                }
                else if (repeater.ID == "rptAdditionalSubjects")
                {
                    // Handle Additional Subjects
                    ProcessAdditionalSubjects(item, studentId, modifiedBy, comgrp);
                }
                else 
                {
                    // Handle other subject groups
                    CheckBox chk = (CheckBox)item.FindControl("chkSubject");
                    if (chk != null && chk.Checked)
                    {
                        string subjectPaperCode = chk.Attributes["Value"];
                        DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectPaperCode);
                        if (subjectInfo.Rows.Count > 0)
                        {
                            string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                            string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();
                            dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            // Handle exception (you can log the error if needed)
            throw ex;
        }
    }

    private void ProcessElectiveSubjects(string studentId, string modifiedBy, int comgrp)
    {
        try
        {
            
            var selected1 = Request.Form.GetValues("chkElectiveSubjects") ?? new string[0];
            var selected2 = Request.Form.GetValues("chkGroup2Subjects") ?? new string[0];

         
            HashSet<string> uniqueSubjects = new HashSet<string>(selected1);
            foreach (string sub in selected2)
            {
                uniqueSubjects.Add(sub);
            }

         
            if (uniqueSubjects.Count != 3)
            {
                throw new Exception("Exactly 3 elective subjects must be selected.");
            }

            foreach (string subjectCode in uniqueSubjects)
            {
                SaveSubjectByCode(subjectCode, studentId, modifiedBy, comgrp);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ProcessAdditionalSubjects(RepeaterItem item, string studentId, string modifiedBy, int comgrp)
    {
        try
        {
            for (int i = 1; i <= 3; i++)
            {
                string chkId = "chkAdditional" + i;
                CheckBox chk = item.FindControl(chkId) as CheckBox;

                if (chk != null && chk.Checked)
                {
                    string code = chk.Attributes["Value"];
                    if (!string.IsNullOrEmpty(code))
                    {
                        SaveSubjectByCode(code, studentId, modifiedBy, comgrp);
                    }
                }
            }
        }
        catch (Exception ex)
        {

            throw;
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
                dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
            }

        }
        catch (Exception ex)
        {

            throw;
        }
   
    }
    private void ApplyPreviouslySelectedSubjects(string studentId)
    {
        try
        {
            // Fetch applied subject codes
            DataTable appliedSubjects = dl.GetAppliedSubjects(studentId); // You must implement this in DBHelper

            HashSet<string> selectedCodes = new HashSet<string>();
            foreach (DataRow row in appliedSubjects.Rows)
            {
                selectedCodes.Add(row["SubjectPaperCode"].ToString());
            }

            // Apply to all repeaters
            CheckRepeaterSubjects(rptCompulsorySubjects, selectedCodes);
            CheckRepeaterSubjects(rptCompulsorySubjects2, selectedCodes);
            CheckRepeaterSubjects(rptVocationalAdditionalSubjects, selectedCodes);
            CheckRepeaterSubjects(rptElectiveSubjects, selectedCodes);
            CheckRepeaterSubjects(rptAdditionalSubjects, selectedCodes);
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    private void CheckRepeaterSubjects(Repeater repeater, HashSet<string> selectedCodes)
    {
        try
        {
            foreach (RepeaterItem item in repeater.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSubject");
                if (chk != null && chk.Attributes["Value"] != null)
                {
                    string code = chk.Attributes["Value"];
                    if (selectedCodes.Contains(code))
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