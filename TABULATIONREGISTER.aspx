<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TABULATIONREGISTER.aspx.cs" Inherits="TABULATIONREGISTER" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tabulation Register</title>
    <style>
        /* Basic styling for the report */
        body {
            font-family: 'Times New Roman', serif;
            font-size: 13px;
        }

        /* Essential border for tables and cells */
        .border-black {
            border: 1px solid black !important;
        }

        /* Styling for standard bordered tables */
        .table-bordered td, .table-bordered th {
            border: 1px solid black !important;
            padding: 2px !important;
            text-align: center;
            vertical-align: middle;
        }

        /* Header styling for the board name */
        .bseb-header {
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            line-height: 1.3;
        }

        /* Sub-header styling */
        .sub-header {
            font-size: 16px;
            text-align: center;
        }

        /* Smaller text for specific elements */
        .small-text {
            font-size: 12px;
        }

        /* Bold font weight */
        .fw-bold {
            font-weight: bold;
        }

        /* Text rotation (if needed, not used in current layout but kept from original) */
        .rotate-text {
            writing-mode: vertical-rl;
            transform: rotate(180deg);
        }

        /* Logo sizing */
        .logo {
            width: 80px;
            height: 80px;
        }

        /* Custom table padding */
        .custom-table th, .custom-table td {
            padding: 2px 4px !important;
        }

        /* Table with no borders (used for header layout) */
        .noborder-table, .noborder-table th, .noborder-table td {
            border: none !important;
            text-align: center;
        }

        /* Specific styling for the main data table (tbl2) */
        .tbl2 th, .tbl2 td {
            border: 1px solid black;
            border-collapse: collapse; /* Ensures single borders */
            font-family: Arial, sans-serif;
            font-size: 12px;
            padding: 4px 8px;
            text-align: center;
        }

        /* Background for table headers */
        th {
            background: #f0f0f0;
        }

        /* Styles for college sections to enable page breaks for printing */
        .college-section {
            page-break-after: always; /* Forces a page break after each college section */
            margin-bottom: 20px; /* Adds some space between sections */
        }

    </style>
