<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewstudentregdetalis.aspx.cs" Inherits="viewstudentregdetalis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .preview-img {
            width: 120px;
            height: 140px;
            border: 1px solid #ccc;
            object-fit: cover;
            margin-bottom: 5px;
        }

        .signature-box {
            width: 140px;
            border: 1px solid #000;
            padding: 5px;
            text-align: center;
        }

        .signature-img {
            width: 100%;
            height: 80px;
            object-fit: contain;
        }

        table {
            width: 100%;
            border: 1px solid black;
        }

        td {
            padding: 6px 10px;
            vertical-align: top;
        }

        .label {
            font-weight: bold;
            float: right;
        }

        .brd_1 tr {
            border: 1px solid black;
        }

        .brd_1 th td {
            border: 1px solid black;
        }

        .page-break {
            page-break-before: always;
        }

        input[type=checkbox] {
            transform: scale(1.5);
        }
        /*  @media print {
    #btnPrint,
    #btnUpdate,
    input[type="checkbox"],
    label[for="declaration"] {
        display: none !important;
    }

    body {
        -webkit-print-color-adjust: exact;
    }*/
        /*}*/
    </style>
    <style type="text/css" media="print">
        /* General Print Adjustments */
        body {
            margin: 0;
            padding: 0;
            -webkit-print-color-adjust: exact; /* Ensures backgrounds/colors print */
            font-family: Arial, sans-serif; /* Consistent font for print */
            font-size: 10pt; /* Smaller base font size for better fit */
        }

        /* Hide UI elements not needed for print */
        #btnPrint,
        #btnUpdate,
        input[type="checkbox"],
        label[for="declaration"],
        .card-header-action, /* Hides the entire action section in header */
        .sidebar, /* Hide your sidebar/navigation if you have one */
        .navbar, /* Hide your top navigation bar */
        .footer, /* Hide any footer elements */
        .form-group.d-flex.justify-content-end.mt-4, /* Hide the button group specifically */
        .page-header, /* If you have a page title header */
        .breadcrumb { /* Hide breadcrumbs */
            display: none !important;
        }

        /* Main Content Container Adjustment */
        /* Assuming 'confirmdata' is your main content wrapper */
        #confirmdata {
            width: 100%; /* Take full width of the printable area */
            max-width: 780px; /* Standard A4 width is around 793px at 96 DPI */
            margin: 0 auto; /* Center the content */
            padding: 15mm; /* Add some print margins around the content */
            box-sizing: border-box; /* Include padding in the width */
        }

        /* Card/Panel Styling for Print */
        .card {
            border: 1px solid #ccc; /* Add a subtle border for separation */
            box-shadow: none; /* Remove shadows */
            margin-bottom: 15px; /* Spacing between sections */
        }

        .card-header {
            background-color: #f0f0f0 !important; /* Light background for headers */
            border-bottom: 1px solid #ccc;
            padding: 8px 15px;
            font-weight: bold;
            text-align: center; /* Center header text */
        }

        .card-body {
            padding: 10px 15px; /* Adjust padding within card body */
        }

        /* Table Styling */
        table {
            width: 100% !important; /* Ensure tables take full available width */
            border-collapse: collapse; /* Remove space between cells */
            margin-bottom: 10px;
        }

        table, th, td {
            border: 1px solid #000 !important; /* Ensure all table cells have borders */
            font-size: 9pt; /* Smaller font for table content */
            padding: 3px 5px; /* Adjust cell padding */
            word-wrap: break-word; /* Prevent long words from overflowing */
        }

        th {
            background-color: #e9e9e9; /* Light background for table headers */
            text-align: left; /* Align headers to left */
            font-weight: bold;
        }

        /* Specific Label/Value Pairs (if you're using a grid or custom layout) */
        /* Assuming a layout like: <div class="row"><div class="col-6"><label>...</label><span>...</span></div></div> */
        .form-group {
            margin-bottom: 5px; /* Reduce space between form groups */
            display: flex; /* Use flexbox for aligned labels/values */
            flex-wrap: nowrap; /* Prevent wrapping in flex items */
            align-items: baseline;
        }

            .form-group label {
                font-weight: bold;
                flex: 0 0 35%; /* Allocate fixed width for labels */
                max-width: 35%;
                padding-right: 5px;
                text-align: right; /* Right align labels for better readability */
            }

            .form-group span {
                flex: 1; /* Take remaining space */
                word-wrap: break-word;
                text-align: left;
            }

        /* If your layout uses Bootstrap's col-md-6 etc. directly */
        .col-md-6, .col-sm-6, .col-12 {
            width: 50% !important; /* Force 50% width for columns */
            float: left !important; /* Ensure they are side-by-side */
            padding-left: 5px !important;
            padding-right: 5px !important;
            box-sizing: border-box; /* Include padding in width calculation */
        }

        .col-md-4, .col-sm-4 {
            width: 33.33% !important; /* Force 33.33% width for columns */
            float: left !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
            box-sizing: border-box;
        }

        .col-md-12 {
            width: 100% !important;
            float: none !important;
        }

        .row {
            clear: both; /* Clear floats after each row */
            margin-left: 0 !important;
            margin-right: 0 !important;
        }


        /* Images (Photo and Signature) */
        .photo-signature-section {
            display: flex; /* Use flexbox to keep them side-by-side */
            justify-content: space-around; /* Distribute space evenly */
            align-items: flex-end; /* Align at the bottom (for text below image) */
            margin-top: 20px;
            margin-bottom: 20px;
        }

        .photo-container, .signature-container {
            text-align: center;
            width: 45%; /* Give them space */
            border: 1px dashed #ccc; /* Add a dashed border as seen in forms */
            padding: 5px;
        }

        .preview-img, .signature-img {
            max-width: 100px; /* Fixed small size for print */
            max-height: 100px;
            width: auto;
            height: auto;
            border: 1px solid #ccc;
            padding: 2px;
            display: block; /* Images on their own line */
            margin: 5px auto; /* Center images */
        }

        .signature-text, .photo-text {
            font-size: 8pt;
            margin-top: 5px;
            text-align: center;
        }

        /* Ensure text is black and readable */
        p, label, span, h4, h5, td, th {
            color: #000 !important;
        }

        /* Page Breaks - Important for multi-page documents */
        .card {
            page-break-inside: avoid; /* Avoid breaking a card in the middle */
        }

        .page-break-after {
            page-break-after: always; /* Force a page break after specific elements */
        }

        .page-break-before {
            page-break-before: always;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="confirmdata">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h4>CONFIRM DETAILS</h4>
                        <div class="card-header-action">
                            <div class="print-btn-container">
                                <button type="button" id="btndownloadpdf" visible="false" runat="server" class="btn btn-primary" onclick="downloadPDF()">Download PDF</button>

                                <asp:HiddenField ID="hfCollegeId" runat="server" />
                                <%--<asp:Button ID="btnPrint" CssClass="btn btn-info" runat="server" Text="PRINT" OnClick="btnPrint_Click" />--%>
                            </div>
                            <%--   <a href="register_27.aspx" class="btn btn-warning">Back to List</a>
                            <a href="#" class="btn btn-warning">Back</a>--%>
                        </div>
                    </div>
                    <div id="printableDiv">
                        <div class="card-body">

                            <h4 class="text-center d-none" id="hidelble">Confirm Details</h4>
                            <table class=" mb-3">
                                <tr>
                                    <td class="label">Student Name:</td>
                                    <td>
                                        <asp:Label ID="lblStudentName" runat="server" />
                                    </td>
                                    <td class="label">Category:</td>
                                    <td>
                                        <asp:Label ID="lblCategory" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Father Name:</td>
                                    <td>
                                        <asp:Label ID="lblFatherName" runat="server" />
                                    </td>
                                    <td class="label">Mother Name:</td>
                                    <td>
                                        <asp:Label ID="lblMotherName" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">+2 School/College Name:</td>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" />
                                    </td>
                                    <td class="label">Date Of Birth:</td>
                                    <td>
                                        <asp:Label ID="lblDOB" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="label">Matric/Class X Board's Name:</td>
                                    <td>
                                        <asp:Label ID="lblMatricBoardName" runat="server" />
                                    </td>
                                    <td class="label">Matric/Class X Board's Roll Code:</td>
                                    <td>
                                        <asp:Label ID="lblRollCode" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Roll Number:</td>
                                    <td>
                                        <asp:Label ID="lblRollNumber" runat="server" />
                                    </td>
                                    <td class="label">Passing Year:</td>
                                    <td>
                                        <asp:Label ID="lblPassingYear" runat="server" />
                                    </td>
                                </tr>


                                <tr>
                                    <td class="label">Gender:</td>
                                    <td>
                                        <asp:Label ID="lblGender" runat="server" />
                                    </td>
                                    <td class="label">Caste Category:</td>
                                    <td>
                                        <asp:Label ID="lblCaste" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Differently Abled:</td>
                                    <td>
                                        <asp:Label ID="lblDifferentlyAbled" runat="server" />
                                    </td>


                                    <td class="label">Nationality:</td>
                                    <td>
                                        <asp:Label ID="lblNationality" runat="server" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="label">Religion:</td>
                                    <td>
                                        <asp:Label ID="lblReligion" runat="server" />
                                    </td>
                                    <td class="label">Area:</td>
                                    <td>
                                        <asp:Label ID="lblArea" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="label">Mobile No of Student:</td>
                                    <td>
                                        <asp:Label ID="lblMobileNo" runat="server" />
                                    </td>
                                    <td class="label">Email ID:</td>
                                    <td>
                                        <asp:Label ID="lblEmailId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">BSEB Unique Id:</td>
                                    <td>
                                        <asp:Label ID="lbluniqueno" runat="server" />
                                    </td>
                                    <td class="label">Aadhar No:</td>
                                    <td>
                                        <asp:Label ID="lblAadharNo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Address:</td>
                                    <td>
                                        <asp:Label ID="lblAddress" runat="server" />
                                    </td>
                                    <td class="label">Pin Code:</td>
                                    <td>
                                        <asp:Label ID="lblPinCode" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Student Bank A/C No:</td>
                                    <td>
                                        <asp:Label ID="lblAccountNo" runat="server" />
                                    </td>
                                    <td class="label">Marital Status:</td>
                                    <td>
                                        <asp:Label ID="lblMaritalStatus" runat="server" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="label">Bank & Branch Name:</td>
                                    <td>
                                        <asp:Label ID="lblBankBranch" runat="server" />
                                    </td>
                                    <td class="label">IFSC Code:</td>
                                    <td>
                                        <asp:Label ID="lblIFSC" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="label">Two Identification (i):</td>
                                    <td>
                                        <asp:Label ID="lblIdent1" runat="server" />
                                    </td>
                                    <td class="label">(ii):</td>
                                    <td>
                                        <asp:Label ID="lblIdent2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Medium:</td>
                                    <td>
                                        <asp:Label ID="lblMedium" runat="server" />
                                    </td>
                                    <td class="label">Aapar Id:</td>
                                    <td>
                                        <asp:Label ID="lbl_aparid" runat="server" />
                                    </td>

                                </tr>

                            </table>


                            <table class="table  table-md brd_1" border="1" id="printable">
                                <thead>
                                    <tr>
                                        <th>S.No.</th>
                                        <th>Subject Group</th>
                                        <th>Subject Code</th>
                                        <th>Subject Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptSubjects" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Container.ItemIndex + 1 %></td>
                                                <td><%# Eval("SubjectGroup") %></td>
                                                <td><%# Eval("SubjectPaperCode") %></td>
                                                <td><%# Eval("PaperType") == null || Eval("PaperType").ToString() == "" 
        ? Eval("SubjectName") 
        : Eval("SubjectName") + " (" + Eval("PaperType") + ")" %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>

                            <div class="page-break"></div>
                            <!-- Forces new PDF page -->


                            <div class="row mt-4">
                                <div class="col-md-6 text-center photo-signature">

                                    <asp:Image ID="imgPhoto" runat="server" CssClass="preview-img" />
                                    <%--<p>
                                        <asp:Label ID="lblimgPhoto" runat="server" />
                                    </p>--%>
                                </div>
                                <div class="col-md-6 text-center photo-signature">
                                    <div class="signature-box">
                                        <asp:Image ID="imgSignature" runat="server" CssClass="signature-img" />
                                        <%--<div class="mt-1">
                                            <p class="signature">
                                                <asp:Label ID="lblimgSignature" runat="server" />
                                            </p>

                                        </div>--%>
                                    </div>


                                </div>
                            </div>

                            <div class="mt-4 text-center" id="hideThisDiv">
                                <%--  <p>
                                    <strong>Amount:</strong>
                                    <asp:Label ID="lblStuFees" runat="server" />
                                </p>--%>
                                <%--  <h5 id="declarationContainer" runat="server">
                                       
                                        <label class="form-check-label text-danger font-17">NOTE: 

पंजीयन प्रक्रिया पूर्ण करने के लिए घोषणा पत्र अपलोड करना अनिवार्य है इसे  Declaration Upload Menu में जाकर अपलोड करेंगे। 

To complete the registration process, it is mandatory to upload the declaration form. You can upload it from the Declaration Upload Menu.  </label>
                                    </h5>--%>
                                <h5 id="declarationContainer" runat="server">
                                    <label class="form-check-label font-18">
                                        <span class="text-danger">NOTE: 
                                           
                                            पंजीयन प्रक्रिया पूर्ण करने के लिए घोषणा पत्र अपलोड करना अनिवार्य है इसे  
            <span class="text-info">Declaration Upload Menu</span>
                                            में जाकर अपलोड करेंगे। 
                                        </span>
                                        <br />
                                        <span class="text-danger">To complete the registration process, it is mandatory to upload the declaration form. 
            You can upload it from the 
            <span class="text-info">Declaration Upload Menu.</span>
                                        </span>
                                    </label>
                                </h5>
                                <br />

                                <div class="col-md-12">
                                    <asp:HiddenField ID="hfCategoryType" runat="server" />
                                    <asp:Button ID="btnUpdateReg" runat="server" Text="Edit" CssClass="btn btn-primary px-4 mt-2 font-17" OnClick="btnUpdateRegistration_Click" />
                                    <asp:Button ID="btnUpdate" runat="server" Text="Save" CssClass="btn btn-primary px-4 mt-2 font-17" OnClick="btnUpdate_Click" OnClientClick="return validateRegViewDetails();" />

                                </div>

                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>

    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script>
        function downloadPDF() {
            // Hide/show elements
            document.getElementById('hideThisDiv').style.display = 'none';
            document.getElementById('hidelble').classList.remove('d-none');

            const element = document.getElementById("printableDiv");



            // Generate PDF
            html2pdf().set({
                margin: 8,
                filename: 'ConfirmDetails.pdf',
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 2 },
                jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
            }).from(element).save().then(() => {
                // Restore UI
                document.getElementById('hideThisDiv').style.display = 'block';
                document.getElementById('hidelble').classList.add('d-none');
            });
        }

    </script>

    <script>

      <%--  function validateRegViewDetails() {
            debugger
            var declarationCheckbox = document.getElementById('<%= declaration.ClientID %>');

            if (!declarationCheckbox.checked) {
                sweetAlert({
                    title: "Declaration Required",
                    text: "Please agree to the declaration by checking the box to proceed.",
                    type: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: true
                });
                return false;
            }
        }--%>

    </script>
</asp:Content>

