<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="payregstudentfee.aspx.cs" Inherits="payregstudentfee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- In <head> -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a076d05399.js" crossorigin="anonymous"></script>
    <!-- for eye icon -->

    <%--    <style>
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

        .payment {
            display: ruby;
        }

        .pagination {
            text-align: center;
            margin-top: 15px;
            justify-content: flex-end !important;
        }

        .steps {
            padding: 10px;
            font-size: 14px;
            font-weight: 500;
            color: #000;
            border-bottom: 1px solid #ccc;
        }

        #pagination a {
            margin: 0 5px;
            cursor: pointer;
            padding: 5px 10px;
            border: 1px solid #ccc;
        }

            #pagination a.active {
                background-color: #6777ef;
                color: white;
                border: 1px solid #6777ef;
            }
        /* ====== Marquee ====== */
        marquee {
            color: #2f8dd3;
            font-weight: bold;
            margin: 10px 0 15px;
        }

            marquee a {
                color: #003399;
                text-decoration: underline;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <marquee behavior="scroll" direction="left" scrollamount="3">
        <span style="color: red;">
            <i class="fa fa-phone fa-flip-horizontal"></i>Bihar School Examination Board Helpline : 0612-2230039
        </span>
        &nbsp;&nbsp;&nbsp;
       <span style="color: blue;">For any query related to Student's Registration please contact at
           <a href="mailto:bsebinterhelpdesk@gmail.com">
               <i class="fa fa-envelope"></i>bsebinterhelpdesk@gmail.com
           </a>
       </span>
    </marquee>

    <div class="row">
        <div class="col-12">
            <div class="card">

                <!-- ✅ Steps -->
                <%-- <div class="steps">
                <p class="font-13 font-bold" style="color: red">
                    नोट :- रजिस्ट्रेशन फॉर्म की लास्ट तिथि को चालान के माध्यम से पेमेंट न करे कृप्या अंतिम तिथि में ऑनलाइन नेट बैंकिंग का प्रयोग करें |
                </p>
                Follow The Step: 1.Login --> 2.Download Registration form --> 3.Make Payment -->
                4.Payment Status Verification --> 5.Fill Student Registration Form -->
                6.Display Registered Students --> 7.Logout
            </div>--%>

                <!-- ✅ Card Header -->
                <div class="card-header">
                    <h4>Registration Payment</h4>
                </div>

                <!-- ✅ Card Body -->
                <div class="card-body">

                    <!-- 🔹 Registration Form -->
                    <div class="form-row">
                        <div class="form-group col-md-6" runat="server" id="txt_location">
                            <label for="location">+2 School/College Code & Name<span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" ID="txt_collegename" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                            <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                        </div>

                        <div class="form-group col-md-6">
                            <label for="faculty">Faculty<span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control form-select"></asp:DropDownList>
                            <span id="facultyError" style="display: none; color: red;">Please Select Faculty</span>
                        </div>

                        <div class="form-group col-md-12">
                            <label for="category">Student Category</label>
                            <asp:DropDownList runat="server" ID="ddl_category" CssClass="col-md-6 form-control form-select">
                                <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                <asp:ListItem Value="Private">Private</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group col-md-6" style="display: grid;">
                            <label>Payment Status Condition</label>
                            <div class="payment">
                                <div class="form-check">
                                    <asp:RadioButton runat="server" ID="rdo_makepayment" CssClass="font-weight-bold ml-2 text-success" Text="&nbsp;&nbsp;Make Payment" GroupName="rdo" Style="margin-right: 5px;" />
                                </div>
                                <div class="form-check">
                                    <asp:RadioButton runat="server" ID="rdo_payemntstatus" CssClass="font-weight-bold ml-2 text-danger" Text="&nbsp;&nbsp;Payment Status" GroupName="rdo" Style="margin-right: 5px;" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-md-6">
                            <label for="paymode">Pay Mode</label>
                            <asp:DropDownList runat="server" ID="ddl_paymode" CssClass="form-control form-select">
                                <%--  <asp:ListItem Value="ALL">Select Bank</asp:ListItem>--%>
                                <asp:ListItem Selected="True" Value="Indian Bank">Indian Bank</asp:ListItem>
                                <%--  <asp:ListItem Value="Axis Bank">Axis Bank</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group col-md-6">
                            <label>Available Seat in Regular</label>
                            <asp:TextBox runat="server" ID="txt_regularseats" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Available Seat in Private</label>
                            <asp:TextBox runat="server" ID="txt_privateseats" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <!-- 🔹 Buttons -->
                    <div class="form-group mt-3">
                        <asp:Button runat="server" ID="btn_getdetails" CssClass="btn btn-primary mr-2" Text="GET DETAILS" OnClick="btn_getdetails_Click" OnClientClick="return validateFaculty();" />
                    </div>

                    <!-- 🔹 No Records -->
                    <div class="col-12" id="divpnlNoRecords" runat="server" visible="false">
                        <asp:Panel ID="pnlNoRecords" runat="server" CssClass="alert alert-danger text-center mt-3">
                            No student records found matching your criteria.
                        </asp:Panel>
                    </div>

                    <!-- 🔹 Student Details -->
                    <div class="col-12" id="divstudentdetails" runat="server">
                        <div class="section-title">
                            <h6>Student Details</h6>
                        </div>
                        <%--  <div class="text-danger mt-3">
                        <strong>नोट: Student Registration में उन्ही छात्रों का नाम दिखेगा जिनका शुल्क Success होगा।</strong>
                    </div>--%>
                        <div class="h6 mt-3 text-center">
                            <asp:HiddenField ID="hfTotalAmount" runat="server" />
                            <strong>Total Selected Fee Amount: ₹ <span id="totalAmount">0</span></strong>
                        </div>

                        <hr />

                        <div class="form-group mt-3 text-right" id="searchInputDIV" runat="server" visible="false">
                            <input type="text" id="searchInput" class="form-control" placeholder="Search by Student, Father, Mother Name or DOB" style="width: 300px; display: inline-block;" onkeyup="filterAndPaginate();" />
                        </div>

                        <div class="table-responsive">
                            <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                                <table class="table table-hover table-bordered dataTable" id="dataTable">
                                    <thead>
                                        <tr>
                                            <th class="repeater-checkbox">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" />
                                                <asp:HiddenField ID="hfSelectedIds" runat="server" />
                                                <asp:HiddenField ID="hfSelectedStudentFees" runat="server" />
                                            </th>
                                           
                                            <th>OFSS No.</th>
                                            <th>Name</th>
                                            <th>Father Name</th>
                                            <th>Mother Name</th>
                                            <th>DOB</th>
                                            <th>Board Name</th>
                                            <th>Category</th>
                                            <th>Fee Amount</th>
                                            <th>Delete</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tableBody">
                                        <asp:Repeater runat="server" ID="rptStudents" EnableViewState="false" ClientIDMode="Static">
                                            <ItemTemplate>
                                                <tr data-visible="true">
                                                    <td class="repeater-checkbox">
                                                        <asp:CheckBox ID="chkRowSelect" runat="server" AutoPostBack="false" OnClientClick="updateSelectAllState();" />
                                                        <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                    </td>
                                                    <td><%#Eval("OfssReferenceNo") %></td>
                                                    <td><%#Eval("Name") %></td>
                                                    <td><%# Eval("FatherName") %></td>
                                                    <td><%# Eval("MotherName") %></td>
                                                    <td><%# Eval("Dob") != DBNull.Value ? string.Format("{0:dd-MM-yyyy}", Eval("Dob")) : "" %></td>
                                                    <td><%# Eval("BoardName") %></td>
                                                    <td><%# Eval("CategoryName") %></td>
                                                    <td class="fee-amount" data-amount='<%# Eval("FeeAmount") %>'>
                                                        <asp:Label ID="lblFee" runat="server" Text='<%# Eval("FeeAmount") %>' />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                <asp:Label ID="lblEntriesCount" runat="server" CssClass="mt-2 d-block text-left"></asp:Label>
                            </asp:Panel>

                            <div class="text-center">
                                <asp:Button runat="server" ID="btn_paynow" Text="Paynow" CssClass="btn btn-warning" OnClick="btn_paynow_Click" />
                            </div>
                        </div>

                        <asp:Panel ID="pnlPager" runat="server" CssClass="pagination" Visible="true">
                            <div id="pagination"></div>
                        </asp:Panel>
                    </div>

                    <!-- 🔹 Payment Transaction -->
                    <div class="col-12" id="divpayment" runat="server">
                        <div class="section-title">
                            <h6>Transaction Details</h6>
                        </div>
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered" id="table-1">
                                <thead>
                                    <tr>
                                        <th>S. No.</th>
                                        <th>+2 School/College Code</th>
                                        <th>Transaction ID</th>
                                        <th>Student No</th>
                                        <th>Payment Amount</th>
                                        <th>Payment Initiate Date</th>
                                        <th>Payment Updated Date</th>
                                        <th>Status</th>
                                        <th>View Details</th>
                                        <th>Delete</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rpt_getpayemnt" OnItemCommand="rpt_getpayemnt_ItemCommand" OnItemDataBound="rpt_getpayemnt_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" /></td>
                                                <td><%# Eval("CollegeCode") %></td>
                                                <td><%# Eval("ClientTxnId") %></td>
                                                <td><%# Eval("StudentsPerTransaction") %></td>
                                                <td><%# Eval("PaymentAmount") %></td>
                                                <td><%# Eval("PaymentInitiateDate", "{0:dd-MM-yyyy}") %></td>
                                                <td><%# Eval("PaymentUpdatedDate", "{0:dd-MM-yyyy}") %></td>
                                                <td><%# Eval("PaymentStatus") %></td>
                                                <asp:HiddenField runat="server" ID="hf_status" Value='<%# Eval("PaymentStatus") %>' />
                                                <td>
                                                    <div style="display: inline-flex;">
                                                        <!-- View Button -->
                                                        <a href='viewpaymentdetails.aspx?id=<%# Eval("ClientTxnId") %>'
                                                            class="btn btn-sm btn-primary me-2"
                                                            target="_blank">View
                                                        </a>

                                                        <!-- Download Receipt Button -->
                                                        <asp:LinkButton ID="lnkDownload"
                                                            runat="server"
                                                            CommandArgument='<%# Eval("PaymentReceiptPath") %>'
                                                            OnCommand="DownloadReceipt"
                                                            CssClass="btn btn-sm btn-primary"
                                                            Visible='<%# !string.IsNullOrEmpty(Eval("PaymentReceiptPath").ToString()) %>'>
    Receipt
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="lnk_Delete" CommandArgument='<%# Eval("ClientTxnId") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete this record?');">Delete</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <!-- end card-body -->
            </div>
            <!-- end card -->
        </div>
    </div>


    <%--<script>
    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll(".open-modal").forEach(function (el) {
            el.addEventListener("click", function () {
                const targetId = el.getAttribute("data-target");
                const modalElement = document.getElementById(targetId);

                let modalInstance;
                if (!modalElement.modalInstance) {
                    modalInstance = new bootstrap.Modal(modalElement, {
                        backdrop: false,
                        keyboard: true
                    });
                    modalElement.modalInstance = modalInstance; // store in DOM element
                } else {
                    modalInstance = modalElement.modalInstance;
                }

                modalInstance.show();
            });
        });
    });
