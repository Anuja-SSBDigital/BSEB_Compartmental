using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class registerPrivate : System.Web.UI.Page
{
    DBHelper dl = new DBHelper();

    public void getcollegewiseseatsummary()
    {
        int CollegeId = 0;
        if (Session["CollegeName"].ToString() == "Admin")
        {
            DataTable dtres = dl.getcollegeidbasedonCollegecode(txt_CollegeName.Text);

            if (dtres.Rows.Count > 0)
            {
                CollegeId = Convert.ToInt32(dtres.Rows[0]["Pk_CollegeId"].ToString());


            }

        }
        else
        {
            CollegeId = Convert.ToInt32(hfCollegeId.Value);
        }
        DataTable dt = dl.GetCollegeWiseSeatSummary(CollegeId,Convert.ToInt32(ddlFaculty.SelectedValue));
        if (dt != null && dt.Rows.Count > 0)
        {

            txt_regularseats.Text = dt.Rows[0]["RemainingRegularSeats"].ToString();
            txt_privateseats.Text = dt.Rows[0]["RemainingPrivateSeats"].ToString();
            lbl_totalpayment.Text = dt.Rows[0]["TotalPaymentDone"].ToString();
            lbl_totalformsubmitted.Text = dt.Rows[0]["TotalFormSubmitted"].ToString();
            lbl_pymntdonefrmntsubmitd.Text = dt.Rows[0]["PaymentDoneFormNotSubmitted"].ToString();
        }
        else
        {
            txt_regularseats.Text = "0";
            txt_privateseats.Text = "0";
            lbl_totalpayment.Text = "0";
            lbl_totalformsubmitted.Text = "0";
            lbl_pymntdonefrmntsubmitd.Text = "0";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["CollegeId"] != null)
            {
                if (Session["CollegeName"].ToString() == "Admin")
                {
                    txt_CollegeName.Text = "";
                }
                else
                {
                    txt_CollegeName.Text = Session["CollegeCode"].ToString() + " | " + Session["CollegeName"].ToString();

                    txt_CollegeName.ReadOnly = true;
                    string CollegeId = Session["CollegeId"].ToString();
                    hfCollegeId.Value = CollegeId.ToString();
                }
               
                BindFacultydropdown();
               
            }
            pnlNoRecords.Visible = false;
            pnlStudentTable.Visible = false;

        }


    }

    public void BindFacultydropdown()
    {
        DataTable dtfaculty = dl.getPrivateRegFacultyfordropdown();
        if (dtfaculty.Rows.Count > 0)
        {
            ddlFaculty.DataSource = dtfaculty;
            ddlFaculty.DataTextField = "FacultyName";
            ddlFaculty.DataValueField = "Pk_FacultyId";
            ddlFaculty.DataBind();
            ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
        }
        else
        {
            ddlFaculty.Items.Clear();
            ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
        }
    }

    protected void btnViewRecord_Click(object sender, EventArgs e)
    {
        getcollegewiseseatsummary();
        string CollegeId = "";
        if (Session["CollegeName"].ToString() == "Admin")
        {
            DataTable dt = dl.getcollegeidbasedonCollegecode(txt_CollegeName.Text);

            if (dt.Rows.Count > 0)
            {
                CollegeId = dt.Rows[0]["Pk_CollegeId"].ToString();

            }

        }
        else
        {
            CollegeId = hfCollegeId.Value;
        }
        string RegistrationMode = Request.Form["regMode"];
        string CategoryType = "Private";

        string facultyId = ddlFaculty.SelectedValue;
        string StudentName = txtStudentName.Text.Trim();
        DataTable result = dl.GetStudentRegiListData(CollegeId, facultyId, RegistrationMode, CategoryType, StudentName);
        bool hasRecords = result != null && result.Rows.Count > 0;
        pnlNoRecords.Visible = !hasRecords;
        pnlStudentTable.Visible = hasRecords;

        rptStudentList.DataSource = hasRecords ? result : null;
        rptStudentList.DataBind();

    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            string studentId = ((Button)sender).CommandArgument;
            string categoryType ="Private";

            string url = "viewstudentregdetalis.aspx?studentId=" + Server.UrlEncode(studentId.ToString()) +
              "&CategoryType=" + Server.UrlEncode(categoryType) +
              "&from=registerPrivatePage";
            Response.Redirect(url, false);
            // DataTable result = dl.ViewStudentRegDetails(studentId, categoryType);
        }
        catch (Exception ex)
        {

            throw;
        }


    }



    protected void rptStudentList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btnRegister = (Button)e.Item.FindControl("btnRegister");
                Button btnView = (Button)e.Item.FindControl("btnView");

                DataRowView row = (DataRowView)e.Item.DataItem;

                string registrationMode = Request.Form["regMode"] != null ? Request.Form["regMode"].ToLower() : "";
                string categoryType = "Private";

                bool isRegFeeSubmit = false;
                bool isRegFormSubmit = false;

                if (row["IsRegFeeSubmit"] != DBNull.Value)
                {
                    isRegFeeSubmit = Convert.ToBoolean(row["IsRegFeeSubmit"]);
                }

                if (row["IsRegFormSubmit"] != DBNull.Value)
                {
                    isRegFormSubmit = Convert.ToBoolean(row["IsRegFormSubmit"]);
                }


                btnRegister.Visible = false;
                btnView.Visible = false;


                if (registrationMode == "non-ofss" && categoryType == "Private")
                {
                    if (isRegFeeSubmit && !isRegFormSubmit)
                    {

                        btnRegister.Visible = true;
                    }
                    else if (isRegFeeSubmit && isRegFormSubmit)
                    {

                        btnView.Visible = true;
                    }
                }

                else if (registrationMode == "display-registered")
                {
                    if (isRegFeeSubmit && isRegFormSubmit)
                    {
                        btnView.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }



    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string studentId = ((Button)sender).CommandArgument;
        string encodedStudentId = HttpUtility.UrlEncode(studentId);
        string registrationMode = Request.Form["regMode"];
        string categoryType = "Private";

            string url = "studentregform.aspx?studentId=" + Server.UrlEncode(studentId.ToString()) +
                            "&categoryType=" + Server.UrlEncode(categoryType);
            Response.Redirect(url, false);
    }



    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string studentId = ((Button)sender).CommandArgument;

    }

    protected void btnAddStudentReg_Click(object sender, EventArgs e)
    {
        string studentId = "0";
        string encodedStudentId = HttpUtility.UrlEncode(studentId);
        string registrationMode = Request.Form["regMode"];
        string categoryType = "Private";


        string url = "studentregform.aspx?categoryType=" + Server.UrlEncode(categoryType);
        Response.Redirect(url, false);
    }

    protected void btn_correction_Click(object sender, EventArgs e)
    {

        string studentId = ((LinkButton)sender).CommandArgument;
        string encodedStudentId = HttpUtility.UrlEncode(studentId);
        string correctionMode = "Correctionmode";
        string categoryType = "Private";


        string url = "studentregform.aspx?studentId=" + Server.UrlEncode(studentId.ToString()) +
                        "&categoryType=" + Server.UrlEncode(categoryType) + "&correctionMode=" + Server.UrlEncode(correctionMode);
        Response.Redirect(url, false);
    }


}

