using iTextSharp.text.pdf.qrcode;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public partial class ChallanRecall : System.Web.UI.Page
{
    DBHelper db = new DBHelper();

    public void getchallandetails()
    {
        DataTable result = db.getChallanDetailsbasedonTxnId(txt_CollegeCode.Text,2);
        if (result != null && result.Rows.Count > 0)
        {
            rptransactiondetails.DataSource = result;
            rptransactiondetails.DataBind();
        }
        else
        {
            rptransactiondetails.DataSource = null;
            rptransactiondetails.DataBind();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["CollegeId"] != null)
            {

            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        getchallandetails();
    }

    protected void rptransactiondetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hf_Isdeleted = (HiddenField)e.Item.FindControl("hf_Isdeleted");
            LinkButton lnkrestore = (LinkButton)e.Item.FindControl("lnkrestore");
            string ggd = hf_Isdeleted.Value;

            if (hf_Isdeleted.Value == "True")
            {
                lnkrestore.Visible = true;
				
            }
            else
            {
                lnkrestore.Visible = false;
            }
        }
    }

    protected async void rptransactiondetails_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "lnk_Restore")
        {
            try
            {
                HiddenField hf_ClientTxnId = (HiddenField)e.Item.FindControl("hf_ClientTxnId");
                HiddenField hf_PaymentId = (HiddenField)e.Item.FindControl("hf_PaymentId");
                HiddenField hf_bankgateway = (HiddenField)e.Item.FindControl("hf_bankgateway");
                HiddenField hf_GatewayTxnId = (HiddenField)e.Item.FindControl("hf_GatewayTxnId");

                string clientTxnId = hf_ClientTxnId.Value.Trim();
                string PaymentId = hf_PaymentId.Value.Trim();
                string bankGateway = hf_bankgateway.Value.Trim();

                if (bankGateway.Equals("Indian Bank", StringComparison.OrdinalIgnoreCase))
                {
                    // 🔹 Call SabPaisa API
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                    string url = "https://txnenquiry.sabpaisa.in/SPTxtnEnquiry/TransactionEnquiryServlet?clientCode=" + clientCode + "&clientXtnId=" + clientTxnId;
                    string responseString = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }

                    // Parse XML response
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseString);
                    XmlNode txnNode = xmlDoc.SelectSingleNode("/transaction");

                    if (txnNode != null)
                    {
                        string apiStatus = txnNode.Attributes["status"] != null
                             ? txnNode.Attributes["status"].Value
                             : "";

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

                        if (errorcode == "400")
                        {
                            return; // Skip this transaction
                        }

                        // Update DB
                        db.UpdateChallanInquiry(clientTxnId, apiStatus, paymentStatusCode, bankTxnId, paidAmount, paymentmode, paymentUpdateddate);

                        // Restore payment record
                        db.RestorePaymentAndStudentPayment(Convert.ToInt32(PaymentId));

                        if (apiStatus.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                        {
                            DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(clientTxnId, 0);
                            if (dsStudents != null && dsStudents.Tables.Count > 0)
                            {
                                DataTable dtStudents = dsStudents.Tables[1];
                                foreach (DataRow rowst in dtStudents.Rows)
                                {
                                    int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                    db.UpdateStudentExamFeeSubmit(studentId);
                                }
                            }

                            ScriptManager.RegisterStartupScript(this, GetType(), "RestoreSuccess",
                                "swal({ title: 'Success', text: 'Transaction restored successfully!', icon: 'success' });", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(
    this,
    GetType(),
    "RestoreFailed",
    "swal({ title: 'Failed', text: 'Transaction status: " + apiStatus + "', icon: 'error' });",
    true
);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoData",
                            "swal({ title: 'Error', text: 'No transaction data found.', icon: 'warning' });", true);
                    }
                }
                else if (bankGateway.Equals("Axis Bank", StringComparison.OrdinalIgnoreCase))
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

                    string PlainText = "CID=" + CID +
                                          "&RID=" + clientTxnId +
                                           "&CRN=" + RID +
                                           "&VER=1.0" +
                                           "&TYP=" + TYP +
                                           "&CKS=" + CKS;

                    // Encrypt the request
                    string encryptedRequest = Encrypt(PlainText, ENCKEY);

                    // URL-encode the request before sending
                    string enquiryUrl = "https://easypay.axisbank.co.in/index.php/api/enquiry?i=" + HttpUtility.UrlEncode(encryptedRequest);

                    string encryptedResponse = "";
                    using (WebClient wc = new WebClient())
                    {
                        encryptedResponse = wc.DownloadString(enquiryUrl);
                    }

                    if (encryptedResponse.StartsWith("error=", StringComparison.OrdinalIgnoreCase))
                    {
                        // Log the error and continue with next transaction

                        return; // inside a loop
                    }

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



                    db.UpdateChallanInquiry(clientTxnId, statusMsg, statusCode, txnId, paidAmount, paymentMode, paymentDate);

                    if (statusCode == "000")
                    {
                        DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(clientTxnId, 0);
                        if (dsStudents != null && dsStudents.Tables.Count > 0)
                        {
                            DataTable dtStudents = dsStudents.Tables[1];
                            foreach (DataRow rowst in dtStudents.Rows)
                            {
                                int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                db.UpdateStudentExamFeeSubmit(studentId);
                            }
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "RestoreSuccess",
                            "swal({ title: 'Success', text: 'Transaction restored successfully!', icon: 'success' });", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "RestoreFailed",
                            "swal({{ title: 'Failed', text: 'Transaction status: {statusMsg}', icon: 'error' }});", true);
                    }
                }

                else if (bankGateway.Equals("HDFC Bank", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string paymentId = hf_GatewayTxnId.Value.Trim();
                        if (!string.IsNullOrEmpty(paymentId))
                        {

                            var keySecret = "1S8pjORn6ii2M4gOAVq2QhoC";
                            var keyId = "rzp_live_Ra4140tIQFMO9O";

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

                            //LogMessage("Razorpay Response: " + jsonResponse);

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
                                    // Parse the amount (Razorpay sends it in paise)
                                    paidAmount = decimal.Parse(amountStr ?? "0", CultureInfo.InvariantCulture);

                                    // Convert paise to rupees
                                    paidAmount = paidAmount / 100;
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
                                    //continue;
                                }


                                // Update database
                                db.UpdateChallanInquiry(clientTxnId, statusdb, "", bankTxnId, paidAmount.ToString(), paymentMode, "");

                                // Update student fees if payment successful
                                if (statusdb == "SUCCESS")
                                {
                                    DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(clientTxnId, 0);
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "RestoreError",
                   "swal({{ title: 'Error', text: 'Restore failed: {ex.Message}', icon: 'error' }});", true);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "RestoreError",
                   "swal({{ title: 'Error', text: 'Restore failed: {ex.Message}', icon: 'error' }});", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "RestoreError",
                    "swal({{ title: 'Error', text: 'Restore failed: {ex.Message}', icon: 'error' }});", true);
            }
        }

        //if (e.CommandName == "lnk_Restore")
        //{
        //    try
        //    {
        //        HiddenField hf_ClientTxnId = (HiddenField)e.Item.FindControl("hf_ClientTxnId");
        //        HiddenField hf_PaymentId = (HiddenField)e.Item.FindControl("hf_PaymentId");
        //        HiddenField hf_bankgateway = (HiddenField)e.Item.FindControl("hf_bankgateway");
        //        string clientTxnId = hf_ClientTxnId.Value.Trim();
        //        string PaymentId = hf_PaymentId.Value.Trim();

        //        string clientCode = ConfigurationManager.AppSettings["Clientcode"];
        //        string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
        //        string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];

        //        string url = "https://txnenquiry.sabpaisa.in/SPTxtnEnquiry/TransactionEnquiryServlet?clientCode="
        //                     + clientCode + "&clientXtnId=" + clientTxnId;

        //        string responseString = "";

        //        // 🔹 Make request
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "GET";

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                responseString = reader.ReadToEnd();
        //            }
        //        }

        //        // 🔹 Parse XML
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(responseString);
        //        XmlNode txnNode = xmlDoc.SelectSingleNode("/transaction");

        //        if (txnNode != null)
        //        {
        //            string apiStatus = txnNode.Attributes["status"] != null ? txnNode.Attributes["status"].Value: "";

        //            string paymentStatusCode = txnNode.Attributes["sabPaisaRespCode"] != null ? txnNode.Attributes["sabPaisaRespCode"].Value : "";

        //            string bankTxnId = txnNode.Attributes["txnId"] != null ? txnNode.Attributes["txnId"].Value: "";

        //            string paidAmount = txnNode.Attributes["payeeAmount"] != null ? txnNode.Attributes["payeeAmount"].Value: "";

        //            string paymentUpdateddate = txnNode.Attributes["transCompleteDate"] != null  ? txnNode.Attributes["transCompleteDate"].Value : "";

        //            string paymentmode = txnNode.Attributes["paymentMode"] != null ? txnNode.Attributes["paymentMode"].Value : "";

        //            string errorcode = txnNode.Attributes["errorCode"] != null ? txnNode.Attributes["errorCode"].Value : "";


        //            if (errorcode == "400")
        //            {

        //                return; // go to next txnNode

        //            }

        //            // 🔹 Update DB with transaction details
        //            db.UpdateChallanInquiry(
        //                clientTxnId,
        //                apiStatus,
        //                paymentStatusCode,
        //                bankTxnId,
        //                paidAmount,
        //                paymentmode,
        //                paymentUpdateddate
        //            );

        //            // 🔹 Restore payment record
        //            db.RestorePaymentAndStudentPayment(Convert.ToInt32(PaymentId));
        //            //db.UpdatePaymentIsDeletedZero(clientTxnId);

        //            if (apiStatus.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
        //            {
        //                DataSet dsStudents = db.GetStdntPaymntDetailsTxnIdwise(clientTxnId);

        //                if (dsStudents != null && dsStudents.Tables.Count > 0)
        //                {
        //                    DataTable dtStudents = dsStudents.Tables[1];

        //                    foreach (DataRow rowst in dtStudents.Rows)
        //                    {
        //                        int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
        //                        db.UpdateStudentRegFeeSubmit(studentId);
        //                    }
        //                }

        //                // ✅ Show success message
        //                ScriptManager.RegisterStartupScript(
        //                    this,
        //                    GetType(),
        //                    "RestoreSuccess",
        //                    "swal({ title: 'Success', text: 'Transaction restored successfully!', icon: 'success' });",
        //                    true
        //                );
        //            }
        //            else
        //            {
        //                // ❌ Transaction not successful
        //                ScriptManager.RegisterStartupScript(
        //                    this,
        //                    GetType(),
        //                    "RestoreFailed",
        //                    "swal({{ title: 'Failed', text: 'Transaction status: {apiStatus}', icon: 'error' }});",
        //                    true
        //                );
        //            }
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("No transaction data found for " + clientTxnId);
        //            ScriptManager.RegisterStartupScript(
        //                this,
        //                GetType(),
        //                "NoData",
        //                "swal({ title: 'Error', text: 'No transaction data found.', icon: 'warning' });",
        //                true
        //            );
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // 🔥 Log exception
        //        System.Diagnostics.Debug.WriteLine("Restore Error: " + ex.Message);

        //        // ❌ Show error message
        //        ScriptManager.RegisterStartupScript(
        //            this,
        //            GetType(),
        //            "RestoreError",
        //            "swal({{ title: 'Error', text: 'Restore failed: {ex.Message}', icon: 'error' }});",
        //            true
        //        );
        //    }
        //}


    }

    public static string EncryptData(string plainText, string iv, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

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

    public ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting, string iv, string key)
    {
        csp.Mode = CipherMode.CBC;
        csp.Padding = PaddingMode.PKCS7;
        csp.Key = Encoding.UTF8.GetBytes(key);
        csp.IV = Encoding.UTF8.GetBytes(iv);

        return encrypting ? csp.CreateEncryptor() : csp.CreateDecryptor();
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
}