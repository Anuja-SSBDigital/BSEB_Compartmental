<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commerce.aspx.cs" Inherits="commerce" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>BSEB Registration Form</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            line-height: 1.3;
        }

        .form-header {
            text-align: center;
            font-weight: bold;
        }

            .form-header .board-title {
                font-size: 18px;
                text-transform: uppercase;
            }

            .form-header .exam-title {
                font-size: 16px;
                text-decoration: underline;
                margin-bottom: 30px;
            }

        .faculty-box {
            text-align: right;
            margin-top: -30px;
            margin-bottom: 10px;
        }

            .faculty-box span {
                border: 1px solid black;
                padding: 5px 20px;
                font-weight: normal;
            }

        .hindi-line {
            text-align: center;
            margin-top: 10px;
            font-size: 16px;
        }

        .session-line {
            text-align: center;
            font-weight: bold;
            margin-top: 5px;
            font-size: 16px;
        }

            .session-line u {
                text-decoration: underline;
            }

        .container {
            width: 800px;
            margin: 0 auto;
            border: 1px solid #000;
            padding: 20px;
        }

        .header, .section-header {
            text-align: center;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .header, .section-header_b {
            text-align: center;
            font-weight: bold;
            /*border: 1px solid black;*/
            /*width: max-content;*/
            margin: 0 auto 10px auto; /* Center horizontally + 10px bottom margin */
        }

        .faculty {
            display: inline-block;
            border: 1px solid #000;
            padding: 2px 5px;
            background-color: #f0f0f0;
        }

        .notes, .section-a {
            border: 1px solid #000;
            padding: 10px;
            margin-bottom: 10px;
        }

            .notes p, .section-a p {
                margin: 5px 0;
            }

        .form-details {
            display: flex;
            justify-content: space-between;
        }

            .form-details table {
                width: 70%;
                border-collapse: collapse;
            }

            .form-details table, .form-details th, .form-details td {
                border: 1px solid #000;
                padding: 5px;
            }

        .photo-box {
            width: 25%;
            border: 1px solid #000;
            height: 180px;
            display: flex;
            justify-content: center;
            align-items: center;
            position: relative;
            background-color: #e0e0e0;
        }

            .photo-box::before {
                content: "✖";
                position: absolute;
                font-size: 60px;
                color: red;
                font-weight: bold;
            }

        .footer {
            text-align: right;
            margin-top: 10px;
        }

        table {
            border: 1px solid black;
            border-collapse: collapse;
            width: 100%;
        }

        th, td {
            border: 1px solid black;
            padding: 4px 8px;
            vertical-align: middle;
        }

        .photo-cell {
            width: 140px;
            height: 180px;
            text-align: center;
        }

            .photo-cell img {
                width: 100%;
                height: auto;
                max-height: 180px;
            }

        .bold {
            font-weight: bold;
        }

        .with-border {
            border: 2px solid #2196F3;
        }
        /* No border for these cells */
        .no-border {
            border: none;
        }

        .input-row {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
        }

            .input-row label {
                width: 300px;
            }

            .input-row input[type="text"],
            .input-row input[type="email"] {
                width: 300px;
                padding: 5px;
            }

        .checkbox-group {
            display: flex;
            gap: 20px;
        }

        .note {
            font-size: 12px;
            font-style: italic;
            margin-top: 5px;
        }

        .main-box {
            border: 1.5px solid #333;
            margin: 30px auto;
            padding: 18px 20px 10px 20px;
            max-width: 800px;
            background: #fff;
        }

        h2 {
            text-align: center;
            font-size: 1.18em;
            margin-bottom: 2px;
            margin-top: 0;
        }

        h3 {
            text-align: center;
            font-size: 1em;
            margin-top: 0;
            margin-bottom: 12px;
            font-weight: normal;
        }

        .instructions {
            font-size: 0.97em;
            margin-bottom: 16px;
            text-align: justify;
        }

        .voc-table {
            width: 100%;
            border-collapse: collapse;
            margin: 18px 0 10px 0;
        }

            .voc-table td, .voc-table th {
                border: none;
                padding: 3px 6px;
                font-size: 1em;
                vertical-align: middle;
            }

                .voc-table td.checkbox-cell {
                    width: 28px;
                    text-align: center;
                }

                .voc-table td.subject-label {
                    text-align: left;
                    white-space: nowrap;
                }

                .voc-table td.code {
                    width: 40px;
                    text-align: left;
                    color: #222;
                }

        .signature-row {
            margin-top: 18px;
            display: flex;
            justify-content: space-between;
        }

        .signature-box {
            border: 1.5px solid #333;
            width: 100%;
            height: 38px;
            margin-top: 0;
            margin-bottom: 0;
            display: flex;
            align-items: flex-end;
            justify-content: center;
            font-size: 0.97em;
            background: #fff;
        }

        .signature-label {
            text-align: center;
            font-size: 0.97em;
            margin-top: 2px;
        }

        .principal-section {
            margin-top: 18px;
            display: flex;
            justify-content: flex-end;
        }

        .principal-box {
            border: 1.5px solid #333;
            width: 250px;
            height: 115px;
            margin-left: auto;
            background: #fff;
            display: flex;
            align-items: flex-end;
            justify-content: center;
            font-size: 0.97em;
        }

        .note-box {
            border: 1px solid #aaa;
            background: #f9f9f9;
            font-size: 0.98em;
            padding: 7px 9px;
            margin-top: 14px;
            margin-bottom: 8px;
        }

        .footer {
            text-align: right;
            font-size: 0.93em;
            color: #444;
            margin-top: 6px;
        }

        .q33-box {
            border: 2px solid #000;
            padding: 16px;
            margin: 20px 0;
        }
    </style>
    <script>
        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }
    </script>
