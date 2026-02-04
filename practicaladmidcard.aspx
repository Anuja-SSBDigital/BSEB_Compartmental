<%@ Page Language="C#" AutoEventWireup="true" CodeFile="practicaladmidcard.aspx.cs" Inherits="practicaladmidcard" %>

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

        ol {
            list-style-position: inside;
            list-style-type: decimal;
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
            font-size: 17px;
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

            ol {
                list-style-type: decimal !important;
                list-style-position: inside !important;
            }

            li {
                display: list-item !important;
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
        }
    </style>
</head>
<body oncontextmenu="return false;" onkeydown="return preventKeys(event)">
    <div class="container" id="admitCard">
        <!-- Header Section -->
        <div class="header">
            <div class="row">
                <div class="col-md-3">
                    <img src="assets/img/bsebimage.jpg" alt="Bihar Board Logo" class="logo">
                </div>
                <div class="col-md-6">
                    <div class="title">
                        <strong>बिहार विद्यालय परीक्षा समिति</strong><br>
                        <strong>BIHAR SCHOOL EXAMINATION BOARD </strong>
                    </div>
                    <div class="sub-title">
                        <strong>इन्टरमीडिएट कम्पार्टमेन्टस परीक्षा 2026</strong>
                        <br>
                        <strong>INTERMEDIATE COMPARTMENTAL EXAMINATION, 2026 </strong>
                        <br>
                        <strong>प्रायोगिक परीक्षा का प्रवेश-पत्र</strong>
                        <br />
                        <strong>Admit Card of Practical Examination </strong>
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

        <table style="width: 100%; border-collapse: collapse;">
            <tr>
                <!-- Left Side: Student Details -->
                <td style="width: 80%; vertical-align: top; padding-right: 10px;">
                    <table style="width: 100%; font-size: 14px; border-collapse: collapse;">
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
                            <td><strong>दिव्यांग कोटि:</strong></td>
                            <td>NO</td>
                        </tr>
                        <tr>
                            <td><strong>सूचीकरण संख्या/वर्ष:</strong></td>
                            <td>R-110091882-22</td>
                            <td><strong>परीक्षार्थी की कोटि:</strong></td>
                            <td>COMPARTMENTAL</td>
                        </tr>
                        <tr>
                            <td><strong>रौल कोड:</strong></td>
                            <td>11009</td>
                            <td><strong>रौल क्रमांक:</strong></td>
                            <td>24130059</td>
                            <td><strong>लिंग:</strong></td>
                            <td>FEMALE</td>
                        </tr>
                        <tr>
                            <td><strong>परीक्षा केंद्र का नाम:</strong></td>
                            <td colspan="5">GOVT. BOY'S HIGH SCHOOL RAJENDRA NAGAR, PATNA</td>
                        </tr>
                    </table>
                </td>

                <!-- Right Side: Photo and Signature -->
                <td style="width: 20%; text-align: center; vertical-align: top;">
                    <div style="border: 1px solid black; padding: 5px; display: inline-block;">
                        <img src="assets/img/users/user-5.png" alt="Student Photo" style="width: 100%; max-width: 160px; height: auto;">
                    </div>
                    <div style="margin-top: 10px;">
                        <img src="assets/img/ss.png" alt="Signature" style="width: 100%; max-width: 180px; height: auto;">
                    </div>
                </td>
            </tr>
        </table>


        <!-- Examination Table -->
        <h3 style="font-size: 14px; font-weight: bold; margin-bottom: 5px; margin-top: 12px;">प्रायोगिक परीक्षा के विषय (निर्धारित परीक्षा कार्यक्रम सहित)
        </h3>
        <%--<table class="table table-bordered text-center align-middle" style="font-size: 16px; border: 2px solid #000;">

            <thead>
                <tr>
                    <th>प्रायोगिक विषय</th>
                    <th>विषय कोड</th>
                    <th>विषय का नाम</th>
                    <th>परीक्षा की तिथि</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="2">वैकल्पिक विषय</td>
                    <td>117</td>
                    <td>PHYSICS</td>
                    <td rowspan="3">15.05.2024<br>
                        to<br>
                        16.05.2024
                    </td>
                </tr>
                <tr>
                    <td>118</td>
                    <td>CHEMISTRY</td>
                </tr>
                <tr>
                    <td>अतिरिक्त विषय</td>
                    <td colspan="2"></td>
                </tr>
            </tbody>
        </table>--%>

        <table class="table table-bordered text-center align-middle" style="font-size: 16px; border: 2px solid #000;">
            <thead>
                <tr>
                    <th>प्रायोगिक विषय</th>
                    <th>विषय कोड</th>
                    <th>विषय का नाम</th>
                    <th>परीक्षा की तारीख</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="2" style="border: none">वैकल्पिक विषय</td>
                    <td>117</td>
                    <td>PHYSICS</td>
                    <td rowspan="3">15.05.2024<br>
                        to<br>
                        16.05.2024</td>
                </tr>
                <tr>
                    <td>118</td>
                    <td>CHEMISTRY</td>
                </tr>
                <tr>
                    <td>अतिरिक्त विषय</td>
                    <td colspan="2"></td>
                </tr>
            </tbody>
        </table>

        <div class="" style="font-family: 'Noto Sans Devanagari', 'Mangal', 'Arial', sans-serif; font-size: 17px; margin-top:65px;">

            <!-- Signature block -->
            <div class="row ">
                <div class="col-md-6">
                    <strong>महाविद्यालय / +2 विद्यालय प्रधान का हस्ताक्षर</strong><br>
                   <center> <em>एवं मुहर</em></center>
                </div>
                <div class="col-md-6 text-end">
                    <strong>परीक्षा नियंत्रक (COMITO)</strong>
                </div>
            </div>
            <hr />
            <!-- Heading -->
            <h5 class="text-center mb-3"><strong>परीक्षार्थी के लिए आवश्यक निर्देश</strong></h5>

            <!-- Instructions -->
            <ol style="font-size: small;">

                <li>प्रायोगिक परीक्षा दिनांक 15.05.2024 से 16.05.2024 तक केन्द्राधीक्षकों द्वारा आयोजित की जायेगी। केन्द्राधीक्षक दिनांक 15.05.2024 से 16.05.2024 तक की अवधि में परीक्षार्थियों की संख्या के अनुसार विभिन्न पाली का निर्धारण करके प्रायोगिक परीक्षा केन्द्र पर आयोजित करेंगे तथा प्रायोगिक विषयों की परीक्षा आयोजित करेंगे।</li>
                <li>परीक्षार्थी अपने प्रवेश-पत्र में उल्लिखित प्रायोगिक परीक्षा केन्द्र पर दिनांक 15.05.2024 को प्रातः 09:00 बजे अनिवार्य रूप से जाकर परीक्षा केन्द्र के परिसर की सूचना पट्ट से यह जानकारी प्राप्त कर लें कि उनके द्वारा प्रविष्ट विषय की प्रायोगिक परीक्षा किस तिथि एवं किस पाली में संचालित होगी, जिसमें उन्हें प्रतिभागिता होना आवश्यक है।</li>
                <li>परीक्षार्थी के प्रत्येक प्रायोगिक विषय की परीक्षा के लिए 08 पृष्ठ की केवल एक ही उत्तरपुस्तिका निर्गत/अवस्थित की जायेगी। परीक्षाओं उत्तरपुस्तिका लेनी है यह सुनिश्चित कर लें कि इसमें 8 पृष्ठ हैं एवं सही क्रम में हैं।</li>
                <li>उत्तरपुस्तिका प्राप्त होने ही परीक्षार्थी अपने प्रवेश-पत्र एवं उत्तरपुस्तिका पर प्रिंटेड विवरण (Details) का मिलान कर यह अवश्य सुनिश्चित हो लें कि जो उत्तरपुस्तिका परीक्षा द्वारा उसे दी गई है, वह उन्हीं की है। मिलान विवरण सही पाए जाने पर उसे संबंधित परीक्षक को प्राप्त करवा दिया जाए।</li>
                <li>उत्तरपुस्तिका प्राप्त होने पर परीक्षार्थी उनके आवरण पृष्ठ के शीर्ष “प्रश्नों का उत्तर लिखें” के लिए विवक्षित स्थान छोड़ें एवं उसके अनुकूल उत्तर लिखना प्रारंभ करें।</li>
                <li>परीक्षार्थी अपनी उत्तरपुस्तिका के कवर पृष्ठ के ऊपर भाग में क्रमांक-(1) में अपने अंकपत्र देने का नामांकन अंकित करते हुए क्रमांक-(2) में अपना पूर्ण नाम साफ-साफ लिखें। इसके अनावश्यक रूप से गड़बड़ी मान्य नहीं की जायेगी।</li>
                <li>प्रायोगिक परीक्षा की उत्तरपुस्तिका के आवरण पृष्ठ के नीचे वाले भाग को परीक्षक द्वारा भरवाने का कार्य किया जायेगा। अन्य परीक्षार्थी इस भाग को भरें, तो परीक्षा का उस विषय में मूल्यांकन नहीं किया जाएगा तथा परीक्षार्थी को उस विषय की परीक्षा/अंक प्रदान करने के लिए योग्य नहीं होगा।</li>
                <li>उत्तरपुस्तिका के प्रत्येक पृष्ठ पर परीक्षार्थी अपने हस्ताक्षर करें या न करें।</li>
                <li>यदि पत्रक को सही करने की आवश्यकता हो, तो परीक्षार्थी उत्तरपुस्तिका के उस पृष्ठ पर कार्य करने के पश्चात उसे काट दें तथा (x) करें।</li>
                <li>उत्तरपुस्तिका के अंतरिक पृष्ठ पर किसी प्रकार की कोई शंका न लिखें अन्यथा उसका स्थान रद्द किया जायेगा। परीक्षार्थी परीक्षा हॉल के साथ स्थान का कुछ भी नहीं लिखें, कोई नाम या परीक्षक के उपयोग का नहीं है।</li>
                <li>उत्तरपुस्तिका को फोल्ड न करें- फटे नहीं तथा कोई अन्य चिन्ह न छोड़ें।</li>
                <li>प्रश्न-पत्र में से कोई प्रश्न अनिवार्य न हो तो उसकी स्पष्ट स्थिति दें।</li>
                <li>टाइपरेटर, व्हाइट लिक्विड का इस्तेमाल करना वर्जित होगा। अन्यथा परीक्षा अमान्य कर दी जाएगी।</li>
                <li>प्रत्येक प्रश्न के सामने अंकित अंक को छोड़ कोई भी टिप्पणी न करें।</li>
                <li>अंतिम परीक्षा द्वारा उत्तरपुस्तिका पर उत्तर लिखने के उपरांत परीक्षक/केन्द्राधीक्षक की विधि के अनुसार हस्ताक्षर कीजिए ताकि उत्तरपुस्तिका को मूल्यांकन हेतु भेजा जा सके।</li>
                <li>परीक्षार्थी अपनी उत्तरपुस्तिका को अन्तर्निहित परीक्षा के उपरांत स्थान पर ही जमा करें।</li>
                <li>परीक्षा केन्द्र में कैलकुलेटर, मोबाइल फोन, इयर फोन, पेजर, घड़ी, ब्लूटूथ, इत्यादि इलेक्ट्रॉनिक डिवाइस का प्रयोग वर्जित है।</li>
                <li>जाँच परीक्षा में गैर-उत्तरवर्ती या जाँच परीक्षा में अनुशंसित उत्तर/अनुशंसित उत्तर ही प्रायोगिक परीक्षा, 2024 में कार्यान्वित किया जा सकता है।</li>
            </ol>

        </div>
        <hr style="border: 1px solid black; margin: 10px 0;">
        <div id="infoDiv"><b></b></div>

        <div class="text-center mt-4 mb-5">
            <button onclick="generatePDF(this)" class="btn btn-primary" id="downloadBtn">Download PDF</button>
        </div>


    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
    <script>
        function preventKeys(e) {
            if (e.ctrlKey && ['c', 'v', 'u', 's'].includes(e.key.toLowerCase())) {
                alert("This action is disabled.");
                e.preventDefault();
                return false;
            }
        }

        //async function generatePDF(btn) {
        //    // Hide button
        //    btn.style.display = 'none';

        //    const { jsPDF } = window.jspdf;
        //    const pdf = new jsPDF('p', 'mm', 'a4');
        //    const element = document.getElementById('admitCard');

        //    // Get current date & time
        //    const now = new Date();
        //    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        //    const dateStr = now.toLocaleDateString('en-US', options);
        //    const timeStr = now.toLocaleTimeString('en-US');
        //    const footerText = `${dateStr} ${timeStr} Page 1 of 1`;

        //    const canvas = await html2canvas(element, {
        //        scale: 2,
        //        useCORS: true,
        //        scrollY: -window.scrollY
        //    });

        //    const imgData = canvas.toDataURL('image/jpeg', 1.0);
        //    const imgProps = pdf.getImageProperties(imgData);
        //    const pdfWidth = pdf.internal.pageSize.getWidth();
        //    const pageHeight = pdf.internal.pageSize.getHeight();

        //    // Resize image to fit A4 height exactly
        //    const imgHeight = pageHeight - 6; // smaller footer space
        //    // ...
        //    const finalWidth = (imgProps.width * imgHeight) / imgProps.height;

        //    const x = (pdfWidth - finalWidth) / 2; // center horizontally

        //    pdf.addImage(imgData, 'JPEG', x, 0, finalWidth, imgHeight);

        //    // Add footer text
        //    pdf.setFontSize(10);
        //    pdf.text(footerText, pdf.internal.pageSize.getWidth() / 2, pageHeight - 5, { align: 'center' });

        //    pdf.save('AdmitCard.pdf');
        //}
        async function generatePDF(btn) {
            btn.style.display = 'none';

            const { jsPDF } = window.jspdf;
            const pdf = new jsPDF('p', 'mm', 'a4');
            const element = document.getElementById('admitCard');

            const now = new Date();
            const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            const dateStr = now.toLocaleDateString('en-US', options);
            const timeStr = now.toLocaleTimeString('en-US');
            const footerText = `${dateStr} ${timeStr} Page 1 of 1`;

            const canvas = await html2canvas(element, {
                scale: 2,
                useCORS: true,
                scrollY: -window.scrollY
            });

            const imgData = canvas.toDataURL('image/jpeg', 1.0);
            const imgProps = pdf.getImageProperties(imgData);
            const pdfWidth = pdf.internal.pageSize.getWidth();
            const pageHeight = pdf.internal.pageSize.getHeight();

            const footerSpace = 10; // leave exactly 10mm for footer

            // Proper image height to fit above footer
            const imgHeight = pageHeight - footerSpace;
            const finalWidth = (imgProps.width * imgHeight) / imgProps.height;
            const x = (pdfWidth - finalWidth) / 2;

            pdf.addImage(imgData, 'JPEG', x, 0, finalWidth, imgHeight);

            // Footer
            pdf.setFontSize(9);
            pdf.text(footerText, pdf.internal.pageSize.getWidth() / 2, pageHeight - 3, { align: 'center' });

            pdf.save('AdmitCard.pdf');
        }


    </script>

</body>
</html>
