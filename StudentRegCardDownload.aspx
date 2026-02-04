<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentRegCardDownload.aspx.cs" Inherits="StudentRegCardDownload" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>BSEB Login</title>
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
            font-weight:600 ;
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
    <form runat="server" id="formid">
        <!-- Header -->
       
        <div class="header text-center">
            <img src="assets/img/bsebimage2.png" class="logo" alt="Logo">
            Bihar School Examination Board
        </div>
        <!-- Main Card -->
        <div class="card-wrapper justify-content-center align-items-center">
            <div class="right-section text-center">
              
                <h5> Download Dummy Registration Card</h5>
                <%--<p></p>--%>
                <hr>
             

                <div onsubmit="return validateCaptcha()">
                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-user"></i></span>
                        <asp:TextBox runat="server" ID="txt_studentname" CssClass="form-control" placeholder="Student Name"></asp:TextBox>
                    </div>
                     <div id="studentNameError" class="error-message" style="display: none;">Please enter student name.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                        <asp:TextBox runat="server" ID="txt_dob" CssClass="form-control" TextMode="Date" placeholder="DOB" max="2015-12-31"></asp:TextBox>
                    </div>
                    <div id="dobError" class="error-message" style="display: none;">Please select date of birth.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-graduation-cap"></i></span>
                             <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                             </asp:DropDownList>
                           
                        <%--<asp:TextBox runat="server" ID="txt_faculty" CssClass="form-control" placeholder="Faculty"></asp:TextBox>--%>
                    </div>
                     <div id="facultyError" class="error-message" style="display: none;">Please select a Faculty.</div>

                    <div class="mb-3 input-group">
                        <span class="input-group-text"><i class="fas fa-code"></i></span>
                        <asp:TextBox runat="server" ID="txt_collegecode" CssClass="form-control" placeholder="College Code" TextMode="Number" oninput="enforceMaxLength(this, 5)"></asp:TextBox>
                    </div>
                      <div id="collegeCodeError" class="error-message" style="display: none;">Please enter college code.</div>

                    <div class="captcha-box">
                        <span id="num1">11</span> + 
                        <span id="num2">10</span> =
                        <input type="number" class="form-control" id="captchaInput" placeholder="Prove you are a human" required>
                        <button type="button" class="btn btn-light btn-sm" onclick="generateCaptcha()"><i class="fas fa-sync-alt"></i></button>
                    </div>

                    <input type="hidden" id="captchaResult">
                              <asp:Button ID="btnDwnlDummyCard" runat="server" Text="Submit" CssClass="btn btn-login" OnClientClick="return validateForm();" OnClick="DwnlDummyCard" />
                      <a href="login.aspx" target="_blank" class="">Back To Login</a>
 

                    <%--<asp:Button ID="Button1" runat="server" Text="Submit" CssClass="btn btn-login" OnClientClick="return validateCaptcha();" />--%>
                </div>
            </div>
        </div>

        <script src="assets/bundles/sweetalert/sweetalert.min.js"></script>
        <script src="assets/js/page/sweetalert.js"></script>
        <script>
            function generateCaptcha() {
                const n1 = Math.floor(Math.random() * 20) + 1;
                const n2 = Math.floor(Math.random() * 20) + 1;
                document.getElementById("num1").textContent = n1;
                document.getElementById("num2").textContent = n2;
                document.getElementById("captchaResult").value = n1 + n2;
                document.getElementById("captchaInput").value = '';
            }

            //function validateCaptcha() {
            //    const correct = parseInt(document.getElementById("captchaResult").value);
            //    const user = parseInt(document.getElementById("captchaInput").value);
            //    if (user !== correct) {
            //        swal({
            //            title: 'Failed',
            //            text: 'Captcha incorrect. Try again.',
            //            icon: 'error',
            //            button: 'Retry'
            //        });
            //        generateCaptcha();
            //        return false;
            //    }
            //    return true;
            //}

            // window.onload = generateCaptcha;

            function validateForm() {
                debugger
                let isValid = true;
                document.getElementById("studentNameError").style.display = "none";
                document.getElementById("dobError").style.display = "none";
                document.getElementById("facultyError").style.display = "none";
                document.getElementById("collegeCodeError").style.display = "none";
                const correct = parseInt(document.getElementById("captchaResult").value);
                const user = parseInt(document.getElementById("captchaInput").value);

                const studentName = document.getElementById("<%= txt_studentname.ClientID %>").value.trim();
                if (studentName === "") {
                    document.getElementById("studentNameError").style.display = "block";
                    return false;
                }
                const dob = document.getElementById("<%= txt_dob.ClientID %>").value.trim();
                if (dob === "") {
                    document.getElementById("dobError").style.display = "block";
                    return false;
                }
                const ddlFaculty = document.getElementById("<%= ddlFaculty.ClientID %>");
                const faculty = ddlFaculty.value;
                if (faculty === "" || faculty === "0") {
                    document.getElementById("facultyError").style.display = "block";
                    return false;
                }
                const collegeCode = document.getElementById("<%= txt_collegecode.ClientID %>").value.trim();
                if (collegeCode === "") {
                    document.getElementById("collegeCodeError").style.display = "block";
                    return false;
                }
                if (user !== correct) {
                    swal({
                        title: 'Failed',
                        text: 'Captcha incorrect. Try again.',
                        icon: 'error',
                        button: 'Retry'
                    });
                    generateCaptcha();
                    return false;
                }
            }


            function enforceMaxLength(el, maxLength) {
                if (el.value.length > maxLength) {
                    el.value = el.value.slice(0, maxLength);
                }
            }
            window.onload = generateCaptcha;
        </script>
    </form>
</body>
</html>
