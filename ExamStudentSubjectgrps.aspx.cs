using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExamStudentSubjectgrps : System.Web.UI.Page
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
                    string encryptedStudentId = Request.QueryString["studentId"];
                    string studentId = CryptoHelper.Decrypt(encryptedStudentId);
                   // string studentId = Request.QueryString["studentId"];
                    string FacultyId = Request.QueryString["FacultyId"];
                    hnd_FacultyId.Value = FacultyId;
                    string ExamTypeId = Request.QueryString["ExamTypeId"];
                    hnd_extype.Value = ExamTypeId;
                    Session["ExamTypeId"] = ExamTypeId.ToString();
                    string fromPage = Request.QueryString["from"];
                   
                    string collegeCode = Request.QueryString["collegeCode"];
                    string ExamCorrectionForm = Request.QueryString["ExamCorrectionForm"];
                    string StudentExamRegForm = Request.QueryString["StudentExamRegForm"];
                    hnd_ExamCorrectionForm.Value = ExamCorrectionForm;
                    hnd_StudentExamRegForm.Value = StudentExamRegForm;

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

                        string CollegeId = "";
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
                        DataTable AllSubjects = new DataTable();
                        DataView dvCompulsoryHindiEng = new DataView();
                        if (ExamTypeId == "2" || ExamTypeId == "4" || ExamTypeId == "6")
                        {
                            AllSubjects = dl.GetSubjectsForComp_Imp_Qual(Convert.ToInt32(FacultyId), Convert.ToInt32(CollegeId), Convert.ToInt32(studentId));

                            if (ExamTypeId == "2" || ExamTypeId == "4")
                            {
                                // ========== Compulsory: Hindi & English (ComGrp = 1)
                                dvCompulsoryHindiEng = new DataView(AllSubjects);
                                dvCompulsoryHindiEng.RowFilter = "GroupName = 'Compulsory' AND ComGrp = '1'";
                                rptCompulsorySubjects.DataSource = dvCompulsoryHindiEng;
                                rptCompulsorySubjects.DataBind();

                                if (ExamTypeId == "2")
                                {
                                    comp1title.Visible = rptCompulsorySubjects.Items.Count > 0;
                                    rptCompulsorySubjects.Visible = rptCompulsorySubjects.Items.Count > 0;
                                }
                               

                                // ========== Compulsory: Urdu or Other (ComGrp = 2)
                                DataView dvCompulsoryUrdu = new DataView(AllSubjects);
                                dvCompulsoryUrdu.RowFilter = "GroupName = 'Compulsory' AND ComGrp = '2'";
                                rptCompulsorySubjects2.DataSource = dvCompulsoryUrdu;
                                rptCompulsorySubjects2.DataBind();
                                if (ExamTypeId == "2")
                                {
                                    comp1title2.Visible = rptCompulsorySubjects2.Items.Count > 0;
                                    rptCompulsorySubjects2.Visible = rptCompulsorySubjects2.Items.Count > 0;
                                }
                                if (FacultyId == "4")
                                {
                                    bool hasPaperType = AllSubjects.Columns.Contains("PaperType");

                                    var vocElective = AllSubjects.AsEnumerable()
                                        .Where(r => r["GroupName"].ToString() == "Elective")
                                        .GroupBy(r =>
                                        {
                                            string paperType = "";
                                            if (hasPaperType)
                                            {
                                                paperType = r["PaperType"] == DBNull.Value ? "" : r["PaperType"].ToString();
                                            }
                                            return string.IsNullOrWhiteSpace(paperType) ? "Paper-1" : paperType;
                                        })
                                        .ToDictionary(g => g.Key, g => g.ToList());

                                    DataTable vocElectiveTable = new DataTable();
                                    vocElectiveTable.Columns.Add("Name1"); // Paper-1 Name
                                    vocElectiveTable.Columns.Add("Code1"); // Paper-1 Code
                                    vocElectiveTable.Columns.Add("Name2"); // Paper-2 Name
                                    vocElectiveTable.Columns.Add("Code2"); // Paper-2 Code

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
                                    divAdditionalSubjects.Visible = false;
                                    //rptVocElectiveSubjects.DataSource = vocElectiveTable;
                                    //rptVocElectiveSubjects.DataBind();

                                    //// Keep the same visibility logic you used but make sure parent section is visible
                                    //VocElectiveSection.Visible = true;
                                    //rptVocElectiveSubjects.Visible = rptVocElectiveSubjects.Items.Count > 0;

                                    //// Hide other irrelevant sections as your original code did
                                    //ElectiveSection.Visible = false;
                                    //ElectiveCard.Visible = false;
                                    //divAdditionalSubjects.Visible = false;
                                    //ElectiveSection.Visible = false;
                                    //VocElectiveSection.Visible = true;
                                    //if (ExamTypeId == "2")
                                    //{


                                    //    ElectiveCard.Visible = rptElectiveSubjects.Items.Count > 0;
                                    //    VocElectiveSection.Visible = true;
                                    //    divAdditionalSubjects.Visible = false;
                                    //}
                                }


                                //if (FacultyId == "4")
                                //{
                                //    var vocElective = AllSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Elective")
                                //     .GroupBy(r =>
                                //     {
                                //         var paperType = r["PaperType"] == DBNull.Value ? "" : r["PaperType"].ToString();
                                //         return string.IsNullOrWhiteSpace(paperType) ? "Paper-1" : paperType;
                                //     }).ToDictionary(g => g.Key, g => g.ToList());

                                //    DataTable vocElectiveTable = new DataTable();
                                //    vocElectiveTable.Columns.Add("Name1"); // Paper-1 Name
                                //    vocElectiveTable.Columns.Add("Code1"); // Paper-1 Code
                                //    vocElectiveTable.Columns.Add("Name2"); // Paper-2 Name
                                //    vocElectiveTable.Columns.Add("Code2"); // Paper-2 Code

                                //    // Get lists for Paper-1 and Paper-2
                                //    var paper1 = vocElective.ContainsKey("Paper-1") ? vocElective["Paper-1"] : new List<DataRow>();
                                //    var paper2 = vocElective.ContainsKey("Paper-2") ? vocElective["Paper-2"] : new List<DataRow>();

                                //    int maxCount = Math.Max(paper1.Count, paper2.Count);

                                //    for (int i = 0; i < maxCount; i++)
                                //    {
                                //        DataRow newRow = vocElectiveTable.NewRow();

                                //        if (i < paper1.Count)
                                //        {
                                //            newRow["Name1"] = paper1[i]["SubjectPaperName"];
                                //            newRow["Code1"] = paper1[i]["SubjectPaperCode"];
                                //        }

                                //        if (i < paper2.Count)
                                //        {
                                //            newRow["Name2"] = paper2[i]["SubjectPaperName"];
                                //            newRow["Code2"] = paper2[i]["SubjectPaperCode"];
                                //        }

                                //        vocElectiveTable.Rows.Add(newRow);
                                //    }

                                //    rptVocElectiveSubjects.DataSource = vocElectiveTable;
                                //    rptVocElectiveSubjects.DataBind();

                                //    ElectiveSection.Visible = false;
                                //    VocElectiveSection.Visible = true;

                                //}
                                else
                                {
                                    var electiveRows = AllSubjects.AsEnumerable().Where(row => row.Field<string>("ComGrp") == "3").ToList();

                                    DataTable electiveReshaped = new DataTable();
                                    electiveReshaped.Columns.Add("Name1");
                                    electiveReshaped.Columns.Add("Code1");
                                    electiveReshaped.Columns.Add("Name2");
                                    electiveReshaped.Columns.Add("Code2");

                                    for (int i = 0; i < electiveRows.Count; i += 2)
                                    {
                                        DataRow newRow = electiveReshaped.NewRow();
                                        newRow["Name1"] = electiveRows[i]["SubjectPaperName"];
                                        newRow["Code1"] = electiveRows[i]["SubjectPaperCode"];
                                        if (i + 1 < electiveRows.Count)
                                        {
                                            newRow["Name2"] = electiveRows[i + 1]["SubjectPaperName"];
                                            newRow["Code2"] = electiveRows[i + 1]["SubjectPaperCode"];
                                        }
                                        electiveReshaped.Rows.Add(newRow);
                                    }

                                    rptElectiveSubjects.DataSource = electiveReshaped;
                                    rptElectiveSubjects.DataBind();
                                    ViewState["ElectiveSubjects"] = electiveReshaped;
                                    VocElectiveSection.Visible = false;

                                    if (ExamTypeId == "2")
                                    {
                                        ElectiveCard.Visible = rptElectiveSubjects.Items.Count > 0;
                                        VocElectiveSection.Visible = false;
                                        divAdditionalSubjects.Visible = false;

                                        Elective1title.Visible = rptElectiveSubjects.Items.Count > 0;
                                        Elective1title2.Visible = rptElectiveSubjects.Items.Count > 0;
                                        rptElectiveSubjects.Visible = rptElectiveSubjects.Items.Count > 0;
                                    }

                                }
                                // ========== Elective (GroupName = 'Elective')
                              
                                //if (ExamTypeId == "2")
                                //{
                                //    ElectiveCard.Visible = rptElectiveSubjects.Items.Count > 0;
                                //    VocElectiveSection.Visible = false;
                                //    divAdditionalSubjects.Visible = false;

                                //    Elective1title.Visible = rptElectiveSubjects.Items.Count > 0;
                                //    Elective1title2.Visible = rptElectiveSubjects.Items.Count > 0;
                                //    rptElectiveSubjects.Visible = rptElectiveSubjects.Items.Count > 0;
                                //}

                            }
                            else
                            {
                                // ExamTypeId == "6" → hide Compulsory + Elective sections
                                rptCompulsorySubjects.Visible = false;
                                rptCompulsorySubjects2.Visible = false;
                                comp1title.Visible = false;
                                comp1title2.Visible = false;
                               

                               // rptElectiveSubjects.Visible = false;
                                //Elective1title.Visible = false;
                                //Elective1title2.Visible = false;
                                ElectiveCard.Visible = false;
                                divVocationalSubjects.Visible = false;
                               // VocElectiveSection.Visible = false;
                            }
                            if (ExamTypeId == "4" || ExamTypeId =="6")
                            {
                                // ========== Additional Subjects (3-column layout)
                                var additionalRows = AllSubjects.AsEnumerable().Where(row => row.Field<string>("ComGrp") == "4").ToList();
                            if (additionalRows.Any())
                            {
                                DataTable additional = additionalRows.CopyToDataTable();
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
                                divAdditionalSubjects.Visible = true;
                                Addi1title.Visible = rptAdditionalSubjects.Items.Count > 0;
                                Add2title.Visible = rptAdditionalSubjects.Items.Count > 0;
                                divAdditionalSubjects.Visible = rptAdditionalSubjects.Items.Count > 0;
                            }
                            else
                            {
                                divAdditionalSubjects.Visible = false;
                            }

                            // ========== Vocational Additional
                            //if (ExamTypeId == "4")
                            //{
                                BindRepeater(AllSubjects, "Vocational Additional", rptVocationalAdditionalSubjects);
                            }
                         
                            bool hasImproved = AllSubjects.AsEnumerable().Any(row => row.Field<bool?>("Improved") == true);

                            if (ExamTypeId != "4")
                            {
                                //ApplyPreviouslySelectedSubjects(studentId, "0");
                                if (ExamTypeId == "1" && ExamCorrectionForm == "ExamCorrectionForm")
                                {
                                    ApplyPreviouslySelectedSubjects(studentId, "1");
                                }
                                else
                                {
                                    ApplyPreviouslySelectedSubjects(studentId, ExamTypeId);
                                    //ApplyPreviouslySelectedSubjects(studentId, "0");
                                }
                            }
                            else if (ExamTypeId == "4" && hasImproved)
                            {
                                ApplyPreviouslySelectedSubjects(studentId, ExamTypeId);
                            }
                        }

                        else if (ExamTypeId == "1" || ExamTypeId == "3" || ExamTypeId == "5")
                        {
                            //if (ExamTypeId == "1" || ExamTypeId == "5")
                            //{
                                AllSubjects = dl.GetExamSubjectsByGroup(Convert.ToInt32(FacultyId), Convert.ToInt32(CollegeId),Convert.ToInt32(studentId));
                           
                                //AllSubjects = dl.GetExamSubjectsByGroup(Convert.ToInt32(FacultyId), Convert.ToInt32(CollegeId), 0, Convert.ToInt32(studentId));
                            //}
                            //else
                            //{
                            //    AllSubjects = dl.GetExamSubjectsByGroup(Convert.ToInt32(FacultyId), Convert.ToInt32(CollegeId), 3, Convert.ToInt32(studentId));
                            //}
                            dvCompulsoryHindiEng = new DataView(AllSubjects);
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
                            var additionalRows = AllSubjects.AsEnumerable().Where(r => r["GroupName"].ToString() == "Additional");

                            if (additionalRows.Any())
                            {
                                DataTable additional = additionalRows.CopyToDataTable();

                                DataTable reshaped = new DataTable();
                                for (int i = 1; i <= 3; i++)
                                {
                                    reshaped.Columns.Add("Name" + i);
                                    reshaped.Columns.Add("Code" + i);
                                    //reshaped.Columns.Add("PaperId" + i);
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
                                            //newRow["PaperId" + (j + 1)] = additional.Rows[i + j]["Pk_SubjectPaperId"];
                                        }
                                    }
                                    reshaped.Rows.Add(newRow);
                                }

                                rptAdditionalSubjects.DataSource = reshaped;
                                rptAdditionalSubjects.DataBind();
                            }
                            else
                            {
                                rptAdditionalSubjects.DataSource = null;
                                rptAdditionalSubjects.DataBind();
                                divAdditionalSubjects.Visible = false;
                            }






                            BindRepeater(AllSubjects, "Compulsory", rptCompulsorySubjects2);
                            BindRepeater(AllSubjects, "Vocational Additional", rptVocationalAdditionalSubjects);

                           
                            //if (ExamTypeId != "3" || ExamTypeId != "4")
                            if (ExamTypeId != "4")
                            {
                               
                                if (ExamTypeId == "1" && ExamCorrectionForm == "ExamCorrectionForm")
                                {
                                    ApplyPreviouslySelectedSubjects(studentId, "1");
                                }
                                else
                                {
                                    ApplyPreviouslySelectedSubjects(studentId, ExamTypeId);
                                }
                                //if (ExamTypeId == "3")
                                //{

                                //}
                                //ViewState["IsLocked"] = true;
                            }



                        }

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
                div_vocational.Visible = rptVocationalAdditionalSubjects.Items.Count > 0;
                Borderline_vocational.Visible = rptVocationalAdditionalSubjects.Items.Count > 0;
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
            //string studentId = Request.QueryString["studentId"];
            string encryptedStudentId = Request.QueryString["studentId"];
            string studentId = CryptoHelper.Decrypt(encryptedStudentId);
            string ExamTypeId = Request.QueryString["ExamTypeId"];
            string modifiedBy = Session["CollegeId"].ToString();
            string FacultyId = Request.QueryString["FacultyId"];
            if (string.IsNullOrEmpty(studentId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Student ID is missing.');", true);
                return;
            }
            LoadComGrpMap(studentId);
            if (!SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy, 5, Convert.ToInt32(ExamTypeId)))
                return;
            //if (facultyid == "4")
            //{
            //    saveselectedsubjects(rptvocelectivesubjects, studentid, modifiedby, 3, convert.toint32(examtypeid));
            //}
            if (FacultyId == "4")
            {
                SaveSelectedSubjects(rptVocElectiveSubjects, studentId, modifiedBy, 3, Convert.ToInt32(ExamTypeId));
            }
            // Process each Repeater
            SaveSelectedSubjects(rptCompulsorySubjects, studentId, modifiedBy, 1, Convert.ToInt32(ExamTypeId));
            SaveSelectedSubjects(rptCompulsorySubjects2, studentId, modifiedBy, 2, Convert.ToInt32(ExamTypeId));
            // SaveSelectedSubjects(rptVocationalAdditionalSubjects, studentId, modifiedBy,-1);
            SaveSelectedSubjects(rptElectiveSubjects, studentId, modifiedBy, 3, Convert.ToInt32(ExamTypeId));
            SaveSelectedSubjects(rptAdditionalSubjects, studentId, modifiedBy, 4, Convert.ToInt32(ExamTypeId));

            //string encryptedStudentId = CryptoHelper.Encrypt(studentId);
            string script = @"
                    <script>
                        Swal.fire({
                            icon: 'success',
                            title: 'Subjects Submitted',
                            text: 'Subjects submitted successfully!',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = 'StudentPhotoAndsignatureDetails.aspx?studentId=" + Server.UrlEncode(encryptedStudentId.ToString()) + "&ExamTypeId=" + Server.UrlEncode(ExamTypeId.ToString()) + @"';
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
    private bool SaveSelectedSubjects(Repeater repeater, string studentId, string modifiedBy, int comgrp, int examTypeId)
    {
        try
        {
            if (repeater.ID == "rptVocationalAdditionalSubjects")
            {
                HashSet<string> selectedCodes = new HashSet<string>();
                bool vocationalSubjectSelected = false;
                DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

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

                // Optional: Handle college ID logic (unchanged)
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
                }

                // ✅ Handle UPDATE/DELETE logic
                foreach (DataRow row in existingSubjects.Rows)
                {
                    string existingCode = row["SubjectPaperCode"].ToString();
                    int pkId = Convert.ToInt32(row["Pk_StudentPaperAppliedId"]);

                    if (examTypeId == 4)
                    {
                        if (selectedCodes.Contains(existingCode))
                        {
                            DataTable subjectInfo = dl.GetSubjectDetailsByCode(existingCode);
                            if (subjectInfo.Rows.Count > 0)
                            {
                                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                                string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();
                                dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                            }
                        }
                        else
                        {
                            dl.UpdateImprovmentStudentPaperAppliedById(pkId, 0);
                        }
                    }
                    else
                    {
                        if (!selectedCodes.Contains(existingCode))
                        {
                            dl.DeleteStudentPaperAppliedById(pkId);
                        }
                    }
                }

                // ✅ Insert new only for examTypeId = 1,3,5
                if (examTypeId == 1 || examTypeId == 3 || examTypeId == 5)
                {
                    foreach (string code in selectedCodes)
                    {
                        bool alreadySaved = existingSubjects.AsEnumerable()
                            .Any(r => r["SubjectPaperCode"].ToString() == code);

                        if (!alreadySaved)
                        {
                            SaveSubjectByCode(code, studentId, modifiedBy, comgrp, examTypeId);
                        }
                    }
                }

                return true;
            }

            foreach (RepeaterItem item in repeater.Items)
            {
                if (repeater.ID == "rptElectiveSubjects")
                {
                    ProcessElectiveSubjects(studentId, modifiedBy, comgrp, examTypeId);
                    return true;
                }
                else if (repeater.ID == "rptVocElectiveSubjects")
                {
                    ProcessVocElectiveSubjects(studentId, modifiedBy, comgrp, examTypeId);
                    return true;
                }
                else if (repeater.ID == "rptAdditionalSubjects")
                {
                    ProcessAdditionalSubjects(item, studentId, modifiedBy, comgrp, examTypeId);
                }
                else
                {
                    // ✅ FINAL ELSE LOGIC HANDLING
                    CheckBox chk = (CheckBox)item.FindControl("chkSubject");
                    if (chk != null)
                    {
                        string subjectPaperCode = chk.Attributes["Value"];
                        bool isChecked = chk.Checked;

                        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
                        DataRow existingRow = existingSubjects.AsEnumerable()
                            .FirstOrDefault(r => r["SubjectPaperCode"].ToString() == subjectPaperCode);

                        if (examTypeId == 4)
                        {
                            if (existingRow != null)
                            {
                                int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);

                                if (isChecked)
                                {
                                    // ✅ If selected, update as improvement
                                    DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectPaperCode);
                                    if (subjectInfo.Rows.Count > 0)
                                    {
                                        string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                                        string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();
                                        dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                                    }
                                }
                                else
                                {
                                    // ✅ If not selected, mark not improved
                                    dl.UpdateImprovmentStudentPaperAppliedById(pkId, 0);
                                }
                            }
                            else if (isChecked)
                            {
                                // ✅ If not already saved, and selected, insert
                                SaveSubjectByCode(subjectPaperCode, studentId, modifiedBy, comgrp, examTypeId);
                            }
                        }
                        else
                        {
                            // ✅ Default logic for examTypeId 1/3/5
                            if (isChecked)
                            {
                                SaveSubjectByCode(subjectPaperCode, studentId, modifiedBy, comgrp, examTypeId);
                            }
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    private void ProcessElectiveSubjects(string studentId, string modifiedBy, int comgrp, int examTypeId)
    {
        var selected1 = Request.Form.GetValues("chkElectiveSubjects") ?? new string[0];
        var selected2 = Request.Form.GetValues("chkGroup2Subjects") ?? new string[0];
        string ExamCorrectionForm = Request.QueryString["ExamCorrectionForm"];
        List<string> selectedCodes = new List<string>(selected1);
        selectedCodes.AddRange(selected2);

        // ✅ 1. Validate only if examTypeId != 4
        //if (examTypeId != 4 && selectedCodes.Count != 3)
        if (examTypeId ==3 && selectedCodes.Count != 3)
        {
            throw new Exception("Exactly 3 elective subjects must be selected.");
        }
        //else if (examTypeId ==1 && selectedCodes.Count != 3 && ExamCorrectionForm == "ExamCorrectionForm")
        //{
        //    throw new Exception("Exactly 3 elective subjects must be selected.");
        //}

        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

        // ✅ 2. Insert missing subjects
        //while (existingSubjects.Rows.Count < selectedCodes.Count)
        while (existingSubjects.Rows.Count < selectedCodes.Count)
        {
            int index = existingSubjects.Rows.Count;
            SaveSubjectByCode(selectedCodes[index], studentId, modifiedBy, comgrp, examTypeId);

            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }


        // ✅ 3. DELETE ONLY if examTypeId != 4
        if (examTypeId != 4)
        {
            while (existingSubjects.Rows.Count > selectedCodes.Count)
            {
                int pkId = Convert.ToInt32(existingSubjects.Rows[0]["Pk_StudentPaperAppliedId"]);
                dl.DeleteStudentPaperAppliedById(pkId);
                existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
            }
        }

        // ✅ 4. Update or improvement logic
        for (int i = 0; i < existingSubjects.Rows.Count; i++)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[i]["Pk_StudentPaperAppliedId"]);
            string existingSubjectCode = existingSubjects.Rows[i]["SubjectPaperCode"].ToString(); // adjust this column name as needed

            if (examTypeId == 4)
            {
                // For improvement exam, check if subject exists in selected codes
                if (selectedCodes.Contains(existingSubjectCode))
                {
                    DataTable subjectInfo = dl.GetSubjectDetailsByCode(existingSubjectCode);
                    if (subjectInfo.Rows.Count > 0)
                    {
                        string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                        string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                        dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                    }
                }
                else
                {
                    // Not selected, mark improvement = 0
                    dl.UpdateImprovmentStudentPaperAppliedById(pkId, 0);
                }
            }
            else
            {
                // For regular exams (1,3,5) use update method
                if (i < selectedCodes.Count)
                {
                    string subjectCode = selectedCodes[i];
                    DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectCode);
                    if (subjectInfo.Rows.Count > 0)
                    {
                        string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                        string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                        dl.ExamUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                    }
                }
            }
        }
    }

    private void ProcessVocElectiveSubjects(string studentId, string modifiedBy, int comgrp, int examTypeId)
    {
        var selected1 = Request.Form.GetValues("chkVocElectiveSubjects") ?? new string[0];
        var selected2 = Request.Form.GetValues("chkGroup2Subjects") ?? new string[0];
        string ExamCorrectionForm = Request.QueryString["ExamCorrectionForm"];

        List<string> selectedCodes = new List<string>(selected1);
        selectedCodes.AddRange(selected2);

        // ✅ 1. Validate only if examTypeId != 4
        if (examTypeId != 4 && selectedCodes.Count != 3)
        {
            throw new Exception("Exactly 3 elective subjects must be selected.");
        }

        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

        // ✅ 2. Insert missing subjects
        //while (existingSubjects.Rows.Count < selectedCodes.Count)
        //{
        //    SaveSubjectByCode(selectedCodes[existingSubjects.Rows.Count], studentId, modifiedBy, comgrp, examTypeId);
        //    existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        //}
        while (existingSubjects.Rows.Count < 3)
        {
            int index = existingSubjects.Rows.Count;
            SaveSubjectByCode(selectedCodes[index], studentId, modifiedBy, comgrp, examTypeId);
            //SaveSubjectByCode(selectedCodes[existingSubjects.Rows.Count], studentId, modifiedBy, comgrp, examTypeId);
            existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
        }
        // ✅ 3. DELETE ONLY if examTypeId != 4
        //if (examTypeId != 4 || examTypeId != 1)
        if (examTypeId != 4)
        {
            while (existingSubjects.Rows.Count > 3)
            {
                int pkId = Convert.ToInt32(existingSubjects.Rows[0]["Pk_StudentPaperAppliedId"]);
                dl.DeleteStudentPaperAppliedById(pkId);
                existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);
            }
        }

        // ✅ 4. Update or improvement logic
        for (int i = 0; i < existingSubjects.Rows.Count; i++)
        {
            int pkId = Convert.ToInt32(existingSubjects.Rows[i]["Pk_StudentPaperAppliedId"]);
            string existingSubjectCode = existingSubjects.Rows[i]["SubjectPaperCode"].ToString(); // adjust this column name as needed

            if (examTypeId == 4)
            {
                // For improvement exam, check if subject exists in selected codes
                if (selectedCodes.Contains(existingSubjectCode))
                {
                    DataTable subjectInfo = dl.GetSubjectDetailsByCode(existingSubjectCode);
                    if (subjectInfo.Rows.Count > 0)
                    {
                        string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                        string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                        dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                    }
                }
                else
                {
                    // Not selected, mark improvement = 0
                    dl.UpdateImprovmentStudentPaperAppliedById(pkId, 0);
                }
            }
            else
            {
                // For regular exams (1,3,5) use update method
                if (i < selectedCodes.Count)
                {
                    string subjectCode = selectedCodes[i];
                    DataTable subjectInfo = dl.GetSubjectDetailsByCode(subjectCode);
                    if (subjectInfo.Rows.Count > 0)
                    {
                        string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
                        string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                        dl.ExamUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                    }
                }
            }
        }
    }

    private void ProcessAdditionalSubjects(RepeaterItem item, string studentId, string modifiedBy, int comgrp, int examTypeId)
    {
        DataTable existingSubjects = dl.GetSubjectsByStudentIdAndComGrp(studentId, comgrp);

        for (int i = 1; i <= 3; i++)
        {
            string chkId = "chkAdditional" + i;
            string hfId = "hfCode" + i;
            //string hfPaperIdId = "hfPaperId" + i;

            CheckBox chk = item.FindControl(chkId) as CheckBox;
            HiddenField hfCode = item.FindControl(hfId) as HiddenField;
            //HiddenField hfPaperId = item.FindControl(hfPaperIdId) as HiddenField;
            if (chk != null && hfCode != null)
            {
                string code = hfCode.Value;
                bool isChecked = chk.Checked;
               // string paperId = hfPaperId.Value;
                if (string.IsNullOrEmpty(code))
                    continue;

                DataTable subjectInfo = dl.GetSubjectDetailsByCode(code);
                if (string.IsNullOrEmpty(code))
                    continue;

                string subjectPaperId = subjectInfo.Rows[0]["SubjectPaperId"].ToString();
              //  string subjectGroupId = subjectInfo.Rows[0]["SubjectGroupId"].ToString();

                DataRow existingRow = existingSubjects.AsEnumerable()
                    .FirstOrDefault(r => r["SubjectPaperCode"].ToString() == code);
                string subjectGroupId = "3";
                if (examTypeId == 4)
                {
                    // ✅ IMPROVEMENT Exam Logic
                    if (existingRow != null)
                    {
                        int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);

                        if (isChecked)
                        {
                            dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                        }
                        else
                        {
                            dl.UpdateImprovmentStudentPaperAppliedById(pkId, 0);
                        }
                    }
                    else if (isChecked)
                    {
                        // ✅ If not saved yet, and selected, insert as new
                        //subjectGroupId = "3";
                        dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
                    }
                }
                else if (examTypeId == 1 || examTypeId == 3 || examTypeId == 5)
                {
                    if (isChecked)
                    {
                        if (existingRow == null)
                        {
                            //subjectGroupId = "3"; // default group for new inserts
                            dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
                        }
                        else
                        {
                            int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);
                            dl.ExamUpdateStudentSubjectByPkId(pkId.ToString(), subjectPaperId, subjectGroupId, modifiedBy);
                        }
                    }
                    else
                    {
                        if (existingRow != null)
                        {
                            int pkId = Convert.ToInt32(existingRow["Pk_StudentPaperAppliedId"]);
                            dl.DeleteStudentPaperAppliedById(pkId);
                        }
                    }
                }
            }
        }
    }


    private void SaveSubjectByCode(string subjectPaperCode, string studentId, string modifiedBy, int comgrp, int examTypeId)
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
                    if (examTypeId == 4)
                    {
                        dl.ExamImprovemnetUpdateStudentSubjectByPkId(pkId, subjectPaperId, subjectGroupId, modifiedBy);
                    }
                    else
                    {
                        dl.ExamUpdateStudentSubjectByPkId(pkId, subjectPaperId, subjectGroupId, modifiedBy);
                    }
                }
                else
                {
                    dl.InsertStudentSubjectUsingSP(studentId, subjectPaperId, subjectGroupId, modifiedBy, comgrp);
                }
            }
        }
        catch (Exception ex)
        {
            throw; // Keep default exception throw
        }
    }


    //Selectedchekbox logic 
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
    private void ApplyPreviouslySelectedSubjects(string studentId,string ExamTypeId)
    {
        try
        {
            DataTable appliedSubjects = new DataTable();
            string FacultyId = Request.QueryString["FacultyId"];
            bool hasExistingSubjects = false;
            if (ExamTypeId !="4")
            {
                 appliedSubjects = dl.GetAppliedSubjects(studentId);
            }
            else
            {
                appliedSubjects = dl.GetExamImporovedAppliedSubjects(studentId);
            }

            if (appliedSubjects != null && appliedSubjects.Rows.Count > 0)
            {
                hasExistingSubjects = true;
            }

            // Store the locked status in ViewState for the front-end
            // ViewState["IsLocked"] = hasExistingSubjects.ToString();
            // If ExamTypeId = 1 → Do NOT lock subjects
            string ExamCorrectionForm = Request.QueryString["ExamCorrectionForm"];

            if (ExamTypeId == "1" && ExamCorrectionForm == "ExamCorrectionForm")
            {
                ViewState["IsLocked"] = "False";
            }
            else if (FacultyId == "4")
            {
                ViewState["IsLocked"] = hasExistingSubjects.ToString();

            }
            else
            {
                ViewState["IsLocked"] = hasExistingSubjects.ToString();
            }

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
            if (FacultyId == "4")
            {

                ApplyToVocElectiveSubjects(rptVocElectiveSubjects, groupWiseCodes, "3");
            }
            else
            {
                ApplyToElectiveSubjects(rptElectiveSubjects, groupWiseCodes, "3");
            }
                ApplyToCompulsorySubjects(rptCompulsorySubjects, groupWiseCodes, "1");
            ApplyToCompulsorySubjects(rptCompulsorySubjects2, groupWiseCodes, "2");
            
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