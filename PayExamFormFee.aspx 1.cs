using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PayExamFormFee : System.Web.UI.Page
{
    DBHelper db = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            if (!IsPostBack)
            {
                Binddropdown();

                divpayment.Visible = false;
                divstudentdetails.Visible = false;
                rdo_makepayment.Checked = true;


                if (Session["CollegeName"].ToString() == "Admin")
                {
                    txt_collegename.Text = "";
                   
                    ddl_paymode.Items.Add(new ListItem("Axis Bank", "Axis Bank"));
                    
                }
                else
                {
                    txt_collegename.Text = Session["CollegeCode"].ToString() + " | " + Session["CollegeName"].ToString();
                    txt_collegename.ReadOnly = true;
                }
            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }


    private void LogMessage(string message)
    {
        try
        {
            string logPath = Server.MapPath("~/Logs/PaymentLog.txt");
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message);
            }
        }
        catch
        {
            // Avoid throwing errors from logger itself
        }
    }

    public void Binddropdown()
    {
        DataTable dtstate = db.getFacultyfordropdown();
        if (dtstate.Rows.Count > 0)
        {
            ddlFaculty.DataSource = dtstate;
            ddlFaculty.DataTextField = "FacultyName";
            ddlFaculty.DataValueField = "Pk_FacultyId";
            ddlFaculty.DataBind();
            ddlFaculty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Faculty", "0"));
        }
        else
        {
            ddlFaculty.Items.Clear();
            ddlFaculty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Faculty", "0"));
        }
        DataTable dtExamcat = db.getExamCatfordropdown();
        var filtered = dtExamcat.AsEnumerable().Where(row => row.Field<int>("Pk_ExamTypeId") != 5);

        if (filtered.Any())
        {
            ddlExamcat.DataSource = filtered.CopyToDataTable();
            ddlExamcat.DataTextField = "ExamTypeName";
            ddlExamcat.DataValueField = "Pk_ExamTypeId";
            ddlExamcat.DataBind();
            ddlExamcat.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Exam Category", "0"));
        }
        else
        {
            ddlExamcat.Items.Clear();
        }

        // ddlExamcat.Items.Insert(0, new ListItem("Select Exam Category", "0"));

    }

    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        try
        {

            string Collegename = Session["CollegeName"].ToString();
            int facultyId = Convert.ToInt32(ddlFaculty.SelectedValue);
            int ExamId = Convert.ToInt32(ddlExamcat.SelectedValue);
          
            if (rdo_payemntstatus.Checked == true)
            {
                LogMessage("Payment status radio checked.");
                int collegeid = 0;
                string collegecode = "";
                if (Collegename != "Admin")
                {
                    collegeid = Convert.ToInt32(Session["CollegeId"]);
                    LogMessage("CollegeId from session: " + collegeid);
                }
                else
                {
                    collegecode = txt_collegename.Text;
                    LogMessage("CollegeCode from textbox: " + collegecode);
                }
                // Force TLS 1.2 for secure API calls
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                LogMessage("TLS 1.2 enforced for API calls.");
                DataTable result = db.GetExamPaymentDetails(collegeid, collegecode, ExamId);
                LogMessage("Fetched " + (result != null ? result.Rows.Count.ToString() : "0") + " records from GetExamPaymentDetails.");
                if (result != null && result.Rows.Count > 0)
                {

                    foreach (DataRow row in result.Rows)
                    {
                        string clientTxnId = row["ClientTxnId"].ToString();
                        LogMessage("Processing transaction: " + clientTxnId);
                        string rowStatus = row.Table.Columns.Contains("PaymentStatus") && row["PaymentStatus"] != DBNull.Value ? row["PaymentStatus"].ToString() : string.Empty;
                        LogMessage("RowStatus: " + rowStatus);

                        string paymentGateway = row["BankGateway"].ToString();
                        LogMessage("PaymentGateway: " + paymentGateway);

                        if (string.IsNullOrEmpty(rowStatus) || rowStatus.Equals("FAILED", StringComparison.OrdinalIgnoreCase) || rowStatus.Equals("INITIATED", StringComparison.OrdinalIgnoreCase) || rowStatus.Equals("CHALLAN_GENERATED", StringComparison.OrdinalIgnoreCase) || rowStatus.Equals("PENDING", StringComparison.OrdinalIgnoreCase) ||
        rowStatus.Equals("FAILURE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (paymentGateway.Equals("Indian Bank", StringComparison.OrdinalIgnoreCase))
                            {
                                string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                                string url = "https://txnenquiry.sabpaisa.in/SPTxtnEnquiry/TransactionEnquiryServlet?clientCode=" + clientCode + "&clientXtnId=" + clientTxnId;
                                LogMessage("Indian Bank enquiry URL: " + url);

                                string responseString = "";
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                request.Method = "GET";

                                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    responseString = reader.ReadToEnd();
                                }
                                LogMessage("Indian Bank Response: " + responseString);
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(responseString);
                                XmlNode txnNode = xmlDoc.SelectSingleNode("/transaction");

                                if (txnNode != null)
                                {
                                    string apiStatus = txnNode.Attributes["status"] != null  ? txnNode.Attributes["status"].Value : "";
                                    LogMessage("API Status: " + apiStatus);

                                    string paymentStatusCode = txnNode.Attributes["sabPaisaRespCode"] != null
                                                                ? txnNode.Attributes["sabPaisaRespCode"].Value
                                                                : "";

                                    string bankTxnId = txnNode.Attributes["txnId"] != null
                                                                ? txnNode.Attributes["txnId"].Value
                                                                : "";

                                    string paidAmount = txnNode.Attributes["payeeAmount"] != null
                                                                ? txnNode.Attributes["payeeAmount"].Value
                                                                : "";

                                    string paymentUpdateddate = txnNode.Attributes["transCompleteDate"] != null
                                                                ? txnNode.Attributes["transCompleteDate"].Value
                                                                : "";

                                    string paymentmode = txnNode.Attributes["paymentMode"] != null
                                                                ? txnNode.Attributes["paymentMode"].Value
                                                                : "";

                                    string errorcode = txnNode.Attributes["errorCode"] != null
                                                                ? txnNode.Attributes["errorCode"].Value
                                                                : "";
                                    LogMessage("ErrorCode: " + errorcode);

                                    if (errorcode == "400")
                                    {

                                        continue; // go to next txnNode
                                        LogMessage("break: " + errorcode);
                                    }

                                    // Update database
                                    db.UpdateChallanInquiry(clientTxnId, apiStatus, paymentStatusCode, bankTxnId, paidAmount, paymentmode, paymentUpdateddate);

                                    // Update student fees if SUCCESS
                                    if (apiStatus.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(clientTxnId, ExamId);
                                        LogMessage("Fetched dataset for txn " + clientTxnId + ", Tables: " + (dsStudents != null ? dsStudents.Tables.Count.ToString() : "0"));
                                        if (dsStudents != null && dsStudents.Tables.Count > 0)
                                        {
                                            DataTable dtStudents = dsStudents.Tables[1];
                                            foreach (DataRow rowst in dtStudents.Rows)
                                            {
                                                int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                                db.UpdateStudentExamFeeSubmit(studentId);
                                                LogMessage("Student fee updated: " + studentId);
                                            }
                                        }
                                       
                                    }

                                    // Show alert only if errorCode exists AND is not 400
                                    //                                if (!string.IsNullOrEmpty(errorcode) && errorcode != "400")
                                    //                                {
                                    //                                    string script = @"
                                    //swal({{
                                    //    title: 'Failed',
                                    //    text: 'Transaction ID: {clientTxnId}\nError Code: {errorcode}',
                                    //    icon: 'error',
                                    //    button: 'Retry'
                                    //}});";
                                    //                                    ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
                                    //                                    System.Diagnostics.Debug.WriteLine("Transaction error for " + clientTxnId + ": " + errorcode);
                                    //                                }
                                }
                            }
                            else if (paymentGateway.Equals("Axis Bank", StringComparison.OrdinalIgnoreCase))
                            {
                                // 🔹 Axis Enquiry API
                                string CID = ConfigurationManager.AppSettings["Axis_CID"];
                                string RID = clientTxnId;
                                string key = ConfigurationManager.AppSettings["Axis_ChecksumKey"];
                                string ENCKEY = ConfigurationManager.AppSettings["Axis_ENCKEY"];
                                string TYP = ConfigurationManager.AppSettings["Axis_TYP"];

                                // Compute checksum
                                string checksumInput = CID + RID + RID + key;
                                string CKS = sha256_hash(checksumInput);

                                string PlainText = "CID=" + CID + "&RID=" + clientTxnId +  "&CRN=" + RID + "&VER=1.0" + "&TYP=" + TYP + "&CKS=" + CKS;

                                // Encrypt the request
                                string encryptedRequest = Encrypt(PlainText, ENCKEY);

                                // URL-encode the request before sending
                                string enquiryUrl = "https://easypay.axisbank.co.in/index.php/api/enquiry?i=" + HttpUtility.UrlEncode(encryptedRequest);

                                string encryptedResponse = "";
                                using (WebClient wc = new WebClient())
                                {
                                    encryptedResponse = wc.DownloadString(enquiryUrl);
                                }

                                // Handle Axis Bank errors
                                if (encryptedResponse.StartsWith("error=", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Log the error and continue with next transaction

                                    continue; // inside a loop
                                }
                                try
                                {
                                    // URL-decode the response first
                                    string decodedResponse = HttpUtility.UrlDecode(encryptedResponse);

                                    // Convert Base64 to byte array
                                    byte[] encryptedBytes = Convert.FromBase64String(decodedResponse);

                                    // Decrypt using AES128 ECB PKCS7
                                    string plainResponse = "";
                                    using (RijndaelManaged rij = new RijndaelManaged())
                                    {
                                        rij.Key = Encoding.UTF8.GetBytes(ENCKEY.PadRight(16, ' ')); // ensure 16 bytes
                                        rij.Mode = CipherMode.ECB;
                                        rij.Padding = PaddingMode.PKCS7;

                                        ICryptoTransform decryptor = rij.CreateDecryptor();
                                        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                                        plainResponse = Encoding.UTF8.GetString(decryptedBytes);
                                    }

                                    // Parse response into dictionary
                                    var responseDict = plainResponse.Split('&')
                                                                    .Select(part => part.Split('='))
                                                                    .ToDictionary(split => split[0], split => split[1]);

                                    string statusCode = responseDict["STC"];
                                    string statusMsg = responseDict["RMK"];
                                    string paidAmount = responseDict["AMT"];
                                    string bankRef = responseDict["BRN"];
                                    string txnId = responseDict["TRN"];
                                    string paymentMode = responseDict["PMD"];
                                    string paymentDate = responseDict["TET"];

                                    // Update DB
                                    db.UpdateChallanInquiry(RID, statusMsg, statusCode, txnId, paidAmount, paymentMode, paymentDate);
                                 

                                    if (statusCode == "000") // Success
                                    {
                                        DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(RID, ExamId);
                                        if (dsStudents != null && dsStudents.Tables.Count > 0)
                                        {
                                            DataTable dtStudents = dsStudents.Tables[1];
                                            foreach (DataRow rowst in dtStudents.Rows)
                                            {
                                                int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                                db.UpdateStudentExamFeeSubmit(studentId);
                                            }
                                        }
                                    }
                                }
                                catch (FormatException fe)
                                {
                                    string errorMessage = fe.Message.Replace("'", "\\'");
                                    string script = @"
        swal({{
            title: 'Error',
            text: 'Invalid Base64 response from Axis Bank: {errorMessage}',
            icon: 'error',
            button: 'Close'
        }});";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Base64Error", script, true);
                                }
                                catch (CryptographicException ce)
                                {
                                    string errorMessage = ce.Message.Replace("'", "\\'");
                                    string script = @"
        swal({{
            title: 'Error',
            text: 'AES Decryption failed for Axis Bank response: {errorMessage}',
            icon: 'error',
            button: 'Close'
        }});";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "AESDecryptError", script, true);
                                }

                                
                            }
                            else if (paymentGateway.Equals("HDFC Bank", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string paymentId = row["GatewayTxnId"].ToString();
                                    if (!string.IsNullOrEmpty(paymentId))
                                    {

                                        var keyId = ConfigurationManager.AppSettings["RazorpayKeyId"];
                                        var keySecret = ConfigurationManager.AppSettings["RazorpayKeySecret"];

                                        // Prepare Basic Auth
                                        string authInfo = Convert.ToBase64String(Encoding.ASCII.GetBytes(keyId + ":" + keySecret));
                                        string url = "https://api.razorpay.com/v1/payments/" + paymentId;

                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                        request.Method = "GET";
                                        request.Headers.Add("Authorization", "Basic " + authInfo);

                                        string jsonResponse = "";
                                        using (var response = (HttpWebResponse)request.GetResponse())
                                        using (var reader = new StreamReader(response.GetResponseStream()))
                                        {
                                            jsonResponse = reader.ReadToEnd();
                                        }

                                        LogMessage("Razorpay Response: " + jsonResponse);

                                        if (!string.IsNullOrEmpty(jsonResponse))
                                        {
                                            var paymentResponse = JObject.Parse(jsonResponse);

                                            string statusdb = "";
                                            string apiStatus = paymentResponse["status"].ToString();
                                            if (apiStatus == "captured")
                                            {
                                                statusdb = "SUCCESS";

                                            }
                                            else if (apiStatus == "failure")
                                            {
                                                statusdb = "FAILED";
                                            }
                                            else if (apiStatus == "authorized")
                                            {
                                                statusdb = "AUTHORIZED";
                                            }

                                            string bankTxnId = null;

                                            //// First, try to get UPI transaction ID
                                            //if (paymentResponse["acquirer_data"] != null && paymentResponse["acquirer_data"]["upi_transaction_id"] != null)
                                            //{
                                            //    bankTxnId = paymentResponse["acquirer_data"]["upi_transaction_id"].ToString();
                                            //}

                                            // Fallback to payment ID if UPI txn ID is null
                                            if (string.IsNullOrEmpty(bankTxnId) && paymentResponse["order_id"] != null)
                                            {
                                                bankTxnId = paymentResponse["order_id"].ToString();
                                            }

                                            // Get the amount in paise
                                            decimal paidAmount = 0;
                                            string amountStr = paymentResponse["amount"].ToString();
                                            try
                                            {
                                                paidAmount = decimal.Parse(amountStr ?? "0", CultureInfo.InvariantCulture);
                                            }
                                            catch
                                            {
                                                paidAmount = 0; // fallback in case of invalid value
                                            }

                                            string paymentMode = paymentResponse["method"].ToString() ?? "";
                                            string paymentUpdatedDate = paymentResponse["created_at"].ToString();
                                            string errorCode = null;

                                            if (paymentResponse["error"] != null && paymentResponse["error"]["code"] != null)
                                            {
                                                errorCode = paymentResponse["error"]["code"].ToString();
                                            }

                                            // Now check the value
                                            if (errorCode == "BAD_REQUEST_ERROR")
                                            {
                                                // Skip or continue
                                                continue;
                                            }


                                            // Update database
                                            db.UpdateChallanInquiry(clientTxnId, statusdb, "", bankTxnId, paidAmount.ToString(), paymentMode, "");

                                            // Update student fees if payment successful
                                            if (statusdb == "SUCCESS")
                                            {
                                                DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(clientTxnId, ExamId);
                                                if (dsStudents != null && dsStudents.Tables.Count > 1)
                                                {
                                                    DataTable dtStudents = dsStudents.Tables[1];
                                                    foreach (DataRow rowst in dtStudents.Rows)
                                                    {
                                                        int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                                        db.UpdateStudentExamFeeSubmit(studentId);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (WebException wex)
                                {
                                    LogMessage("Razorpay WebException for " + row["GatewayTxnId"].ToString() + ": " + wex.Message);
                                }
                                catch (Exception ex)
                                {
                                    LogMessage("Error processing payment " + row["GatewayTxnId"].ToString() + ": " + ex.Message);
                                }
                            }

                            else
                            {
                                // txnNode is null
                                string script = @"
swal({{
    title: 'Failed',
    text: 'No transaction data found for Transaction ID: {clientTxnId}',
    icon: 'error',
    button: 'Retry'
}});";
                                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
                                System.Diagnostics.Debug.WriteLine("No transaction data found for " + clientTxnId);
                            }
                        }

                    }




                    
                  
                }
                //else
                //{
                //    rpt_getpayemnt.DataSource = null;
                //    rpt_getpayemnt.DataBind();
                //    divpnlNoRecords.Visible = true;
                //    divstudentdetails.Visible = false;
                //    //pnlStudentTable.Visible = false;
                //}
                // Rebind fresh data
                DataTable result1 = db.GetExamPaymentDetails(collegeid, collegecode, ExamId);
                if (result1 != null && result1.Rows.Count > 0)
                {
                    rpt_getpayemnt.DataSource = result1;
                    rpt_getpayemnt.DataBind();

                    divpayment.Visible = true;
                    divstudentdetails.Visible = false;
                    divpnlNoRecords.Visible = false;
                    pnlStudentTable.Visible = false;
                }
                else
                {
                    rpt_getpayemnt.DataSource = null;
                    rpt_getpayemnt.DataBind();
                    divpnlNoRecords.Visible = true;
                    pnlStudentTable.Visible = false;
                }

            }
            else
            {
                //string faculty = "";
                string CollegeName = "";
                if (Collegename == "Admin")
                {

                    CollegeName = txt_collegename.Text;

                }
                else
                {

                    string CollegeNameAndCode = txt_collegename.Text.Trim();
                    string[] CollegeNameSplit = CollegeNameAndCode.Split('|');
                    CollegeName = CollegeNameSplit[0].Trim();
                }


                string CollegeId = "";
                if (Session["CollegeName"].ToString() == "Admin")
                {
                    DataTable dt = db.getcollegeidbasedonCollegecode(txt_collegename.Text);

                    if (dt.Rows.Count > 0)
                    {
                        CollegeId = dt.Rows[0]["Pk_CollegeId"].ToString();
                    }
                }
                else if (Session["CollegeId"] != null)
                {
                    CollegeId = Session["CollegeId"].ToString();
                }

                DataTable result = db.getExamDwnldStudentData(CollegeId, "", facultyId, ExamId, "", "makepayment");
                if (result != null && result.Rows.Count > 0)
                {
                    rptStudents.DataSource = result;
                    rptStudents.DataBind();
                    divpayment.Visible = false;
                    divstudentdetails.Visible = true;
                    pnlStudentTable.Visible = true;
                    divpnlNoRecords.Visible = false;
                    pnlPager.Visible = true;
                    searchInputDIV.Visible = true;
                    pnlPager.Visible = true;

                }
                else
                {
                    rptStudents.DataSource = null;
                    rptStudents.DataBind();
                    divpayment.Visible = false;
                    divstudentdetails.Visible = false;
                    pnlStudentTable.Visible = false;
                    divpnlNoRecords.Visible = true;
                    pnlPager.Visible = false;
                    searchInputDIV.Visible = false;
                }
            }
        }
        catch (Exception exOuter)
        {
            string script = string.Format(@"
    swal({{
        title: 'Failed',
        text: '{0}',
        icon: 'error',
        button: 'Retry'
    }});", exOuter.Message.Replace("'", "\\'"));

            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
            System.Diagnostics.Debug.WriteLine("General Error: " + exOuter.Message);
        }
    }

    protected void btn_paynow_Click(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = "";
            string collegeName = "";
            int collegeId = 0;
            string Mobileno = "";
            string EmailId = "";

            if (Session["CollegeName"].ToString() == "Admin")
            {
                DataTable dtres = db.getcollegeidbasedonCollegecode(txt_collegename.Text);

                if (dtres.Rows.Count > 0)
                {
                    collegeCode = dtres.Rows[0]["CollegeCode"].ToString();
                    collegeName = dtres.Rows[0]["CollegeName"].ToString();
                    collegeId = Convert.ToInt32(dtres.Rows[0]["Pk_CollegeId"]);
                    Mobileno = dtres.Rows[0]["PrincipalMobileNo"].ToString();
                    EmailId = dtres.Rows[0]["EmailId"].ToString();
                }
            }
            else
            {
                Mobileno = Session["PrincipalMobileNo"].ToString();
                collegeCode = Session["CollegeCode"].ToString();
                collegeName = Session["CollegeName"].ToString();
                collegeId = Convert.ToInt32(Session["CollegeId"]);
                EmailId = Session["EmailId"].ToString();
            }

            string username = Session["username"].ToString();

            // ✅ Get selected IDs from hidden field
            string selectedStudentIds = hfSelectedIds.Value;

            if (string.IsNullOrEmpty(selectedStudentIds))
            {
                string script = @"
            swal({
                title: 'Failed',
                text: 'Please select at least one student',
                icon: 'error',
                button: 'Retry'
            });";
                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
                return;
            }

            // ✅ Calculate total amount from selected students
            //long totalAmount = 0;
            //string[] selectedIdsArray = selectedStudentIds.Split(',');

            //foreach (RepeaterItem item in rptStudents.Items)
            //{
            //    HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
            //    Label lblFee = (Label)item.FindControl("lblFee");

            //    if (hfStudentID != null && lblFee != null && selectedIdsArray.Contains(hfStudentID.Value))
            //    {
            //        totalAmount += Convert.ToInt64(lblFee.Text);
            //    }
            //}

            string payerName = collegeCode;
            string payerMobile = "";
           // string payerMobile = Mobileno;
            string payerEmail = "";
            //string payerEmail = EmailId;

            string clientTxnId = "N/A";
            string studentFeeMapping = hfSelectedStudentFees.Value;
            decimal totalAmount = 0;

            if (!string.IsNullOrEmpty(studentFeeMapping))
            {
                foreach (string item in studentFeeMapping.Split(','))
                {
                    string[] parts = item.Split(':');
                    decimal fee;
                    if (parts.Length == 2 && decimal.TryParse(parts[1], out fee))
                    {
                        totalAmount += fee;
                    }
                }
            }

            // ✅ Insert payment details into DB
            string message = db.InsertExamStudentPayment(
                collegeId,
                2,
                ddl_paymode.SelectedValue,
                totalAmount,
                selectedStudentIds
            );

            if (message.StartsWith("Success"))
            {
                int txnIndex = message.IndexOf("Transaction ID:");
                if (txnIndex != -1)
                {
                    clientTxnId = message.Substring(txnIndex + "Transaction ID:".Length).Trim();
                }

                //string script = @"
                //swal({
                //    title: 'Success!',
                //    text: 'Payment completed successfully.',
                //    icon: 'success',
                //    button: 'OK'
                //});";
                //ScriptManager.RegisterStartupScript(this, GetType(), "PaymentSuccess", script, true);
            }
            else
            {
                string script = @"
            swal({
                title: 'Failed!',
                text: '" + message.Replace("'", "\\'") + @"',
                icon: 'error',
                button: 'OK'
            });";
                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFail", script, true);
                return;
            }

            if (ddl_paymode.SelectedValue == "Axis Bank")
            {

                // -------- AXIS Bank Gateway Logic --------
                string CID = ConfigurationManager.AppSettings["Axis_CID"];
                string RU = "https://intermediate.biharboardonline.com/Exam26/responseAxis.aspx";
                string ChecksumKey = ConfigurationManager.AppSettings["Axis_ChecksumKey"];
                string TYP = ConfigurationManager.AppSettings["Axis_TYP"];
                string ENCKEY = ConfigurationManager.AppSettings["Axis_ENCKEY"];

                string safeEmail = string.IsNullOrWhiteSpace(payerEmail) ? "NA" : payerEmail;
                string safeMobile = string.IsNullOrWhiteSpace(payerMobile) ? "0000000000" : payerMobile;

                string PPI = collegeCode + "|" + collegeName + "|" + clientTxnId + "|" + safeEmail + "|" + safeMobile + "|" + totalAmount;

                string TranUrl = "https://easypay.axisbank.co.in/index.php/api/payment";
                string checksumInput = CID + clientTxnId + clientTxnId + totalAmount + ChecksumKey;
                string Checksum = sha256_hash(checksumInput);
                string PlainText = "CID=" + CID +
                        "&RID=" + clientTxnId +
                        "&CRN=" + clientTxnId +
                        "&AMT=" + totalAmount +
                        "&VER=1.0&TYP=" + TYP +
                        "&CNY=INR&RTU=" + RU +
                        "&PPI=" + PPI +
                        "&RE1=MN&RE2=&RE3=&RE4=&RE5=&CKS=" + Checksum;

                string encryptedstring = Encrypt(PlainText, ENCKEY);


                NameValueCollection data = new NameValueCollection();
                data.Add("i", encryptedstring);
                RedirectAndPOST(this.Page, TranUrl, data);


            }
            else if (ddl_paymode.SelectedValue == "HDFC Bank")
            {
                var keyId = ConfigurationManager.AppSettings["RazorpayKeyId"];
                var keySecret = ConfigurationManager.AppSettings["RazorpayKeySecret"];

                string currency = "INR";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Razorpay order creation
                RazorpayClient client = new RazorpayClient(keyId, keySecret);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", totalAmount); // amount in the smallest currency unit (paise)
                options.Add("currency", currency);
                options.Add("receipt", clientTxnId);
                options.Add("payment_capture", 1); // auto capture enabled

                //  Create Razorpay order
                Order order = client.Order.Create(options);

                //  Retrieve generated orderId
                string orderId = order["id"].ToString();

                db.UpdateOrderidofrazorpay(clientTxnId, orderId);

                // Generate embedded Razorpay checkout script
                string formHtml =
     "<form id='razorpayForm' method='POST' action='https://api.razorpay.com/v1/checkout/embedded'>" +
     "<input type='hidden' name='key_id' value='" + keyId + "'/>" +
     "<input type='hidden' name='amount' value='" + totalAmount + "'/>" +
     "<input type='hidden' name='currency' value='INR'/>" +
     "<input type='hidden' name='order_id' value='" + orderId + "'/>" +
     "<input type='hidden' name='name' value='INTERMEDIATE ANNUAL EXAM SESSION 2026'/>" +
     "<input type='hidden' name='description' value='Payment Transaction'/>" +
     "<input type='hidden' name='callback_url' value='http://localhost:58223/responseHDFC.aspx'/>" +
     "<input type='hidden' name='cancel_url' value='http://localhost:58223/responseHDFC.aspx'/>" +
     "<button type='submit'>Pay Now</button>" +
     "</form>" +
     "<script>document.getElementById('razorpayForm').submit();</script>";

                Response.Write(formHtml);
                HttpContext.Current.ApplicationInstance.CompleteRequest();





                // Register the script for client-side execution
                ClientScript.RegisterStartupScript(this.GetType(), "razorpayForm", "document.write('" + formHtml.Replace("'", "\\'") + "');", true);

            }
            else
            {
                string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                string transUserName = ConfigurationManager.AppSettings["transUserName"];
                string transUserPassword = ConfigurationManager.AppSettings["transUserPassword"];
                string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];
                string callbackUrl = ConfigurationManager.AppSettings["callbackUrl"];
                string mcc = ConfigurationManager.AppSettings["mcc"];
                string AmountType = ConfigurationManager.AppSettings["AmountType"];
                string channelid = ConfigurationManager.AppSettings["channelid"];



                string query = "";
                string address = "";

                query = query + "payerName=" + payerName.Trim() + "";
                query = query + "&payerEmail=" + payerEmail.Trim() + "";
                query = query + "&payerMobile=" + payerMobile.Trim() + "";
                query = query + "&clientCode=" + clientCode.Trim() + "";
                query = query + "&transUserName=" + transUserName.Trim() + "";
                query = query + "&transUserPassword=" + transUserPassword.Trim() + "";
                query = query + "&payerAddress=" + address + "";
                query = query + "&clientTxnId=" + clientTxnId.Trim() + "";
                query = query + "&amount=" + totalAmount.ToString() + "";

                query = query + "&amountType=" + AmountType.Trim() + "";
                query = query + "&channelId=" + channelid.Trim() + "";
                query = query + "&mcc=" + mcc.Trim() + "";
                query = query + "&callbackUrl=" + callbackUrl.Trim() + "";

                // Pass extra parameters in Udf1 to udf20 
                //query = query + "&udf1=" + Class.Trim() + "";
                //query = query + "&udf2=" + Roll.Trim() + "";
                Encryption enc = new Encryption();
                // Encrypting the query string
                string encdata = enc.EncryptString(authKey, authIV, query);

                // Create an HTML form for submitting the request to the payment gateway
                string respString = "<html>" +
                                  "<body onload='document.forms[0].submit()'>" +   // Auto-submit on load
                                  "<form action=\"https://securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1\" method=\"post\">" +
                                           //"<form action=\"https://stage-securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1\" method=\"post\">" +
                                          "<input type=\"hidden\" name=\"encData\" value=\"" + encdata + "\" id=\"frm1\">" +
                                          "<input type=\"hidden\" name=\"clientCode\" value=\"" + clientCode + "\" id=\"frm2\">" +
                                          "<noscript><input type=\"submit\" value=\"Click here to continue\"></noscript>" + // fallback if JS is disabled
                                      "</form>" +
                                  "</body>" +
                               "</html>";


                Response.Write(respString);
            }
        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message.Replace("'", "\\'");
            string script = @"
    swal({
        title: 'Error',
        text: 'Error during payment initialization: " + safeMessage + @"',
        icon: 'error',
        button: 'Close'
    });";
            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentInitError", script, true);
        }



    }


    //public string forwardToSabPaisa(
    //  string clientCode, string transUserName, string transUserPassword,
    //  string authKey, string authIV, string payerName, string payerEmail, string payerMobile,
    //  string payerAddress, string clientTxnId, string amount, string amountType,
    //  string channelId, string mcc, string callbackUrl)
    //{
    //    string query = "?clientCode=" + HttpUtility.UrlEncode(clientCode) +
    //                   "&transUserName=" + HttpUtility.UrlEncode(transUserName) +
    //                   "&transUserPassword=" + HttpUtility.UrlEncode(transUserPassword) +
    //                   "&authKey=" + HttpUtility.UrlEncode(authKey) +
    //                   "&authIV=" + HttpUtility.UrlEncode(authIV) +
    //                   "&payerName=" + HttpUtility.UrlEncode(payerName) +
    //                   "&payerEmail=" + HttpUtility.UrlEncode(payerEmail) +
    //                   "&payerMobile=" + HttpUtility.UrlEncode(payerMobile) +
    //                   "&payerAddress=" + HttpUtility.UrlEncode(payerAddress) +
    //                   "&clientTxnId=" + HttpUtility.UrlEncode(clientTxnId) +
    //                   "&amount=" + HttpUtility.UrlEncode(amount) +
    //                   "&amountType=" + HttpUtility.UrlEncode(amountType) +
    //                   "&channelId=" + HttpUtility.UrlEncode(channelId) +
    //                   "&mcc=" + HttpUtility.UrlEncode(mcc) +
    //                   "&callbackUrl=" + HttpUtility.UrlEncode(callbackUrl);

    //    return EncryptString(query, authIV, authKey);
    //}
    //public static string EncryptString(string plainText, string iv, string key)
    //{
    //    using (var csp = new AesCryptoServiceProvider())
    //    {
    //        csp.Mode = CipherMode.CBC;
    //        csp.Padding = PaddingMode.PKCS7;
    //        csp.IV = Encoding.UTF8.GetBytes(iv);
    //        csp.Key = Encoding.UTF8.GetBytes(key);

    //        ICryptoTransform encryptor = csp.CreateEncryptor();
    //        byte[] inputBuffer = Encoding.UTF8.GetBytes(plainText);
    //        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

    //        return Convert.ToBase64String(encryptedBytes);
    //    }
    //}

    private static ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting, String AuthIV, String AuthKey)
    {
        csp.Mode = CipherMode.CBC;
        csp.Padding = PaddingMode.PKCS7;
        String iv = AuthIV;
        String AESKey1 = AuthKey;
        csp.IV = Encoding.UTF8.GetBytes(iv);
        byte[] inputBuffer = Encoding.UTF8.GetBytes(AESKey1);
        csp.Key = Encoding.UTF8.GetBytes(AESKey1);
        if (encrypting)
        {
            return csp.CreateEncryptor();
        }
        return csp.CreateDecryptor();
    }

    public static String sha256_hash(String value)
    {
        StringBuilder Sb = new StringBuilder();
        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }
        return Sb.ToString();
    }


    public string Encrypt(string input, string key)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);
        Aes kgen = Aes.Create("AES");
        kgen.Mode = CipherMode.ECB;
        //kgen.Padding = PaddingMode.None;
        kgen.Key = keyArray;
        ICryptoTransform cTransform = kgen.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string cipherText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = new byte[16]; // must be same IV as used in Encrypt
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

    private void RedirectAndPOST(Page page, string TranUrl, NameValueCollection data)
    {
        try
        {

            //Prepare the Posting form
            string strForm = PreparePOSTForm(TranUrl, data);
            //Add a literal control the specified page holding the Post Form, this is to submit the Posting form with the request.
            page.Controls.Add(new LiteralControl(strForm));



        }
        catch

        { }
    }

    private static String PreparePOSTForm(string url, NameValueCollection data)
    {
        try
        {
            //Set a name for the form
            string formID = "PostForm";

            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }
            strForm.Append("</form>");

            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");

            //Return the form and the script concatenated. (The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void rpt_getpayemnt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "lnk_Delete")
        {
            string clientTxnId = e.CommandArgument.ToString();

            int result = db.DeletePaymentDetails(clientTxnId);
            if (result == 0)
            {
                //string script = "alert('Payment record deleted successfully.'); window.location=window.location.href;";
                //ClientScript.RegisterStartupScript(this.GetType(), "DeleteSuccess", script, true);

                string script = @"
swal({
    title: 'Success!',
    text: 'Payment record deleted successfully.',
    icon: 'success',
    button: 'OK'
}).then(function() {
    window.location = 'PayExamFormFee.aspx'; 
});";

                ScriptManager.RegisterStartupScript(this, GetType(), "RecordDeleted", script, true);


            }
            else
            {


                string script = @"
    swal({
        title: 'Failed!',
        text: 'Payment record not found or deletion failed',
        icon: 'error',
        button: 'OK'
    });";

                ScriptManager.RegisterStartupScript(this, GetType(), "DeleteFail", script, true);



                //string script = "alert('Payment record not found or deletion failed.');";
                //ClientScript.RegisterStartupScript(this.GetType(), "DeleteFail", script, true);
            }
        }
    }

    protected void rpt_getpayemnt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            HiddenField hf_status = (HiddenField)e.Item.FindControl("hf_status");

            // Check user's role from Session or any auth source
            string Collegename = Session["CollegeName"] as string;
            string status = hf_status.Value;

            if (status != "SUCCESS")
            {
                lnkDelete.Visible = true;
            }
            else
            {
                lnkDelete.Visible = false;
            }
        }
    }
}