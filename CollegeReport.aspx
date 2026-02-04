<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollegeReport.aspx.cs" Inherits="CollegeReport"  MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Page background */
        body {
            background-color: #f2f4f7;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
        }

        /* Card wrapper */
        .report-card {
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 6px 16px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            margin-bottom: 30px;
        }

        /* Card header */
        .report-header {
            /*background: #3059c4 !important;*/ /* Corporate navy blue */
            color: #ffffff;
            padding: 18px 24px;
            font-size: 1.3rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: .5px;
        }

        /* Table */
        .summary-table {
            width: 100%;
            border-collapse: collapse;
        }

        .summary-table thead th {
            background: #f8f9fb;
            color: #495057;
            text-align: center;
            padding: 12px;
            font-weight: 600;
            font-size: 0.95rem;
            border-bottom: 2px solid #dee2e6;
        }

        /* Zebra rows */
        .summary-table tbody tr:nth-child(even) {
            background-color: #f8f9fb;
        }

        .summary-table tbody tr:hover {
            background-color: #e9ecef;
            transition: background-color 0.3s ease;
        }

        .summary-table tbody td {
            text-align: center;
            padding: 12px;
            font-size: 0.95rem;
            border-bottom: 1px solid #dee2e6;
        }

        /* Column specific styles */
        .summary-table tbody td:first-child {
            font-weight: 600;
            color: #002b5c;
        }

        .summary-table tbody td:nth-child(2) {
            text-align: left;
            padding-left: 20px;
        }

        .summary-table tbody td:last-child {
            font-weight: 600;
            color: #28a745;
        }

        /* Responsive design for small screens */
        @media (max-width: 768px) {
            .summary-table thead {
                display: none;
            }

            .summary-table tbody td {
                display: block;
                text-align: right;
                padding-left: 50%;
                position: relative;
            }

            .summary-table tbody td::before {
                content: attr(data-label);
                position: absolute;
                left: 15px;
                width: 50%;
                text-align: left;
                font-weight: bold;
                color: #495057;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  <div class="row mt-4">
    <div class="col-12">
        <div class="report-card">
            <div class="report-header text-center bg-main">
                Overall Report
            </div>

            <div class="p-4">

                <!-- Summary Labels -->
                <div class="row mb-4 text-center">
                    <div class="col-md-4">
                        <div class="summary-card p-3 border rounded">
                            <h6 class="text-muted">Exam Forms Downloaded</h6>
                            <asp:Label ID="lblExamFormDownloaded" runat="server" CssClass="h5 d-block mt-2"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="summary-card p-3 border rounded">
                            <h6 class="text-muted">Exam Forms Submitted</h6>
                            <asp:Label ID="lblExamFormSubmitted" runat="server" CssClass="h5 d-block mt-2"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="summary-card p-3 border rounded">
                            <h6 class="text-muted">Fee Submitted</h6>
                            <asp:Label ID="lblFeeSubmitted" runat="server" CssClass="h5 d-block mt-2"></asp:Label>
                        </div>
                    </div>
                </div>

                <!-- Fee Per Exam Grid -->
                <div class="table-responsive mb-3">
                    <asp:GridView ID="gvFeePerExam" runat="server" CssClass="table summary-table" AutoGenerateColumns="true"></asp:GridView>
                </div>

                <!-- Form Per Exam Grid -->
                <div class="table-responsive mb-3">
                    <asp:GridView ID="gvFormPerExam" runat="server" CssClass="table summary-table" AutoGenerateColumns="true"></asp:GridView>
                </div>

                <!-- Update Button -->
                <div class="text-center mt-3">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary px-4" OnClick="btnUpdate_Click"  />
                </div>

            </div>
        </div>
    </div>
</div>


</asp:Content>
