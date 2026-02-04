<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bihar School Examination Board</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        body {
            margin: 0;
            font-family: Arial, sans-serif;
            text-align: center;
        }

        /* Header (Logo only, responsive) */
        .header img {
            max-width: 100%;
            height: auto;
            display: block;
            margin: 0 auto;
        }

        /* Buttons */
        .btn {
            display: inline-block;
            margin: 20px auto;
            padding: 15px 25px;
            font-size: 18px;
            font-weight: bold;
            text-decoration: none;
            color: #fff;
            border-radius: 6px;
            transition: 0.3s;
        }
        .btn-purple {
            background: #673ab7;
        }
        .btn-purple:hover {
            background: #5e35b1;
        }
        .btn-blue {
            background: #003366;
        }
        .btn-blue:hover {
            background: #002244;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header Section (Logo only) -->
        <div class="header">
            <img src="assets/img/logo_bseb_homepage.jpg" alt="BSEB Logo" />
        </div>

        <!-- Buttons -->
        <div>
            <a href="login.aspx" class="btn btn-purple">
                Click here for Registration Session Intermediate 2025-2027 (Colllege Login)
            </a>
        </div>
     <%--   <div>
            <a href="#" class="btn btn-blue">
                Click here for 12th BBOSE 2nd Senior Secondary Examination , December 2024 - Admit Card (Student Login)
            </a>
        </div>--%>
    </form>
</body>
</html>
