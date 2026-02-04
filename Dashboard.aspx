<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        /* Custom Pills Styling */
        .nav-pills .nav-link {
            border-radius: 12px;
            padding: 10px 20px;
            margin: 0 5px;
            font-weight: 500;
            color: #555;
            border: 1px solid #ddd;
            transition: all 0.3s ease-in-out;
        }

            .nav-pills .nav-link.active {
                background-color: #4f46e5;
                color: #fff;
                border-color: #4f46e5;
                box-shadow: 0 4px 12px rgba(79, 70, 229, 0.3);
            }

            .nav-pills .nav-link:hover {
                background-color: #f0f0ff;
                border-color: #4f46e5;
                color: #4f46e5;
            }

        .tab-content {
            margin-top: 20px;
            padding: 20px;
            border-radius: 12px;
            background: #fff;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.08);
        }

        .card-soft-green {
            background-color: #eafee4 !important;
        }

        .card-soft-blue {
            background-color: #caddf2 !important;
        }

        .card-soft-yellow {
            background-color: #fff7d3 !important;
        }
           .card-soft-2 {
            background-color: #f8d9ff !important;
        }
        .card-title {
            font-weight: bold;
            font-size: 1.2rem;
            margin-bottom: .7rem;
        }

        .stat-number {
            font-weight: bold;
            float: right;
        }

        .card {
            border-radius: 10px;
            padding: 1.1rem 1.7rem;
            min-height: 170px;
        }

        .mb-md-0 {
            margin-bottom: 0 !important;
        }

        .bg-orange {
            background-color: #fff7ec !important;
            border-left: 6px solid #E67E22;
        }
