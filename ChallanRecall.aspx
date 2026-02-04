<%@ Page Title="BSEB - Admin Dashboard | Challan Recall " Language="C#" MasterPageFile="~/MasterPage.master" Async="true" AutoEventWireup="true" CodeFile="ChallanRecall.aspx.cs" Inherits="ChallanRecall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table-responsive {
            margin-top: 20px;
        }

        .repeater-checkbox {
            width: 30px;
            text-align: center;
        }

        .repeater-col {
            padding: 8px;
        }

        .required::after {
            content: " *";
            color: red;
        }

        .text-red {
            color: red;
            font-size: 0.9rem;
        }

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
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Challan Recall Data</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="collegeCode" class="form-label required">+2 School/College Code</label>
                                <asp:TextBox ID="txt_CollegeCode" runat="server" CssClass="form-control"></asp:TextBox>
                                <span id="txttransactionidval" style="display: none; color: red;">Please Enter CollegeCode</span>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4">
                        <asp:Button ID="btn_search" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClientClick="return validateForm();" OnClick="btn_search_Click" />
                    </div>
                    <hr />
                    <div class="table-responsive">
                        <asp:Panel ID="pnlcollegeCodeTable" runat="server" >
                            <table class="table table-hover table-bordered" id="table-1">
                                <thead>
                                    <tr>
                                        <th>S. No.</th>
                                        <th>+2 School/College</th>
                                        <th>Transaction ID</th>
                                        <th>Student No</th>
                                        <th>Paid Amount</th>
                                        <th>Payment Initiate Date</th>
                                        <th>Payment Updated Date</th>
                                        <th>Status</th>
                                      
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptransactiondetails" OnItemDataBound="rptransactiondetails_ItemDataBound" OnItemCommand="rptransactiondetails_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                </td>
                                                <td><%# Eval("CollegeCode") %></td>
                                                <td><%# Eval("ClientTxnId") %></td>
                                                <td><%# Eval("StudentsPerTransaction") %></td>
                                                <td><%# Eval("AmountPaid") %></td>
                                                <td><%# Eval("PaymentInitiateDate", "{0:dd-MM-yyyy}") %></td>
                                                <td><%# Eval("PaymentUpdatedDate", "{0:dd-MM-yyyy}") %></td>
                                                <td><%# Eval("PaymentStatus") %></td>
                                                <asp:HiddenField runat="server" ID="hf_status" Value='<%# Eval("PaymentStatus") %>' />
                                                <asp:HiddenField runat="server" ID="hf_bankgateway" Value='<%# Eval("BankGateway") %>' />
                                              <asp:HiddenField runat="server" ID="hf_GatewayTxnId" Value='<%# Eval("GatewayTxnId") %>'/>
                                                <td>
                                                   <asp:HiddenField runat="server" ID="hf_ClientTxnId" Value='<%# Eval("ClientTxnId") %>'/>
                                                   <asp:HiddenField runat="server" ID="hf_PaymentId" Value='<%# Eval("Pk_PaymentId") %>'/>
                                                    <asp:HiddenField runat="server" ID="hf_Isdeleted" Value='<%# Eval("IsDeleted") %>' />
                                                    <asp:LinkButton ID="lnkrestore" runat="server" CommandName="lnk_Restore" CommandArgument='<%# Eval("Pk_PaymentId") %>' CssClass="btn btn-danger btn-sm">Restore
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Visible="false" CssClass="alert alert-danger text-center mt-3">
                            No student records found matching your criteria.
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function validateForm() {
            var collegeCode = document.getElementById('<%= txt_CollegeCode.ClientID %>').value.trim();
            var errorLabel = document.getElementById('txttransactionidval');
            if (collegeCode === "") {
                errorLabel.style.display = 'inline';
                return false;
            } else {
                errorLabel.style.display = 'none';
                return true;
            }
        }
       
    </script>
</asp:Content>
