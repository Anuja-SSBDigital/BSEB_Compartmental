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

public partial class payregstudentfee : System.Web.UI.Page
{
    DBHelper db = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            if (!IsPostBack)
            {

                txt_collegename.Text = Session["CollegeName"].ToString() + " | " + Session["CollegeCode"].ToString();
                BindFacultydropdown();
                BindCollegedropdown();
                divpayment.Visible = false;
                divstudentdetails.Visible = false;
                rdo_makepayment.Checked = true;
                if (Session["UserName"].ToString()=="Admin@bseb.com")
                {
                    ddldiv_location.Visible = true;
                    txt_location.Visible = false;
                }
                else
                {
                    ddldiv_location.Visible = false;
                    txt_location.Visible = true;
                }
            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    public void BindCollegedropdown()
    {
        DataTable dtstate = db.getcollegefordropdown();
        if (dtstate.Rows.Count > 0)
        {
            dtstate.Columns.Add("DisplayName", typeof(string));

            foreach (DataRow row in dtstate.Rows)
            {
                row["DisplayName"] = row["CollegeCode"].ToString() + " - " + row["CollegeName"].ToString();
            }

            ddl_location.DataSource = dtstate;
            ddl_location.DataTextField = "DisplayName"; 
                    ddl_location.DataValueField = "Id";
            ddl_location.DataBind();
            ddl_location.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Location", "-1"));
        }
        else
        {
            ddl_location.Items.Clear();
            ddl_location.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Location", "-1"));
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
            ddlFaculty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Faculty", "-1"));
        }
        else
        {
            ddlFaculty.Items.Clear();
            ddlFaculty.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Faculty", "-1"));
        }
    }

    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        string username = Session["UserName"].ToString();
       
        if (rdo_payemntstatus.Checked == true)
        {
            int collegeid = 0;
            if (username == "Admin@bseb.com")
            {

                collegeid = Convert.ToInt32(ddl_location.SelectedValue);

            }
            else
            {
                collegeid = Convert.ToInt32(Session["CollegeId"]);
            }
            DataTable result = db.GetStudentPaymentDetails(collegeid);
            rpt_getpayemnt.DataSource = result;
            rpt_getpayemnt.DataBind();
            divpayment.Visible = true;
            divstudentdetails.Visible = false;
        }
        else
        {
            string faculty = "";
            string CollegeName = "";
            if (username == "Admin@bseb.com")
            {

                CollegeName = ddl_location.SelectedItem.Text;
               
            }
            else
            {
                faculty = ddlFaculty.SelectedValue;
                string CollegeNameAndCode = txt_collegename.Text.Trim();
                string[] CollegeNameSplit = CollegeNameAndCode.Split('|');
                 CollegeName = CollegeNameSplit[0].Trim();
            }
           
            //string StudentName = txtStudentName.Text.Trim();

            DataTable result = db.getStudentData(CollegeName, "", faculty, ddl_category.SelectedValue);

            rptStudents.DataSource = result;
            rptStudents.DataBind();
            divpayment.Visible = false;
            divstudentdetails.Visible = true;
        }

    }


    protected void btn_paynow_Click(object sender, EventArgs e)
    {
        string collegeCode = Session["CollegeCode"].ToString();
        string username = Session["username"].ToString();

        string selectedStudentIds = "";
        long totalAmount = 0;

        foreach (RepeaterItem item in rptStudents.Items)
        {
            CheckBox chk = (CheckBox)item.FindControl("chkSelect");
            HiddenField hfStudentID = (HiddenField)item.FindControl("hfStudentID");
            Label lblFee = (Label)item.FindControl("lblFee");

            if (chk != null && chk.Checked)
            {
                selectedStudentIds += hfStudentID.Value + ",";
                totalAmount += Convert.ToInt64(lblFee.Text);
            }
        }

        // Remove trailing comma
        if (selectedStudentIds.EndsWith(","))
            selectedStudentIds = selectedStudentIds.TrimEnd(',');

        if (string.IsNullOrEmpty(selectedStudentIds))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select at least one student.');", true);
            return;
        }

       
        string clientTxnId = "BSEBREG" + DateTime.Now.ToString("yyyyMMddHHmmss");
        string payerName = collegeCode;  
        string payerMobile = "";  
        string payerEmail = username; 


        string message = db.InsertStudentPaymentDetails(Convert.ToInt32(Session["CollegeId"]), 1, ddl_paymode.SelectedValue, totalAmount, clientTxnId, selectedStudentIds);

        // Load config
        string paymentURL = ConfigurationManager.AppSettings["PaymentURL"];
        string clientCode = ConfigurationManager.AppSettings["Clientcode"];
        string userName = ConfigurationManager.AppSettings["UserName"];
        string password = ConfigurationManager.AppSettings["Password"];
        string authIV = ConfigurationManager.AppSettings["AuthenticationIV"];
        string authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
        string callbackURL = ConfigurationManager.AppSettings["callbackUrl"];

        
        string encData = "?clientName=" + clientCode +
                   "&usern=" + userName +
                   "&pass=" + password +
                   "&amt=" + totalAmount +
                   "&txnId=" + clientTxnId +
                   "&firstName=" + payerName +
                   "&lstName=" +
                   "&contactNo=" + payerMobile +
                   "&Email=" + payerEmail +
                   "&Add=ss" +
                   "&ru=" + callbackURL +
                   "&failureURL=" + callbackURL;

        // Encrypt query
        string encrypted = EncryptString(encData, authIV, authKey);
        encrypted = encrypted.Replace("+", "%2B");

        string finalURL = paymentURL + "?query=" + encrypted + "&clientName=" + clientCode + "&output=embed";

        // Redirect
        Response.Redirect(finalURL);

       
    }

    public static string EncryptString(string plainText, string iv, string key)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            ICryptoTransform encryptor = GetCryptoTransform(csp, true, iv, key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            return Convert.ToBase64String(encrypted);
        }
    }

    private static ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting, string iv, string key)
    {
        csp.Mode = CipherMode.CBC;
        csp.Padding = PaddingMode.PKCS7;
        csp.IV = Encoding.UTF8.GetBytes(iv);
        csp.Key = Encoding.UTF8.GetBytes(key);
        return encrypting ? csp.CreateEncryptor() : csp.CreateDecryptor();
    }

    public static string DecryptString(string encrypted, string AuthIV, string AuthKey)
    {
        using (var csp = new AesCryptoServiceProvider())
        {
            var decryptor = GetCryptoTransform(csp, false, AuthIV, AuthKey);
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);    
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);        
            string decrypted = Encoding.UTF8.GetString(decryptedBytes);
            return decrypted;
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
                string script = "alert('Payment record deleted successfully.'); window.location=window.location.href;";
                ClientScript.RegisterStartupScript(this.GetType(), "DeleteSuccess", script, true);
            }
            else
            {
                string script = "alert('Payment record not found or deletion failed.');";
                ClientScript.RegisterStartupScript(this.GetType(), "DeleteFail", script, true);
            }
        }
    }

    protected void rpt_getpayemnt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            // Check user's role from Session or any auth source
            string Username = Session["UserName"] as string;
            if (Username != "Admin@bseb.com")
            {
                lnkDelete.Visible = false; 
            }
        }
    }
}