using log4net;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IdentityModel.Protocols.WSTrust;
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

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

public partial class responseAxis : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(responseAxis));
    private static bool isLog4NetConfigured = false;

    private void EnsureLog4NetConfigured()
    {
        if (!isLog4NetConfigured)
        {
            var logRepository = log4net.LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
            string configFile = Server.MapPath("~/Web.config"); // Using web.config
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(configFile));

            isLog4NetConfigured = true;
            log.Info("log4net configured.");
        }
    }

    private void EnsureLogFolder()
    {
        string logFolder = Server.MapPath("~/logs");
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }
    }
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
        EnsureLogFolder();
        EnsureLog4NetConfigured();
        log.Info("Page_Load started.");
        if (!IsPostBack)
        {
            try
            {
                log.Info("Page_Load started.");
              //  string i = "bzmAJ8GUSQu6MYvG1OczPjP0MZaH2CKnZPnoRbQ7Eyp5SSoGChWDWp8COlEyKFVgJlH3pCeKJ0QIZ46%2B5fj2a66VfDsZRd%2F%2FVXb3DqhSs8r%2B7baqwG7Zq1q6fItAwZ5qqlAfn49avnxr5Rvs4BUuLjyPW7muIy28nMRhWS8vHOzFSk6yz4L8PEE6GLjuFI1Dva6XCKHSfqbyY1%2FVq32fIGJEu37XJQ%2FCfJ7%2B5nCK8oV%2BdgrJuTYcKMazZNM%2FmDMaBkEcWTFd7PIgebamXqrvpA%3D%3D";

                string i = Request.QueryString["i"];
                if (!string.IsNullOrEmpty(i))
                {
                    string key = ConfigurationManager.AppSettings["Axis_ENCKEY"];

                    log.Info("Received query string parameter 'i': " + i);
                    string decodedI = HttpUtility.UrlDecode(i).Replace(" ", "+");

                    // Check if the parameter 'i' is not empty
                    if (!string.IsNullOrWhiteSpace(i))
                    {
                        log.Info("Parameter 'i' is not empty, proceeding with decryption.");

                        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

                        // URL Decode the 'i' parameter
                        string decoded = HttpUtility.UrlDecode(i);
                        log.Info("Decoded 'i': {decoded}");

                        // Convert the base64 string to byte array
                        byte[] toEncryptArray = Convert.FromBase64String(decodedI);

                        // Start Rijndael decryption process
                        using (RijndaelManaged rDel = new RijndaelManaged())
                        {
                            rDel.Key = keyArray;
                            rDel.Mode = CipherMode.ECB;

                            // Create decryptor
                            ICryptoTransform cTransform = rDel.CreateDecryptor();

                            // Perform decryption
                            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                            string decrypted = UTF8Encoding.UTF8.GetString(resultArray);
                            log.Info("Decrypted string: {decrypted}");

                            // Parse the decrypted response into key-value pairs
                            var keyValuePairs = decrypted.Split('&')
                                .Select(x => x.Split(new[] { '=' }, 2))
                                .Where(x => x.Length == 2)
                                .ToDictionary(x => x[0], x => x[1]);

                            // Example usage:
                            string txnStatus = keyValuePairs.ContainsKey("RMK") ? keyValuePairs["RMK"] : "";
                            string statusCode = keyValuePairs.ContainsKey("STC") ? keyValuePairs["STC"] : "";
                            string paymentMode = keyValuePairs.ContainsKey("PMD") ? keyValuePairs["PMD"] : "";
                            string clientTxnId = keyValuePairs.ContainsKey("RID") ? keyValuePairs["RID"] : "";
                            string paidAmount = keyValuePairs.ContainsKey("AMT") ? keyValuePairs["AMT"] : "";
                            string AxisTxnId = keyValuePairs.ContainsKey("TRN") ? keyValuePairs["TRN"] : "";
                            string bankTxnId = keyValuePairs.ContainsKey("BRN") ? keyValuePairs["BRN"] : "";
                            string transDate = keyValuePairs.ContainsKey("TET") ? keyValuePairs["TET"] : "";

                            log.Info("Parsed values: txnStatus={txnStatus}, clientTxnId={clientTxnId}, paidAmount={paidAmount}");

                            // Update labels with the parsed values
                            lblStatus.Text = txnStatus;
                            lblBankTransId.Text = AxisTxnId;
                            lblClientTransId.Text = clientTxnId;
                            lbl_amountpaid.Text = paidAmount;
                            lbl_paymentdate.Text = transDate;

                            int respayment = 0;

                            // Check clientTxnId and decide DB update logic
                            if (clientTxnId.StartsWith("EXM"))
                            {
                                log.Info("Client transaction starts with 'EXM', updating with special flag.");
                                respayment = db.UpdateStudentPaymentDetails(clientTxnId, paidAmount, txnStatus.ToUpper(), paymentMode, AxisTxnId, bankTxnId, transDate, statusCode, "", "", "", "", "", "1");
                            }
                            else
                            {
                                log.Info("Client transaction does not start with 'EXM', updating without special flag.");
                                respayment = db.UpdateStudentPaymentDetails(clientTxnId, paidAmount, txnStatus.ToUpper(), paymentMode, AxisTxnId, bankTxnId, transDate, statusCode, "", "", "", "", "", "0");
                            }

                            // Handle the response based on DB update and payment status
                            if (txnStatus.ToUpper() == "SUCCESS")
                            {
                                log.Info("Payment processed successfully. Both DB and Bank confirm success.");
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
                            else if (txnStatus.ToUpper() == "FAILED")
                            {
                                log.Warn("Payment failed as per bank response.");
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
                            else if (txnStatus.ToUpper() == "PENDING")
                            {
                                log.Warn("The payment is currently pending.");
                                imgFailure.Visible = true;
                                string script = @"
    swal({
        title: 'Payment Pending',
        text: 'The payment is currently pending. Please check again later.',
        icon: 'error',
        button: 'Retry'
    });";
                                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);
                            }
                            else if (respayment == 2)
                            {
                                log.Warn("No DB record found for the provided ClientTxnId.");
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
                                log.Error("Unknown error occurred during payment processing.");
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
                    }
                    else
                    {
                        log.Info("No response received from Axis Bank.");
                        Response.Write("No response received from Axis Bank.");
                    }
                }
                else
                {
                    log.Warn("No response received from any payment gateway.");
                    Response.Write("No response received from any payment gateway.");
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occurred during payment processing.", ex);

                string errorMessage = ex.Message.Replace("'", "\\'"); // Escape single quotes
                string script = @"
        swal({{
            title: 'Error',
            text: '{errorMessage}',
            icon: 'error',
            button: 'Close'
        }});";

                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentProcessingError", script, true);
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