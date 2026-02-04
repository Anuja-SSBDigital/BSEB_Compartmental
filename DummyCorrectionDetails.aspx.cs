using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class DummyCorrectionDetails : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
   if (Session["CollegeId"] != null)
   {

   }
   else
   {
       Response.Redirect("Login.aspx");
   }
    }

    protected void ViewCorrectiondata_Click(object sender, EventArgs e)
    {
        DataSet ds = dl.GetChangesAndDummyDownloads(ddlDummyCorrectionDetailstype.SelectedValue);

        if (ddlDummyCorrectionDetailstype.SelectedValue == "CORRECTION")
        {
            rptCorrection.DataSource = ds;   // Correction changes table
            rptCorrection.DataBind();
            rptDownload.Visible = false;
            rptCorrection.Visible = true;
            rptPracticalAdmitCard.Visible = false;
            rptTheoryAdmitCard.Visible = false;



        }
        else if (ddlDummyCorrectionDetailstype.SelectedValue == "DOWNLOAD")
        {
            rptDownload.DataSource = ds;   // Dummy download table
            rptDownload.DataBind();
            rptCorrection.Visible = false;
            rptDownload.Visible = true;
            rptPracticalAdmitCard.Visible = false;
            rptTheoryAdmitCard.Visible = false;


        }
        else if (ddlDummyCorrectionDetailstype.SelectedValue == "PRACTICALADMITCARD")
        {
            rptPracticalAdmitCard.DataSource = ds;
            rptPracticalAdmitCard.DataBind();
            rptCorrection.Visible = false;
            rptDownload.Visible = false;
            rptPracticalAdmitCard.Visible = true;
            rptTheoryAdmitCard.Visible = false;
        }
        else if (ddlDummyCorrectionDetailstype.SelectedValue == "THEORYADMITCARD")
        {
            rptTheoryAdmitCard.DataSource = ds;
            rptTheoryAdmitCard.DataBind();
            rptCorrection.Visible = false;
            rptDownload.Visible = false;
            rptPracticalAdmitCard.Visible = false;
            rptTheoryAdmitCard.Visible = true;
        }
    }
}
