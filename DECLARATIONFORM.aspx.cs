using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Services;
using System.Web;

public partial class DECLARATIONFORM : System.Web.UI.Page
{
    // Assuming DBHelper is correctly implemented and accessible
    DBHelper dl = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Check if user is logged in (session has CollegeId)
            if (Session["CollegeId"] != null)
            {
                string studentIds = Request.QueryString["studentIds"]; // comma-separated IDs
                string categoryType = Request.QueryString["categoryType"];
                string CollegeId = Request.QueryString["CollegeId"];
                string fromPage = Request.QueryString["fromPage"];
                if (!string.IsNullOrEmpty(studentIds))
                {
                    if (fromPage == "ManageDeclarationForm")
                    {
                        BtnBack.Visible = true;
                    }
                    string[] idArray = studentIds.Split(',');
                    // Pass the array of IDs to BindAllStudents
                    BindAllStudents(idArray, categoryType);
                }
                else
                {
                    // Handle case where no studentIds are provided
                    Response.Write("No student IDs provided.");
                }
            }
            else
            {
                // Redirect to login if not authenticated
                Response.Redirect("Login.aspx");
            }
        }
    }

    private void BindAllStudents(string[] studentIds, string categoryType)
    {
        DataTable allStudentsDt = new DataTable();
        bool firstTable = true;

        foreach (string sid in studentIds)
        {
            DataTable dt = dl.ViewStudentRegDetails(sid.Trim(), categoryType);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (firstTable)
                {
                    allStudentsDt = dt.Clone();
                    firstTable = false;
                }
                allStudentsDt.Merge(dt);
            }
        }

        if (allStudentsDt.Rows.Count > 0)
        {
            List<StudentDeclarationData> students = new List<StudentDeclarationData>();
            //string collegeId = Session["CollegeId"].ToString();

            string collegeId = Request.QueryString["CollegeId"].ToString();
            //if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
            //{
            //    DataTable dt = dl.getcollegeidbasedonCollegecode(Request.QueryString["CollegeId"].ToString());

            //    if (dt.Rows.Count > 0)
            //    {
            //        collegeId = dt.Rows[0]["Pk_CollegeId"].ToString();
            //    }
            //}
            //else if (Session["CollegeId"] != null)
            //{
            //    collegeId = Session["CollegeId"].ToString();
            //}
            string dirPath = "~/Uploads/StudentsReg/Signatures/";
            string dirPHOTOSPath = "~/Uploads/StudentsReg/Photos/";

            foreach (DataRow dr in allStudentsDt.Rows)
            {
                StudentDeclarationData student = new StudentDeclarationData
                {
                    StudentName = dr["StudentName"].ToString().ToUpper(),
                    FatherName = dr["FatherName"].ToString().ToUpper(),
                    MotherName = dr["MotherName"].ToString().ToUpper(),
                    DOB = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]).ToString("dd/MM/yyyy") : string.Empty,
                    Gender = dr["GenderName"].ToString().ToUpper(),
                    MobileNo = dr["MobileNo"].ToString().ToUpper(),
                    StudentPhotoPath = dirPHOTOSPath + dr["StudentPhotoPath"].ToString().ToUpper(),
                    StudentSignaturePath = dirPath + dr["StudentSignaturePath"].ToString().ToUpper(),
                    CollegeName = dr["Collegedetails"] != null ? dr["Collegedetails"].ToString().ToUpper() : string.Empty,
                    ApaarId = dr["ApaarId"] != DBNull.Value ? dr["ApaarId"].ToString().ToUpper() : string.Empty,
                    EmailId = dr["EmailId"] != DBNull.Value ? dr["EmailId"].ToString() : string.Empty,
                    AadharNo = dr["AadharNumber"] != DBNull.Value ? dr["AadharNumber"].ToString().ToUpper() : string.Empty,
                    Caste = dr["CasteCategoryName"] != DBNull.Value ? dr["CasteCategoryName"].ToString().ToUpper() : string.Empty,
                    Category = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString().ToUpper() : string.Empty,
                    Religion = dr["Religion"] != DBNull.Value ? dr["Religion"].ToString().ToUpper() : string.Empty,
                    MaritalStatus = dr["MaritalStatus"] != DBNull.Value ? dr["MaritalStatus"].ToString().ToUpper() : string.Empty,
                    Area = dr["AreaName"] != DBNull.Value ? dr["AreaName"].ToString().ToUpper() : string.Empty,
                    Medium = dr["ExamMediumName"] != DBNull.Value ? dr["ExamMediumName"].ToString().ToUpper() : string.Empty,
                    Nationality = dr["Nationality"] != DBNull.Value ? dr["Nationality"].ToString().ToUpper() : string.Empty,
                    DifferentlyAbled = dr["DifferentlyAbled"] != DBNull.Value ? dr["DifferentlyAbled"].ToString().ToUpper() : string.Empty,
                    Ident1 = dr["IdentificationMark1"] != DBNull.Value ? dr["IdentificationMark1"].ToString().ToUpper() : string.Empty,
                    Ident2 = dr["IdentificationMark2"] != DBNull.Value ? dr["IdentificationMark2"].ToString().ToUpper() : string.Empty,
                    Address = dr["StudentAddress"] != DBNull.Value ? dr["StudentAddress"].ToString().ToUpper() : string.Empty,
                    District = dr["DistrictName"] != DBNull.Value ? dr["DistrictName"].ToString().ToUpper() : string.Empty,
                    PinCode = dr["PinCode"] != DBNull.Value ? dr["PinCode"].ToString().ToUpper() : string.Empty,
                    BankBranch = dr["BankBranchName"] != DBNull.Value ? dr["BankBranchName"].ToString().ToUpper() : string.Empty,
                    AccountNo = dr["StudentBankAccountNo"] != DBNull.Value ? dr["StudentBankAccountNo"].ToString().ToUpper() : string.Empty,
                    IFSC = dr["IFSCCode"] != DBNull.Value ? dr["IFSCCode"].ToString().ToUpper() : string.Empty,
                    OfssReferenceNo = dr["OfssNo"] != DBNull.Value ? dr["OfssNo"].ToString().ToUpper() : string.Empty,
                    StudentUniqueId = dr["StudentUniqueId"] != DBNull.Value ? dr["StudentUniqueId"].ToString().ToUpper() : string.Empty

                    
                };

                // ✅ Subject binding logic
                string sid = studentIds[0]; // use outer loop studentId, not from dr
                string facultyId = dr.Table.Columns.Contains("FacultyId") ? dr["FacultyId"].ToString() : null;

                if (!string.IsNullOrEmpty(sid) && !string.IsNullOrEmpty(facultyId))
                {
                    DataTable dtSubjects = dl.GetDummyStudentSubjectDetails(Convert.ToInt32(sid),Convert.ToInt32(collegeId),Convert.ToInt32(facultyId));

                    if (dtSubjects != null && dtSubjects.Rows.Count > 0)
                    {
                        int electiveCount = 1;

                        foreach (DataRow sdr in dtSubjects.Rows)
                        {
                            string subjectName = sdr["SubjectName"].ToString().ToUpper();
                            string subjectGroup = sdr["SubjectGroup"].ToString();

                            if (subjectGroup.Contains("Compulsory subject group-1"))
                            {
                                student.Compulsory1 = subjectName;
                            }
                            else if (subjectGroup.Contains("Compulsory subject group-2"))
                            {
                                student.Compulsory2 = subjectName;
                            }
                            else if (subjectGroup.Contains("Elective subject group"))
                            {
                                if (electiveCount == 1) student.Elective1 = subjectName;
                                else if (electiveCount == 2) student.Elective2 = subjectName;
                                else if (electiveCount == 3) student.Elective3 = subjectName;
                                electiveCount++;
                            }
                          
                            else if (subjectGroup.Contains("Vocational Additional subject group"))
                            {
                                student.Vocational = subjectName;
                            }
                            else if (subjectGroup.Contains("Additional subject group"))
                            {
                                student.Additional = subjectName;
                            }
                        }
                    }
                }

                students.Add(student);
            }

            rptStudents.DataSource = students;
            rptStudents.DataBind();
        }
        else
        {
            Response.Write("No records found for the selected students.");
        }
    }



    public class StudentDeclarationData
    {
        //public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string StudentPhotoPath { get; set; }
        public string StudentSignaturePath { get; set; }
        //public string MIL { get; set; }
        //public string SIL { get; set; }
        public string CollegeName { get; set; }
        //public string ApplicationNo { get; set; }
        public string ApaarId { get; set; }
        public string EmailId { get; set; }
        public string AadharNo { get; set; }
        public string Caste { get; set; }
        public string Category { get; set; }
        public string Religion { get; set; }
        public string MaritalStatus { get; set; }
        public string Area { get; set; }
        public string Medium { get; set; }
        public string Nationality { get; set; }
        public string DifferentlyAbled { get; set; }
        public string Ident1 { get; set; }
        public string Ident2 { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string District { get; set; }
        public string PinCode { get; set; }
        public string BankBranch { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }

        public string Compulsory1 { get; set; }
        public string Compulsory2 { get; set; }
        public string Elective1 { get; set; }
        public string Elective2 { get; set; }
        public string Elective3 { get; set; }
        public string Additional { get; set; }
        public string Vocational { get; set; }
        public string OfssReferenceNo { get; set; }
        public string StudentUniqueId { get; set; }

    }

    [WebMethod]
    public static bool MarkDeclarationFormDownloaded(string studentIds)
    {
        try
        {
            DBHelper dl = new DBHelper();
            bool allSucceeded = true;
            string[] ids = studentIds.Split(',');

            foreach (string id in ids)
            {
             
                 bool result = dl.DeclarationFormDownloaded(id.Trim());
                if (!result) allSucceeded = false;
            }

            // Placeholder return
            return allSucceeded;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}