using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Paymentupdate : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
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

    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        DataSet result1 = db.GetExamPaymentDataDatewise(ddl_paymode.SelectedValue,Convert.ToDateTime(txtfromDate.Text), Convert.ToDateTime(txttoDate.Text));
        if (result1 != null && result1.Tables.Count > 0 && result1.Tables[0].Rows.Count > 0)
        {
            rpt_getpayemnt.DataSource = result1.Tables[0];
            rpt_getpayemnt.DataBind();
            pnlPager.Visible = true;
            pnlStudentTable.Visible = true;
            searchInputDIV.Visible = true;
        }
        else
        {
            rpt_getpayemnt.DataSource = null;
            rpt_getpayemnt.DataBind();
            pnlPager.Visible = false;
            pnlStudentTable.Visible = false;
            searchInputDIV.Visible = false;


        }
    }

    protected void UpdateAllTransaction_Click(object sender, EventArgs e)
    {
        string selectedStudentIds = hfSelectedIds.Value;

        string txnids = hfselectedclientxnid.Value;
        
      //  string txnids = "EXM4207120250920073610039944,EXM3323320250920075621199701"; // comma-separated txnIds
        if (string.IsNullOrEmpty(txnids))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                "swal({ title: 'Failed', text: 'Please select at least one Transaction Id', icon: 'error', button: 'Retry' });", true);
            return;
        }

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string[] txnIdArray = txnids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        // Initialize concatenated strings
        // Initialize concatenated strings
        string updatedTxns = "";
        string notUpdatedTxns = "";
        string skippedTxns = "";
        string failedTxns = "";

        foreach (string clientTxnId in txnIdArray)
        {
            string txnIdTrimmed = clientTxnId.Trim();
            if (string.IsNullOrEmpty(txnIdTrimmed)) continue;

            try
            {

                if (ddl_paymode.SelectedValue == "Indian Bank") // SabPaisa
                {

                    string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                    string url = "https://txnenquiry.sabpaisa.in/SPTxtnEnquiry/TransactionEnquiryServlet?clientCode="
                                 + clientCode + "&clientXtnId=" + txnIdTrimmed;

                    string responseString = "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = reader.ReadToEnd();
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseString);
                    XmlNode txnNode = xmlDoc.SelectSingleNode("/transaction");

                    if (txnNode != null)
                    {
                        string apiStatus = txnNode.Attributes["status"] != null ? txnNode.Attributes["status"].Value : "";
                        string errorcode = txnNode.Attributes["errorCode"] != null ? txnNode.Attributes["errorCode"].Value : "";

                        if (errorcode == "400")
                        {
                            skippedTxns += txnIdTrimmed + ", ";
                            continue;
                        }

                        int rowsAffected = db.UpdateChallanInquiry(txnIdTrimmed, apiStatus,
                            txnNode.Attributes["sabPaisaRespCode"] != null ? txnNode.Attributes["sabPaisaRespCode"].Value : "",
                            txnNode.Attributes["txnId"] != null ? txnNode.Attributes["txnId"].Value : "",
                            txnNode.Attributes["payeeAmount"] != null ? txnNode.Attributes["payeeAmount"].Value : "",
                            txnNode.Attributes["paymentMode"] != null ? txnNode.Attributes["paymentMode"].Value : "",
                            txnNode.Attributes["transCompleteDate"] != null ? txnNode.Attributes["transCompleteDate"].Value : "");

                        db.updateschedulerbit(txnIdTrimmed);

                        if (rowsAffected > 0) updatedTxns += txnIdTrimmed + ", ";
                        else notUpdatedTxns += txnIdTrimmed + ", ";

                        if (apiStatus.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
                        {
                            DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(txnIdTrimmed,0);
                            if (dsStudents != null && dsStudents.Tables.Count > 1)
                            {
                                foreach (DataRow rowst in dsStudents.Tables[1].Rows)
                                {
                                    int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                    db.UpdateStudentExamFeeSubmit(studentId);
                                }
                            }
                        }
                        else
                        {
                            failedTxns += txnIdTrimmed + " (" + apiStatus + "), ";
                        }
                    }
                    else
                    {
                        failedTxns += txnIdTrimmed + " (No Response), ";
                    }
                }
                else if (ddl_paymode.SelectedValue == "Axis Bank")
                {
                    string CID = ConfigurationManager.AppSettings["Axis_CID"];
                    string RID = txnIdTrimmed;
                    string key = ConfigurationManager.AppSettings["Axis_ChecksumKey"];
                    string ENCKEY = ConfigurationManager.AppSettings["Axis_ENCKEY"];
                    string TYP = ConfigurationManager.AppSettings["Axis_TYP"];

                    // Compute checksum
                    string checksumInput = CID + RID + RID + key;
                    string CKS = sha256_hash(checksumInput);

                    string PlainText = "CID=" + CID + "&RID=" + txnIdTrimmed + "&CRN=" + RID + "&VER=1.0" + "&TYP=" + TYP + "&CKS=" + CKS;

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
                        skippedTxns += txnIdTrimmed + ", ";
                        continue; // inside a loop
                    }

                    string decodedResponse = HttpUtility.UrlDecode(encryptedResponse);


                    decodedResponse = decodedResponse.Replace(" ", "+").Replace("\r", "").Replace("\n", "").Trim();
                    decodedResponse = System.Text.RegularExpressions.Regex.Replace(decodedResponse, @"[^A-Za-z0-9\+/=]", "");

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

                    db.updateschedulerbit(txnIdTrimmed);


                    if (statusCode == "000") // Success
                    {
                        DataSet dsStudents = db.GetExmPaymntDetailsTxnIdwise(txnIdTrimmed,0);
                        if (dsStudents != null && dsStudents.Tables.Count > 0)
                        {
                            DataTable dtStudents = dsStudents.Tables[1];
                            foreach (DataRow rowst in dtStudents.Rows)
                            {
                                int studentId = Convert.ToInt32(rowst["Fk_StudentId"]);
                                db.UpdateStudentExamFeeSubmit(studentId);
                            }
                        }
                        updatedTxns += txnIdTrimmed + ", ";
                    }
                    else
                    {
                        failedTxns += txnIdTrimmed + " (" + statusMsg + "), ";
                    }
                }

                else if (ddl_paymode.SelectedValue == "HDFC Bank")
                {

                }


            }
            catch (Exception ex)
            {
                failedTxns += txnIdTrimmed + " (Error: " + ex.Message + "), ";
            }
        }

        if (updatedTxns.EndsWith(", ")) updatedTxns = updatedTxns.Substring(0, updatedTxns.Length - 2);
        if (notUpdatedTxns.EndsWith(", ")) notUpdatedTxns = notUpdatedTxns.Substring(0, notUpdatedTxns.Length - 2);
        if (skippedTxns.EndsWith(", ")) skippedTxns = skippedTxns.Substring(0, skippedTxns.Length - 2);
        if (failedTxns.EndsWith(", ")) failedTxns = failedTxns.Substring(0, failedTxns.Length - 2);

        // Build final message
        string finalMsg = "";
        if (!string.IsNullOrEmpty(updatedTxns)) finalMsg += "Updated: " + updatedTxns + "\\n";
        if (!string.IsNullOrEmpty(notUpdatedTxns)) finalMsg += "Not Updated: " + notUpdatedTxns + "\\n";
        if (!string.IsNullOrEmpty(skippedTxns)) finalMsg += "Skipped: " + skippedTxns + "\\n";
        if (!string.IsNullOrEmpty(failedTxns)) finalMsg += "Failed/Aborted: " + failedTxns;

        // If final message is empty (no txn processed), show default message
        if (string.IsNullOrEmpty(finalMsg)) finalMsg = "No transactions processed.";

        // Escape single quotes for JavaScript safety
        finalMsg = finalMsg.Replace("'", "\\'");

        // SweetAlert script
        string script = "swal({ title: 'Process Completed', text: '" + finalMsg + "', icon: 'success', button: 'OK' });";
        ScriptManager.RegisterStartupScript(this, GetType(), "FinalTxnUpdateMsg", script, true);





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
}