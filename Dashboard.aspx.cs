using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;

public partial class Dashboard : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Dashboard));
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
    DBHelper dl = new DBHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            log.Info("Page_Load started.");

            if (Session["CollegeId"] != null)
            {
                log.Info("Session CollegeId found: " + Session["CollegeId"]);
                if (Session["CollegeName"].ToString() == "Admin")
                {
                    txt_Panel1CollegeName.Text = "";
                    btngetCount.Visible = true;
                    lblpanel1college.Visible = true;
                    txt_Panel1CollegeName.Visible = true;
                    
                    //txt_CollegeName.Text = "";
                    //btngetsummary.Visible = true;
                    //lblcollege.Visible = true;
                    //txt_CollegeName.Visible = true;
                }
                else
                {
                    log.Info("User is not Admin. Running GetCount() and BindSummary().");
                    GetCount();
                   // BindSummary();
                }

                  

            }
            else
            {
                log.Warn("Session CollegeId is null. Redirecting to Login.aspx.");
                Response.Redirect("Login.aspx");
            }
        }
       
    }
    //private void BindSummary()
    //{
    //    //int CollegeId = Convert.ToInt32(Session["CollegeId"]);
    //    string CollegeId = "";
    //    if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
    //    {
    //        DataTable dt = dl.getcollegeidbasedonCollegecode(txt_CollegeName.Text);

    //        if (dt.Rows.Count > 0)
    //        {
    //            CollegeId = dt.Rows[0]["Pk_CollegeId"].ToString();
    //        }
    //    }
    //    else if (Session["CollegeId"] != null)
    //    {
    //        CollegeId = Session["CollegeId"].ToString();
    //    }
    //    else
    //    {

    //        Response.Redirect("Login.aspx");
    //        return;
    //    }
    //    DataSet ds = dl.GetCollegeWiseSeatSummaryForInfo(Convert.ToInt32(CollegeId));

    //    if (ds.Tables.Count > 0)
    //    {
    //        rptRegulareSeatMatrix.DataSource = ds.Tables[0];
    //        rptRegulareSeatMatrix.DataBind();

    //        rptPrivateSeatMatrix.DataSource = ds.Tables[0];
    //        rptPrivateSeatMatrix.DataBind();
    //    }

    //    if (ds.Tables.Count > 1)
    //    {
    //        rptFeeSubmitted.DataSource = ds.Tables[1];
    //        rptFeeSubmitted.DataBind();
    //    }

    //    if (ds.Tables.Count > 2)
    //    {
    //        rptUnsuccessful.DataSource = ds.Tables[2];
    //        rptUnsuccessful.DataBind();
    //    }

    //    if (ds.Tables.Count > 3)
    //    {
    //        rptFormSubmitted.DataSource = ds.Tables[3];
    //        rptFormSubmitted.DataBind();
    //    }

    //}

    //protected void btngetsummary_Click(object sender, EventArgs e)
    //{
    //    BindSummary();
    //}


    private void GetCount()
    {
        string CollegeId = "";
        if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
        {
            DataTable dt = dl.getcollegeidbasedonCollegecode(txt_Panel1CollegeName.Text);

            if (dt.Rows.Count > 0)
            {
                CollegeId = dt.Rows[0]["Pk_CollegeId"].ToString();
            }
        }
        else if (Session["CollegeId"] != null)
        {
            CollegeId = Session["CollegeId"].ToString();
        }
        else
        {

            Response.Redirect("Login.aspx");
            return;
        }
        DataTable dtResult = dl.GetDeclarationAndPaymentStatusCount(Convert.ToInt32(CollegeId));
        if (dtResult != null && dtResult.Rows.Count > 0)
        {
            DataRow dr = dtResult.Rows[0];

            int paymentCount = Convert.ToInt32(dr["PaymentStatusCount"]);
            int declarationCount = Convert.ToInt32(dr["DeclarationFormSubmittedCount"]);
            int declarationNotSubmittedCount = Convert.ToInt32(dr["NotSubmittedCount"]);

            lblPaymentStatusCount.Text = paymentCount + " (" + NumberToHindiWords(paymentCount) + ")";
            lblDeclarationNotSubmittedCount.Text = declarationNotSubmittedCount + " (" + NumberToHindiWords(declarationNotSubmittedCount) + ")";
            lblDeclarationNotSubmittedCount2.Text = declarationNotSubmittedCount + " (" + NumberToHindiWords(declarationNotSubmittedCount) + ")";
            lblDeclarationNotSubmittedCount3.Text = declarationNotSubmittedCount + " (" + NumberToHindiWords(declarationNotSubmittedCount) + ")";
            lblDeclarationFormSubmittedCount.Text = declarationCount + " (" + NumberToHindiWords(declarationCount) + ")";
            lblDeclarationFormSubmittedCount1.Text = declarationCount + " (" + NumberToHindiWords(declarationCount) + ")";
        }
    }
    protected void btngetCount_Click(object sender, EventArgs e)
    {

        GetCount();
    }

    public static string NumberToHindiWords(int number)
    {
        string[] numbersTill99 = {
        "शून्य","एक","दो","तीन","चार","पाँच","छह","सात","आठ","नौ",
        "दस","ग्यारह","बारह","तेरह","चौदह","पंद्रह","सोलह","सत्रह","अठारह","उन्नीस",
        "बीस","इक्कीस","बाईस","तेईस","चौबीस","पच्चीस","छब्बीस","सत्ताईस","अट्ठाईस","उनतीस",
        "तीस","इकतीस","बत्तीस","तैंतीस","चौंतीस","पैंतीस","छत्तीस","सैंतीस","अड़तीस","उनतालीस",
        "चालीस","इकतालीस","बयालीस","तैंतालीस","चौंतालीस","पैंतालीस","छयालिस","सैंतालीस","अड़तालीस","उनचास",
        "पचास","इक्यावन","बावन","तिरेपन","चौवन","पचपन","छप्पन","सत्तावन","अट्ठावन","उनसठ",
        "साठ","इकसठ","बासठ","तिरेसठ","चौंसठ","पैंसठ","छियासठ","सड़सठ","अड़सठ","उनहत्तर",
        "सत्तर","इकहत्तर","बहत्तर","तिहत्तर","चौहत्तर","पचहत्तर","छिहत्तर","सतहत्तर","अठहत्तर","उन्यासी",
        "अस्सी","इक्यासी","बयासी","तिरासी","चौरासी","पचासी","छियासी","सत्तासी","अठासी","नवासी",
        "नब्बे","इक्यानवे","बानवे","तिरेानवे","चौरानवे","पचानवे","छियानवे","सत्तानवे","अट्ठानवे","निन्यानवे"
    };

        if (number < 100)
            return numbersTill99[number];

        if (number < 1000)
        {
            int h = number / 100;
            int r = number % 100;
            return numbersTill99[h] + " सौ" + (r > 0 ? " " + NumberToHindiWords(r) : "");
        }

        if (number < 100000)
        {
            int th = number / 1000;
            int r = number % 1000;
            return NumberToHindiWords(th) + " हजार" + (r > 0 ? " " + NumberToHindiWords(r) : "");
        }

        if (number < 10000000)
        {
            int l = number / 100000;
            int r = number % 100000;
            return NumberToHindiWords(l) + " लाख" + (r > 0 ? " " + NumberToHindiWords(r) : "");
        }

        // करोड़ तक
        int cr = number / 10000000;
        int rr = number % 10000000;
        return NumberToHindiWords(cr) + " करोड़" + (rr > 0 ? " " + NumberToHindiWords(rr) : "");
    }

}