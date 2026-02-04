<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DECLARATIONFORM.aspx.cs" Inherits="DECLARATIONFORM" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Declaration Form</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .container {
            padding: 50px;
            margin-top: 20px;
        }

        .header {
            text-align: center;
            margin-bottom: 20px;
        }

        .logo {
            max-width: 110px;
            margin-bottom: 10px;
        }

        .photo {
            width: 120px;
            height: 150px;
            border: 1px solid #ccc;
            margin-top: 10px;
        }

        .table-details, .subjects-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

            .table-details th, .table-details td, .subjects-table th, .subjects-table td {
                border: 1px solid black;
                padding: 8px;
                text-align: center;
            }

            .subjects-table th {
                font-weight: 600;
                background-color: #f8f8f8;
            }

            .subjects-table .hindi {
                font-family: 'Noto Serif Devanagari', sans-serif;
                font-size: 0.9rem;
            }

        .box {
            border: 1px solid #000;
            padding: 10px;
            border-radius: 40px;
        }

        .sig-line {
            border: 1px solid #000;
            height: 85px;
            margin-bottom: 5px;
            min-width: 120px;
        }

        .section-title {
            font-weight: bold;
            text-align: center;
            margin-bottom: 5px;
        }

        ol li {
            margin-bottom: 5px;
        }

        .borderline {
            border: 1px solid black;
            padding: 10px;
        }

        .double-line {
            border: none;
            border-top: 3px double black;
            margin: 7px 0;
        }

        @media print {
            body {
                margin: 0;
                padding: 0;
                font-size: 20px;
                -webkit-print-color-adjust: exact !important;
                print-color-adjust: exact !important;
                color-adjust: exact !important;
            }

            @page {
                size: A4 portrait;
                margin: 10mm;
            }

            #admitCard {
                transform: scale(0.9);
                transform-origin: top left;
                width: 100%;
                height: auto;
                page-break-inside: avoid;
            }

            .btn, .no-print {
                display: none !important;
            }

            .container {
                border: 1px solid black;
                margin: 0;
                padding: 0;
                width: 100%;
                page-break-inside: avoid;
            }

            .row, .table, .table th, .table td, .header, .instructions, .signature {
                page-break-inside: avoid;
                break-inside: avoid;
            }

            .table {
                font-size: 30px;
            }

            img {
                max-width: 100%;
                height: auto;
            }

            hr {
                border-top: 1px solid black !important;
            }

            .custom-hr {
                margin: 1rem 0;
                color: inherit;
                border: none;
                border-top: 2px solid #000;
                opacity: 1;
            }
        }

        table.student-details td:first-child,
        table.student-details td:nth-child(odd) {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="no-print text-center my-3">
            <a href="ManageDeclarationForm.aspx" class="btn btn-primary no-print" visible="false" id="BtnBack" runat="server" style="text-decoration: none !important;">Back</a>
            <button type="button" class="btn btn-primary" onclick="generatePDF()">Download PDF</button>
        </div>

        <asp:Repeater ID="rptStudents" runat="server">
            <ItemTemplate>
                <div class="container admitCardItem" id="admitCard">
                    <div class="header">
                        <div class="row">
                            <div class="col-md-3">
                                <img src="assets/img/bsebimage.jpg" alt="Bihar Board Logo" class="logo">
                            </div>
                            <div class="col-md-6">
                                <div class="title">
                                    <strong>BIHAR SCHOOL EXAMINATION BOARD</strong>
                                </div>
                                <div class="sub-title">
                                    <strong>INTERMEDIATE REGISTRATION FORM (SESSION 2025-27)</strong>
                                    <br>

                                    <strong>DECLARATION FORM / घोषणा पत्र</strong>
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr class="double-line">

                    <p><u>1. PERSONAL DETAILS</u></p>
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td style="width: 75%; vertical-align: top; padding-right: 10px;">
                                <table class="student-details" style="width: 100%; font-size: 15px; border-collapse: collapse;">
                                    <tr>
                                        <td>+2 SCHOOL/COLLEGE CODE & NAME:</td>
                                        <td><%# Eval("CollegeName") %></td>
                                    </tr>
                                    <tr>
                                        <td>OFSS CAF NO:</td>
                                        <td><%# Eval("OfssReferenceNo") %></td>
                                    </tr>
                                    <tr>
                                        <td>BSEB UNIQUE ID:</td>
                                        <td><%# Eval("StudentUniqueId") %></td>
                                    </tr>
                                    <tr>
                                        <td>APAAR ID:</td>
                                        <td><%# Eval("ApaarId") %></td>
                                    </tr>
                                    <tr>
                                        <td>STUDENT NAME:</td>
                                        <td><%# Eval("StudentName") %></td>
                                    </tr>
                                    <tr>
                                        <td>MOTHER NAME:</td>
                                        <td><%# Eval("MotherName") %></td>
                                    </tr>
                                    <tr>
                                        <td>FATHER NAME:</td>
                                        <td><%# Eval("FatherName") %></td>
                                    </tr>
                                    <tr>
                                        <td>EMAIL:</td>
                                        <td><%# Eval("EmailId") %></td>
                                    </tr>
                                    <tr>
                                        <td>GENDER:</td>
                                        <td><%# Eval("Gender") %></td>
                                    </tr>
                                    <tr>
                                        <td>DATE OF BIRTH:</td>
                                        <td><%# Eval("DOB") %></td>
                                        <td>MOBILE NO:</td>
                                        <td><%# Eval("MobileNo") %></td>
                                    </tr>
                                    <tr>
                                        <td>AADHAR NO:</td>
                                        <td><%# Eval("AadharNo") %></td>
                                        <td>CASTE CATEGORY:</td>
                                        <td><%# Eval("Caste") %></td>
                                    </tr>
                                    <tr>
                                        <td>CATEGORY:</td>
                                        <td><%# Eval("Category") %></td>
                                        <td>RELIGION:</td>
                                        <td><%# Eval("Religion") %></td>
                                    </tr>
                                    <tr>
                                        <td>MARITAL STATUS:</td>
                                        <td><%# Eval("MaritalStatus") %></td>
                                        <td>AREA:</td>
                                        <td><%# Eval("Area") %></td>
                                    </tr>
                                    <tr>
                                        <td>MEDIUM:</td>
                                        <td><%# Eval("Medium") %></td>
                                        <td>NATIONALITY:</td>
                                        <td><%# Eval("Nationality") %></td>
                                    </tr>
                                    <tr>
                                        <td>DIFFERENTLY ABLED:</td>
                                        <td><%# Eval("DifferentlyAbled").ToString() == "1" ? "YES" : "NO" %></td>
                                    </tr>
                                    <tr>
                                        <td>IDENTIFICATION MARK 1:</td>
                                        <td colspan="3"><%# Eval("Ident1") %></td>
                                    </tr>
                                    <tr>
                                        <td>IDENTIFICATION MARK 2:</td>
                                        <td colspan="3"><%# Eval("Ident2") %></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 25%; text-align: center; vertical-align: top;">
                                <!-- Student Photo -->
                                <div style="border: 1px solid black; padding: 5px; display: inline-block;">
                                    <img src='<%# ResolveUrl(Eval("StudentPhotoPath").ToString()) %>'
                                        alt="Photo" width="160px" crossorigin="anonymous" />
                                </div>

                                <!-- Student Signature -->
                                <div style="margin-top: 10px;">
                                    <img src='<%# ResolveUrl(Eval("StudentSignaturePath").ToString()) %>'
                                        alt="Signature" width="180px" crossorigin="anonymous" />
                                </div>


                            </td>

                        </tr>
                    </table>

                    <hr class="double-line">

                    <p><u>2. ADDRESS DETAILS</u></p>
                    <table class="student-details" style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td>ADDRESS:</td>
                            <td><%# Eval("Address") %></td>
                        </tr>
                        <tr>
                            <td>TOWN/CITY:</td>
                            <td><%# Eval("Town") %></td>
                            <td>DISTRICT:</td>
                            <td><%# Eval("District") %></td>
                            <td>PINCODE:</td>
                            <td><%# Eval("PinCode") %></td>
                        </tr>
                    </table>

                    <hr class="double-line">

                    <p><u>3. BANK DETAILS</u></p>
                    <table class="student-details" style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td style="width: 75%; vertical-align: top; padding-right: 10px;">
                                <table style="width: 100%; font-size: 15px; border-collapse: collapse;">
                                    <tr>
                                        <td>BANK NAME:</td>
                                        <td><%# Eval("BankBranch") %></td>
                                    </tr>
                                    <tr>
                                        <td>STUDENT A/C NO.:</td>
                                        <td><%# Eval("AccountNo") %></td>
                                        <td>IFSC CODE:</td>
                                        <td><%# Eval("IFSC") %></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <hr class="double-line">

                    <!-- Subjects Offered -->
                    <p><u>4. SUBJECT DETAILS</u></p>
                    <table class="student-details" style="width: 100%; border-collapse: collapse;">
                        <!-- Compulsory Subjects -->
                        <tr>
                            <td colspan="4" style="font-weight: bold; background-color: #f2f2f2;">Compulsory Subjects
                            </td>
                        </tr>
                        <tr>
                            <td>Subject 1:</td>
                            <td><%# string.IsNullOrEmpty(Convert.ToString(Eval("Compulsory1"))) ? "-" : Convert.ToString(Eval("Compulsory1")) %></td>
                            <td>Subject 2:</td>
                            <td><%# string.IsNullOrEmpty(Convert.ToString(Eval("Compulsory2"))) ? "-" : Convert.ToString(Eval("Compulsory2")) %></td>
                        </tr>

                        <!-- Elective Subjects -->
                        <tr>
                            <td colspan="4" style="font-weight: bold; background-color: #f2f2f2;">Elective Subjects
                            </td>
                        </tr>
                        <tr>
                            <td>Subject 1:</td>
                            <td><%# string.IsNullOrEmpty(Convert.ToString(Eval("Elective1"))) ? "-" : Convert.ToString(Eval("Elective1")) %></td>
                            <td>Subject 2:</td>
                            <td><%# string.IsNullOrEmpty(Convert.ToString(Eval("Elective2"))) ? "-" : Convert.ToString(Eval("Elective2")) %></td>
                        </tr>
                        <tr>
                            <td>Subject 3:</td>
                            <td><%# string.IsNullOrEmpty(Convert.ToString(Eval("Elective3"))) ? "-" : Convert.ToString(Eval("Elective3")) %></td>
                            <td></td>
                            <td></td>
                        </tr>

                        <!-- Additional Subject -->
                        <tr>
                            <td colspan="4" style="font-weight: bold; background-color: #f2f2f2;">Additional Subject
                            </td>
                        </tr>
                        <tr>
                            <td>Subject:</td>
                            <td colspan="3"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Additional"))) ? "-" : Convert.ToString(Eval("Additional")) %></td>
                        </tr>

                        <!-- Vocational Subjects -->
                        <tr>
                            <td colspan="4" style="font-weight: bold; background-color: #f2f2f2;">Vocational Trade
                            </td>
                        </tr>
                        <tr>
                            <td>Subject:</td>
                            <td colspan="3"><%# string.IsNullOrEmpty(Convert.ToString(Eval("Vocational"))) ? "-" : Convert.ToString(Eval("Vocational")) %></td>
                        </tr>
                    </table>


                    <hr class="double-line">

                    <div class="box mb-3">
                        <div class="section-title">घोषणा (DECLARATION)</div>
                        <p>
                            प्रमाणित किया जाता है कि इस आवेदन पत्र में दी गई सूचनाएँ पूरी तरह से सही एवं शुद्ध हैं और इसमें कहीं पर भी किसी प्रकार के संशोधन की आवश्यकता नहीं है। जो भी सुधार एवं संशोधन थे, सब कर लिए गए हैं।
                        </p>
                        <div class="row text-center">
                            <div class="col-md-4">
                                <div class="sig-line"></div>
                                Signature Of Parent/Guardian
                            </div>
                            <div class="col-md-4">
                                <div class="sig-line"></div>
                                Student's Signature In Hindi
                            </div>
                            <div class="col-md-4">
                                <div class="sig-line"></div>
                                Student's Signature In English
                            </div>
                        </div>
                    </div>

                    <div class="box mb-3">
                        <div class="row text-center">
                            <div class="col-md-7">
                                <p>
                                    प्रमाणित किया जाता है कि ऊपर दिए गए सभी विवरणी का मिलान विद्यालय के सभी अभिलेखों से पूर्णरूपेण कर लिया गया है। तदनुसार उक्त परीक्षार्थी का पंजीयन आवेदन पत्र स्वीकार किया जाए।
                                </p>
                            </div>
                            <div class="col-md-5">
                                <div class="justify-content-end mt-3 row text-center">
                                    <div class="col-md-10">
                                        <div class="sig-line"></div>
                                        Signature & Seal Of Principal
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div style="page-break-after: always;"></div>
            </ItemTemplate>
        </asp:Repeater>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
    <script>
        async function generatePDF() {
            const { jsPDF } = window.jspdf;
            const pdf = new jsPDF('p', 'mm', 'a4');

            // Get all student divs
            const admitCards = document.querySelectorAll('.admitCardItem');

            for (let i = 0; i < admitCards.length; i++) {
                const element = admitCards[i];

                const canvas = await html2canvas(element, {
                    scale: 2,
                    useCORS: true
                });

                const imgData = canvas.toDataURL('image/jpeg', 1.0);
                const imgProps = pdf.getImageProperties(imgData);
                const pdfWidth = pdf.internal.pageSize.getWidth();
                const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

                if (i > 0) pdf.addPage(); // Add new page for each student except first one
                pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);
            }

            // ✅ Save combined PDF
            pdf.save('DeclarationformPDF.pdf');

            // ✅ Get studentId & categoryType from query string
            const urlParams = new URLSearchParams(window.location.search);
            const studentIds = urlParams.get('studentIds');
            const categoryType = urlParams.get('CategoryType');

            if (!studentIds) {
                alert("StudentId not found in query string.");
                return;
            }

            // ✅ Call server-side WebMethod via fetch
            fetch("DECLARATIONFORM.aspx/MarkDeclarationFormDownloaded", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify({ studentIds: studentIds })
            })
                .then(response => response.json())
                .then(result => {
                    console.log("Server result:", result);
                    if (result.d === true) {
                        let redirectUrl = '';
                        if (categoryType === 'Private') {
                            redirectUrl = 'registerPrivate.aspx';
                        } else if (categoryType === 'Regular') {
                            redirectUrl = 'register_27.aspx';
                        } else {
                            redirectUrl = 'ManageDeclarationForm.aspx';
                        }
                        window.location.href = redirectUrl;
                    } else {
                        alert("Error updating DeclarationFormDownloaded status.");
                    }
                });
        }

    </script>
</body>
</html>
