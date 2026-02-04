<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CollegeUserDetails.aspx.cs" Inherits="CollegeUserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table-responsive {
            margin-top: 20px;
        }

        .repeater-col {
            padding: 8px;
        }

        .required::after {
            content: " *";
            color: red;
        }

        .text-red {
            color: red;
            font-size: 0.9rem;
        }

        .is-invalid {
            border-color: red !important;
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

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>College User Details</h4>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="form-label required">College / +2 Code</label>
                                <asp:TextBox ID="txtcollegeCode" runat="server" CssClass="form-control" ReadOnly="true" />
                                <small id="txtcollegeCodeErr" class="text-danger d-none">Please Enter College Code</small>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="form-label required">College Name</label>
                                <asp:TextBox ID="txtcollegeName" runat="server" CssClass="form-control"/>
                          <small id="txtcollegeNameErr" class="text-danger" style="display:none;">Please Enter College Name</small>

                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="form-label required">User Name</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" ReadOnly="true" />
                                <small id="txtUserNameErr" class="text-danger d-none">Please Enter User Name</small>
                            </div>
                        </div>

                       <!-- Principal Name -->
                          <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label required">Principal Name</label>
                            <asp:TextBox ID="txtPrincipalName" runat="server" CssClass="form-control" />
                            <small id="txtPrincipalNameErr" class="text-danger" style="display: none;">Please Enter Principal Name</small>
                        </div>
                              </div>
                        <!-- Principal Mobile No -->
                          <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label required">Principal Mobile No</label>
                            <asp:TextBox ID="txtPrincipalConNo" runat="server" CssClass="form-control" MaxLength="10" oninput="enforceMaxLength(this, 10)" />
                            <span id="txtPrincipalConNoErr" class="text-danger" style="display: none;">Please Enter Principal Mobile No</span>
                        </div>
                        </div>
                        <!-- Email -->
                          <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label required">Email Address</label>
                            <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" TextMode="Email" />
                            <span id="txtEmailIdErr" class="text-danger" style="display: none;">Please Enter Email Address</span>
                        </div>
                              </div>

                    </div>

                    <div class="form-group mt-4">
                        <asp:HiddenField ID="hdnCollegeId" runat="server" />
                        <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnUpdateCollegeDetailsByCollegeCode" />
                        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary ml-2" OnClick="btnChangePassword_Click" />
                    </div>

                    <%-- Change Password Panel --%>
                    <asp:Panel ID="pnlChangePassword" runat="server" Visible="false" CssClass="mt-4 border p-3 rounded shadow-sm bg-light">
                        <h5>Change Password</h5>
                        <div class="row">
                            <div class="col-md-6">
                                <label class="form-label required">User Name</label>
                                <asp:TextBox ID="txtChangeUserName" runat="server" CssClass="form-control" ReadOnly="true" disabled="true" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label required">New Password</label>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" />
                                <span id="txtNewPasswordErr" class="text-red d-none"></span>
                            </div>
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnSubmitNewPassword" runat="server" Text="Submit Password" CssClass="btn btn-success"  OnClick="btnSubmitNewPassword_Click" />
                            <%--<asp:Button ID="btnSubmitNewPassword" runat="server" Text="Submit Password" CssClass="btn btn-success" OnClientClick="return validatePassword();" OnClick="btnSubmitNewPassword_Click" />--%>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

  <script type="text/javascript">
      function validateForm() {

          debugger
          var collegeName = document.getElementById('<%= txtcollegeName.ClientID %>');
          var collegeNameErr = document.getElementById('txtcollegeNameErr');
          if (!collegeName.value.trim()) {
              collegeNameErr.style.display = 'inline';
              collegeName.classList.add("is-invalid");
              collegeName.focus();
              return false;
          } else {
              collegeNameErr.style.display = 'none';
              collegeName.classList.remove("is-invalid");
          }
          // Principal Name Validation
          var nameInput = document.getElementById('<%= txtPrincipalName.ClientID %>');
        var nameErr = document.getElementById('txtPrincipalNameErr');
        if (!nameInput.value.trim()) {
            nameErr.style.display = 'inline';
            nameInput.classList.add("is-invalid");
            nameInput.focus();
            return false;
        } else {
            nameErr.style.display = 'none';
            nameInput.classList.remove("is-invalid");
        }

        // Mobile Number Validation
        var mobileField = document.getElementById('<%= txtPrincipalConNo.ClientID %>');
        var mobile = mobileField.value.trim();
        var mobileErrorSpan = document.getElementById('txtPrincipalConNoErr');
        var mobilePattern = /^[6-9]\d{9}$/;

        if (!mobile) {
            mobileErrorSpan.style.display = "inline";
            mobileErrorSpan.textContent = "Please enter Mobile Number.";
            mobileField.classList.add("is-invalid");
            mobileField.focus();
            return false;
        } else if (!mobilePattern.test(mobile)) {
            mobileErrorSpan.style.display = "inline";
            mobileErrorSpan.textContent = "Please enter a valid 10-digit Mobile Number starting with 6, 7, 8, or 9.";
            mobileField.classList.add("is-invalid");
            mobileField.focus();
            return false;
        } else {
            mobileErrorSpan.style.display = "none";
            mobileField.classList.remove("is-invalid");
        }

        // Email Validation
        var emailField = document.getElementById('<%= txtEmailId.ClientID %>');
        var email = emailField.value.trim();
        var emailErrorSpan = document.getElementById('txtEmailIdErr');
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        if (!email) {
            emailErrorSpan.style.display = "inline";
            emailErrorSpan.textContent = "Please enter Email.";
            emailField.classList.add("is-invalid");
            emailField.focus();
            return false;
        } else if (!emailPattern.test(email)) {
            emailErrorSpan.style.display = "inline";
            emailErrorSpan.textContent = "Please enter a valid Email.";
            emailField.classList.add("is-invalid");
            emailField.focus();
            return false;
        } else {
            emailErrorSpan.style.display = "none";
            emailField.classList.remove("is-invalid");
        }

        return true;
    }

  <%--  function validatePassword() {
        var passwordInput = document.getElementById('<%= txtNewPassword.ClientID %>');
          var passwordError = document.getElementById("txtNewPasswordErr");
          var password = passwordInput.value.trim();
         var passwordPattern = /^[A-Z0-9]{8}$/;

          if (!password) {
              passwordError.textContent = "Please enter password.";
              passwordError.style.display = "inline";
              passwordInput.classList.add("is-invalid");
              passwordInput.focus();
              return false;
          } else if (!passwordPattern.test(password)) {
              passwordError.textContent = "Password must be exactly 10 characters using only capital letters and numbers.";
              passwordError.style.display = "inline";
              passwordInput.classList.add("is-invalid");
              passwordInput.focus();
              return false;
          } else {
              passwordError.textContent = "";
              passwordError.style.display = "none";
              passwordInput.classList.remove("is-invalid");
          }

          return true;
      }--%>

      function enforceMaxLength(el, maxLength) {
          if (el.value.length > maxLength) {
              el.value = el.value.slice(0, maxLength);
          }
      }
  </script>


</asp:Content>
