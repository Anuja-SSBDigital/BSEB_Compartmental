<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SeatMatrix.aspx.cs" Inherits="SeatMatrix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%-- <style>
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

    </style>--%>
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

     .repeater-checkbox {
         text-align: center;
         width: 40px;
     }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                     <h4>Seat Matrix</h4>
                    </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-6" runat="server" id="ddldiv_location">
                            <label for="location">+2 School/College Code & Name</label>
                            <asp:TextBox runat="server" ID="txt_collegecode" CssClass="form-control"></asp:TextBox>
                        </div>
                       <%-- <div class="form-group col-md-6">
                            <label for="faculty">Faculty</label>
                            <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                            </asp:DropDownList>
                        </div>--%>
                    </div>
                    <!-- Buttons -->
                    <div class="form-group mt-3">
                        <asp:Button runat="server" ID="btn_submit" CssClass="btn btn-primary mr-2" Text="VIEW RECORD" OnClick="btn_submit_Click" />
                        <%-- <button type="submit" class="btn btn-primary mr-2">GET DETAILS</button>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-12" id="divstudentdetails" runat="server">
            <div class="card">
             <%--   <div class="card-header"></div>--%>
                <div class="card-body">
                    <div class="section-title"><h6>Seat Matrix List</h6></div>

                    <div class="table-responsive">
                        <%--<table class="table table-bordered table-hover table-sm" id="table-1">--%>
                        <table class="table table-hover table-bordered" id="table-1">
                            <thead>
                                <tr>

                                    <th>S. No.</th>
                                    <th>+2 School/College Code & Name</th>
                                    <th>Faculty</th>
                                    <th>Regular Seats</th>
                                    <th>Private Seats</th>
                                    <th>Action</th>

                                </tr>
                            </thead>
                            <!-- Same head and form as before -->
                            <!-- Only the <tbody> of table is shown below with 10 students -->
                            <tbody>
                                <asp:Repeater runat="server" ID="rpt_seatmatrix" OnItemCommand="rpt_seatmatrix_ItemCommand">
                                    <ItemTemplate>
                                        <tr>

                                            <td>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />

                                            </td>
                                            <td><%# Eval("CollegeName") + " (" + Eval("CollegeCode") + ")" %></td>
                                            <td><%#Eval("FacultyName") %></td>
                                            <td>
                                                <asp:HiddenField ID="hf_SeatMatrixId" runat="server" Value='<%# Eval("Pk_SeatMatrixId") %>' />
                                                <asp:Label ID="lblRegularSeats" runat="server" Text='<%# Eval("RegularSeats") %>' Visible='<%# !(bool)Eval("IsEdit") %>' />
                                                <asp:TextBox ID="txtRegularSeats" runat="server" CssClass="form-control" Text='<%# Eval("RegularSeats") %>' Visible='<%# (bool)Eval("IsEdit") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPrivateSeats" runat="server" Text='<%# Eval("PrivateSeats") %>' Visible='<%# !(bool)Eval("IsEdit") %>' />
                                                <asp:TextBox ID="txtPrivateSeats" runat="server" CssClass="form-control" Text='<%# Eval("PrivateSeats") %>' Visible='<%# (bool)Eval("IsEdit") %>' />
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lnk_edit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("Pk_SeatMatrixId") %>'
                                                    CssClass="btn btn-sm btn-primary" Visible='<%# !(bool)Eval("IsEdit") %>' Text="Edit" />
                                                <asp:LinkButton ID="lnk_update" runat="server" CommandName="Update" CommandArgument='<%# Eval("Pk_SeatMatrixId") %>'
                                                    CssClass="btn btn-sm btn-success" Visible='<%# (bool)Eval("IsEdit") %>' Text="Update" />
                                                <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="Cancel" CommandArgument='<%# Eval("Pk_SeatMatrixId") %>'
                                                    CssClass="btn btn-sm btn-secondary" Visible='<%# (bool)Eval("IsEdit") %>' Text="Cancel" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
