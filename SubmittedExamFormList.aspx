<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SubmittedExamFormList.aspx.cs" Inherits="SubmittedExamFormList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Download Registration Form</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>+2 School Code & Name  <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                                <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                            </div>

                            <div class="form-group">
                                <label>Academic Session</label>
                                <asp:DropDownList runat="server" class="form-control" ID="dr_session">
                                    <asp:ListItem Selected>2024-2026</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Faculty Name <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                                </asp:DropDownList>
                                <span id="facultyError" class="text-danger" style="display: none;">Please select a Faculty.</span>
                            </div>
                            <div class="form-group">
                                <label>Student Name</label>
                                <asp:TextBox ID="txtStudentName" runat="server" CssClass="form-control" placeholder="Enter Student Name"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4 text-center">
                        <asp:Button ID="Button1" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClick="btnGetStudentData" OnClientClick="return validateFaculty();" />
                    </div>

                    <div class="text-center mt-3">
                        <asp:Button ID="btnDownloadPDF" runat="server" Text="Download PDF" CssClass="btn btn-success" OnClick="btnDownloadPDF_Click" Visible="false" />
                    </div>

                    <hr />

                    <div class="table-responsive">
                        <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                            <table class="table table-striped table-bordered" id="table-1">
                                <thead>
                                    <tr>
                                        <th class="repeater-checkbox">
                                            <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                                            Select All
                                        </th>
                                        <th>College</th>
                                        <th>Student Name</th>
                                        <th>Father Name</th>
                                        <th>Mother Name</th>
                                        <th>Faculty</th>
                                        <th>DOB</th>
                                        <th>Form Downloaded</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptStudents">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="repeater-checkbox">
                                                    <asp:CheckBox ID="chkRowSelect" runat="server" AutoPostBack="true" OnCheckedChanged="RowCheckbox_CheckedChanged" />
                                                    <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                                                </td>
                                                <td class="repeater-col">
                                                    <%# Eval("College") %>
                                                    <asp:HiddenField ID="hfCollege" runat="server" Value='<%# Eval("CollegeId") %>' />
                                                    <%-- Added here --%>
                                                </td>
                                                <td class="repeater-col"><%# Eval("StudentName") %></td>
                                                <td class="repeater-col"><%# Eval("FatherName") %></td>
                                                <td class="repeater-col"><%# Eval("MotherName") %></td>
                                                <td class="repeater-col">
                                                    <%# Eval("Faculty") %>
                                                    <asp:HiddenField ID="hfFaculty" runat="server" Value='<%# Eval("FacultyId") %>' />
                                                    <%-- Added here --%>
                                                </td>
                                                <td class="repeater-col"><%# Eval("Dob", "{0:yyyy-MM-dd}") %></td>
                                                <td class="repeater-col">
                                                    <%# Eval("FormDownloaded").ToString() == "True" || Eval("FormDownloaded").ToString().ToUpper() == "YES" ? "YES" : "NO" %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="table-light">
                                                <td class="repeater-checkbox">
                                                    <asp:CheckBox ID="chkRowSelect" runat="server" AutoPostBack="true" OnCheckedChanged="RowCheckbox_CheckedChanged" />
                                                    <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                                                </td>
                                                <td class="repeater-col">
                                                    <%# Eval("College") %>
                                                    <asp:HiddenField ID="hfCollege" runat="server" Value='<%# Eval("CollegeId") %>' />
                                                </td>
                                                <td class="repeater-col"><%# Eval("StudentName") %></td>
                                                <td class="repeater-col"><%# Eval("FatherName") %></td>
                                                <td class="repeater-col"><%# Eval("MotherName") %></td>
                                                <td class="repeater-col">
                                                    <%# Eval("Faculty") %>
                                                    <asp:HiddenField ID="hfFaculty" runat="server" Value='<%# Eval("FacultyId") %>' />
                                                </td>
                                                <td class="repeater-col"><%# Eval("Dob", "{0:yyyy-MM-dd}") %></td>
                                                <td class="repeater-col">
                                                    <%# Eval("FormDownloaded").ToString() == "True" || Eval("FormDownloaded").ToString().ToUpper() == "YES" ? "YES" : "NO" %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
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
            var facultyDropdown = document.getElementById('<%= ddlFaculty.ClientID %>');
               var errorSpan = document.getElementById('facultyError');

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
                return true;
            }
        }
    </script>
</asp:Content>

