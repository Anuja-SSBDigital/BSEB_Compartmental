<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExaminationForm.aspx.cs" Inherits="ExaminationForm" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>BSEB Examination Form</title>
    <style>
        /* Your existing CSS styles go here */
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        .lineheart_a {
            line-height: 1.6;
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
            page-break-after: always; /* This will create a new page for each student */
        }

        .header, .section-header {
            text-align: center;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .header, .section-header_b {
            text-align: center;
            font-weight: bold;
            margin: 0 auto 10px auto;
        }

        .faculty {
            display: inline-block;
            border: 1px solid #000;
            padding: 2px 5px;
            background-color: #f0f0f0;
        }

        .notes, .section-a {
            border: 1px solid #000;
            padding: 2px;
            margin-bottom: 4px;
        }

            .notes p, .section-a p {
                margin: 0px 0;
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
            /*border: 1px solid black;*/
            /*border-collapse: collapse;*/
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
            gap: 10px; /* space between label and input */
            flex-wrap: wrap; /* wrap if screen is small */
        }

            .input-row label {
                width: 180px; /* fixed width for labels */
                /*font-weight: bold;*/
            }

        .form-input {
            flex: 1; /* take remaining space */
            padding: 5px 10px;
            border: 1px solid #000;
            border-radius: 3px;
            font-size: 14px;
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
                border: solid 1px;
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

        .signature-row2 {
            margin-top: 0px;
            display: flex;
            justify-content: flex-end;
        }

        .signature-box2 {
            border: 1.5px solid #333;
            width: 250px;
            height: 50px;
            margin-top: 0;
            margin-bottom: 0;
            display: flex;
            align-items: flex-end;
            justify-content: center;
            font-size: 0.97em;
            background: #fff;
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
            border: 12px solid #aaa;
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

        .boxinside {
            border: 1px solid #000;
            margin-bottom: 2px;
        }

        .subject-group {
            margin-bottom: 20px;
            padding: 10px;
            border: 1px solid #ccc;
        }

            .subject-group h4 {
                margin-top: 0;
            }

        #section_1733 {
            margin-top: 20px;
            margin-bottom: -10px;
        }

        .btn {
            padding: 10px 20px;
            background-color: #3f51b5; /* Fluree-style blue */
            color: #fff;
            font-size: 16px;
            font-weight: 500;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 8px rgba(63, 81, 181, 0.2);
        }

            .btn:hover {
                background-color: #303f9f;
                box-shadow: 0 6px 12px rgba(63, 81, 181, 0.3);
            }

            .btn:active {
                background-color: #1a237e;
                transform: scale(0.98);
            }

            .btn:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

        @media print {
            .container {
                border: none;
            }

            .page {
                page-break-after: always !important;
                page-break-inside: avoid !important;
            }

                .page:last-child {
                    page-break-after: auto !important;
                }
        }

        .form-container {
            border: 1px solid #000;
            padding: 10px;
            margin-bottom: 20px;
            font-size: 13px;
            font-family: Arial, sans-serif;
        }

        .note-box {
            border: 2px solid #000;
            padding: 6px;
            margin-bottom: 8px;
            font-size: 12px;
        }

            .note-box.small {
                font-size: 11px;
            }

        .section-header {
            margin: 0 auto; /* center the whole div */
        }

        .section-header {
            text-align: center;
            font-weight: bold;
            border: 1px solid #000;
            padding: 20px;
            margin: 8px 0;
        }



        .section-header2 {
            margin: 0 auto; /* center the whole div */
        }

        .section-header2 {
            text-align: center;
            font-weight: bold;
            border: 1px solid #000;
            padding: 4px;
            margin: 8px 0;
        }

        .section-header3 {
            margin: 0 auto !important; /* center the whole div */
        }

        .section-header3 {
            text-align: center;
            font-weight: bold;
            border: 1px solid #000;
            width: 145px;
            padding: 4px;
        }

        .form-table td {
            border: 1px solid #000;
            padding: 5px;
            vertical-align: top;
        }

            /* Remove left border of 2nd column */
            .form-table td + td {
                border-left: none;
            }

        .photo-cell {
            text-align: center;
            width: 120px;
        }

            .photo-cell img {
                width: 100px;
                height: 120px;
                border: 1px solid #000;
            }

        .signature {
            margin-top: 4px;
            font-size: 11px;
        }

        .digit-boxes {
            display: flex;
            gap: 0px;
        }

        .digit-box {
            display: inline-block;
            width: 25px;
            height: 25px;
            text-align: center;
            border: 1px solid #000;
            font-weight: bold;
            line-height: 25px;
            font-size: 14px;
        }

        .separator {
            margin: 0 4px;
            font-weight: bold;
        }

        .checkbox-group {
            display: flex;
            gap: 20px; /* spacing between checkboxes */
            margin-top: 5px;
            flex-wrap: wrap;
        }

        .checkbox-label {
            display: flex;
            align-items: center;
            gap: 5px;
            font-size: 14px;
        }

            .checkbox-label .note {
                font-size: 12px;
                color: #555;
            }
        /*
        #pagewrap {
            max-width: 900px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 14px;
        }
*/
        #pagewrap .section-title {
            font-size: 13px;
        }



        /* Only remove borders for subject tables inside this section */
        /*   #pagewrap .q33-box table,
        #pagewrap .voc-table {
            border-collapse: collapse;
            border: 1px solid black;
        }*/

        #pagewrap .q33-box table th,
        #pagewrap .q33-box table td,
        #pagewrap .voc-table th,
        #pagewrap .voc-table td {
            border: none; /* remove borders only here */
            /* padding: 8px 10px;
                    text-align: left;
                    vertical-align: middle;*/
        }

        #pagewrap .q33-box table th {
            font-weight: bold;
            text-align: center;
            padding-bottom: 12px;
        }

        #pagewrap input[type="checkbox"] {
            transform: scale(1.2);
            margin-left: 5px;
        }

        #pagewrap .note, #pagewrap .note-box {
            background-color: #fcfcfc;
            /*                border-left: 4px solid #3498db;
*/ padding: 8px 12px;
            margin-top: 10px;
            font-size: 15px;
            color: black;
        }

        #pagewrap .signature-row {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
        }

        #pagewrap .principal-section {
            display: flex;
            justify-content: flex-end;
            margin-top: 30px;
        }

        #pagewrap .signature-box {
            width: 200px;
            height: 50px;
            border: 1px solid #333;
            border-radius: 4px;
            background: #fff;
        }

        #pagewrap .signature-label {
            text-align: center;
            margin-top: 5px;
            font-size: 13px;
        }

        #pagewrap .checkbox-cell input[type="checkbox"] {
            transform: scale(1.2);
            margin-left: 10px;
        }

        #pagewrap .instructions {
            font-size: 15px;
            color: black;
            line-height: 2;
            margin-bottom: 10px;
        }

        .tbl-2 {
            border: none;
            width: 100%;
        }

        }
    </style>
    <style type="text/css" media="print">
        #btnPrint {
            display: none !important;
        }

        #btnback {
            display: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="print-btn-container" style="text-align: center !important; margin-bottom: 10px;">
            <%-- <asp:Button ID="btnPrint" runat="server" Text="PDF" OnClientClick="window.print(); return false;" />--%>
            <a href="DwnldExamForm.aspx" class="btn btn-primary no-print" id="btnback" style="text-decoration: none !important;">Back</a>
            <asp:Button ID="btnPrint" runat="server" Text="PDF" OnClick="btnPrint_Click" CssClass="btn" />

        </div>

        <asp:Repeater ID="rptStudents" runat="server" OnItemDataBound="rptStudents_ItemDataBound">
            <ItemTemplate>

                <div class="container">
                    <div class="lineheart_a">
                        <div class="page" id="page1">
                            <div class="form-header">
                                <div class="board-title" style="font-size: x-large;">BIHAR SCHOOL EXAMINATION BOARD</div>
                                <div class="exam-title"></div>

                                <div class="faculty-box">

                                    <span>Faculty -<%# Eval("FacultyName") %></span>

                                </div>
                                <div class="hindi-line">
                                    <strong>Online Examination Application Form for Intermediate Annual Examination, 2026
                                    <p>(Only for Regular & Private student's registered for Session-2024-26)</p></strong>
                                </div>

                                <div class="session-line">
                                    <u>EXAMINATION FORM-    SESSION:2024-26</u>
                                </div>
                            </div>

                            <div class="notes">

                                <p><strong>नोट:-(i) सूचीकरण हेतु आपके द्वारा दी गई सूचनाओं के आधार पर आपका सूचीकरण विवरण इस प्रपत्र के खण्ड 'A' (क्रमांक- 1 से 17) में अंकित है।</p>
                                <p>(ii) खण्ड 'A' (क्रमांक- 1 से 17) के अंकित विवरणों में विद्यार्थी द्वारा किसी भी प्रकार का कोई छेड़-छाड़/परिवर्तन नहीं किया जाएगा। अर्थात् क्रमांक- 1 से 17 तक में विद्यार्थी द्वारा कुछ भी नहीं लिखा जाएगा।</p>
                                <p>(iii) विद्यार्थी द्वारा इस आवेदन प्रपत्र में मात्र खण्ड 'B' के बिन्दुओं को ही भरा जाएगा।</p></strong>
                            </div>

                            <div class="section-header2">खण्ड – 'A'</div>



                            <div class="section-a">



                                <p><strong>नोट:-खण्ड 'A' (क्रमांक- 1 से 17) के अंकित विवरणों में विद्यार्थी द्वारा किसी भी प्रकार का कोई छेड़-छाड़/परिवर्तन नहीं किया जाएगा। अर्थात् क्रमांक- 1 से 17 तक में विद्यार्थी द्वारा कुछ भी नहीं लिखा जाएगा।</p>
                                </strong>

                                <table>

                                    <tr>
                                        <td class="no-border">1. Registration No. & Year.</td>
                                        <td colspan="4"><%# Eval("RegistrationNo") %></td>
                                        <td rowspan="9" class="photo-cell no-border">
                                            <img src='<%# ResolveUrl(Eval("StudentPhotoPath").ToString()) %>' alt="Student Photo" /><br />
                                            <img src='<%# ResolveUrl(Eval("StudentSignaturePath").ToString()) %>' alt="Student Signature" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">2. BSEB Unique ID </td>
                                        <td colspan="4"><%# Eval("UniqueNo") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">3.Student Category</td>
                                        <td colspan="4"><%# Eval("CategoryName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">4. College/+2 School Code</td>
                                        <td class="value-cell">
                                            <div class="digit-boxes">
                                                <span class="digit-box"><%# Eval("CollegeCode").ToString().Length > 0 ? Eval("CollegeCode").ToString()[0].ToString() : "" %></span>
                                                <span class="digit-box"><%# Eval("CollegeCode").ToString().Length > 1 ? Eval("CollegeCode").ToString()[1].ToString() : "" %></span>
                                                <span class="digit-box"><%# Eval("CollegeCode").ToString().Length > 2 ? Eval("CollegeCode").ToString()[2].ToString() : "" %></span>
                                                <span class="digit-box"><%# Eval("CollegeCode").ToString().Length > 3 ? Eval("CollegeCode").ToString()[3].ToString() : "" %></span>
                                                <span class="digit-box"><%# Eval("CollegeCode").ToString().Length > 4 ? Eval("CollegeCode").ToString()[4].ToString() : "" %></span>
                                            </div>
                                        </td>
                                        <%--<td colspan="4"><%# Eval("CollegeCode") %></td>--%>
                                    </tr>
                                    <tr>
                                        <td class="no-border">5. College/+2 School Name</td>
                                        <td colspan="4"><%# Eval("CollegeName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">6. District Name</td>
                                        <td colspan="4"><%# Eval("DistrictName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">7. Student’s Name</td>
                                        <td colspan="4"><%# Eval("StudentName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">8. Mother’s Name</td>
                                        <td><%# Eval("MotherName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">9. Father’s Name</td>
                                        <td><%# Eval("FatherName") %></td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">10. Date of Birth</td>
                                        <td class="no-border">
                                            <div class="digit-boxes">
                                                <!-- Day -->
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 0) %></span>
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 1) %></span>

                                                <!-- Month -->
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 2) %></span>
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 3) %></span>

                                                <!-- Year -->
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 4) %></span>
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 5) %></span>
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 6) %></span>
                                                <span class="digit-box"><%# GetDobDigit(Eval("DOB"), 7) %></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="no-border">11. Matric/Class X Passing Board’s Name</td>
                                        <td><%# Eval("MatricBoardName") %></td>
                                    </tr>
                                </table>
                                <table class="tbl-2">
                                    <tr>
                                        <td class="no-border">12. Matric/Class X Board’s  </td>
                                        <td class="no-border">
                                            <div class="matric-boxes">

                                                <!-- Roll Code -->
                                                <label>Roll Code:</label>
                                                <asp:TextBox ID="txtRollCode" runat="server" MaxLength="4" Width="85px"
                                                    Text='<%# Eval("MatricRollCode") %>'></asp:TextBox>

                                                &nbsp;&nbsp;

            <!-- Roll Number -->
                                                <label>Roll Number:</label>
                                                <asp:TextBox ID="txtRollNo" runat="server" MaxLength="4" Width="85px"
                                                    Text='<%# Eval("MatricRollNumber") %>'></asp:TextBox>

                                                &nbsp;&nbsp;

            <!-- Passing Year -->
                                                <label>Passing Year:</label>
                                                <asp:TextBox ID="txtPassingyear" runat="server" MaxLength="4" Width="85px"
                                                    Text='<%# Eval("MatricPassingYear") %>'></asp:TextBox>

                                            </div>
                                        </td>
                                    </tr>

                                    <!-- ✅ Redesigned part (15–17) -->

                                    <tr>
                                        <td class="no-border">13. Gender</td>
                                        <td class="no-border">
                                            <input type="checkbox" <%# Eval("Gender").ToString() == "Male" ? "checked" : "" %> />
                                            Male
        <input type="checkbox" <%# Eval("Gender").ToString() == "Female" ? "checked" : "" %> />
                                            Female
        <input type="checkbox" <%# Eval("Gender").ToString() == "Others" ? "checked" : "" %> />
                                            Others
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="no-border">14. Caste Category</td>
                                        <td class="no-border">
                                            <input type="checkbox" <%# Eval("CasteCategory").ToString() == "General" ? "checked" : "" %> />
                                            General
        <input type="checkbox" <%# Eval("CasteCategory").ToString() == "SC" ? "checked" : "" %> />
                                            SC
        <input type="checkbox" <%# Eval("CasteCategory").ToString() == "ST" ? "checked" : "" %> />
                                            ST
        <input type="checkbox" <%# Eval("CasteCategory").ToString() == "EBC" ? "checked" : "" %> />
                                            EBC
        <input type="checkbox" <%# Eval("CasteCategory").ToString() == "BC" ? "checked" : "" %> />
                                            BC
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="no-border">15. Differently Abled</td>
                                        <td class="no-border">
                                            <input type="checkbox" <%# Eval("DifferentlyAbled").ToString() == "Yes" ? "checked" : "" %> />
                                            Yes
        <input type="checkbox" <%# Eval("DifferentlyAbled").ToString() == "No" ? "checked" : "" %> />
                                            No

                                         &nbsp;&nbsp;
                                        <label>Specify (if yes):</label>
                                            <input type="text" runat="server" id="Text4" maxlength="4" style="width: 270px;" />

                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="no-border">16. Nationality</td>
                                        <td class="no-border">
                                            <input type="checkbox" <%# Eval("Nationality").ToString() == "Indian" ? "checked" : "" %> />
                                            Indian
        <input type="checkbox" <%# Eval("Nationality").ToString() == "Other" ? "checked" : "" %> />
                                            Other
                                         &nbsp;&nbsp;
                                      
                                            <input type="text" runat="server" id="Text5" maxlength="4" style="width: 240px;" />
                                            <label>(As per Rule)</label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="no-border">17. Religion</td>
                                        <td class="no-border">
                                            <input type="checkbox" <%# Eval("Religion").ToString() == "HINDU" ? "checked" : "" %> />
                                            Hindu
        <input type="checkbox" <%# Eval("Religion").ToString() == "ISLAM" ? "checked" : "" %> />
                                            Islam
        <input type="checkbox" <%# Eval("Religion").ToString() == "SIKH" ? "checked" : "" %> />
                                            Sikh
        <input type="checkbox" <%# Eval("Religion").ToString() == "CHRISTIAN" ? "checked" : "" %> />
                                            Christian
        <input type="checkbox" <%# Eval("Religion").ToString() == "JAIN" ? "checked" : "" %> />
                                            Jain
        <input type="checkbox" <%# Eval("Religion").ToString() == "BUDHIST" ? "checked" : "" %> />
                                            Baudh
        <input type="checkbox" <%# Eval("Religion").ToString() == "OTHERS" ? "checked" : "" %> />
                                            Others
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>
                        <div class="page form-container" id="page2">
                            <div class="section" id="">


                                <div class="section-header2">खण्ड - 'B'</div>
                                <center>
                                    <p>
                                        (विद्यार्थी द्वारा केवल खंड 'B' के बिंदुओं (क्रमांक 18 से 34 तक) को ही भरा जाएगा)
                                    </p>
                                </center>


                            </div>
                            <table id="section_1733_table">
                                <tr>
                                    <td>
                                        <div class="" style="display: block !important;">
                                            <label><strong>18. कृपया ''आधार नंबर'' अंकित करें। </strong></label>
                                            <p>यदि विद्यार्थी का ''आधार नंबर'' आवंटित नहीं हुआ है, तो विद्यार्थी के द्वारा इस आशय की घोषणा क्रमांक–19 में की जानी आवश्यक होगी कि उन्हें ''आधार नंबर'' आवंटित नहीं हुआ है।)</p>
                                        </div>
                                        <div class="" style="font-style: normal; margin-top: -20px;">
                                            <b>PLEASE MENTION "AADHAR NUMBER”.
                                            </b>
                                            <br>
                                            (If student has not enrolled in Aadhar and doesn't have "Aadhar number" then he/she is required to submit declaration in Sl. No. 19 that he/she has not been enrolled in Aadhar and has not got "Aadhar number".)                                       
                                        </div>
                                    </td>
                                    <td class="no-border">
                                        <div class="digit-boxes">
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 0 ? Eval("AadharNumber").ToString().Substring(0,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 1 ? Eval("AadharNumber").ToString().Substring(1,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 2 ? Eval("AadharNumber").ToString().Substring(2,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 3 ? Eval("AadharNumber").ToString().Substring(3,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 4 ? Eval("AadharNumber").ToString().Substring(4,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 5 ? Eval("AadharNumber").ToString().Substring(5,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 6 ? Eval("AadharNumber").ToString().Substring(6,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 7 ? Eval("AadharNumber").ToString().Substring(7,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 8 ? Eval("AadharNumber").ToString().Substring(8,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 9 ? Eval("AadharNumber").ToString().Substring(9,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 10 ? Eval("AadharNumber").ToString().Substring(10,1) : "" %>--%></span>
                                            <span class="digit-box"><%--<%# Eval("AadharNumber").ToString().Length > 11 ? Eval("AadharNumber").ToString().Substring(11,1) : "" %>--%></span>

                                        </div>
                                        <p style="text-align: center;">
                                            आधार नम्बर
                                            <br />
                                            Aadhar number

                                        </p>
                                    </td>
                                </tr>
                            </table>
                            <div class="section-a">
                                <p>
                                    19. यदि विद्यार्थी के द्वारा उपर्युक्त क्रमांक-18 में ’’आधार नंबर’’ अंकित नहीं किया गया है, तो उनके द्वारा निम्नांकित घोषणा की जाएगीः-
                                    <br>
                                    <strong>(कृपया नोट करें कि यहाँ किसी भी तरह की गलत घोषणा के लिए विद्यार्थी के विरूद्ध कार्रवाई की जा सकेगी तथा आधार नम्बर नहीं होने के संबंध में इस मिथ्या/गलत घोषणा के कारण उनका अभ्यर्थित्व रद्द किया जा सकता है।)
                                    </strong>
                                </p>
                                <div class="note" style="text-decoration: underline; font-style: normal;">
                                    <strong>घोषणा:- मैं, एतद् द्वारा घोषित करता हूँ कि मैंने ‘‘आधार नंबर’’ आवंटित करने के लिए आवेदन नहीं किया है तथा मुझे ‘‘आधार नंबर’’ आवंटित नहीं हुआ है। मैं यह भी समझता हूँ कि मेरे द्वारा की गई इस मिथ्या/गलत घोषणा के आधार पर मेरा अभ्यर्थित्व रद्द किया जा सकता है।
                                    </strong>
                                    <p>
                                        If student has not given "Aadhar number" in Sl. No. 18 above, then following declaration should be given by student :-
                                    </p>
                                    <p>
                                        <strong>(Please note that any WRONG DECLARATION made here, may invite action against the student and his/her candidature may be cancelled due to making false declaration about non-allotment of "Aadhar number")<br />
                                            DECLARATION :- I, hereby declare that I have not enrolled in Aadhar and have not got any "Aadhar number". I also understand that any false declaration made by me in this regard may have consequence of cancellation of my candidature.</strong>
                                    </p>
                                </div>
                                <div class="signature-row2">
                                    <div>
                                        <div class="signature-box2"></div>
                                        <div class="signature-label">
                                            Signature of student<br>
                                            विद्यार्थी का हस्ताक्षर
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="section">


                                <div class="input-row">
                                    <label style="width: 330px;">20. Area where the institution is situated <strong>(कृपया ✓ करें)</strong></label>
                                    <div class="checkbox-group">
                                        <label>
                                            <input type="checkbox" disabled="disabled" <%# Eval("AreaName") != null && Eval("AreaName").ToString() == "Rural" ? "checked" : "" %> />
                                            Rural
                                        </label>
                                        <label>
                                            <input type="checkbox" disabled="disabled" <%# Eval("AreaName") != null && Eval("AreaName").ToString() == "Urban" ? "checked" : "" %> />
                                            Urban
                                        </label>
                                    </div>
                                </div>


                                <div class="input-row">
                                    <label>21. Sub-Division (where the institution is situated)</label>
                                    <div class="subdivision-row">
                                        <%-- <%# Eval("SubDivisionName") %>--%> &nbsp;
                                         <input type="text" class="form-input" />
                                        <%--  <label>
                                            <input type="checkbox" name="rural_<%# Container.ItemIndex %>" value="Rural" />
                                            Rural
                                        </label>
                                        &nbsp;
                                        <label>
                                            <input type="checkbox" name="urban_<%# Container.ItemIndex %>" value="Urban" />
                                            Urban
                                        </label>--%>
                                    </div>
                                </div>

                                <!-- Mobile and Email -->
                                <div class="input-row">
                                    <label>22. Mobile No.</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("MobileNo") %>--%>' readonly />

                                    <label>23. E-Mail Id</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("EmailId") %>--%>' readonly />
                                </div>

                                <!-- Student’s Name in Hindi -->
                                <div class="input-row">
                                    <label>24. Student’s Name in Hindi</label>
                                    <input type="text" class="form-input" />
                                </div>

                                <!-- Mother’s Name in Hindi -->
                                <div class="input-row">
                                    <label>25. Mother’s Name in Hindi</label>
                                    <input type="text" class="form-input" />
                                </div>

                                <!-- Father’s Name in Hindi -->
                                <div class="input-row">
                                    <label>26. Father’s Name in Hindi</label>
                                    <input type="text" class="form-input" />
                                </div>

                                <!-- Student’s Address -->
                                <div class="input-row">
                                    <label>27. Student’s Address in English</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("StudentAddress") %>--%>' />
                                </div>
                                <div class="input-row">
                                    <label>28. Marital Status (वैवाहिक स्थिति)</label>
                                    <div class="subdivision-row">

                                        <input type="checkbox" />
                                        Married
                                        
                                       
                                            <input type="checkbox" />
                                        Unmarried
                                       
                                    </div>

                                    <!-- Two textboxes after the checkboxes -->


                                    <input type="text" class="marital-status-box" style="width: 150px; margin-right: 10px;" placeholder="If Married write in this" />
                                    <input type="text" class="marital-status-box" style="width: 150px;" placeholder="If Unmarried write in this " />



                                </div>
                                <div class="input-row">
                                    <label>29. Student’s Bank A/C No.*</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("StudentBankAccountNo") %>--%>' />

                                    <label>30. IFSC Code*</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("IFSCCode") %>--%>' />
                                </div>



                                <div class="input-row">
                                    <label>31. Bank & Branch Name*</label>
                                    <input type="text" class="form-input" value='<%--<%# Eval("BankBranchName") %>--%>' />
                                    <label style="font-size: smaller;">(Sl.No. 29, 30 ,31 are not compulsory, all other fields are compulsory)</label>

                                </div>

                                <div class="input-row">
                                    <label>32. Two identification marks of student</label>
                                    <div>
                                        i.
                                     <input type="text" class="form-input" value="<%--<%# Eval("IdentificationMark1") %>--%>">

                                        <br>
                                        ii.
                                   <input type="text" class="form-input" value="<%--<%# Eval("IdentificationMark2") %>--%>">
                                    </div>
                                </div>

                                <div class="input-row">
                                    <label style="width: 580px;">
                                        33. Medium (language) of appearing in examination (परीक्षा में उपस्थित होने का माध्यम) (Please tick √)</label>
                                    <div class="subdivision-row">


                                        <input type="checkbox" disabled="disabled" />
                                        <%-- <%# Eval("MediumName") != null && Eval("MediumName").ToString().ToLower() == "hindi" ? "checked" : "" %> />--%>
                                        Hindi
                                     
                                            <input type="checkbox" disabled="disabled" />
                                        <%--  <%# Eval("MediumName") != null && Eval("MediumName").ToString().ToLower() == "english" ? "checked" : "" %> />--%>
                                        English

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div id="pagewrap">
                        <div class="page page3" id="page3">
                            <div class="q33-box">
                                <div class="section-title" style="padding-left: 5px;"><strong>34. Subject details with their numerical codes:-</strong></div>
                                <div class="boxinside">
                                    <div class="section-title" style="text-align: center;">
                                        <strong>Compulsory Subject Group (Total 200 Marks)</strong>
                                        <p>(Select (&#10003;) one subject from each group - each subject: 100 Marks)</p>
                                    </div>
                                </div>
                                <table class="boxinside">
                                    <tr>
                                        <th colspan="2" style="border-right: 1px solid black; border-bottom: 1px solid;">Compulsory Subject Group-1 (100 Marks)<br>
                                            (Select any one subject)</th>
                                        <th colspan="2" style="border-bottom: 1px solid;">Compulsory Subject Group-2 (100 Marks)<br>
                                            (Select any one subject, which is not selected under Compulsory Subject Group-1)</th>
                                    </tr>
                                    <asp:Repeater ID="rptCompulsorySubjectsCombined" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Group1SubjectName") %>
                                            </td>
                                            <td>
                                                <%# Eval("Group1SubjectCode") %>  <%-- ✅ New Code column --%>
                                            </td>
                                            <td style="border-right: 1px solid black">
                                                <%# Eval("Group1CheckboxHtml") %>
                                            </td>

                                            <td>
                                                <%# Eval("Group2SubjectName") %>
                                            </td>
                                            <td>
                                                <%# Eval("Group2SubjectCode") %>  <%-- ✅ New Code column --%>
                                            </td>
                                            <td style="border-right: 1px solid black">
                                                <%# Eval("Group2CheckboxHtml") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                                </table>
                                <div class="boxinside">
                                    <div class="section-title" style="text-align: center;">
                                        <strong>Elective Subject Group (Total 300 Marks)</strong>
                                        <p>(Select (&#10003;) any three subjects - each 100 Marks)</p>
                                    </div>
                                    <table>
                                        <asp:Repeater ID="rptElectiveSubjects" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Name1") %></td>
                                                    <td>
                                                        <%# Eval("Checkbox1Html") %>
                                                    </td>
                                                    <td><%# Eval("Name2") %></td>
                                                    <td>
                                                        <%# Eval("Checkbox2Html") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <asp:Repeater ID="rptVocElectiveSubjects" runat="server" Visible="false">
                                            <ItemTemplate>
                                                <tr>
                                                    <!-- Subject 1 -->
                                                    <td>
                                                        <%# Eval("Name1") %>
                                                    
                                                    </td>
                                                    <td>
                                                        <span><%# Eval("Code1") %></span> <%# Eval("Checkbox1Html") %>
                                                    </td>
                                                    <!-- Subject 2 -->
                                                    <td>
                                                        <%# Eval("Name2") %>
                                                    
                                                    </td>
                                                    <td>
                                                        <span><%# Eval("Code2") %></span>  <%# Eval("Checkbox2Html") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </table>
                                </div>
                                <div class="boxinside">
                                    <div class="section-title" style="text-align: center;">
                                        <strong>Additional Subject Group (100 Marks)</strong>

                                        <p style="font-size: 89%;"><strong>i.</strong>The student who desires to keep additional subject, must select (✓) any one subject from following subject group which he/she has not selected under compulsory subject group-1 or compulsory subject group-2 or elective subject group.</p>
                                        <p style="font-size: 89%;">
                                            <strong>ii.</strong> The student who does not want to keep additional subject, need not to select any subject under this subject group.
                    The student who does not want to keep additional subject, need not to select any subject under this subject group.
                                        </p>

                                    </div>
                                    <table>
                                        <asp:Repeater ID="rptAdditionalSubjects" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <%-- Column 1 --%>
                                                    <td><%# Eval("Name1") %></td>
                                                    <td><%# Eval("Checkbox1Html") %></td>
                                                    <%-- Column 2 --%>
                                                    <td><%# Eval("Name2") %></td>
                                                    <td><%# Eval("Checkbox2Html") %></td>
                                                    <%-- Column 3 --%>
                                                    <td><%# Eval("Name3") %></td>
                                                    <td><%# Eval("Checkbox3Html") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <div class="note boxinside">
                                        <b>Note:</b> Computer Science, Yoga & Phy. Edu. and Multimedia & Web.Tech. cannot be interchanged/swapped with any other subject.

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="page" id="page4">
                            <asp:Panel ID="pnlVocational" runat="server" Visible="false">
                                <div class="q33-box">

                                    <h2>Vocational Trade Group (100 Marks)</h2>
                                    <h3>(For students of Science, Commerce & Arts faculties)</h3>

                                    <div class="instructions">
                                        (i) विज्ञान, वाणिज्य एवं कला संकायों के विद्यार्थियों के लिए सत्र 2022-24 से राज्य के सभी जिलों के कुछ चिन्हित +2 विद्यालय/विद्यालयों में विभिन्न व्यावसायिक ट्रेंड के पठन-पाठन की शुरुआत की गई है, जिनकी सूची समिति के पोर्टल/वेबसाइट पर प्रदर्शित की गई है। व्यावसायिक ट्रेंड का पठन-पाठन अनिवार्य (Compulsory), ऐच्छिक (Elective) एवं अतिरिक्त (Additional) विषय संरचना के अलावा होगा।<br>
                                        (ii) उपरोक्त +2 विद्यालयों, जिनकी सूची समिति के पोर्टल/वेबसाइट पर प्रदर्शित की गई है, के वैसे विद्यार्थी, जिन्हें व्यावसायिक ट्रेंड आवंटित किया गया है, के द्वारा इस ट्रेंड समूह में से किसी एक ट्रेंड का चयन किया जाता एवं उसकी परीक्षा (सैद्धान्तिक एवं प्रायोगिक) में सम्मिलित होना अनिवार्य है। यद्यपि कि, इसके प्राप्तांक की गणना वार्षिक निर्णय हेतु नहीं की जाएगी तथा इसे किसी अन्य विषय के प्राप्तांक से परिवर्तित (Interchange/Swap) नहीं किया जाएगा।<br>
                                        (iii) राज्य/जिला के अन्य +2 विद्यालय, जो व्यावसायिक ट्रेंड का पठन-पाठन हेतु चिन्हित नहीं हैं, के विद्यार्थियों द्वारा इस ट्रेंड समूह में से किसी ट्रेंड का चयन नहीं किया जाएगा।

                                    </div>

                                    <table class="voc-table">
                                        <asp:Repeater ID="rptVocationalAdditionalSubjects" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="subject-label"><%# Eval("SubjectPaperName") %></td>
                                                    <td class="code">(<%# Eval("SubjectPaperCode") %>)</td>
                                                    <td class="checkbox-cell">
                                                        <%# Eval("CheckboxHtml") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </table>

                                </div>
                            </asp:Panel>
                            <div class="note-box">
                                प्रमाणित किया जाता है कि इस आवेदन पत्र में दी गई सूचना पूरी तरह से सही एवं शुद्ध हैं और इसमें कहीं पर भी किसी प्रकार के संशोधन की आवश्यकता नहीं है। जो भी सुधार एवं संशोधन थे, सब करा लिए गए हैं।

                   

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
                            </div>
                            <div class="note-box">
                                प्रमाणित किया जाता है कि ऊपर दिए गए सभी विवरण का मिलान महाविद्यालय/+2 विद्यालय के सभी अभिलेखों से पूर्णरूपेण कर लिया गया है। तत्संबंध में उक्त विद्यार्थी का सूचीकरण/अनुमति आवेदन पत्र स्वीकार किया जाए।

                   

                                <div class="principal-section">
                                    <div>
                                        <div class="principal-box"></div>
                                        <div class="signature-label">Signature &amp; seal of Principal</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <%--<script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js" ></script>--%>

        <script type="text/javascript" src="assets/js/customforexamform.js"></script>
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script type="text/javascript">


            //function updateAndPrint() {
            //    const urlParams = new URLSearchParams(window.location.search);
            //    const studentData = urlParams.get("studentData");

            //    if (studentData) {
            //        $.ajax({
            //            type: "POST",
            //            url: "ExaminationForm.aspx/UpdateDownloaded",
            //            data: JSON.stringify({ studentData: studentData }),
            //            contentType: "application/json; charset=utf-8",
            //            dataType: "json",
            //            success: function (response) {
            //                if (response.d && response.d.startsWith("error:")) {
            //                    alert("Update failed: " + response.d);  // 🔔 Show server error
            //                } else {
            //                    console.log("Update result:", response.d);
            //                }
            //                window.print();
            //            },
            //            error: function (xhr, status, error) {
            //                alert("AJAX error: " + error); // 🔔 Show client error
            //                window.print();
            //            }
            //        });
            //    } else {
            //        window.print();
            //    }
            //}
</script>
    </form>



</body>
</html>
