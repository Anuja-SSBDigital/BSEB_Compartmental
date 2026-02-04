<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DummyRegCertificate.aspx.cs" Inherits="DummyRegCertificate" %>

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
            padding: 30px;
            margin-top: 20px;
        }

        .header {
            text-align: center;
            margin-bottom: 20px;
        }

        .logo {
            max-width: 145px;
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
            line-height:1.4;
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

        #infoDiv {
            margin-top: 20px;
            font-family: Arial, sans-serif;
            font-size: 14px;
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
    </style>
</head>
<body>
    <form runat="server" id="form1">
    <div class="text-center mt-4 mb-5">
        <a href="DownloadDummyRegCard.aspx" class="btn btn-primary no-print" style="text-decoration: none !important;">Back</a>
        <button type="button" onclick="generatePDF()" class="btn btn-primary no-print">Download PDF</button>
     <%--   <button onclick="generatePDF()" class="btn btn-primary no-print">Download PDF</button>--%>
    </div>
    <asp:Repeater ID="rptStudents" runat="server" OnItemDataBound="rptStudents_ItemDataBound">
        <ItemTemplate>
            <div class="container" id="admitCard">
                <div class="header">
                    <div class="row">
                        <div class="col-md-3">
                            <img src="assets/img/bsebimage.jpg" alt="Bihar Board Logo" class="logo">
                        </div>
                        <div class="col-md-6">
                            <div class="title">
                                <strong>बिहार विद्यालय परीक्षा समिति</strong><br>
                                <strong>BIHAR SCHOOL EXAMINATION BOARD</strong>
                            </div>
                            <div class="sub-title">
                                <strong>डमी सूचीकरण प्रमाण-पत्र</strong>
                                <br>
                                <strong>DUMMY REGISTRATION CERTIFICATE</strong>
                                <br>
                                <strong>इंटरमीडिएट सत्र 2024-26</strong>
                                <br />
                                   <asp:HiddenField ID="hfFacultyId" runat="server" Value='<%# Eval("FacultyId") %>' />
                                <asp:HiddenField ID="hfHasVocationalSubjects" runat="server" Value='<%# Eval("HasVocationalSubjects") %>' />

                                      <%--  <asp:PlaceHolder ID="phRegular" runat="server" Visible='<%# !(bool)Eval("HasVocationalSubjects") %>'>
                                            <strong>INTERMEDIATE SESSION 2024-26</strong>
                                        </asp:PlaceHolder>
        
                                        <asp:PlaceHolder ID="phVocational" runat="server" Visible='<%# (bool)Eval("HasVocationalSubjects") %>'>
                                            <strong>INTERMEDIATE VOCATIONAL COURSE SESSION 2024-26</strong>
                                        </asp:PlaceHolder>--%>
                                  <asp:Label runat="server" ID="lbl_title" Visible="false"> <strong>INTERMEDIATE SESSION 2024-26</strong></asp:Label>
                                    <asp:Label runat="server" ID="lbl_titlevocational" Visible="false"> <strong>INTERMEDIATE VOCATIONAL COURSE SESSION 2024-26</strong></asp:Label>
                                  

                            </div>
                        </div>
                    </div>
                </div>

                <div class="borderline">
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                              
                            <td style="width: 75%; vertical-align: top; padding-right: 10px;">
                           
                                <table style="width: 100%; font-size: 17px; border-collapse: collapse;">
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>विद्यार्थी का नाम:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("StudentName") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>माता का नाम:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("MotherName") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>पिता का नाम:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("FatherName") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>लिंग:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("Gender") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>कोटि:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("CategoryName") %></td>
                                        <td style="padding: 3px 5px;"><strong>जाति कोटि:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("Caste") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>राष्ट्रीयता:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("Nationality") %></td>
                                        <td style="padding: 3px 5px;"><strong>धर्म:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("Religion") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>सूचीकरण संख्या/वर्ष:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("RegistrationNo") %></td>
                                        <td style="padding: 3px 5px;"><strong>जन्म की तारीख:</strong></td>
                                       <td style="padding: 3px 5px;">
                                             <%# Eval("DOB") != DBNull.Value && Eval("DOB") != null ? Eval("DOB").ToString() : "" %>
                                       <%--    <%# Eval("DOB") != DBNull.Value && Eval("DOB") != null ? ((DateTime)Eval("DOB")).ToString("dd/MM/yyyy") : "" %>--%>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong>संकाय :</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("Faculty") %></td>
                                        <td style="padding: 3px 5px;"><strong>वैवाहिक स्थिति:</strong></td>
                                        <td style="padding: 3px 5px;"><%# Eval("MaritalStatus") %></td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 3px 5px;"><strong> <asp:Label ID="lblExamSchoolHindi" runat="server" /></strong></td>
                                        <td style="padding: 3px 5px;" colspan="3"><%# Eval("College") %></td>
                                    </tr>
                                </table>
                            </td>

                            <td style="width: 25%; text-align: center; vertical-align: top;">
                                <div style="border: 1px solid black; padding: 5px; display: inline-block;">
                                    <img src='<%# Page.ResolveUrl(Eval("StudentPhotoPath").ToString()) %>' alt="Student Photo" style="width: 100%; max-width: 130px; height: auto;">
                                </div>
                                <div style="margin-top: 10px;">
                                    <img src='<%# Page.ResolveUrl(Eval("StudentSignaturePath").ToString()) %>' alt="Signature" style="width: 100%; max-width: 130px; height: auto;">
                                </div>
                            </td>
                        </tr>
                    </table>

                    <table class="subjects-table">
                        <thead>
                             <asp:PlaceHolder ID="phHeaderNormal" runat="server">
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
                                    <th colspan="2">व्यावसायिक ट्रेड<br>(Vocational Trade)</th>
                                </asp:PlaceHolder>

                                 </tr>
                              </asp:PlaceHolder>
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
                    <div class="mb-3 row" style="margin-top: 80px;">
                        <div class="col-md-6">
                             <strong><asp:Label ID="lblHindiSchool" runat="server" /><br></strong>
                             <%--   <em>एवं मुहर</em>--%>
                        </div>
                        <div class="col-md-6 text-end">

                            <strong>परीक्षा नियंत्रक (30मा0)</strong>
                        </div>
                    </div>
                    <div class=" mb-3">
                        <h6 class="text-center"><strong>विद्यार्थियों / शिक्षण संस्थान के प्रधान के लिए आवश्यक निर्देश :</strong></h6>
                        <ol>
                            <li>डमी सूचीकरण प्रमाण पत्र किसी भी शैक्षणिक गतिविधि के लिए अनुमान्य नहीं होगा।</li>
                            <li>मूल सूचीकरण प्रमाण पत्र जारी होने के बाद किसी भी प्रकार की त्रुटि का सुधार नहीं किया जाएगा।</li>
                            <li>नीचे मुद्रित घोषणा-पत्र को विद्यार्थी व उसके माता / पिता/अभिभावक तथा शिक्षण संस्थान के प्रधान के द्वारा अनिवार्य रूप से हस्ताक्षरित किया जाएगा।</li>
                        </ol>
                    </div>

                    <div class="box mb-3">
                        <div class="section-title"><b><u>घोषणा-पत्र</u></b></div>
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


                </div>

                <b>
                    <div id="infoDiv"></div>
                </b>
            </div>

           
            <div style="page-break-after: always;" class="no-print"></div>
        </ItemTemplate>
    </asp:Repeater>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

  <script>
      async function generatePDF() {
          
          const { jsPDF } = window.jspdf;
          const pdf = new jsPDF('p', 'mm', 'a4');
          const elements = document.querySelectorAll('.container'); 

          for (let i = 0; i < elements.length; i++) {
              const element = elements[i];
              const canvas = await html2canvas(element, {
                  scale: 2,
                  useCORS: true
              });

              const imgData = canvas.toDataURL('image/jpeg', 1.0);
              const imgProps = pdf.getImageProperties(imgData);
              const pdfWidth = pdf.internal.pageSize.getWidth();
              const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

              if (i > 0) {
                  pdf.addPage();
              }
              pdf.addImage(imgData, 'JPEG', 0, 0, pdfWidth, pdfHeight);
          }

          pdf.save('AdmitCards.pdf');
      }
  </script>
    </form>

</body>
</html>
