<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExamFormStatus.aspx.cs" Inherits="ExamFormStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table-responsive {
            margin-top: 20px;
        }

        .repeater-checkbox {
            width: 30px;
            text-align: center;
        }

        .repeater-col {
            padding: 8px;
        }

        table {
            border-collapse: collapse !important; /* Ensures no double borders */
            width: 100%;
        }

            table th,
            table td {
                border: 1px solid #333 !important; /* Darker and consistent grid border */
                padding: 10px;
                font-size: 14px;
                vertical-align: middle;
            }

            table tr:nth-child(even) {
                background-color: #f9f9f9;
            }

        .repeater-checkbox {
            text-align: center;
            width: 40px;
        }

        label.form-label.required-label {
            font-weight: 600 !important;
        }

        .boldfont {
            font-weight: 700 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Get Exam Form Status</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>+2 School Code & Name<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hfCollegeId" />
                                <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                               
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="faculty" class="form-label">Faculty<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                <span id="facultyError" class="text-danger" style="display: none;">Please select a faculty.</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Status<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control select2">
                                    <asp:ListItem Value="ALL">Select Status</asp:ListItem>
                                    <asp:ListItem Value="Challan Generated">Challan Generated – Fee Pending</asp:ListItem>
                                    <asp:ListItem Value="Fee Paid">Fee Paid – Form Not Filled</asp:ListItem>
                                    <asp:ListItem Value="Form Filled">Form Filled</asp:ListItem>

                                </asp:DropDownList>
                                <span id="StatusError" class="text-danger" style="display: none;">Please select a Status.</span>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4 text-center">
                        <asp:Button ID="btnviewrecord" runat="server" Text="VIEW RECORD" OnClick="btnviewrecord_Click" OnClientClick="return validateFaculty();" CssClass="btn btn-primary mr-2" />
                    </div>

                    <hr />
                    <div class="table-responsive">
                        <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                            <table class="table table-hover table-bordered" id="table-1">
                                <thead>
                                    <tr>

                                        <th>Student Name</th>
                                        <th>Father Name</th>
                                        <th>Mother Name</th>
                                        <%--<th>Faculty</th>--%>
                                        <th>Faculty</th>
                                        <th>Registration No</th>
                                        <th>Transaction Id</th>
                                        <th>Payment Status</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptStudents">
                                        <ItemTemplate>
                                            <tr>


                                                <%--<asp:HiddenField ID="hfCollege" runat="server" Value='<%# Eval("CollegeId") %>' />--%>
                                                <td><%# Eval("StudentFullName") %></td>
                                                <td><%# Eval("FatherName") %></td>
                                                <td><%# Eval("MotherName") %></td>
                                                <td>
                                                    <%# Eval("FacultyName") %>
                                                      
                                                </td>
                                                <td> <%# Eval("RegistrationNo") %></td>
                                                <td> <%# Eval("ClientTxnId") %></td>
                                                <td> <%# Eval("PaymentStatus") %></td>

                                                <%--  <asp:HiddenField ID="hfexamtypid" runat="server" Value='<%# Eval("ExamTypeId") %>' />
                                             <td class="repeater-col"><%# Eval("Dob", "{0:yyyy-MM-dd}") %></td>--%>
                                            </tr>
                                        </ItemTemplate>

                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="pnlNoRecords" runat="server" Visible="false" CssClass="alert alert-danger text-center mt-3">
                            No student records found matching your criteria.
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function validateFaculty() {
           
            var facultyDropdown = document.getElementById('<%= ddlFaculty.ClientID %>');
            var statusDropdown = document.getElementById('<%= ddl_status.ClientID %>');
      var errorSpan = document.getElementById('facultyError');
            var statuserrorSpan = document.getElementById('StatusError');

              var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
              var collegeNameErrorSpan = document.getElementById('CollegeNameError');
              if (collegeNameInput.value.trim() === "") {
                  collegeNameErrorSpan.style.display = "inline";
                  collegeNameInput.classList.add("is-invalid");
                  collegeNameInput.focus();
                  return false;
              } else {
                  collegeNameErrorSpan.style.display = "none";
                  collegeNameInput.classList.remove("is-invalid");
              }

              if (facultyDropdown.value === "0" || facultyDropdown.value === "") {
                  errorSpan.style.display = "inline";

                  facultyDropdown.focus();
                  return false;
              } else {
                  errorSpan.style.display = "none";
                  facultyDropdown.classList.remove("is-invalid");
                 
            }
            if (statusDropdown.value == "ALL" || statusDropdown.value === "") {
                statuserrorSpan.style.display = "inline";

                statusDropdown.focus();
                return false;
            } else {
                statuserrorSpan.style.display = "none";
                statusDropdown.classList.remove("is-invalid");
             
            }
          }
    </script>
</asp:Content>
