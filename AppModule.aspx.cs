using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class AppModule : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AppModule));
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            string CollegeName = Session["CollegeName"].ToString();
            //lblCollegeName.Text = CollegeName;
        }
        else
        {
            Response.Redirect("Login.aspx");
            return;
        }
    }
    protected void lnkPreExamination_Click(object sender, EventArgs e)
    {
       
        //Session["ActiveModule"] = "preexam";
       
        Response.Redirect("DwnldCompartMentalExamForm.aspx"); 
    }

    protected void lnkStudentRegistration_Click(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            

            Response.Redirect("Dashboard.aspx");
        }
        else
        {
            Response.Redirect("Login.aspx");
            
        }
    }
}