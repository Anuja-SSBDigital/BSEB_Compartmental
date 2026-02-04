<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="registerPrivate.aspx.cs" Inherits="registerPrivate" %>

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
            font-weight: 700 !important;
        }

        .custom-line {
            margin: 3px 0; /* reduce space between lines */
        }


        marquee {
            color: #2f8dd3;
            font-weight: bold;
            margin: 10px 0 15px;
        }

            marquee a {
                color: #003399;
                text-decoration: underline;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <marquee behavior="scroll" direction="left" scrollamount="3">
            <span style="color: red;">
                <i class="fa fa-phone fa-flip-horizontal"></i>Bihar School Examination Board Helpline : 0612-2230039
            </span>
            &nbsp;&nbsp;&nbsp;
    <span style="color: blue;">For any query related to Student's Registration please contact at
        <a href="mailto:bsebinterhelpdesk@gmail.com">
            <i class="fa fa-envelope"></i>bsebinterhelpdesk@gmail.com
        </a>
    </span>
        </marquee>
    </div>
    <div class="card">
        <div class="card-header">
            <h4>Register Private Student</h4>

        </div>
        <div class="card-body">

            <div class="form-horizontal">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <div class="form-group">

                            <label for="location" class="col-form-label">+2 School/College Code & Name<span class="text-danger">*</span></label>
                            <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                            <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                            <asp:HiddenField ID="hfCollegeId" runat="server" />

                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="faculty" class="col-form-label">Faculty<span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                            </asp:DropDownList>
                            <span id="facultyError" class="text-danger" style="display: none;">Please select a faculty.</span>
                        </div>
                    </div>

                    <div class="col-md-6 mt-3">
                        <div class="form-group mb-3">
                            <label class="form-label d-block">Registration Mode</label>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="regMode" id="nonOfss" value="non-ofss">
                                <label class="form-check-label" for="nonOfss"><b>NON-OFSS DATA</b></label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="regMode" id="displayRegistered" value="display-registered">
                                <%-- Changed ID and value --%>
                                <label class="form-check-label" for="displayRegistered"><b>REGISTERED LIST</b></label>
                                <%-- Changed for attribute --%>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:Button ID="btnAddStudentReg" runat="server" CssClass="btn btn-success" Text="Add New Student" OnClick="btnAddStudentReg_Click" />

                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 mt-3">
                        <div class="form-group mb-3">
                            <label class="col-form-label">Search Student by Name</label>
                            <%--<input type="text" class="form-control" placeholder="Enter Name"  ID="txtStudentName">--%>
                            <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="form-group mb-3" runat="server" visible="false">
                    <label class="form-label d-block">Registration Type</label>
                    <div class="form-check form-check-inline">
                        <input class="form-check-input" type="radio" name="regType" id="private" value="Private" checked="checked">
                        <label class="form-check-label" for="private">PRIVATE</label>
                    </div>
                </div>

                <div class="form-row mb-3">
                    <div class="form-group col-md-6">
                        <label>Available Seat in Regular</label>
                        <asp:TextBox runat="server" ID="txt_regularseats" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                    </div>
                    <div class="form-group col-md-6">
                        <label>Available Seat in Private</label>
                        <asp:TextBox runat="server" ID="txt_privateseats" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                    </div>
                </div>

                <div class="form-group mt-4 text-center">

                    <asp:Button ID="btnViewRecord" runat="server" CssClass="btn btn-primary" Text="View Record" OnClick="btnViewRecord_Click" OnClientClick="return validateFaculty();" />
                </div>
            </div>
            <!-- Result Summary -->
            <div class="info-box mt-4 text-center">
                <p class="text-dark boldfont custom-line">
                    Total No. of Student Payment Done: <strong>
                        <asp:Label runat="server" ID="lbl_totalpayment"></asp:Label>
                    </strong>
                </p>
                <p class="text-success boldfont custom-line">
                    Total No. of Student Form Submitted: <strong>
                        <asp:Label runat="server" ID="lbl_totalformsubmitted"></asp:Label>
                    </strong>
                </p>
                <p class="text-danger boldfont custom-line">
                    Payment done but Form not Submitted: <strong>
                        <asp:Label runat="server" ID="lbl_pymntdonefrmntsubmitd"></asp:Label>
                    </strong>
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
                                <th>Student Name</th>
                                <th>Father Name</th>
                                <th>Mother Name</th>
                                <th>Year Of Passing</th>
                                <th>Click to Register</th>
                                <th>View</th>
                                <th>Correction</th>
                                <th>Delete</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptStudentList" runat="server" OnItemDataBound="rptStudentList_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td><%# Eval("CollegeCode") %></td>
                                        <td><%# Eval("StudentName") %></td>
                                        <td><%# Eval("FatherName") %></td>
                                        <td><%# Eval("MotherName") %></td>
                                        <td><%# Eval("YearOfPassing") %></td>
                                        <td>
                                            <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-sm btn-info" Text="Register" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnRegister_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnView" runat="server" CssClass="btn btn-sm btn-primary" Text="View" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnView_Click" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btn_correction" Visible="false"
                                                runat="server"
                                                CssClass="btn btn-sm btn-primary btn-square"
                                                CommandArgument='<%# Eval("StudentId") %>'
                                                OnClick="btn_correction_Click"
                                                ToolTip="Edit">
<i class="fa fa-edit text-white"></i>
                                            </asp:LinkButton>


                                        </td>
                                        <td>
                                            <%--  <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" Text="Delete" CommandArgument='<%# Eval("StudentId") %>' OnClick="btnDelete_Click" />--%>
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
            debugger;

            var facultyDropdown = document.getElementById('<%= ddlFaculty.ClientID %>');
            var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
            var facultyErrorSpan = document.getElementById('facultyError');
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
                facultyErrorSpan.style.display = "inline";
                facultyDropdown.classList.add("is-invalid");
                facultyDropdown.focus();
                return false;
            } else {
                facultyErrorSpan.style.display = "none";
                facultyDropdown.classList.remove("is-invalid");
            }

            return true;
        }


        window.onload = function () {
            document.getElementById("nonOfss").checked = true;
            handleRegModeChange();

            var regModeRadios = document.getElementsByName("regMode");
            for (var i = 0; i < regModeRadios.length; i++) {
                regModeRadios[i].addEventListener("change", handleRegModeChange);
            }
        };
        //window.onload = function () {
        //    document.getElementById("nonOfss").checked = true;
        //    handleRegModeChange();

        //    var regModeRadios = document.getElementsByName("regMode");
        //    for (var i = 0; i < regModeRadios.length; i++) {
        //        regModeRadios[i].addEventListener("change", handleRegModeChange);
        //    }
        //};

    </script>

</asp:Content>
