<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExamForm.aspx.cs" Inherits="ExamForm" %>

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
        

        .boldfont {
    font-weight: 800 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <div class="card-header">
            <h4>Examination Form</h4>
        </div>
        <div class="card-body">
            <div class="mt-4">
                <div class="form-horizontal">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="location" class="form-label">+2 School/College Code & Name <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                                <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                                <asp:HiddenField ID="hfCollegeId" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="faculty" class="form-label">Faculty<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                <span id="facultyError" class="text-danger" style="display: none;">Please select a faculty.</span>
                            </div>
                        </div>

                        <div class="col-md-6 mt-3">
                            <%--<div class="form-group">
                                <label for="category">Student Category<span class="text-danger">*</span></label>
                                <asp:DropDownList runat="server" ID="ddl_category" CssClass="form-control select2">
                                    <asp:ListItem Value="0">Select Category</asp:ListItem>
                                    <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                    <asp:ListItem Value="Private">Private</asp:ListItem>
                                </asp:DropDownList>
                                <span id="CatError" class="text-danger" style="display: none;">Please select Category.</span>
                            </div>--%>
                             <div class="form-group">
                                 <label>Exam Category <span class="text-danger">*</span></label>
                                 <asp:DropDownList ID="ddlExamcat" runat="server" CssClass="form-control select2">
                                 </asp:DropDownList>
                                 <span id="ExamCatError" class="text-danger" style="display: none;">Please select a Exam Category.</span>
                             </div>
                        </div>
                        <div class="col-md-6 mt-3">
                          <%--  <div class="form-group">
                                <label>Exam Category <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlExamcat" runat="server" CssClass="form-control select2">
                                </asp:DropDownList>
                                <span id="ExamCatError" class="text-danger" style="display: none;">Please select a Exam Category.</span>
                            </div>--%>
                             <div class="form-group mb-3">
                                 <label class="form-label">Search Student by Name</label>
                            
                                 <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name" class="form-control" />
                             </div>
                                                    </div>
                        <div class="col-md-6 mt-3">
                           <%-- <div class="form-group mb-3">
                                <label class="form-label">Search Student by Name</label>
                                <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name" class="form-control" />
                            </div>--%>
                        </div>
                    </div>
                    <div class="form-group mt-4 text-center">
                        <asp:Button ID="btnViewRecord" runat="server" CssClass="btn btn-primary" Text="View Record" OnClick="btnViewRecord_Click" OnClientClick="return validateFaculty();" />
                    </div>
                </div>
            </div>


            <!-- Result Summary -->
            <div class="info-box mt-4 text-center">
                <p class="text-dark boldfont" >
                    Total No. of Student Payment Done: <strong>
                        <asp:Label runat="server" ID="lbl_totalpayment"></asp:Label></strong>
                </p>
                <p class="text-success boldfont">
                    Total No. of Student Form Submitted: <strong>
                        <asp:Label runat="server" ID="lbl_totalformsubmitted"></asp:Label></strong>
                </p>
                <p class="text-danger boldfont">
                    Payment done but Form not Submitted: <strong>
                        <asp:Label runat="server" ID="lbl_pymntdonefrmntsubmitd"></asp:Label></strong>
                </p>
            </div>

            <div class="table-responsive" id="studentlistdata">
                <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                    <%--<table class="table table-bordered table-hover table-sm" id="table-1">--%>
                    <table class="table table-hover table-bordered" id="table-1">
                        <thead>
                            <tr>
                                <th>S. No.</th>
                                <th>+2 School/College Code</th>
                                <th>Registration Number</th>
                                <th>Student Name</th>
                                <th>Father Name</th>
                                <th>Mother Name</th>
								 <th>Edit</th>                             
                                <%--<th>Correction</th>--%>
                                 <th>Remarks</th>
                                <th>View</th>
                                <th>Status</th>
                               
                            </tr>
                        </thead>

                        <tbody>
                            <asp:Repeater ID="rptStudentList" runat="server" OnItemDataBound="rptStudentList_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td><%# Eval("CollegeCode") %></td>
                                        <td><%# Eval("RegistrationNo") %></td>
                                        <td><%# Eval("StudentName") %></td>
                                        <td><%# Eval("FatherName") %></td>
                                        <td><%# Eval("MotherName") %></td>
										<td><asp:Button ID="btnRegister" runat="server" CssClass="btn btn-sm btn-info" Text="Edit" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnEdit_Click" /></td>
                                       
                                        <%--<td><asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-info" Text="Correction" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnCorreaction_Click" /></td>--%>
                                         <asp:HiddenField ID="hfExamFeeSubmit" runat="server" Value='<%# Eval("IsExamFeeSubmit") %>' />
 <asp:HiddenField ID="hfExamFormSubmit" runat="server" Value='<%# Eval("IsExamFormSubmit") %>' />
 <asp:HiddenField ID="hfExamTypeid" runat="server" Value='<%# Eval("ExamTypeid") %>' />
   <td>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("CorrectionRemarks") %>'></asp:Label>
                                        </td>
                                       
                                            
                                        <td>
                                            <asp:Button ID="btnView" runat="server" CssClass="btn btn-sm btn-primary" Text="View" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnView_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="badge"></asp:Label>
                                        </td>
                                       
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

    <script type="text/javascript">
        function validateFaculty() {
            debugger
            var facultyDropdown = document.getElementById('<%= ddlFaculty.ClientID %>');
            var errorSpan = document.getElementById('facultyError');
            var examDropdown = document.getElementById('<%= ddlExamcat.ClientID %>');
            var examerror = document.getElementById('ExamCatError');
           <%-- var CatDropdown = document.getElementById('<%= ddl_category.ClientID %>');
            var CatError = document.getElementById('CatError');--%>
            var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
            var collegeNameErrorSpan = document.getElementById('CollegeNameError');


            collegeNameErrorSpan.style.display = "none";
            errorSpan.style.display = "none";
            examerror.style.display = "none";
            //CatError.style.display = "none";


            if (collegeNameInput.value.trim() === "") {
                collegeNameErrorSpan.style.display = "inline";
                collegeNameInput.classList.add("is-invalid");
                collegeNameInput.focus();
                return false;
            } else {
                collegeNameInput.classList.remove("is-invalid");
            }


            if (facultyDropdown.value === "0" || facultyDropdown.value === "") {
                errorSpan.style.display = "inline";
                facultyDropdown.classList.add("is-invalid");
                facultyDropdown.focus();
                return false;
            } else {
                facultyDropdown.classList.remove("is-invalid");
            }


            if (examDropdown.value === "0" || examDropdown.value === "") {
                examerror.style.display = "inline";
                examDropdown.classList.add("is-invalid");
                examDropdown.focus();
                return false;
            } else {
                examDropdown.classList.remove("is-invalid");
            }


            //if (CatDropdown.value === "0" || CatDropdown.value === "") {
            //    CatError.style.display = "inline";
            //    CatDropdown.classList.add("is-invalid");
            //    CatDropdown.focus();
            //    return false;
            //} else {
            //    CatDropdown.classList.remove("is-invalid");
            //}


            //return true;
        }


        window.onload = function () {
            var regTypeRadios = document.getElementsByName("regType");
            for (var i = 0; i < regTypeRadios.length; i++) {
                regTypeRadios[i].addEventListener("change", handleRegTypeChange);
            }
        };

    </script>

</asp:Content>
