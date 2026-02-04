using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml;
using log4net;



public partial class payregstudentfee : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    Encryption cs = new Encryption();
    



    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            if (!IsPostBack)
            {

                BindFacultydropdown();

                divpayment.Visible = false;
                divstudentdetails.Visible = false;
                rdo_makepayment.Checked = true;


                if (Session["CollegeName"].ToString() == "Admin")
                {
                    txt_collegename.Text = "";
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

    public void getcollegewiseseatsummary()
    {
        int CollegeId = 0;
        if (Session["CollegeName"].ToString() == "Admin")
        {
            DataTable dtres = db.getcollegeidbasedonCollegecode(txt_collegename.Text);

            if (dtres.Rows.Count > 0)
            {
                CollegeId = Convert.ToInt32(dtres.Rows[0]["Pk_CollegeId"].ToString());

            }

        }
        else
        {
            CollegeId = Convert.ToInt32(Session["CollegeId"].ToString());
        }
        DataTable dt = db.GetCollegeWiseSeatSummary(CollegeId, Convert.ToInt32(ddlFaculty.SelectedValue));
        if (dt != null && dt.Rows.Count > 0)
        {

            txt_regularseats.Text = dt.Rows[0]["RemainingRegularSeats"].ToString();
            txt_privateseats.Text = dt.Rows[0]["RemainingPrivateSeats"].ToString();
        }
        else
        {
            txt_regularseats.Text = "0";
            txt_privateseats.Text = "0";
        }
    }

    public void BindFacultydropdown()
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
    }

    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        getcollegewiseseatsummary();
        string Collegename = Session["CollegeName"].ToString();

        if (rdo_payemntstatus.Checked == true)
        {
            int collegeid = 0;
            string collegecode = "";
            if (Collegename != "Admin")
            {
                collegeid = Convert.ToInt32(Session["CollegeId"]);
            }
            else
            {
                collegecode = txt_collegename.Text;
            }
            // Force TLS 1.2 for secure API calls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            DataTable result = db.GetStudentPaymentDetails(collegeid, collegecode);
            if (result != null && result.Rows.Count > 0)
            {
                try
                {


                    foreach (DataRow row in result.Rows)
                    {
                        string clientTxnId = row["ClientTxnId"].ToString();
                        string rowStatus = row["PaymentStatus"] != DBNull.Value ? row["PaymentStatus"].ToString() : "";
                        //string clientCode1 = "BSBI781"; // or from config
                        // string clientTxnId1 = "REG6305020250910181442029188"; // sample txn id

                        // ✅ Only call API if status is NULL or FAILED
                        if (!string.IsNullOrEmpty(clientTxnId) &&
     (string.IsNullOrEmpty(rowStatus) ||
      rowStatus.Equals("FAILED", StringComparison.OrdinalIgnoreCase) ||
      rowStatus.Equals("INITIATED", StringComparison.OrdinalIgnoreCase)))
                        {
                            string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                            string url = "https://txnenquiry.sabpaisa.in/SPTxtnEnquiry/TransactionEnquiryServlet?clientCode="
                                         + clientCode + "&clientXtnId=" + clientTxnId;

                            // ✅ Correct: Use HttpClient to call API
                            string responseString = "";

                            // 🔹 Use WebRequest (same as your working btnCheckTxn_Click)
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.Method = "GET";

                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    responseString = reader.ReadToEnd();
                                }
                            }

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseString);
                            XmlNode txnNode = xmlDoc.SelectSingleNode("/transaction");



                            if (txnNode != null)
                            {
                                string apiStatus = txnNode.Attributes["status"].Value ?? "";
                                string paymentStatusCode = txnNode.Attributes["sabPaisaRespCode"].Value ?? "";
                                string bankTxnId = txnNode.Attributes["txnId"].Value ?? "";
                                string paidAmount = txnNode.Attributes["payeeAmount"].Value ?? "";
                                string paymentUpdateddate = txnNode.Attributes["transCompleteDate"].Value ?? "";
                                string paymentmode = txnNode.Attributes["paymentMode"].Value ?? "";

                                db.UpdateChallanInquiry(
                                    clientTxnId,
                                    apiStatus,
                                    paymentStatusCode,
                                    bankTxnId,
                                    paidAmount,
                                    paymentmode,
                                    paymentUpdateddate
                                );

                                if (apiStatus.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                                {
                                    DataSet dsStudents = db.GetStdntPaymntDetailsTxnIdwise(clientTxnId);

                                    if (dsStudents != null && dsStudents.Tables.Count > 0)
                                    {
                                        DataTable dtStudents = dsStudents.Tables[1];

                                        foreach (DataRow rowst in dtStudents.Rows)
                                        {
                                            int studentId = Convert.ToInt32(rowst["Fk_StudentId"]); // or your column name
                                            db.UpdateStudentRegFeeSubmit(studentId);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("No transaction data found for " + clientTxnId);
                            }

                        }
                    }

                }
                catch (Exception exOuter)
                {
                    System.Diagnostics.Debug.WriteLine("General Error: " + exOuter.Message);
                }

                // Rebind fresh data
                result = db.GetStudentPaymentDetails(collegeid, collegecode);
                rpt_getpayemnt.DataSource = result;
                rpt_getpayemnt.DataBind();

                divpayment.Visible = true;
                divstudentdetails.Visible = false;
                divpnlNoRecords.Visible = false;
            }

            else
            {
                rpt_getpayemnt.DataSource = null;
                rpt_getpayemnt.DataBind();
                divpnlNoRecords.Visible = true;
            }
            //int collegeid = 0;
            //string collegecode = "";
            //if (Collegename != "Admin")
            //{


            //    collegeid = Convert.ToInt32(Session["CollegeId"]);

            //}
            //else
            //{
            //    collegecode = txt_collegename.Text;
            //}
            //DataTable result = db.GetStudentPaymentDetails(collegeid, collegecode);
            //if (result != null && result.Rows.Count > 0)
            //{
            //    rpt_getpayemnt.DataSource = result;
            //    rpt_getpayemnt.DataBind();
            //    divpayment.Visible = true;
            //    divstudentdetails.Visible = false;
            //    divpnlNoRecords.Visible = false;
            //}
            //else
            //{
            //    rpt_getpayemnt.DataSource = null;
            //    rpt_getpayemnt.DataBind();
            //    divpnlNoRecords.Visible = true;
            //}


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

            string CollegeCode = "";
            string CollegeId = "";
            if (Session["CollegeName"].ToString() == "Admin")
            {
                CollegeCode = txt_collegename.Text;
                CollegeId = "";
            }
            else
            {
                CollegeCode = "";
                CollegeId = Session["CollegeId"].ToString();
            }

            DataTable result = db.getStudentData(CollegeId, CollegeCode, "", ddlFaculty.SelectedValue, ddl_category.SelectedValue);
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


    protected void btn_paynow_Click(object sender, EventArgs e)
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
        // string payerName = collegeName + " | " + collegeCode;
        string payerMobile = Mobileno; // optional
        string payerEmail = EmailId;

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
        string message = db.InsertStudentPaymentDetails(
            collegeId,
            1, // PaymentTypeId (assumed)
            ddl_paymode.SelectedValue,
           totalAmount,
            selectedStudentIds
        );

        if (message.Contains("Seat limit exceeded"))
        {
            string script = @"
            swal({
                title: 'Failed!',
                text: '" + message.Replace("'", "\\'") + @"',
                icon: 'error',
                button: 'OK'
            });";
            ScriptManager.RegisterStartupScript(this, GetType(), "PaymentSeatLimit", script, true);
            return;
        }
        else if (message.StartsWith("Success"))
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




        // ✅ Prepare SabPaisa redirect values
        string clientCode = ConfigurationManager.AppSettings["Clientcode"];
        string transUserName = ConfigurationManager.AppSettings["transUserName"];
        string transUserPassword = ConfigurationManager.AppSettings["transUserPassword"];
        string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
        string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];
        string callbackUrl = ConfigurationManager.AppSettings["callbackUrl"];
        string mcc = ConfigurationManager.AppSettings["mcc"];
        string AmountType = ConfigurationManager.AppSettings["AmountType"];
        string channelid = ConfigurationManager.AppSettings["channelid"];

        string respString = "";
        //string query = "";

        //query = query + "payerName=" + payerName.Trim() + "";
        //query = query + "&payerEmail=" + payerEmail.Trim() + "";
        //query = query + "&payerMobile=" + payerMobile.Trim() + "";
        //query = query + "&clientCode=" + clientCode.Trim() + "";
        //query = query + "&transUserName=" + transUserName.Trim() + "";
        //query = query + "&transUserPassword=" + transUserPassword.Trim() + "";
        //query = query + "&payerAddress=" + "" + "";
        //query = query + "&clientTxnId=" + clientTxnId.Trim() + "";
        //query = query + "&amount=" + totalAmount.ToString() + "";
        //query = query + "&amountType=" + AmountType.Trim() + "";
        //query = query + "&channelId=" + channelid.Trim() + "";
        //query = query + "&mcc=" + mcc.Trim() + "";
        //query = query + "&callbackUrl=" + callbackUrl.Trim() + "";

        //// Pass extra parameters in Udf1 to udf20 
        ////query = query + "&udf1=" + Class.Trim() + "";
        ////query = query + "&udf2=" + Roll.Trim() + "";
        //Encryption enc = new Encryption();
        //// Encrypting the query string
        //string encdata = enc.EncryptString(authKey, authIV, query);

        //// Create an HTML form for submitting the request to the payment gateway
        //string respString = "<html>" +
        //                    "<body>" +
        //                        "<form action=\"https://stage-securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1\" method=\"post\">" +
        //                        "<input type=\"hidden\" name=\"encData\" value=\"" + encdata + "\" id=\"frm1\">" +
        //                        "<input type=\"hidden\" name=\"clientCode\" value=\"" + clientCode + "\" id=\"frm2\">" +
        //                        "<input type=\"submit\" name=\"submit\" value=\"submit\" id=\"submitButton\">" +
        //                        "</form>" +
        //                    "</body>" +
        //                    "</html>";

        // For production Use this Url :- https://securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1
        // For testing Use this Url :- https://stage-securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1

        // Write the form HTML to the response
        Response.Write(respString);

        //string encryptedData = forwardToSabPaisa(
        //    clientCode,
        //    transUserName,
        //    transUserPassword,
        //    authKey,
        //    authIV,
        //    payerName,
        //    payerEmail,
        //    payerMobile,
        //    "",
        //    clientTxnId,
        //  totalAmount.ToString(),
        //    "INR",
        //    "W",
        //    "8795",
        //    callbackUrl
        //);

        //string respString = "<html>" +
        //    "<body onload='document.forms[\"sabPaisaForm\"].submit()'>" +
        //      "<form name='sabPaisaForm' method='post' action='https://securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1'>" +
        //  //  "<form name='sabPaisaForm' method='post' action='https://stage-securepay.sabpaisa.in/SabPaisa/sabPaisaInit?v=1'>" +
        //    "<input type='hidden' name='encData' value='" + encryptedData + "' />" +
        //    "<input type='hidden' name='clientCode' value='" + clientCode + "' />" +
        //    "</form>" +
        //    "</body>" +
        //    "</html>";

        //Response.Clear();
        //Response.Write(respString);
        //Response.End();

    }

    public string forwardToSabPaisa(
      string clientCode, string transUserName, string transUserPassword,
      string authKey, string authIV, string payerName, string payerEmail, string payerMobile,
      string payerAddress, string clientTxnId, string amount, string amountType,
      string channelId, string mcc, string callbackUrl)
    {
        string query = "?clientCode=" + HttpUtility.UrlEncode(clientCode) +
                       "&transUserName=" + HttpUtility.UrlEncode(transUserName) +
                       "&transUserPassword=" + HttpUtility.UrlEncode(transUserPassword) +
                       "&authKey=" + HttpUtility.UrlEncode(authKey) +
                       "&authIV=" + HttpUtility.UrlEncode(authIV) +
                       "&payerName=" + HttpUtility.UrlEncode(payerName) +
                       "&payerEmail=" + HttpUtility.UrlEncode(payerEmail) +
                       "&payerMobile=" + HttpUtility.UrlEncode(payerMobile) +
                       "&payerAddress=" + HttpUtility.UrlEncode(payerAddress) +
                       "&clientTxnId=" + HttpUtility.UrlEncode(clientTxnId) +
                       "&amount=" + HttpUtility.UrlEncode(amount) +
                       "&amountType=" + HttpUtility.UrlEncode(amountType) +
                       "&channelId=" + HttpUtility.UrlEncode(channelId) +
                       "&mcc=" + HttpUtility.UrlEncode(mcc) +
                       "&callbackUrl=" + HttpUtility.UrlEncode(callbackUrl);

        return EncryptString(query, authIV, authKey);
    }
    public static string EncryptString(string plainText, string iv, string key)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            csp.Mode = CipherMode.CBC;
            csp.Padding = PaddingMode.PKCS7;
            csp.IV = Encoding.UTF8.GetBytes(iv);
            csp.Key = Encoding.UTF8.GetBytes(key);

            ICryptoTransform encryptor = csp.CreateEncryptor();
            byte[] inputBuffer = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    public static string DecryptString(string encrypted, string iv, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(encrypted);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

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
    window.location = 'payregstudentfee.aspx'; 
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

    //Axis Bank
    //private string CalculateChecksum(string file)
    //{
    //    string result = string.Empty;
    //    try
    //    {
    //        SHA256 sha256 = SHA256.Create();
    //        sha256 = System.Security.Cryptography.SHA256.Create();
    //        byte[] inputbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(file);
    //        byte[] checksum = sha256.ComputeHash(inputbytes);
    //        result = (BitConverter.ToString(checksum).Replace("-", string.Empty)).ToLower();
    //        // End of using fileStream
    //    }
    //    catch (Exception ex)
    //    {
    //        //DisplayMessage("Error occured in CalculateChecksum() " + ex.Message);
    //    }
    //    return result;
    //}

    //private void RedirectAndPOST(Page page, string TranUrl, NameValueCollection data)
    //{
    //    try
    //    {

    //        //Prepare the Posting form
    //        string strForm = PreparePOSTForm(TranUrl, data);
    //        //Add a literal control the specified page holding the Post Form, this is to submit the Posting form with the request.
    //        page.Controls.Add(new LiteralControl(strForm));



    //    }
    //    catch

    //    { }
    //}

    //private static String PreparePOSTForm(string url, NameValueCollection data)
    //{
    //    try
    //    {
    //        //Set a name for the form
    //        string formID = "PostForm";

    //        //Build the form using the specified data to be posted.
    //        StringBuilder strForm = new StringBuilder();
    //        strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
    //        foreach (string key in data)
    //        {
    //            strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
    //        }
    //        strForm.Append("</form>");

    //        //Build the JavaScript which will do the Posting operation.
    //        StringBuilder strScript = new StringBuilder();
    //        strScript.Append("<script language='javascript'>");
    //        strScript.Append("var v" + formID + " = document." + formID + ";");
    //        strScript.Append("v" + formID + ".submit();");
    //        strScript.Append("</script>");

    //        //Return the form and the script concatenated. (The order is important, Form then JavaScript)
    //        return strForm.ToString() + strScript.ToString();

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    protected void rpt_getpayemnt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            HiddenField hf_status = (HiddenField)e.Item.FindControl("hf_status");

            // Check user's role from Session or any auth source
            string Collegename = Session["CollegeName"] as string;
            string status = hf_status.Value;

            //if (Collegename == "Admin")
            //{
            //    // Admin sees delete button always
            //    lnkDelete.Visible = true;
            //}
            //else
            //{

            if (status != "SUCCESS")
            {
                lnkDelete.Visible = true;
            }
            else
            {
                lnkDelete.Visible = false;
            }
            //}
        }
    }

    protected void DownloadReceipt(object sender, CommandEventArgs e)
    {
        string relativePath = e.CommandArgument.ToString();  // e.g. "uploads/Receipt123.pdf"
        if (string.IsNullOrEmpty(relativePath)) return;

        // ✅ Map to physical file on server
        string fullPath = Server.MapPath("~/" + relativePath);

        if (File.Exists(fullPath))
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream"; // force download
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullPath));
            Response.TransmitFile(fullPath);
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('Receipt file not found.');</script>");
        }
    }
}