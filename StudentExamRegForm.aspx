<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="StudentExamRegForm.aspx.cs" Inherits="StudentExamRegForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .required::after {
            content: " *";
            color: red;
        }

        .text-red {
            color: red;
            font-size: 0.9rem;
        }

        #reg_subselect {
            display: none;
        }

        .preview-img {
            width: 120px;
            height: 140px;
            border: 1px solid #ccc;
            object-fit: cover;
            margin-bottom: 5px;
        }

        .signature-box {
            width: 140px;
            border: 1px solid #000;
            padding: 5px;
            text-align: center;
        }

        .signature-img {
            width: 100%;
            height: 80px;
            object-fit: contain;
        }

        table {
            width: 100%;
        }

        td {
            padding: 6px 10px;
            vertical-align: top;
        }

        .label {
            font-weight: bold;
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Student Details</h4>
                    <%-- <div class="card-header-action">

                        <a href="register_27.aspx" class="btn btn-warning">Back to List</a>
                    </div>--%>
                </div>
                <div class="card-body">

                    <div id="reg_student">

                        <div class="row">
                            <!-- Left Column -->
                            <div class="col-md-12">
                                <%-- <div class="form-group">
                                    <label>Category:</label>
                                    <div class="d-flex align-items-center mt-1" style="gap: 30px;">
                                        <div>
                                            <asp:RadioButton ID="rdoRegular" runat="server" GroupName="category" />
                                            <label for="rdoRegular" class="ml-1">Regular</label>
                                        </div>
                                        <div>
                                            <asp:RadioButton ID="rdoPrivate" runat="server" GroupName="category" />
                                            <label for="rdoPrivate" class="ml-1">Private</label>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>

                        <!-- Row 1 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="collegeName" class="form-label required">+2 School/College Name:</label>
                                    <asp:HiddenField runat="server" ID="hnd_extype" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtcollegeName" runat="server" Placeholder="College Name" class="form-control" ReadOnly="true" />
                                    <span id="txtcollegeNameErr" style="display: none; color: red;">Please Enter College</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="collegeCode" class="form-label required">+2 School/College Code:</label>
                                    <asp:TextBox ID="txtcollegeCode" runat="server" Placeholder="College Code" class="form-control" oninput="enforceMaxLength(this,6)" />
                                    <span id="txtcollegeCodeErr" style="display: none; color: red;">Please Enter College Code</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 2 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="studentName" class="form-label required">Student's Name:</label>
                                    <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name" class="form-control" />
                                    <span id="txtStudentNameErr" style="display: none; color: red;">Please Enter Student Name</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="motherName" class="form-label required">Mother's Name:</label>

                                    <asp:TextBox ID="txtmotherName" runat="server" Placeholder="Mother Name" class="form-control" />
                                    <span id="txtmotherNameErr" style="display: none; color: red;">Please Enter Mother Name</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 3 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="fatherName" class="form-label required">Father's Name:</label>

                                    <asp:TextBox ID="txtfatherName" runat="server" Placeholder="Father Name" class="form-control" />
                                    <span id="txtfatherNameErr" style="display: none; color: red;">Please Enter Father Name</span>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server" id="div_faculty">
                                <div class="mb-3 form-group">
                                    <label for="Faculty" class="form-label required">Faculty:</label>

                                    <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2" Enabled="False">
                                    </asp:DropDownList>
                                    <span id="ddlFacultyErr" style="display: none; color: red;">Please Select Faculty</span>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="dob" class="form-label required">Date of Birth:</label>
                                    <asp:TextBox ID="txtDOB" runat="server" class="form-control datepicker" TextMode="Date" max="2011-12-31" />
                                    <span id="txtDOBErr" style="display: none; color: red;">Please Enter DOB</span>
                                </div>
                            </div>
                            <%--<div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="dob" class="form-label">ApaarId :</label>
                                    <asp:TextBox ID="txtApaarId" runat="server" class="form-control" oninput="enforceMaxLength(this, 12)" />
                                </div>
                            </div>--%>


                        </div>
                        <%--  --%>
                        <!-- Row 4 -->
                        <div class="row">
                             <div class="col-md-6">
                             <div class="mb-3 form-group">
                                 <label for="district" class="form-label required">District Name:</label>

                                 <asp:TextBox ID="txtdistrict" runat="server" Placeholder="Enter District" class="form-control" />
                                 <span id="txtdistrictErr" style="display: none; color: red;">Please Enter District Name</span>
                             </div>
                         </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="subDivision" class="form-label required">Sub Division Name:</label>

                                    <asp:TextBox ID="txtsubDivision" runat="server" Placeholder="Enter sub division name" class="form-control" />
                                    <span id="txtsubDivisionErr" style="display: none; color: red;">Please Enter Sub Division</span>
                                </div>
                            </div>
                           
                        </div>

                        <!-- Row 5 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="boardName" class="form-label required">Matric/Class X Passing Board's Name:</label>

                                    <asp:DropDownList ID="ddlMatrixBoard" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="ddlMatrixBoardErr" style="display: none; color: red;">Please Select Board</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="boardRollCode" class="form-label required">Matric/Class X Board's Roll Code:</label>

                                    <asp:TextBox ID="txtboardRollCode" runat="server" Placeholder="Enter  roll code" class="form-control"  oninput="enforceMaxLength(this, 5)" />
                                    <span id="txtboardRollCodeErr" style="display: none; color: red;">Please Enter Roll Code</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 6 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="rollNumber" class="form-label required">Roll Number:</label>

                                    <asp:TextBox ID="txtrollNumber" runat="server" class="form-control" oninput="enforceMaxLength(this,9)"/>
                                    <span id="txtrollNumberErr" style="display: none; color: red;">Please Enter Roll Number</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="passingYear" class="form-label required">Passing Year:</label>

                                    <asp:TextBox ID="txtpassingYear" runat="server" class="form-control" oninput="enforceMaxLength(this,6)"/>
                                    <span id="txtpassingYearErr" style="display: none; color: red;">Please Enter Passing Year</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 7 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="gender" class="form-label required">Gender:</label>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="ddlGenderError" class="text-danger" style="display: none;">Please Select Gender.</span>

                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label for="caste" class="form-label required">Caste Category:</label>
                                    <asp:DropDownList ID="ddlCasteCategory" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="ddlCasteCategoryError" class="text-danger" style="display: none;">Please Select CastCategory.</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 8 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label">Differently Abled</label><br>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton ID="rdoAbledYes" runat="server" GroupName="differentlyAbled" />

                                        <label class="form-check-label" for="abledYes">Yes</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton ID="rdoAbledNo" runat="server" GroupName="differentlyAbled" />

                                        <label class="form-check-label" for="abledNo">No</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Nationality:</label>
                                    <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="NationalityError" class="text-danger" style="display: none;">Please select a Nationality.</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 9 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Religion:</label>
                                    <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="ReligionError" class="text-danger" style="display: none;">Please select a Religion.</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label">Area:</label>
                                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>

                                </div>
                            </div>
                        </div>

                        <!-- Row 10 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Mobile No of Student:</label>
                                    <asp:TextBox ID="txtMobile" runat="server" class="form-control"  oninput="enforceMaxLength(this, 10)" />
                                    <span id="txtMobileError" class="text-danger" style="display: none;"></span>
                                </div>
                            </div>
                            <%--<div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label">Parent/Guardian No:</label>
                                    <asp:TextBox ID="txtparentno" runat="server" class="form-control" oninput="enforceMaxLength(this, 10)" />
                                    <span id="txtparentnoError" class="text-danger" style="display: none;"></span>
                                </div>
                            </div>--%>
                            <div class="col-md-12 mb-3 form-group">
                                <div class="text-danger">
                                   <strong> अपना वैध मोबाइल नंबर ही प्रविष्ट करें। भविष्य में सभी संबंधित सूचनाएं इसी मोबाइल नंबर पर प्रदान की जाएँगी। गलत मोबाइल नंबर प्रविष्ट करने की स्थिति में जिम्मेदारी आपकी होगी। 
                                </div>
                            </div>
                        </div>

                        <!-- Row 11 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label  required">Address:</label>
                                    <asp:TextBox ID="txtAdress" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" Columns="50"></asp:TextBox>
                                      <span id="txtAdressError" class="text-danger" style="display: none;">Please select a Marital.</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">E-Mail ID:</label>
                                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" />
                                    <span id="txtEmailError" class="text-danger" style="display: none;"></span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 12 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Marital Status:</label>

                                    <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="MaritalError" class="text-danger" style="display: none;">Please select a Marital.</span>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Pin Code:</label>

                                    <asp:TextBox ID="txtpincode" runat="server" class="form-control" oninput="enforceMaxLength(this, 6)"/>
                                      <span id="txtpincodeError" class="text-danger" style="display: none;">Please Enter Pincode.</span>
                                </div>
                            </div>
                        </div>

                        <!-- Row 13 -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3 form-group">
                                    <label class="form-label">Bank & Branch Name:</label>

                                    <asp:TextBox ID="txtBranchName" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label">IFSC Code:</label>

                                    <asp:TextBox ID="txtIFSCCode" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label">Student's Bank A/C No.:</label>

                                    <asp:TextBox ID="txtBankACNo" runat="server" class="form-control" oninput="enforceMaxLength(this,25)" />

                                </div>
                            </div>
                        </div>

                        <!-- Row 14 -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Two Identification (i):</label>
                                    <asp:TextBox ID="txtIdentification1" runat="server" CssClass="form-control" />
                                    <label class="form-label required">Two Identification (ii):</label>
                                    <asp:TextBox ID="txtIdentification2" runat="server" CssClass="form-control" />
                                    <span id="IdentificationError" style="display: none; color: red;">Please enter at least one identification mark.</span>

                                </div>
                            </div>

                        </div>

                        <!-- Row 15 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3 form-group">
                                    <label class="form-label required">Medium:</label>

                                    <asp:DropDownList ID="ddlMedium" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <span id="MediumError" class="text-danger" style="display: none;">Please select a Medium.</span>
                                </div>
                            </div>
                        </div>
                        <div id="showaadhardiv" runat="server">

                          <div class="mb-3 form-group">
                            <label class="form-label"><strong>Do you have Aadhar?</strong></label><br>
                            <div class="form-check form-check-inline">
                                <asp:HiddenField ID="hfAadharOption" runat="server" ClientIDMode="Static" />
                                <asp:RadioButton  ID="aadharYes"   runat="server"  GroupName="aadhar_option" CssClass="form-check-input"  onclick="toggleAadharDivs()" />
                                <label class="form-check-label" for="<%= aadharYes.ClientID %>">Yes</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:RadioButton   ID="aadharNo"   runat="server"   GroupName="aadhar_option"   CssClass="form-check-input"  onclick="toggleAadharDivs()" />
                                <label class="form-check-label" for="<%= aadharNo.ClientID %>">No</label>
                            </div>
                        </div>

                            <!-- Div shown if Aadhar = Yes -->
                           <div id="aadharYesDiv" runat="server" style="display:none;">
                                <div class="border p-2 text-center mb-3">
                                    <strong>PLEASE MENTION "AADHAR NUMBER"</strong><br>
                                    (कृपया "आधार नंबर" अंकित करें):<br>
                                    <br>
                                    <div class="row justify-content-center align-items-center">
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtAadharNumber" runat="server" oninput="enforceMaxLength(this, 12)" placeholder="Enter Aadhar Number" class="form-control text-center" />
                                            <span id="txtAadharNumberError" class="text-danger" style="display: none;">Enter Adhar Number</span>
                                        </div>
                                        <div class="col-md-1 text-danger fs-5">*</div>
                                    </div>
                                    <div class="mt-2 text-start">
                                        <small>(If candidate has not enrolled in Aadhar and doesn’t have “Aadhar number” then he/she is required to submit declaration in column 13 that he/she has not been enrolled in Aadhar and has not got “Aadhar number”)<br>
                                            (यदि अभ्यर्थी का “आधार नंबर” आवंटित नहीं हुआ है, तो अभ्यर्थी द्वारा इस आशय की घोषणा स्तम्भ-13 में की जानी आवश्यक होगी कि उसे “आधार नंबर” आवंटित नहीं हुआ है)
                                        </small>
                                    </div>
                                </div>
                            </div>

                            <!-- Div shown if Aadhar = No -->
                          <div id="aadharNoDiv" runat="server" style="display:none;">
                                <div class="declaration-box declaration-text">
                                    <strong>If candidate has not given Aadhar number in column 12 above, then following declaration should be given by candidate:</strong><br>
                                    <br>

                                    <em>(Please note that any WRONG DECLARATION made here, may invite action against the candidate and his/her candidature may be cancelled due to making falseful declaration about non-allotment of Aadhar number)</em><br>
                                    <br>

                                    <strong>DECLARATION:</strong> I, hereby declare that I have not enrolled in Aadhar and have not got any Aadhar number. I also understand that any false declaration made by me in this regard may have consequence of cancellation of my candidature.<br>
                                    <br>

                                    <strong>यदि अभ्यर्थी के द्वारा उपर्युक्त स्तंभ-12 में “आधार नंबर” अंकित नहीं किया गया है, तो उनके द्वारा निम्नांकित घोषणा की जाएगी।</strong><br>
                                    <br>

                                    <em>(कृपया नोट करें कि यहाँ की गई किसी भी तरह की गलत घोषणा के लिए अभ्यर्थी के विरुद्ध कार्रवाई की जा सकती है तथा आधार नंबर नहीं होने के संबंध में इस मिथ्या / गलत घोषणा के कारण उनका अभ्यर्थित्व रद्द किया जा सकता है)</em><br>
                                    <br>

                                    <strong>घोषणा:</strong> मैं, एतद्द्वारा घोषित करता हूँ कि मैंने “आधार नंबर” आवंटित कराने के लिए आवेदन नहीं किया है तथा मुझे “आधार नंबर” आवंटित नहीं हुआ है। मैं यह भी समझता हूँ कि मेरे द्वारा की गई इस मिथ्या / गलत घोषणा के आधार पर मेरा अभ्यर्थित्व रद्द किया जा सकता है।
                                </div>
                               <div class="mt-3">
                                    <label for="fuAadharFile"><strong>Upload Supporting Document</strong></label>
                                    <asp:FileUpload ID="fuAadharFile" runat="server" CssClass="form-control" />
                                     <span id="txtAadharFileError" class="text-danger" style="display: none;">Upload JPG/JPEG up to 20KB</span>
                                 <%--   <small class="text-muted">Upload JPG/JPEG up to 100KB</small>--%>
                                </div>
                            </div>
                            <!-- Aadhar Instruction Block -->

                        </div>
                        <div class="col-12 text-center">
                            <asp:HiddenField ID="hfStudentId" runat="server" />
                            <asp:HiddenField ID="hfFaculty" runat="server" />
                            <asp:HiddenField ID="hfStudentIdEncrypted" runat="server" />


                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClientClick="return validateStudentDetails();" />
                            <%--<asp:Button ID="btnAddStudentReg" runat="server" CssClass="btn btn-success" Text="Next" OnClientClick="return goToNextStep();" UseSubmitBehavior="false" Style="display: none;" />--%>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
   <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

 <script type="text/javascript">

     function validateStudentDetails() {
         const examType = document.getElementById('<%= hnd_extype.ClientID %>').value;
         // District validation
         var DistrictField = document.getElementById('<%= txtdistrict.ClientID %>');
         var district = DistrictField.value.trim();
         var txtdistrictErrSpan = document.getElementById('txtdistrictErr');

         if (district === "") {
             txtdistrictErrSpan.style.display = "inline";
             txtdistrictErrSpan.textContent = "Please enter District.";
             DistrictField.classList.add("is-invalid");
             DistrictField.focus();
             return false;
         } else {
             txtdistrictErrSpan.style.display = "none";
             DistrictField.classList.remove("is-invalid");
         }

         // Division validation
         var DivisionField = document.getElementById('<%= txtsubDivision.ClientID %>');
         var division = DivisionField.value.trim();
         var txtsubDivisionErrSpan = document.getElementById('txtsubDivisionErr');

         if (division === "") {
             txtsubDivisionErrSpan.style.display = "inline";
             txtsubDivisionErrSpan.textContent = "Please enter Division.";
             DivisionField.classList.add("is-invalid");
             DivisionField.focus();
             return false;
         } else {
             txtsubDivisionErrSpan.style.display = "none";
             DivisionField.classList.remove("is-invalid");
         }


         if (examType == 2 || examType == 3 || examType == 4 || examType == 6)
         {
             var dobField = document.getElementById('<%= txtDOB.ClientID %>');
             var dob = dobField.value.trim();
             var txtDOBErrSpan = document.getElementById('txtDOBErr');

             if (dob === "") {
                 txtDOBErrSpan.style.display = "inline";
                 txtDOBErrSpan.textContent = "Please enter Date of Birth.";
                 dobField.classList.add("is-invalid");
                 dobField.focus();
                 return false;
             } else {
                 txtDOBErrSpan.style.display = "none";
                 dobField.classList.remove("is-invalid");
             }

             // ✅ Matric Board Name
                         var ddlMatrixBoard = document.getElementById('<%= ddlMatrixBoard.ClientID %>');
                         var ddlMatrixBoardErr = document.getElementById('ddlMatrixBoardErr');
                         if (ddlMatrixBoard.value === "" || ddlMatrixBoard.value === "0") {
                             ddlMatrixBoardErr.style.display = "inline";
                             ddlMatrixBoard.classList.add("is-invalid");
                             ddlMatrixBoard.focus();
                             return false;
                         } else {
                             ddlMatrixBoardErr.style.display = "none";
                             ddlMatrixBoard.classList.remove("is-invalid");
                         }

                         // ✅ Matric Roll Code
                         var rollCodeField = document.getElementById('<%= txtboardRollCode.ClientID %>');
                         var rollCodeErr = document.getElementById('txtboardRollCodeErr');
                         if (rollCodeField.value.trim() === "") {
                             rollCodeErr.style.display = "inline";
                             rollCodeErr.textContent = "Please enter Roll Code.";
                             rollCodeField.classList.add("is-invalid");
                             rollCodeField.focus();
                             return false;
                         } else {
                             rollCodeErr.style.display = "none";
                             rollCodeField.classList.remove("is-invalid");
                         }

                         // ✅ Roll Number
                         var rollNumberField = document.getElementById('<%= txtrollNumber.ClientID %>');
                         var rollNumberErr = document.getElementById('txtrollNumberErr');
                         if (rollNumberField.value.trim() === "") {
                             rollNumberErr.style.display = "inline";
                             rollNumberErr.textContent = "Please enter Roll Number.";
                             rollNumberField.classList.add("is-invalid");
                             rollNumberField.focus();
                             return false;
                         } else {
                             rollNumberErr.style.display = "none";
                             rollNumberField.classList.remove("is-invalid");
                         }

                         // ✅ Passing Year
                         var passingYearField = document.getElementById('<%= txtpassingYear.ClientID %>');
                var passingYearErr = document.getElementById('txtpassingYearErr');
                if (passingYearField.value.trim() === "") {
                    passingYearErr.style.display = "inline";
                    passingYearErr.textContent = "Please enter Passing Year.";
                    passingYearField.classList.add("is-invalid");
                    passingYearField.focus();
                    return false;
                } else {
                    passingYearErr.style.display = "none";
                    passingYearField.classList.remove("is-invalid");
                }

                // ✅ Gender
                var ddlGender = document.getElementById('<%= ddlGender.ClientID %>');
                var ddlGenderError = document.getElementById('ddlGenderError');
                if (ddlGender.value === "" || ddlGender.value === "0") {
                    ddlGenderError.style.display = "inline";
                    ddlGender.classList.add("is-invalid");
                    ddlGender.focus();
                    return false;
                } else {
                    ddlGenderError.style.display = "none";
                    ddlGender.classList.remove("is-invalid");
                }

                    // ✅ Caste Category
                    var ddlCasteCategory = document.getElementById('<%= ddlCasteCategory.ClientID %>');
                    var ddlCasteCategoryError = document.getElementById('ddlCasteCategoryError');
                    if (ddlCasteCategory.value === "" || ddlCasteCategory.value === "0") {
                        ddlCasteCategoryError.style.display = "inline";
                        ddlCasteCategory.classList.add("is-invalid");
                        ddlCasteCategory.focus();
                        return false;
                    } else {
                        ddlCasteCategoryError.style.display = "none";
                        ddlCasteCategory.classList.remove("is-invalid");
                    }

                    // ✅ Nationality
                    var ddlNationality = document.getElementById('<%= ddlNationality.ClientID %>');
                    var NationalityError = document.getElementById('NationalityError');
                    if (ddlNationality.value === "" || ddlNationality.value === "0") {
                        NationalityError.style.display = "inline";
                        ddlNationality.classList.add("is-invalid");
                        ddlNationality.focus();
                        return false;
                    } else {
                        NationalityError.style.display = "none";
                        ddlNationality.classList.remove("is-invalid");
                    }

                    // ✅ Religion
                             var ddlReligion = document.getElementById('<%= ddlReligion.ClientID %>');
                             var ReligionError = document.getElementById('ReligionError');
                             if (ddlReligion.value === "" || ddlReligion.value === "0") {
                                 ReligionError.style.display = "inline";
                                 ddlReligion.classList.add("is-invalid");
                                 ddlReligion.focus();
                                 return false;
                             } else {
                                 ReligionError.style.display = "none";
                                 ddlReligion.classList.remove("is-invalid");
                             }
         }
         // Mobile validation (as is)
         var mobileField = document.getElementById('<%= txtMobile.ClientID %>');
        var mobile = mobileField.value.trim();
        var mobileErrorSpan = document.getElementById('txtMobileError');
        var mobilePattern = /^[6-9]\d{9}$/;

        if (mobile === "") {
            mobileErrorSpan.style.display = "inline";
            mobileErrorSpan.textContent = "Please enter Mobile Number.";
            mobileField.classList.add("is-invalid");
            mobileField.focus();
            return false;
        } else if (!mobilePattern.test(mobile)) {
            mobileErrorSpan.style.display = "inline";
            mobileErrorSpan.textContent = "Please enter a valid 10-digit Mobile Number starting with 6, 7, 8, or 9.";
            mobileField.classList.add("is-invalid");
            mobileField.focus();
            return false;
        } else {
            mobileErrorSpan.style.display = "none";
            mobileField.classList.remove("is-invalid");
        }

         // Address validation
         var addressField = document.getElementById('<%= txtAdress.ClientID %>');
         var address = addressField.value.trim();
         var addressErrorSpan = document.getElementById('txtAdressError');

         if (address === "") {
             addressErrorSpan.style.display = "inline";
             addressErrorSpan.textContent = "Please enter Address.";
             addressField.classList.add("is-invalid");
             addressField.focus();
             return false;
         } else {
             addressErrorSpan.style.display = "none";
             addressField.classList.remove("is-invalid");
         }
        // Email validation (as is)
        var email = document.getElementById('<%= txtEmail.ClientID %>').value;
        var emailErrorSpan = document.getElementById('txtEmailError');
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        if (email === "") {
            emailErrorSpan.style.display = "inline";
            emailErrorSpan.textContent = "Please enter Email.";
            document.getElementById('<%= txtEmail.ClientID %>').classList.add("is-invalid");
        return false;
    } else if (!emailPattern.test(email)) {
        emailErrorSpan.style.display = "inline";
        emailErrorSpan.textContent = "Please enter a valid Email.";
        document.getElementById('<%= txtEmail.ClientID %>').classList.add("is-invalid");
        return false;
    } else {
        emailErrorSpan.style.display = "none";
        document.getElementById('<%= txtEmail.ClientID %>').classList.remove("is-invalid");
        }

         // Marital Status validation
         var ddlMaritalStatus = document.getElementById('<%= ddlMaritalStatus.ClientID %>');
         var maritalErrorSpan = document.getElementById('MaritalError');
         if (ddlMaritalStatus.value === "" || ddlMaritalStatus.value === "0") {
             maritalErrorSpan.style.display = "inline";
             ddlMaritalStatus.classList.add("is-invalid");
             ddlMaritalStatus.focus();
             return false;
         } else {
             maritalErrorSpan.style.display = "none";
             ddlMaritalStatus.classList.remove("is-invalid");
         }

         // Pin Code validation
         var pincodeField = document.getElementById('<%= txtpincode.ClientID %>');
         var pincode = pincodeField.value.trim();
         var pincodeErrorSpan = document.getElementById('txtpincodeError'); // Assuming you add this span in your HTML
         var pincodePattern = /^\d{6}$/;

         if (pincode === "") {
             pincodeErrorSpan.style.display = "inline";
             pincodeErrorSpan.textContent = "Please enter Pin Code.";
             pincodeField.classList.add("is-invalid");
             pincodeField.focus();
             return false;
         } else if (!pincodePattern.test(pincode)) {
             pincodeErrorSpan.style.display = "inline";
             pincodeErrorSpan.textContent = "Please enter a valid 6-digit Pin Code.";
             pincodeField.classList.add("is-invalid");
             pincodeField.focus();
             return false;
         } else {
             pincodeErrorSpan.style.display = "none";
             pincodeField.classList.remove("is-invalid");
         }

       

    // Identification validation
    var identification1Field = document.getElementById('<%= txtIdentification1.ClientID %>');
         var identification2Field = document.getElementById('<%= txtIdentification2.ClientID %>');
         var identificationErrorSpan = document.getElementById('IdentificationError');

         if (identification1Field.value.trim() === "" && identification2Field.value.trim() === "") {
             identificationErrorSpan.style.display = "inline";
             identification1Field.classList.add("is-invalid");
             identification2Field.classList.add("is-invalid");
             identification1Field.focus();
             return false;
         } else {
             identificationErrorSpan.style.display = "none";
             identification1Field.classList.remove("is-invalid");
             identification2Field.classList.remove("is-invalid");
         }
         // Medium validation
         var ddlMedium = document.getElementById('<%= ddlMedium.ClientID %>');
         var mediumErrorSpan = document.getElementById('MediumError');
         if (ddlMedium.value === "" || ddlMedium.value === "0") {
             mediumErrorSpan.style.display = "inline";
             ddlMedium.classList.add("is-invalid");
             ddlMedium.focus();
             return false;
         } else {
             mediumErrorSpan.style.display = "none";
             ddlMedium.classList.remove("is-invalid");
         }
        // Aadhar validation (as is)
        const aadharYes = document.getElementById('<%= aadharYes.ClientID %>');
        const aadharNo = document.getElementById('<%= aadharNo.ClientID %>');
        const AadharNumber = document.getElementById('<%= txtAadharNumber.ClientID %>');
        const txtAadharNumberError = document.getElementById('txtAadharNumberError');
        const txtAadharFileError = document.getElementById('txtAadharFileError');
        const fileUpload = document.getElementById('<%= fuAadharFile.ClientID %>');
        const aadharPattern = /^\d{12}$/;

        // clear previous errors
        txtAadharNumberError.style.display = "none";
        txtAadharFileError.style.display = "none";
        AadharNumber.classList.remove("is-invalid");

        if (!aadharYes.checked && !aadharNo.checked) {
            swal("Error", "Please select whether you have Aadhar.", "error");
            return false;
        }

        let aadharValue = "";
        let aadharFileName = "";
        let aadharFileExtension = "";

        // ✅ YES case
        if (aadharYes.checked) {
            aadharValue = AadharNumber.value.trim();
            if (aadharValue === "") {
                txtAadharNumberError.style.display = "inline";
                txtAadharNumberError.textContent = "Aadhar number is required.";
                AadharNumber.classList.add("is-invalid");
                AadharNumber.focus();
                return false;
            } else if (!aadharPattern.test(aadharValue)) {
                txtAadharNumberError.style.display = "inline";
                txtAadharNumberError.textContent = "Please enter a valid 12-digit Aadhar Number.";
                AadharNumber.classList.add("is-invalid");
                AadharNumber.focus();
                return false;
            } else {
                txtAadharNumberError.style.display = "none";
                AadharNumber.classList.remove("is-invalid");
            }
        }

        // ✅ NO case
        if (aadharNo.checked) {
            if (!fileUpload || fileUpload.files.length === 0) {
                txtAadharFileError.style.display = "inline";
                txtAadharFileError.textContent = "Please upload supporting Aadhar document.";
                fileUpload.focus();
                return false;
            }
            const file = fileUpload.files[0];
            if (file.size > 20480) { // 20 KB
                txtAadharFileError.style.display = "inline";
                txtAadharFileError.textContent = "File size must be ≤ 20 KB.";
                return false;
            }
            const ext = file.name.split('.').pop().toLowerCase();
            if (ext !== 'jpg' && ext !== 'jpeg' &&  ext !== 'png') {
                txtAadharFileError.style.display = "inline";
                txtAadharFileError.textContent = "Only JPG/JPEG/PNG files allowed.";
                return false;
            }
            aadharFileName = fileUpload.files[0].name;
            aadharFileExtension = fileUpload.files[0].name.split('.').pop().toLowerCase();
        }

         if (examType == 1 || examType == 5)
         {
             // Call the server method with individual arguments
             PageMethods.UpdateStudent(
                 document.getElementById('<%= hfStudentId.ClientID %>').value,
        document.getElementById('<%= hfFaculty.ClientID %>').value,
        mobile,
        email,

        document.getElementById('<%= txtAdress.ClientID %>').value,
        document.getElementById('<%= ddlMaritalStatus.ClientID %>').value,
        document.getElementById('<%= txtpincode.ClientID %>').value,
        document.getElementById('<%= txtBranchName.ClientID %>').value,
        document.getElementById('<%= txtIFSCCode.ClientID %>').value,
        document.getElementById('<%= txtBankACNo.ClientID %>').value,
        document.getElementById('<%= txtIdentification1.ClientID %>').value,
        document.getElementById('<%= txtIdentification2.ClientID %>').value,
    document.getElementById('<%= ddlMedium.ClientID %>').value,
    document.getElementById('<%= hnd_extype.ClientID %>').value,
    document.getElementById('<%= txtcollegeCode.ClientID %>').value,
    aadharValue,
    aadharFileName,
    aadharFileExtension,
        document.getElementById('<%= txtdistrict.ClientID %>').value,
        document.getElementById('<%= txtsubDivision.ClientID %>').value,
        function (response) {
            if (response.status === "success") {
                debugger
                swal("Success", response.message, "success").then(() => {
                    // Get the values from the form fields directly
            <%--var studentId = document.getElementById('<%= hfStudentId.ClientID %>').value;--%>
                    var studentIdEncrypted = document.getElementById('<%= hfStudentIdEncrypted.ClientID %>').value;
            var facultyId = document.getElementById('<%= hfFaculty.ClientID %>').value;
            var examTypeId = document.getElementById('<%= hnd_extype.ClientID %>').value;
                    var collegeCode = document.getElementById('<%= txtcollegeCode.ClientID %>').value;
                    var StudentExamRegForm = "StudentExamRegForm";
                    var url = "EditExamStudentSubjectgrps.aspx?studentId=" + encodeURIComponent(studentIdEncrypted) +
                        "&FacultyId=" + encodeURIComponent(facultyId) +
                        "&ExamTypeId=" + encodeURIComponent(examTypeId) +
                        "&collegeCode=" + encodeURIComponent(collegeCode) + "&StudentExamRegForm=StudentExamRegForm";
                    window.location.href = url;
                    //var url = "ExamStudentSubjectgrps.aspx?studentId=" + encodeURIComponent(studentIdEncrypted) +
                    //    "&FacultyId=" + encodeURIComponent(facultyId) +
                    //    "&ExamTypeId=" + encodeURIComponent(examTypeId) +
                    //    "&collegeCode=" + encodeURIComponent(collegeCode) + "&StudentExamRegForm=StudentExamRegForm";
                    //window.location.href = url;
                });
            } else {
                swal("Error", response.message, "error");
            }
        },
        function (err) {
            swal("Error", "Server error: " + err.get_message(), "error");
        }
    );
         }
         else {
             // Call the server method with individual arguments
             PageMethods.UpdateStudentDetails(
                 document.getElementById('<%= hfStudentId.ClientID %>').value,
                 document.getElementById('<%= hfFaculty.ClientID %>').value,
                 mobile,
                 email,
                 document.getElementById('<%= txtAdress.ClientID %>').value,
                 document.getElementById('<%= ddlMaritalStatus.ClientID %>').value,
                 document.getElementById('<%= txtpincode.ClientID %>').value,
                 document.getElementById('<%= txtBranchName.ClientID %>').value,
                 document.getElementById('<%= txtIFSCCode.ClientID %>').value,
                 document.getElementById('<%= txtBankACNo.ClientID %>').value,
                 document.getElementById('<%= txtIdentification1.ClientID %>').value,
                 document.getElementById('<%= txtIdentification2.ClientID %>').value,
                 document.getElementById('<%= ddlMedium.ClientID %>').value,
                 document.getElementById('<%= hnd_extype.ClientID %>').value,
                 document.getElementById('<%= txtcollegeCode.ClientID %>').value,
                 aadharValue,
                 aadharFileName,
                 aadharFileExtension,
                 document.getElementById('<%= txtdistrict.ClientID %>').value,
                document.getElementById('<%= txtsubDivision.ClientID %>').value,
                document.getElementById('<%= ddlMatrixBoard.ClientID %>').value,       
                document.getElementById('<%= txtboardRollCode.ClientID %>').value,     
                document.getElementById('<%= txtrollNumber.ClientID %>').value,         
                document.getElementById('<%= txtpassingYear.ClientID %>').value,        
                document.getElementById('<%= ddlGender.ClientID %>').value,             
                document.getElementById('<%= ddlCasteCategory.ClientID %>').value,      
                document.getElementById('<%= ddlNationality.ClientID %>').value,        
                document.getElementById('<%= ddlReligion.ClientID %>').value,           
                document.getElementById('<%= txtDOB.ClientID %>').value,           
    function (response) {
        if (response.status === "success") {
            swal("Success", response.message, "success").then(() => {
               <%--var studentId = document.getElementById('<%= hfStudentId.ClientID %>').value;--%>
                var studentIdEncrypted = document.getElementById('<%= hfStudentIdEncrypted.ClientID %>').value;
                var facultyId = document.getElementById('<%= hfFaculty.ClientID %>').value;
                var examTypeId = document.getElementById('<%= hnd_extype.ClientID %>').value;
                var collegeCode = document.getElementById('<%= txtcollegeCode.ClientID %>').value;
                var StudentExamRegForm = "StudentExamRegForm";

                var url = "EditExamStudentSubjectgrps.aspx?studentId=" + encodeURIComponent(studentIdEncrypted) +
                    "&FacultyId=" + encodeURIComponent(facultyId) +
                    "&ExamTypeId=" + encodeURIComponent(examTypeId) +
                    "&collegeCode=" + encodeURIComponent(collegeCode) + "&StudentExamRegForm=StudentExamRegForm";
                window.location.href = url;
                //var url = "ExamStudentSubjectgrps.aspx?studentId=" + encodeURIComponent(studentIdEncrypted) +
                //    "&FacultyId=" + encodeURIComponent(facultyId) +
                //    "&ExamTypeId=" + encodeURIComponent(examTypeId) +
                //    "&collegeCode=" + encodeURIComponent(collegeCode) + "&StudentExamRegForm=StudentExamRegForm";
                //window.location.href = url;
            });
                     } else {
                         swal("Error", response.message, "error");
                     }
                 },
                 function (err) {
                     swal("Error", "Server error: " + err.get_message(), "error");
                 }
             );

         }

        return false; // Prevent default form submission
    }

     // Handle Aadhar div visibility on page load
     window.onload = function () {
         debugger;
         const examType = document.getElementById('<%= hnd_extype.ClientID %>').value;
         const lockedFields = [
            '<%= txtStudentName.ClientID %>',
            '<%= txtfatherName.ClientID %>',
            '<%= txtmotherName.ClientID %>',
           <%-- '<%= txtDOB.ClientID %>',--%>
            '<%= ddlFaculty.ClientID %>',
            '<%= txtcollegeCode.ClientID %>'
            // add more if needed:
         ];

         // ✅ Fields to enable when examType = 1 or 5
         const regularPrivateFields = [
            '<%= txtMobile.ClientID %>',
            '<%= txtAdress.ClientID %>',
            '<%= txtEmail.ClientID %>',
            '<%= ddlMaritalStatus.ClientID %>',
            '<%= txtpincode.ClientID %>',
            '<%= txtBranchName.ClientID %>',
            '<%= txtIFSCCode.ClientID %>',
            '<%= txtBankACNo.ClientID %>',
            '<%= txtIdentification1.ClientID %>',
            '<%= txtIdentification2.ClientID %>',
            '<%= ddlMedium.ClientID %>',
            '<%= showaadhardiv.ClientID %>',
            '<%= aadharYes.ClientID %>',
            '<%= aadharNo.ClientID %>',
            '<%= txtAadharNumber.ClientID %>',
            '<%= aadharNoDiv.ClientID %>',
            '<%= fuAadharFile.ClientID %>',
            '<%= txtdistrict.ClientID %>',
            '<%= txtsubDivision.ClientID %>',
          
        ];

        // ✅ Collect all inputs, selects, textareas, divs
        const allFields = document.querySelectorAll('input, select, textarea, div');

        // --- CASE 1: ExamType = 1 or 5 (current logic) ---
        if (examType === "1" || examType === "5") {
            const enableSet = new Set(regularPrivateFields);
            allFields.forEach(function (field) {
                if (field.id === '<%= btnUpdate.ClientID %>') return; // skip update button
                field.disabled = !enableSet.has(field.id);
            });
        }

        // --- CASE 2: ExamType = 2,3,4,6 (open all except locked fields) ---
         if (["2", "3", "4", "6"].includes(examType)) {
             allFields.forEach(function (field) {
                 if (field.id === '<%= btnUpdate.ClientID %>') return; // skip update button
                 if (lockedFields.includes(field.id) && field.id !== '<%= txtDOB.ClientID %>')
                 {
            field.disabled = true;
            } else {
            field.disabled = false;
        }
    });
         }


        //const enableFieldIds = examType === "1" || examType === "5" ;
        //const enableSet = new Set(enableFieldIds);
        //const allFields = document.querySelectorAll('input, select, textarea, button, div');

        //allFields.forEach(function(field) {
        //    if (field.id === 'btnUpdate') return;
        //    field.disabled = !enableSet.has(field.id);
        //});

        const updateBtn = document.getElementById('<%= btnUpdate.ClientID %>');
        if (updateBtn) {
            updateBtn.disabled = false;
        }

       <%-- if (examType == "3") {
            const NextBtn = document.getElementById('<%= btnAddStudentReg.ClientID %>');
            if (NextBtn) {
                NextBtn.style.display = "inline-block";
                NextBtn.disabled = false;
            }
        }--%>

         // Handle Aadhar options and toggle visibility of sections
         const yesRadio = document.getElementById('<%= aadharYes.ClientID %>');
         const noRadio = document.getElementById('<%= aadharNo.ClientID %>');
    const aadharNumberField = document.getElementById('<%= txtAadharNumber.ClientID %>');
         const hfAadharOption = document.getElementById('<%= hfAadharOption.ClientID %>');

         if (yesRadio && noRadio && hfAadharOption) {
             // ✅ Always default to Yes, no matter what value is in txtAadharNumber
             yesRadio.checked = true;
             noRadio.checked = false;
             hfAadharOption.value = "Yes";

             // Show correct divs
             toggleAadharDivs();

             // Handle changes
             yesRadio.addEventListener('change', function () {
                 hfAadharOption.value = "Yes";
                 toggleAadharDivs();
             });

             noRadio.addEventListener('change', function () {
                 hfAadharOption.value = "No";
                 toggleAadharDivs();
             });
         }

         //if (yesRadio && noRadio && hfAadharOption) {
         //    if (aadharNumberField && aadharNumberField.value.trim() !== "") {
         //        yesRadio.checked = true;
         //        hfAadharOption.value = "Yes";
         //    } else {
         //        noRadio.checked = true;
         //        hfAadharOption.value = "No";
         //    }

         //    // Correctly call the function on load to set initial state
         //    toggleAadharDivs();

         //    yesRadio.addEventListener('change', function () {
         //        hfAadharOption.value = "Yes";
         //        toggleAadharDivs();
         //    });

         //    noRadio.addEventListener('change', function () {
         //        hfAadharOption.value = "No";
         //        toggleAadharDivs();
         //    });
         //}
    };

    // Corrected toggleAadharDivs function
     function toggleAadharDivs() {
         const yesRadio = document.getElementById('<%= aadharYes.ClientID %>');
      const noRadio = document.getElementById('<%= aadharNo.ClientID %>');
      const yesDiv = document.getElementById('<%= aadharYesDiv.ClientID %>');
    const noDiv = document.getElementById('<%= aadharNoDiv.ClientID %>');

    if (!yesDiv || !noDiv) {
        console.warn('Aadhar sections not found.');
        return;
    }

    if (yesRadio && yesRadio.checked) {
        yesDiv.style.display = 'block';
        noDiv.style.display = 'none';
        document.getElementById('<%= hfAadharOption.ClientID %>').value = 'Yes';
    } else if (noRadio && noRadio.checked) {
        yesDiv.style.display = 'none';
        noDiv.style.display = 'block';
             document.getElementById('<%= hfAadharOption.ClientID %>').value = 'No';
         }
     }


     // Ensure the correct state on initial page load
     window.addEventListener('load', toggleAadharDivs);


    function goToNextStep() {
        var studentId = document.getElementById('<%= hfStudentId.ClientID %>').value;
        var facultyId = document.getElementById('<%= hfFaculty.ClientID %>').value;
        var collegeCode = document.getElementById('<%= txtcollegeCode.ClientID %>').value;
        var examTypeId = document.getElementById('<%= hnd_extype.ClientID %>').value;

         PageMethods.GoToNextStep(studentId, facultyId, collegeCode, examTypeId,
             function (response) {
                 if (response.status === "success") {
                     swal("Success", response.message, "success").then(() => {
                         var url = "ExamStudentSubjectgrps.aspx?studentId=" + encodeURIComponent(studentId) +
                             "&FacultyId=" + encodeURIComponent(facultyId) +
                             "&ExamTypeId=" + encodeURIComponent(examTypeId) +
                             "&collegeCode=" + encodeURIComponent(collegeCode);
                         window.location.href = url;
                     });
                 } else {
                     swal("Error", response.message, "error");
                 }
             },
             function (error) {
                 swal("Error", "Server error: " + error.get_message(), "error");
             }
         );

         return false; // Prevent form submission
     }

     // Enforce max length for fields
     function enforceMaxLength(el, maxLength) {
         // Remove non-numeric characters anywhere in the value
         el.value = el.value.replace(/[^0-9]/g, '');
         // Limit length
         if (el.value.length > maxLength) {
             el.value = el.value.slice(0, maxLength);
         }
     }
     //function enforceMaxLength(el, maxLength) {
     //    if (el.value.length > maxLength) {
     //        el.value = el.value.slice(0, maxLength);
     //    }
     //}

 </script>

</asp:Content>
