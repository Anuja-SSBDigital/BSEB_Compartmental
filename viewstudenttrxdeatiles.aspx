<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewstudenttrxdeatiles.aspx.cs" Inherits="viewstudenttrxdeatiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-12" id="divpayment" runat="server">
        <div class="card">
            <div class="card-header">
                 <h4>Registration Payment</h4></div>
            <div class="card-body">
                <div class="section-title">Transaction Details</div>
                <!-- Table/data goes here -->
                <div class="table-responsive">
                    <table class="table table-bordered table-striped table-sm" id="table-2">
                        <!-- Fixed Header Section -->
                        
                        <tr>
                            <th>Sr No</th>
                            <th>Student Name</th>

                            <th>Location</th>
                            <th>Faculty</th>
                            <th>Board Name</th>
                            <th>DOB</th>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>1</td>

                            <td>1</td>
                            <td>1</td>
                            <td>1</td>
                            <td>1</td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

