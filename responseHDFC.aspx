<%@ Page Language="C#" AutoEventWireup="true" CodeFile="responseHDFC.aspx.cs" Inherits="responseHDFC" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Response</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f7f9fc;
            margin: 0;
            padding: 20px;
        }

        .outerbox-cograts {
            max-width: 600px;
            margin: 0 auto;
            background-color: #fff;
            border: 1px solid #ddd;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            border-radius: 8px;
            overflow: hidden;
        }

        .header {
          
            text-align: center;
            padding: 20px;
        }

            .header img {
                width: 60px;
            }

            .header .sus {
                display: block;
                font-size: 18px;
                color: #2d7f2d;
                margin-top: 10px;
                font-weight: bold;
            }

        .outerbox-cogratsPAD {
            padding: 20px;
        }

        table.detail-table {
            width: 100%;
            border-collapse: collapse;
        }

            table.detail-table td {
                padding: 10px;
                font-size: 15px;
                border-bottom: 1px solid #eee;
            }

                table.detail-table td:first-child {
                    font-weight: bold;
                    color: #333;
                    width: 40%;
                }

        .information {
            text-align: center;
            padding: 15px 10px;
            background: #f1f1f1;
            font-size: 14px;
            margin-top: 5px;
        }

        .submitBtn {
            background-color: #0061a7;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            font-size: 15px;
            cursor: pointer;
        }

            .submitBtn:hover {
                background-color: #004b82;
            }

        @media print {
            .submitBtn {
                display: none;
            }

            #btn_back {
                display: none;
            }
        }
    </style>

    <script type="text/javascript">
        function printPage() {
            window.print();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivCongratulation" runat="server" class="outerbox-cograts">
            <div style="display: flex; justify-content: space-between; align-items: center; padding: 15px; border-bottom: 2px solid #444;">
                <!-- Left side: Board Name + Details -->
                <div>
                    <img src="assets/img/bsebimage2.png" alt="BSEB Logo" style="width: 80px; height: auto;">
                </div>
                <div style="text-align: left;">
                    <h2 style="margin: 0; font-size: 20px; font-weight: bold; color: #222;">BIHAR SCHOOL EXAMINATION BOARD
                    </h2>
                    <div style="font-size: 14px; color: #444; margin-top: 4px;">
                        INTERMEDIATE REGISTRATION SESSION (2024-26)
                    </div>
                   <%-- <div style="font-size: 13px; color: #555;">
                        FOR THE INTERMEDIATE REGISTRATION,(SESSION 2025-27)
                    </div>--%>
                </div>

                <!-- Right side: Logo -->
                
            </div>

            <div class="header" runat="server">
                <div align="center">
                    <asp:Image ID="imgSuccess" runat="server" ImageUrl="assets/img/check-mark.png" Visible="false" AlternateText="Success" />
                    <asp:Image ID="imgFailure" runat="server" ImageUrl="assets/img/remove.png" Visible="false" AlternateText="Failure" />
                </div>
                <span class="sus">
                    <asp:Label ID="lblPaymentMsg" runat="server"></asp:Label>
                </span>
                <div style="text-align: center; margin-top: 20px;">
                  <p>Receipt</p>
                </div>
            </div>
            <div class="outerbox-cogratsPAD">
                <table class="detail-table">
                    <tr>
                        <td>+2 School/College Code:</td>
                        <td>
                            <asp:Label ID="lblApplicantName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Transaction ID:</td>
                        <td>
                            <asp:Label ID="lblClientTransId" runat="server"></asp:Label></td>
                    </tr>
                     <%-- <tr>
      <td>Order ID:</td>
      <td>
          <asp:Label ID="lbl_orderid" runat="server"></asp:Label></td>
  </tr>--%>
                    <tr>
                        <td>Bank Transaction ID:</td>
                        <td>
                            <asp:Label ID="lblBankTransId" runat="server"></asp:Label></td>
                    </tr>

                    <tr>
                        <td>Amount Paid:</td>
                        <td>
                            <asp:Label ID="lbl_amountpaid" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Payment Date:</td>
                        <td>
                            <asp:Label ID="lbl_paymentdate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Status:</td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    </tr>
                </table>


                <div style="text-align: center; margin-top: 20px;">
                    <asp:Button runat="server" CssClass="submitBtn btn-primary" ID="btn_back" OnClick="btn_back_Click" Text="Back To Page"  />
                </div>

                <div style="text-align: center; margin-top: 20px;">
                    <button type="button" class="submitBtn" onclick="printPage()">Print</button>
                </div>

                <%--      <div class="information">
                    For any doubt please call <strong>Help Desk No. <span style="color: #0061a7;">0612-2230051</span></strong>
                    and refer your Reference Number.
                </div>--%>
            </div>
        </div>
        <script src="assets/js/app.min.js"></script>

        <script src="assets/bundles/sweetalert/sweetalert.min.js"></script>
        <!-- Page Specific JS File -->
        <script src="assets/js/page/sweetalert.js"></script>
    </form>

</body>

</html>