.text-2{
    color:#8f00b0;
}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
        <!-- Pills Navigation -->
        <ul class="nav nav-pills justify-content-start mb-3" id="pills-tab" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home" type="button" role="tab">
                    Registration Overview

                </button>
            </li>
          <%--  <li class="nav-item" role="presentation">
                <button class="nav-link" id="pills-profile-tab" data-bs-toggle="pill" data-bs-target="#pills-profile" type="button" role="tab">
                    Payment Summary

                </button>
            </li>--%>

        </ul>

        <!-- Pills Content -->
        <div class="tab-content font-18" id="pills-tabContent">
            <div class="tab-pane fade show active" id="pills-home" role="tabpanel">


                <div class="text-center mb-3">
                    <span style="color: #E74C3C;font-size: 1.3rem;font-weight: bold;text-decoration-line: underline;">+2 विद्यालय/शिक्षण संस्थान के प्रधान कृपया ध्यान दें :-
                    </span>
                </div>
                 <div class="form-group">
                     <label runat="server" id="lblpanel1college" visible="false">+2 School/College Code<span class="text-danger">*</span></label>
                     <asp:TextBox ID="txt_Panel1CollegeName" runat="server" CssClass="form-control"
                         placeholder="Enter CollegeCode" Visible="false">
                     </asp:TextBox>
                     <span id="Panel1CollegeNameError" class="text-danger" style="display: none;">Please Enter College.</span>

                 </div>
                 <div class="form-group mt-4 text-center">
                     <asp:Button ID="btngetCount" runat="server" Text="Get Summary" CssClass="btn btn-primary" OnClick="btngetCount_Click" Visible="false" OnClientClick="return validateCollege();" />
                 

                 </div>
                <div class="mb-4">
                    <p style="color: black;">
                        आपके +2 विद्यालय/शिक्षण संस्थान द्वारा कुल
            <%--<span style="color: #007bff; font-weight: bold;">12 (बारह)</span>विद्यार्थियों का पंजीयन (रजिस्ट्रेशन) की प्रक्रिया प्रारम्भ की गई है किंतु--%> 
            <span style="color: #007bff; font-weight: bold;"><asp:Label ID="lblPaymentStatusCount" runat="server" /></span>विद्यार्थियों का पंजीयन शुल्क (रजिस्ट्रेशन पेमेंट ) सफलतापूर्वक जमा किया गया है जिसमे मात्र
            <span style="color: #27AE60; font-weight: bold;"><asp:Label ID="lblDeclarationFormSubmittedCount" runat="server" /></span>विद्यार्थियों का ही सफलतापूर्वक पंजीयन कराया गया है जिसकी विवरणी तालिका - 2 (Table - 2) में अंकित प्रक्रिया अनुसार देखी जा सकती है।
            <%--<span style="color: #27AE60; font-weight: bold;">2 (दो)</span>विद्यार्थियों का ही सफलतापूर्वक पंजीयन कराया गया है जिसकी विवरणी तालिका - 2 (Table - 2) में अंकित प्रक्रिया अनुसार देखी जा सकती है।--%>
                    </p>
                    <p style="color: black;">
                       आपके +2  विद्यालय/शिक्षण संस्थान के
            <span style="color: #E67E22; font-weight: bold;"><asp:Label ID="lblDeclarationNotSubmittedCount" runat="server" /></span>
                        विधार्थियों का पंजीयन पूर्ण नहीं हुआ है क्योंकि विद्यार्थी, उनके माता/पिता तथा प्रधानाध्यापक द्वारा हस्ताक्षरयुक्त घोषणा पत्र अपलोड नहीं किया गया है,  इन विद्यार्थियों की सूची एवं पंजीयन प्रक्रिया की स्थिति तालिका - 1 (Table - 1) में दी गई है। अतः निम्नलिखित प्रक्रिया अनुसार हस्ताक्षरित घोषणा पत्र अपलोड कर सुगमता पूर्वक शेष बचे
            <span style="color: #E67E22; font-weight: bold;"><asp:Label ID="lblDeclarationNotSubmittedCount2" runat="server" /></span>
                        विद्यार्थियों का पंजीयन पूर्ण किया जा सकता है।
                    </p>
                </div>

                <div class="row">
                    <!-- Table 1 Card -->
                    <div class="col-md-6 mb-3">
                        <div class="card  h-100" style="border: 1px solid;">
                            <div class="card-header text-center">
                                <b style="color: orangered">तालिका - 1 (Table-1)
                                </b> 
                            </div>
                            <div class="card-body">
                                <p class="text-bold" style="color: orangered">
                                    आपके +2 विद्यालय/शिक्षण संस्थान के 
                        <span style="color: #E67E22; font-weight: bold;"><asp:Label ID="lblDeclarationNotSubmittedCount3" runat="server" /> </span>
                                    विद्यार्थियों, उनके माता/पिता तथा प्रधानाचार्य द्वारा हस्ताक्षरयुक्त घोषणा पत्र अपलोड नहीं किये जाने के कारण इनका पंजीयन (रजिस्ट्रेशन) पूर्ण नहीं हो सका है। इसे निम्नांकित प्रक्रिया के अनुसार पूर्ण करें :-
                                </p>
                                <br />

                                <p>
                                    मेनू में <b>Declaration Upload</b> पर क्लिक करने के उपरांत सूचीबद्ध छात्र/छात्रा के समक्ष Upload विकल्प पर क्लिक करें। आवश्यक निर्देश देखकर फॉर्म डाउनलोड कर सकते हैं।
                                </p>
                                <div class="text-center mb-3">
                                    <a href="ManageDeclarationForm.aspx" class="btn btn-orange" style="background-color: orangered; color: white">VIEW PENDING DECLARATION FORM LIST</a>
                                </div>
                                <div class="font-weight-bold mb-2">घोषणा पत्र देने की प्रक्रिया पूर्ण करने हेतु Steps :-</div>
                                <ol>
                                    <li>
                                       
                                        <b>Declaration Upload List</b> में दिए गए <b>Download Declaration Form</b> ऑप्शन का उपयोग करके संबंधित छात्र/छात्रा का Declaration Form डाउनलोड करें।
                                    </li>
                                    <li>
                                       
                                        Declaration Form पर छात्र/छात्रा, माता/पिता तथा प्रधानाचार्य के हस्ताक्षर प्राप्त करें एवं फॉर्म स्कैन कर लें।
                                    </li>
                                    <li> 
                                        स्कैन किए गए Declaration Form को Upload List में Upload करें, जिसके पश्चात Declaration Form सफलतापूर्वक अपलोड किया जा सकता है।
                                    </li>
                                </ol>
                            </div>
                        </div>
                    </div>
                    <!-- Table 2 Card -->
                    <div class="col-md-6 mb-3">
                        <div class="card h-100" style="border: 1px solid;">
                            <div class="card-header text-center">
                                <b style="color: #0e9a0a9c">तालिका - 2 (Table-2)
                                </b>
                            </div>
                            <div class="card-body">
                                <p style="color: #0e9a0a9c">
                                    आपके +2 विद्यालय/शिक्षण संस्थान के 
                        <span style="color: #27AE60; font-weight: bold;"><asp:Label ID="lblDeclarationFormSubmittedCount1" runat="server" /> </span>
                                    विद्यार्थियों का सफलतापूर्वक किया गया पंजीयन नियमांकित प्रक्रिया के अनुसार देखा जा सकता है :-
                                </p>
                                <br />
                                   
                                <p>
                                    मेनू में <b>Registered Student List</b> पर क्लिक कर के सफलतापूर्वक पंजीकृत किए गए छात्र/छात्राओं की सूची एवं उनकी विवरणी को देखा जा सकता है, अथवा इस बटन के माध्यम से भी विवरणी देखी जा सकती है।
                                </p>
                                <div class="text-center mb-3">
                                    <a href="StudentRegisteredList.aspx" class="btn btn-green" style="background-color: #0e9a0a9c; color: white">VIEW REGISTRATION COMPLETED STUDENT DETAILS</a>
                                </div>
                                <div class="font-weight-bold mb-2">पंजीकृत छात्र / छात्रा की विवरणी की अवलोकन कर पंजीयन विवरणी डाउनलोड करने के Steps :-</div>
                                <ol>
                                    <li>
                                       
                                        <b>Registered Student List</b> Menu के सामने दिए गए <b>View</b> बटन का उपयोग करके संबंधित छात्र/छात्रा की पूरी विवरणी देखी जा सकती है।
                                    </li>
                                    <li>
                                       
                                        <b>Print</b> बटन का उपयोग करके विवरणी फाइल/छात्र/छात्रा का पंजीकरण की पूरी विवरणी डाउनलोड की जा सकती है।
                                    </li>
                                </ol>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
  

    <!-- Bootstrap JS + Icons -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <script type="text/javascript">
        (function () {
            function saveActive(target) {
                try { localStorage.setItem('activeTab', target); } catch (e) { }
                if (history.replaceState) {
                    history.replaceState(null, '', location.pathname + location.search + target);
                }
            }

            function activateTab(target) {
                if (!target) return;

                var trigger = document.querySelector('#pills-tab [data-bs-target="' + target + '"]');
                var pane = document.querySelector(target);

                if (!trigger || !pane) return;

                // Deactivate all
                document.querySelectorAll('#pills-tab .nav-link').forEach(function (el) {
                    el.classList.remove('active');
                    el.setAttribute('aria-selected', 'false');
                });
                document.querySelectorAll('#pills-tabContent .tab-pane').forEach(function (el) {
                    el.classList.remove('show', 'active');
                });

                // Activate selected (manual)
                trigger.classList.add('active');
                trigger.setAttribute('aria-selected', 'true');
                pane.classList.add('show', 'active');

                // Also use Bootstrap API if available (keeps internal state in sync)
                if (window.bootstrap && bootstrap.Tab) {
                    new bootstrap.Tab(trigger).show();
                }
            }

            function restoreAndWire() {
                // Wire saving on tab change
                document.querySelectorAll('#pills-tab [data-bs-toggle="pill"]').forEach(function (btn) {
                    // Fires when Bootstrap actually shows a tab
                    btn.addEventListener('shown.bs.tab', function (e) {
                        var t = e.target.getAttribute('data-bs-target');
                        saveActive(t);
                    });
                    // Fallback if shown.bs.tab didn't fire for any reason
                    btn.addEventListener('click', function () {
                        var t = this.getAttribute('data-bs-target');
                        saveActive(t);
                    });
                });

                // Prefer hash (e.g., #pills-profile), else localStorage
                var target =
                    (location.hash && document.querySelector(location.hash) ? location.hash : null) ||
                    localStorage.getItem('activeTab');

                if (target) activateTab(target);
            }

            // Initial load
            document.addEventListener('DOMContentLoaded', restoreAndWire);

            // Support ASP.NET UpdatePanel partial postbacks (if used)
            if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    restoreAndWire();
                    var t = (location.hash && document.querySelector(location.hash) ? location.hash : null) ||
                        localStorage.getItem('activeTab');
                    if (t) activateTab(t);
                });
            }
        })();
    </script>

    <script type="text/javascript">
        // Attach keydown listener once page loads
      <%--  document.addEventListener("DOMContentLoaded", function () {
            var txtCollege = document.getElementById("<%= txt_CollegeName.ClientID %>");
            if (txtCollege) {
                txtCollege.addEventListener("keydown", function (event) {
                    if (event.key === "Enter") {
                        event.preventDefault(); // stop default submit
                        document.getElementById("<%= btngetsummary.ClientID %>").click();
                    }
                });
            }
        });
        function validateFaculty() {

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

           
        }--%>
        function validateCollege()
        {

            var Panel1collegeNameInput = document.getElementById('<%= txt_Panel1CollegeName.ClientID %>');
            var Panel1collegeNameErrorSpan = document.getElementById('Panel1CollegeNameError');
            if (Panel1collegeNameInput.value.trim() === "") {
                Panel1collegeNameErrorSpan.style.display = "inline";
                Panel1collegeNameInput.classList.add("is-invalid");
                Panel1collegeNameInput.focus();
                return false;
            } else {
                Panel1collegeNameErrorSpan.style.display = "none";
                Panel1collegeNameInput.classList.remove("is-invalid");
            }
        }
    </script>



</asp:Content>

