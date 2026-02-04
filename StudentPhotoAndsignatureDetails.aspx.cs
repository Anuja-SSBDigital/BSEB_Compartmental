using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class StudentPhotoAndsignatureDetails : System.Web.UI.Page
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
                    string ExamTypeId = Request.QueryString["ExamTypeId"];

                    if (!string.IsNullOrEmpty(studentId))
                    {
                        DataTable studentDetails = dl.GetStudentDetails(studentId);

                        if (studentDetails.Rows.Count > 0)
                        {
                            string studentPhotoPath = studentDetails.Rows[0]["StudentPhotoPath"].ToString();
                            string studentSignaturePath = studentDetails.Rows[0]["StudentSignaturePath"].ToString();
                            string FacultyId = studentDetails.Rows[0]["FacultyId"].ToString();
                            string CategoryType = studentDetails.Rows[0]["CategoryName"].ToString();

                            string dirPath = "~/Uploads/StudentsReg/Signatures/";
                            string dirPHOTOSPath = "~/Uploads/StudentsReg/Photos/";
                            hfFacultyId.Value = FacultyId;
                            hfCategoryType.Value = CategoryType;
                            if (!string.IsNullOrEmpty(studentPhotoPath))
                            {
                                hfExistingPhotoPath.Value = dirPHOTOSPath + studentPhotoPath;
                                // Use ResolveUrl to get the virtual URL path
                                impPhoto.ImageUrl = ResolveUrl(dirPHOTOSPath + studentPhotoPath);
                            }
                            else
                            {
                                impPhoto.ImageUrl = "";
                                hfExistingPhotoPath.Value = "";
                            }

                            if (!string.IsNullOrEmpty(studentSignaturePath))
                            {
                                // Use ResolveUrl to get the virtual URL path
                                imgSign.ImageUrl = ResolveUrl(dirPath + studentSignaturePath);
                                hfExistingSignaturePath.Value = dirPath + studentSignaturePath;
                            }
                            else
                            {
                                imgSign.ImageUrl = "";
                                hfExistingSignaturePath.Value = "";
                            }
                        }
                        else
                        {
                            impPhoto.ImageUrl = "";
                            imgSign.ImageUrl = "";
                        }
                    }
                    else
                    {
                        impPhoto.ImageUrl = "";
                        imgSign.ImageUrl = "";
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            //string studentId = Request.QueryString["studentId"];
            string encryptedStudentId = Request.QueryString["studentId"];
            string studentId = CryptoHelper.Decrypt(encryptedStudentId);
            if (string.IsNullOrEmpty(studentId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Student ID missing.');", true);
                return;
            }

            // Store only file names in DB
            string photoFileName = string.Empty;
            string signatureFileName = string.Empty;

            // Base virtual paths
            string photoBaseVirtual = "~/Uploads/StudentsReg/Photos";
            string signBaseVirtual = "~/Uploads/StudentsReg/Signatures";

            // Convert to physical paths
            string photoBasePath = Server.MapPath(photoBaseVirtual);
            string signBasePath = Server.MapPath(signBaseVirtual);

            // Ensure directories exist
            if (!System.IO.Directory.Exists(photoBasePath))
                System.IO.Directory.CreateDirectory(photoBasePath);

            if (!System.IO.Directory.Exists(signBasePath))
                System.IO.Directory.CreateDirectory(signBasePath);

            // -------------------- PHOTO Upload --------------------
            if (stuPhoto.HasFile)
            {
                if (stuPhoto.PostedFile.ContentLength > 102400) // 100 KB
                {
                    string photoSizeWarningScript = @"swal({
                    title: 'File Too Large',
                    text: 'Photo file is too large.',
                    icon: 'warning',
                    button: 'OK'
                });";
                    ClientScript.RegisterStartupScript(this.GetType(), "FileSizeWarning", photoSizeWarningScript, true);
                    return;
                }

                string extension = System.IO.Path.GetExtension(stuPhoto.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    string invalidPhotoType = @"swal('Invalid File Type', 'Invalid photo file type.', 'error');";
                    ClientScript.RegisterStartupScript(this.GetType(), "invalidPhotoType", invalidPhotoType, true);
                    return;
                }

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string randomNum = new Random().Next(10000, 99999).ToString();
                photoFileName = randomNum + "_" + timestamp + extension;

                string fullPath = System.IO.Path.Combine(photoBasePath, photoFileName);
                stuPhoto.SaveAs(fullPath);
            }

            // -------------------- SIGNATURE Upload --------------------
            if (stuSignature.HasFile)
            {
                if (stuSignature.PostedFile.ContentLength > 20480) // 20 KB
                {
                    string sweetAlertScript = @"swal('File Too Large', 'Signature file is too large.', 'warning');";
                    ClientScript.RegisterStartupScript(this.GetType(), "sweetalert", sweetAlertScript, true);
                    return;
                }

                string extension = System.IO.Path.GetExtension(stuSignature.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    string invalidTypeScript = @"swal('Invalid File Type', 'Invalid signature file type.', 'error');";
                    ClientScript.RegisterStartupScript(this.GetType(), "invalidFileType", invalidTypeScript, true);
                    return;
                }

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string randomNum = new Random().Next(10000, 99999).ToString();
                signatureFileName = randomNum + "_" + timestamp + extension;

                string fullPath = System.IO.Path.Combine(signBasePath, signatureFileName);
                stuSignature.SaveAs(fullPath);
            }

            // -------------------- Update database --------------------
            bool updated = dl.UpdateStudentPhotoAndSignature(studentId, photoFileName, signatureFileName);
            string reshistory = dl.InsertStudentUploadHistory(Convert.ToInt32(studentId), photoFileName, signatureFileName);

            // -------------------- Redirect URLs --------------------
            string FacultyId = hfFacultyId.Value;
            string CategoryType = hfCategoryType.Value;
            string ExamTypeId = Request.QueryString["ExamTypeId"];
            string url = "";

            if (!string.IsNullOrEmpty(ExamTypeId))
            {
     
                url = "ViewExamDetalis.aspx?studentId=" + Server.UrlEncode(encryptedStudentId) +
                      "&ExamTypeId=" + Server.UrlEncode(ExamTypeId);
            }
            else
            {
                url = "viewstudentregdetalis.aspx?studentId=" + Server.UrlEncode(studentId) +
                      "&CategoryType=" + Server.UrlEncode(CategoryType);
            }

            // -------------------- Final SweetAlert Success --------------------
            string successScript =
            "<script src='https://unpkg.com/sweetalert/dist/sweetalert.min.js'></script>" +
            "<script>" +
            "swal({" +
            "title: 'Success!'," +
            "text: 'Student Details updated successfully.'," +
            "icon: 'success'" +
            "}).then(function() {" +
            "window.location = '" + url + "';" +
            "});" +
            "</script>";

            ClientScript.RegisterStartupScript(this.GetType(), "success", successScript);
        }
        catch (Exception ex)
        {
            // Log error or show alert
            string errorScript = "swal('Error', 'An error occurred: {ex.Message}', 'error');";
            ClientScript.RegisterStartupScript(this.GetType(), "error", errorScript, true);
        }
    }




    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = Request.QueryString["studentId"];
            string FacultyId = hfFacultyId.Value;
            string url = "StudentSubjectgrps.aspx?studentId=" + Server.UrlEncode(studentId) +
                         "&FacultyId=" + Server.UrlEncode(FacultyId) +
                         "&from=photoPage";
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

            throw;
        }

    }


}
