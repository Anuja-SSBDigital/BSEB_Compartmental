using System;
using System.Activities.Statements;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;


public partial class response : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    Encryption sabPaisa = new Encryption();
    public Dictionary<string, string> sabPaisaResponse(string query, string authIV, string authKey)
    {
        string decQuery = "";
        //modified by Ritika lath on 1st april 2019
        query.Replace("%2B", "+");
        // query.Replace(" ", "+");

        decQuery = DecryptString(query, authIV, authKey);
        Dictionary<string, string> dictParams = new Dictionary<string, string>();

        dictParams = quearyParser(decQuery);

        /*foreach (KeyValuePair<string, string> pair in dictParams)
            {
                    Console.WriteLine(pair.Key.ToString ()+ "  -  "  + pair.Value.ToString () );
            }*/
        return dictParams;
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
    private static Dictionary<string, string> quearyParser(String values)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        //String values = "pgRespCode=F&PGTxnNo=37811436&SabPaisaTxId=7399702091711511122664015&issuerRefNo=NA&authIdCode=0&amount=57.0&clientTxnId=TESTING020917115040588&firstName=TPK&lastName=Test&payMode=CreditCards&email=test@gmail.com&mobileNo=9908944111&spRespCode=0000&cid=null&bid=null&clientCode=CXY10&payeeProfile=Student&transDate=Sat Sep 02 11:55:00 IST 2017&spRespStatus=success¶m3=BE&challanNo=&reMsg=null&orgTxnAmount=55.0&programId=mtech";

        string[] sites = values.Split('&');
        String[] token;

        foreach (string s in sites)
        {
            token = s.Split('=');
            dict.Add(token.GetValue(0).ToString(), token.GetValue(1).ToString());
        }
        return dict;
    }

    public static string DecryptString(string encrypted, string AuthIV, string AuthKey)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            // encrypted = encrypted.Substring(0, encrypted.IndexOf("&"));
            var d = GetCryptoTransform(csp, false, AuthIV, AuthKey);
            byte[] output = Convert.FromBase64String(encrypted);

            byte[] decryptedOutput = d.TransformFinalBlock(output, 0, output.Length);
            string decypted = Encoding.UTF8.GetString(decryptedOutput);
            return decypted;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string encryptedResponse = Request.Form["encResponse"];

                if (string.IsNullOrEmpty(encryptedResponse))
                {
                    //divresponse.InnerHtml = "No response received from SabPaisa.";
                    //return;
                }

                string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];

                // URL decode the query in case it was URL encoded
                string decodedQuery = HttpUtility.UrlDecode(encryptedResponse);
                string decQuery = decodedQuery.Replace("%2B", "+").Replace("%2F", "/").Replace("%3D", "=");
                decQuery = sabPaisa.DecryptString(authKey, authIV, decQuery);

                Dictionary<string, string> dictParams = sabPaisa.queryParser(decQuery);
                // Format the response dictionary into a string for display
                string formattedString = string.Join("<br/>", dictParams.Select(kv => kv.Key + ": " + kv.Value));

                string ghg = formattedString;
                //foreach (KeyValuePair<string, string> pair in sabPaisaRespdict)
                //{
                //    //divresponse.InnerHtml += "<br/>" + pair.Key + " - " + pair.Value;

                //    if (pair.Key.Equals("statusCode", StringComparison.OrdinalIgnoreCase))
                //    {
                //        lblStatus.Text = pair.Value;
                //    }
                //}

                // Optional: Update DB based on payment status
                string payerName = dictParams.ContainsKey("payerName") ? dictParams["payerName"] : "";
                string payerEmail = dictParams.ContainsKey("payerEmail") ? dictParams["payerEmail"] : "";
                string payerMobile = dictParams.ContainsKey("payerMobile") ? dictParams["payerMobile"] : "";
                string clientTxnId = dictParams.ContainsKey("clientTxnId") ? dictParams["clientTxnId"] : "";
                string payerAddress = dictParams.ContainsKey("payerAddress") ? dictParams["payerAddress"] : "";
                string amount = dictParams.ContainsKey("amount") ? dictParams["amount"] : "";
                string clientCode = dictParams.ContainsKey("clientCode") ? dictParams["clientCode"] : "";
                string paidAmount = dictParams.ContainsKey("paidAmount") ? dictParams["paidAmount"] : "";
                string paymentMode = dictParams.ContainsKey("paymentMode") ? dictParams["paymentMode"] : "";
                string bankName = dictParams.ContainsKey("bankName") ? dictParams["bankName"] : "";
                string amountType = dictParams.ContainsKey("amountType") ? dictParams["amountType"] : "";
                string status = dictParams.ContainsKey("status") ? dictParams["status"] : "";
                string statusCode = dictParams.ContainsKey("statusCode") ? dictParams["statusCode"] : "";
                string challanNumber = dictParams.ContainsKey("challanNumber") ? dictParams["challanNumber"] : "";
                string sabpaisaTxnId = dictParams.ContainsKey("sabpaisaTxnId") ? dictParams["sabpaisaTxnId"] : "";
                string sabpaisaMessage = dictParams.ContainsKey("sabpaisaMessage") ? dictParams["sabpaisaMessage"] : "";
                string bankMessage = dictParams.ContainsKey("bankMessage") ? dictParams["bankMessage"] : "";
                string bankErrorCode = dictParams.ContainsKey("bankErrorCode") ? dictParams["bankErrorCode"] : "";
                string sabpaisaErrorCode = dictParams.ContainsKey("sabpaisaErrorCode") ? dictParams["sabpaisaErrorCode"] : "";
                string bankTxnId = dictParams.ContainsKey("bankTxnId") ? dictParams["bankTxnId"] : "";
                string transDate = dictParams.ContainsKey("transDate") ? dictParams["transDate"] : "";
                lblApplicantName.Text = payerName;
                lblBankTransId.Text = bankTxnId;
                lblClientTransId.Text = clientTxnId;
                lblStatus.Text = status;
                lbl_amountpaid.Text = paidAmount;
                lbl_paymentdate.Text = transDate;



                int respayment = 0;
                //if (clientTxnId.StartsWith("EXM"))
                //{
                    respayment = db.UpdateStudentPaymentDetails(clientTxnId, paidAmount, status, paymentMode, sabpaisaTxnId, bankTxnId, transDate, statusCode, sabpaisaMessage, challanNumber, sabpaisaErrorCode, bankMessage, bankErrorCode, "1");
                //}
               

                // Handle based on both DB update and payment status
                if (respayment == 0 && status.ToUpper() == "SUCCESS")
                {
                    // Success from both bank and DB
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
                else if (respayment == 0 && status.ToUpper() == "FAILED")
                {
                    // Bank marked it as failed
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
                else if (respayment == 2)
                {
                    // No DB record found
                    imgFailure.Visible = true;
                    string script = @"
                swal({
                    title: 'No Record Found',
                    text: 'No payment record found for the given ClientTxnId.',
                    icon: 'warning',
                    button: 'Close'
                });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PaymentNotFound", script, true);
                }
                else
                {
                    // Unknown case
                    imgFailure.Visible = true;
                    string script = @"
                swal({
                    title: 'Error',
                    text: 'An unknown error occurred during payment processing.',
                    icon: 'error',
                    button: 'Close'
                });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PaymentUnknownError", script, true);
                }

            }
            catch (Exception ex)
            {
                imgFailure.Visible = true;
                string safeMessage = ex.Message.Replace("'", "\\'");
                string script = @"
    swal({
        title: 'Error',
        text: 'Error processing response: " + safeMessage + @"',
        icon: 'error',
        button: 'Close'
    });";
                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentCatchError", script, true);
            }
        }
    }

    //protected void btn_back_Click(object sender, EventArgs e)
    //{
    //    string clientTxnId = lblClientTransId.Text;
    //    if (string.IsNullOrEmpty(clientTxnId)) clientTxnId = "unknown_" + DateTime.Now.Ticks;

    //    string receiptFileName = clientTxnId + "_receipt.pdf";
    //    string folderPath = Server.MapPath("~/uploads/paymentreceipts/");
    //    Directory.CreateDirectory(folderPath);
    //    string fullFilePath = Path.Combine(folderPath, receiptFileName);

    //    try
    //    {
    //        // Render the DIV (server control) to HTML
    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);
    //        btn_back.Visible = false; // hide button before rendering

    //        DivCongratulation.RenderControl(hw);
    //        btn_back.Visible = true; // restore visibility if you still need it later

    //        string htmlFragment = sw.ToString();

    //        // Fix relative image paths
    //        string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
    //        htmlFragment = htmlFragment.Replace("src=\"assets/", "src=\"" + baseUrl + "assets/");

    //        // Add minimal CSS
    //        string css = @"<style>
    //    body{font-family: Arial, Helvetica, sans-serif; font-size:12px;}
    //    .detail-table td:first-child{font-weight:bold;}
    //    table.detail-table td{padding:6px; border-bottom:1px solid #eee;}
    //    .header .sus{color:#2d7f2d;}
    //    .outerbox-cograts{padding:0; margin:0;}
    //    </style>";

    //        string fullHtml = "<html><head><meta charset='utf-8'/>" + css + "</head><body>" + htmlFragment + "</body></html>";

    //        // Create PDF using XMLWorker
    //        using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
    //        {
    //            using (Document pdfDoc = new Document(PageSize.A4, 36, 36, 54, 36))
    //            {
    //                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
    //                pdfDoc.Open();

    //                using (var sr = new StringReader(fullHtml))
    //                {
    //                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
    //                }

    //                pdfDoc.Close();
    //            }
    //        }

    //        // Update DB with relative path
    //        string relativePath = "/uploads/paymentreceipts/" + receiptFileName;
    //        bool updated = db.UpdatePaymentReceiptPath(clientTxnId, relativePath);

    //        if (updated)
    //        {
    //            if (clientTxnId.StartsWith("EXM"))
    //            {
    //                Response.Redirect("PayExamFormFee.aspx");
    //            }
    //            else
    //            {
    //                Response.Redirect("payregstudentfee.aspx");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("Error saving receipt PDF: " + ex.Message);
    //    }
    //}
    protected void btn_back_Click(object sender, EventArgs e)
    {
        string clientTxnId = lblClientTransId.Text;
        //if (string.IsNullOrEmpty(clientTxnId))
        //    clientTxnId = "unknown_" + DateTime.Now.Ticks;

        //string receiptFileName = clientTxnId + "_receipt.pdf";
        //string folderPath = Server.MapPath("~/uploads/paymentreceipts/");
        //Directory.CreateDirectory(folderPath);
        //string fullFilePath = Path.Combine(folderPath, receiptFileName);

        //try
        //{
        //    using (FileStream fs = new FileStream(fullFilePath, FileMode.Create))
        //    {
        //        Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
        //        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
        //        doc.Open();

        //        // Fonts
        //        Font headerFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK);
        //        Font subHeaderFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.DARK_GRAY);
        //        Font labelFont = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK);
        //        Font valueFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);

        //        // --- Header (Logo + Title) ---
        //        PdfPTable headerTable = new PdfPTable(2);
        //        headerTable.WidthPercentage = 100;
        //        headerTable.SetWidths(new float[] { 20f, 80f });

        //        string logoPath = Server.MapPath("~/assets/img/bsebimage2.png");
        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
        //        logo.ScaleToFit(70f, 70f);

        //        PdfPCell logoCell = new PdfPCell(logo);
        //        logoCell.Border = Rectangle.NO_BORDER;
        //        logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        headerTable.AddCell(logoCell);

        //        PdfPCell titleCell = new PdfPCell();
        //        titleCell.Border = Rectangle.NO_BORDER;
        //        titleCell.AddElement(new Paragraph("BIHAR SCHOOL EXAMINATION BOARD", headerFont));
        //        titleCell.AddElement(new Paragraph("INTERMEDIATE REGISTRATION SESSION (2025-27)", subHeaderFont));
        //        headerTable.AddCell(titleCell);

        //        doc.Add(headerTable);

        //        // Spacer
        //        doc.Add(new Paragraph("\n"));

        //        // --- Receipt Heading ---
        //        Paragraph receiptHeading = new Paragraph("Receipt", headerFont);
        //        receiptHeading.Alignment = Element.ALIGN_CENTER;
        //        doc.Add(receiptHeading);

        //        doc.Add(new Paragraph("\n"));

        //        // --- Detail Table ---
        //        PdfPTable detailTable = new PdfPTable(2);
        //        detailTable.WidthPercentage = 100;
        //        detailTable.SetWidths(new float[] { 40f, 60f });
        //        detailTable.SpacingBefore = 10f;
        //        detailTable.SpacingAfter = 20f;

        //        AddRow(detailTable, "+2 School/College Code & Name:", lblApplicantName.Text, labelFont, valueFont);
        //        AddRow(detailTable, "Transaction ID:", lblClientTransId.Text, labelFont, valueFont);
        //        AddRow(detailTable, "Bank Transaction ID:", lblBankTransId.Text, labelFont, valueFont);
        //        AddRow(detailTable, "Amount Paid:", lbl_amountpaid.Text, labelFont, valueFont);
        //        AddRow(detailTable, "Payment Date:", lbl_paymentdate.Text, labelFont, valueFont);
        //        AddRow(detailTable, "Status:", lblStatus.Text, labelFont, valueFont);

        //        doc.Add(detailTable);

        //        doc.Close();
        //    }

        //    // Save DB path
        //    string relativePath = "/uploads/paymentreceipts/" + receiptFileName;
        //    db.UpdatePaymentReceiptPath(clientTxnId, relativePath);

            // Redirect
            if (clientTxnId.StartsWith("EXM"))
                Response.Redirect("PayExamFormFee.aspx");
            else
                Response.Redirect("payregstudentfee.aspx");
        //}
        //catch (Exception ex)
        //{
        //    Response.Write("Error saving receipt PDF: " + ex.Message);
        //}
    }

    private void AddRow(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
    {
        PdfPCell cell1 = new PdfPCell(new Phrase(label, labelFont));
        cell1.Border = Rectangle.NO_BORDER;
        cell1.PaddingBottom = 8;
        table.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(value, valueFont));
        cell2.Border = Rectangle.NO_BORDER;
        cell2.PaddingBottom = 8;
        table.AddCell(cell2);
    }
    //protected void btn_back_Click(object sender, EventArgs e)
    //{
    //    string clientTxnId = lblClientTransId.Text;
    //    string payerName = lblApplicantName.Text;
    //    string amount = lbl_amountpaid.Text;
    //    string status = lblStatus.Text;
    //    string paymentDate = lbl_paymentdate.Text;
    //    string bankTxnId = lblBankTransId.Text;

    //    try
    //    {
    //        // 1. Define file name and path
    //        string receiptFileName = clientTxnId + "_receipt.pdf";
    //        string folderPath = Server.MapPath("~/uploads/paymentreceipts/");
    //        Directory.CreateDirectory(folderPath);
    //        string fullFilePath = Path.Combine(folderPath, receiptFileName);

    //        // 2. Generate PDF (iTextSharp example)
    //        using (FileStream fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
    //        using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
    //        using (iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs))
    //        {
    //            doc.Open();
    //            doc.Add(new iTextSharp.text.Paragraph("Payment Receipt"));
    //            doc.Add(new iTextSharp.text.Paragraph("-------------------------------"));
    //            doc.Add(new iTextSharp.text.Paragraph("Transaction ID: " + clientTxnId));
    //            doc.Add(new iTextSharp.text.Paragraph("Bank Transaction ID: " + bankTxnId));
    //            doc.Add(new iTextSharp.text.Paragraph("Name: " + payerName));
    //            doc.Add(new iTextSharp.text.Paragraph("Amount Paid: " + amount));
    //            doc.Add(new iTextSharp.text.Paragraph("Payment Date: " + paymentDate));
    //            doc.Add(new iTextSharp.text.Paragraph("Status: " + status));
    //            doc.Close();
    //        }

    //        // 3. Save PDF path in DB
    //        string relativePath = "/uploads/paymentreceipts/" + receiptFileName;
    //        bool updated = db.UpdatePaymentReceiptPath(clientTxnId, relativePath);

    //        // 4. Redirect based on condition
    //        if (updated)
    //        {
    //            if (clientTxnId.StartsWith("EXM"))
    //            {
    //                Response.Redirect("PayExamFormFee.aspx");
    //            }
    //            else
    //            {
    //                Response.Redirect("payregstudentfee.aspx");
    //            }
    //        }
    //        else
    //        {
    //            // If DB update failed
    //            Response.Write("Error: Could not update receipt path in DB.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("Error generating receipt: " + ex.Message);
    //    }
    //}

    //protected void btn_back_Click(object sender, EventArgs e)
    //{
    //    string clienttxnid = lblClientTransId.Text;
    //    if (clienttxnid.StartsWith("EXM"))
    //    {
    //        Response.Redirect("PayExamFormFee.aspx");
    //    }
    //    else
    //    {
    //        Response.Redirect("payregstudentfee.aspx");
    //    }


    //}
}