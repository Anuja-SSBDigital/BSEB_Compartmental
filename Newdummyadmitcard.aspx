<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Newdummyadmitcard.aspx.cs" Inherits="Newdummyadmitcard" %>

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
                border: none !important;
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

        .subjects-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
            line-height: 1.4;
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

        .list-item {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <div class="container" id="admitCard">
        <!-- Header Section -->
        <div class="header">
            <div class="row">
                <div class="col-md-3">
                    <img src="assets/img/bsebimage.jpg" alt="Bihar Board Logo" class="logo float-lg-start">
                </div>
                <div class="col-md-6 align-content-center h5">
                    <div class="title">
                        <strong>BIHAR SCHOOL EXAMINATION BOARD</strong>
                    </div>
                    <div class="sub-title">
                        <strong>INTERMEDIATE ANNUAL EXAMINATION, 2025</strong>
                        <br>
                        <strong><u>SECOND DUMMY ADMIT CARD</u></strong>
                        <br>
                        <strong>सैद्धान्तिक परीक्षा का प्रवेश-पत्र</strong>
                        <br />

                    </div>
                </div>
                <div class="col-md-3">

                    <label><strong>संकाय:</strong> कला</label><br>
                    <label><strong>FACULTY:</strong> SCIENCE</label>
                </div>
            </div>
        </div>

        <!-- Student Details Section -->
        <%--<div class="row">
            <div class="col-md-9">
                <p>* BSEB UNIQUE Id:- <strong>2212260090047</strong> </p>
                <p><strong>कॉलेज/+2 स्कूल का नाम:</strong> COLLEGE OF COMMERCE, ARTS & SCIENCE, PATNA</p>
                <p><strong>परीक्षार्थी का नाम:</strong> ANJALI KUMARI</p>
                <p><strong>माता का नाम:</strong> SHEELA DEVI</p>
                <p><strong>पिता का नाम:</strong> AJAY CHOUDHARY</p>
                <p><strong>वैवाहिक स्थिति:</strong> Unmarried</p>

                <div class="row">
                    <div class="col-md-6">
                        <p><strong>परीक्षार्थी का आधार नं०:</strong> 936940683491</p>
                        <p><strong>सूचीकरण संख्या/वर्ष:</strong> R-110090014-21</p>
                        <p><strong>रौल कोड:</strong> 11009</p>
                        <p><strong>दिव्यांग कोटि:</strong> NO</p>

                    </div>
                    <div class="col-md-6">
                        <p><strong>परीक्षार्थी की कोटि:</strong> COMPARTMENTAL</p>
                        <p><strong>रौल क्रमांक:</strong> 23310014</p>
                        <p><strong>लिंग:</strong> FEMALE</p>
                    </div>
                </div>
                <p><strong>परीक्षा केंद्र का नाम:</strong> GOVT. BOY'S HIGH SCHOOL RAJENDRA NAGAR, PATNA</p>
            </div>
            <div class="col-md-3">
                <div style="border: 1px solid black; display: inline-block; padding: 5px;">
                    <img src="assets/img/users/user-5.png" alt="Student Photo" style="width: 200px; height: auto;"><br>
                </div>
                <div class="mt-2">
                    <img src="assets/img/ss.png" alt="Signature" style="width: 212px; height: auto;">
                </div>
            </div>
        </div>--%>

        <table style="width: 100%; border-collapse: collapse; font-size: 16px;">
            <tr>
                <!-- Left side: Student Info -->
                <td style="width: 75%; vertical-align: top; padding-right: 10px;">
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td><strong>* BSEB UNIQUE Id:</strong></td>
                            <td>2222260091880</td>
                        </tr>
                        <tr>
                            <td><strong>कॉलेज/+2 स्कूल का नाम:</strong></td>
                            <td>COLLEGE OF COMMERCE, ARTS & SCIENCE, PATNA</td>
                        </tr>
                        <tr>
                            <td><strong>परीक्षार्थी का नाम:</strong></td>
                            <td>NIKKI KUMARI</td>
                        </tr>
                        <tr>
                            <td><strong>माता का नाम:</strong></td>
                            <td>INDU DEVI</td>
                        </tr>
                        <tr>
                            <td><strong>पिता का नाम:</strong></td>
                            <td>MADAN LAL GUPTA</td>
                        </tr>
                        <tr>
                            <td><strong>वैवाहिक स्थिति:</strong></td>
                            <td>Unmarried</td>
                        </tr>
                        <tr>
                            <td><strong>परीक्षार्थी का आधार नं:</strong></td>
                            <td>656429986643</td>
                        </tr>
                        <tr>
                            <td><strong>दिव्यांग कोटि:</strong></td>
                            <td>NO</td>
                        </tr>
                        <tr>
                            <td><strong>सूचीकरण संख्या/वर्ष:</strong></td>
                            <td>R-110091882-22</td>
                        </tr>
                        <tr>
                            <td><strong>परीक्षार्थी की कोटि:</strong></td>
                            <td>COMPARTMENTAL</td>
                        </tr>
                        <tr>
                            <td><strong>रौल कोड:</strong></td>
                            <td>11009</td>
                        </tr>
                        <tr>
                            <td><strong>रौल क्रमांक:</strong></td>
                            <td>24130059</td>
                        </tr>
                        <tr>
                            <td><strong>लिंग:</strong></td>
                            <td>FEMALE</td>
                        </tr>
                        <tr>
                            <td><strong>परीक्षा केंद्र का नाम:</strong></td>
                            <td>GOVT. BOY'S HIGH SCHOOL RAJENDRA NAGAR, PATNA</td>
                        </tr>
                    </table>
                </td>

                <!-- Right side: Photo and Signature -->
                <td style="width: 25%; text-align: center; vertical-align: top;">
                    <div style="border: 1px solid black; display: inline-block;">
                        <img src="assets/img/users/user-5.png" alt="Student Photo" style="width: 100%; max-width: 140px; height: auto;">
                    </div>
                    <div style="margin-top: 10px;">
                        <img src="assets/img/logo_v1.png" alt="Signature" style="width: 100%; max-width: 160px; height: auto;">
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
                    <%--<th colspan="2">व्यावसायिक ट्रेड<br>
                                    (Vocational Trade)</th>--%>
                    <asp:PlaceHolder ID="phVocationalHeader" runat="server" Visible='<%# Eval("HasVocationalSubjects") %>'>
                        <th colspan="2">व्यावसायिक ट्रेड<br>
                            (Vocational Trade)</th>
                    </asp:PlaceHolder>

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
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("HasVocationalSubjects") %>'>
                        <th>विषय कोड</th>
                        <th>विषय का नाम</th>
                    </asp:PlaceHolder>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><b>भाषा विषय-1</b></td>
                    <td><%# Eval("CompulsorySubject1Code") %></td>
                    <td><%# Eval("CompulsorySubject1Name") %></td>
                    <td><b>ऐच्छिक विषय-1</b></td>
                    <td><%# Eval("ElectiveSubject1Code") %></td>
                    <td><%# Eval("ElectiveSubject1Name") %></td>
                    <td rowspan="3"><%# Eval("AdditionalSubjectCode") %></td>
                    <td rowspan="3"><%# Eval("AdditionalSubjectName") %></td>
                    <%--    <td rowspan="3"><%# Eval("VocationalSubjectCode1Code") %></td>
                                <td rowspan="3"><%# Eval("VocationalSubjectName1Name") %></td>--%>
                    <asp:PlaceHolder ID="phVocational" runat="server" Visible='<%# Convert.ToBoolean(Eval("HasVocationalSubjects")) %>'>
                        <td rowspan="3"><%# Eval("VocationalSubjectCode1Code") %></td>
                        <td rowspan="3"><%# Eval("VocationalSubjectName1Name") %></td>

                    </asp:PlaceHolder>
                </tr>
                <tr>
                    <td><b>भाषा विषय-2</b></td>
                    <td><%# Eval("CompulsorySubject2Code") %></td>
                    <td><%# Eval("CompulsorySubject2Name") %></td>
                    <td><b>ऐच्छिक विषय-2</b></td>
                    <td><%# Eval("ElectiveSubject2Code") %></td>
                    <td><%# Eval("ElectiveSubject2Name") %></td>
                </tr>

                <tr>
                    <td colspan="3"></td>
                    <%-- <td><%# Eval("CompulsorySubject3Code") %></td>
                                <td><%# Eval("CompulsorySubject3Name") %></td>--%>
                    <td><b>ऐच्छिक विषय-3</b></td>
                    <td><%# Eval("ElectiveSubject3Code") %></td>
                    <td><%# Eval("ElectiveSubject3Name") %></td>
                </tr>
            </tbody>
        </table>

        <div class="" style="font-family: 'Noto Sans Devanagari', 'Mangal', 'Arial', sans-serif; font-size: 17px; margin-top: 65px;">

            <!-- Signature block -->
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>महाविद्यालय / +2 विद्यालय प्रधान का हस्ताक्षर</strong><br>
                    <em>एवं मुहर</em>
                </div>
                <div class="col-md-6 text-end">
                    <strong>परीक्षा नियंत्रक (COMITO)</strong>
                </div>
            </div>
            <%--<hr style="border-top:var(--bs-border-width) solid black;opacity: 1;border: 2px;" />--%>
            <hr style="font-size: 16px; border: 2px solid black !important; opacity: 1.25 !important;" />
            <!-- Heading -->
            <h5 class="text-center mb-3"><strong>परीक्षार्थी के लिए आवश्यक निर्देश</strong></h5>
            <ol>
                <li class="list-item">डमी प्रवेश-पत्र में यदि किसी विद्यार्थी के नाम, माता/पिता के नाम के स्पेलिंग में त्रुटि हो, वैवाहिक स्थिति, कोटि, लिंग, विषय, फोटो या हस्ताक्षर आदि में किसी प्रकार की त्रुटि परिलक्षित होती है, तो उससे संबंधित साक्ष्य एवं अपने हस्ताक्षर के साथ डमी प्रवेश-पत्र में संशोधन कर ऑनलाइन सुधार हेतु दिनांक 12-12-2024 तक अपने +2 विद्यालय/महाविद्यालय प्रधान को हस्तगत कराना सुनिश्चित करेंगे तथा डमी प्रवेश-पत्र की दूसरी प्रति शिक्षण संस्थान के प्रधान का हस्ताक्षर एवं मुहर प्राप्त कर अपने पास सुरक्षित रख लेंगे।
                </li>
                <li class="list-item">संबंधित +2 विद्यालय/महाविद्यालय प्रधान दिनांक 12-12-2024 तक की अवधि में विद्यार्थी के द्वारा डमी प्रवेश-पत्र में प्रतिवेदित त्रुटि का ऑनलाइन सुधार अनिवार्य रूप से करना सुनिश्चित करेंगे।
                </li>
                <li class="list-item">डमी प्रवेश-पत्र में विद्यार्थी का रौल नम्बर, परीक्षा केन्द्र का नाम तथा परीक्षा की तिथि अंकित नहीं किया गया है। मूल प्रवेश-पत्र में इसे जारी किया जाएगा।
                </li>
                <li class="list-item">इन्टरमीडिएट परीक्षा सत्र 2023-25 के नियमित कोटि के विद्यार्थियों के परीक्षा का विषय उनके सूचीकरण प्रमाण-पत्र के आधार पर अंकित किया गया है।
                </li>
                <li class="list-item">इसी प्रकार पूर्व के सत्रों में सूचीकृत वैसे पूर्ववर्ती विद्यार्थी, जो अभी तक इन्टरमीडिएट परीक्षा में अनुत्तीर्ण हैं अथवा सूचीकृत होने के उपरांत किसी कारणवश परीक्षा आवेदन नहीं भर पाये या परीक्षा में सम्मिलित नहीं हो पाये, उनके परीक्षा का विषय सूचीकरण प्रमाण-पत्र के आधार पर ही अंकित किया गया है।
                </li>
                <li class="list-item">समुन्नत कोटि के विद्यार्थियों के परीक्षा का विषय वही अंकित किया गया है, जिस उत्तीर्ण विषय/विषयों के परीक्षाफल को बेहतर किये जाने हेतु (For Improvement) उनके द्वारा आवेदन किया गया है।
                </li>
                <li class="list-item">कम्पार्टमेन्टल कोटि के पात्र परीक्षार्थियों का इंटरमीडिएट वार्षिक परीक्षा, 2025 के लिए उन्हीं विषय/विषयों (अधिकतम 02) को अंकित किया गया है, जिसमें वे अभी तक अनुत्तीर्ण हैं।
                </li>
                <li class="list-item">यह डमी प्रवेश-पत्र है, जिसके आधार पर परीक्षार्थी का परीक्षा केन्द्र में प्रवेश मान्य नहीं है।

                </li>
            </ol>
            <!-- Instructions -->

        </div>
        <div class="text-center mt-4 mb-5">
            <button onclick="generatePDF(this)" class="btn btn-primary">Download PDF</button>
        </div>

    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

    <%-- <script>
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

           // Dimensions for the student photo in the PDF (in mm)
           const photoWidthInPDF = 60; // Adjust as needed
           const photoHeightInPDF = 50; // Adjust as needed
           const photoXPosition = 170; // Adjust the X coordinate
           const photoYPosition = 30;  // Adjust the Y coordinate

           // Dimensions for the signature in the PDF (in mm)
           const signatureWidthInPDF = 40; // Adjust as needed
           const signatureHeightInPDF = 15; // Adjust as needed
           const signatureXPosition = 160; // Adjust the X coordinate
           const signatureYPosition = 75;  // Adjust the Y coordinate

           // Add the entire canvas (admit card)
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

           // Function to get the image data for a specific element
           async function getImageData(selector) {
               const imageElement = element.querySelector(selector);
               if (!imageElement) return null;
               const imageCanvas = await html2canvas(imageElement, { useCORS: true });
               return imageCanvas.toDataURL('image/png');
           }

           // Get and add the student photo
           const photoData = await getImageData('div > img[alt="Student Photo"]');
           if (photoData) {
               pdf.addImage(photoData, 'PNG', photoXPosition, photoYPosition, photoWidthInPDF, photoHeightInPDF);
           }

           // Get and add the signature
           const signatureData = await getImageData('div > img[alt="Signature"]');
           if (signatureData) {
               pdf.addImage(signatureData, 'PNG', signatureXPosition, signatureYPosition, signatureWidthInPDF, signatureHeightInPDF);
           }

           pdf.save('AdmitCard.pdf');
       }
   </script>--%>
    <script type="text/javascript">
        //async function generatePDF(button) {

        //    button.style.display = 'none'; // Hide the button during export
        //    const { jsPDF } = window.jspdf;
        //    const pdf = new jsPDF('p', 'mm', 'a4');
        //    const element = document.getElementById('admitCard');

        //    const canvas = await html2canvas(element, {
        //        scale: 2,
        //        useCORS: true
        //    });

        //    const imgData = canvas.toDataURL('image/jpeg', 1.0);
        //    const imgProps = pdf.getImageProperties(imgData);
        //    const pdfWidth = pdf.internal.pageSize.getWidth();
        //    const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

        //    // If content height exceeds one page
        //    let heightLeft = pdfHeight;
        //    let position = 0;

        //    pdf.addImage(imgData, 'JPEG', 0, position, pdfWidth, pdfHeight);
        //    heightLeft -= pdf.internal.pageSize.getHeight();

        //    while (heightLeft > 0) {
        //        position = heightLeft - pdfHeight;
        //        pdf.addPage();
        //        pdf.addImage(imgData, 'JPEG', 0, position, pdfWidth, pdfHeight);
        //        heightLeft -= pdf.internal.pageSize.getHeight();
        //    }

        //    pdf.save('AdmitCard.pdf');
        //    button.style.display = 'inline-block'; // Optional: Show again

        //}
        async function generatePDF(button) {
            button.style.display = 'none'; // Hide the button during export

            const { jsPDF } = window.jspdf;
            const pdf = new jsPDF('p', 'mm', 'a4');

            const element = document.getElementById('admitCard');

            // Create high-res canvas
            const canvas = await html2canvas(element, {
                scale: 2,
                useCORS: true
            });

            const imgData = canvas.toDataURL('image/jpeg', 1.0);

            // Calculate dimensions
            const pdfWidth = pdf.internal.pageSize.getWidth();
            const pdfHeight = pdf.internal.pageSize.getHeight();

            // Draw image to full page only (no pagination)
            pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);

            pdf.save('AdmitCard.pdf');

            button.style.display = 'inline-block'; // Optional: Show again
        }

    </script>
</body>
</html>