<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CollegeProfile.aspx.cs" Inherits="CollegeProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');

        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body { font-family: 'Poppins', sans-serif; background: #f0f4ff; }

/*        .top-bar { width: 100%; height: 6px; background: linear-gradient(90deg, #4f6ef7, #6c8fff); }*/

        .card {
            background: #fff;
            border-radius: 16px;
            box-shadow: 0 4px 30px rgba(79,110,247,.10);
            padding: 40px 48px 48px;
            width: 100%;
            max-width: 1160px;
            margin: 36px auto 48px;
        }

        .card-title {
            font-size: 1.35rem;
            font-weight: 600;
            color: #2d3a6b;
            margin-bottom: 32px;
            padding-bottom: 14px;
            border-bottom: 2px solid #e8edff;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .card-title::before {
            content: '';
            display: inline-block;
            width: 4px;
            height: 22px;
            background: linear-gradient(180deg,#4f6ef7,#6c8fff);
            border-radius: 4px;
        }

        .alert {
            padding: 14px 20px;
            border-radius: 10px;
            font-size: .875rem;
            margin-bottom: 24px;
            display: none;
            align-items: center;
            gap: 10px;
        }
        .alert.success { background: #eafaf1; color: #1e7e45; border-left: 4px solid #27ae60; }
        .alert.error   { background: #fff0f0; color: #c0392b; border-left: 4px solid #e74c3c; }

        .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 24px 32px; }
        .full-width { grid-column: 1 / -1; }
        .field { display: flex; flex-direction: column; gap: 6px; }

        .field label { font-size: .78rem; font-weight: 500; color: #4f6ef7; letter-spacing: .02em; }
        .field label .req { color: #e74c3c; margin-left: 2px; }

        .field input, .field textarea {
            width: 100%;
            padding: 13px 16px;
            border: 1.5px solid #dde4f8;
            border-radius: 10px;
            font-family: 'Poppins', sans-serif;
            font-size: .9rem;
            color: #2d3a6b;
            background: #f7f9ff;
            outline: none;
            transition: border-color .2s, box-shadow .2s, background .2s;
        }

        .field input:focus, .field textarea:focus {
            border-color: #4f6ef7;
            background: #fff;
            box-shadow: 0 0 0 3px rgba(79,110,247,.12);
        }

        .field input:disabled {
            background: #edf0fa;
            color: #8a94bb;
            cursor: not-allowed;
            border-color: #e0e5f5;
        }

        .field textarea { resize: vertical; min-height: 100px; }

        .field input.invalid, .field textarea.invalid {
            border-color: #e74c3c;
            background: #fff8f8;
        }

        .field-error { font-size: .73rem; color: #e74c3c; min-height: 16px; margin-top: 2px; }

        .btn-row { margin-top: 36px; display: flex; justify-content: flex-end; gap: 14px; }

        .btn {
            padding: 13px 34px;
            border: none;
            border-radius: 10px;
            font-family: 'Poppins', sans-serif;
            font-size: .92rem;
            font-weight: 600;
            cursor: pointer;
            transition: transform .15s, box-shadow .15s;
        }

        .btn:active { transform: scale(.97); }

        .btn-primary 
        {
            background: linear-gradient(135deg, #4f6ef7, #6c8fff);
            color: #fff;
            box-shadow: 0 4px 18px rgba(79,110,247,.35);
        }

        .btn-primary:hover { box-shadow: 0 6px 24px rgba(79,110,247,.45); }

        .btn-secondary { background: #f0f3ff; color: #4f6ef7; border: 1.5px solid #c5d0f7; }
        .btn-secondary:hover { background: #e4eaff; }

        @media (max-width: 700px) {
            .grid { grid-template-columns: 1fr; }
            .full-width { grid-column: 1; }
            .card { padding: 28px 20px 36px; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <%--   <div class="top-bar"></div>--%>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <div class="card">
        <div class="card-title">College Profile</div>

        <div class="alert success" id="msgSuccess">
            &#10003; Profile updated successfully.
        </div>
        <div class="alert error" id="msgError">
            &#9888; Please fix the errors below before saving.
        </div>

        <div class="grid">

            <div class="field">
                <label>College Code</label>
                <input type="text" id="cstCollegeCode" runat="server" disabled="disabled" />
            </div>

            <div class="field">
                <label>Principal Mobile Number <span class="req">*</span></label>
                <input type="text" id="cstPrincipalMobile" runat="server" maxlength="10" placeholder="10-digit mobile number" />
                <div class="field-error" id="errMobile"></div>
            </div>

            <div class="field">
                <label>College Name</label>
                <input type="text" id="cstCollegeName" runat="server" disabled="disabled" />
            </div>

            <div class="field">
                <label>Principal Email ID <span class="req">*</span></label>
                <input type="text" id="cstPrincipalEmail" runat="server" placeholder="example@domain.com" />
                <div class="field-error" id="errEmail"></div>
            </div>

            <div class="field">
                <label>District Name</label>
                <input type="text" id="cstDistrictName" runat="server" disabled="disabled" />
            </div>

            <div class="field">
                <label>Sub-Division Name <span class="req">*</span></label>
                <input type="text" id="cstSubDivision" runat="server" placeholder="Enter sub-division name" />
                <div class="field-error" id="errSubDivision"></div>
            </div>

            <div class="field">
                <label>UDISE Code <span class="req">*</span></label>
                <input type="text" id="cstUDISECode" runat="server" placeholder="Enter UDISE code" />
                <div class="field-error" id="errUDISE"></div>
            </div>

            <div class="field">
                <label>Block Name <span class="req">*</span></label>
                <input type="text" id="cstBlockName" runat="server" placeholder="Enter block name" />
                <div class="field-error" id="errBlock"></div>
            </div>

            <div class="field">
                <label>Principal Name <span class="req">*</span></label>
                <input type="text" id="cstPrincipalName" runat="server" placeholder="Enter principal name" />
                <div class="field-error" id="errPrincipalName"></div>
            </div>

            <div class="field">
                <label>PIN Code <span class="req">*</span></label>
                <input type="text" id="cstPinCode" runat="server" maxlength="6" placeholder="6-digit PIN code" />
                <div class="field-error" id="errPin"></div>
            </div>

            <div class="field full-width">
                <label>Full Address <span class="req">*</span></label>
                <textarea id="cstFullAddress" runat="server" placeholder="Enter complete address"></textarea>
                <div class="field-error" id="errAddress"></div>
            </div>

        </div>

        <div class="btn-row">
         <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Update Profile"  OnClick="btnSave_Click" OnClientClick="return validateAndSubmit();" />
        </div>


    </div>

    <script type="text/javascript">
        function validateAndSubmit() {

            var valid = true;

            // Clear previous errors
            $(".field-error").text("");
            $("input, textarea").removeClass("invalid");

            // UDISE Code
            if ($("#<%= cstUDISECode.ClientID %>").val().trim() === "")
            {
                $("#errUDISE").text("UDISE Code is required.");
                $("#<%= cstUDISECode.ClientID %>").addClass("invalid");
                valid = false;
            }

            // Principal Name
            if ($("#<%= cstPrincipalName.ClientID %>").val().trim() === "")
            {
                $("#errPrincipalName").text("Principal Name is required.");
                $("#<%= cstPrincipalName.ClientID %>").addClass("invalid");
                valid = false;
            }

            // Mobile
            var mob = $("#<%= cstPrincipalMobile.ClientID %>").val().trim();
            if (mob === "")
            {
                $("#errMobile").text("Principal Mobile Number is required.");
                $("#<%= cstPrincipalMobile.ClientID %>").addClass("invalid");
                    valid = false;
            }
            else if (!/^\d+$/.test(mob))
            {
                    $("#errMobile").text("Digits only allowed.");
                    valid = false;
            }
            else if (!/^[6-9]/.test(mob))
            {
                    $("#errMobile").text("Must start with 6-9.");
                    valid = false;
            }
            else if (mob.length !== 10)
            {
                    $("#errMobile").text("Must be 10 digits.");
                    valid = false;
            }

    // Email
              var email = $("#<%= cstPrincipalEmail.ClientID %>").val().trim();
            if (email === "")
            {
                $("#errEmail").text("Email is required.");
                $("#<%= cstPrincipalEmail.ClientID %>").addClass("invalid");
                valid = false;
            }
            else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
            {
                $("#errEmail").text("Invalid email format.");
                valid = false;
            }

            // SubDivision
            if ($("#<%= cstSubDivision.ClientID %>").val().trim() === "")
            {
                $("#errSubDivision").text("Sub-Division is required.");
                valid = false;
            }

    // Block
            if ($("#<%= cstBlockName.ClientID %>").val().trim() === "")
            {
                $("#errBlock").text("Block Name is required.");
                valid = false;
             }

    // PIN
            var pin = $("#<%= cstPinCode.ClientID %>").val().trim();
            if (pin === "")
            {
                $("#errPin").text("PIN is required.");
                valid = false;
            }
            else if (!/^\d{6}$/.test(pin))
            {
                    $("#errPin").text("PIN must be 6 digits.");
                    valid = false;
            }

    // Address
            if ($("#<%= cstFullAddress.ClientID %>").val().trim() === "")
            {
                $("#errAddress").text("Address is required.");
                valid = false;
            }

            return valid; // VERY IMPORTANT
        }


    </script>

</asp:Content>
