using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeatMatrix : System.Web.UI.Page
{
    DBHelper db = new DBHelper();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] != null)
        {
            if (!IsPostBack)
            {

                //BindFacultydropdown();

            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    public void GetSeatMatrixdata()
    {

        DataTable dt = db.GetSeatMatrixData(txt_collegecode.Text.Trim());

        if (dt.Rows.Count > 0)
        {
            // Add IsEdit column to support row editing
            if (!dt.Columns.Contains("IsEdit"))
                dt.Columns.Add("IsEdit", typeof(bool));

            foreach (DataRow row in dt.Rows)
                row["IsEdit"] = false;

            // Store in ViewState to use during Edit/Update
            ViewState["SeatMatrixData"] = dt;

            rpt_seatmatrix.DataSource = dt;
            rpt_seatmatrix.DataBind();
        }
        else
        {
            rpt_seatmatrix.DataSource = null;
            rpt_seatmatrix.DataBind();
        }

    }

    //public void BindFacultydropdown()
    //{
    //    DataTable dtfaculty = db.getFacultyfordropdown();
    //    if (dtfaculty.Rows.Count > 0)
    //    {
    //        ddlFaculty.DataSource = dtfaculty;
    //        ddlFaculty.DataTextField = "FacultyName";
    //        ddlFaculty.DataValueField = "Pk_FacultyId";
    //        ddlFaculty.DataBind();
    //        ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
    //    }
    //    else
    //    {
    //        ddlFaculty.Items.Clear();
    //        ddlFaculty.Items.Insert(0, new ListItem("Select Faculty", "0"));
    //    }
    //}



    protected void btn_submit_Click(object sender, EventArgs e)
    {
        GetSeatMatrixdata();
    }

    protected void rpt_seatmatrix_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int seatMatrixId = Convert.ToInt32(e.CommandArgument);

        // Fetch data from ViewState
        DataTable dt = ViewState["SeatMatrixData"] as DataTable;

        if (e.CommandName == "Edit")
        {
            // Set IsEdit = true for selected row, false for others
            foreach (DataRow row in dt.Rows)
            {
                row["IsEdit"] = (Convert.ToInt32(row["Pk_SeatMatrixId"]) == seatMatrixId);
            }

            ViewState["SeatMatrixData"] = dt;
            rpt_seatmatrix.DataSource = dt;
            rpt_seatmatrix.DataBind();
        }
        else if (e.CommandName == "Cancel")
        {
            // Reset all IsEdit flags to false
            foreach (DataRow row in dt.Rows)
            {
                row["IsEdit"] = false;
            }

            ViewState["SeatMatrixData"] = dt;
            rpt_seatmatrix.DataSource = dt;
            rpt_seatmatrix.DataBind();
        }
        else if (e.CommandName == "Update")
        {
            try
            {
                // Get new values from textboxes
                TextBox txtRegularSeats = (TextBox)e.Item.FindControl("txtRegularSeats");
                TextBox txtPrivateSeats = (TextBox)e.Item.FindControl("txtPrivateSeats");

                int regularSeats = int.Parse(txtRegularSeats.Text.Trim());
                int privateSeats = int.Parse(txtPrivateSeats.Text.Trim());

                // Update DB
                db.UpdateSeatMatrix(seatMatrixId, regularSeats, privateSeats); // You must have this method

                // Refresh the latest data after update
                DataTable updatedDt = db.GetSeatMatrixData(txt_collegecode.Text.Trim());

                // Re-add IsEdit column
                if (!updatedDt.Columns.Contains("IsEdit"))
                    updatedDt.Columns.Add("IsEdit", typeof(bool));

                foreach (DataRow row in updatedDt.Rows)
                    row["IsEdit"] = false;

                ViewState["SeatMatrixData"] = updatedDt;

                rpt_seatmatrix.DataSource = updatedDt;
                rpt_seatmatrix.DataBind();
            }
            catch (Exception ex)
            {

                string script = @"
    swal({
        title: 'Update failed',
         text: '" + ex.Message.Replace("'", "\\'") + @"',
        icon: 'error',
        button: 'Retry'
    });";
                ScriptManager.RegisterStartupScript(this, GetType(), "PaymentFailedBank", script, true);

                // Handle error (optional: show alert)
                //Response.Write("<script>alert('Update failed: " + ex.Message + "');</script>");
            }
        }
    }
}