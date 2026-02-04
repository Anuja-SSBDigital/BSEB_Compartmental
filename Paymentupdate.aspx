<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Paymentupdate.aspx.cs" Inherits="Paymentupdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table-responsive { margin-top: 20px; }
        .repeater-checkbox { text-align: center; width: 40px; }
        .repeater-col { padding: 8px; }
        table { border-collapse: collapse !important; width: 100%; }
        table th, table td { border: 1px solid #333 !important; padding: 10px; font-size: 14px; vertical-align: middle; }
        table tr:nth-child(even) { background-color: #e9e9e9; }
        .pagination {
      text-align: center;
      margin-top: 15px;
      justify-content: flex-end !important;
  }

      .pagination a, .pagination span {
          display: inline-block;
          padding: 8px 16px;
          text-decoration: none;
          color: black;
          border: 1px solid #ddd;
          margin: 0 4px;
      }

          .pagination a.active {
              background-color: #6777ef;
              color: white;
              border: 1px solid #6777ef;
          }

          .pagination a:hover:not(.active) {
              background-color: #ddd;
          }
        .form-col { width: 50px; text-align: center; white-space: nowrap; padding: 2px 4px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Payment Update</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="form-group col-md-4">
     <label for="paymode">Pay Mode<span class="text-danger">*</span></label>
     <asp:DropDownList runat="server" ID="ddl_paymode" CssClass="form-control form-select">
         <asp:ListItem Value="ALL">Select Bank</asp:ListItem>
         <asp:ListItem Value="Indian Bank">Indian Bank</asp:ListItem>
         <asp:ListItem Text="Axis Bank" Value="Axis Bank"></asp:ListItem>
     </asp:DropDownList>
     <span id="BankError" style="display: none; color: red;">Please Select Bank</span>
 </div>
                        <div class="form-group col-md-4">
                            <label>From Date</label>
                            <asp:TextBox ID="txtfromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                             <span id="fromDateError" style="color:red;font-size:13px;"></span>
                        </div>
                        <div class="form-group col-md-4">
                            <label>To Date</label>
                            <asp:TextBox ID="txttoDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                             <span id="toDateError" style="color:red;font-size:13px;"></span>
                        </div>
                    </div>
                    <div class="form-group mt-3">
                        <asp:Button runat="server" ID="btn_getdetails" CssClass="btn btn-primary mr-2" OnClick="btn_getdetails_Click" Text="GET DETAILS"  OnClientClick="return validateDates();" />
                    </div>

                    <!-- 🔍 Search Box -->
                    <div class="form-group mt-3 text-right" id="searchInputDIV" runat="server" visible="false">
                        <input type="text" id="searchInput" class="form-control" placeholder="Search by Registration No" style="width: 300px; display: inline-block;" onkeyup="filterAndPaginate();" />
                    </div>

                    <div class="table-responsive">
                        <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                             <div class="col-md-12 text-lg-right">
                                <asp:Button ID="UpdateAllTransaction" runat="server" OnClick="UpdateAllTransaction_Click" Text="Update All Transaction" CssClass="btn btn-primary mb-2" />
                            </div>
                            <table class="table table-hover table-bordered dataTable" id="dataTable">
                                <thead>
                                    <tr>
                                        <th class="repeater-checkbox">
                                            <asp:HiddenField ID="hfSelectedIds" runat="server" />
                                            <asp:HiddenField ID="hfselectedclientxnid" runat="server" />
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" />
                                        </th>
                                        <th>S.No.</th>
                                        <th>+2 School/College Code</th>
                                        <th>Transaction ID</th>
                                        <th>Student No</th>
                                        <th>Payment Amount</th>
                                        <th>Status</th>
                                    </tr>
                                </thead>
                                <tbody id="tableBody">
                                    <asp:Repeater runat="server" ID="rpt_getpayemnt" EnableViewState="false" ClientIDMode="Static">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="repeater-checkbox">
                                                    <asp:CheckBox ID="chkRowSelect" runat="server" AutoPostBack="false" OnClientClick="updateSelectAllState();" />
                                                    <asp:HiddenField ID="hf_PaymentId" runat="server" Value='<%# Eval("Pk_PaymentId") %>' />
                                                   
                                                </td>
                                                <td class="repeater-col"><%# Container.ItemIndex + 1 %></td>
                                                <td><%# Eval("CollegeCode") %></td>
                                                <td><%# Eval("ClientTxnId") %></td>
                                                <td><%# Eval("StudentsPerTransaction") %></td>
                                                <td><%# Eval("PaymentAmount") %></td>
                                                <td><%# Eval("PaymentStatus") %></td>
                                                <asp:HiddenField runat="server" ID="hf_status" Value='<%# Eval("PaymentStatus") %>' />
                                                <asp:HiddenField runat="server" ID="hf_clientxnid" Value='<%# Eval("ClientTxnId") %>' />
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                         
                            <asp:Label ID="lblEntriesCount" runat="server" CssClass="mt-2 d-block text-left"></asp:Label>
                        </asp:Panel>

                        <asp:Panel ID="pnlNoRecords" runat="server" Visible="false" CssClass="alert alert-danger text-center mt-3">
                            No student records found matching your criteria.
                        </asp:Panel>
                    </div>

                    <!-- Pagination Panel -->
                    <asp:Panel ID="pnlPager" runat="server" CssClass="pagination" Visible="false">
      <div id="pagination"></div>
  </asp:Panel>

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateDates() {
            var fromDate = document.getElementById('<%= txtfromDate.ClientID %>').value;
            var toDate = document.getElementById('<%= txttoDate.ClientID %>').value;

            // clear previous errors
            document.getElementById("fromDateError").innerText = "";
            document.getElementById("toDateError").innerText = "";

            var BankDropdown = document.getElementById('<%= ddl_paymode.ClientID %>');
            var BankError = document.getElementById('BankError');

            //let isValid = true;
            if (BankDropdown.value === "ALL" || BankDropdown.value === "") {
                BankError.style.display = "inline";

                BankDropdown.focus();
                return false;
            } else {
                BankError.style.display = "none";
                BankDropdown.classList.remove("is-invalid");

            }

            if (!fromDate) {
                document.getElementById("fromDateError").innerText = "Please select From Date.";
               return false;
            }

            if (!toDate) {
                document.getElementById("toDateError").innerText = "Please select To Date.";
                return false;
            }

            if (fromDate && toDate) {
                var from = new Date(fromDate);
                var to = new Date(toDate);

                if (from > to) {
                    document.getElementById("toDateError").innerText = "To Date must be greater than or equal to From Date.";
                    return false;
                }
            }


          

          //  return isValid; // false will stop postback
        }
        var currentPage = 1;
        var rowsPerPage = 100;

        document.addEventListener("DOMContentLoaded", function () {
            setupSelectAll();
            filterAndPaginate();
        });

        function getSelectedIds() {
            var ids = [];
            document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                var cb = row.querySelector("input[type=checkbox]");
                var hf = row.querySelector("input[type=hidden][id$='hfStudentID']");
                if (cb && cb.checked && hf) ids.push(hf.value);
            });
            return ids;
        }

        function updateHiddenField() {
            var hiddenField = document.getElementById('<%= hfSelectedIds.ClientID %>');
            hiddenField.value = getSelectedIds().join(',');
        }

        function attachRowHandlers() {
            document.querySelectorAll("#dataTable tbody input[type=checkbox]").forEach(function (cb) {
                cb.removeEventListener('change', updateSelectAllState);
                cb.addEventListener('change', updateSelectAllState);
            });
        }

        function setupSelectAll() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            if (!master) return;

            master.addEventListener('change', function () {
                var checked = this.checked;
                document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                    if (row.style.display !== "none") {
                        var cb = row.querySelector("input[type=checkbox]");
                        if (cb) cb.checked = checked;
                    }
                });
                updateSelectAllState();
            });

            attachRowHandlers();
            updateSelectAllState();
        }

        function updateSelectAllState() {
            var master = document.getElementById('<%= chkSelectAll.ClientID %>');
            var checkboxes = document.querySelectorAll('#dataTable tbody tr[data-visible="true"]');
            var visibleCheckboxes = Array.from(checkboxes).filter(row => row.style.display !== "none").map(row => row.querySelector("input[type=checkbox]"));

            var total = visibleCheckboxes.length;
            var checkedCount = visibleCheckboxes.filter(cb => cb && cb.checked).length;

            master.checked = checkedCount === total && total > 0;
            master.indeterminate = checkedCount > 0 && checkedCount < total;

            updateHiddenField();
        }

        function getSelectedData() {
            var ids = [];
            var txnid = [];

            document.querySelectorAll("#dataTable tbody tr").forEach(function (row) {
                var cb = row.querySelector("input[type=checkbox]");
                var hf = row.querySelector("input[type=hidden][id$='hf_PaymentId']");
                var clntxnid = row.querySelector("input[type=hidden][id$='hf_clientxnid']");

                if (cb && cb.checked && hf && clntxnid) {
                    var paymentId = hf.value;
                    var pyntxnid = clntxnid.value;

                    ids.push(paymentId);
                    txnid.push(pyntxnid); // ✅ push into the array
                }
            });

            return { ids: ids, txnid: txnid };
        }

        function updateHiddenField() {
            var data = getSelectedData();

            // already present hidden field for IDs
            var hiddenIds = document.getElementById('<%= hfSelectedIds.ClientID %>');
    hiddenIds.value = data.ids.join(',');

    // new hidden field for transaction ids
    var hiddentxnid = document.getElementById('<%= hfselectedclientxnid.ClientID %>');
    hiddentxnid.value = data.txnid.join(',');
}
        // 🔍 Filtering + Pagination
        function filterAndPaginate() {
            var searchText = document.getElementById("searchInput").value.toLowerCase();
            var rows = document.querySelectorAll("#dataTable tbody tr");

            rows.forEach(function (row) {
                var TransactionId = row.cells[3].textContent.toLowerCase();
                row.dataset.visible = TransactionId.includes(searchText) ? "true" : "false";
            });

            currentPage = 1;
            paginateFilteredTable();
        }

        function paginateFilteredTable() {
            var allRows = document.querySelectorAll("#dataTable tbody tr");
            var visibleRows = Array.from(allRows).filter(row => row.dataset.visible !== "false");

            var totalRows = visibleRows.length;
            var totalPages = Math.ceil(totalRows / rowsPerPage) || 1;
            currentPage = Math.min(Math.max(currentPage, 1), totalPages);

            allRows.forEach(row => row.style.display = "none");
            var start = (currentPage - 1) * rowsPerPage;
            var end = start + rowsPerPage;

            visibleRows.slice(start, end).forEach(row => row.style.display = "");
            renderPagination(totalPages);
            attachRowHandlers();
            updateSelectAllState();

            var lblEntries = document.getElementById('<%= lblEntriesCount.ClientID %>');
            lblEntries.innerText = totalRows === 0 ? "No entries found" : `Showing ${start + 1} to ${Math.min(end, totalRows)} of ${totalRows} entries`;
        }

        function renderPagination(totalPages) {
            var container = document.getElementById('pagination');
            if (!container) return;
            container.innerHTML = '';
            if (totalPages <= 1) return;

            // Prev
            var prev = document.createElement('a');
            prev.textContent = 'Prev';
            prev.href = 'javascript:void(0);';
            if (currentPage === 1) prev.classList.add('disabled');
            prev.addEventListener('click', () => { if (currentPage > 1) { currentPage--; paginateFilteredTable(); } });
            container.appendChild(prev);

            // Page numbers
            var maxVisible = 1;
            var startPage = Math.max(1, currentPage - Math.floor(maxVisible / 2));
            var endPage = Math.min(totalPages, startPage + maxVisible - 1);

            if (startPage > 1) { addPageLink(container, 1); if (startPage > 2) addDots(container); }
            for (let i = startPage; i <= endPage; i++) addPageLink(container, i, i === currentPage);
            if (endPage < totalPages) { if (endPage < totalPages - 1) addDots(container); addPageLink(container, totalPages); }

            // Next
            var next = document.createElement('a');
            next.textContent = 'Next';
            next.href = 'javascript:void(0);';
            if (currentPage === totalPages) next.classList.add('disabled');
            next.addEventListener('click', () => { if (currentPage < totalPages) { currentPage++; paginateFilteredTable(); } });
            container.appendChild(next);
        }

        function addPageLink(container, pageNum, isActive = false) {
            var link = document.createElement('a');
            link.textContent = pageNum;
            link.href = 'javascript:void(0);';
            if (isActive) link.classList.add('active');
            link.addEventListener('click', () => { currentPage = pageNum; paginateFilteredTable(); });
            container.appendChild(link);
        }

        function addDots(container) {
            var span = document.createElement('span');
            span.textContent = '...';
            container.appendChild(span);
        }
    </script>
</asp:Content>
