<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppModule.aspx.cs" Inherits="AppModule" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Module Dashboard</title>
    <!-- Font Awesome CDN -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet">
    <style>
        body {
            margin: 0;
            font-family: 'Segoe UI', sans-serif;
            background: #ffffff;
            color: #333;
        }

        /* ====== Top Header (Logo + Logout) ====== */
        .header-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #3059c4;
            padding: 5px 20px;
            color: white;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }

        .header-left {
            display: flex;
            align-items: center;
        }

            .header-left .logo {
                width: 42px;
                margin-right: 12px;
            }

            .header-left h2 {
                font-size: 16px;
                margin: 0;
                color: white;
                font-weight: normal;
            }

        .header-right h2 {
            font-size: 16px;
            margin: 0;
            color: white;
            font-weight: normal;
        }

        .header-left h1 strong {
            font-weight: 600;
        }

        .header-right {
            display: flex;
            align-items: center;
            gap: 15px;
            font-size: 15px;
        }

        .logout-link {
            color: white;
            text-decoration: none;
            display: flex;
            align-items: center;
            gap: 6px;
            font-weight: 600;
        }

            .logout-link:hover {
                text-decoration: underline;
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

        /* ====== Second Header (Modules + College Name) ====== */
        .header-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 8px 20px;
            margin-bottom: 15px;
        }

        .module-heading {
            font-size: 16px;
            font-weight: 600;
            color: #1b2642;
        }

        /* ====== Module Cards ====== */
        .modules {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
            padding: 0 20px;
        }

        .module-card {
            width: 250px;
            height: 130px;
            border-radius: 6px;
            color: #fff;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            transition: transform 0.2s;
            text-decoration: none;
        }

            .module-card:hover {
                transform: scale(1.03);
            }

        .blue {
            background-color: #2f8dd3;
        }

        .green {
            background-color: #48a17f;
        }

        .module-icon img {
            width: 50px;
            height: 50px;
            margin-bottom: 10px;
            filter: drop-shadow(0 2px 3px rgba(0,0,0,0.3));
            border-radius: 10px;
        }

        .module-title {
            font-size: 16px;
            font-weight: 500;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Main Header -->
        <header class="header-bar">
            <div class="header-left">
                <img src="assets/img/bsebimage2.png" alt="BSEB Logo" class="logo" />
                <h2><strong>Bihar</strong> School Examination Board</h2>
            </div>
            <div class="header-right">
                <h2> <a href="AppModule.aspx" class="logout-link" title="">
                    <i class="fa fa-fw fa-th"></i>My Modules
                </a></h2>

                <a href="logout.aspx" class="logout-link" title="Logout">
                    <i class="fas fa-sign-out-alt"></i>Logout
                </a>
            </div>
        </header>
        <hr / style="color:#d2d6de">
        <!-- Marquee -->
        <marquee behavior="scroll" direction="left" scrollamount="3">
            <span style="color: red;">
                <i class="fa fa-phone"></i>Bihar School Examination Board Helpline : 0612-2230039
            </span>
            &nbsp;&nbsp;&nbsp;
            <span style="color: blue;">For any query related to Student's Registration please contact at
                <a href="mailto:bsebinterhelpdesk@gmail.com">
                    <i class="fa fa-envelope"></i>bsebinterhelpdesk@gmail.com
                </a>
            </span>
        </marquee>
       <%--  <p class="mt-4" style="color:red;font-weight:bold;font-size: 20px;margin-left: 20px;">
     नोट - पंजीयन फॉर्म भरने के पश्चात्  घोषणा पत्र अनिवार्य रूप से अपलोड करे। जिनका घोषणा पत्र अपलोड नहीं किया जायेगा उनका पंजीयन फॉर्म सबमिट नहीं माना जायेगा।
 </p>--%>
        <!-- Secondary Header (Modules + College Name) -->
        <div class="header-container">
            <div class="module-heading">Assigned Module(s)</div>
            <div class="module-heading">
    +2 School/College Code & Name:
    <br />
    <% 
        bool isAdmin = Session["CollegeName"] != null && Session["CollegeName"].ToString() == "Admin";
        if (!isAdmin)
        {
    %>
        <%: Session["CollegeCode"] != null ? Session["CollegeCode"].ToString() : "No College Code" %> |
    <% 
        }
    %>
    <%: Session["CollegeName"] != null ? Session["CollegeName"].ToString() : "No College Name" %>
</div>

        </div>

        <!-- Module Cards -->
        <div class="modules">
          <%--  <asp:LinkButton ID="lnkStudentRegistration" runat="server" OnClick="lnkStudentRegistration_Click" CssClass="module-card green">
                <div class="module-icon">
                    <img src="assets/img/registered.gif" alt="Student Registration Icon" />
                </div>
                <div class="module-title">Student Registration</div>
            </asp:LinkButton>--%>

            <asp:LinkButton ID="lnkPreExamination" runat="server" OnClick="lnkPreExamination_Click" CssClass="module-card blue">
                <div class="module-icon">
                    <img src="assets/img/question.gif" alt="Pre Exam Icon" />
                </div>
                <div class="module-title">Pre Examination</div>
            </asp:LinkButton>
        </div>
    </form>
</body>
</html>
