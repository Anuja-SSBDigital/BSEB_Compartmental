<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

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
        html, body {
            overflow-x: hidden; /* remove horizontal scrollbar */
            margin: 0;
            padding: 0;
            width: 100%;
        }

        .row {
            margin-left: 0;
            margin-right: 0;
        }

        body {
            background-color: #eef5ff; /* light blue tint */
            /*background: url('assets/img/bhr2.jpg') no-repeat center center fixed;*/
            background-size: cover;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 0;
            min-height: 100vh;
        }

        .header {
            /*background: #1b2642;*/
            background: #3059c4;
            color: white;
            padding: 5px 50px;
            font-weight: bold;
            font-size: 18px;
        }

        .steps {
            background: #f8f9fa;
            padding: 2px 160px;
            font-size: 11px;
            font-weight: 500;
            color: #000;
            border-bottom: 1px solid #ccc;
        }

        .step-line {
            height: 1.5px;
            background-color: #ccc;
            margin: 0 4px;
        }

        .circle {
            font-weight: 600;
            line-height: 1;
        }

        .card-grid-row {
            min-height: 85vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .card-same {
            /* remove flex:1 so width is controlled only by Bootstrap col-7/col-5 */
            min-width: auto;
            max-width: 100%;
            height: 37em;
        }

        .info-card, .right-section {
            background: #fff;
            border-radius: 18px;
            /* remove min-height so each card takes height based on its content */
            box-shadow: 0 0 25px rgba(0,0,0,0.12);
            padding: 32px 30px;
        }

        .logo_head {
            width: 70px;
            margin: 0 auto 12px auto;
        }

        .form-control {
            border-radius: 6px;
            padding: 8px 12px;
            font-size: 14px;
        }

        .captcha-box {
            display: flex;
            gap: 8px;
            align-items: center;
            margin-bottom: 12px;
        }

        .btn-login {
            background-color: #3059c4;
            border: none;
            color: white;
            font-weight: 600;
            width: 100%;
            padding: 10px 12px;
            border-radius: 6px;
            font-size: 15px;
        }

            .btn-login:hover {
                background-color: #e66000;
            }

        @media(max-width: 991px) {
            .card-grid-row {
                flex-direction: column;
            }
        }
    </style>
</head>
<body>
    <form runat="server" id="formid">
        <!-- Header -->
        <!-- Header -->
        <div class="row">
            <div class="header d-flex justify-content-between align-items-center px-3 py-2">
                <!-- Left: Logo + Title -->

                <div class="col-lg-8">
                    <div class="align-items-center d-flex justify-content-center">
                        <img src="assets/img/bsebimage2.png" alt="Logo" style="width: 45px; margin-right: 8px;">
                        <h5 class="mb-0 fw-bold">BIHAR SCHOOL EXAMINATION BOARD</h5>
                    </div>

                </div>
                <div class="col-lg-4">
                    <div class="small" style="line-height: 1.4;">
                        <div><span class="fw-lighter">Help Desk No.:</span> 0612-2230039<span style="color: #d32f2f;" class="fw-lighter"></span></div>
                        <%-- <div class="fw-lighter">(10 AM to 6 PM)</div>--%>
                        <div><span class="fw-lighter">Help Desk Email: <b>bsebinterhelpdesk@gmail.com</b></span> <%--<span style="color: #d32f2f;" class="fw-lighter"></span>--%></div>
                        <%--       <div><span class="fw-lighter">Payment Help Desk No.:</span> <span style="color: #d32f2f;" class="fw-lighter"></span></div>--%>
                    </div>
                </div>
                <!-- Right: Help Desk Info -->

            </div>
        </div>

        <!-- Steps -->
        <!-- Steps -->
        <div class="steps text-center py-2">
            <div class="d-flex justify-content-center align-items-center flex-wrap gap-2">
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">1</div>
                    <small style="font-size: 12px;">Login</small>
                </div>
                <div class="step-line flex-grow-1"></div>
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">2</div>
                    <small style="font-size: 12px;">Download Examination form </small>
                </div>
                <div class="step-line flex-grow-1"></div>
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">3</div>
                    <small style="font-size: 12px;">Examination payment </small>
                </div>
                <div class="step-line flex-grow-1"></div>
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">4</div>
                    <small style="font-size: 12px;">Payment Status</small>
                </div>
                <div class="step-line flex-grow-1"></div>
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">5</div>
                    <small style="font-size: 12px;">Fill Examination Form</small>
                </div>
                <%-- <div class="step-line flex-grow-1"></div>--%>
                <%--  <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">6</div>
                    <small style="font-size: 12px;">Declaration Upload</small>
                </div>--%>
                <div class="step-line flex-grow-1"></div>
                <div class="step-item text-center">
                    <div class="circle bg-primary text-white rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 28px; height: 28px; font-size: 13px;">6</div>
                    <small style="font-size: 12px;">View Submitted Form</small>
                </div>
            </div>
        </div>



        <!-- Main Card Layout -->
        <div class="container-fluid">
            <div class="row justify-content-center align-items-start card-grid-row mt-3">

                <!-- LEFT: INFO (col-7) -->
                <div class="col-lg-8 col-md-12 mb-4">
                    <div class="card-same">
                        <div class="right-section d-flex flex-column h-100">
                            <h4 class="mb-3 text-center">Information And Notifications</h4>
                            <hr>
                            <%-- <p style="color: #e66000; font-weight: 500; font-size: 16px;">
                                <i class="fas fa-home"></i>नोट: यहाँ पर आप महत्वपूर्ण सूचना देख सकते हैं।
                            </p>--%>

                            <%--  <p class="mt-4" style="color:red; font-weight:bold;font-size: 30px">
                               The website will remain closed for maintenance today from 9:00 PM to 10:30 PM.
                            </p>--%>
                            <a href="assets/Notifications/Vigyapti-for-Inter-Annual-Exam-2026.pdf"
                                download="Vigyapti-for-Inter-Annual-Exam-2026.pdf"
                                style="color: #3059c4; display: block; font-size: 25px;"
                                class="mt-4">
                                <i class="fas fa-file-download"></i>Click here to download vigyapti for Intermediate Annual Examination 2026 
                            </a>
                            <a href="javascript:void(0);"
                                onclick="downloadAllFiles()"
                                style="color: #3059c4; display: block; font-size: 25px;"
                                class="mt-4">
                                <i class="fas fa-file-download"></i>Click here to download all Ex-student Exam Forms
                            </a>
							
							 <%--<a href="http://intermediate.biharboardonline.com/Exam26/StudentExamDummyCard.aspx"  style="color: #3059c4; display: block; font-size: 25px;"
   class="mt-4">
   Click here for Intermediate Dummy Admit Card(Student Login)
</a>--%>
<a href="#" 
   onclick="showClosedAlert(); return false;" 
   style="color: #3059c4; display: block; font-size: 25px;">
   Click here for Intermediate Dummy Admit Card (Student Login)
</a>

                            <script>
							
							function showClosedAlert() {
    Swal.fire({
        title: 'Date Closed',
        text: 'Dummy Admit Card download window has been closed.',
        icon: 'info',
        confirmButtonText: 'OK',
        confirmButtonColor: '#3059c4'
    });
}
                                function downloadAllFiles() {
                                    const files = [
                                        "assets/Notifications/Vocational-Ex-Students.pdf",
                                        "assets/Notifications/Art-Ex-Student's.pdf",
                                        "assets/Notifications/Commerce-Ex-Student's.pdf",
                                        "assets/Notifications/Science-Ex-Students.pdf"
                                    ];

                                    files.forEach(file => {
                                        const a = document.createElement("a");
                                        a.href = file;
                                        a.download = file.split('/').pop(); // keep original filename
                                        document.body.appendChild(a);
                                        a.click();
                                        document.body.removeChild(a);
                                    });
                                }
                            </script>

                            <%--<a href="assets/Notifications/CommercePrivate.pdf"
                                download="CommercePrivate.pdf"
                                style="color: #3059c4; display: block; font-size:20px;"
                                class="mt-4">
                                <i class="fas fa-file-download"></i>Click here to download Commerce Private Registration form Session 2025-27
                            </a>

                            <a href="assets/Notifications/IntermediateRegistration_Vigyapt_for_session_2025-27.pdf"
                                download="Intermediate_Registration_Vigyapti_2025-27.pdf"
                                style="color: #3059c4; display: block; font-size: 20px;"
                                class="mt-4">
                                <i class="fas fa-file-download"></i>Click here to download Intermediate 2025-27 Registration Vigyapti
                            </a>
                              <a href="assets/Notifications/registration_declaration_vigyapti.pdf"
      download="registration_declaration_vigyapti.pdf"
      style="color: #3059c4; display: block; font-size: 20px;"
      class="mt-4">
      <i class="fas fa-file-download"></i>Click here to download Intermediate session 2025-27 Registration Declaration Vigyapti
  </a>
                            <p class="mt-4" style="color:red; font-weight:bold;font-size: 20px">
                                नोट - पंजीयन फॉर्म भरने के पश्चात्  घोषणा पत्र अनिवार्य रूप से अपलोड करे। जिनका घोषणा पत्र अपलोड नहीं किया जायेगा उनका पंजीयन फॉर्म सबमिट नहीं माना जायेगा।
                            </p>--%>
                        </div>
                    </div>
                </div>

                <!-- RIGHT: LOGIN (col-5) -->
                <div class="col-lg-4 col-md-12">
                    <div class="card-same">
                        <div class="info-card text-center d-flex flex-column h-100">
                            <img src="assets/img/bsebimage.jpg" class="logo_head" alt="Logo">
                            <h5>Bihar School Examination Board</h5>
                            <p>Intermediate Annual Examination 2026</p>
                            <hr>
                            <p><i class="fas fa-user"></i>Login to start your session</p>

                            <!-- Login form -->
                            <div>
                                <div class="mb-3 input-group">
                                    <span class="input-group-text"><i class="fas fa-user"></i></span>
                                    <asp:TextBox runat="server" ID="txt_username" CssClass="form-control" placeholder="UserName"></asp:TextBox>
                                </div>
                                <div class="mb-3 input-group">
                                    <span class="input-group-text"><i class="fas fa-lock"></i></span>
                                    <asp:TextBox runat="server" ID="txt_password" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                                    <button class="btn btn-outline-secondary" type="button" onclick="togglePassword()">
                                        <i class="fas fa-eye" id="toggleIcon"></i>
                                    </button>
                                </div>
                                <div class="captcha-box">
                                    <span id="num1">11</span> + 
              <span id="num2">10</span> =
              <input type="number" class="form-control" id="captchaInput" placeholder="Prove you are a human">
                                    <button type="button" class="btn btn-light btn-sm" onclick="generateCaptcha()"><i class="fas fa-sync-alt"></i></button>
                                </div>
                                <input type="hidden" id="captchaResult">
                                <asp:Button ID="Button1" runat="server" Text="Login" OnClick="btn_login_Click"
                                    CssClass="btn btn-login" OnClientClick="return validateCaptcha();" />
                                <%--                                <a href="StudentRegCardDownload.aspx" target="_blank" class="d-block mt-2">Download Student Registration Card--%>
                                <%-- </a>--%>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <!-- Scripts -->
		<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
            function validateCaptcha() {
                const correct = parseInt(document.getElementById("captchaResult").value);
                const user = parseInt(document.getElementById("captchaInput").value);
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
                return true;
            }
            function togglePassword() {
                const passwordField = document.getElementById("<%= txt_password.ClientID %>");
                const icon = document.getElementById("toggleIcon");
                if (passwordField.type === "password") {
                    passwordField.type = "text";
                    icon.classList.remove("fa-eye");
                    icon.classList.add("fa-eye-slash");
                } else {
                    passwordField.type = "password";
                    icon.classList.remove("fa-eye-slash");
                    icon.classList.add("fa-eye");
                }
            }
            window.onload = generateCaptcha;
        </script>
    </form>
</body>
</html>
