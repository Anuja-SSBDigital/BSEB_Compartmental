<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CollegeMaster.aspx.cs" Inherits="CollegeMaster" %>

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

        .required::after {
            content: " *";
            color: red;
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

        .text-red {
            color: red;
            font-size: 0.9rem;
        }
    </style>
    <script type="text/javascript">
        function validateForm() {
            var collegeCode = document.getElementById('<%= txtcollegeCode.ClientID %>').value.trim();
            var errorLabel = document.getElementById('txtcollegeCodeErr');
            if (collegeCode === "") {
                errorLabel.style.display = 'inline';
                return false;
            } else {
                errorLabel.style.display = 'none';
                return true;
            }
        }
        //if (performance.navigation.type === 1) {
        //    // Browser refresh detected
        //    window.location.href = window.location.pathname + "?reset=true";
        //}
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>College Master</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="collegeCode" class="form-label required">+2 School/College Code & Name</label>
                                <asp:TextBox ID="txtcollegeCode" runat="server" CssClass="form-control"></asp:TextBox>
                                <span id="txtcollegeCodeErr" style="display: none; color: red;">Please Enter College Code</span>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4">
                        <asp:Button ID="Button1" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClientClick="return validateForm();" OnClick="btnGetCollegeMasterByCollegeCode" />
                    </div>

                    <hr />

                    <div class="table-responsive">
                        <asp:Panel ID="pnlcollegeCodeTable" runat="server" Visible="false">
                            <table class="table table-hover table-bordered" id="table-1">
                                <thead>
                                    <tr>
                                        <th>Sr. No</th>
                                        <th>+2 School/College Code</th>
                                        <th>+2 School/College Name</th>
                                        <th>User Name</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptcollegeCode">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="repeater-col"><%# Container.ItemIndex + 1 %></td>
                                                <td class="repeater-col"><%# Eval("CollegeCode") %></td>
                                                <td class="repeater-col"><%# Eval("Collegename") %></td>
                                                <td class="repeater-col"><%# Eval("UserName") %></td>
                                                <td class="repeater-col">
                                                    <a href='CollegeUserDetails.aspx?collegeCode=<%# Eval("CollegeCode") %>' class="btn btn-sm btn-primary">Edit</a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="table-light">
                                                <td class="repeater-col"><%# Container.ItemIndex + 1 %></td>
                                                <td class="repeater-col"><%# Eval("CollegeCode") %></td>
                                                <td class="repeater-col"><%# Eval("Collegename") %></td>
                                                <td class="repeater-col"><%# Eval("UserName") %></td>
                                                <td class="repeater-col">
                                                    <a href='CollegeUserDetails.aspx?collegeCode=<%# Eval("CollegeCode") %>' class="btn btn-sm btn-primary">Edit</a>
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
</asp:Content>
