<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="payregstudentfee.aspx.cs" Inherits="payregstudentfee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">Student Registration Payment</div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="class">Class</label>
                            <select id="class" class="form-control">
                                <option selected>Intermediate</option>
                            </select>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="location">Location</label>
                            <asp:TextBox runat="server" ID="txt_collegename" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="faculty">Faculty</label>
                            <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="category">Student Category</label>
                            <asp:DropDownList runat="server" ID="ddl_category" CssClass="form-control">
                                <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                <asp:ListItem Value="Private">Private</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group col-md-6">
                            <label>Payment Status Condition</label>
                            <div class="form-check">
                                <asp:RadioButton runat="server" ID="rdo_makepayment"  Text="Make Payment" GroupName="rdo" />
                                <%--  <label class="form-check-label" for="makePayment">Make Payment</label>--%>
                            </div>
                            <div class="form-check">
                                <asp:RadioButton runat="server" ID="rdo_payemntstatus" Text="Payment Status" GroupName="rdo" />
                                <%-- <input class="form-check-input" type="radio" name="paystatus" id="paymentDone">--%>
                                <%--<label class="form-check-label" for="paymentDone">Payment Status</label>--%>
                            </div>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="paymode">Pay Mode</label>
                            <asp:DropDownList runat="server" ID="ddl_paymode" CssClass="form-control">
                                <asp:ListItem Value="Indian Bank">Indian Bank</asp:ListItem>
                                <asp:ListItem Value="HDFC Bank">HDFC Bank</asp:ListItem>
                            </asp:DropDownList>
                            <%-- <select id="paymode" runat="server" class="form-control">
                                <option selected>Indian Bank</option>
                                <option>HDFC Bank</option>
                            </select>--%>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Available Seat in Regular</label>
                            <input type="text" class="form-control" value="437" readonly>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Available Seat in Private</label>
                            <input type="text" class="form-control" value="256" readonly>
                        </div>
                    </div>
                    <!-- Buttons -->
                    <div class="form-group mt-3">
                        <asp:Button runat="server" ID="btn_getdetails" CssClass="btn btn-primary mr-2" Text="GET DETAILS" OnClick="btn_getdetails_Click" />
                        <%-- <button type="submit" class="btn btn-primary mr-2">GET DETAILS</button>--%>
                        <button type="reset" class="btn btn-secondary mr-2">RESET</button>
                        <button type="button" class="btn btn-primary">UPDATE ALL TRANSACTION STATUS</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-12" id="divstudentdetails" runat="server">
            <div class="card">
                <div class="card-header">Student List</div>
                <div class="card-body">
                    <div class="section-title">Student Details</div>
                    <div class="text-danger mt-3">
                        <strong>नोट:</strong> Student Registration में उन्ही छात्रों का नाम दिखेगा जिनका शुल्क Success होगा।
                    </div>
                    <div class="mt-3">
                        <strong>Total Selected Fee Amount: ₹ <span id="totalAmount">0</span></strong>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-sm" id="table-1">
                            <thead>
                                <tr>
                                    <th class="repeater-checkbox">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" ClientIDMode="Static" onclick="selectAll(this)" />

                                    </th>
                                    <th>S. No.</th>
                                    <th>OFSS No.</th>
                                    <th>Name</th>
                                    <th>Father Name</th>
                                    <th>Mother Name</th>
                                    <th>DOB</th>
                                    <th>Board Name</th>
                                    <th>Category</th>
                                    <th>Form Submitted Date</th>
                                    <th>Fee Amount</th>
                                    <th>Delete</th>
                                </tr>
                            </thead>
                            <!-- Same head and form as before -->
                            <!-- Only the <tbody> of table is shown below with 10 students -->
                            <tbody>
                                <asp:Repeater runat="server" ID="rptStudents">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="repeater-checkbox">
                                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="itemCheckbox" onclick="checkIndividual()" />


                                                <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />

                                            </td>
                                            <td><%#Eval("OfssReferenceNo") %></td>
                                            <td><%#Eval("StudentName") %></td>
                                            <td><%# Eval("FatherName") %></td>
                                            <td><%# Eval("MotherName") %></td>
                                            <td><%# Eval("Dob", "{0:yyyy-MM-dd}") %></td>
                                            <td><%# Eval("BoardName") %></td>
                                            <td><%# Eval("CategoryName") %></td>
                                            <td></td>
                                            <td class="fee-amount" data-amount='<%# Eval("FeeAmount") %>'>
                                                <asp:Label ID="lblFee" runat="server" Text='<%# Eval("FeeAmount") %>' CssClass="d-none" /></td>

                                            <%-- <td>
                                                <button class="btn btn-danger btn-sm">Delete</button></td>--%>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                        <div class="text-center">
                            <asp:Button runat="server" ID="btn_paynow" Text="Paynow" CssClass="btn btn-warning" OnClick="btn_paynow_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-12" id="divpayment" runat="server">
            <div class="card">
                <div class="card-header">Student Registration Payment</div>
                <div class="card-body">
                    <div class="section-title">Transaction Details</div>
                    <!-- Table/data goes here -->
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-sm" id="table-2">
                            <thead>
                                <tr>

                                    <th>S. No.</th>
                                    <th>CollegeCode</th>
                                    <th>Transaction ID</th>
                                    <th>Student No</th>
                                    <th>Paid Amount</th>
                                    <th>Status</th>
                                    <th>View Payment Status</th>
                                    <th>View Details</th>
                                    <th>Delete</th>
                                </tr>
                            </thead>
                            <!-- Same head and form as before -->
                            <!-- Only the <tbody> of table is shown below with 10 students -->
                            <tbody>
                                <asp:Repeater runat="server" ID="rpt_getpayemnt">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                            </td>
                                            <td><%#Eval("CollegeCode") %></td>
                                            <td><%#Eval("ClientTxnId") %></td>
                                            <td><%# Eval("StudentsPerTransaction") %></td>
                                            <td><%# Eval("AmountPaid") %></td>
                                            <td><%# Eval("PaymentStatus") %></td>
                                             <td></td>
                                            <td>
                                                <i class="fa-1x fa-eye fas text-linkedin" onclick="paymentdetalis();"
                                                    style="cursor: pointer;"
                                                    data-bs-toggle="modal"
                                                    data-bs-target='<%# "#studentModal" + Container.ItemIndex %>'></i>

                                                <div class="modal fade"
                                                    id='<%# "studentModal" + Container.ItemIndex %>'
                                                    tabindex="-1"
                                                    aria-labelledby='<%# "studentModalLabel" + Container.ItemIndex %>'
                                                    aria-hidden="true">
                                                    <div class="modal-dialog modal-md">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <h5 class="modal-title" id='<%# "studentModalLabel" + Container.ItemIndex %>'>Student Details</h5>
                                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </div>
                                                            <div class="modal-body">
                                                                <p><strong>Txn ID:</strong> <%# Eval("ClientTxnId") %></p>
                                                                <p><strong>Student Name:</strong> <%# Eval("StudentFullName") %></p>
                                                                <p><strong>Category:</strong> <%# Eval("CategoryName") %></p>
                                                                <p><strong>Faculty:</strong> <%# Eval("Fk_FacultyId") %></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>

                                           

                                            <td>
                                                <button class="btn btn-danger btn-sm">Delete</button></td>
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
    <script>
        function selectAll(source) {
            debugger;
            var checkboxes = document.querySelectorAll("input[id*='chkSelect']");
            console.log("Checkboxes found:", checkboxes.length);

            checkboxes.forEach(function (cb) {
                cb.checked = source.checked;
            });

            calculateTotal();
        }

        function checkIndividual() {
            debugger;
            var checkboxes = document.querySelectorAll("input[id*='chkSelect']");
            var selectAllBox = document.querySelector("input[id*='chkSelectAll']");

            var allChecked = Array.from(checkboxes).every(cb => cb.checked);
            if (selectAllBox) {
                selectAllBox.checked = allChecked;
            }

            calculateTotal();
        }

        function calculateTotal() {
            debugger;
            var checkboxes = document.querySelectorAll("input[id*='chkSelect']");
            var total = 0;

            checkboxes.forEach(function (cb) {
                if (cb.checked) {
                    var row = cb.closest('tr');
                    if (row) {
                        var feeCell = row.querySelector('.fee-amount');
                        if (feeCell) {
                            var amount = parseFloat(feeCell.getAttribute('data-amount')) || parseFloat(feeCell.textContent) || 0;
                            total += amount;
                        }
                    }
                }
            });

            var totalAmountSpan = document.getElementById('totalAmount');
            if (totalAmountSpan) {
                totalAmountSpan.textContent = total.toFixed(2);
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            calculateTotal();
        });


        function  paymentdetalis(){
            $("#myModal").show();
        }
    </script>

</asp:Content>
