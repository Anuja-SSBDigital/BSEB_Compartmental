<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DownloadPracticaladmitcard.aspx.cs" Inherits="DownloadPracticaladmitcard" %>
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
    /* table {
         border-collapse: collapse;
         width: 80%;
         margin: 20px auto;
     }
     th, td {
         border: 1px solid #ddd;
         padding: 8px;
         text-align: center;
     }
     th {
         background: #f2f2f2;
     }*/
     .pagination {
         text-align: center;
         margin-top: 15px;
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
         background-color: #4CAF50;
         color: white;
         border: 1px solid #4CAF50;
     }
     .pagination a:hover:not(.active) {
         background-color: #ddd;
     }
    </style>
                <script type="text/javascript">
                    function validateFaculty() {
                        var ddlFaculty = document.getElementById('<%= ddlFaculty.ClientID %>');
                 var facultyError = document.getElementById('facultyError');

                 var examDropdown = document.getElementById('<%= ddlExamcat.ClientID %>');
                 var examerror = document.getElementById('ExamCatError');

                 var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
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

                        if (ddlFaculty.value === "0" || ddlFaculty.value === "") {
                            facultyError.style.display = 'block';
                            return false;
                        } else {
                            facultyError.style.display = 'none';
                            //return true;
                        }

                        if (examDropdown.value === "0" || examDropdown.value === "") {
                            examerror.style.display = "inline";
                            examDropdown.classList.add("is-invalid");
                            examDropdown.focus();
                            return false;
                        } else {
                            examDropdown.classList.remove("is-invalid");
                        }

                    }

                    var currentPage = 1;
                    var rowsPerPage = 25;

                    document.addEventListener("DOMContentLoaded", function () {
                        setupSelectAll();
                        filterAndPaginate(); // Load initial table
                    });

                    function getSelectedIds() {
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

        // ✅ Only select rows visible on current page
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
    // ✅ Only check visible rows (current page)
    var checkboxes = document.querySelectorAll('#dataTable tbody tr[data-visible="true"]');
    var visibleCheckboxes = Array.from(checkboxes).filter(function (row) {
        return row.style.display !== "none";
    }).map(function (row) {
        return row.querySelector("input[type=checkbox]");
    });

    var total = visibleCheckboxes.length;
    var checkedCount = visibleCheckboxes.filter(cb => cb && cb.checked).length;

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


// ====== 🔍 Filtering + Pagination ======
function filterAndPaginate() {
    var searchText = document.getElementById("searchInput").value.toLowerCase();
    var rows = document.querySelectorAll("#dataTable tbody tr");

    rows.forEach(function (row) {
        var studentName = row.cells[1].textContent.toLowerCase();
        var fatherName = row.cells[2].textContent.toLowerCase();
        var motherName = row.cells[3].textContent.toLowerCase();
        var dob = row.cells[4].textContent.toLowerCase();

        var match = studentName.includes(searchText) ||
            fatherName.includes(searchText) ||
            motherName.includes(searchText) ||
            dob.includes(searchText);

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

 </asp:Content>
   

    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h4>Practical Admit Card</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>+2 School/College Code & Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txt_CollegeName" runat="server" CssClass="form-control" placeholder="Enter CollegeCode"></asp:TextBox>
                                    <span id="CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Faculty<span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="facultyError" class="text-danger" style="display: none;">Please select a Faculty.</span>
                                </div>
                            </div>
                           <div class="col-md-6">
                                <div class="form-group">
                                    <label>Exam Category <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlExamcat" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="ExamCatError" class="text-danger" style="display: none;">Please select a Exam Category.</span>
                                </div>
                           </div>
                        </div>

                        <div class="form-group mt-4 text-center">
                            <asp:Button ID="btnviewrecord" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClick="btnGetStudentDummyExamData" OnClientClick="return validateFaculty();" />
                        </div>
                        <div class="text-right mt-3">
                            <asp:Button ID="btnDownloadPDF" runat="server" Text="Download PDF" CssClass="btn btn-success" OnClick="btnDownloadPDF_Click" Visible="false" />
                        </div>
                        <hr />
                             <!-- 🔍 Search Box -->
      <div class="form-group mt-3 text-right" id="searchInputDIV" runat="server" Visible="false" >
          <input type="text" id="searchInput" class="form-control" placeholder="Search by Student, Father, Mother Name or DOB" style="width: 300px; display: inline-block;" onkeyup="filterAndPaginate();" />
      </div>
<div class="table-responsive">
                  <asp:Panel ID="pnlStudentTable" runat="server" Visible="false">
                      <table class="table table-hover table-bordered dataTable" id="dataTable">
                          <thead>
                              <tr>
                                  <th class="repeater-checkbox">
                                      <asp:HiddenField ID="hfSelectedIds" runat="server" />
                                      <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" />
                                      Select All
                                  </th>
                                  <th>Registration No</th>
                                      <th>Roll No</th>
                                     <th>Student Name</th>
                                            <th>Father Name</th>
                                            <th>Mother Name</th>
                                            <%--<th>Faculty</th>--%>
                                            <th>Gender</th>
                                        
                              </tr>
                          </thead>
                          <tbody id="tableBody">
                              <asp:Repeater runat="server" ID="rptStudents" EnableViewState="false" ClientIDMode="Static">
                                  <ItemTemplate>
                                      <tr data-visible="true">
                                          <td class="repeater-checkbox">
                                              <asp:CheckBox ID="chkRowSelect" runat="server" AutoPostBack="false" OnClientClick="updateSelectAllState();" />
                                              <asp:HiddenField ID="hfStudentID" runat="server" Value='<%# Eval("StudentID") %>' />
                                              <asp:HiddenField ID="hfCollege" runat="server" Value='<%# Eval("CollegeId") %>' />
                                              <asp:HiddenField ID="hfFaculty" runat="server" Value='<%# Eval("FacultyId") %>' />
                                              <asp:HiddenField ID="hfexamtypid" runat="server" Value='<%# Eval("ExamTypeId") %>' />
                                          </td>
                                           <td class="repeater-col"><%# Eval("RegistrationNo") %></td>
                                             <td class="repeater-col"><%# Eval("RollNumber") %></td>
                                         <td class="repeater-col"><%# Eval("StudentName") %></td>
                                                    <td class="repeater-col"><%# Eval("FatherName") %></td>
                                                    <td class="repeater-col"><%# Eval("MotherName") %></td>
                                                    <%--<td class="repeater-col">
                                                        <%# Eval("Faculty") %>
                                                        <asp:HiddenField ID="hfFaculty" runat="server" Value='<%# Eval("FacultyId") %>' />
                                                          <asp:HiddenField ID="hfexamtypid" runat="server" Value='<%# Eval("ExamTypeId") %>' />
                                                    </td>--%>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("FacultyId") %>' />
                                                    <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("ExamTypeId") %>' />
                                                    <td class="repeater-col"><%# Eval("GenderName") %></td>
                                                 
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

              <asp:Panel ID="pnlPager" runat="server" CssClass="pagination" Visible="false">
                  <div id="pagination"></div>
              </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
      
        <%-- <script type="text/javascript">
             function validateFaculty() {
                 var ddlFaculty = document.getElementById('<%= ddlFaculty.ClientID %>');
            var facultyError = document.getElementById('facultyError');

            var collegeNameInput = document.getElementById('<%= txt_CollegeName.ClientID %>');
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

                 if (ddlFaculty.value === "0" || ddlFaculty.value === "") {
                     facultyError.style.display = 'block';
                     return false;
                 } else {
                     facultyError.style.display = 'none';
                     return true;
                 }
             }

             function toggleDownloadButton() {
                 var anyChecked = false;
                 var checkboxes = document.querySelectorAll('input[type="checkbox"][id$="chkRowSelect"]');
                 checkboxes.forEach(function (checkbox) {
                     if (checkbox.checked) {
                         anyChecked = true;
                     }
                 });

                 var btn = document.getElementById("btnDownloadPDF");
                 if (btn) {
                     btn.style.display = anyChecked ? "inline-block" : "none";
                 }
             }

             function selectAllCheckboxes(source) {
                 var checkboxes = document.querySelectorAll('input[type="checkbox"][id$="chkRowSelect"]');
                 checkboxes.forEach(function (checkbox) {
                     checkbox.checked = source.checked;
                 });
                 toggleDownloadButton();
             }

             window.onload = function () {
                 toggleDownloadButton();
             };

         </script>--%>
    </asp:Content>
