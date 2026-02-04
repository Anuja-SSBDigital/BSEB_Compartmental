using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web;

public partial class InterRegistrationForm : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(InterRegistrationForm));
    private static bool isLog4NetConfigured = false;

    private void EnsureLog4NetConfigured()
    {
        if (!isLog4NetConfigured)
        {
            var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
            string configFile = Server.MapPath("~/Web.config"); // Using web.config
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
    DBHelper dl = new DBHelper();
    protected DataTable mergedTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        EnsureLogFolder();
        EnsureLog4NetConfigured();
        log.Info("Page_Load started.");

        if (!IsPostBack)
        {
            try
            {
                log.Debug("Page is not a postback. Processing initial request.");
                string encodedStudentData = Request.QueryString["studentData"];
                if (!string.IsNullOrEmpty(encodedStudentData))
                {
                    log.Debug("Query string 'studentData' is not empty.");
                    string decodedStudentData = Server.UrlDecode(encodedStudentData);
                    log.Debug("Decoded student data: " + decodedStudentData);

                    List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    log.Info(string.Format("Found {0} student entries to process.", selectedStudents.Count));

                    mergedTable = CreateStudentDataTable();
                    string FacultyId = string.Empty;

                    foreach (string studentEntry in selectedStudents)
                    {
                        string[] parts = studentEntry.Split('|');
                        if (parts.Length == 3)
                        {
                            string studentIdStr = parts[0];
                            string CollegeId = parts[1];
                            FacultyId = parts[2];

                            int studentId;
                            if (int.TryParse(studentIdStr, out studentId))
                            {
                                log.Info(string.Format("Fetching data for StudentID: {0}, College: {1}, Faculty: {2}", studentId, CollegeId, FacultyId));
                                DataTable dt = dl.GetStudentInterRegiFormData(studentId, CollegeId, FacultyId);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    log.Info(string.Format("Data fetched successfully for student ID: {0}. Merging rows.", studentId));
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
                                        newRow["DOB"] = sourceRow["DOB"] != DBNull.Value && sourceRow["DOB"] != null? ParseDateOfBirth(sourceRow["DOB"]): "";
                                        newRow["MatricBoardName"] = sourceRow["MatricBoardName"];
                                        newRow["MatricRollCode"] = sourceRow["MatricRollCode"];
                                        newRow["MatricRollNumber"] = sourceRow["MatricRollNumber"];
                                        newRow["MatricPassingYear"] = sourceRow["MatricPassingYear"];
                                        newRow["Gender"] = sourceRow["Gender"];
                                        newRow["CasteCategory"] = sourceRow["CasteCategory"];
                                        newRow["Nationality"] = sourceRow["Nationality"];
                                        newRow["Religion"] = sourceRow["Religion"];
                                        // newRow["AadharNumber"] = sourceRow["AadharNumber"];
                                        //  newRow["SubDivisionName"] = sourceRow["SubDivisionName"];
                                        //newRow["MobileNo"] = sourceRow["MobileNo"];
                                        //newRow["EmailId"] = sourceRow["EmailId"];
                                        //newRow["StudentAddress"] = sourceRow["StudentAddress"];
                                        //newRow["AreaName"] = sourceRow["AreaName"];
                                        //newRow["MaritalStatus"] = sourceRow["MaritalStatus"];
                                        //newRow["StudentBankAccountNo"] = sourceRow["StudentBankAccountNo"];
                                        //newRow["BankBranchName"] = sourceRow["BankBranchName"];
                                        //newRow["IFSCCode"] = sourceRow["IFSCCode"];
                                        //newRow["IdentificationMark1"] = sourceRow["IdentificationMark1"];
                                        //newRow["IdentificationMark2"] = sourceRow["IdentificationMark2"];
                                        //newRow["MediumName"] = sourceRow["MediumName"];

                                        //newRow["ApaarId"] = sourceRow["ApaarId"];
                                        newRow["BSEB_Unique_ID"] = sourceRow["BSEB_Unique_ID"];
                                      //  newRow["DistrictName"] =Session["DistrictName"].ToString();
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
                                        mergedTable.Rows.Add(newRow);
                                        log.Debug(string.Format("Added row for student ID: {0} to merged table.", studentId));
                                    }
                                }
                                else
                                {
                                    log.Warn(string.Format("No data found for StudentID: {0}, College: {1}, Faculty: {2}.", studentId, CollegeId, FacultyId));
                                }
                            }
                            else
                            {
                                log.Warn(string.Format("Failed to parse student ID from '{0}'.", studentIdStr));
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
                    else
                    {
                        log.Warn("Merged table has no rows. Not binding the repeater.");
                    }
                }
                else
                {
                    log.Warn("Query string 'studentData' is null or empty. No data to process.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Page_Load: ", ex);
            }
        }
        else
        {
            log.Debug("Page is a postback. Skipping initial request processing.");
        }

        log.Info("Page_Load completed.");
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
    private DataTable CreateStudentDataTable()
    {
        log.Debug("Creating empty student data table.");
        DataTable dt = new DataTable();
        dt.Columns.Add("FacultyName");
        dt.Columns.Add("OFSSCAFNo");
        dt.Columns.Add("CategoryName");
        dt.Columns.Add("CollegeCode");
        dt.Columns.Add("CollegeName");
        dt.Columns.Add("DistrictName");
        dt.Columns.Add("StudentName");
        dt.Columns.Add("MotherName");
        dt.Columns.Add("FatherName");
		log.Info("DOB is here");
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
        //dt.Columns.Add("AadharNumber");
        //dt.Columns.Add("SubDivisionName");
        //dt.Columns.Add("MobileNo");
        //dt.Columns.Add("EmailId");
        //dt.Columns.Add("StudentAddress");
        //dt.Columns.Add("AreaName");
        //dt.Columns.Add("MaritalStatus");
        //dt.Columns.Add("StudentBankAccountNo");
        //dt.Columns.Add("BankBranchName");
        //dt.Columns.Add("IFSCCode");
        //dt.Columns.Add("IdentificationMark1");
        //dt.Columns.Add("IdentificationMark2");
        //dt.Columns.Add("MediumName");
        dt.Columns.Add("FacultyId");
        dt.Columns.Add("BSEB_Unique_ID");
        // dt.Columns.Add("ApaarId");
        return dt;
    }

    public StudentSubjectData LoadSubjects(string FacultyId, string CollegeId)
    {
        log.Info(string.Format("LoadSubjects started for FacultyId: {0}, CollegeId: {1}", FacultyId, CollegeId));
        StudentSubjectData subjectData = new StudentSubjectData();
        DataTable allSubjects = dl.GetSubjectPapersByFacultyAndGroup(FacultyId);

        if (allSubjects == null)
        {
            log.Warn("GetSubjectPapersByFacultyAndGroup returned a null DataTable.");
            return subjectData;
        }

        log.Debug("LoadSubjects fetched rows: " + allSubjects.Rows.Count);
        foreach (DataRow r in allSubjects.Rows)
        {
            log.Debug(string.Format(
                "Row: Group={0}, Code={1}, Name={2}",
                r["GroupName"], r["SubjectPaperCode"], r["SubjectPaperName"]
            ));
        }

        var englishHindi = allSubjects.AsEnumerable()
            .Where(r =>
                r["GroupName"].ToString() == "Compulsory" &&
                (r["SubjectPaperName"].ToString() == "Hindi" ||
                 r["SubjectPaperName"].ToString() == "English"))
            .ToList();
        log.Debug(string.Format("Found {0} Hindi/English compulsory subjects.", englishHindi.Count));

        var otherCompulsory = allSubjects.AsEnumerable()
            .Where(r =>
                r["GroupName"].ToString() == "Compulsory" &&
                !englishHindi.Any(x => x["SubjectPaperCode"].ToString() == r["SubjectPaperCode"].ToString()))
            .ToList();
        log.Debug(string.Format("Found {0} other compulsory subjects.", otherCompulsory.Count));

        foreach (var subj in englishHindi)
        {
            bool exists = otherCompulsory.Any(x => x["SubjectPaperCode"].ToString() == subj["SubjectPaperCode"].ToString());
            if (!exists)
            {
                otherCompulsory.Insert(0, subj);
                log.Debug(string.Format("Inserted {0} at the top of other compulsory list.", subj["SubjectPaperName"]));
            }
        }

        otherCompulsory = otherCompulsory
            .OrderBy(r => r["SubjectPaperCode"].ToString())
            .ToList();

        var combined = new List<CombinedSubject>();
        int maxRows = Math.Max(englishHindi.Count, otherCompulsory.Count);
        for (int i = 0; i < maxRows; i++)
        {
            CombinedSubject cs = new CombinedSubject();
            cs.Group1SubjectName = i < englishHindi.Count ? englishHindi[i]["SubjectPaperName"].ToString() : "";
            cs.Group1SubjectCode = i < englishHindi.Count ? englishHindi[i]["SubjectPaperCode"].ToString() : "";
            cs.Group2SubjectName = i < otherCompulsory.Count ? otherCompulsory[i]["SubjectPaperName"].ToString() : "";
            cs.Group2SubjectCode = i < otherCompulsory.Count ? otherCompulsory[i]["SubjectPaperCode"].ToString() : "";
            combined.Add(cs);
        }
        subjectData.CompulsorySubjectsCombined = combined;
        log.Debug(string.Format("Created combined list of {0} compulsory subjects.", combined.Count));

        var electives = allSubjects.AsEnumerable()
            .Where(r => r["GroupName"].ToString() == "Elective")
            .ToList();
        log.Debug(string.Format("Found {0} elective subjects.", electives.Count));

        DataTable dtElective = new DataTable();
        dtElective.Columns.Add("Name1"); dtElective.Columns.Add("Code1");
        dtElective.Columns.Add("Name2"); dtElective.Columns.Add("Code2");

        for (int i = 0; i < electives.Count; i += 2)
        {
            DataRow dr = dtElective.NewRow();
            dr["Name1"] = electives[i]["SubjectPaperName"];
            dr["Code1"] = electives[i]["SubjectPaperCode"];
            if (i + 1 < electives.Count)
            {
                dr["Name2"] = electives[i + 1]["SubjectPaperName"];
                dr["Code2"] = electives[i + 1]["SubjectPaperCode"];
            }
            dtElective.Rows.Add(dr);
        }
        subjectData.ElectiveSubjects = dtElective;
        log.Debug(string.Format("Created elective subjects DataTable with {0} rows.", dtElective.Rows.Count));

        var additionals = allSubjects.AsEnumerable()
            .Where(r => r["GroupName"].ToString() == "Additional")
            .ToList();
        log.Debug(string.Format("Found {0} additional subjects.", additionals.Count));

        DataTable dtAdditional = new DataTable();
        dtAdditional.Columns.Add("Name1"); dtAdditional.Columns.Add("Code1");
        dtAdditional.Columns.Add("Name2"); dtAdditional.Columns.Add("Code2");
        dtAdditional.Columns.Add("Name3"); dtAdditional.Columns.Add("Code3");

        for (int i = 0; i < additionals.Count; i += 3)
        {
            DataRow dr = dtAdditional.NewRow();
            for (int j = 0; j < 3 && (i + j) < additionals.Count; j++)
            {
                dr["Name" + (j + 1)] = additionals[i + j]["SubjectPaperName"];
                dr["Code" + (j + 1)] = additionals[i + j]["SubjectPaperCode"];
            }
            dtAdditional.Rows.Add(dr);
        }
        subjectData.AdditionalSubjects = dtAdditional;
        log.Debug(string.Format("Created additional subjects DataTable with {0} rows.", dtAdditional.Rows.Count));

        var vocationalList = allSubjects.AsEnumerable().Where(r => r["GroupName"].ToString().Contains("Vocational Additional")).ToList();
        log.Debug(string.Format("Found {0} vocational additional subjects.", vocationalList.Count));

        if (vocationalList.Count > 0)
            subjectData.VocationalAdditionalSubjects = vocationalList.CopyToDataTable();
        else
            subjectData.VocationalAdditionalSubjects = new DataTable();

        var vocElectives = allSubjects.AsEnumerable()
            .Where(r => r["GroupName"].ToString()
            .Equals("Elective", StringComparison.OrdinalIgnoreCase)).ToList();
        log.Debug(string.Format("Found {0} vocational elective subjects.", vocElectives.Count));

        var grouped = vocElectives.GroupBy(r => r["SubjectPaperName"].ToString())
                .Select(g => new
                {
                    SubjectName = g.Key,
                    Code1 = string.Join(",", g.Select(r => r["SubjectPaperCode"].ToString()).Distinct()),
                    Code2 = string.Join(",", g.Select(r => r.Table.Columns.Contains("SecondCode")
                                ? r["SecondCode"].ToString() : "")
                                .Where(c => !string.IsNullOrEmpty(c)).Distinct())
                }).ToList();

        DataTable dtVocElective = new DataTable();
        dtVocElective.Columns.Add("Name1");
        dtVocElective.Columns.Add("Code1");
        dtVocElective.Columns.Add("Name2");
        dtVocElective.Columns.Add("Code2");

        for (int i = 0; i < grouped.Count; i += 2)
        {
            DataRow dr = dtVocElective.NewRow();
            dr["Name1"] = grouped[i].SubjectName;
            dr["Code1"] = string.IsNullOrEmpty(grouped[i].Code2)
                ? grouped[i].Code1
                : string.Format("{0}, {1}", grouped[i].Code1, grouped[i].Code2);

            if (i + 1 < grouped.Count)
            {
                dr["Name2"] = grouped[i + 1].SubjectName;
                dr["Code2"] = string.IsNullOrEmpty(grouped[i + 1].Code2)
                    ? grouped[i + 1].Code1
                    : string.Format("{0}, {1}", grouped[i + 1].Code1, grouped[i + 1].Code2);
            }
            dtVocElective.Rows.Add(dr);
        }

        subjectData.VocationalElectiveSubjects = dtVocElective;
        log.Debug(string.Format("Created vocational elective subjects DataTable with {0} rows.", dtVocElective.Rows.Count));

        log.Info("LoadSubjects completed.");
        return subjectData;
    }
    protected void rptStudents_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            log.Info("rptStudents_ItemDataBound started.");
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                log.Debug(string.Format("Binding item of type {0}.", e.Item.ItemType));
                string photoPath = DataBinder.Eval(e.Item.DataItem, "StudentPhotoPath").ToString();
                string signaturePath = DataBinder.Eval(e.Item.DataItem, "StudentSignaturePath").ToString();

                Image imgPhoto = (Image)e.Item.FindControl("imgPhoto");
                Image imgSign = (Image)e.Item.FindControl("imgSign");
                if (imgPhoto != null)
                {
                    imgPhoto.ImageUrl = ResolveUrl(photoPath);
                    log.Debug(string.Format("Bound StudentPhotoPath: {0}", photoPath));
                }
                if (imgSign != null)
                {
                    imgSign.ImageUrl = ResolveUrl(signaturePath);
                    log.Debug(string.Format("Bound StudentSignaturePath: {0}", signaturePath));
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                string facultyId = drv["FacultyId"] != null ? drv["FacultyId"].ToString() : "";
                string collegeId = Session["CollegeId"] != null ? Session["CollegeId"].ToString() : "";

                if (string.IsNullOrEmpty(facultyId) || string.IsNullOrEmpty(collegeId))
                {
                    log.Warn(string.Format("Missing FacultyId ('{0}') or CollegeId ('{1}'). Skipping subject loading for this item.", facultyId, collegeId));
                    return;
                }

                StudentSubjectData ssd = LoadSubjects(facultyId, collegeId);

                Repeater rpt1 = e.Item.FindControl("rptCompulsorySubjectsCombined") as Repeater;
                if (rpt1 != null && ssd.CompulsorySubjectsCombined != null)
                {
                    rpt1.DataSource = ssd.CompulsorySubjectsCombined;
                    rpt1.DataBind();
                    log.Debug("rptCompulsorySubjectsCombined bound.");
                }

                Repeater rpt3 = e.Item.FindControl("rptAdditionalSubjects") as Repeater;
                if (rpt3 != null && ssd.AdditionalSubjects != null && ssd.AdditionalSubjects.Rows.Count > 0)
                {
                    rpt3.DataSource = ssd.AdditionalSubjects;
                    rpt3.DataBind();
                    log.Debug("rptAdditionalSubjects bound.");
                }


                Repeater rpt4 = e.Item.FindControl("rptVocationalAdditionalSubjects") as Repeater;
                Panel pnlVocational = e.Item.FindControl("pnlVocational") as Panel;

                if (rpt4 != null && ssd.VocationalAdditionalSubjects != null && ssd.VocationalAdditionalSubjects.Rows.Count > 0)
                {
                    rpt4.DataSource = ssd.VocationalAdditionalSubjects;
                    rpt4.DataBind();
                    pnlVocational.Visible = true;
                    log.Debug("rptVocationalAdditionalSubjects bound and panel set to visible.");
                }
                else
                {
                    pnlVocational.Visible = false;
                    log.Debug("No vocational additional subjects found. Panel set to hidden.");
                }

                Repeater rptElective = e.Item.FindControl("rptElectiveSubjects") as Repeater;
                Repeater rptVocElective = e.Item.FindControl("rptVocElectiveSubjects") as Repeater;

                if (facultyId.Equals("4", StringComparison.OrdinalIgnoreCase))
                {
                    log.Debug("Faculty ID is 4. Hiding elective repeater and showing vocational elective repeater.");
                    if (rptElective != null) rptElective.Visible = false;
                    if (rptVocElective != null) rptVocElective.Visible = true;

                    if (rptVocElective != null && ssd.VocationalElectiveSubjects != null && ssd.VocationalElectiveSubjects.Rows.Count > 0)
                    {
                        rptVocElective.DataSource = ssd.VocationalElectiveSubjects;
                        rptVocElective.DataBind();
                        log.Debug("Vocational elective subjects bound.");
                    }
                }
                else
                {
                    log.Debug(string.Format("Faculty ID is {0}. Hiding vocational elective repeater and showing elective repeater.", facultyId));
                    if (rptElective != null) rptElective.Visible = true;
                    if (rptVocElective != null) rptVocElective.Visible = false;

                    if (rptElective != null && ssd.ElectiveSubjects != null && ssd.ElectiveSubjects.Rows.Count > 0)
                    {
                        rptElective.DataSource = ssd.ElectiveSubjects;
                        rptElective.DataBind();
                        log.Debug("Elective subjects bound.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in rptStudents_ItemDataBound: ", ex);
            throw ex;
        }
        log.Info("rptStudents_ItemDataBound completed.");
    }

    protected string GetDobDigit(object dobValue, int index)
  {
      if (dobValue == null || dobValue == DBNull.Value)
          return "";

      DateTime dob;
      string dobString = dobValue.ToString();

      // Try parsing again with DateTime.TryParseExact, in case it's a different format
      if (DateTime.TryParseExact(dobString, new string[] { "yyyy-MM-dd", "dd/MM/yyyy" },
                                 CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
      {
          string dobStr = dob.ToString("ddMMyyyy");  // Convert to ddMMyyyy format
          if (index >= 0 && index < dobStr.Length)
          {
              log.Info("DOB is here " + dobStr);
              return dobStr[index].ToString();
          }
      }
      else
      {
          log.Warn("Invalid DOB format: " + dobString);
      }

      return "";
  }
    public class CombinedSubject
    {
        public string Group1SubjectName { get; set; }
        public string Group1SubjectCode { get; set; }
        public string Group2SubjectName { get; set; }
        public string Group2SubjectCode { get; set; }
    }
    public class StudentSubjectData
    {
        public List<CombinedSubject> CompulsorySubjectsCombined { get; set; }
        public DataTable ElectiveSubjects { get; set; }
        public DataTable AdditionalSubjects { get; set; }
        public DataTable VocationalAdditionalSubjects { get; set; }
        public DataTable VocationalElectiveSubjects { get; set; }
    }
    private void BindRepeater(DataTable dt, string groupName, System.Web.UI.WebControls.Repeater repeater)
    {
        log.Info(string.Format("BindRepeater started for group: {0}.", groupName));
        try
        {
            log.Debug("Binding repeater for group: " + groupName);
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("GroupName = '{0}'", groupName);
            repeater.DataSource = dv;
            repeater.DataBind();
            log.Debug(string.Format("Repeater for group '{0}' bound with {1} rows.", groupName, dv.Count));
        }
        catch (Exception ex)
        {
            log.Error("Error in BindRepeater: ", ex);
            throw ex;
        }
        log.Info("BindRepeater completed.");
    }


    [System.Web.Services.WebMethod]
    public static string UpdateDownloaded(string studentData)
    { 
        try
        {
            string decodedStudentData = HttpContext.Current.Server.UrlDecode(studentData);
            List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var StuIds in selectedStudents)
            {
                var Parts = StuIds.Split('|');
                if (Parts.Length == 3)
                {
                    int studentId;
                    if (int.TryParse(Parts[0], out studentId))
                    {
                        // Call your existing DL method
                        DBHelper db = new DBHelper();
                        db.UpdateStudentsDownloaded(studentId);
                    }
                }
            }
            return "success";
        }
        catch (Exception ex)
        {
            return "error: " + "Load Page Again";
        }
    }
    //protected void btnPrint_Click(object sender, EventArgs e)
    //{
    //    log.Info("btnPrint_Click started.");
    //    try
    //    {
    //        string encodedStudentData = Request.QueryString["studentData"];
    //        if (!string.IsNullOrEmpty(encodedStudentData))
    //        {
    //            string decodedStudentData = Server.UrlDecode(encodedStudentData);
    //            log.Debug("Decoded student data for printing: " + decodedStudentData);

    //            List<string> selectedStudents = decodedStudentData.Split(new string[] { ",|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
    //            log.Info(string.Format("Found {0} student IDs to update as downloaded.", selectedStudents.Count));

    //            foreach (var StuIds in selectedStudents)
    //            {
    //                var Parts = StuIds.Split('|');
    //                if (Parts.Length == 3)
    //                {
    //                    int studentId;
    //                    if (int.TryParse(Parts[0], out studentId))
    //                    {
    //                        dl.UpdateStudentsDownloaded(studentId);
    //                        log.Debug(string.Format("Updated downloaded status for student ID: {0}.", studentId));
    //                    }
    //                    else
    //                    {
    //                        log.Warn(string.Format("Failed to parse student ID from '{0}' for download status update.", Parts[0]));
    //                    }
    //                }
    //            }
    //        }
    //        ClientScript.RegisterStartupScript(this.GetType(), "printScript", "window.print();", true);
    //        log.Info("Client-side print script registered.");
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error("Error in btnPrint_Click: ", ex);
    //        throw ex;
    //    }
    //    log.Info("btnPrint_Click completed.");
    //}
}