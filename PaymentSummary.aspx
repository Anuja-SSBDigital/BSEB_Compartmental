<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentSummary.aspx.cs" Inherits="PaymentSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .summary-table {
            table-layout: fixed;
            width: 100%;
            border-collapse: collapse;
        }

            .summary-table th,
            .summary-table td {
                text-align: center;
                border: 1px solid #000 !important;
                vertical-align: middle;
                white-space: nowrap;
                padding: 8px;
            }

        .faculty-header {
            position: sticky;
            top: 0;
            z-index: 10;
            background: #f8f9fa;
            font-weight: bold;
        }

        .table-secondary.fw-bold td {
            background: #f1f3f5 !important;
            font-size: 1.1rem;
            font-weight: bold;
            text-align: center;
            padding-left: 12px;
            color: #333;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Exam Form Submission Summary</h4>
                </div>



                <div class="card-body">
                    <div class="form-group">
                        <label runat="server" id="lblcollege" visible="false">+2 School/College Code<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control"
                            placeholder="Enter CollegeCode" Visible="false">
                        </asp:TextBox>
                        <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                    </div>

                    <div class="form-group mt-4 text-center">
                        <asp:Button ID="btngetsummary" runat="server" Text="Get Summary" CssClass="btn btn-primary"
                            OnClick="btngetsummary_Click" Visible="false" />
                    </div>
                    <div class="table-responsive">

                        <!-- ✅ Common Header Table -->
                        <table class="table table-bordered summary-table mb-0">
                            <thead class="faculty-header sticky-top bg-light">
                                <tr>
                                    <th>Faculty / Exam Type</th>
                                    <th>Fee Submitted</th>
                                    <th>Form Submitted</th>
                                    <th>Form Not Submitted</th>
                                </tr>
                            </thead>
                        </table>

                        <!-- ✅ Science Table -->
                        <table class="table table-bordered summary-table mt-2">
                            <tbody>
                                <tr class="table-secondary fw-bold">
                                    <td colspan="4">Science</td>
                                </tr>
                                <tr>
                                    <td>Regular</td>
                                    <td id="tdSciRegFee" runat="server">--</td>
                                    <td id="tdSciRegForm" runat="server">--</td>
                                    <td id="tdSciRegFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Ex Regular</td>
                                    <td id="tdSciExFee" runat="server">--</td>
                                    <td id="tdSciExForm" runat="server">--</td>
                                    <td id="tdSciExFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Compartment</td>
                                    <td id="tdSciCompFee" runat="server">--</td>
                                    <td id="tdSciCompForm" runat="server">--</td>
                                    <td id="tdSciCompFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Improvement</td>
                                    <td id="tdSciImpFee" runat="server">--</td>
                                    <td id="tdSciImpForm" runat="server">--</td>
                                    <td id="tdSciImpFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Qualifying</td>
                                    <td id="tdSciQualFee" runat="server">--</td>
                                    <td id="tdSciQualForm" runat="server">--</td>
                                    <td id="tdSciQualFormNot" runat="server">--</td>
                                </tr>
                            </tbody>
                        </table>

                        <!-- ✅ Arts Table -->
                        <table class="table table-bordered summary-table mb-4">
                            <tbody>
                                <tr class="table-secondary fw-bold">
                                    <td colspan="4">Arts</td>
                                </tr>
                                <tr>
                                    <td>Regular</td>
                                    <td id="tdArtsRegFee" runat="server">--</td>
                                    <td id="tdArtsRegForm" runat="server">--</td>
                                    <td id="tdArtsRegFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Ex Regular</td>
                                    <td id="tdArtsExFee" runat="server">--</td>
                                    <td id="tdArtsExForm" runat="server">--</td>
                                    <td id="tdArtsExFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Compartment</td>
                                    <td id="tdArtsCompFee" runat="server">--</td>
                                    <td id="tdArtsCompForm" runat="server">--</td>
                                    <td id="tdArtsCompFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Improvement</td>
                                    <td id="tdArtsImpFee" runat="server">--</td>
                                    <td id="tdArtsImpForm" runat="server">--</td>
                                    <td id="tdArtsImpFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Qualifying</td>
                                    <td id="tdArtsQualFee" runat="server">--</td>
                                    <td id="tdArtsQualForm" runat="server">--</td>
                                    <td id="tdArtsQualFormNot" runat="server">--</td>
                                </tr>
                            </tbody>
                        </table>

                        <!-- ✅ Commerce Table -->
                        <table class="table table-bordered summary-table">
                            <tbody>
                                <tr class="table-secondary fw-bold">
                                    <td colspan="4">Commerce</td>
                                </tr>
                                <tr>
                                    <td>Regular</td>
                                    <td id="tdComRegFee" runat="server">--</td>
                                    <td id="tdComRegForm" runat="server">--</td>
                                    <td id="tdComRegFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Ex Regular</td>
                                    <td id="tdComExFee" runat="server">--</td>
                                    <td id="tdComExForm" runat="server">--</td>
                                    <td id="tdComExFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Compartment</td>
                                    <td id="tdComCompFee" runat="server">--</td>
                                    <td id="tdComCompForm" runat="server">--</td>
                                    <td id="tdComCompFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Improvement</td>
                                    <td id="tdComImpFee" runat="server">--</td>
                                    <td id="tdComImpForm" runat="server">--</td>
                                    <td id="tdComImpFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Qualifying</td>
                                    <td id="tdComQualFee" runat="server">--</td>
                                    <td id="tdComQualForm" runat="server">--</td>
                                    <td id="tdComQualFormNot" runat="server">--</td>
                                </tr>
                            </tbody>
                        </table>

                        <!-- ✅ Vocational Table -->
                        <table class="table table-bordered summary-table">
                            <tbody>
                                <tr class="table-secondary fw-bold">
                                    <td colspan="4">Vocational</td>
                                </tr>
                                <tr>
                                    <td>Regular</td>
                                    <td id="tdVocRegFee" runat="server">--</td>
                                    <td id="tdVocRegForm" runat="server">--</td>
                                    <td id="tdVocRegFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Ex Regular</td>
                                    <td id="tdVocExFee" runat="server">--</td>
                                    <td id="tdVocExForm" runat="server">--</td>
                                    <td id="tdVocExFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Compartment</td>
                                    <td id="tdVocCompFee" runat="server">--</td>
                                    <td id="tdVocCompForm" runat="server">--</td>
                                    <td id="tdVocCompFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Improvement</td>
                                    <td id="tdVocImpFee" runat="server">--</td>
                                    <td id="tdVocImpForm" runat="server">--</td>
                                    <td id="tdVocImpFormNot" runat="server">--</td>
                                </tr>
                                <tr>
                                    <td>Qualifying</td>
                                    <td id="tdVocQualFee" runat="server">--</td>
                                    <td id="tdVocQualForm" runat="server">--</td>
                                    <td id="tdVocQualFormNot" runat="server">--</td>
                                </tr>
                            </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
