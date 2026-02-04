<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentRegisteredList.aspx.cs" Inherits="StudentRegisteredList" MasterPageFile="~/MasterPage.master" %>

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

        .custom-line {
            margin: 3px 0; /* reduce space between lines */
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Registered Student List</h4>
                    <asp:HiddenField ID="hfCollegeId" runat="server" />

                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>+2 School/College Code & Name<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                                <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Faculty<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                                </asp:DropDownList>
                                <span id="facultyError" class="text-danger" style="display: none;">Please select a Faculty.</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Exam Category <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlExamcat" runat="server" CssClass="form-control form-select">
                                </asp:DropDownList>
                                <span id="ExamCatError" style="display: none; color: red;">Please select a Exam Category.</span>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4 text-center">
                        <asp:Button ID="btnviewrecord" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClick="btnviewrecord_Click" OnClientClick="return validateFaculty();" />
                    </div>
                    <hr />

                    <%-- <div class="text-center">
                                <strong><span id="SpSearchresult" runat="server" Visible="false">Search Result For: </span><asp:Label runat="server" ID="lblCollege" ></asp:Label></strong>
                            </div>

                            <hr />--%>


                    <div class="table-responsive" id="studentlistdata">
                        <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                            <div class="col-md-12 text-lg-right">
                                <asp:Button ID="DwnTransaction_PDF" runat="server" Text="Download PDF" CssClass="btn btn-primary mb-2" OnClick="DwnTransaction_PDF_Click" />
                            </div>
                            <%--<table class="table table-bordered table-striped table-sm" id="table-1">--%>
                            <table class="table table-hover table-bordered" id="table-1">
                                <thead>
                                    <tr>
                                        <th>S. No.</th>
                                        <th>+2 School/College Code</th>
                                        <th>Student Name</th>
                                        <th>Father Name</th>
                                        <th>Mother Name</th>
                                        <th>Year Of Passing</th>

                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="rptStudentList" runat="server">
                                        <itemtemplate>
                                            <tr>
                                                <td><%# Container.ItemIndex + 1 %></td>
                                                <td>
                                                    <asp:Label ID="lblCollegeCode" runat="server" Text='<%# Eval("CollegeCode") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFatherName" runat="server" Text='<%# Eval("FatherName") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMotherName" runat="server" Text='<%# Eval("MotherName") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblYearOfPassing" runat="server" Text='<%# Eval("YearOfPassing") %>'></asp:Label>
                                                </td>

                                            </tr>
                                        </itemtemplate>
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


    <script type="text/javascript">
        function validateFaculty() {
            debugger
            var ddlFaculty = document.getElementById('<%= ddlFaculty.ClientID %>');
            var facultyError = document.getElementById('facultyError');

            var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
            var collegeNameErrorSpan = document.getElementById('CollegeNameError');
            var CategoryInput = document.getElementById('<%= ddlExamcat.ClientID %>');
            var CategoryErrorSpan = document.getElementById('ExamCatError');
            if (collegeNameInput.value.trim() === "") {
                collegeNameErrorSpan.style.display = "inline";
                collegeNameInput.classList.add("is-invalid");
                collegeNameInput.focus();
                return false;
            } else {
                collegeNameErrorSpan.style.display = "none";
                collegeNameInput.classList.remove("is-invalid");
            }

            if (ddlFaculty.value === "0" || ddlFaculty.value === "") {
                facultyError.style.display = 'block';
                return false;
            } else {
                facultyError.style.display = 'none';
                //return true;
            }
            if (CategoryInput.value === "0" || CategoryInput.value === "") {
                CategoryErrorSpan.style.display = 'block';
                return false;
            } else {
                CategoryErrorSpan.style.display = 'none';
                //return true;
            }
        }


    </script>
</asp:Content>
