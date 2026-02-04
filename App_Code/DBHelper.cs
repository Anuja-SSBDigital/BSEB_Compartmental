using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf.qrcode;
using System.IdentityModel.Protocols.WSTrust;
using System.Globalization;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
public class DBHelper
{
    string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
    public readonly ILog log = LogManager.GetLogger
          (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    #region Anuja

    public string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }


    public DataTable LoginUser(string username, string password, out string message)
    {

        DataTable dt = new DataTable();
        message = "";
        try
        {


            string hashedPassword = ComputeSha256Hash(password);

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(isSuccessParam);

                SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(messageParam);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);

                message = messageParam.Value.ToString();
                con.Close();
            }
        }
        catch (Exception ex)
        {

            log.Error("Error in LoginUser: ", ex);
            throw;

        }

        return dt;
    }

    public DataTable getFacultyfordropdown()
    {

        DataTable resultTable = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Pk_FacultyId,FacultyName FROM Faculty_Mst where isactive='1' order by FacultyName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getFacultyfordropdown: ", ex);
            throw;
        }
        return resultTable;

    }

    public bool ClearStudentRegCard(string studentId)
    {
        try
        {
            string query = "UPDATE Student_Mst SET RegCardPath = NULL, IsRegCardUploaded = 0 WHERE Pk_StudentId = @Pk_StudentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Pk_StudentId", studentId);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in ClearStudentRegCard: ", ex);
            throw;
        }

    }


    public string AddOrUpdateCorrectionHistory(int Fk_StudentId, int Fk_CollegeId, string StudentFullName, string FatherName, string MotherName, string UpdatedBy, string mobile,
      string email, string address, string maritalStatus, string pincode, string branchName, string ifscCode, string bankACNo, string identification1, string identification2, string medium, int examTypeId, string aadharNumber, string aadharFileName, string district, string subdivision, string MatrixBoard,
      string RollCode, string RollNumber, string PassingYear, string Gender, string CasteCategory, string Nationality, string Religion, string DOB,int differentlyAbled)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString))
        using (SqlCommand cmd = new SqlCommand("usp_UpdateStudentDetails", con))
        //using (SqlCommand cmd = new SqlCommand("sp_AddOrUpdateCorrectionHistory", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", Fk_StudentId);
            //cmd.Parameters.AddWithValue("@FK_CollegeId", Fk_CollegeId);
            cmd.Parameters.AddWithValue("@StudentFullName", StudentFullName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FatherName", FatherName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MotherName", MotherName ?? (object)DBNull.Value);
            //cmd.Parameters.AddWithValue("@NewDOB", NewDOB);
            //cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Mobile", mobile);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
            cmd.Parameters.AddWithValue("@Pincode", pincode);
            cmd.Parameters.AddWithValue("@BranchName", branchName);
            cmd.Parameters.AddWithValue("@IFSCCode", ifscCode);
            cmd.Parameters.AddWithValue("@BankACNo", bankACNo);
            cmd.Parameters.AddWithValue("@Identification1", identification1);
            cmd.Parameters.AddWithValue("@Identification2", identification2);
            cmd.Parameters.AddWithValue("@Medium", medium);
            cmd.Parameters.AddWithValue("@AadharNumber", string.IsNullOrEmpty(aadharNumber) ? (object)DBNull.Value : aadharNumber);
            //cmd.Parameters.AddWithValue("@ExamTypeId", examTypeId);
            //cmd.Parameters.AddWithValue("@AadharNumber", string.IsNullOrEmpty(aadharNumber) ? (object)DBNull.Value : aadharNumber);
            cmd.Parameters.AddWithValue("@AadharFileName", string.IsNullOrEmpty(aadharFileName) ? (object)DBNull.Value : aadharFileName);
            cmd.Parameters.AddWithValue("@District", district);
            cmd.Parameters.AddWithValue("@SubDivision", subdivision);

            // --- New parameters ---
            cmd.Parameters.AddWithValue("@MatrixBoard", MatrixBoard);
            cmd.Parameters.AddWithValue("@RollCode", RollCode);
            cmd.Parameters.AddWithValue("@RollNumber", RollNumber);
            cmd.Parameters.AddWithValue("@PassingYear", PassingYear);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@CasteCategory", CasteCategory);
            cmd.Parameters.AddWithValue("@Nationality", Nationality);
            cmd.Parameters.AddWithValue("@Religion", Religion);
            cmd.Parameters.AddWithValue("@DOB", DOB ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsDifferentlyAbled", differentlyAbled);


            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return "Correction saved successfully.";
            }
            catch (SqlException ex)
            {
                // Handle custom RAISERROR messages from SQL
                return ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }
        }
    }

    public int UpdateImpQueComExaminationForm(int studentId, string maritalStatus, string Gender, string CasteCategory, string Nationality, string Religion, int IsDifferentlyAbled, string medium)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateImpQueComExaminationForm", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@CasteCategory", CasteCategory);
                cmd.Parameters.AddWithValue("@Nationality", Nationality);
                cmd.Parameters.AddWithValue("@Religion", Religion);
                cmd.Parameters.AddWithValue("@IsDifferentlyAbled", IsDifferentlyAbled);
                cmd.Parameters.AddWithValue("@medium", medium);


                var outParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();
                return (int)(outParam.Value ?? -1);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool UpdateDummyDownloadStatus(string studentId, string fromPage)
    {
        try
        {
            string query = "";
            if (fromPage == "StudentExamDummyCard")
            {
                query = "UPDATE Student_Mst SET IsDummyDownloadByStudent = 1 WHERE Pk_StudentId = @StudentId";
            }
            else
            {
                query = "UPDATE Student_Mst SET IsDummmyDownloadByCollege = 1 WHERE Pk_StudentId = @StudentId";
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();  // <– This is allowed INSIDE helper
                        con.Close();

                        return rows > 0;   // true if updated
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // optional: log error
            throw;
        }
    }
    public bool UpdateOrderidofrazorpay(string ClientTxnId,string BankTxnId)
    {
        try
        {
            string query = "UPDATE Payment_Details SET BankTxnId = @BankTxnId WHERE ClientTxnId = @ClientTxnId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@BankTxnId", BankTxnId);
                cmd.Parameters.AddWithValue("@ClientTxnId", ClientTxnId);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateOrderidofrazorpay: ", ex);
            throw;
        }

    }

    //public string Updatepaymentdataofhdfcorderidwise(
    //string BankTxnId,
    //string AmountPaid,
    //string PaymentStatus,
    //string PaymentMode,
    //string GatewayTxnId,
    //string PaymentUpdatedDate,
    //string BankErrorCode)
    //{
    //    try
    //    {
    //        string query = @"UPDATE Payment_Details 
    //              SET AmountPaid = @AmountPaid,
    //                  PaymentStatus = @PaymentStatus,
    //                  PaymentMode = @PaymentMode,
    //                  GatewayTxnId = @GatewayTxnId,
    //                  PaymentUpdatedDate = @PaymentUpdatedDate,
    //                  BankErrorCode = @BankErrorCode
    //              WHERE BankTxnId = @BankTxnId";

    //        using (SqlConnection con = new SqlConnection(connectionString))
    //        using (SqlCommand cmd = new SqlCommand(query, con))
    //        {
    //            cmd.Parameters.AddWithValue("@BankTxnId", BankTxnId ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@AmountPaid", AmountPaid ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@PaymentStatus", PaymentStatus ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@PaymentMode", PaymentMode ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@GatewayTxnId", GatewayTxnId ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@PaymentUpdatedDate", PaymentUpdatedDate ?? (object)DBNull.Value);
    //            cmd.Parameters.AddWithValue("@BankErrorCode", BankErrorCode ?? (object)DBNull.Value);

    //            con.Open();
    //            int rowsAffected = cmd.ExecuteNonQuery();

    //            if (rowsAffected > 0)
    //                return "0";
    //            else
    //                return "No record found for the given BankTxnId.";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error("Error in Updatepaymentdataofhdfcorderidwise: ", ex);
    //        return "Error: " + ex.Message;
    //    }

    //}


    public string UpdatePaymentDataByBankTxnIdUsingSP(
     string BankTxnId,
     string AmountPaid,
     string PaymentStatus,
     string PaymentMode,
     string GatewayTxnId,
     string PaymentUpdatedDate,
     string BankErrorCode)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_HDFCUpdatePaymentDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parameters — same as SP
                cmd.Parameters.AddWithValue("@BankTxnId", string.IsNullOrEmpty(BankTxnId) ? (object)DBNull.Value : BankTxnId);
                cmd.Parameters.AddWithValue("@AmountPaid", string.IsNullOrEmpty(AmountPaid) ? (object)DBNull.Value : Convert.ToDecimal(AmountPaid));
                cmd.Parameters.AddWithValue("@PaymentStatus", PaymentStatus ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentMode", PaymentMode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GatewayTxnId", GatewayTxnId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentUpdatedDate",
                    string.IsNullOrEmpty(PaymentUpdatedDate) ? (object)DBNull.Value : Convert.ToDateTime(PaymentUpdatedDate));
                cmd.Parameters.AddWithValue("@BankErrorCode", BankErrorCode ?? (object)DBNull.Value);

                con.Open();

                // Execute and capture result (SP returns SELECT StatusCode)
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string status = result.ToString();
                    if (status == "0")
                        return "0"; // Success
                    else if (status == "2")
                        return "2"; // Record not found
                    else
                        return "Unknown status: " + status;
                }
                else
                {
                    return "No response from stored procedure.";
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdatePaymentDataByBankTxnIdUsingSP: ", ex);
            return "Error: " + ex.Message;
        }
    }



    //public DataTable getcollegefordropdown()
    //{
    //    DataTable resultTable = new DataTable();
    //    string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
    //    try
    //    {
    //        using (SqlConnection connection = new SqlConnection(connectionString))
    //        {
    //            connection.Open();

    //            string query = "SELECT * FROM College_Mst order by CollegeCode ASC";

    //            using (SqlCommand command = new SqlCommand(query, connection))
    //            {
    //                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
    //                {
    //                    adapter.Fill(resultTable);
    //                }
    //            }
    //            connection.Close();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error("Error in getcollegefordropdown: ", ex);
    //        throw;
    //    }
    //    return resultTable;

    //}

    public DataTable getStudentData(string CollegeId, string CollegeCode, string StudentName, string facultyId, string subCategory)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudentDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrWhiteSpace(CollegeCode))
                {
                    cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                }

                cmd.Parameters.AddWithValue("@StudentName", StudentName);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);


                if (!string.IsNullOrWhiteSpace(subCategory))
                    cmd.Parameters.AddWithValue("@SubCategory", subCategory);
                else
                    cmd.Parameters.AddWithValue("@SubCategory", DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getStudentData: ", ex);
            throw;
        }

        return dt;
    }

    public DataTable GetStudentInterRegiFormData(int studentId, string CollegeId, string FacultyId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetStudentInterRegiFormData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@FacultyId", FacultyId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentInterRegiFormData: ", ex);
            throw;
        }
        return dt;
    }



    public DataTable GetSubjectsByGroup(string FacultyId, string CollegeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubjectsByGroup", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }

        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectsByGroup: ", ex);
            throw;
        }
        return dt;
    }


    public void UpdateStudentsDownloaded(int studentIds)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Student_Mst SET RegisterFormDownloaded = 1 WHERE Pk_StudentId = @StudentId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentIds);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentsDownloaded: ", ex);
            throw;
        }
    }

    public string InsertStudentPaymentDetails(int collegeId, int feeTypeId, string bankGateway, decimal paymentAmount, string studentIds)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertPaymentWithStudents", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameters
                cmd.Parameters.AddWithValue("@Fk_CollegeId", collegeId);
                cmd.Parameters.AddWithValue("@Fk_FeeTypeId", feeTypeId);
                cmd.Parameters.AddWithValue("@BankGateway", bankGateway);
                cmd.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                cmd.Parameters.AddWithValue("@AmountPaid", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentStatus", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentStatusCode", DBNull.Value);
                cmd.Parameters.AddWithValue("@StudentIds", studentIds);

                // Output parameter for result message
                SqlParameter msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                // Output parameter for generated ClientTxnId
                SqlParameter txnIdParam = new SqlParameter("@GeneratedClientTxnId", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(txnIdParam);

                // Return value
                SqlParameter returnParam = new SqlParameter()
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnParam);

                // Execute
                conn.Open();
                cmd.ExecuteNonQuery();

                // Read output values
                string spMessage = (msgParam.Value != null && msgParam.Value != DBNull.Value)
            ? msgParam.Value.ToString()
            : "No message returned";

                string clientTxnId = (txnIdParam.Value != null && txnIdParam.Value != DBNull.Value)
                    ? txnIdParam.Value.ToString()
                    : "N/A";

                int result = Convert.ToInt32(returnParam.Value);
                if (result == 1)
                    return "Success: " + spMessage + " | Transaction ID: " + clientTxnId;
                else
                    return "Failure: " + spMessage;
                conn.Close();
            }
        }
        catch (SqlException sqlEx)
        {
            log.Error("Error in InsertStudentPaymentDetails: ", sqlEx);
            return "SQL Error: " + sqlEx.Message;
        }
        catch (Exception ex)
        {
            log.Error("Error in InsertStudentPaymentDetails: ", ex);
            return "Error: " + ex.Message;
        }
    }

    public string InsertExamStudentPayment(int collegeId, int feeTypeId, string bankGateway, decimal paymentAmount, string studentIds)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertExamPaymentofStudents", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameters
                cmd.Parameters.AddWithValue("@Fk_CollegeId", collegeId);
                cmd.Parameters.AddWithValue("@Fk_FeeTypeId", feeTypeId);
                cmd.Parameters.AddWithValue("@BankGateway", bankGateway);
                cmd.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                cmd.Parameters.AddWithValue("@AmountPaid", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentStatus", DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentStatusCode", DBNull.Value);
                cmd.Parameters.AddWithValue("@StudentIds", studentIds);

                // Output parameter for result message
                SqlParameter msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(msgParam);

                // Output parameter for generated ClientTxnId
                SqlParameter txnIdParam = new SqlParameter("@GeneratedClientTxnId", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(txnIdParam);

                // Return value
                SqlParameter returnParam = new SqlParameter()
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnParam);

                // Execute
                conn.Open();
                cmd.ExecuteNonQuery();

                // Read output values
                string spMessage = (msgParam.Value != null && msgParam.Value != DBNull.Value)
            ? msgParam.Value.ToString()
            : "No message returned";

                string clientTxnId = (txnIdParam.Value != null && txnIdParam.Value != DBNull.Value)
                    ? txnIdParam.Value.ToString()
                    : "N/A";

                int result = Convert.ToInt32(returnParam.Value);
                if (result == 1)
                    return "Success: " + spMessage + " | Transaction ID: " + clientTxnId;
                else
                    return "Failure: " + spMessage;
                conn.Close();
            }
        }
        catch (SqlException sqlEx)
        {
            log.Error("Error in InsertStudentPaymentDetails: ", sqlEx);
            return "SQL Error: " + sqlEx.Message;
        }
        catch (Exception ex)
        {
            log.Error("Error in InsertStudentPaymentDetails: ", ex);
            return "Error: " + ex.Message;
        }
    }


    public int UpdateStudentPaymentDetails(string ClienttxnId, string Paidamount, string Paymentstatus, string Paymentmode, string GatewayTxnId, string bankTxnId, string Paymentdate, string PaymentstatusCode, string GatewayMessage, string ChallanNumber, string GatewayErrorCode, string BankMessage, string BankErrorCode, string IsExamPayment)
    {
        int result = 0;
        try
        {

            DateTime parsedDate;
            string transDateRaw = Paymentdate;

            // Remove 'IST'
            string cleanedDate = transDateRaw.Replace(" IST", "");

            // Define expected format (without IST)
            string format = "ddd MMM dd HH:mm:ss yyyy";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdatePaymentDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PaymentStatus", Paymentstatus);
                cmd.Parameters.AddWithValue("@AmountPaid", Paidamount);
                cmd.Parameters.AddWithValue("@PaymentMode", Paymentmode);
                cmd.Parameters.AddWithValue("@GatewayTxnId", GatewayTxnId);
                cmd.Parameters.AddWithValue("@BankTxnId", bankTxnId);
                if (DateTime.TryParseExact(cleanedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    cmd.Parameters.AddWithValue("@PaymentUpdatedDate", parsedDate);
                }
                else
                {
                    // Handle invalid date string
                    cmd.Parameters.AddWithValue("@PaymentUpdatedDate", DBNull.Value);
                }
                //cmd.Parameters.AddWithValue("@PaymentUpdatedDate", Paymentdate);

                cmd.Parameters.AddWithValue("@ClientTxnId", ClienttxnId);
                cmd.Parameters.AddWithValue("@PaymentStatusCode", PaymentstatusCode);
                cmd.Parameters.AddWithValue("@GatewayMessage", GatewayMessage);
                cmd.Parameters.AddWithValue("@ChallanNumber", ChallanNumber);
                cmd.Parameters.AddWithValue("@GatewayErrorCode", GatewayErrorCode);
                cmd.Parameters.AddWithValue("@BankMessage", BankMessage);
                cmd.Parameters.AddWithValue("@BankErrorCode", BankErrorCode);
                cmd.Parameters.AddWithValue("@IsExamPayment", IsExamPayment);

                // Output parameter for message
                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParameter);

                conn.Open();
                cmd.ExecuteNonQuery();

                result = (int)returnParameter.Value;

                conn.Close();
            }
        }
        catch (SqlException sqlEx)
        {

            log.Error("Error in UpdateStudentPaymentDetails: ", sqlEx);
            throw;

        }
        catch (Exception ex)
        {

            log.Error("Error in UpdateStudentPaymentDetails: ", ex);
            throw;

        }

        return result;
    }


    public int UpdateChallanRecall(
      string clientTxnId,
      string paymentStatus,
      string paymentStatusCode,
      string gatewayMessage,
      string gatewayErrorCode,
      string bankMessage,
      string bankErrorCode,

      string gatewayTxnId,
      string bankTxnId,
      string paidAmount)
    {
        int result = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateChallanRecall", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientTxnId", clientTxnId);
                cmd.Parameters.AddWithValue("@IsDeleted", 0);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentStatusCode", paymentStatusCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GatewayMessage", gatewayMessage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GatewayErrorCode", gatewayErrorCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BankMessage", bankMessage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BankErrorCode", bankErrorCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GatewayTxnId", gatewayTxnId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BankTxnId", bankTxnId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AmountPaid", paidAmount);
                cmd.Parameters.AddWithValue("@PaymentUpdatedDate", DateTime.Now);

                // Output parameter for return value
                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParameter);
                conn.Open();
                cmd.ExecuteNonQuery();

                result = (int)returnParameter.Value;
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateChallanRecall: ", ex);
            throw;
        }

        return result;
    }

       public void RestorePaymentAndStudentPayment(int pkPaymentId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // 1️⃣ Update Payment_Details by Pk_PaymentId
            string updatePaymentQuery =
                "UPDATE Payment_Details SET IsDeleted = 0 WHERE Pk_PaymentId = @PkPaymentId";

            using (SqlCommand cmd1 = new SqlCommand(updatePaymentQuery, conn))
            {
                cmd1.Parameters.AddWithValue("@PkPaymentId", pkPaymentId);
                cmd1.ExecuteNonQuery();
            }

            // 2️⃣ Update Student_Payment_Details by Fk_PaymentId
            string updateStudentPaymentQuery =
                "UPDATE Student_Payment_Details SET IsDeleted = 0 WHERE Fk_PaymentId = @FkPaymentId";

            using (SqlCommand cmd2 = new SqlCommand(updateStudentPaymentQuery, conn))
            {
                cmd2.Parameters.AddWithValue("@FkPaymentId", pkPaymentId);
                cmd2.ExecuteNonQuery();
            }
        }
    }



    public DataTable GetStudentPaymentDetails(int CollegeId, string Collegecode)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetStudentPaymentDetails", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    if (!string.IsNullOrEmpty(Collegecode))
                    {
                        cmd.Parameters.AddWithValue("@CollegeCode", Collegecode);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                    }

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();

                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetStudentPaymentDetails: ", ex);
            throw;

        }
        return dt;

    }


    public DataTable GetExamPaymentDetails(int CollegeId, string Collegecode,int Examid)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetExamPaymentDetails", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;
                    if (!string.IsNullOrEmpty(Collegecode))
                    {
                        cmd.Parameters.AddWithValue("@CollegeCode", Collegecode);

                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                    }
                    cmd.Parameters.AddWithValue("@ExamId", Examid);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetStudentPaymentDetails: ", ex);
            throw;

        }
        return dt;

    }

    public DataSet GetExamPaymentDataDatewise(string bakgateway,DateTime? fromDate, DateTime? toDate)
    {
        DataSet ds = new DataSet();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_PaymentDataDatewise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@BankGateway", bakgateway);
                    if (fromDate.HasValue)
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);

                    if (toDate.HasValue)
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);

                    // Open connection and fill dataset
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetExamPaymentDetailsDatewise: ", ex);
            throw;
        }

        return ds;
    }


    public DataSet GetStdntPaymntDetailsTxnIdwise(string ClienTxnId)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetStudentPaymentDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientTxnId", ClienTxnId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }


                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetStdntPaymntDetailsTxnIdwise: ", ex);
            throw;

        }
        return ds;

    }

    public DataSet GetExmPaymntDetailsTxnIdwise(string ClienTxnId, int ExamId)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetExamPaymentDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientTxnId", ClienTxnId);
                    if (ExamId > 0)
                    {
                        cmd.Parameters.AddWithValue("@ExamId", ExamId);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }


                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetStdntPaymntDetailsTxnIdwise: ", ex);
            throw;

        }
        return ds;

    }

    public int DeletePaymentDetails(string clientTxnId)
    {
        int statusCode = -1;

        try
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeletePaymentDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientTxnId", clientTxnId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        statusCode = Convert.ToInt32(reader["StatusCode"]);
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {

            log.Error("Error in DeletePaymentDetails: ", ex);
            throw;

        }

        return statusCode;
    }

    public void UpdateSeatMatrix(int seatMatrixId, int regularSeats, int privateSeats)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE SeatMatrix_Mst 
                         SET RegularSeats = @RegularSeats, 
                             PrivateSeats = @PrivateSeats 
                         WHERE Pk_SeatMatrixId = @Pk_SeatMatrixId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RegularSeats", regularSeats);
                    cmd.Parameters.AddWithValue("@PrivateSeats", privateSeats);
                    cmd.Parameters.AddWithValue("@Pk_SeatMatrixId", seatMatrixId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {

            log.Error("Error in UpdateSeatMatrix: ", ex);
            throw;

        }
    }
    public DataTable GetSeatMatrixData(string Collegecode)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetSeatMatrixWithDetails", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    if (Collegecode != "")
                    {
                        cmd.Parameters.AddWithValue("@CollegeCode", Collegecode);
                    }

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }

            }

            // 👇 Add the IsEdit column after fetching data
            if (!dt.Columns.Contains("IsEdit"))
            {
                dt.Columns.Add("IsEdit", typeof(bool));
            }

            // Set all IsEdit flags to false initially
            foreach (DataRow row in dt.Rows)
            {
                row["IsEdit"] = false;
            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetSeatMatrixData: ", ex);
            throw;

        }
        return dt;

    }



    public DataTable GetCollegeWiseSeatSummary(int CollegeId, int facultyid)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetCollegeWiseSeatSummary", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    cmd.Parameters.AddWithValue("@FacultyId", facultyid);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetCollegeWiseSeatSummary: ", ex);
            throw;

        }

        return dt;

    }


    public DataTable GetCollegeWiseEXMSeatSummary(int CollegeId, int facultyid,int ExamtypeId)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetCollegeWiseEXMSeatSummary", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    cmd.Parameters.AddWithValue("@FacultyId", facultyid);
                    cmd.Parameters.AddWithValue("@ExamTypeId", ExamtypeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetCollegeWiseSeatSummary: ", ex);
            throw;

        }

        return dt;

    }

    #endregion

    #region Jinal

    public bool UpdateStudentUsingSP(

int studentId,

string name, string father, string mother, string dob, string mobile,

string email, string district, string subDivision,

string CollegeId, string boardRollCode, string boardRollNumber,

string passingYear, string aadharNumber, string address, string pincode,

string bankAccount, string bankBranch, string ifscCode, string idMark1,

string idMark2, string nationalityId, string religionId, string casteCategoryId,

string genderId, string areaId, string maritalStatusId, string mediumId,

string matrixBoardId, string category, string differentlyAbled)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))

            {

                using (SqlCommand cmd = new SqlCommand("sp_UpdateStudent", conn))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    cmd.Parameters.AddWithValue("@StudentName", name);

                    cmd.Parameters.AddWithValue("@FatherName", father);

                    cmd.Parameters.AddWithValue("@MotherName", mother);

                    cmd.Parameters.AddWithValue("@DOB", dob);

                    cmd.Parameters.AddWithValue("@Mobile", mobile);

                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.Parameters.AddWithValue("@District", district);

                    cmd.Parameters.AddWithValue("@SubDivision", subDivision);

                    //cmd.Parameters.AddWithValue("@CollegeCode", collegeCode);

                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                    cmd.Parameters.AddWithValue("@BoardRollCode", boardRollCode);

                    cmd.Parameters.AddWithValue("@BoardRollNumber", boardRollNumber);

                    cmd.Parameters.AddWithValue("@PassingYear", passingYear);

                    cmd.Parameters.AddWithValue("@AadharNumber", aadharNumber);

                    cmd.Parameters.AddWithValue("@Address", address);

                    cmd.Parameters.AddWithValue("@PinCode", pincode);

                    cmd.Parameters.AddWithValue("@BankAccount", bankAccount);

                    cmd.Parameters.AddWithValue("@BankBranch", bankBranch);

                    cmd.Parameters.AddWithValue("@IFSC", ifscCode);

                    cmd.Parameters.AddWithValue("@IdentificationMark1", idMark1);

                    cmd.Parameters.AddWithValue("@IdentificationMark2", idMark2);

                    cmd.Parameters.AddWithValue("@NationalityId", nationalityId);

                    cmd.Parameters.AddWithValue("@ReligionId", religionId);

                    cmd.Parameters.AddWithValue("@CasteCategoryId", casteCategoryId);

                    cmd.Parameters.AddWithValue("@GenderId", genderId);

                    cmd.Parameters.AddWithValue("@AreaId", areaId);

                    cmd.Parameters.AddWithValue("@MaritalStatusId", maritalStatusId);

                    cmd.Parameters.AddWithValue("@MediumId", mediumId);

                    cmd.Parameters.AddWithValue("@MatrixBoardId", matrixBoardId);

                    cmd.Parameters.AddWithValue("@Category", category);

                    cmd.Parameters.AddWithValue("@DifferentlyAbled", differentlyAbled);

                    conn.Open();

                    int result = cmd.ExecuteNonQuery();

                    return result > 0;
                    conn.Close();
                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in UpdateStudentUsingSP: ", ex);
            throw;

        }


    }


    public DataTable GetSubjectDetailsByCode(string SubjectPaperCode)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubjectDetailsByCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectPaperCode ", SubjectPaperCode);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectDetailsByCode: ", ex);
            throw ex;
        }

        return dt;
    }

    public string InsertStudentSubjectUsingSP(string studentId, string subjectPaperId, string subjectGroupId, string modifiedBy, int comGrp)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveStudentPaperApplied", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Fk_StudentId", studentId);
                    cmd.Parameters.AddWithValue("@Fk_SubjectPaperId", subjectPaperId);
                    cmd.Parameters.AddWithValue("@Fk_SubjectGroupId", subjectGroupId);
                    cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                    cmd.Parameters.AddWithValue("@ComGrp", comGrp);

                    con.Open();
                    cmd.ExecuteNonQuery(); // Use this instead of SqlDataAdapter
                    con.Close();
                }
            }

            return "Success";
        }
        catch (Exception ex)
        {
            log.Error("Error in InsertStudentSubjectUsingSP: ", ex);
            return "Error: " + ex.Message;
        }



    }
    public DataTable GetStudentDetails(string studentId)
    {
        try
        {
            // SQL query to fetch student photo and signature paths
            string query = "SELECT OfssReferenceNo,StudentPhotoPath, StudentSignaturePath,SM.Fk_FacultyId as FacultyId,SM.CategoryName FROM Student_Mst SM INNER JOIN Faculty_Mst FC ON SM.Fk_FacultyId = FC.Pk_FacultyId WHERE Pk_StudentId = @StudentId";

            // Create connection and command
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the studentId parameter
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    // Create data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDetails: ", ex);
            throw ex;
        }
    }

    public int ExecuteNonQuery(SqlCommand cmd)

    {

        using (SqlConnection conn = new SqlConnection(connectionString))

        {

            cmd.Connection = conn;

            conn.Open();

            return cmd.ExecuteNonQuery();
            conn.Close();
        }

    }

    public bool UpdateStudentPhotoAndSignature(string studentId, string photoPath, string signaturePath)
    {
        try
        {
            // Build list of columns to update dynamically
            List<string> updates = new List<string>();

            if (!string.IsNullOrEmpty(photoPath))
                updates.Add("StudentPhotoPath = @Photo");

            if (!string.IsNullOrEmpty(signaturePath))
                updates.Add("StudentSignaturePath = @Sign");

            // If nothing to update, return false
            if (updates.Count == 0)
                return false;

            string query = "UPDATE Student_Mst SET " + string.Join(", ", updates) + " WHERE Pk_StudentId = @StudentId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    if (!string.IsNullOrEmpty(photoPath))
                        cmd.Parameters.AddWithValue("@Photo", photoPath);

                    if (!string.IsNullOrEmpty(signaturePath))
                        cmd.Parameters.AddWithValue("@Sign", signaturePath);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentPhotoAndSignature: ", ex);
            return false;
        }
    }


    public DataTable GetAppliedSubjects(string studentId)
    {
        try
        {
            string query = @"SELECT sp.SubjectPaperCode,Fk_SubjectPaperId as SubjectPaperId,ComGrp,Pk_StudentPaperAppliedId
                     FROM StudentPaperApplied_Mst spa
                     INNER JOIN SubjectPaper_Mst sp ON spa.Fk_SubjectPaperId = sp.Pk_SubjectPaperId
                     WHERE spa.Fk_StudentId = @StudentId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the studentId parameter
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    // Create data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetAppliedSubjects: ", ex);
            throw ex;
        }
    }

    public void UpdateStudentSubjectByPkId(string pkId, string subjectPaperId, string subjectGroupId, string modifiedBy)
    {
        string query = @"
        UPDATE StudentPaperApplied_Mst
        SET 
            Fk_SubjectPaperId = @SubjectPaperId,
            Fk_SubjectGroupId = @SubjectGroupId,
            LastModifiedOn = GETDATE(),
            LastModifiedBy = @ModifiedBy
        WHERE Pk_StudentPaperAppliedId = @PkId";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SubjectPaperId", subjectPaperId);
                cmd.Parameters.AddWithValue("@SubjectGroupId", subjectGroupId);
                cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                cmd.Parameters.AddWithValue("@PkId", pkId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentSubjectByPkId: ", ex);

            throw;
        }
    }

    public DataTable GetStudentSubjectsListByStudentId(int studentId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentSubjectsListByStudentId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId ", studentId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentSubjectsListByStudentId: ", ex);

            throw ex;
        }

        return dt;
    }



    public DataTable getNationalityfordropdown()

    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try

        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_NationalityId,Nationality  FROM Nationality_Mst where IsActive='1' order by Nationality ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)

        {
            log.Error("Error in getNationalityfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }


    public DataTable getcollegeidbasedonCollegecode(string CollegeCode)
    {
        DataTable resultTable = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Pk_CollegeId,CollegeName,CollegeCode,PrincipalMobileNo,EmailId FROM College_Mst WHERE CollegeCode = @CollegeCode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CollegeCode", CollegeCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getcollegeidbasedonCollegecode: ", ex);

            throw;
        }

        return resultTable;
    }

    public DataTable getChallanDetailsbasedonTxnId(string CollegeCode, int FeeTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetChallanRecallData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                    cmd.Parameters.AddWithValue("@FeeTypeId", FeeTypeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }

            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getChallanDetailsbasedonTxnId: ", ex);

            throw;
        }

        return dt;
    }

    public DataTable getGenderfordropdown()
    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_GenderId,GenderName FROM Gender_Mst where IsActive='1' order by GenderName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)
        {
            log.Error("Error in getGenderfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }

    public DataTable getReligionfordropdown()

    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try

        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_ReligionId,Religion FROM Religion_Mst where IsActive='1' order by Religion ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)
        {
            log.Error("Error in getReligionfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }

    public DataTable getBoardMasterfordropdown()

    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try

        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_BoardId,BoardName FROM BoardMaster";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)

        {
            log.Error("Error in getBoardMasterfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }

    public DataTable getMarital_Mstfordropdown()
    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string query = "SELECT Pk_MaritalStatusId,MaritalStatus FROM Marital_Mst where IsActive='1' order by MaritalStatus ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();
            }

        }

        catch (Exception ex)
        {
            log.Error("Error in getMarital_Mstfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }

    public DataTable getArea_Mstfordropdown()

    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try

        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_AreaId,AreaName  FROM Area_Mst where IsActive='1' order by AreaName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }
        catch (Exception ex)
        {
            log.Error("Error in getArea_Mstfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }

    public DataTable getMedium_Mstfordropdown()

    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try

        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT Pk_ExamMediumId,ExamMediumName FROM Medium_Mst where IsActive='1' order by ExamMediumName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)
        {
            log.Error("Error in getMedium_Mstfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }


    public DataTable GetSubmittedExamFormList(string CollegeId, string CollegeCode, string StudentName, string facultyId)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetSubmittedExamFormList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrWhiteSpace(CollegeCode))
                {
                    cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                }

                cmd.Parameters.AddWithValue("@StudentName", StudentName);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubmittedExamFormList: ", ex);
            throw;
        }

        return dt;
    }
    public DataTable getCasteCategory_Mstfordropdown()
    {

        DataTable resultTable = new DataTable();

        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))

            {

                connection.Open();

                string query = "SELECT PK_CasteCategoryId,CasteCategoryName,CasteCategoryCode FROM CasteCategory_Mst where IsActive='1' order by CasteCategoryName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))

                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))

                    {

                        adapter.Fill(resultTable);

                    }

                }
                connection.Close();

            }

        }

        catch (Exception ex)
        {
            log.Error("Error in getCasteCategory_Mstfordropdown: ", ex);

            throw;

        }

        return resultTable;

    }


    public DataTable GetStudentRegiListData(string CollegeId, string facultyId, string RegistrationMode, string CategoryType, string StudentName)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudentRegiListData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@RegistrationMode", RegistrationMode);

                if (!string.IsNullOrWhiteSpace(CategoryType))
                    cmd.Parameters.AddWithValue("@CategoryType", CategoryType);
                else
                    cmd.Parameters.AddWithValue("@CategoryType", DBNull.Value);
                cmd.Parameters.AddWithValue("@StudentName", StudentName);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentRegiListData: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable ViewStudentRegDetails(string studentId, string CategoryType)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ViewStudentRegDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    cmd.Parameters.AddWithValue("@CategoryType", CategoryType);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in ViewStudentRegDetails: ", ex);

            throw ex;
        }

        return dt;
    }

    public int AddUpdateStudent(
       int studentId, int FacultyId,
       string name, string father, string mother, string dob, string mobile,
       string email, string district, string subDivision,
       int CollegeId, string boardRollCode, string boardRollNumber,
       string passingYear, string aadharNumber, string address, string pincode,
       string bankAccount, string bankBranch, string ifscCode, string idMark1,
       string idMark2, int nationalityId, int religionId, int casteCategoryId,
       int genderId, int areaId, int maritalStatusId, int mediumId,
       string matrixBoardId, string category, string differentlyAbled, string ApaarId, string Uniqueid
   )
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddUpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentId == 0 ? (object)DBNull.Value : studentId);
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId == 0 ? (object)DBNull.Value : FacultyId);
                    cmd.Parameters.AddWithValue("@StudentName", name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FatherName", father ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MotherName", mother ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DOB", dob ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", mobile ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@District", district ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubDivision", subDivision ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId == 0 ? (object)DBNull.Value : CollegeId);

                    // ✅ BoardRollCode / BoardRollNumber handling
                    if (string.IsNullOrWhiteSpace(boardRollCode) || boardRollCode == "0")
                        cmd.Parameters.AddWithValue("@BoardRollCode", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@BoardRollCode", Convert.ToInt32(boardRollCode));

                    if (string.IsNullOrWhiteSpace(boardRollNumber))
                        cmd.Parameters.AddWithValue("@BoardRollNumber", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@BoardRollNumber", boardRollNumber);

                    cmd.Parameters.AddWithValue("@PassingYear", passingYear ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AadharNumber", aadharNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PinCode", pincode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BankAccount", bankAccount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BankBranch", bankBranch ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IFSC", ifscCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdentificationMark1", idMark1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdentificationMark2", idMark2 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@NationalityId", nationalityId == 0 ? (object)DBNull.Value : nationalityId);
                    cmd.Parameters.AddWithValue("@ReligionId", religionId == 0 ? (object)DBNull.Value : religionId);

                    cmd.Parameters.AddWithValue("@CasteCategoryId", casteCategoryId == 0 ? (object)DBNull.Value : casteCategoryId);
                    cmd.Parameters.AddWithValue("@GenderId", genderId == 0 ? (object)DBNull.Value : genderId);
                    cmd.Parameters.AddWithValue("@AreaId", areaId == 0 ? (object)DBNull.Value : areaId);
                    cmd.Parameters.AddWithValue("@MaritalStatusId", maritalStatusId == 0 ? (object)DBNull.Value : maritalStatusId);
                    cmd.Parameters.AddWithValue("@MediumId", mediumId == 0 ? (object)DBNull.Value : mediumId);

                    // ✅ MatrixBoardId handling
                    if (string.IsNullOrWhiteSpace(matrixBoardId) || matrixBoardId == "0")
                        cmd.Parameters.AddWithValue("@MatrixBoardId", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@MatrixBoardId", Convert.ToInt32(matrixBoardId));

                    cmd.Parameters.AddWithValue("@Category", category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DifferentlyAbled", differentlyAbled ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ApaarId", ApaarId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Uniqueid", Uniqueid ?? (object)DBNull.Value);

                    conn.Open();

                    // ExecuteScalar returns the first column of the first row from the result set (which is StudentId)
                    object result = cmd.ExecuteScalar();
                    int studentIdResult;
                    if (result != null && int.TryParse(result.ToString(), out studentIdResult))
                    {
                        return studentIdResult;
                    }
                    else
                    {
                        return 0; // or handle error accordingly
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in AddUpdateStudent: ", ex);
            throw;
        }
    }




    public DataTable getPrivateRegFacultyfordropdown()
    {
        DataTable resultTable = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Pk_FacultyId,FacultyName FROM Faculty_Mst where isactive='1' and Pk_FacultyId in(2,3) order by FacultyName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getPrivateRegFacultyfordropdown: ", ex);

            throw;
        }
        return resultTable;

    }

    public DataTable ViewPrivateStudentRegDetails(string CollegeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT stu.CategoryName, FC.FacultyName AS Faculty, stu.Fk_FacultyId AS FacultyId, " +
                               "CU.CollegeName + ' | ' + CU.CollegeCode AS College, CU.CollegeCode " +
                               "FROM Student_Mst stu " +
                               "INNER JOIN Faculty_Mst FC ON stu.Fk_FacultyId = FC.Pk_FacultyId " +
                               "INNER JOIN College_Mst CU ON stu.Fk_CollegeId = CU.Pk_CollegeId " +
                               //"INNER JOIN CollegeUsers CU ON stu.Fk_CollegeId = CU.Id " +
                               "WHERE stu.Fk_CollegeId = @CollegeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter to avoid the error
                    command.Parameters.AddWithValue("@CollegeId", CollegeId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in ViewPrivateStudentRegDetails: ", ex);

            throw ex;
        }
        return dt;
    }

    public bool UpdateStudentRegForm(string studentId)
    {

        try
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("UPDATE Student_Mst SET IsRegFormSubmit=1,UpdateDate = GETDATE() WHERE Pk_StudentId = @studentId"))
                {

                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    return ExecuteNonQuery(cmd) > 0;

                }

            }

        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentRegForm: ", ex);

            return false;

        }

    }


    public int GetVocationalSubjectCount(string facultyId, string collegeId)
    {
        int count = 0;

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetVocationalStudentCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        count = Convert.ToInt32(result);
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetVocationalSubjectCount: ", ex);

            throw;
        }

        return count;
    }

    public DataTable GetCollegeMasterByCollegeCode(string CollegeCode)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCollegeMasterByCollegeCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeCode ", CollegeCode);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetCollegeMasterByCollegeCode: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetCollegeUserDetailsByCollegeCode(string CollegeCode)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCollegeMasterDetailsByCollegeCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeCode ", CollegeCode);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetCollegeUserDetailsByCollegeCode: ", ex);

            throw ex;
        }

        return dt;
    }
    public bool UpdateCollegeDetails(int collegeId, string principalName, string principalMobile, string email, string CollegeName)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCollegeMasterDetailsByCollegeCode", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);
                    cmd.Parameters.AddWithValue("@PrincipalName", principalName);
                    cmd.Parameters.AddWithValue("@PrincipalMobileNo", principalMobile);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@CollegeName", CollegeName);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateCollegeDetails: ", ex);

            throw ex;
        }
    }

    public int CheckVocationalCollegeSubjectExists(int fkCollegeId, int fkFacultyId)
    {
        const string spName = "sp_CheckVocationalCollegeSubjectExists";

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(spName, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Fk_CollegeId", SqlDbType.Int).Value = fkCollegeId;
            cmd.Parameters.Add("@Fk_FacultyId", SqlDbType.Int).Value = fkFacultyId;

            conn.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (rdr.Read()) // move to the first row if exists
                {
                    return Convert.ToInt32(rdr["Result"]);
                }
                else
                {
                    return 0; // no row returned
                }
            }
        }
    }

    public bool UpdateUserPassword(string userName, string hashedPassword, string PlainPassword)
    {
        try
        {
            string query = "UPDATE CollegeUsers SET [Password] = @Password, UpdatedDate = GETDATE(), PlainTextPassword=@PlainPassword WHERE UserName = @UserName";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@PlainPassword", PlainPassword);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateUserPassword: ", ex);

            throw;
        }
    }

    public DataTable GetSubjectPapersByFacultyAndGroup(string FacultyId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetSubjectPapersByFacultyAndGroup", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FacultyId", FacultyId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectPapersByFacultyAndGroup: ", ex);

            throw ex;
        }
        return dt;
    }

    public bool UpdateStudentRegCard(string studentId, string regCardPath)
    {
        try
        {
            string query = "UPDATE Student_Mst SET RegCardPath = @RegCardPath, IsRegCardUploaded = 1 WHERE Pk_StudentId = @Pk_StudentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Pk_StudentId", studentId);
                cmd.Parameters.AddWithValue("@RegCardPath", regCardPath);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentRegCard: ", ex);

            throw ex;
        }

    }

    public void DeleteStudentPaperAppliedById(int pkId)
    {
        string query = "DELETE FROM StudentPaperApplied_Mst WHERE Pk_StudentPaperAppliedId = @PkId";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PkId", pkId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in DeleteStudentPaperAppliedById: ", ex);
            // Optionally rethrow if needed
            throw;
        }
    }
    public DataTable GetSubjectsByStudentIdAndComGrp(string studentId, int comGrp)
    {
        string query = @"
        SELECT Pk_StudentPaperAppliedId, SubjectPaperCode
        FROM StudentPaperApplied_Mst
        INNER JOIN SubjectPaper_Mst 
        ON StudentPaperApplied_Mst.Fk_SubjectPaperId = SubjectPaper_Mst.Pk_SubjectPaperId
        WHERE Fk_StudentId = @StudentId AND ComGrp = @ComGrp";

        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@ComGrp", comGrp);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt); // Fills the DataTable with query results
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectsByStudentIdAndComGrp: ", ex);
            // Optionally rethrow if needed
            throw;
        }
        return dt;
    }

    public DataTable GetStudentDummyRegCertificateData(int studentId, string collegeId, string facultyId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentDummyRegiFormData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);
                    cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDummyRegCertificateData: ", ex);
            // Optionally rethrow if needed
            throw;
        }
        return dt;
    }

    public DataTable GetStudentDummyRegData(string CollegeId, string facultyId)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudentDummyRegData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDummyRegData: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetDummyStudentSubjectDetails(int studentId, int collegeId, int facultyId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetDummyStudentSubjectDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentID", studentId);
                    cmd.Parameters.AddWithValue("@facultyId", facultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetDummyStudentSubjectDetails: ", ex);
            throw ex;
        }

        return dt;
    }

    public DataTable GetStudentDownloadDummyRegCardData(int Collegecode, int FacultyId, string StudentName, string DOB)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentDownloadDummyRegCardData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Collegecode", Collegecode);
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId == 0 ? (object)DBNull.Value : FacultyId);
                    cmd.Parameters.AddWithValue("@StudentName", string.IsNullOrWhiteSpace(StudentName) ? (object)DBNull.Value : StudentName);

                    if (string.IsNullOrWhiteSpace(DOB))
                    {
                        cmd.Parameters.AddWithValue("@DOB", DBNull.Value);
                    }
                    else
                    {
                        string[] acceptedFormats = new string[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                        DateTime parsedDOB;

                        if (DateTime.TryParseExact(DOB, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDOB))
                        {
                            // Format to dd-MM-yyyy to match SQL format 105
                            cmd.Parameters.AddWithValue("@DOB", parsedDOB);
                        }
                        else
                        {
                            throw new FormatException("DOB '" + DOB + "' is not in a valid format. Expected yyyy-MM-dd or dd/MM/yyyy or MM/dd/yyyy.");
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDownloadDummyRegCardData: ", ex);
            throw;
        }
    }

    public DataTable getExamCatfordropdown()
    {

        DataTable resultTable = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Pk_ExamTypeId,ExamTypeName FROM ExamType_Mst where IsActive='1' order by ExamTypeName ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getExamCatfordropdown: ", ex);
            throw;
        }
        return resultTable;

    }

    public DataTable getExamDwnldStudentData(string CollegeId, string StudentName, int facultyId, int ExamId, string CategoryName, string subCategory)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetExamDwnldStudentData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;
                //if (!string.IsNullOrWhiteSpace(CollegeCode))
                //{
                //    cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                //}
                //else
                //{
                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                //}

                cmd.Parameters.AddWithValue("@StudentName", StudentName);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@ExamId", ExamId);
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);


                if (!string.IsNullOrWhiteSpace(subCategory))
                    cmd.Parameters.AddWithValue("@SubCategory", subCategory);
                else
                    cmd.Parameters.AddWithValue("@SubCategory", DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in getStudentData: ", ex);
            throw;
        }

        return dt;
    }



    public int UpdateStudentExamRegForm(
     int studentId,
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
     int examTypeId,
     string aadharNumber,
     string aadharFileName,
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
     string DOB
     )
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateStudentExamRegForm", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                cmd.Parameters.AddWithValue("@Pincode", pincode);
                cmd.Parameters.AddWithValue("@BranchName", branchName);
                cmd.Parameters.AddWithValue("@IFSCCode", ifscCode);
                cmd.Parameters.AddWithValue("@BankACNo", bankACNo);
                cmd.Parameters.AddWithValue("@Identification1", identification1);
                cmd.Parameters.AddWithValue("@Identification2", identification2);
                cmd.Parameters.AddWithValue("@Medium", medium);
                cmd.Parameters.AddWithValue("@ExamTypeId", examTypeId);
                cmd.Parameters.AddWithValue("@AadharNumber", string.IsNullOrEmpty(aadharNumber) ? (object)DBNull.Value : aadharNumber);
                cmd.Parameters.AddWithValue("@AadharFileName", string.IsNullOrEmpty(aadharFileName) ? (object)DBNull.Value : aadharFileName);
                cmd.Parameters.AddWithValue("@District", district);
                cmd.Parameters.AddWithValue("@SubDivision", subdivision);

                // --- New parameters ---
                cmd.Parameters.AddWithValue("@MatrixBoard", MatrixBoard);
                cmd.Parameters.AddWithValue("@RollCode", RollCode);
                cmd.Parameters.AddWithValue("@RollNumber", RollNumber);
                cmd.Parameters.AddWithValue("@PassingYear", PassingYear);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@CasteCategory", CasteCategory);
                cmd.Parameters.AddWithValue("@Nationality", Nationality);
                cmd.Parameters.AddWithValue("@Religion", Religion);
                cmd.Parameters.AddWithValue("@DOB", DOB ?? (object)DBNull.Value);


                var outParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();
                return (int)(outParam.Value ?? -1);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetExamStudentRegiListData(int CollegeId, int facultyId, int ExamId, string StudentName)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetExamFormStudentRegiListData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@ExamId", ExamId);
                //cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                //cmd.Parameters.AddWithValue("@RegistrationType", RegistrationType);

                //if (!string.IsNullOrWhiteSpace(CategoryType))
                //    cmd.Parameters.AddWithValue("@CategoryType", CategoryType);
                //else
                //    cmd.Parameters.AddWithValue("@CategoryType", DBNull.Value);
                cmd.Parameters.AddWithValue("@StudentName", StudentName);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in sp_GetExamFormStudentRegiListData: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetDwnldExaminationFormData(int studentId, int CollegeId, int FacultyId, int ExamTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetDwnldExaminationFormData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@FacultyId", FacultyId);
                cmd.Parameters.AddWithValue("@ExamTypeId", ExamTypeId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentInterRegiFormData: ", ex);
            throw;
        }
        return dt;
    }

    public DataTable GetStudentExamRegDetails(int studentId, int ExamTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentExamRegDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    //cmd.Parameters.AddWithValue("@registrationType", registrationType);
                    cmd.Parameters.AddWithValue("@ExamTypeId", ExamTypeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentExamRegDetails: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetStudentExamSubjectsList(int studentId, int examTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentSubjectsListByStudentId", con))//not change in sp and not also create sp 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId ", studentId);
                    cmd.Parameters.AddWithValue("@examTypeId ", examTypeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentSubjectsListByStudentId: ", ex);

            throw ex;
        }

        return dt;
    }

    public bool UpdateStudentExamForm(string studentId)
    {

        try
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("UPDATE Student_Mst SET IsExamFormSubmit=1,UpdateDate = GETDATE() WHERE Pk_StudentId = @studentId"))
                {

                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    return ExecuteNonQuery(cmd) > 0;

                }

            }

        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentRegForm: ", ex);

            return false;

        }

    }

    public DataTable GetStudentDummyadmitData(string CollegeId, string facultyId, int ExamId)
    {

        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetExamDummyAdmitCardData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Fk_CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@Fk_FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@ExamId", ExamId);


                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDummyRegData: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetStudentExamAddmitCardSubjectDetails(int studentId, int collegeId, int facultyId, int examTypeId, bool isPractical)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentExamAddmitCardSubjectDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentID", studentId);
                    cmd.Parameters.AddWithValue("@facultyId", facultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);
                    cmd.Parameters.AddWithValue("@ExamTypeId", examTypeId);
                    cmd.Parameters.AddWithValue("@IsPractical", isPractical);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentExamAddmitCardSubjectDetails: ", ex);
            throw;
        }

        return dt;
    }

    public DataTable GetStudentDummyExamCertificateData(int studentId, int collegeId, int facultyId)
    {

        DataTable dt = new DataTable();

        try

        {

            using (SqlConnection con = new SqlConnection(connectionString))

            {

                using (SqlCommand cmd = new SqlCommand("GetStudentExamDummyFormData", con))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    cmd.Parameters.AddWithValue("@CollegeId", collegeId);

                    cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                }

            }

        }

        catch (Exception ex)

        {

            log.Error("Error in GetStudentDummyRegCertificateData: ", ex);

            // Optionally rethrow if needed

            throw;

        }

        return dt;

    }


    public DataTable GetExamSubjectsByGroup(int FacultyId, int CollegeId, int StudentId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetExamSubjectsByGroup", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    //cmd.Parameters.AddWithValue("@ExamTypeId", ExamTypeId);
                    cmd.Parameters.AddWithValue("@StudentId", StudentId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }

        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectsByGroup: ", ex);
            throw;
        }
        return dt;
    }

    public DataTable GetSubjectsForComp_Imp_Qual(int FacultyId, int CollegeId, int StudenId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubjectsForComp_Imp_Qual", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId);
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    cmd.Parameters.AddWithValue("@StudentId", StudenId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }

        }
        catch (Exception ex)
        {
            log.Error("Error in GetSubjectsByGroup: ", ex);
            throw;
        }
        return dt;
    }

    public void UpdateImprovmentStudentPaperAppliedById(int pkId, int improvedFlag)
    {
        string query = @"
        UPDATE StudentPaperApplied_Mst
        SET 
            improved = @Improved,
            LastModifiedOn = GETDATE()
        WHERE Pk_StudentPaperAppliedId = @PkId";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Improved", improvedFlag);
                cmd.Parameters.AddWithValue("@PkId", pkId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateImprovmentStudentPaperAppliedById", ex);
            throw;
        }
    }


    public void ExamUpdateStudentSubjectByPkId(string pkId, string subjectPaperId, string subjectGroupId, string modifiedBy)
    {
        string query = @"
        UPDATE StudentPaperApplied_Mst
        SET 
            Fk_SubjectPaperId = @SubjectPaperId,
            Fk_SubjectGroupId = @SubjectGroupId,
            LastModifiedOn = GETDATE(),
            LastModifiedBy = @ModifiedBy
        WHERE Pk_StudentPaperAppliedId = @PkId";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SubjectPaperId", subjectPaperId);
                cmd.Parameters.AddWithValue("@SubjectGroupId", subjectGroupId);
                cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                cmd.Parameters.AddWithValue("@PkId", pkId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentSubjectByPkId: ", ex);

            throw;
        }
    }

    public void ExamImprovemnetUpdateStudentSubjectByPkId(string pkId, string subjectPaperId, string subjectGroupId, string modifiedBy)
    {
        string query = @"
        UPDATE StudentPaperApplied_Mst
        SET 
            Fk_SubjectPaperId = @SubjectPaperId,
            Fk_SubjectGroupId = @SubjectGroupId,
            improved=1,
            LastModifiedOn = GETDATE(),
            LastModifiedBy = @ModifiedBy
        WHERE Pk_StudentPaperAppliedId = @PkId";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SubjectPaperId", subjectPaperId);
                cmd.Parameters.AddWithValue("@SubjectGroupId", subjectGroupId);
                cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
                cmd.Parameters.AddWithValue("@PkId", pkId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentSubjectByPkId: ", ex);

            throw;
        }
    }
    public DataTable GetExamImporovedAppliedSubjects(string studentId)
    {
        try
        {
            string query = @"SELECT sp.SubjectPaperCode,Fk_SubjectPaperId as SubjectPaperId,ComGrp,Pk_StudentPaperAppliedId
                     FROM StudentPaperApplied_Mst spa
                     INNER JOIN SubjectPaper_Mst sp ON spa.Fk_SubjectPaperId = sp.Pk_SubjectPaperId
                     WHERE spa.Fk_StudentId = @StudentId and  improved=1";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the studentId parameter
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    // Create data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetExamImporovedAppliedSubjects: ", ex);
            throw ex;
        }
    }

    public DataTable GetExamFormStatus(int collegeId, int facultyId, string status)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetExamFormStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Handle CollegeId
                    if (collegeId > 0)
                        cmd.Parameters.AddWithValue("@CollegeId", collegeId);
                    else
                        cmd.Parameters.AddWithValue("@CollegeId", DBNull.Value);

                    // Handle FacultyId
                    if (facultyId > 0)
                        cmd.Parameters.AddWithValue("@FacultyId", facultyId);
                    else
                        cmd.Parameters.AddWithValue("@FacultyId", DBNull.Value);

                    // Handle Status
                    if (!string.IsNullOrEmpty(status))
                        cmd.Parameters.AddWithValue("@Status", status);
                    else
                        cmd.Parameters.AddWithValue("@Status", DBNull.Value);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetExamFormStatus: ", ex);
            throw ex;
        }

        return dt;
    }

    public DataTable GetDeclarationFormData(string CollegeId, string facultyId, string subCategory)
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetDeclarationFormData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                if (!string.IsNullOrWhiteSpace(subCategory))
                    cmd.Parameters.AddWithValue("@CategoryName", subCategory);
                else
                    cmd.Parameters.AddWithValue("@CategoryName", DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetDeclarationFormData: ", ex);
            throw;
        }

        return dt;
    }


    public bool UploadDeclarationForm(string studentId, string DeclarationFormpath)
    {
        try
        {
            string query = "UPDATE Student_Mst SET DeclarationFormPath = @DeclarationForm, IsDeclarationFormSubmitted = 1,IsRegFormSubmit=1  WHERE Pk_StudentId = @Pk_StudentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Pk_StudentId", studentId);
                cmd.Parameters.AddWithValue("@DeclarationForm", DeclarationFormpath);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UploadDeclarationForm: ", ex);

            throw ex;
        }

    }
    public DataSet GetCollegeWiseSeatSummaryForInfo(int CollegeId)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("GetFeeSubmissionSummary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }


                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in GetFeeSubmissionSummary: ", ex);
            throw;

        }
        return ds;

    }

    public bool DeclarationFormDownloaded(string studentId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Student_Mst SET IsDeclarationFormDownloaded=1,UpdateDate = GETDATE() WHERE Pk_StudentId = @studentId", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                    conn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in DeclarationFormDownloaded: ", ex);
            return false;
        }
    }

    public bool DeleteDeclarationForm(string studentId, string DeclarationFormpath)

    {

        try

        {

            string query = "UPDATE Student_Mst SET DeclarationFormPath = @DeclarationForm, IsDeclarationFormSubmitted = 0,IsRegFormSubmit=0  WHERE Pk_StudentId = @Pk_StudentId";

            using (SqlConnection con = new SqlConnection(connectionString))

            using (SqlCommand cmd = new SqlCommand(query, con))

            {

                cmd.Parameters.AddWithValue("@Pk_StudentId", studentId);

                cmd.Parameters.AddWithValue("@DeclarationForm", DeclarationFormpath);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
                con.Close();
            }

        }

        catch (Exception ex)

        {

            log.Error("Error in UploadDeclarationForm: ", ex);

            throw ex;

        }

    }

    public DataTable GetDeclarationAndPaymentStatusCount(int CollegeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDeclarationAndPaymentStatusCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetDeclarationAndPaymentStatusCount: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetStudentRegisteredListData(int CollegeId, int facultyId, string CategoryType)
    {

        DataTable dt = new DataTable();

        try
        {

            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = new SqlCommand("sp_GetStudentRegisteredListData", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                if (!string.IsNullOrWhiteSpace(CategoryType))

                    cmd.Parameters.AddWithValue("@CategoryType", CategoryType);

                else

                    cmd.Parameters.AddWithValue("@CategoryType", DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {

                    adapter.Fill(dt);

                }

            }

        }

        catch (Exception ex)
        {

            log.Error("Error in GetDeclarationFormData: ", ex);

            throw;

        }

        return dt;

    }


    public string InsertStudentUploadHistory(int studentId, string photoPath, string signPath)
    {
        string result = "";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddStudentUploadHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
                    cmd.Parameters.Add("@NewphotoFileName", SqlDbType.NVarChar, 255).Value = photoPath;
                    cmd.Parameters.Add("@NewsignatureFileName", SqlDbType.NVarChar, 255).Value = signPath;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        result = "Inserted successfully";
                    else
                        result = "Insert failed";
                }
            }
        }
        catch (Exception ex)
        {
            result = "Error: " + ex.Message;
        }

        return result;
    }

    public bool UpdatePaymentReceiptPath(string clientTxnId, string receiptPath)
    {
        try
        {
            string query = "UPDATE Payment_Details " +
                           "SET PaymentReceiptPath = @ReceiptPath " +
                           "WHERE ClientTxnId = @ClientTxnId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ReceiptPath", receiptPath);
                cmd.Parameters.AddWithValue("@ClientTxnId", clientTxnId);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;   // ✅ return true if update success
                con.Close();
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdatePaymentReceiptPath: ", ex);
            throw;
        }
    }

    #endregion

    public int UpdateChallanInquiry(
  string clientTxnId,
  string paymentStatus,
  string paymentStatusCode,
  string bankTxnId,
  string paidAmount,
  string paymentMode,
  string paymentUpdatedDate)
    {
        int rowsAffected = 0;
        SqlConnection conn = new SqlConnection(connectionString);

        try
        {
            string query = @"
         UPDATE Payment_Details
         SET 
             PaymentStatus = @PaymentStatus,
             PaymentStatusCode = @PaymentStatusCode,
             BankTxnId = @BankTxnId,
             AmountPaid = @AmountPaid,
             PaymentMode = @PaymentMode,
             PaymentUpdatedDate = @PaymentUpdatedDate
         WHERE ClientTxnId = @ClientTxnId";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ClientTxnId", clientTxnId);
            cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
            cmd.Parameters.AddWithValue("@PaymentStatusCode", paymentStatusCode);
            cmd.Parameters.AddWithValue("@BankTxnId", bankTxnId);
            cmd.Parameters.AddWithValue("@AmountPaid", paidAmount);
            cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
            cmd.Parameters.AddWithValue("@PaymentUpdatedDate", paymentUpdatedDate);

            conn.Open();
            rowsAffected = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateChallanInquiry: ", ex);
            throw;
        }
        finally
        {
            // ✅ Explicitly close the connection
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

        return rowsAffected;
    }
    public void UpdateStudentRegFeeSubmit(int studentId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Student_Mst SET IsRegFeeSubmit = 1 WHERE Pk_StudentId = @StudentId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        log.Info(string.Format("StudentId {0} updated successfully.", studentId));
                    else
                        log.Warn(string.Format("StudentId {0} update failed!", studentId));

                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentRegFeeSubmit: ", ex);
            throw;
        }
    }

    public void updateschedulerbit( string tnxid)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Payment_Details SET IsUpdateByScheduler = 1 WHERE ClientTxnId = @tnxid";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@tnxid", tnxid);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();


                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in updateschedulerbit: ", ex);
            throw;
        }
    }

    public void UpdateStudentExamFeeSubmit(int studentId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Student_Mst SET IsExamFeeSubmit = 1 WHERE Pk_StudentId = @StudentId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        log.Info(string.Format("StudentId {0} updated successfully.", studentId));
                    else
                        log.Warn(string.Format("StudentId {0} update failed!", studentId));

                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentRegFeeSubmit: ", ex);
            throw;
        }
    }

    public void UpdateExamStudentsFormDownloaded(int studentIds)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Student_Mst SET ExamFormDownloaded = 1 WHERE Pk_StudentId = @StudentId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentIds);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in UpdateStudentsDownloaded: ", ex);
            throw;
        }
    }


    public DataTable GetStudentExaminationListData(int CollegeId, int facultyId, string ExamTypeId)
    {

        DataTable dt = new DataTable();

        try
        {

            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = new SqlCommand("sp_GetStudentExaminationListData", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CollegeId", CollegeId);

                cmd.Parameters.AddWithValue("@FacultyId", facultyId);

                if (!string.IsNullOrWhiteSpace(ExamTypeId))

                    cmd.Parameters.AddWithValue("@Fk_ExamTypeId", ExamTypeId);

                else

                    cmd.Parameters.AddWithValue("@Fk_ExamTypeId", DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {

                    adapter.Fill(dt);

                }

            }

        }

        catch (Exception ex)
        {

            log.Error("Error in GetStudentExaminationListData: ", ex);

            throw;

        }

        return dt;

    }

    public DataTable ExamGetStudentSubjectsListByStudentId(int studentId, int ExamTypeId)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ExamGetStudentSubjectsListByStudentId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@studentId ", studentId);
                    cmd.Parameters.AddWithValue("@examTypeId ", ExamTypeId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentSubjectsListByStudentId: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataSet GetExamCollegeWiseSeatSummaryForInfo(int CollegeId)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetExamFeeFormSummary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeId", CollegeId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }


                }

            }
        }
        catch (Exception ex)
        {

            log.Error("Error in sp_GetExamFeeFormSummary: ", ex);
            throw;

        }
        return ds;

    }


    public DataSet GetStudentStats(bool updateFee)
    {
        DataSet ds = new DataSet();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_StudentFormAndFeeStats", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UpdateFee", updateFee ? 1 : 0);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds); // fills all result sets into ds.Tables
                }
            }
        }

        return ds;
    }


    public decimal GetTotalStudentFee(string studentIdsCsv)
    {
        if (string.IsNullOrEmpty(studentIdsCsv))
            return 0;

        decimal totalAmount = 0;
       

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_GetTotalStudentFee", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentIds", studentIdsCsv);

            conn.Open();

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
                totalAmount = Convert.ToDecimal(result);
        }

        return totalAmount;
    }

    public DataTable GetStudentDownloadDummyExmCardData(int Collegecode, int FacultyId, string RegistrationNo, string DOB)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentDownloadDummyExmCardData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Collegecode", Collegecode);
                    cmd.Parameters.AddWithValue("@RegistrationNo", string.IsNullOrWhiteSpace(RegistrationNo) ? (object)DBNull.Value : RegistrationNo);
                    cmd.Parameters.AddWithValue("@FacultyId", FacultyId == 0 ? (object)DBNull.Value : FacultyId);


                    if (string.IsNullOrWhiteSpace(DOB))
                    {
                        cmd.Parameters.AddWithValue("@DOB", DBNull.Value);
                    }
                    else
                    {
                        string[] acceptedFormats = new string[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                        DateTime parsedDOB;

                        if (DateTime.TryParseExact(DOB, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDOB))
                        {
                            // Format to dd-MM-yyyy to match SQL format 105
                            cmd.Parameters.AddWithValue("@DOB", parsedDOB);
                        }
                        else
                        {
                            throw new FormatException("DOB '" + DOB + "' is not in a valid format. Expected yyyy-MM-dd or dd/MM/yyyy or MM/dd/yyyy.");
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDownloadDummyExmCardData: ", ex);
            throw;
        }
    }

    public DataSet GetChangesAndDummyDownloads(string DummyCorrectionDetails)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetCorrectionDummyReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportType", DummyCorrectionDetails);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                con.Open();
                da.Fill(ds);
                con.Close();

                return ds;
            }
        }
    }


    public DataTable GetStudentPracticalDummyadmitData(string CollegeId, string facultyId, int ExamId)
    {

        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetExamDummyAdmitCardData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Fk_CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@Fk_FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@ExamId", ExamId);
                cmd.Parameters.AddWithValue("@IsPractical", 1);


                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDummyRegData: ", ex);

            throw ex;
        }

        return dt;
    }

    public DataTable GetStudentTheoryDummyadmitData(string CollegeId, string facultyId, int ExamId)
    {

        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetExamDummyAdmitCardData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Fk_CollegeId", CollegeId);
                cmd.Parameters.AddWithValue("@Fk_FacultyId", facultyId);
                cmd.Parameters.AddWithValue("@ExamId", ExamId);
                cmd.Parameters.AddWithValue("@IsTheory", 1);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in GetStudentDummyRegData: ", ex);

            throw ex;
        }

        return dt;
    }

    public bool UpdatePracticalAndTheoryDummyDownloadStatus(string studentId, string fromPage)
    {
        try
        {
            string query = "";
            if (fromPage == "PracticalDummy")
            {
                query = "UPDATE Student_Mst SET IsPracticaldmitCardDownload = 1 WHERE Pk_StudentId = @StudentId";
            }
            else
            {
                query = "UPDATE Student_Mst SET IsTheoryAdmitCardDownload = 1 WHERE Pk_StudentId = @StudentId";
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();  // <– This is allowed INSIDE helper
                        con.Close();

                        return rows > 0;   // true if updated
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // optional: log error
            throw;
        }
    }

}