</script>--%>

    <script>


        function validateFaculty() {
            debugger
            var facultyDropdown = document.getElementById('<%= ddlFaculty.ClientID %>');
            var errorSpan = document.getElementById('facultyError');

            var collegeNameInput = document.getElementById('<%= txt_collegename.ClientID %>');
            var collegeNameErrorSpan = document.getElementById('CollegeNameError');
            if (collegeNameInput.value.trim() === "") {
                collegeNameErrorSpan.style.display = "inline";
                collegeNameInput.classList.add("is-invalid");
                collegeNameInput.focus();
                return false;
            } else {
                collegeNameErrorSpan.style.display = "none";
                collegeNameInput.classList.remove("is-invalid");
            }

            if (facultyDropdown.value === "0" || facultyDropdown.value === "") {
                errorSpan.style.display = "inline";

                facultyDropdown.focus();
                return false;
            } else {
                errorSpan.style.display = "none";
                facultyDropdown.classList.remove("is-invalid");
                return true;
            }
        }




        var currentPage = 1;
        var rowsPerPage = 100;

        document.addEventListener("DOMContentLoaded", function () {
            setupSelectAll();
            filterAndPaginate();
            updateTotalAmount();
        });


        function getSelectedData() {
            var ids = [];
            var fees = [];

            document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                var cb = row.querySelector("input[type=checkbox]");
                var hf = row.querySelector("input[type=hidden][id$='hfStudentID']");
                var feeCell = row.querySelector(".fee-amount");

                if (cb && cb.checked && hf && feeCell) {
                    var studentId = hf.value;
                    var amount = parseFloat(feeCell.getAttribute("data-amount")) || 0;

                    ids.push(studentId);
                    fees.push(studentId + ":" + amount); // build mapping
                }
            });

            return { ids: ids, fees: fees };
        }

        function updateHiddenField() {
            var data = getSelectedData();

            // already present hidden field for IDs
            var hiddenIds = document.getElementById('<%= hfSelectedIds.ClientID %>');
            hiddenIds.value = data.ids.join(',');

            // new hidden field for ID:Fee mapping
            var hiddenFees = document.getElementById('<%= hfSelectedStudentFees.ClientID %>');
            hiddenFees.value = data.fees.join(',');
        }


      <%--  function getSelectedIds() {
            var ids = [];
            document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                var cb = row.querySelector("input[type=checkbox]");
                var hf = row.querySelector("input[type=hidden][id$='hfStudentID']");
                if (cb && cb.checked && hf) {
                    ids.push(hf.value);
                }
            });
            return ids;
        }

        function updateHiddenField() {
            var hiddenField = document.getElementById('<%= hfSelectedIds.ClientID %>');
            hiddenField.value = getSelectedIds().join(',');
        }--%>

        function updateTotalAmount() {
            var total = 0;
            document.querySelectorAll('#dataTable tbody tr[data-visible="true"]').forEach(function (row) {
                var cb = row.querySelector("input[type=checkbox]");
                if (cb && cb.checked) {
                    var feeCell = row.querySelector(".fee-amount");
                    if (feeCell) {
                        var amount = parseFloat(feeCell.getAttribute("data-amount")) || 0;
                        total += amount;
                    }
                }
            });
            document.getElementById("totalAmount").innerText = total.toFixed(2);

            var hf = document.getElementById('<%= hfTotalAmount.ClientID %>');
            if (hf) {
                hf.value = total.toFixed(2);
            }
        }

        function attachRowHandlers() {
            document.querySelectorAll("#dataTable tbody input[type=checkbox]").forEach(function (cb) {
                cb.removeEventListener('change', handleCheckboxChange);
                cb.addEventListener('change', handleCheckboxChange);
            });
        }

        function handleCheckboxChange() {
            updateSelectAllState();
            updateTotalAmount();
        }

     <%--   function setupSelectAll() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            if (!master) return;

            master.addEventListener('change', function () {
                var checked = this.checked;
                document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                    if (row.dataset.visible !== "false") {
                        var cb = row.querySelector("input[type=checkbox]");
                        if (cb) cb.checked = checked;
                    }
                });
                updateSelectAllState();
                updateTotalAmount();
            });

            attachRowHandlers();
            updateSelectAllState();
            updateTotalAmount();
        }--%>
        function setupSelectAll() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            if (!master) return;

            master.addEventListener('change', function () {
                var checked = this.checked;
                // ✅ Select only the rows currently shown (not hidden by pagination)
                document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                    if (row.style.display !== "none") {
                        var cb = row.querySelector("input[type=checkbox]");
                        if (cb) cb.checked = checked;
                    }
                });
                updateSelectAllState();
                updateTotalAmount();
            });


            attachRowHandlers();
            updateSelectAllState();
            updateTotalAmount();
        }

      <%--  function updateSelectAllState() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            var checkboxes = document.querySelectorAll('#dataTable tbody tr[data-visible="true"] input[type=checkbox]');
            var total = checkboxes.length;
            var checkedCount = 0;

            checkboxes.forEach(function (cb) {
                if (cb.checked) checkedCount++;
            });

            if (checkedCount === 0) {
                master.checked = false;
                master.indeterminate = false;
            } else if (checkedCount === total) {
                master.checked = true;
                master.indeterminate = false;
            } else {
                master.checked = false;
                master.indeterminate = true;
            }

            updateHiddenField();
        }--%>

        function updateSelectAllState() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            var checkboxes = document.querySelectorAll('#dataTable tbody tr');
            var visibleCheckboxes = [];

            checkboxes.forEach(function (row) {
                if (row.style.display !== "none") {
                    var cb = row.querySelector("input[type=checkbox]");
                    if (cb) visibleCheckboxes.push(cb);
                }
            });

            var total = visibleCheckboxes.length;
            var checkedCount = visibleCheckboxes.filter(cb => cb.checked).length;

            if (checkedCount === 0) {
                master.checked = false;
                master.indeterminate = false;
            } else if (checkedCount === total) {
                master.checked = true;
                master.indeterminate = false;
            } else {
                master.checked = false;
                master.indeterminate = true;
            }

            updateHiddenField();
        }

        function filterAndPaginate() {
            var searchText = document.getElementById("searchInput").value.toLowerCase();
            var rows = document.querySelectorAll("#dataTable tbody tr");

            rows.forEach(function (row) {
                var OfssReferenceNo = row.cells[1].textContent.toLowerCase();
                var studentName = row.cells[2].textContent.toLowerCase();
                var fatherName = row.cells[3].textContent.toLowerCase();
                var motherName = row.cells[4].textContent.toLowerCase();
                var dob = row.cells[5].textContent.toLowerCase();
                var BoardName = row.cells[6].textContent.toLowerCase();
                var Category = row.cells[7].textContent.toLowerCase();
                var FeeAmount = row.cells[8].textContent.toLowerCase();

                var match = OfssReferenceNo.includes(searchText) || studentName.includes(searchText) ||
                    fatherName.includes(searchText) || motherName.includes(searchText) ||
                    dob.includes(searchText) || BoardName.includes(searchText) ||
                    Category.includes(searchText) || FeeAmount.includes(searchText);

                row.dataset.visible = match ? "true" : "false";
            });

            currentPage = 1;
            paginateFilteredTable();
        }

        function paginateFilteredTable() {
            var allRows = document.querySelectorAll("#dataTable tbody tr");
            var visibleRows = Array.from(allRows).filter(function (row) {
                return row.dataset.visible !== "false";
            });

            var totalRows = visibleRows.length;
            var totalPages = Math.ceil(totalRows / rowsPerPage) || 1;

            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;

            allRows.forEach(function (row) {
                row.style.display = "none";
            });

            var start = (currentPage - 1) * rowsPerPage;
            var end = start + rowsPerPage;

            visibleRows.slice(start, end).forEach(function (row) {
                row.style.display = "";
            });

            renderPagination(totalPages);
            attachRowHandlers();
            updateSelectAllState();
            updateTotalAmount();

            var lblEntries = document.getElementById('<%= lblEntriesCount.ClientID %>');
            if (totalRows === 0) {
                lblEntries.innerText = "No entries found";
            } else {
                lblEntries.innerText = `Showing ${start + 1} to ${Math.min(end, totalRows)} of ${totalRows} entries`;
            }
        }

        function renderPagination(totalPages) {
            var container = document.getElementById('pagination');
            container.innerHTML = '';

            if (totalPages <= 1) return;

            // === Prev button ===
            var prev = document.createElement('a');
            prev.textContent = 'Prev';
            prev.href = 'javascript:void(0);';
            if (currentPage === 1) prev.classList.add('disabled');
            prev.addEventListener('click', function () {
                if (currentPage > 1) {
                    currentPage--;
                    paginateFilteredTable();
                }
            });
            container.appendChild(prev);

            // === Page numbers with ellipses ===
            var maxVisible = 1; // how many page links to show at once
            var startPage = Math.max(1, currentPage - Math.floor(maxVisible / 2));
            var endPage = Math.min(totalPages, startPage + maxVisible - 1);

            if (startPage > 1) {
                addPageLink(container, 1);
                if (startPage > 2) addDots(container);
            }

            for (let i = startPage; i <= endPage; i++) {
                addPageLink(container, i, i === currentPage);
            }

            if (endPage < totalPages) {
                if (endPage < totalPages - 1) addDots(container);
                addPageLink(container, totalPages);
            }

            // === Next button ===
            var next = document.createElement('a');
            next.textContent = 'Next';
            next.href = 'javascript:void(0);';
            if (currentPage === totalPages) next.classList.add('disabled');
            next.addEventListener('click', function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    paginateFilteredTable();
                }
            });
            container.appendChild(next);
        }

        function addPageLink(container, pageNum, isActive = false) {
            var link = document.createElement('a');
            link.textContent = pageNum;
            link.href = 'javascript:void(0);';
            if (isActive) link.classList.add('active');
            link.addEventListener('click', function () {
                currentPage = pageNum;
                paginateFilteredTable();
            });
            container.appendChild(link);
        }

        function addDots(container) {
            var span = document.createElement('span');
            span.textContent = '...';
            container.appendChild(span);
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>
