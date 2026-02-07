using System;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            bool showPopup = false;

            // Check Admin
            bool isAdmin = Session["CollegeName"] != null && Session["CollegeName"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
                // Not Admin → check profile completion
                if (Session["IsProfileCompleted"] != null)
                {
                    int isProfileCompleted;
                    if (int.TryParse(Session["IsProfileCompleted"].ToString(), out isProfileCompleted))
                    {
                        if (isProfileCompleted == 0)
                        {
                            showPopup = true;
                        }
                    }
                }
            }

            // Important: FindControl because it's in Master
            hfShowCustomOverlay.Value = showPopup ? "1" : "0";

            if (Session["CollegeId"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
            {
                // Admin menus


                //// Hide college menus for Admin


                //li_ExaminationForm.Visible = true;
                li_ExamDwnld.Visible = true;
                li_PayExamFormFee.Visible = true;
                li_collegemster.Visible = true;
                li_studentsatus.Visible = true;
                li_scheduler.Visible = true;
                liExmchallanrecall.Visible = true;
                li_CORRECTIONDUMMYREPORT.Visible = true;
                liRegisteredList.Visible = true;
                li_ExaminationForm.Visible = true;
                li_ExamDwnld.Visible = true;
                //li_DownloadPracticaladmitcard.Visible = true;
                //liExmchallanrecall.Visible = true;
                //li_Downloadadmitcard.Visible = true;
                //li_DownloadPracticaladmitcard.Visible = true;
                //li_Theoryadmitcard.Visible = true;
                //liExmchallanrecall.Visible = true;

            }
            else
            {
                // College menus

                li_ExaminationForm.Visible = true;
                li_ExamDwnld.Visible = true;
                li_PayExamFormFee.Visible = true;
                liRegisteredList.Visible = true;
                //liExmchallanrecall.Visible = true;
                //li_Downloadadmitcard.Visible = true;
                //li_DownloadPracticaladmitcard.Visible = true;
                //li_Theoryadmitcard.Visible = true;
                // Hide admin menus for college
                //li_seatmatrix.Visible = false;
                //li_collegemster.Visible = false;
                //lichallanrecall.Visible = false;
            }
            // Check if user is logged in


        }
        //string activeModule = "";
        //if (Session["ActiveModule"] != null)
        //{
        //    activeModule = Session["ActiveModule"].ToString().ToLower();
        //}
        //else
        //{
        //    activeModule = "registration";
        //    Session["ActiveModule"] = activeModule; 
        //}

        //ApplyModuleVisibility(activeModule);
        // Admin check

    }

}

//private void SetAllModuleLinksVisibility(bool visible)
//{
//    // Student Registration Module
//    li_dwnloadreg.Visible = visible;
//    li_payment.Visible = visible;
//    li_regular.Visible = visible;
//    li_privatestd.Visible = visible;
//    //lidummyregcard.Visible = visible;
//    liRegisteredList.Visible = visible;
//    li_PayExamFormFee.Visible = visible;
//    li_ManageDeclarationForm.Visible=visible;

//    // Pre Examination Module
//    li_ExamDwnld.Visible = visible;
//    li_ExaminationForm.Visible = visible;
//    li_ExaminationForm.Visible = visible;
//    li_seatmatrix.Visible = visible;
//    li_collegemster.Visible = visible;
//    lichallanrecall.Visible = visible;
//    li_DownloadPracticaladmitcard.Visible = visible;
//    li_Downloadadmitcard.Visible = visible;
//    li_Theoryadmitcard.Visible = visible;
//    liExmchallanrecall.Visible = visible;
//}
//private void ApplyModuleVisibility(string activeModule)
//{

//    SetAllModuleLinksVisibility(false);


//    switch (activeModule)
//    {
//        case "registration":
//            li_dwnloadreg.Visible = true;
//            li_payment.Visible = true;
//            li_regular.Visible = true;
//            li_privatestd.Visible = true;
//            liRegisteredList.Visible = true;
//            //lidummyregcard.Visible = true;
//            li_ManageDeclarationForm.Visible = true;
//            if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
//            {
//                li_seatmatrix.Visible = true;
//                li_collegemster.Visible = true;
//                lichallanrecall.Visible = true;
//            }
//            break;
//        case "preexam":
//            if (Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin")
//            {
//                liExmchallanrecall.Visible = true;
//            }
//            li_ExamDwnld.Visible = true;
//            li_PayExamFormFee.Visible = true;
//            li_ExaminationForm.Visible = true;
//            li_Downloadadmitcard.Visible = true;
//            li_DownloadPracticaladmitcard.Visible = true;
//            li_Theoryadmitcard.Visible = true;

//            break;
//        default:

//            break;
//    }
//}