</head>
<body>
    <div class="print-btn-container">
        <input type="button" value="PRINT" class="noprint btn btn-success" onclick="printDiv('pagewrap')" />
    </div>
    <div class="container" id="pagewrap">
        <div class="form-header">
            <div class="board-title">BIHAR SCHOOL EXAMINATION BOARD, PATNA</div>
            <div class="exam-title">INTERMEDIATE EXAMINATION</div>
        </div>

        <div class="faculty-box">
            Faculty - <span>COMMERCE</span>
        </div>

        <div class="hindi-line">
            इंटरमीडिएट कक्षा में सत्र 2025-27 में मान्यता प्राप्त शिक्षण संस्थानों में नामांकित विद्यार्थियों के लिए
        </div>

        <div class="session-line">
            <u>REGISTRATION FORM - SESSION: 2025-27</u>
        </div>

        <div class="notes">
            <p><strong>नोट:-</strong></p>
            <p>(i) OFSS प्रणाली से नामांकन हेतु अपेक्षा दरें इस दस्तावेज के प्रारम्भ पर आपका नामांकन विवरण पत्र के रूप में छापा हुआ है। (छमाही- 1 से 16) में अधिकतम संख्याओं में मध्यवर्ती द्वारा किसी भी प्रकार की छेड़-छाड़/परिवर्तन नहीं किया जाएगा। अधिकतम संख्याएं- 1 से 16 तक मध्यवर्ती द्वारा कुछ भी लिखा जाएगा।</p>
            <p>(ii) मध्यवर्ती द्वारा इस दस्तावेज के प्रारम्भ में छापे गए "B" की स्थिति में छापे गए जानकारी।</p>
        </div>

        <div class="section-a">
            <p class="section-header">खण्ड 'A'</p>
            <p><strong>नोट:-</strong> खण्ड 'A' (छमाही- 1 से 16) में अधिकतम विषयों में मध्यवर्ती द्वारा किसी भी प्रकार की छेड़-छाड़/परिवर्तन नहीं किया जाएगा। अधिकतम संख्याएं- 1 से 16 तक मध्यवर्ती द्वारा कुछ भी लिखा जाएगा।</p>
        </div>

        <table>
            <tr>
                <td class="no-border">1. OFSS CAF No.</td>
                <td colspan="4">24J14143669</td>
                <td rowspan="9" class="photo-cell">
                    <img src="assets/img/users/user-5.png" alt="Student Photo"><br />
                    <img src="assets/img/ss.png" alt="Student Photo">
                </td>
            </tr>
            <tr>
                <td class="no-border">2. Category</td>
                <td class="bold" colspan="4">Regular</td>
            </tr>
            <tr>
                <td class="no-border">3. College/+2 School Code</td>
                <td colspan="4">11031</td>
            </tr>
            <tr>
                <td class="no-border">4. College/+2 School Name</td>
                <td colspan="4">PATNA COLLEGIATE SCHOOL, PATNA</td>
            </tr>
            <tr>
                <td class="no-border">5. District Name</td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td class="no-border">6. Student’s Name</td>
                <td colspan="4">AAKASH KUMAR</td>
            </tr>
            <tr>
                <td class="no-border">7. Mother’s Name</td>
                <td colspan="4">RAMRAKHI</td>
            </tr>
            <tr>
                <td class="no-border">8. Father’s Name</td>
                <td colspan="4">DEELIP RAM</td>
            </tr>
            <tr>
                <td class="no-border">9. Date of Birth</td>
                <td class="bold" colspan="4">14/03/2007</td>
            </tr>
            <tr>
                <td class="no-border">10. Matric/Class X Passing Board's Name</td>
                <td colspan="4">BSEB, Bihar</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">11. Matric/Class X Board’s Roll Code</td>
                <td colspan="4">71015</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">Roll Number</td>
                <td colspan="4">2200120</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">Passing Year</td>
                <td colspan="4">2022</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">12. Gender</td>
                <td colspan="4">Male</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">13. Caste Category</td>
                <td colspan="4">SC</td>
                <td class="no-border"></td>
            </tr>
            <tr>
                <td class="no-border">14. Differently abled</td>
                <td>NO</td>
                <td colspan="2">Specify (if yes)</td>
                <td colspan="1"></td>
            </tr>
            <tr>
                <td class="no-border">15. Nationality</td>
                <td></td>
                <td>Others</td>
                <td></td>
                <td>(As per Rule)</td>
            </tr>
            <tr>
                <td class="no-border">16. Religion</td>
                <td colspan="4"></td>
                <td class="no-border"></td>
            </tr>
        </table>
        <div class="section">
            <div class="section-header">
                <div class="section-a">
                    <p class="section-header_b">खण्ड - 'B'</p>
                    <p>(विद्यार्थी द्वारा मात्र खण्ड 'B' के बिन्दुओं (क्रमांक 17 से 33 तक) को ही भरा जाएगा)</p>
                </div>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <div class="input-row">
                        <label>
                            17. कृपया "आधार नंबर" अंकित करें।<br>
                            (Please mention the Aadhar Number)</label>

                    </div>
                    <div class="note">
                        PLEASE MENTION "AADHAR NUMBER".<br>
                        (If student has not enrolled in Aadhar and does not have "Aadhar number" then he/she is required to submit declaration in Sl. No. 18 that he/she has not been enrolled in Aadhar and has not got "Aadhar number").
                    </div>
                </td>
                <td>
                    <div class="input-row">
                        <input type="text" placeholder="Aadhar Number">
                    </div>
                </td>
            </tr>
        </table>
        <div class="section-a">
            <label>
                18. यदि स्थिति के द्वारा अनुग्रह कक्षा-17 में "आधार नंबर" अंकित नहीं किया गया है, तो उसे द्वारा निम्नलिखित घोषणा की जाएगी:<br>
                (If "Aadhar number" is not mentioned by me in class 17 due to certain reasons, then I am required to make the following declaration)</label>

            <div class="note">
                घोषणा नोट करें कि यहाँ कितने श्री/श्रीमती घोषणा करने के लिए विशेष रूप से कार्य करने को संकेत करता आधार नंबर नहीं होने के सम्बंध में निम्नलिखित घोषणा करें कि मेरे में आधार नंबर अंकित करने के लिए अवेलन नहीं किया गया है आधार नंबर अंकित न करने का एकमात्र कारण यह है कि मेरे आधार नंबर को किसी विशेष प्रयोजन के लिए आधार में अंकित करने हेतु नहीं किया गया है।<br>
                (If student has not got "Aadhar number" in Sl.No. 17 above, then following declaration should be given by student :- Please note that any WRONG DECLARATION made here, may invite action against the student and his/her candidature may be cancelled due to making false declaration about non-allotment of "Aadhar number")<br>
                DECLARATION: I hereby declare that I have not enrolled in Aadhar and have not got any "Aadhar number", I also understand that any false declaration made by me in this regard may have consequence of cancellation of my candidature.
            </div>

            <div class="input-row">
                <label>
                    Signature of student<br>
                    छात्र का हस्ताक्षर</label>
                <input type="text" placeholder="Signature">
            </div>
        </div>

        <div class="section">
            <div class="input-row section-a">
                <label>19. Area where the institution is situated (स्थान) (Please tick √)</label>
                <div class="checkbox-group">
                    <label>
                        <input type="checkbox">
                        Rural</label>
                    <label>
                        <input type="checkbox">
                        Urban</label>
                </div>
            </div>

            <div class="input-row">
                <label>20. Sub-Division (where the institution is situated)</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>21. Mobile No.</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>22. E-Mail Id</label>
                <input type="email">
            </div>

            <div class="input-row">
                <label>23. Student’s Name in Hindi</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>24. Mother’s Name in Hindi</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>25. Father’s Name in Hindi</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>26. Student’s Address</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>27. Marital Status (वैवाहिक स्थिति) (Please tick √)</label>
                <div class="checkbox-group">
                    <label>
                        <input type="checkbox">
                        Married (If Married, write word MARRIED in the box)</label>
                    <label>
                        <input type="checkbox">
                        Unmarried (If Unmarried, write word UNMARRIED in the box)</label>
                </div>
            </div>

            <div class="input-row">
                <label>28. Student’s Bank A/C No.*</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>29. IFSC Code*</label>
                <input type="text">
            </div>

            <div class="note">
                (Sl.No. 28, 29 and 30 are not compulsory, all other fields are compulsory)
            </div>

            <div class="input-row">
                <label>30. Bank & Branch Name*</label>
                <input type="text">
            </div>

            <div class="input-row">
                <label>31. Two identification marks of student</label>
                <div>
                    i.
                    <input type="text"><br>
                    ii.
                    <input type="text">
                </div>
            </div>

            <div class="input-row">
                <label>32. Medium (language) of appearing in examination (परीक्षा में उपस्थित होने का माध्यम) (Please tick √)</label>
                <div class="checkbox-group">
                    <label>
                        <input type="checkbox">
                        Hindi</label>
                    <label>
                        <input type="checkbox">
                        English</label>
                </div>
            </div>
        </div>
        <div class="section-title"><strong>33. Subject details with their numerical codes:-</strong></div>
        <div class="q33-box">
            <div class="section-title"><strong>Compulsory Subject Group (Total 200 Marks)</strong></div>
            <p>(Select (&#10003;) one subject from each group - each subject: 100 Marks)</p>
            <table>
                <tr>
                    <th colspan="2">Compulsory Subject Group-1 (100 Marks)<br>
                        (Select any one subject)</th>
                    <th colspan="2">Compulsory Subject Group-2 (100 Marks)<br>
                        (Select any one subject, which is not selected under Compulsory Subject Group-1)</th>
                </tr>
                <tr>
                    <td>English</td>
                    <td class="checkbox-cell">205
                        <input type="checkbox"></td>
                    <td>English</td>
                    <td class="checkbox-cell">205
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Hindi</td>
                    <td class="checkbox-cell">206
                        <input type="checkbox"></td>
                    <td>Hindi</td>
                    <td class="checkbox-cell">206
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Urdu</td>
                    <td class="checkbox-cell">207
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Maithili</td>
                    <td class="checkbox-cell">208
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Sanskrit</td>
                    <td class="checkbox-cell">209
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Prakrit</td>
                    <td class="checkbox-cell">210
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Magahi</td>
                    <td class="checkbox-cell">211
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Bhojpuri</td>
                    <td class="checkbox-cell">212
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Arabic</td>
                    <td class="checkbox-cell">213
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Persian</td>
                    <td class="checkbox-cell">214
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Pali</td>
                    <td class="checkbox-cell">215
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Bangla</td>
                    <td class="checkbox-cell">216
                        <input type="checkbox"></td>
                </tr>
            </table>
        </div>

        <div class="q33-box">
            <div class="section-title"><strong>Elective Subject Group (Total 300 Marks)</strong></div>
            <p>(Select (&#10003;) any three subjects - each 100 Marks)</p>
            <table>
                <tr>
                    <td>Business Studies</td>
                    <td class="checkbox-cell">217
                    <input type="checkbox"></td>
                    <td>Entrepreneurship</td>
                    <td class="checkbox-cell">218
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Economics</td>
                    <td class="checkbox-cell">219
                    <input type="checkbox"></td>
                    <td>Accountancy</td>
                    <td class="checkbox-cell">220
                    <input type="checkbox"></td>
                </tr>
            </table>
        </div>

        <div class="q33-box">
            <div class="section-title"><strong>Additional Subject Group (100 Marks)</strong></div>
            <ol>
                <li>The student who desires to keep additional subject, must select (&#10003;) any one subject from following subject group which he/she has not selected under compulsory subject group-1 or compulsory subject group-2 or elective subject group.</li>
                <li>The student who does not want to keep additional subject, need not to select any subject under this subject group.</li>
            </ol>
            <table>
                <tr>
                    <td>Business Studies</td>
                    <td class="checkbox-cell">217
                    <input type="checkbox"></td>
                    <td>Entrepreneurship</td>
                    <td class="checkbox-cell">218
                    <input type="checkbox"></td>
                    <td>Economics</td>
                    <td class="checkbox-cell">219
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Accountancy</td>
                    <td class="checkbox-cell">220
                    <input type="checkbox"></td>
                    <td>Computer Science</td>
                    <td class="checkbox-cell">221
                    <input type="checkbox"></td>
                    <td>Multimedia & Web Tech</td>
                    <td class="checkbox-cell">222
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>English</td>
                    <td class="checkbox-cell">223
                    <input type="checkbox"></td>
                    <td>Hindi</td>
                    <td class="checkbox-cell">224
                    <input type="checkbox"></td>
                    <td>Urdu</td>
                    <td class="checkbox-cell">225
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Maithili</td>
                    <td class="checkbox-cell">226
                    <input type="checkbox"></td>
                    <td>Sanskrit</td>
                    <td class="checkbox-cell">227
                    <input type="checkbox"></td>
                    <td>Prakrit</td>
                    <td class="checkbox-cell">228
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Magahi</td>
                    <td class="checkbox-cell">229
                    <input type="checkbox"></td>
                    <td>Bhojpuri</td>
                    <td class="checkbox-cell">230
                    <input type="checkbox"></td>
                    <td>Arabic</td>
                    <td class="checkbox-cell">231
                    <input type="checkbox"></td>
                </tr>
                <tr>
                    <td>Persian</td>
                    <td class="checkbox-cell">232
                    <input type="checkbox"></td>
                    <td>Pali</td>
                    <td class="checkbox-cell">233
                    <input type="checkbox"></td>
                    <td>Bangla</td>
                    <td class="checkbox-cell">234
                    <input type="checkbox"></td>
                </tr>

            </table>
            <div class="note">
                <b>Note:</b> Computer Science, Yoga & Phy. Edu. and Multimedia & Web.Tech. cannot be interchanged/swapped with any other subject.
            </div>
        </div>


        <div class="q33-box">
            <h2>Vocational Trade Group (100 Marks)</h2>
            <h3>(For students of Science, Commerce & Arts faculties)</h3>

            <div class="instructions">
                (i) विज्ञान, वाणिज्य एवं कला संकायों के विद्यार्थियों के लिए सत्र 2022-24 में राज्य के सभी जिलों में खुले विभिन्न +2 विद्यालय/विद्यालयों में विभिन्न व्यावसायिक ट्रेड के एक-एक विषय की शुरुआत की गई है, जिनकी सूची समिति के पोर्टल/वेबसाइट पर प्रकाशित की गई है। व्यावसायिक के एक-एक विषय अनिवार्य (Compulsory), वैकल्पिक (Elective) एवं अतिरिक्त (Additional) विषय समूहों के अंतर्गत हैं।<br>
                (ii) उपरोक्त +2 विद्यालयों, जिनकी सूची समिति के पोर्टल/वेबसाइट पर प्रकाशित की गई है, के कैसे विद्यालयों, जिनके व्यावसायिक के अंतर्गत नामित किया गया है, के द्वारा इस सूची में से किसी एक एक विषय का चयन किया जाता एवं स्कूल परीक्षा (सहायक एवं प्रायोगिक) में अनिवार्य है। यदि कोई, इनके प्रबंधन के कारण श्रेणी निर्धारण हेतु सूचि में शामिल नहीं है तो विषय के प्रबंधन के परस्पर (Interchange/Swap) में चयन किया जाएगा।<br>
                (iii) राज्य/प्रांतीय +2 विद्यालय, जिनमें व्यावसायिक ट्रेड का चयन-पाठन हेतु चिन्हित नहीं है, के विद्यालयों द्वारा इस ट्रेड समूह में से किसी ट्रेड का चयन नहीं किया जाएगा।
            </div>

            <table class="voc-table">
                <tr>
                    <td class="subject-label">Security</td>
                    <td class="code">235</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">Beautician</td>
                    <td class="code">236</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">Tourism</td>
                    <td class="code">237</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td class="subject-label">Retail Management</td>
                    <td class="code">238</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">Automobile</td>
                    <td class="code">239</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">Electronics &amp; H/W 347 H/W</td>
                    <td class="code">240</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                </tr>
                <tr>
                    <td class="subject-label">Beauty &amp; Wellness</td>
                    <td class="code">241</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">Telecom</td>
                    <td class="code">242</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                    <td class="subject-label">IT/ITes</td>
                    <td class="code">243</td>
                    <td class="checkbox-cell">
                        <input type="checkbox"></td>
                </tr>
            </table>
        </div>

        <div class="note-box">
            प्रमाणित किया जाता है कि इस आवेदन पत्र में दी गई सूचनाएं पूरी तरह से सही एवं सच्ची हैं और इनमें कहीं पर भी कोई जानकारी गलत नहीं है। इस आवेदन पत्र के साथ संलग्न सभी प्रमाण पत्र सही हैं। यदि भविष्य में कोई जानकारी असत्य पाई जाती है, तो इसकी जिम्मेदारी मेरी होगी।
        </div>

        <div class="signature-row">
            <div>
                <div class="signature-box"></div>
                <div class="signature-label">Signature of Parent/Guardian</div>
            </div>
            <div>
                <div class="signature-box"></div>
                <div class="signature-label">Student's Signature in Hindi</div>
            </div>
            <div>
                <div class="signature-box"></div>
                <div class="signature-label">Student's Signature in English</div>
            </div>
        </div>

        <div class="note-box">
            प्रमाणित किया जाता है कि उपर्युक्त छात्र/छात्रा का चयन सम्बन्धित संकाय/व्यावसायिक/+2 विद्यालय के सभी अभिलेखों से पूर्णतः सत्य पाया गया है। तदनुसार उक्त विद्यार्थी का सूचनांक/अनुशंसा आवेदन पत्र स्वीकृत किया जाता है।
        </div>

        <div class="principal-section">
            <div>
                <div class="principal-box"></div>
                <div class="signature-label">Signature &amp; seal of Principal</div>
            </div>
        </div>
    </div>
</body>
</html>
