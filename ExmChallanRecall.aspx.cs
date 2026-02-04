using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExmChallanRecall : System.Web.UI.Page
{
    DBHelper db = new DBHelper();

    public void getchallandetails()
    {
        try
        {
            DataTable result = db.getChallanDetailsbasedonTxnId(txt_CollegeCode.Text, 2);
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
        catch (Exception ex)
        {

            throw ex;
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
        try
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
        catch (Exception ex)
        {

            throw ex;
        }
     
    }

    protected async void rptransactiondetails_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnk_Restore")
            {
                HiddenField hf_ClientTxnId = (HiddenField)e.Item.FindControl("hf_ClientTxnId");
                string clientTxnId = hf_ClientTxnId.Value.Trim();

                string clientCode = ConfigurationManager.AppSettings["Clientcode"];
                string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];


                string plainData = "clientCode=" + clientCode + "&clientTxnId=" + clientTxnId;

                string encryptedData = EncryptData(plainData, authIV, authKey);

                var requestBody = new
                {
                    clientCode = clientCode,
                    statusTransEncData = encryptedData
                };

                string jsonPayload = new JavaScriptSerializer().Serialize(requestBody);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://stage-txnenquiry.sabpaisa.in/SPTxtnEnquiry/getTxnStatusByClientxnId", content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    //System.Diagnostics.Debug.WriteLine("🔒 Raw Response = " + responseString);

                    var responseObj = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(responseString);

                    if (responseObj != null && responseObj.ContainsKey("statusResponseData"))
                    {
                        string encryptedResponse = responseObj["statusResponseData"].ToString();

                        string decrypted = DecryptString(encryptedResponse, authIV, authKey);
                        System.Diagnostics.Debug.WriteLine("🔓 Decrypted Response = " + decrypted);

                        string[] pairs = decrypted.Split('&');
                        Dictionary<string, string> data = new Dictionary<string, string>();

                        foreach (string pair in pairs)
                        {
                            string[] parts = pair.Split('=');
                            if (parts.Length == 2)
                            {
                                data[parts[0]] = parts[1];
                            }
                        }

                        //sabPaisaRespdict.ContainsKey("paidAmount") ? sabPaisaRespdict["paidAmount"] : "";
                        //sabPaisaRespdict.ContainsKey("paymentMode") ? sabPaisaRespdict["paymentMode"] : "";
                        //bPaisaRespdict.ContainsKey("bankName") ? sabPaisaRespdict["bankName"] : "";
                        //sabPaisaRespdict.ContainsKey("amountType") ? sabPaisaRespdict["amountType"] : "";
                        //aisaRespdict.ContainsKey("status") ? sabPaisaRespdict["status"] : "";
                        //sabPaisaRespdict.ContainsKey("statusCode") ? sabPaisaRespdict["statusCode"] : "";
                        // = sabPaisaRespdict.ContainsKey("challanNumber") ? sabPaisaRespdict["challanNumber"] : "";
                        // = sabPaisaRespdict.ContainsKey("sabpaisaTxnId") ? sabPaisaRespdict["sabpaisaTxnId"] : "";
                        //ge = sabPaisaRespdict.ContainsKey("sabpaisaMessage") ? sabPaisaRespdict["sabpaisaMessage"] : "";
                        //sabPaisaRespdict.ContainsKey("bankMessage") ? sabPaisaRespdict["bankMessage"] : "";
                        // = sabPaisaRespdict.ContainsKey("bankErrorCode") ? sabPaisaRespdict["bankErrorCode"] : "";
                        //Code = sabPaisaRespdict.ContainsKey("sabpaisaErrorCode") ? sabPaisaRespdict["sabpaisaErrorCode"]


                        //abPaisaRespdict.ContainsKey("bankTxnId") ? sabPaisaRespdict["bankTxnId"] : "";
                        //abPaisaRespdict.ContainsKey("transDate") ? sabPaisaRespdict["transDate"] : "";



                        string status = data.ContainsKey("status") ? data["status"] : "";
                        string paymentStatusCode = data.ContainsKey("statusCode") ? data["statusCode"] : "";
                        string gatewayMessage = data.ContainsKey("sabpaisaMessage") ? data["sabpaisaMessage"] : "";
                        string gatewayErrorCode = data.ContainsKey("sabpaisaErrorCode") ? data["sabpaisaErrorCode"] : "";
                        string bankMessage = data.ContainsKey("bankMessage") ? data["bankMessage"] : "";
                        string bankErrorCode = data.ContainsKey("bankErrorCode") ? data["bankErrorCode"] : "";

                        string gatewayTxnId = data.ContainsKey("sabpaisaTxnId") ? data["sabpaisaTxnId"] : "";
                        string bankTxnId = data.ContainsKey("bankTxnId") ? data["bankTxnId"] : "";
                        string paidAmount = data.ContainsKey("paidAmount") ? data["paidAmount"] : "";

                        //              string clientTxnId,
                        //string paymentStatus,
                        //string paymentStatusCode,
                        //string gatewayMessage,
                        //string gatewayErrorCode,
                        //string bankMessage,
                        //string bankErrorCode,
                        //string challanNumber,
                        //string gatewayTxnId,
                        //string bankTxnId,
                        //decimal? paidAmount,

                        if (status == "SUCCESS")
                        {
                            int result = db.UpdateChallanRecall(clientTxnId, status, paymentStatusCode, gatewayMessage, gatewayErrorCode, bankMessage, bankErrorCode, gatewayTxnId, bankTxnId, paidAmount);
                            if (result == 0)
                            {

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertBox", "alert('✅ Exam Challan restored successfully.');", true);
                            }
                            else if (result == 2)
                            {

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertBox", "alert('❌ Failed: Transaction ID not found.');", true);
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertBox", "alert('❌ Restore failed. Please try again.');", true);
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertBox", "alert('❌ Restore Failed. Status: " + status + "');", true);
                        }

                    }
                    else
                    {
                        // Handle error/log
                        System.Diagnostics.Debug.WriteLine("❗ Encrypted field 'statusResponseData' not found.");
                    }

                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    
    }

    public static string EncryptData(string plainText, string iv, string key)
    {
        try
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
        catch (Exception ex)
        {

            throw ex;
        }
      
    }

    public static string DecryptString(string encrypted, string iv, string key)
    {
        try
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
        catch (Exception ex)
        {

            throw ex;
        }
     
    }

    public ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting, string iv, string key)
    {
        try
        {
            csp.Mode = CipherMode.CBC;
            csp.Padding = PaddingMode.PKCS7;
            csp.Key = Encoding.UTF8.GetBytes(key);
            csp.IV = Encoding.UTF8.GetBytes(iv);

            return encrypting ? csp.CreateEncryptor() : csp.CreateDecryptor();
        }
        catch (Exception ex)
        {

            throw ex;
        }
     
    }
}