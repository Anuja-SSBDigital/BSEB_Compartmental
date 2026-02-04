<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PracticalAdmitCertificate.aspx.cs" Inherits="PracticalAdmitCertificate" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0">--%>
    <meta name="viewport" content="width=1200">
    <title>Practical Admit Certificate</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet">
    <%--<style>
        body {
            font-family: none;
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
            max-width: 130px;
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
        font-size: 17px !important; /* Increased font size */
        line-height: 1.4;
        -webkit-print-color-adjust: exact !important;
        print-color-adjust: exact !important;
    }

    .container, .table-details, .subjects-table, .row, .table, .header, .instructions {
        font-size: inherit !important;
    }

  
        .subjects-table th, .subjects-table td {
            padding: 10px 12px !important;
            line-height: 2 !important;
        }

    @page {
        size: A4 portrait;
        margin: 10mm 12mm 10mm 12mm; /* Top, Right, Bottom, Left */
    }

    .btn, .no-print {
        display: none !important;
    }

    .container {
        width: 100%;
        border: 1px solid #000;
        margin: 0 auto;
        padding: 15px; /* Add inner spacing */
        box-sizing: border-box;
        page-break-inside: avoid;
    }

    .table-details, .subjects-table {
        font-size: 16px; /* Slightly larger */
    }

    .row, .table, .table th, .table td, .header, .instructions, .signature {
        page-break-inside: avoid;
        break-inside: avoid;
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
    </style>--%>

    <style>
        body {
            font-family: none;
        }

        ol {
            list-style-type: decimal;
        }

        .container {
            padding: 30px;
            margin-top: 20px;
        }

        .header {
            text-align: center;
            margin-bottom: 20px;
        }

        .logo {
            max-width: 130px;
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
            body, td, th, input, label {
                font-size: 17px !important; /* Ensures larger, uniform text everywhere */
                line-height: 1.4;
            }

            -webkit-print-color-adjust: exact !important;
            print-color-adjust: exact !important;

            .container, .table-details, .subjects-table, .row, .table, .header, .instructions {
                font-size: inherit !important;
            }

                .subjects-table th, .subjects-table td {
                    padding: 10px 12px !important;
                    line-height: 2 !important;
                }

            @page {
                size: A4 portrait;
                margin: 10mm 12mm 10mm 12mm;
            }

            .btn, .no-print {
                display: none !important;
            }

            .container {
                width: 100%;
                border: 1px solid #000;
                margin: 0 auto;
                padding: 15px;
                box-sizing: border-box;
                page-break-inside: avoid;
            }

            .table-details, .subjects-table {
                font-size: 16px !important;
            }

            .row, .table, .table th, .table td, .header, .instructions, .signature {
                page-break-inside: avoid;
                break-inside: avoid;
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

        hr {
            opacity: 1 !important;
        }

        tbody {
            color: black;
        }

        li {
            color: black;
        }
    </style>

</head>
<body>
    <!-- Loading Overlay -->
    <div id="loadingOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0,0,0,0.6); z-index: 9999; justify-content: center; align-items: center;">
        <div class="spinner-border text-light" role="status" style="width: 3rem; height: 3rem;">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="text-light mt-3" style="font-size: 1.2rem;">Generating PDF, please wait...</div>
    </div>
    <form runat="server" id="form1">
        <div class="text-center mt-4 mb-5">
            <a href="DownloadPracticaladmitcard.aspx" class="btn btn-primary no-print" style="text-decoration: none !important;">Back</a>
            <button type="button" onclick="generatePDF()" class="btn btn-primary no-print">Download PDF</button>
        </div>
        <asp:Repeater ID="rptStudents" runat="server" OnItemDataBound="rptStudents_ItemDataBound">
            <ItemTemplate>
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
                                    <strong>
                                        <asp:Label ID="lblExamTitleHindi" runat="server" CssClass="hindi-title" /></strong>
                                    <br />
                                    <strong>
                                        <asp:Label ID="lblExamTitle" runat="server" CssClass="english-title" /></strong>
                                    <br>
                                    <strong>प्रायोगिक परीक्षा का प्रवेश-पत्र</strong>
                                    <br />
                                    <strong>Admit Card For Practical Examination </strong>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:PlaceHolder ID="phFaculty" runat="server">
                                    <asp:Label runat="server" ID="lblFacultyHindi" />
                                    <br />
                                </asp:PlaceHolder>
                                <label style="margin-left: 37px;"><strong>FACULTY:</strong> <%# Eval("FacultyName") %></label>
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
                    <table style="width: 100%; border-collapse: collapse; font-family: system-ui;text-transform: uppercase;">
                        <tr>
                            <!-- Left Side: Student Details -->
                            <td style="width: 85%; vertical-align: top; padding-right: 10px; font-family: sans-serif; line-height: 1.8;">
                                <table style="width: 100%; font-size: 17px; line-height: 2; border-collapse: collapse; font-weight: 600; font-family: system-ui;">
                                    <asp:HiddenField ID="hfFacultyId" runat="server" Value='<%# Eval("FacultyId") %>' />
                                    <colgroup>
                                        <col style="width: 25%;">
                                        <col style="width: 25%;">
                                        <col style="width: 25%;">
                                        <col style="width: 25%;">
                                    </colgroup>

                                    <tr>
                                        <td style="font-size: large;">BSEB UNIQUE Id:</td>
                                        <td colspan="3" style="word-break: break-word; white-space: normal;"><%# Eval("UniqueNo") %></td>
                                    </tr>

                                    <tr>
                                        <%--<td>+2 स्कूल का नाम</td>--%>
                                        <td><strong>
                                            <asp:Label ID="lblCollegeName" runat="server" /></strong></td>
                                        <td colspan="3" style="word-break: break-word; white-space: normal;"><%# Eval("CollegeName") %></td>
                                    </tr>
                                    <tr>
                                        <td>परीक्षार्थी का नाम:</td>
                                        <td><%# Eval("StudentName") %></td>
                                    </tr>
                                    <tr>
                                        <td>माता का नाम:</td>
                                        <td><%# Eval("MotherName") %></td>
                                    </tr>
                                    <tr>
                                        <td>पिता का नाम:</td>
                                        <td><%# Eval("FatherName") %></td>
                                    </tr>
                                    <tr>
                                        <td>वैवाहिक स्थिति:</td>
                                        <td><%# Eval("MaritalStatus") %></td>
                                    </tr>
                                    <tr>
                                        <td>परीक्षार्थी का आधार नं: </td>
                                        <td><%# Eval("AadharNo") %></td>
                                        <td>दिव्यांग कोटि:</td>
                                        <td><%# Eval("Disability") != DBNull.Value && Convert.ToBoolean(Eval("Disability")) ? "YES" : "NO" %></td>
                                    </tr>
                                    <tr>
                                        <td>सूचीकरण संख्या/वर्ष:</td>
                                        <td><%# Eval("RegistrationNo") %></td>
                                        <td>परीक्षार्थी की कोटि:</td>
                                        <td><%# Eval("ExamTypeName") %></td>
                                    </tr>
                                    <tr>
                                        <td>रौल कोड:</td>
                                        <%--<td><%# Eval("CollegeCode") %></td>--%>
                                        <td><%# Eval("RollCode") %></td>
                                        <td>रौल क्रमांक:</td>
                                        <td><%# Eval("RollNumber") %></td>
                                        <td>लिंग:</td>
                                        <td style="padding-left: 20px;"><%# Eval("Gender") %></td>
                                    </tr>
                                    <tr>
                                        <td>परीक्षा केंद्र का नाम:</td>
                                        <td colspan="5"><%# Eval("PracticalExamCenterName") %></td>
                                    </tr>
                                </table>
                            </td>

                            <!-- Right Side: Photo and Signature -->
                            <td style="width: 15%; text-align: center; vertical-align: top;">
                                <div style="border: 1px solid black; padding: 5px; display: inline-block;">
                                    <img src='<%# ResolveUrl(Eval("StudentPhotoPath").ToString()) %>' alt="Photo" style="/* width: 100%; *//* max-width: 160px; *//* height: auto; */width: 160px; /* adjustable common display size */height: 160px; /* keeps it square */object-fit: cover; /* crops to fill without distortion */border: 2px solid #ccc; /* optional border for photo look */border-radius: 4px; /* slight rounding, optional */" />
                                </div>
                                <div style="margin-top: 10px;">
                                    <img src='<%# ResolveUrl(Eval("StudentSignaturePath").ToString()) %>' alt="Signature" style="width: 100%; max-width: 180px; height: auto; width: 180px; /* adjustable common display size */height: 40px; /* keeps it square *//* object-fit: cover; */ /* crops to fill without distortion *//* border: 2px solid #ccc; */ /* optional border for photo look *//* border-radius: 4px; */ /* slight rounding, optional */" />
                                </div>
                            </td>
                        </tr>
                    </table>


                    <!-- Examination Table -->
                    <h3 style="font-size: 14px; margin-bottom: 5px; margin-top: 12px; font-weight: 600; font-family: system-ui;">प्रायोगिक परीक्षा के विषय (निर्धारित परीक्षा कार्यक्रम सहित)
                    </h3>


                    <table class="table  align-middle table-details" style="border: 2px solid #000;">
                        <thead>
                            <tr>
                                <th>प्रायोगिक विषय</th>
                                <th>विषय कोड</th>
                                <th>विषय का नाम</th>
                                <th>परीक्षा की तारीख</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr runat="server" id="trElective1">
                                <td rowspan="3" style="border: none">वैकल्पिक विषय</td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject1Code") %></td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject1Name") %> <%# Eval("ElectiveSubject1PaperType") %></td>
                                <%--<td rowspan="3" style="border-bottom: hidden; vertical-align: middle;">--%>
                                <%--<td rowspan="5" style="vertical-align: middle; font-weight: 600; font-size: larger;">--%>
                              <td runat="server" id="tdExamDate" style="vertical-align: middle; font-weight: 600; font-size: larger;">
                                    <div style="text-align: center;">
                                        <asp:Label ID="lblExamStartDate" runat="server" CssClass="hindi-title" /><br />
                                        <span>To</span><br />
                                        <asp:Label ID="lblExamToDate" runat="server" CssClass="hindi-title" />
                                    </div>
                                </td>
                            </tr>
                            <tr runat="server" id="trElective2">
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject2Code") %></td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject2Name") %> <%# Eval("ElectiveSubject2PaperType") %></td>
                            </tr>
                            <tr runat="server" id="trElective3">
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject3Code") %></td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("ElectiveSubject3Name") %> <%# Eval("ElectiveSubject3PaperType") %></td>
                            </tr>
                            <tr runat="server" id="trAdditional">
                                <td>अतिरिक्त विषय</td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("AdditionalSubjectCode") %></td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("AdditionalSubjectName") %> <%# Eval("AdditionalSubjectPaperType") %></td>
                                <%-- <td style="text-align: center;"><%# Eval("AdditionalSubjectDate") %></td>--%>
                            </tr>
                            <tr runat="server" id="trVocational">
                                <td>व्यावसायिक विषय</td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("VocationalSubjectCode") %></td>
                                <td style="font-weight: 600; font-size: larger;"><%# Eval("VocationalSubjectName") %>  <%# Eval("VocationalSubjectPaperType") %></td>

                            </tr>
                        </tbody>
                    </table>


                    <div class="" style="font-family: 'Noto Sans Devanagari', 'Mangal', 'Arial', sans-serif; font-size: 17px; margin-top: 25px;">
                        <div class="col-md-12 text-end">

                            <img src="assets/img/COE%20sign.png" style="width: 11%;" />

                        </div>
                        <div class="mb-3 row" style="">


                            <div class="col-md-6">

                                <strong>
                                    <asp:Label ID="lblExamSubjectHindi" runat="server" /><br>
                                </strong>
                            </div>

                            <div class="col-md-6 text-end">
                                <strong>परीक्षा नियंत्रक (उ0मा)</strong>
                            </div>
                        </div>
                        <!-- Signature block -->
                        <hr style="border: 1px solid black; margin: 10px 0;" />
                        <!-- Heading -->
                        <h6 class="text-center mb-3"><u>परीक्षार्थी के लिए आवश्यक निदेश</u></h6>

                        <!-- Instructions -->

                        <ol style='<%# Eval("FacultyName").ToString() == "SCIENCE" ? "font-size: 14px;": "font-size: 16px;" %>'>
                            <%-- <li>प्रायोगिक परीक्षा दिनांक 10/01/2025 से 20/01/2025 तक संचालित होगी। केन्द्राधीक्षक दिनांक 10/01/2025 से 20/01/2025 तक की अवधि में परीक्षार्थियों की संख्या के अनुसार तिथि एवं पाली का निर्धारण करके प्रायोगिक परीक्षा केन्द्र पर उपस्थित सभी परीक्षार्थियों के प्रायोगिक विषयों की परीक्षा आयोजित करेंगे।</li>
                            <li>परीक्षार्थी अपने इस प्रवेश-पत्र में उल्लिखित प्रायोगिक परीक्षा केन्द्र पर दिनांक 10-01-2025 को पूर्वाह्न 09:00 बजे अनिवार्य रूप से जाकर परीक्षा केन्द्र के परिसर की सूचना पट्ट से यह जानकारी प्राप्त कर लें कि उनके द्वारा चयनित विषय की प्रायोगिक परीक्षा किस तिथि एवं किस पाली में संचालित होगी, जिसमें उन्हें सम्मिलित होना अनिवार्य है।</li>
                            <li>परीक्षार्थी के प्रत्येक प्रायोगिक विषय की परीक्षा के लिए 08 पृष्ठों की केवल एक ही उत्तरपुस्तिका मिलेगी। अतिरिक्त उत्तरपुस्तिका नहीं दी जाएगी। परीक्षार्थी उत्तरपुस्तिका लेते ही यह सुनिश्चित कर लें कि इसमें 8 पृष्ठ हैं एवं सही क्रम में हैं।</li>
                            <li>उत्तरपुस्तिका प्राप्त होते ही परीक्षार्थी अपने प्रवेश-पत्र तथा उत्तरपुस्तिका पर मुद्रित विवरण (Details) का मिलान कर यह अवश्य सुनिश्चित कर लें कि जो उत्तरपुस्तिका परीक्षा द्वारा उन्हें दी गई है, वह उन्हीं की है। मिलान विवरण सही होने पर उसे तुरंत परीक्षक को वापस लौटा दिया जाए।</li>
                            <li>उत्तरपुस्तिका प्राप्त होने पर परीक्षार्थी उनके आवरण पृष्ठ के शीर्षक “परीक्षार्थियों के लिए निर्देश” अवश्य पढ़ें एवं उसका अनुपालन करें।</li>
                            <li>परीक्षार्थी अपनी उत्तरपुस्तिका के कवर पृष्ठ के ऊपर दायें भागों में क्रमांक-(1) में अपने उत्तर देने का माध्यम अंकित करते हुए क्रमांक-(2) में अपना पूर्ण हस्ताक्षर अंकित करें। इसके अलावा उत्तर मुद्रित विवरण में किसी भी प्रकार की कोई छेड़-छाड़ नहीं करें।</li>
                            <li>प्रायोगिक परीक्षा की उत्तरपुस्तिका के आवरण पृष्ठ के बायें भाग एवं नीचे भाग परीक्षक द्वारा सम्पादित किया जायेगा। अगर परीक्षार्थी इस भाग को भरते हैं, तो परीक्षा का उस विषय में मूल्यांकन नहीं किया जा सकता है। वे लोग बिना आन्तरिक/बाह्य परीक्षकों के भरने के लिए दिया गया है।</li>
                            <li>उत्तरपुस्तिका के प्रत्येक पृष्ठ को परीक्षार्थी हस्ताक्षर करें अथवा न करें।</li>
                            <li>यदि एक कार्ड करने की आवश्यकता हो, तो परीक्षार्थी उत्तरपुस्तिका के अंतिम पृष्ठ पर एक कार्ड करके उसे काट दें/क्रॉस (x) कर दें।</li>
                            <li>उत्तरपुस्तिका के आंतरिक पृष्ठों में लाइन ड्रा/विजिबल स्थान साफ रखा जाय। इस स्थान के अलावा के पृष्ठों में कुछ भी नहीं लिखें, चूँकि यह भाग परीक्षा के उपयोग के लिए है।</li>
                            <li>उत्तरपुस्तिका के पृष्ठों को कोई- फोल्ड नहीं करें या बीच-बीच में दाग-धब्बे नहीं हो।</li>
                            <li>प्रश्न-पत्र में कोई प्रश्न संख्या अनिवार्य हो तो उसके अनुसार ही संख्या लिखें।</li>
                            <li>व्हाइटनर, ब्लेड तथा नाखून का इस्तेमाल करना सर्वथा वर्जित है, अन्यथा परीक्षा अमान्य कर दी जाएगी।</li>
                            <li>प्रत्येक प्रश्न के समक्ष होने पर अंकित से नीचे तक सीधी रेखा डालें।</li>
                            <li>प्रायोगिक परीक्षा समाप्ति के बाद उपलब्ध करायी गयी उत्तरपुस्तिका पर परीक्षा की समाप्ति उपरांत उत्तर-पत्र पर अंकित करके उत्तरपुस्तिका की क्रम संख्या लिखवाकर अपना हस्ताक्षर करना अनिवार्य होगा, तथा उसके बाद संबंधित प्रयोग गोले को नीले/काले पेन से परीक्षक द्वारा भरवाया जाएगा। फिर परीक्षार्थी द्वारा उत्तरपुस्तिका जमा की जाएगी।</li>
                            <li>परीक्षार्थी अपनी उत्तरपुस्तिका को अन्तिम परीक्षा समाप्ति के पश्चात नियत स्थान पर ही जमा करें।</li>
                            <li>परीक्षा भवन में कैलकुलेटर, मोबाईल फोन, ईयरफोन, ब्लूटूथ, स्मार्टवॉच अथवा इस प्रकार का कोई अन्य इलेक्ट्रॉनिक उपकरण का लाना सख्त मना है।</li>
                            <li>जाँच परीक्षा में गैर-उत्तरवर्ती या जाँच परीक्षा में अनुशंसित छात्र/छात्रा इंटरमीडिएट वार्षिक प्रायोगिक परीक्षा, 2025 में सम्मिलित नहीं हो सकते हैं।</li>
                            --%>
                            <li>प्रायोगिक परीक्षा दिनांक 10-01-2026 से  20-01-2026 तक संचालित होगी। केन्द्राधीक्षक दिनांक 10-01-2026 से  20-01-2026 तक की अवधि में परीक्षार्थीयों की संख्या के अनुसार तिथि एवं पाली का निर्धारण करके प्रायोगिक परीक्षा केन्द्र पर आवंटित सभी परीक्षार्थियों के प्रायोगिक विषयों की परीक्षा आयोजित करेंगें। </li>
                            <li>परीक्षार्थी अपने इस प्रवेश-पत्र में उल्लिखित प्रायोगिक परीक्षा केंद्रों पर दिनांक 10-01-2026 को पूर्वाह्न 09:00 बजे अनिवार्य रूप से जाकर परीक्षा केन्द्र के परिसर की सूचना पट्ट से यह जानकारी प्राप्त कर लेगें कि उनके द्वारा चयनित विषय की प्रायोगिक परीक्षा किस तिथि एवं किस पाली में संचालित होगी, जिसमें उन्हें सम्मिलित होना अनिवार्य है।</li>
                            <li>परीक्षार्थी के प्रत्येक प्रायोगिक विषय की परीक्षा के लिए 08 पृष्ठों की केवल एक ही उत्तरपुस्तिका मिलेगी। अतिरिक्त उत्तरपुस्तिका नहीं दी जाएगी। परीक्षार्थी उत्तरपुस्तिका लेते ही यह सुनिश्चित कर लें कि इसमें 8 पृष्ठ है एवं सही क्रम में है।</li>
                            <li>उत्तरपुस्तिका प्राप्त होते ही परीक्षार्थी अपने प्रवेश-पत्र तथा उत्तरपुस्तिका पर मुद्रित विवरणों (Details) का मिलान कर यह अवश्य सुनिश्चित हो लें कि जो उत्तरपुस्तिका परीक्षक द्वारा उन्हें दी गई है, वह उन्हीं की है। भिन्न विवरणों की उत्तरपुस्तिका प्राप्त होने पर उसे तुरंत परीक्षक को वापस लौटा दिया जाए।</li>
                            <li>उत्तरपुस्तिका प्राप्त होने पर परीक्षार्थी उनके आवरण पृष्ठ के पीछे अंकित 'परीक्षार्थियों के लिए निर्देश' अवश्य पढ़े एवं उसका अनुपालन करें।</li>
                            <li>परीक्षार्थी अपनी उत्तरपुस्तिका के कवर पृष्ठ के ऊपरी बायें तथा दायें भागों में क्रमांक-(1) में अपने उत्तर देने का माध्यम अंकित करते हुए क्रमांक-(2) में अपना पूर्ण हस्ताक्षर अंकित करें। इसके अलावा अन्य मुद्रित विवरणों में किसी भी प्रकार से कोई छेड़-छाड़ नहीं करें।</li>
                            <li>प्रायोगिक परीक्षा की उत्तरपुस्तिका के आवरण पृष्ठ के निचले बायें एवं दायें भागों को परीक्षार्थी द्वारा कदापि नहीं भरा जाएगा। अगर परीक्षार्थी इस भाग को भरते हैं, तो परीक्षार्थी का इस विषय में परीक्षाफल रद्द किया जा सकता है। ये दोनो भाग आंतरिक/बाह्य परीक्षकों को भरने के लिए दिया गया है।</li>
                            <li>उत्तरपुस्तिका के पन्नों के दोनो पृष्ठों पर तथा प्रत्येक लाइन पर लिखें एवं पृष्ठों को नष्ट न करें।</li>
                            <li>यदि रफ कार्य करने की आवश्यकता हो, तो परीक्षार्थी उत्तरपुस्तिका के अंतिम पृष्ठ पर रफ कार्य करके उसे काट दे/क्रॉस  (x) कर दें।</li>
                            <li>उत्तरपुस्तिका के आंतरिक पृष्ठों पर दाहिने हाशिए में लाइन खींचकर सादा स्थान छोड़ रखा गया है। शेष स्थान रूल्ड है। परीक्षार्थी दाहिने हाशिए के सादे स्थान में कुछ भी नहीं लिखेंगें, चूँकि  यह भाग परीक्षक के उपयोग के लिए है।</li>
                            <li>उत्तरपुस्तिका के पृष्ठों को मोड़े-फाड़े नहीं तथा बीच-बीच में व्यर्थ ही खाली न छोड़े।</li>
                            <li>प्रश्न-पत्र में दी हुई संख्या के अनुसार अपने उत्तरों की संख्या लिखें।</li>
                            <li>व्हाइटनर, ब्लेड तथा नाखून का इस्तेमाल करना सर्वथा वर्जित है, अन्यथा परीक्षाफल अमान्य कर दिया जाएगा।</li>
                            <li>प्रश्न्नोत्तर के समाप्त होने पर अंतिम में नीचे एक क्षैतिज रेखा खींच दें।</li>
                            <li>आंतरिक परीक्षक द्वारा उपलब्ध कराये गए उपस्थिति-पत्रक में परीक्षार्थी द्वारा यथा-स्तम्भ परीक्षा की तिथि अंकित करते हुए उत्तरपुस्तिका की क्रम संख्या लिखकर अपना हस्ताक्षर किया जाएगा। परीक्षार्थी की उपस्थित, अनुपस्थित एवं निष्कासन से संबंधित संगत गोले को नीले/काले पेन से परीक्षक द्वारा भरा जाएगा न कि परीक्षार्थी द्वारा।</li>
                            <li>परीक्षार्थी अपनी उत्तरपुस्तिका को आन्तरिक परीक्षक के पास जमा किये बिना परीक्षा भवन न छोड़े।</li>
                            <li>परीक्षा केन्द्र में कैलकुलेटर, मोबाइल फोन, इयर फोन, पेजर, ब्लूटूथ या इस प्रकार का कोई अन्य इलेक्ट्रॉनिक उपकरण ले जाना सख्त मना है।</li>
                            <%--<li>जाँच परीक्षा में गैर-उत्प्रेषित या जाँच परीक्षा में अनुपस्थित छात्र/छात्रा इन्टरमीडिएट वार्षिक प्रायोगिक परीक्षा,2026 में कदापि सम्मिलित नहीं हो सकते हैं।</li>--%>
                            <li id="liExamNote" runat="server">जाँच परीक्षा में गैर-उत्प्रेषित या जाँच परीक्षा में अनुपस्थित छात्र/छात्रा इन्टरमीडिएट वार्षिक प्रायोगिक परीक्षा, 2026 में कदापि सम्मिलित नहीं हो सकते हैं।</li>





                        </ol>


                    </div>
                    <hr style="border: 1px solid black; margin: 5px 0;">
                    <div id="infoDiv"><b></b></div>


                </div>

            </ItemTemplate>
        </asp:Repeater>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

        <%-- <script>
            window.onload = function () {
                window.generatePDF = async function () {
                    document.getElementById('loadingOverlay').style.display = 'flex';
                    const { jsPDF } = window.jspdf;
                    const pdf = new jsPDF('p', 'mm', 'a4');
                    const containers = document.querySelectorAll('.container');

                    // Timestamp for footer
                    const now = new Date();
                    const formattedDate = now.toLocaleString('en-US', {
                        weekday: 'long', year: 'numeric', month: 'long', day: 'numeric',
                        hour: 'numeric', minute: '2-digit', second: '2-digit', hour12: true
                    });

                    for (let i = 0; i < containers.length; i++) {
                        const original = containers[i];

                        // 1️⃣ Clone the container
                        const clone = original.cloneNode(true);

                        // 2️⃣ Force desktop styles on the clone
                        clone.style.width = '1200px';
                        clone.style.minWidth = '1200px';
                        clone.style.maxWidth = '1200px';
                        clone.style.margin = '0 auto';
                        clone.style.transform = 'scale(1)';
                        clone.style.fontSize = '16px'; // adjust if needed

                        // 3️⃣ Append clone off-screen
                        clone.style.position = 'absolute';
                        clone.style.top = '-9999px';
                        document.body.appendChild(clone);

                        // 4️⃣ Capture PDF
                        const canvas = await html2canvas(clone, {
                            scale: 2.5,  // higher scale for sharp PDF
                            useCORS: true,
                            logging: false
                        });

                        const imgData = canvas.toDataURL('image/jpeg', 1.0);
                        const imgProps = pdf.getImageProperties(imgData);
                        const pdfWidth = pdf.internal.pageSize.getWidth();
                        const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

                        if (i > 0) pdf.addPage();

                        pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);

                        const pageText = `${formattedDate}    Page ${i + 1} of ${containers.length}`;
                        pdf.setFontSize(8);
                        pdf.setTextColor(0, 0, 0);
                        pdf.text(pageText, pdfWidth / 2, 294, { align: 'center' });

                        // 5️⃣ Remove clone
                        document.body.removeChild(clone);
                    }

                    pdf.save('PracticalAdmitCard.pdf');
                };

            }
        </script>--%>

        <script>
            window.onload = function () {
                window.generatePDF = async function () {
                    // Show loader
                    document.getElementById('loadingOverlay').style.display = 'flex';

                    const { jsPDF } = window.jspdf;
                    const pdf = new jsPDF('p', 'mm', 'a4');
                    const containers = document.querySelectorAll('.container');

                    // Timestamp for footer
                    const now = new Date();
                    const formattedDate = now.toLocaleString('en-US', {
                        weekday: 'long', year: 'numeric', month: 'long', day: 'numeric',
                        hour: 'numeric', minute: '2-digit', second: '2-digit', hour12: true
                    });

                    try {
                        for (let i = 0; i < containers.length; i++) {
                            const original = containers[i];

                            // Clone the container
                            const clone = original.cloneNode(true);

                            // Force desktop styles on the clone
                            clone.style.width = '1200px';
                            clone.style.minWidth = '1200px';
                            clone.style.maxWidth = '1200px';
                            clone.style.margin = '0 auto';
                            clone.style.transform = 'scale(1)';
                            clone.style.fontSize = '16px';

                            // Append clone off-screen
                            clone.style.position = 'absolute';
                            clone.style.top = '-9999px';
                            document.body.appendChild(clone);

                            // Capture with html2canvas
                            const canvas = await html2canvas(clone, {
                                scale: 2.5,
                                useCORS: true,
                                logging: false
                            });

                            const imgData = canvas.toDataURL('image/jpeg', 1.0);
                            const imgProps = pdf.getImageProperties(imgData);
                            const pdfWidth = pdf.internal.pageSize.getWidth();
                            const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

                            if (i > 0) pdf.addPage();
                            pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);

                            const pageText = `${formattedDate} Page ${i + 1} of ${containers.length}`;
                            pdf.setFontSize(8);
                            pdf.setTextColor(0, 0, 0);
                            pdf.text(pageText, pdfWidth / 2, 294, { align: 'center' });

                            // Remove clone
                            document.body.removeChild(clone);
                        }

                        pdf.save('PracticalAdmitCard.pdf');
                    } catch (error) {
                        console.error('PDF generation failed:', error);
                        alert('Error generating PDF. Please try again.');
                    } finally {
                        // Always hide loader when done (success or error)
                        document.getElementById('loadingOverlay').style.display = 'none';
                    }
                };
            }
        </script>
    </form>

</body>
</html>

