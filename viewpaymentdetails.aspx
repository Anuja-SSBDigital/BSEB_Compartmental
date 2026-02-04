<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewpaymentdetails.aspx.cs" Inherits="viewpaymentdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-collapse: collapse !important; /* Ensures no double borders */
            width: 100%;
        }

            table th,
            table td {
                border: 1px solid #333 !important; /* Darker and consistent grid border */
                padding: 10px;
                font-size: 14px;
                vertical-align: middle;
            }

            table tr:nth-child(even) {
                background-color: #f9f9f9;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-12" id="divpayment" runat="server">
        <div class="card">
            <%-- <div class="card-header">
                 <h4>Registration Payment</h4></div>--%>
            <div class="card-body">
                <div class="section-title">Transaction Details</div>

                <!-- Table/data goes here -->
                <div class="row">
                    <div class="col-md-6">
                        <a href="PayExamFormFee.aspx" Class="btn btn-primary mb-2" > Back</a>

                    </div>
                    <div class="col-md-6 text-right">
                        <asp:Button ID="DwnTransaction_PDF" runat="server" Text="Download PDF" CssClass="btn btn-primary mb-2" OnClick="DwnTransaction_PDF_Click" />


                    </div>
                </div>
                <div class="table-responsive">

                    <table class="table table-striped table-bordered" id="table-1">
                        <thead>
                            <tr>
                                <th>Sr No</th>
                                <th>Transaction Id</th>
                                <th>Student Name</th>
                                <th>Father Name</th>
                                <%--<th>Mother Name</th>--%>
                                <%--<th>Location</th>--%>
                                <th>Faculty</th>
                                <th>Board Name</th>
                                <th>DOB</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpt_getpayemnt" OnItemDataBound="rpt_getpayemnt_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" /></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblTxn"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="StudentFullName" Text='<%# Eval("StudentFullName") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="FatherName" Text='<%# Eval("FatherName") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="FacultyName" Text='<%# Eval("FacultyName") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="BoardName" Text='<%# Eval("BoardName") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="dob" Text='<%# Eval("DOB", "{0:dd-MM-yyyy}") %>'></asp:Label></td>

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

