<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentExamDummyCard.aspx.cs" Inherits="StudentExamDummyCard" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>BSEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap & FontAwesome -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet">

    <style>
        body {
            background-color: #f9f9f9;
            background-image: linear-gradient(45deg, #eee 25%, transparent 25%, transparent 75%, #eee 75%, #eee), linear-gradient(45deg, #eee 25%, transparent 25%, transparent 75%, #eee 75%, #eee);
            background-size: 20px 20px;
            background-position: 0 0, 10px 10px;
            background-size: cover;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 0;
            height: 100vh;
            display: flex;
            flex-direction: column;
            overflow: auto; /* Prevent body scroll */
        }

        .header {
            background: #415791;
            color: white;
            padding: 16px;
            font-weight: 600;
            font-size: 20px;
            text-align: center;
        }

        .card-wrapper {
            flex: 1;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 30px 15px;
        }

        .right-section {
            background: white;
            padding: 30px 25px;
            border-radius: 16px;
            width: 100%;
            max-width: 600px;
            box-shadow: 0 0 30px rgba(0, 0, 0, 0.1);
        }

        .logo {
            width: 64px;
            margin-bottom: 10px;
        }

        .form-control {
            border-radius: 6px !important;
            padding: 10px 14px;
            font-size: 14px;
        }

        .input-group {
            margin-bottom: 15px;
        }

        .captcha-box {
            display: flex;
            gap: 8px;
            align-items: center;
            margin-bottom: 18px;
        }

        .btn-login {
            background-color: #415791;
            border: none;
            color: white;
            font-weight: 600;
            width: 100%;
            padding: 10px;
            border-radius: 6px;
            font-size: 14px;
        }

        .btn-login:hover {
            background-color: #e66000;
        }

        @media(max-width: 576px) {
            .right-section {
                padding: 20px 15px;
            }

            .captcha-box {
                flex-direction: column;
                align-items: flex-start;
            }

            .captcha-box input,
            .captcha-box button {
                width: 100%;
            }
        }

        .error-message {
            color: red;
            font-size: 0.875em;
            text-align: left;
            margin-top: -10px;
            margin-bottom: 10px;
        }
    </style>

</head>
<body>
    <!-- Note: onsubmit added so Enter key triggers client-side validation -->
    <form runat="server" id="formid" onsubmit="return validateForm();">
        <!-- Header -->
           <asp:ScriptManager runat="server" ID="sm1" />
        <div class="header text-center">
            <img src="assets/img/bsebimage2.png" class="logo" alt="Logo">
            Bihar School Examination Board
        </div>

        <!-- Main Card -->
        <div class="card-wrapper justify-content-center align-items-center">
            <div class="right-section text-center">

                <h5> Download Dummy Admit Card</h5>
                <hr>

                <div>
                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-code"></i></span>
                        <asp:TextBox runat="server" ID="txt_collegecode" CssClass="form-control" placeholder="College Code" TextMode="Number" oninput="enforceMaxLength(this, 5)"></asp:TextBox>
                    </div>
                    <div id="collegeCodeError" class="error-message" style="display: none;">Please enter college code.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-user"></i></span>
                        <asp:TextBox runat="server" ID="txt_RegistrationNo" CssClass="form-control" placeholder="Registration No"></asp:TextBox>
                    </div>
                    <div id="RegistrationNoError" class="error-message" style="display: none;">Please enter Registration No.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-graduation-cap"></i></span>
                       <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                             </asp:DropDownList>
                    </div>
                    <div id="facultyError" class="error-message" style="display: none;">Please select a Faculty.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                        <asp:TextBox runat="server" ID="txt_dob" CssClass="form-control" TextMode="Date" placeholder="DOB" max="2015-12-31"></asp:TextBox>
                    </div>
                    <div id="dobError" class="error-message" style="display: none;">Please select date of birth.</div>

                                     <div class="captcha-box">
                      <span id="num1">11</span> + 
                        <span id="num2">10</span> =
                        <input type="number" class="form-control" id="captchaInput" placeholder="Prove you are a human">
                      <button type="button" class="btn btn-light btn-sm" onclick="generateCaptcha()"><i class="fas fa-sync-alt"></i></button>
                  </div>
                  <input type="hidden" id="captchaResult">
          <%--        <asp:Button ID="Button1" runat="server" Text="Login" OnClick="btn_login_Click"
                      CssClass="btn btn-login" OnClientClick="return validateCaptcha();" />--%>

                    <asp:Button ID="btnDwnlDummyCard" runat="server" Text="Submit" CssClass="btn btn-login" OnClientClick="return validateForm();" OnClick="DwnlDummyCard" />

                </div>
            </div>
        </div>

         
    </form>
</body>
</html>
<script src="assets/bundles/sweetalert/sweetalert.min.js"></script>

<script>
    window.onload = function () {
        // generate captcha first
        generateCaptcha();

        // Get query string
        const urlParams = new URLSearchParams(window.location.search);
        const msg = urlParams.get('msg');

        if (msg === 'nodata') {
            // Show SweetAlert (modern swal with icon)
            alert("No Data Found! No records available.");
        }
    };
    function validateForm() {
        debugger
        // 1. Clear previous errors
        document.getElementById("collegeCodeError").style.display = "none";
        document.getElementById("RegistrationNoError").style.display = "none";
        document.getElementById("facultyError").style.display = "none";
        document.getElementById("dobError").style.display = "none";


        // 2. Get form elements
        const collegeCodeEl = document.getElementById("<%= txt_collegecode.ClientID %>");
        const regNoEl = document.getElementById("<%= txt_RegistrationNo.ClientID %>");
        const ddlFaculty = document.getElementById("<%= ddlFaculty.ClientID %>");
        const dobEl = document.getElementById("<%= txt_dob.ClientID %>");
        const captchaInputEl = document.getElementById("captchaInput"); // Added to easily access the element

        // Safety check for critical elements
        if (!collegeCodeEl || !regNoEl || !ddlFaculty || !dobEl || !captchaInputEl) {
            console.error("Critical form element not found. Allowing postback.");
            return true;
        }


        // 3. Validation Checks (College Code, Registration No, Faculty, DOB)
        const collegeCode = collegeCodeEl.value.trim();
        if (collegeCode === "") {
            document.getElementById("collegeCodeError").style.display = "block";
            collegeCodeEl.focus();
            collegeCodeEl.scrollIntoView({ behavior: "smooth", block: "center" });
            return false;
        }

        const regNo = regNoEl.value.trim();
        if (regNo === "") {
            document.getElementById("RegistrationNoError").style.display = "block";
            regNoEl.focus();
            regNoEl.scrollIntoView({ behavior: "smooth", block: "center" });
            return false;
        }

        const faculty = ddlFaculty.value;
        if (faculty === "" || faculty === "0") {
            document.getElementById("facultyError").style.display = "block";
            ddlFaculty.focus();
            ddlFaculty.scrollIntoView({ behavior: "smooth", block: "center" });
            return false;
        }

        const dob = dobEl.value.trim();
        if (dob === "") {
            document.getElementById("dobError").style.display = "block";
            dobEl.focus();
            dobEl.scrollIntoView({ behavior: "smooth", block: "center" });
            return false;
        }

        // 4. Captcha Validation (The Fix!)
        const userInputValue = captchaInputEl.value.trim();
        const correct = parseInt(document.getElementById("captchaResult").value);

        // CHECK 1: Ensure the input field is NOT empty
        if (userInputValue === "") {
            swal({
                title: 'Attention',
                text: 'Please Enter the Captcha.',
                icon: 'warning',
                button: 'OK'
            });
            captchaInputEl.focus();
            return false;
        }

        // CHECK 2: Ensure the input value is correct
        const user = parseInt(userInputValue);
        if (user !== correct) {
            swal({
                title: 'Failed',
                text: 'Captcha incorrect. Try again.',
                icon: 'error',
                button: 'Retry'
            });
            generateCaptcha(); // Regenerate for a new try
            captchaInputEl.focus();
            return false;
        }

        // 5. If all validations pass, allow postback
        return true;
    }

    function generateCaptcha() {
        const n1 = Math.floor(Math.random() * 20) + 1;
        const n2 = Math.floor(Math.random() * 20) + 1;
        document.getElementById("num1").textContent = n1;
        document.getElementById("num2").textContent = n2;
        document.getElementById("captchaResult").value = n1 + n2;
        document.getElementById("captchaInput").value = ''; // Clear input on refresh
    }

    // Call generateCaptcha when the page loads
    window.onload = generateCaptcha;

    function enforceMaxLength(el, maxLength) {
        if (el.value.length > maxLength) {
            el.value = el.value.slice(0, maxLength);
        }
    }
</script>