</head>
<body class="p-2">
    <form id="form1" runat="server">
        <asp:DropDownList runat="server" ID="ddl_faculty">
            <asp:ListItem Value="ARTS">ARTS</asp:ListItem>
            <asp:ListItem Value="SCIENCE">SCIENCE</asp:ListItem>
            <asp:ListItem Value="COMMERCE">COMMERCE</asp:ListItem>
            <asp:ListItem Value="VOCATIONAL">VOCATIONAL</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" ID="btn_submit" Text="Submit" OnClick="btn_submit_Click"/>
         <asp:Button ID="btnPrint" runat="server" Text="PDF" OnClientClick="window.print(); return false;" />
        <%-- Outer Repeater: Groups data by College --%>
        <asp:Repeater ID="rpt_CollegeGrouped" runat="server" OnItemDataBound="rpt_CollegeGrouped_ItemDataBound">
            <ItemTemplate>
                <%-- This div represents a single college's section --%>
                <div class="college-section">
                    <%-- College Header Section --%>
                    <div class="colleagename">
                        <table class="noborder-table" style="width: 100%; border-collapse: collapse; font-family: 'Mangal', 'Nirmala UI', Arial, sans-serif;">
                            <tr>
                                <td>
                                    <img src="img/logo.png" alt="BSEB Logo" style="height: 70px;">
                                </td>
                                <td style="text-align: center; font-size: 24px; font-weight: bold;">
                                    बिहार विद्यालय परीक्षा समिति, पटना<br>
                                    <span style="font-size: 20px;">BIHAR SCHOOL EXAMINATION BOARD, PATNA</span>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><span style="font-size: 14px; font-weight: bold;">TABULATION REGISTER</span></td>
                                <td style="text-align: center; font-size: 16px;">
                                    <span>इंटरमीडिएट वार्षिक परीक्षा ,2025</span>
                                    <span style="padding: 0 20px;">INTERMEDIATE ANNUAL EXAMINATION, 2025</span>
                                </td>
                                <td style="text-align: left; font-size: 14px; font-weight: bold;">
                                    FACULTY: <asp:Label runat="server" ID="lbl_faculty"></asp:Label>
                                </td>
                                <td style="text-align: right; font-size: 14px; font-weight: bold;">PAGE NO.: <asp:Label runat="server" ID="lbl_count"></asp:Label></td>
                            </tr>
                        </table>

                        <%-- Main Data Table for Students and Subjects --%>
                        <table class="tbl2" border="1" style="width: 100%; border-collapse: collapse; font-family: 'Courier New', Courier, monospace; font-size: 14px; margin-top: 10px;">
                            <thead>
                                <tr>
                                    <th colspan="1">रोल कोड<br>ROLL CODE</th>
                                    <th colspan="7">
                                        विद्यालय/महाविद्यालय का नाम<br>NAME OF THE +2 SCHOOL /COLLEGE
                                        <br />
                                        <%-- College Name Label (bound to the outer repeater's data item) --%>
                                        <asp:Label runat="server" ID="lbl_collegename_rpt" Text='<%# Eval("CollegeName") %>'></asp:Label>
                                    </th>
                                    <th colspan="12">परीक्षा केन्द्र<br>EXAMINATION CENTRE</th>
                                </tr>
                                <tr>
                                    <th rowspan="4">क्रमांक<br>ROLL NO.</th>
                                    <th rowspan="4">
                                        BSEB Unique Id.<br />NAME OF CANDIDATE
                                        <br>NAME OF MOTHER
                                        <br>NAME OF FATHER
                                    </th>
                                    <th rowspan="2" colspan="2">ENLISTMENT NO.</th>
                                    <%-- ADDED COMPULSORY SUBJECT HEADER --%>
                                    <th colspan="4">COMPULSORY SUBJECT<br>Marks</th>
                                    <th colspan="4">ELECTIVE SUBJECT<br>Marks</th>
                                    <th colspan="5">ADDITIONAL &amp; VOCATIONAL<br>TRADE SUBJECT<br>Marks</th>
                                    <th>Marks Sheet<br>Number.</th>
                                    <th>कुल योग<br><strong>AGGREGATE</strong></th>
                                </tr>
                                <tr>
                                    <%-- COMPULSORY SUBJECT sub-headings --%>
                                    <th>Subject</th>
                                    <th>Theory</th>
                                    <th>Prac.Reg</th>
                                    <th>Total<br>Marks</th>
                                    <%-- ELECTIVE SUBJECT sub-headings --%>
                                    <th>Subject</th>
                                    <th>Theory</th>
                                    <th>Prac.</th>
                                    <th>Total<br>Marks</th>
                                    <%-- ADDITIONAL & VOCATIONAL sub-headings --%>
                                    <th>Subject</th>
                                    <th>Theory</th>
                                    <th>Prac.</th>
                                    <th>CCE</th>
                                    <th>Total<br>Marks</th>
                                    <th>Certificate<br>Number.</th>
                                    <th>परीक्षाफल<br><strong>RESULT</strong></th>
                                </tr>
                                <tr>
                                    <th colspan="2">SEX</th>
                                    <%-- COMPULSORY, ELECTIVE, ADDITIONAL placeholders --%>
                                    <th colspan="4"></th>
                                    <th colspan="4"></th>
                                    <th colspan="5"></th>
                                    <th colspan="2"></th>
                                </tr>
                                <tr>
                                    <th>CASTE</th>
                                    <th>CATEGORY</th>
                                    <%-- empty for COMPULSORY, ELECTIVE, ADDITIONAL --%>
                                    <th colspan="4"></th>
                                    <th colspan="4"></th>
                                    <th colspan="5"></th>
                                    <th colspan="2"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <%-- Inner Repeater: Iterates through students within the current college --%>
                                <asp:Repeater ID="rpt_grouped" runat="server" OnItemDataBound="rpt_grouped_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <%-- Student Basic Info (Roll Code, Roll No) --%>
                                            <td rowspan="3">
                                                <%# Eval("RollCode") %><br />
                                                <%# Eval("RollNumber") %>
                                            </td>
                                            <%-- Student Name, Mother, Father, BSEB Unique ID --%>
                                            <td rowspan="3">
                                                <%# Eval("BSEB_Unique_Id") %><br />
                                                <b><%# Eval("StudentFullName") %></b><br />
                                                <%# Eval("MotherName") %><br />
                                                <%# Eval("FatherName") %>
                                            </td>
                                            <%-- Registration No, Gender, Caste, Category --%>
                                            <td rowspan="3" colspan="2">
                                                <%# Eval("RegistrationNo") %><br />
                                                <%# Eval("Gender") %><br />
                                                <%# Eval("CastCategoryName") %><br />
                                                <%# Eval("CategoryName") %>
                                            </td>
                                            <%-- COMPULSORY SUBJECT in nested table --%>
                                            <td colspan="4">
                                                <table class="noborder-table" style="width: 100%; border-collapse: collapse; ">
                                                    <asp:Repeater ID="rpt_compulsory" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Theory") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("PracticalMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject_Total") %></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </td>
                                            <%-- Elective Subjects Section (nested table with its own repeater) --%>
                                            <td colspan="4">
                                                <table style="width: 100%; border-collapse: collapse;" class="noborder-table">
                                                    <asp:Repeater ID="rpt_elective" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Theory") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("PracticalMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject_Total") %></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </td>
                                            <%-- Additional & Vocational Subjects Section (nested table with its own repeaters) --%>
                                            <td colspan="5">
                                                <table style="width: 100%; border-collapse: collapse;" class="noborder-table">
                                                    <asp:Repeater ID="rpt_additional" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Theory") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("PracticalMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("CCEMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject_Total") %></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <asp:Repeater ID="rpt_vocational" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Theory") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("PracticalMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("CCEMarks") %></td>
                                                                <td style="border: 1px solid #000; padding: 2px; text-align: center;"><%# Eval("Subject_Total") %></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </td>
                                            <%-- Marks Sheet Number (empty in your provided HTML, but column exists) --%>
                                            <td></td>
                                            <%-- Aggregate Marks and Result Division --%>
                                            <td rowspan="3">
                                                <%# Eval("AggregateMarks") %>
                                                <br />
                                                <%# Eval("Result_Division") %>
                                            </td>
                                        </tr>
                                        <%-- Empty rows to correctly handle rowspan=3 for student info --%>
                                        <tr></tr>
                                        <tr></tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <style type="text/css" media="print">
    #btnPrint {
        display: none !important;
    }
    #btn_submit{
        display: none !important;
    }
    #ddl_faculty{
        display: none !important;
    }
</style>
    </form>
</body>
</html>