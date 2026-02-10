using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class TheoryAdmitCertificate : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();            

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(TheoryAdmitCertificate));
    private static bool isLog4NetConfigured = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        EnsureLogFolder();
        EnsureLog4NetConfigured();

        if (!IsPostBack)
        {
            log.Info("Page_Load started");
            try
            {
                string encodedStudentData = Request.QueryString["studentData"];
                string Studentname = Request.QueryString["Studentname"];
                string Collegecode = Request.QueryString["Collegecode"];
                string FacultyId = Request.QueryString["FacultyId"];
                string DOB = Request.QueryString["Dob"];
                string fromPage = Request.QueryString["from"];
                //string examtypid = Request.QueryString["examTypeId"];

                log.Debug(string.Format("Query Params: studentData={0}, Studentname={1}, Collegecode={2}, FacultyId={3}, DOB={4}, from={5}",
                    encodedStudentData, Studentname, Collegecode, FacultyId, DOB, fromPage));

                if (!string.IsNullOrEmpty(encodedStudentData))
                {
                    string decodedStudentData = Server.UrlDecode(encodedStudentData);
                    List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    log.Info(string.Format("Decoded student data: {0}, Count: {1}", decodedStudentData, selectedStudents.Count));

                    DataTable finalStudentData = CreateCombinedStudentDataTableSchema();

                    foreach (string studentEntry in selectedStudents)
                    {
                        string[] parts = studentEntry.Split('|');
                        if (parts.Length == 4)
                        {
                            string studentIdStr = parts[0];
                            string collegeId = parts[1];
                            string facultyId = parts[2];
                            string examtypid = parts[3];

                            int studentId;
                            if (int.TryParse(studentIdStr, out studentId))
                            {
                                log.Debug(string.Format("Processing studentId={0}, collegeId={1}, facultyId={2}", studentId, collegeId, facultyId));

                                DataTable dtPersonalDetails = dl.GetStudentDummyExamCertificateData(studentId, Convert.ToInt32(collegeId), Convert.ToInt32(facultyId));
                                DataTable dtSubjectsForCurrentStudent = dl.GetStudentExamAddmitCardSubjectDetails(studentId, Convert.ToInt32(collegeId), Convert.ToInt32(facultyId), Convert.ToInt32(examtypid), false);
                                log.Debug("finalStudentData  " + dtPersonalDetails);
                                if (dtPersonalDetails.Rows.Count > 0)
                                {
                                    DataRow studentRow = dtPersonalDetails.Rows[0];
                                    DataRow combinedRow = finalStudentData.NewRow();

                                    //foreach (DataColumn col in dtPersonalDetails.Columns)
                                    //{
                                    //    if (finalStudentData.Columns.Contains(col.ColumnName))
                                    //    {
                                    //        combinedRow[col.ColumnName] = studentRow[col.ColumnName] == DBNull.Value ? DBNull.Value : studentRow[col.ColumnName];
                                    //    }
                                    //}
                                    foreach (DataColumn col in dtPersonalDetails.Columns)
                                    {
                                        if (finalStudentData.Columns.Contains(col.ColumnName))
                                        {
                                            if (col.ColumnName == "StudentPhotoPath")
                                            {
                                                string photoFileName = studentRow[col] != DBNull.Value ? studentRow[col].ToString() : "";

                                                combinedRow["StudentPhotoPath"] = string.IsNullOrEmpty(photoFileName) ? (object)DBNull.Value : "~/Uploads/StudentsReg/Photos/" + photoFileName;
                                            }
                                            else if (col.ColumnName == "StudentSignaturePath")
                                            {
                                                string signatureFileName = studentRow[col] != DBNull.Value ? studentRow[col].ToString() : "";

                                                combinedRow["StudentSignaturePath"] = string.IsNullOrEmpty(signatureFileName) ? (object)DBNull.Value : "~/Uploads/StudentsReg/Signatures/" + signatureFileName;
                                            }
                                            else
                                            {

                                                combinedRow[col.ColumnName] = studentRow[col] == DBNull.Value ? DBNull.Value : studentRow[col];
                                            }
                                            //if (col.ColumnName == "DOB")
                                            //{
                                            //    if (studentRow[col] != DBNull.Value)
                                            //    {
                                            //        DateTime dob = Convert.ToDateTime(studentRow[col]);
                                            //        combinedRow[col.ColumnName] = dob.ToString("MM/dd/yyyy");
                                            //        log.Debug("DOB  " + combinedRow[col.ColumnName]);
                                            //    }
                                            //    else
                                            //    {
                                            //        combinedRow[col.ColumnName] = DBNull.Value;
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //   combinedRow[col.ColumnName] = studentRow[col] == DBNull.Value ? DBNull.Value : studentRow[col];
                                            //}
                                        }
                                    }

                                    bool downloadUpdateSuccess = btnDownloadUpdate_Click(studentIdStr);
                                    if (!downloadUpdateSuccess)
                                    {
                                        log.Warn("Failed to update download status for studentId=" + studentIdStr);
                                    }
                                    CategorizeAndPopulateSubjects(dtSubjectsForCurrentStudent, combinedRow, Convert.ToInt32(facultyId),collegeId);

                                    finalStudentData.Rows.Add(combinedRow);
                                }
                                else
                                {
                                    log.Warn(string.Format("No personal details found for studentId={0}", studentId));
                                }
                            }
                            else
                            {
                                log.Warn(string.Format("Invalid studentId in entry: {0}", studentEntry));
                            }
                        }
                        else
                        {
                            log.Warn(string.Format("Invalid student entry format: {0}", studentEntry));
                        }
                    }

                    if (finalStudentData.Rows.Count > 0)
                    {
                        rptStudents.DataSource = finalStudentData;
                        rptStudents.DataBind();
                        log.Info(string.Format("Bound {0} student rows to repeater.", finalStudentData.Rows.Count));
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error in Page_Load", ex);
                throw;
            }
        }
    }
    protected void rptStudents_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Get controls
            HiddenField hf = (HiddenField)e.Item.FindControl("hfFacultyId");
            Label lblFacultyHindi = (Label)e.Item.FindControl("lblFacultyHindi");
            PlaceHolder phFaculty = (PlaceHolder)e.Item.FindControl("phFaculty");
            Label lblExamTitle = (Label)e.Item.FindControl("lblExamTitle");
            Label lblExamTitleHindi = (Label)e.Item.FindControl("lblExamTitleHindi");
            Label lblExamSubjectHindi = (Label)e.Item.FindControl("lblExamSubjectHindi");
            Label lblExamSchoolHindi = (Label)e.Item.FindControl("lblExamSchoolHindi");
            HtmlTableRow trVocational = (HtmlTableRow)e.Item.FindControl("trVocational");

            Label lblCollegeName = e.Item.FindControl("lblCollegeName") as Label;
            // Get FacultyName
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string facultyName = drv["FacultyName"].ToString().Trim().ToUpper();
            string ExamTypeName = drv["ExamTypeName"].ToString().Trim().ToUpper();
            bool hasVocational = drv["HasVocationalSubjects"] != DBNull.Value && Convert.ToBoolean(drv["HasVocationalSubjects"]);


            HtmlTableCell tdElective1 = e.Item.FindControl("tdElective1") as HtmlTableCell;
            HtmlTableCell tdElective2 = e.Item.FindControl("tdElective2") as HtmlTableCell;
            HtmlTableCell tdElective3 = e.Item.FindControl("tdElective3") as HtmlTableCell;
            HtmlTableCell tdAdditionalHeader = e.Item.FindControl("tdAdditionalHeader") as HtmlTableCell;
            // Map to Hindi
            string hindiFaculty = string.Empty;

            switch (facultyName)
            {
                case "ARTS":
                    hindiFaculty = "संकाय: <strong>कला</strong>";
                    break;
                case "SCIENCE":
                    hindiFaculty = "संकाय: <strong>विज्ञान</strong>";
                    break;
                case "COMMERCE":
                    hindiFaculty = "संकाय: <strong>वाणिज्य</strong>";
                    break;
            }
            if (ExamTypeName == "QUALIFYING")
            {
                //
                tdAdditionalHeader.InnerHtml = "Qualifying Subject ";
            }
            else
            {
                tdAdditionalHeader.InnerHtml = "अतिरिक्त विषय(Additional Subject)";
            }
            // Show/hide section
            if (facultyName == "VOCATIONAL")
            {
                phFaculty.Visible = false;
                lblExamTitle.Text = "INTERMEDIATE ANNUAL (VOCATIONAL COURSE) EXAMINATION, 2026";
                lblExamTitleHindi.Text = "इंटरमीडिएट वार्षिक (व्यावसायिक पाठ्यक्रम) परीक्षा, 2026";
                lblExamSubjectHindi.Text = "सैद्धान्तिक वार्षिक (व्यावसायिक पाठ्यक्रम) परीक्षा के विषय (निर्धारित परीक्षा कार्यक्रम सहित)";
                lblCollegeName.Text = "+2 स्कूल का नाम";

                if (tdElective1 != null)
                    tdElective1.InnerHtml = "ऐच्छिक विषय-1 <br /> (फाउंडेशन कोर्स )";

                if (tdElective2 != null)
                    tdElective2.InnerHtml = "ऐच्छिक विषय-2 <br /> (व्यावसायिक शिक्षा ट्रेड पत्र-I )";

                if (tdElective3 != null)
                    tdElective3.InnerHtml = "ऐच्छिक विषय-3 <br /> (व्यावसायिक शिक्षा ट्रेड पत्र-II )";
                lblExamSchoolHindi.Text = "+2 विद्यालय प्रधान का हस्ताक्षर एवं मुहर";
                trVocational.Visible = hasVocational;
            }
            else if (ExamTypeName == "COMPARTMENTAL")
            {
                lblExamTitle.Text = "INTERMEDIATE COMPARTMENTAL EXAMINATION, 2026";
                lblExamTitleHindi.Text = "इंटरमीडिएट कम्पार्टमेंटल परीक्षा, 2026";
            }
            else
            {
                phFaculty.Visible = true;
                lblFacultyHindi.Text = "<label><strong>" + hindiFaculty + "</strong></label>";
                lblExamTitle.Text = "INTERMEDIATE SPECIAL EXAMINATION, 2026";
                lblExamTitleHindi.Text = "इंटरमीडिएट विशेष परीक्षा, 2026";
                lblCollegeName.Text = "कॉलेज/+2 स्कूल का नाम";
                if (tdElective1 != null)
                    tdElective1.InnerHtml = "ऐच्छिक विषय-1";

                if (tdElective2 != null)
                    tdElective2.InnerHtml = "ऐच्छिक विषय-2";

                if (tdElective3 != null)
                    tdElective3.InnerHtml = "ऐच्छिक विषय-3";
                lblExamSubjectHindi.Text = "सैद्धान्तिक वार्षिक परीक्षा के विषय (निर्धारित परीक्षा कार्यक्रम सहित)";
                lblExamSchoolHindi.Text = "महाविद्यालय / +2 विद्यालय प्रधान का हस्ताक्षर एवं मुहर";
                trVocational.Visible = hasVocational;
            }
        }
    }

    private DataTable CreateCombinedStudentDataTableSchema()
    {
        try
        {
            log.Debug("Creating combined student data table schema.");
            DataTable dt = new DataTable();

            // Student Personal Info
            dt.Columns.Add("StudentID", typeof(int));
            dt.Columns.Add("StudentName", typeof(string));
            dt.Columns.Add("FatherName", typeof(string));
            dt.Columns.Add("MotherName", typeof(string));
            dt.Columns.Add("CollegeCode", typeof(string));
            //dt.Columns.Add("DOB", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("MaritalStatus", typeof(string));
            dt.Columns.Add("AadharNo", typeof(string));
            dt.Columns.Add("Disability", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Caste", typeof(string));
            dt.Columns.Add("Nationality", typeof(string));
            dt.Columns.Add("Religion", typeof(string));
            dt.Columns.Add("StudentCategory", typeof(string));
            dt.Columns.Add("BsebId", typeof(string));

            dt.Columns.Add("CollegeId", typeof(int));

            dt.Columns.Add("FacultyId", typeof(int));
            dt.Columns.Add("FacultyName", typeof(string));
            dt.Columns.Add("Faculty", typeof(string));
            dt.Columns.Add("College", typeof(string));
            dt.Columns.Add("CollegeName", typeof(string));
            dt.Columns.Add("RollCode", typeof(string));
            dt.Columns.Add("RollNo", typeof(string));
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Columns.Add("OfssReferenceNo", typeof(string));
            dt.Columns.Add("ExamTypeName", typeof(string));
            dt.Columns.Add("RollNumber", typeof(string));
            dt.Columns.Add("FormDownloaded", typeof(bool));
            dt.Columns.Add("UniqueNo", typeof(string));

            // Exam Info

            dt.Columns.Add("ExamCenter", typeof(string));
            dt.Columns.Add("ExamCenterName", typeof(string));
            dt.Columns.Add("TheoryExamCenterName", typeof(string));

            // Student Images
            dt.Columns.Add("StudentPhotoPath", typeof(string));
            dt.Columns.Add("StudentSignaturePath", typeof(string));

            // Compulsory Subjects
            dt.Columns.Add("CompulsorySubject1Code", typeof(string));
            dt.Columns.Add("CompulsorySubject1Name", typeof(string));
            dt.Columns.Add("CompulsorySubject1Date", typeof(string));
            dt.Columns.Add("CompulsorySubject1Shift", typeof(string));
            dt.Columns.Add("CompulsorySubject1Time", typeof(string));

            dt.Columns.Add("CompulsorySubject2Code", typeof(string));
            dt.Columns.Add("CompulsorySubject2Name", typeof(string));
            dt.Columns.Add("CompulsorySubject2Date", typeof(string));
            dt.Columns.Add("CompulsorySubject2Shift", typeof(string));
            dt.Columns.Add("CompulsorySubject2Time", typeof(string));

            dt.Columns.Add("CompulsorySubject3Code", typeof(string));
            dt.Columns.Add("CompulsorySubject3Name", typeof(string));
            dt.Columns.Add("CompulsorySubject3Date", typeof(string));
            dt.Columns.Add("CompulsorySubject3Shift", typeof(string));
            dt.Columns.Add("CompulsorySubject3Time", typeof(string));

            // Elective Subjects
            dt.Columns.Add("ElectiveSubject1Code", typeof(string));
            dt.Columns.Add("ElectiveSubject1Name", typeof(string));
            dt.Columns.Add("ElectiveSubject1Date", typeof(string));
            dt.Columns.Add("ElectiveSubject1Shift", typeof(string));
            dt.Columns.Add("ElectiveSubject1Time", typeof(string));

            dt.Columns.Add("ElectiveSubject2Code", typeof(string));
            dt.Columns.Add("ElectiveSubject2Name", typeof(string));
            dt.Columns.Add("ElectiveSubject2Date", typeof(string));
            dt.Columns.Add("ElectiveSubject2Shift", typeof(string));
            dt.Columns.Add("ElectiveSubject2Time", typeof(string));


            dt.Columns.Add("ElectiveSubject3Code", typeof(string));
            dt.Columns.Add("ElectiveSubject3Name", typeof(string));
            dt.Columns.Add("ElectiveSubject3Date", typeof(string));
            dt.Columns.Add("ElectiveSubject3Shift", typeof(string));
            dt.Columns.Add("ElectiveSubject3Time", typeof(string));
            // Additional Subject
            dt.Columns.Add("AdditionalSubjectCode", typeof(string));
            dt.Columns.Add("AdditionalSubjectName", typeof(string));
            dt.Columns.Add("AdditionalSubjectDate", typeof(string));
            dt.Columns.Add("AdditionalSubjectShift", typeof(string));
            dt.Columns.Add("AdditionalSubjectTime", typeof(string));

            // Vocational Subjects
            //dt.Columns.Add("VocationalSubjectCode1Code", typeof(string));
            //dt.Columns.Add("VocationalSubjectName1Name", typeof(string));
            dt.Columns.Add("VocationalSubjectCode", typeof(string));
            dt.Columns.Add("VocationalSubjectName", typeof(string));
            dt.Columns.Add("VocationalSubjectDate", typeof(string));
            dt.Columns.Add("VocationalSubjectShift", typeof(string));
            dt.Columns.Add("VocationalSubjectTime", typeof(string));
            dt.Columns.Add("HasVocationalSubjects", typeof(bool));

            dt.Columns.Add("CompulsorySubject1PaperType", typeof(string));
            dt.Columns.Add("CompulsorySubject2PaperType", typeof(string));
            dt.Columns.Add("ElectiveSubject1PaperType", typeof(string));
            dt.Columns.Add("ElectiveSubject2PaperType", typeof(string));
            dt.Columns.Add("ElectiveSubject3PaperType", typeof(string));
            dt.Columns.Add("AdditionalSubjectPaperType", typeof(string));
            dt.Columns.Add("VocationalSubjectPaperType", typeof(string));

            return dt;
        }
        catch (Exception ex)
        {
            log.Error("Error in CreateCombinedStudentDataTableSchema", ex);
            throw;
        }
    }
    private void CategorizeAndPopulateSubjects(DataTable dtSubjects, DataRow targetRow, int facultyId, string collegeId)
    {
        try
        {
            log.Debug(string.Format("Categorizing subjects for FacultyId={0}", facultyId));

            List<DataRow> compulsorySubjects = new List<DataRow>();
            List<DataRow> electiveSubjects = new List<DataRow>();
            List<DataRow> additionalSubjects = new List<DataRow>();
             List<DataRow> vocationalSubjects = new List<DataRow>();

            foreach (DataRow r in dtSubjects.Rows)
            {
                string subjectGroup = r["SubjectGroup"] != DBNull.Value ? r["SubjectGroup"].ToString() : "";
                string subjectName = r["SubjectName"] != DBNull.Value ? r["SubjectName"].ToString() : "";
                int subjectCode = r["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(r["SubjectPaperCode"]) : -1;
                string paperType = r["PaperType"] != DBNull.Value ? r["PaperType"].ToString() : "";

                if (string.IsNullOrEmpty(subjectGroup) && string.IsNullOrEmpty(subjectName) && subjectCode == -1)
                    continue;

                if (subjectGroup.StartsWith("Compulsory subject group-1"))
                {
                    targetRow["CompulsorySubject1Code"] = subjectCode != -1 ? subjectCode.ToString() : "";
                    targetRow["CompulsorySubject1Name"] = subjectName;
                    if (r["ExamDate"] != DBNull.Value) targetRow["CompulsorySubject1Date"] = Convert.ToDateTime(r["ExamDate"]).ToString("dd-MM-yyyy");
                    targetRow["CompulsorySubject1Shift"] = r["ExamShift"];
                    targetRow["CompulsorySubject1Time"] = r["ExamTime"];
                    targetRow["CompulsorySubject1PaperType"] = paperType;
                }
                else if (subjectGroup.StartsWith("Compulsory subject group-2"))
                {
                    targetRow["CompulsorySubject2Code"] = subjectCode != -1 ? subjectCode.ToString() : "";
                    targetRow["CompulsorySubject2Name"] = subjectName;
                    if(r["ExamDate"] != DBNull.Value) targetRow["CompulsorySubject2Date"] = Convert.ToDateTime(r["ExamDate"]).ToString("dd-MM-yyyy");
                    targetRow["CompulsorySubject2Shift"] = r["ExamShift"];
                    targetRow["CompulsorySubject2Time"] = r["ExamTime"];
                    targetRow["CompulsorySubject2PaperType"] = paperType;
                }
                else if (subjectGroup.Contains("Compulsory") && !subjectGroup.StartsWith("Compulsory subject group-"))
                {
                    targetRow["CompulsorySubject3Code"] = subjectCode != -1 ? subjectCode.ToString() : "";
                    targetRow["CompulsorySubject3Name"] = subjectName;
                    if (r["ExamDate"] != DBNull.Value) targetRow["CompulsorySubject3Date"] = Convert.ToDateTime(r["ExamDate"]).ToString("dd-MM-yyyy");
                    targetRow["CompulsorySubject3Shift"] = r["ExamShift"];
                    targetRow["CompulsorySubject3Time"] = r["ExamTime"];
                    targetRow["CompulsorySubject3PaperType"] = paperType;
                }
                else if (subjectGroup.Contains("Vocational"))
                {
                    vocationalSubjects.Add(r);
                }
                else if (subjectGroup.Contains("Elective") || subjectGroup.Contains("Optional"))
                {
                    electiveSubjects.Add(r);
                }
                else if (subjectGroup.Contains("Additional"))
                {
                    additionalSubjects.Add(r);
                }
            }

            electiveSubjects.Sort(delegate (DataRow a, DataRow b)
            {
                int aCode = a["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(a["SubjectPaperCode"]) : int.MaxValue;
                int bCode = b["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(b["SubjectPaperCode"]) : int.MaxValue;
                return aCode.CompareTo(bCode);
            });

            for (int i = 0; i < electiveSubjects.Count && i < 3; i++)
            {
                DataRow sub = electiveSubjects[i];
                int code = sub["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(sub["SubjectPaperCode"]) : -1;
                string name = sub["SubjectName"] != DBNull.Value ? sub["SubjectName"].ToString() : "";
                string paperType = sub["PaperType"] != DBNull.Value ? sub["PaperType"].ToString() : "";

                targetRow["ElectiveSubject" + (i + 1) + "Code"] = code != -1 ? code.ToString() : "";
                targetRow["ElectiveSubject" + (i + 1) + "Name"] = name;
                targetRow["ElectiveSubject" + (i + 1) + "PaperType"] = paperType;
                if (sub["ExamDate"] != DBNull.Value)
                {
                    DateTime examDate = Convert.ToDateTime(sub["ExamDate"]);
                    targetRow["ElectiveSubject" + (i + 1) + "Date"] = examDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    targetRow["ElectiveSubject" + (i + 1) + "Date"] = "";
                }
                targetRow["ElectiveSubject"+(i + 1)+"Shift"] = sub["ExamShift"];
                targetRow["ElectiveSubject"+(i + 1)+"Time"] = sub["ExamTime"];
            }

            if (additionalSubjects.Count > 0)
            {
                DataRow sub = additionalSubjects[0];
                int code = sub["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(sub["SubjectPaperCode"]) : -1;
                string name = sub["SubjectName"] != DBNull.Value ? sub["SubjectName"].ToString() : "";

                targetRow["AdditionalSubjectCode"] = code != -1 ? code.ToString() : "";
                targetRow["AdditionalSubjectName"] = name;
                if (sub["ExamDate"] != DBNull.Value) targetRow["AdditionalSubjectDate"] = Convert.ToDateTime(sub["ExamDate"]).ToString("dd-MM-yyyy");
                targetRow["AdditionalSubjectShift"] = sub["ExamShift"];
                targetRow["AdditionalSubjectTime"] = sub["ExamTime"];
                targetRow["AdditionalSubjectPaperType"] = sub["PaperType"].ToString();
            }

            if (vocationalSubjects.Count > 0)
            {
                DataRow sub = vocationalSubjects[0];
                int code = sub["SubjectPaperCode"] != DBNull.Value ? Convert.ToInt32(sub["SubjectPaperCode"]) : -1;
                string name = sub["SubjectName"] != DBNull.Value ? sub["SubjectName"].ToString() : "";

                //targetRow["VocationalSubjectCode1Code"] = code != -1 ? code.ToString() : "";
                //targetRow["VocationalSubjectName1Name"] = name;
                targetRow["VocationalSubjectCode"] = code != -1 ? code.ToString() : "";
                targetRow["VocationalSubjectName"] = name;
                if (sub["ExamDate"] != DBNull.Value) targetRow["VocationalSubjectDate"] = Convert.ToDateTime(sub["ExamDate"]).ToString("dd-MM-yyyy");
                targetRow["VocationalSubjectShift"] = sub["ExamShift"];
                targetRow["VocationalSubjectTime"] = sub["ExamTime"];
                targetRow["VocationalSubjectPaperType"] = sub["PaperType"].ToString();
            }

            int isExists = dl.CheckVocationalCollegeSubjectExists(Convert.ToInt32(collegeId), facultyId);

            if (isExists == 1)
            {
                targetRow["HasVocationalSubjects"] = true;
            }
            else
            {
                targetRow["HasVocationalSubjects"] = false;
            }
            //bool isVocationalFaculty = (facultyId == 4);
            //targetRow["HasVocationalSubjects"] = vocationalSubjects.Count > 0 || isVocationalFaculty;

            //log.Debug(string.Format("Subjects categorized: Compulsory={0}, Elective={1}, Additional={2}, Vocational={3}", 3, electiveSubjects.Count, additionalSubjects.Count, vocationalSubjects.Count));
        }
        catch (Exception ex)
        {
            log.Error("Error in CategorizeAndPopulateSubjects", ex);
            throw;
        }
    }

    public bool btnDownloadUpdate_Click(string studentIds)
    {
        try
        {
            DBHelper dl = new DBHelper();
            bool allSuccessful = true;
            string fromPage = "TheoryDummy";

            string[] ids = studentIds.Split(',');

            bool isStudent = (fromPage == "TheoryDummy");

            foreach (string id in ids)
            {
                bool result = dl.UpdatePracticalAndTheoryDummyDownloadStatus(id.Trim(), fromPage);
                if (!result)
                    allSuccessful = false;
            }

            return allSuccessful;
        }
        catch
        {
            return false;
        }
    }
    private void EnsureLog4NetConfigured()
    {
        if (!isLog4NetConfigured)
        {
            var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
            string configFile = Server.MapPath("~/Web.config");
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(configFile));

            isLog4NetConfigured = true;
            log.Info("log4net configured.");
        }
    }

    private void EnsureLogFolder()
    {
        string logFolder = Server.MapPath("~/logs");
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }
    }



}
