<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Table with Pagination & Select All</title>
  <style>
    table {
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
    }
    .pagination {
      text-align: center;
      margin-top: 10px;
    }
    .pagination button {
      margin: 2px;
      padding: 5px 10px;
    }
  </style>
</head>
<body>

<h2 style="text-align:center;">Custom Table with Pagination & Select All</h2>

<table id="dataTable">
  <thead>
    <tr>
      <th><input type="checkbox" id="selectAll" onclick="toggleSelectAll(this)"> Select All</th>
      <th>ID</th>
      <th>Name</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>

<div class="pagination" id="pagination"></div>
    <script>
  // Sample Data
  const data = [];
  for (let i = 1; i <= 53; i++) {
    data.push({ id: i, name: "Name " + i });
  }

  const rowsPerPage = 10;
  let currentPage = 1;

  // Store selected rows globally (works across pages)
  let selectedRows = new Set();

  function renderTable(page) {
    currentPage = page;
    const tbody = document.querySelector("#dataTable tbody");
    tbody.innerHTML = "";

    const start = (page - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    const pageData = data.slice(start, end);

    pageData.forEach(row => {
      const tr = document.createElement("tr");
      const checked = selectedRows.has(row.id) ? "checked" : "";
      tr.innerHTML = `
        <td><input type="checkbox" value="${row.id}" ${checked} onclick="toggleRow(this)"></td>
        <td>${row.id}</td>
        <td>${row.name}</td>
      `;
      tbody.appendChild(tr);
    });

    renderPagination();
    updateSelectAllCheckbox();
  }

  function renderPagination() {
    const totalPages = Math.ceil(data.length / rowsPerPage);
    const pagination = document.getElementById("pagination");
    pagination.innerHTML = "";

    for (let i = 1; i <= totalPages; i++) {
      const btn = document.createElement("button");
      btn.textContent = i;
      if (i === currentPage) btn.disabled = true;
      btn.onclick = () => renderTable(i);
      pagination.appendChild(btn);
    }
  }

  function toggleRow(checkbox) {
    const id = parseInt(checkbox.value);
    if (checkbox.checked) {
      selectedRows.add(id);
    } else {
      selectedRows.delete(id);
    }
    updateSelectAllCheckbox();
  }

  function toggleSelectAll(master) {
    if (master.checked) {
      // Add all rows across all pages
      data.forEach(row => selectedRows.add(row.id));
    } else {
      // Clear all selections
      selectedRows.clear();
    }
    renderTable(currentPage);
  }

  function updateSelectAllCheckbox() {
    const master = document.getElementById("selectAll");
    if (selectedRows.size === data.length) {
      master.checked = true;
      master.indeterminate = false;
    } else if (selectedRows.size > 0) {
      master.checked = false;
      master.indeterminate = true;
    } else {
      master.checked = false;
      master.indeterminate = false;
    }
  }

  // Initialize
  renderTable(1);
    </script>

</body>
</html>

