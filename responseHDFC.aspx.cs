using Newtonsoft.Json.Linq;
using Razorpay.Api;
using Razorpay.Api.Errors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class responseHDFC : System.Web.UI.Page
{
    DBHelper db = new DBHelper();

    private void LogMessage(string message)
    {
        try
        {
            string logPath = Server.MapPath("~/Logs/HDFCLOG.txt");
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 1️⃣ Get Parameters from the Query String
            string paymentId = Request.Params["razorpay_payment_id"];
            string orderId = Request.Params["razorpay_order_id"];
            string signature = Request.Params["razorpay_signature"];

            string body;
            using (var reader = new StreamReader(Request.InputStream))
            {
                body = reader.ReadToEnd();
            }

            //string decoded = HttpUtility.UrlDecode(body);

            string decoded = WebUtility.UrlDecode(body);

            var parts = decoded.Split('&');

            // Extract key/value pairs into a dictionary

            var dict = new Dictionary<string, string>();

            foreach (var part in parts)

            {

                var kv = part.Split(new[] { '=' }, 2);

                if (kv.Length == 2)

                    dict[kv[0]] = kv[1];

            }

            string errorCode = dict.ContainsKey("error[code]") ? dict["error[code]"] : "";
            string errorDescription = dict.ContainsKey("error[description]") ? dict["error[description]"] : "";
            string errorSource = dict.ContainsKey("error[source]") ? dict["error[source]"] : "";
            string errorStep = dict.ContainsKey("error[step]") ? dict["error[step]"] : "";
            string errorReason = dict.ContainsKey("error[reason]") ? dict["error[reason]"] : "";
            string errorMetadata = dict.ContainsKey("error[metadata]") ? dict["error[metadata]"] : "";


            string paymentIdMeta = "";
            string orderIdMeta = "";

            // Only parse metadata if it’s non-empty and looks like JSON
            if (!string.IsNullOrWhiteSpace(errorMetadata) && errorMetadata.Contains(":"))
            {
                try
                {
                    var metaParts = errorMetadata
                        .Trim('{', '}')
                        .Replace("\"", "")
                        .Split(',');

                    foreach (var meta in metaParts)
                    {
                        var kv = meta.Split(':');
                        if (kv.Length != 2) continue; // skip invalid parts

                        string key = kv[0].Trim();
                        string value = kv[1].Trim();

                        if (key == "payment_id") paymentIdMeta = value;
                        if (key == "order_id") orderIdMeta = value;
                    }
                }
                catch
                {
                    // fallback: invalid or unexpected metadata format
                    paymentIdMeta = "";
                    orderIdMeta = "";
                }
            }


            //string errorCode = Request.Params["error[code]"];
            //string errorDescription = Request.Params["error[description]"];

            //string errorReason = Request.Params["error[reason]"];
            //string errorStep = Request.Params["error[step]"];
            //string paymentIdMeta = Request.Params["error[metadata][payment_id]"];
            //string orderIdMeta = Request.Params["error[metadata][order_id]"];







            LogMessage("Incoming Request: PID=" + paymentId + ", OID=" + orderId + ", SIGN=" + signature);

            if (!string.IsNullOrEmpty(errorCode))
            {
                string safeDesc = (errorDescription ?? "Unknown error").Replace("'", "").Replace("\n", " ");

                string orderRef = !string.IsNullOrEmpty(orderIdMeta) ? orderIdMeta : orderId;
                string paymentRef = !string.IsNullOrEmpty(paymentIdMeta) ? paymentIdMeta : paymentId;

               // lblClientTransId.Text = clientTxnId;
                lblBankTransId.Text = orderRef;
                //lbl_amountpaid.Text = paidAmount.ToString("N2") + " " + currency;
               // lbl_paymentdate.Text = transactionDateString;
                lblStatus.Text = errorStep;
               // lbl_orderid.Text = paymentRef;


                // ✅ Update failure in database
                string updateResult = db.UpdatePaymentDataByBankTxnIdUsingSP(
                    orderRef,
                    null,              // AmountPaid
                   errorStep,            // PaymentStatus
                    "",               // PaymentMode
                    paymentRef, // GatewayTxnId
                    DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    errorCode            // BankErrorCode
                );

                LogMessage("Payment FAILED update result: " + updateResult);

                // 🔔 Show failure alert
                string failScript =
                    "swal({" +
                    "title: 'Payment Failed'," +
                    "text: 'Code: " + errorCode +
                    "\\nDescription: " + safeDesc +
                    (string.IsNullOrEmpty(errorReason) ? "" : "\\nReason: " + errorReason) +
                    (string.IsNullOrEmpty(errorStep) ? "" : "\\nStep: " + errorStep) +
                    (string.IsNullOrEmpty(paymentIdMeta) ? "" : "\\nPayment ID: " + paymentIdMeta) +
                    (string.IsNullOrEmpty(orderIdMeta) ? "" : "\\nOrder ID: " + orderIdMeta) + "'," +
                    "icon: 'error'," +
                    "button: 'Retry'" +
                    "});";

                ScriptManager.RegisterStartupScript(this, GetType(), "fail", failScript, true);
                imgFailure.Visible = true;
                return;
            }




            // 2️⃣ Validate parameters
            if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(signature))
            {
                string alertScript = "alert('❌ Payment parameters missing or payment was cancelled.');";
                ClientScript.RegisterStartupScript(this.GetType(), "alertMissingParams", alertScript, true);
                return;
            }

            // 3️⃣ Retrieve Razorpay keys
            var keySecret = "1S8pjORn6ii2M4gOAVq2QhoC";
            var keyId = "rzp_live_Ra4140tIQFMO9O";

            try
            {
                RazorpayClient client = new RazorpayClient(keyId, keySecret);

                Dictionary<string, string> options = new Dictionary<string, string>();
                options.Add("razorpay_order_id", orderId);
                options.Add("razorpay_payment_id", paymentId);
                options.Add("razorpay_signature", signature);

                Utils.verifyPaymentSignature(options);

                // 4️⃣ Fetch payment details
                Payment payment = client.Payment.Fetch(paymentId);

                string status = payment.Attributes["status"].ToString() ?? "";
                string statusdb = "";
                if (status == "captured")
                    statusdb = "SUCCESS";
                else if (status == "failure")
                    statusdb = "FAILED";
                else if (status == "authorized")
                    statusdb = "AUTHORIZED";

                string bank = payment.Attributes["bank"].ToString() ?? "";
                string paymentMode = payment.Attributes["method"].ToString() ?? "";

                long amountInPaise = Convert.ToInt64(payment.Attributes["amount"]);
                decimal paidAmount = amountInPaise / 100; // you can divide by 100m if needed

                string currency = payment.Attributes["currency"].ToString() ?? "INR";
                long createdAtUnix = Convert.ToInt64(payment.Attributes["created_at"]);
                DateTime transactionDate = DateTimeOffset.FromUnixTimeSeconds(createdAtUnix).LocalDateTime;
                string transactionDateString = transactionDate.ToString("dd-MMM-yyyy hh:mm:ss tt");

                string paymentErrorCode = payment.Attributes["error_code"].ToString() ?? "";
                string paymentErrorDescription = payment.Attributes["error_description"].ToString() ?? "";

                Order order = client.Order.Fetch(payment.Attributes["order_id"].ToString());
                var notes = order.Attributes["notes"] as Dictionary<string, object>;
                string clientTxnId = "";
                if (notes != null && notes.ContainsKey("clientTxnId"))
                {
                    clientTxnId = notes["clientTxnId"].ToString() ?? "";
                }

                LogMessage("Signature verification successful.");
                LogMessage("Status: " + status);
                LogMessage("Bank: " + bank);
                LogMessage("Payment Mode: " + paymentMode);
                LogMessage("Amount: " + paidAmount.ToString() + " " + currency);
                LogMessage("Transaction Date: " + transactionDateString);
                LogMessage("Error Code: " + errorCode);

                lblClientTransId.Text = clientTxnId;
                lblBankTransId.Text = orderId;
                lbl_amountpaid.Text = paidAmount.ToString("N2") + " " + currency;
                lbl_paymentdate.Text = transactionDateString;
                lblStatus.Text = statusdb;
                //lbl_orderid.Text = orderId;

                string respayment = db.UpdatePaymentDataByBankTxnIdUsingSP(orderId, paidAmount.ToString(), statusdb, paymentMode, paymentId, transactionDateString, errorCode);

                if (respayment == "0" && statusdb.ToUpper() == "SUCCESS")
                {


                    imgSuccess.Visible = true;
                    string script = @"
                swal({
                    title: 'Success!',
                    text: 'Payment processed successfully.',
                    icon: 'success',
                    button: 'OK'
                });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PaymentSuccess", script, true);
                }
                else if (respayment == "0" && (statusdb.ToUpper() == "FAILED" || statusdb.ToUpper() == "AUTHORIZED"))
                {
                    imgFailure.Visible = true;
                    string script = @"
                swal({
                    title: 'Payment Failed',
                    text: 'Payment failed as per bank response. Please try again.',
                    icon: 'error',
                    button: 'Retry'
                });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
                }
            }
            catch (Razorpay.Api.Errors.BadRequestError ex)
            {
                // 🔴 Handle Razorpay BadRequest (like invalid orderId, paymentId, etc.)
                LogMessage("Razorpay BadRequestError: " + ex.Message);
                imgFailure.Visible = true;
                string alertScript = "alert('⚠️ Razorpay Bad Request: " + ex.Message.Replace("'", "") + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "alertBadRequest", alertScript, true);
            }
            catch (Razorpay.Api.Errors.SignatureVerificationError ex)
            {
                // 🔒 Handle invalid or tampered signature
                LogMessage("SignatureVerificationError: " + ex.Message);
                imgFailure.Visible = true;
                string alertScript = "alert('❌ Signature verification failed. Please contact support.');";
                ClientScript.RegisterStartupScript(this.GetType(), "alertSignatureFail", alertScript, true);
            }
            catch (Exception ex)
            {
                // ⚠️ Handle any general/unexpected errors
                LogMessage("General Exception: " + ex.Message);
                string safeError = ex.Message.Replace("'", "").Replace("\n", " ");
                string alertScript = "alert('⚠️ Unexpected error: " + safeError + "');";
                ClientScript.RegisterStartupScript(this.GetType(), "alertGeneralError", alertScript, true);
            }
        }

    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Response.Redirect("PayExamFormFee.aspx");
    }
}
    
