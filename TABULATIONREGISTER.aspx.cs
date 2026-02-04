using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class TABULATIONREGISTER : System.Web.UI.Page
{
    protected int TotalCollegeCount { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        // Ensure data is bound only on the initial page load, not on postbacks
        if (!IsPostBack)
        {
            
        }
    }

    private void BindData()
    {
        // Retrieve connection string from Web.config
        string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_tabulationregister", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Set the faculty parameter for the stored procedure
                    cmd.Parameters.AddWithValue("@faculty", ddl_faculty.SelectedValue); // You can make this dynamic if needed

                    // Add output parameter for return value (if used by SP)
                    SqlParameter retValue = new SqlParameter("@retValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(retValue);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dtResult = new DataTable();
                        da.Fill(dtResult); // Fill DataTable with data from stored procedure

                        if (dtResult.Rows.Count > 0)
                        {
                            // Step 1: Group the entire result set by CollegeName
                            var groupedByCollege = dtResult.AsEnumerable()
                                .GroupBy(r => r.Field<string>("CollegeName"))
                                .Select(g => new
                                {
                                    CollegeName = g.Key, // The college name for this group
                                    // Assuming Faculty is consistent across all rows for a given college
                                    Faculty = g.First().Field<string>("Faculty"),
                                    // Step 2: Within each college group, group by student details
                                    Students = g.GroupBy(s => new
                                    {
                                        RollCode = s.Field<string>("RollCode"),
                                        RollNumber = s.Field<string>("RollNumber"),
                                        RegistrationNo = s.Field<string>("RegistrationNo"),
                                        FatherName = s.Field<string>("FatherName"),
                                        MotherName = s.Field<string>("MotherName"),
                                        StudentFullName = s.Field<string>("StudentFullName"),
                                        BSEB_Unique_Id = s.Field<string>("BSEB_Unique_Id"),
                                        Gender = s.Field<string>("Gender"),
                                        CategoryName = s.Field<string>("CategoryName"),
                                        CastCategoryName = s.Field<string>("CastCategoryName"),
                                        Result_Division = s.Field<string>("Result_Division"),
                                        AggregateMarks = s.Field<string>("AggregateMarks")
                                    })
                                    .Select(studentGroup => new
                                    {
                                        // Student's core information
                                        RollCode = studentGroup.Key.RollCode,
                                        RollNumber = studentGroup.Key.RollNumber,
                                        RegistrationNo = studentGroup.Key.RegistrationNo,
                                        FatherName = studentGroup.Key.FatherName,
                                        MotherName = studentGroup.Key.MotherName,
                                        StudentFullName = studentGroup.Key.StudentFullName,
                                        BSEB_Unique_Id = studentGroup.Key.BSEB_Unique_Id,
                                        Gender = studentGroup.Key.Gender,
                                        CategoryName = studentGroup.Key.CategoryName,
                                        CastCategoryName = studentGroup.Key.CastCategoryName,
                                        Result_Division = studentGroup.Key.Result_Division,
                                        AggregateMarks = studentGroup.Key.AggregateMarks,

                                        // Step 3: Filter and collect subjects for the current student
                                        // Compulsory Subjects (if you add a repeater for them in ASPX)
                                        CompulsorySubjects = studentGroup.Where(row => row.Field<string>("SubjectGroupName") == "1. अनिवार्य Compulsory")
                                            .Select(row => new
                                            {
                                                Subject = row.Field<string>("Subject"),
                                                FullMarks = row.IsNull("FullMarks") ? 0 : row.Field<int>("FullMarks"),
                                                Theory = row.IsNull("Theory") ? 0 : row.Field<int>("Theory"),
                                                PracticalMarks = row.IsNull("PracticalMarks") ? 0 : row.Field<int>("PracticalMarks"),
                                                Subject_Total = row.Field<string>("Subject_Total"),
                                                CCEMarks = row.IsNull("CCEMarks") ? 0 : row.Field<int>("CCEMarks"),
                                            }).ToList(),

                                        // Elective Subjects
                                        ElectiveSubjects = studentGroup.Where(row => row.Field<string>("SubjectGroupName") == "2. ऐच्छिक Elective")
                                            .Select(row => new
                                            {
                                                Subject = row.Field<string>("Subject"),
                                                FullMarks = row.IsNull("FullMarks") ? 0 : row.Field<int>("FullMarks"),
                                                Theory = row.IsNull("Theory") ? 0 : row.Field<int>("Theory"),
                                                PracticalMarks = row.IsNull("PracticalMarks") ? 0 : row.Field<int>("PracticalMarks"),
                                                Subject_Total = row.Field<string>("Subject_Total"),
                                                CCEMarks = row.IsNull("CCEMarks") ? 0 : row.Field<int>("CCEMarks"),
                                            }).ToList(),

                                        // Additional Subjects
                                        AdditionalSubjects = studentGroup.Where(row => row.Field<string>("SubjectGroupName") == "Additional subject group Vocational (100 marks)")
                                            .Select(row => new
                                            {
                                                Subject = row.Field<string>("Subject"),
                                                FullMarks = row.IsNull("FullMarks") ? 0 : row.Field<int>("FullMarks"),
                                                Theory = row.IsNull("Theory") ? 0 : row.Field<int>("Theory"),
                                                PracticalMarks = row.IsNull("PracticalMarks") ? 0 : row.Field<int>("PracticalMarks"),
                                                Subject_Total = row.Field<string>("Subject_Total"),
                                                CCEMarks = row.IsNull("CCEMarks") ? 0 : row.Field<int>("CCEMarks"),
                                            }).ToList(),

                                        // Vocational Subjects
                                        VocationalSubjects = studentGroup.Where(row => row.Field<string>("SubjectGroupName") == "3. अतिरिक्त Additional")
                                            .Select(row => new
                                            {
                                                Subject = row.Field<string>("Subject"),
                                                FullMarks = row.IsNull("FullMarks") ? 0 : row.Field<int>("FullMarks"),
                                                Theory = row.IsNull("Theory") ? 0 : row.Field<int>("Theory"),
                                                PracticalMarks = row.IsNull("PracticalMarks") ? 0 : row.Field<int>("PracticalMarks"),
                                                Subject_Total = row.Field<string>("Subject_Total"),
                                                CCEMarks = row.IsNull("CCEMarks") ? 0 : row.Field<int>("CCEMarks"),
                                            }).ToList()
                                    }).ToList()
                                }).ToList();
                            TotalCollegeCount = groupedByCollege.Count();
                            // Bind the outermost repeater (for colleges)
                            rpt_CollegeGrouped.DataSource = groupedByCollege;
                            rpt_CollegeGrouped.DataBind();
                        }
                        else
                        {
                            // Alert if no records are found
                            Response.Write("<script>alert('⚠ No records found for this student.');</script>");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Display server error message
            Response.Write("<script>alert('Server Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
        }
    }

    // Event handler for the outer Repeater (rpt_CollegeGrouped)
    protected void rpt_CollegeGrouped_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // Check if the current item is a data item (not header/footer)
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Get the current college data item (anonymous type with CollegeName, Faculty, Students)
            dynamic collegeDataItem = e.Item.DataItem;

            // Find the lbl_faculty label within this college's section and bind its text
            Label lblFaculty = (Label)e.Item.FindControl("lbl_faculty");
           
            if (lblFaculty != null)
            {
                lblFaculty.Text = collegeDataItem.Faculty;
            }
            Label lbl_count = (Label)e.Item.FindControl("lbl_count");
            if (lbl_count != null)
            {
                lbl_count.Text = (e.Item.ItemIndex + 1).ToString();
            }

            // Find the inner Repeater (rpt_grouped) which displays students for this college
            Repeater rptGroupedStudents = (Repeater)e.Item.FindControl("rpt_grouped");
            if (rptGroupedStudents != null)
            {
                // Bind the inner repeater to the 'Students' collection of the current college
                rptGroupedStudents.DataSource = collegeDataItem.Students;
                rptGroupedStudents.DataBind();
            }
        }
    }

    // Event handler for the inner Repeater (rpt_grouped) - for each student
    protected void rpt_grouped_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // Check if the current item is a data item
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Get the current student data item (anonymous type with student details and subject lists)
            dynamic studentDataItem = e.Item.DataItem;

            // Find and bind the Repeater for Elective Subjects
            Repeater rptElective = (Repeater)e.Item.FindControl("rpt_elective");
            if (rptElective != null)
            {
                rptElective.DataSource = studentDataItem.ElectiveSubjects;
                rptElective.DataBind();
            }

            // Find and bind the Repeater for Additional Subjects
            Repeater rptAdditional = (Repeater)e.Item.FindControl("rpt_additional");
            if (rptAdditional != null)
            {
                rptAdditional.DataSource = studentDataItem.AdditionalSubjects;
                rptAdditional.DataBind();
            }

            // Find and bind the Repeater for Vocational Subjects
            Repeater rptVocational = (Repeater)e.Item.FindControl("rpt_vocational");
            if (rptVocational != null)
            {
                rptVocational.DataSource = studentDataItem.VocationalSubjects;
                rptVocational.DataBind();
            }

           // Uncomment the following block if you add a repeater for Compulsory Subjects in your ASPX

            Repeater rptCompulsory = (Repeater)e.Item.FindControl("rpt_compulsory");
            if (rptCompulsory != null)
            {
                rptCompulsory.DataSource = studentDataItem.CompulsorySubjects;
                rptCompulsory.DataBind();
            }
        }
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}