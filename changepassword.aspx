<%@ Page Title="BSEB - Admin Dashboard | Change Password" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="changepassword.aspx.cs" Inherits="changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-container {
            width: 300px;
            margin: 50px auto;
        }

        .error {
            color: red;
        }

        .success {
            color: green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4 class="card-title mb-0">Change Password</h4>
                </div>
                <div class="card-body">

                    <div class="row">



                        <asp:Label ID="lblMessage" runat="server" CssClass="error d-block mb-3"></asp:Label>
                        <asp:Label ID="lblSuccess" runat="server" CssClass="success d-block mb-3"></asp:Label>

                        <div class="form-group col-md-4">
                            <label for="txtOldPassword" class="font-weight-bold">Old Password</label>
                            <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server"
                                ControlToValidate="txtOldPassword" ErrorMessage="Old password is required"
                                CssClass="error" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="txtNewPassword" class="font-weight-bold">New Password</label>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                                ControlToValidate="txtNewPassword" ErrorMessage="New password is required"
                                CssClass="error" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revNewPassword" runat="server"
                                ControlToValidate="txtNewPassword"
                                ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                                ErrorMessage="Password must be at least 8 characters with letters and numbers"
                                CssClass="error" Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="txtConfirmPassword" class="font-weight-bold">Confirm New Password</label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                                ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm password is required"
                                CssClass="error" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvConfirmPassword" runat="server"
                                ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Passwords do not match" CssClass="error" Display="Dynamic">
                            </asp:CompareValidator>
                        </div>

                      
                    </div>
                      <div class="form-group">
                            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password"
                                CssClass="btn btn-primary" />
                        </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>

