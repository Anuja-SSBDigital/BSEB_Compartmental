<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demoregcertificate.aspx.cs" Inherits="Demoregcertificate" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Admit Card</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .container {
            padding: 80px;
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

        .table-details {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

            .table-details th, .table-details td {
                border: 1px solid black;
                padding: 8px;
                text-align: center;
            }

        .instructions {
            margin-top: 20px;
            font-size: 20px;
        }

        .signature {
            text-align: right;
            margin-top: 30px;
        }


        .subjects-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

            .subjects-table th, .subjects-table td {
                border: 1px solid #000;
                padding: 5px;
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

            .subjects-table td {
                vertical-align: middle;
            }

        .box {
            border: 1px solid #000;
            padding: 10px;
        }

        .sig-line {
            border: 1px solid #000;
            height: 40px;
            margin-bottom: 5px;
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

        @media print {
            body {
                margin: 0;
                padding: 0;
                font-size: 30px;
                -webkit-print-color-adjust: exact !important;
                print-color-adjust: exact !important;
                color-adjust: exact !important;
            }

            @page {
                size: A4 portrait;
                margin: 10mm;
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
                border: none; /* Reset border */
                border-top: 2px solid #000; /* Set black top border */
                opacity: 1;
            }
        }
    </style>
</head>
<body>
    <div class="container" id="admitCard">
        <!-- Header Section -->
        <div class="header">
            <div class="row">
                <div class="col-md-3">
                    <img src="assets/img/bsebimage.jpg" alt="Bihar Board Logo" class="logo">
                </div>
                <div class="col-md-6">
                    <div class="title">
                        <strong>बिहार विद्यालय परीक्षा समिति, पटना</strong><br>
                        <strong>BIHAR SCHOOL EXAMINATION BOARD, PATNA</strong>
                    </div>
                    <div class="sub-title">
                        <strong>छात्र सूचीकरण प्रमाण-पत्र</strong>
                        <br>
                        <strong>DUMMY REGISTRATION CERTIFICATE</strong>
                        <br>
                        <strong>इंटरमीडिएट सत्र 2023-25</strong>
                        <br />
                        <strong>INTERMEDIATE SESSION 2023-25</strong>
                    </div>
                </div>

            </div>
        </div>

        <div class="borderline">
            <table style="width: 100%; border-collapse: collapse;">
                <tr>
                    <!-- Left side info -->
                    <td style="width: 75%; vertical-align: top; padding-right: 10px;">
                        <table style="width: 100%; font-size: 17px; border-collapse: collapse;">

                            <tr>
                                <td style="padding: 3px 5px;"><strong>विद्यार्थी का नाम:</strong></td>
                                <td style="padding: 3px 5px;">NIKKI KUMARI</td>
                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>माता का नाम:</strong></td>
                                <td style="padding: 3px 5px;">INDU DEVI</td>
                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>पिता का नाम:</strong></td>
                                <td style="padding: 3px 5px;">MADAN LAL GUPTA</td>
                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>लिंग:</strong></td>
                                <td style="padding: 3px 5px;">FEMALE</td>
                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>श्रेणी:</strong></td>
                                <td style="padding: 3px 5px;">REGULAR</td>
                                <td style="padding: 3px 5px;"><strong>जाति:</strong></td>
                                <td style="padding: 3px 5px;">MN</td>
                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>राष्ट्रीयता:</strong></td>
                                <td style="padding: 3px 5px;">INDIAN</td>
                                <td style="padding: 3px 5px;"><strong>धर्म:</strong></td>
                                <td style="padding: 3px 5px;">INDIAN</td>
                            </tr>

                            <tr>
                                <td style="padding: 3px 5px;"><strong>सूचीकरण संख्या/वर्ष:</strong></td>
                                <td style="padding: 3px 5px;">R-110091882-22</td>
                                <td style="padding: 3px 5px;"><strong>दिव्यांग कोटि:</strong></td>
                                <td style="padding: 3px 5px;">NO</td>
                            </tr>
                            <!-- Two-column layout -->


                            <tr>
                                <td style="padding: 3px 5px;"><strong>संकाय :</strong></td>
                                <td style="padding: 3px 5px;">SCIENCE</td>
                                <td style="padding: 3px 5px;"><strong>वैवाहिक स्थिति:</strong></td>
                                <td style="padding: 3px 5px;">Unmarried</td>

                            </tr>
                            <tr>
                                <td style="padding: 3px 5px;"><strong>परीक्षा केंद्र का नाम:</strong></td>
                                <td style="padding: 3px 5px;" colspan="5">GOVT. BOY'S HIGH SCHOOL RAJENDRA NAGAR, PATNA</td>
                            </tr>
                        </table>
                    </td>

                    <!-- Right side: photo -->
                    <td style="width: 25%; text-align: center; vertical-align: top;">
                        <div style="border: 1px solid black; padding: 5px; display: inline-block;">
                            <img src="assets/img/users/user-5.png" alt="Student Photo" style="width: 100%; max-width: 160px; height: auto;">
                        </div>
                        <div style="margin-top: 10px;">
                            <img src="assets/img/logo_v1.png" alt="Signature" style="width: 100%; max-width: 180px; height: auto;">
                        </div>
                    </td>
                </tr>
            </table>


            <table class="subjects-table">
                <thead>
                    <tr>
                        <th colspan="3">अनिवार्य विषय<br>
                            (Compulsory Subjects)</th>
                        <th colspan="3">ऐच्छिक विषय<br>
                            (Elective Subjects)</th>
                        <th colspan="2">अतिरिक्त विषय<br>
                            (Additional Subject)</th>
                    </tr>
                    <tr>
                        <th>भाषा विषय</th>
                        <th>विषय कोड</th>
                        <th>विषय का नाम</th>
                        <th>ऐच्छिक विषय</th>
                        <th>विषय कोड</th>
                        <th>विषय का नाम</th>
                        <th>विषय कोड</th>
                        <th>विषय का नाम</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>भाषा विषय-1</td>
                        <td>106</td>
                        <td>HINDI</td>
                        <td>ऐच्छिक विषय-1</td>
                        <td>117</td>
                        <td>PHYSICS</td>
                        <td rowspan="3">119</td>
                        <td rowspan="3">BIOLOGY</td>
                    </tr>
                    <tr>
                        <td>भाषा विषय-2</td>
                        <td>105</td>
                        <td>ENGLISH</td>
                        <td>ऐच्छिक विषय-2</td>
                        <td>118</td>
                        <td>CHEMISTRY</td>
                    </tr>
                    <tr>
                        <td>भाषा विषय-3</td>
                        <td>121</td>
                        <td>MATHEMATICS</td>
                        <td>ऐच्छिक विषय-3</td>
                        <td>121</td>
                        <td>MATHEMATICS</td>
                    </tr>
                </tbody>
            </table>
            <div class=" mb-3">
                <h6 class="text-center"><strong>विद्यार्थी/शिक्षण संस्थान के प्रधान के लिए आवश्यक निर्देश :</strong></h6>
                <ol>
                    <li>डमी सूचीकरण प्रमाण पत्र किसी भी शैक्षणिक गतिविधि के लिए अनुमेय नहीं होगा।</li>
                    <li>मूल सूचीकरण प्रमाण पत्र जारी होने के बाद किसी भी प्रकार की त्रुटि का सुधार नहीं किया जाएगा।</li>
                    <li>नीचे मुद्रित घोषणा-पत्र को विद्यार्थी व उसके माता/पिता/अभिभावक तथा शिक्षण संस्थान के प्रधान के द्वारा अनिवार्य रूप से हस्ताक्षरित किया जाएगा।</li>
                </ol>
            </div>

            <div class="box mb-3">
                <div class="section-title">घोषणा-पत्र</div>
                <p>“यह प्रमाणित किया जाता है कि डमी सूचीकरण प्रमाण पत्र के उपर्युक्त विवरणों में कोई त्रुटि नहीं है, सभी विवरण सही हैं।”</p>

                <div class="row text-center">
                    <div class="col-md-6">
                        <div class="sig-line"></div>
                        विद्यार्थी का हस्ताक्षर ↑
                    </div>
                    <div class="col-md-6">
                        <div class="sig-line"></div>
                        माता/पिता/अभिभावक का हस्ताक्षर ↑
                    </div>
                </div>
                <div class="row text-center mt-2">
                    <div class="col-md-6">दिनांक:</div>
                    <div class="col-md-6">दिनांक:</div>
                </div>

                <hr class="my-3" style="border: 1px dotted black;">

                <p>
                    “यह प्रमाणित किया जाता है कि विद्यार्थी द्वारा डमी सूचीकरण प्रमाण-पत्र के त्रुटिपूर्ण विवरण/विवरणों में किए गए सुधार, यदि कोई किया गया हो, 
        तो, के आलोक में संस्थान के अभिलेख से मिलानोपरांत समिति के पोर्टल पर ऑनलाइन माध्यम से अपेक्षित संशोधन किया जा चुका है तथा 
        इस डमी सूचीकरण प्रमाण पत्र में मुद्रित सभी विवरण उक्त के अनुसार व सही हैं।”
                </p>

                <div class="justify-content-end mt-3 row text-center">
                    <div class="col-md-6">
                        <div class="sig-line mx-auto" style="width: 50%"></div>
                        संस्थान प्रधान का हस्ताक्षर ↑
            <div>दिनांक:</div>
                    </div>

                </div>
            </div>

            <div class="text-center mt-4 mb-5">
                <button onclick="generatePDF()" class="btn btn-primary no-print ">Download PDF</button>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
    <script>
        async function generatePDF() {
            const { jsPDF } = window.jspdf;
            const pdf = new jsPDF('p', 'mm', 'a4');
            const element = document.getElementById('admitCard');

            const canvas = await html2canvas(element, {
                scale: 2,
                useCORS: true
            });

            const imgData = canvas.toDataURL('image/jpeg', 1.0);
            const imgProps = pdf.getImageProperties(imgData);
            const pdfWidth = pdf.internal.pageSize.getWidth();
            const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

            // If content height exceeds one page
            let heightLeft = pdfHeight;
            let position = 0;

            pdf.addImage(imgData, 'JPEG', 0, position, pdfWidth, pdfHeight);
            heightLeft -= pdf.internal.pageSize.getHeight();

            while (heightLeft > 0) {
                position = heightLeft - pdfHeight;
                pdf.addPage();
                pdf.addImage(imgData, 'JPEG', 0, position, pdfWidth, pdfHeight);
                heightLeft -= pdf.internal.pageSize.getHeight();
            }

            pdf.save('AdmitCard.pdf');
        }
    </script>
</body>
</html>

