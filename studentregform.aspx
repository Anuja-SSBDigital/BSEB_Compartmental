<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="studentregform.aspx.cs" Inherits="studentregform" %>

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
                                <div class="mb-3">
                                    <div class="form-group">
                                        <lable class="form-label required">
                                            <strong>Category:

                                            </strong>
                                        </lable>
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
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 1 -->
                        <div class="row">
                            <div class="col-md-6">

                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">+2 School/College Name:</label>
                                        <asp:TextBox ID="txtcollegeName" runat="server" Placeholder="College Name" class="form-control" />
                                        <span id="txtcollegeNameErr" style="display: none; color: red;">Please Enter College</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="collegeCode" class="form-label required">+2 School/College Code:</label>
                                        <asp:TextBox ID="txtcollegeCode" runat="server" Placeholder="College Code" class="form-control" />
                                        <span id="txtcollegeCodeErr" style="display: none; color: red;">Please Enter College Code</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 2 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="studentName" class="form-label required">Student's Name:</label>
                                        <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name" ReadOnly="true" class="form-control" />
                                        <span id="txtStudentNameErr" style="display: none; color: red;">Please Enter Student Name</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="motherName" class="form-label required">Mother's Name:</label>

                                        <asp:TextBox ID="txtmotherName" runat="server" Placeholder="Mother Name" ReadOnly="true" class="form-control" />
                                        <span id="txtmotherNameErr" style="display: none; color: red;">Please Enter Mother Name</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 3 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="fatherName" class="form-label required">Father's Name:</label>

                                        <asp:TextBox ID="txtfatherName" runat="server" Placeholder="Father Name" ReadOnly="true" class="form-control" />
                                        <span id="txtfatherNameErr" style="display: none; color: red;">Please Enter Father Name</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" runat="server" id="div_faculty">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="Faculty" class="form-label required">Faculty:</label>

                                        <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2" Enabled="False">
                                        </asp:DropDownList>
                                        <span id="ddlFacultyErr" style="display: none; color: red;">Please Select Faculty</span>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="dob" class="form-label required">Date of Birth:</label>
                                        <asp:TextBox ID="txtDOB" runat="server" class="form-control datepicker" TextMode="Date" max="2011-12-31" />
                                        <span id="txtDOBErr" style="display: none; color: red;">Please Enter DOB</span>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <!-- Row 4 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="district" class="form-label required">District Name:</label>

                                        <asp:TextBox ID="txtdistrict" runat="server" Placeholder="Enter District" class="form-control" />
                                        <span id="txtdistrictErr" style="display: none; color: red;">Please Enter District Name</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="subDivision" class="form-label required">Sub Division Name:</label>

                                        <asp:TextBox ID="txtsubDivision" runat="server" Placeholder="Enter sub division name" class="form-control" />
                                        <span id="txtsubDivisionErr" style="display: none; color: red;">Please Enter Sub Division</span>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <!-- Row 5 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="boardName" class="form-label">Matric/Class X Passing Board's Name:</label>

                                        <asp:DropDownList ID="ddlMatrixBoard" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="ddlMatrixBoardErr" style="display: none; color: red;">Please Select Board</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="boardRollCode" class="form-label">Matric/Class X Board's Roll Code:</label>

                                        <asp:TextBox ID="txtboardRollCode" runat="server" Placeholder="Enter  roll code" class="form-control" TextMode="Number" oninput="enforceMaxLength(this, 5)" />
                                        <span id="txtboardRollCodeErr" style="display: none; color: red;">Please Enter Roll Code</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 6 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">

                                        <label for="rollNumber" class="form-label">Roll Number:</label>

                                        <asp:TextBox ID="txtrollNumber" runat="server" class="form-control" TextMode="Number" />
                                        <span id="txtrollNumberErr" style="display: none; color: red;">Please Enter Roll Number</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="passingYear" class="form-label required">Passing Year:</label>

                                        <asp:TextBox ID="txtpassingYear" runat="server" class="form-control" />
                                        <span id="txtpassingYearErr" style="display: none; color: red;">Please Enter Passing Year</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 7 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="gender" class="form-label required">Gender:</label>
                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="ddlGenderError" class="text-danger" style="display: none;">Please Select Gender.</span>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="caste" class="form-label required">Caste Category:</label>
                                        <asp:DropDownList ID="ddlCasteCategory" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="ddlCasteCategoryError" class="text-danger" style="display: none;">Please Select CastCategory.</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 8 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
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
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Nationality:</label>
                                        <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="NationalityError" class="text-danger" style="display: none;">Please select a Nationality.</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 9 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Religion:</label>
                                        <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="ReligionError" class="text-danger" style="display: none;">Please select a Religion.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">Area:</label>
                                        <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 10 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Mobile No of Student:</label>
                                        <asp:TextBox ID="txtMobile" runat="server" class="form-control" TextMode="Number" oninput="enforceMaxLength(this, 10)" />
                                        <span id="txtMobileError" class="text-danger" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">BSEB Unique Id:</label>
                                        <asp:TextBox ID="txtuniqueid" runat="server" class="form-control" TextMode="Number" />
                                        <span id="txtparentnoError" class="text-danger" style="display: none;"></span>
                                        <%-- <asp:TextBox ID="txtparentno" runat="server" class="form-control" TextMode="Number" oninput="enforceMaxLength(this, 10)" />
                                        <span id="txtparentnoError" class="text-danger" style="display: none;"></span>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 mb-3">
                                <div class="text-danger">
                                    <strong>अपना वैध मोबाइल नंबर ही प्रविष्ट करें। भविष्य में सभी संबंधित सूचनाएं इसी मोबाइल नंबर पर प्रदान की जाएँगी। गलत मोबाइल नंबर प्रविष्ट करने की स्थिति में जिम्मेदारी आपकी होगी।</strong>
                                </div>
                            </div>
                        </div>

                        <!-- Row 11 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">Address:</label>
                                        <asp:TextBox ID="txtAdress" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" Columns="50"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">E-Mail ID:</label>
                                        <asp:TextBox ID="txtEmail" runat="server" class="form-control" />
                                        <span id="txtEmailError" class="text-danger" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 12 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Marital Status:</label>

                                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="MaritalError" class="text-danger" style="display: none;">Please select a Marital.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">Pin Code:</label>

                                        <asp:TextBox ID="txtpincode" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 13 -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">Bank & Branch Name:</label>

                                        <asp:TextBox ID="txtBranchName" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">

                                        <label class="form-label">IFSC Code:</label>

                                        <asp:TextBox ID="txtIFSCCode" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label">Student's Bank A/C No.:</label>

                                        <asp:TextBox ID="txtBankACNo" runat="server" class="form-control" />

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Row 14 -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Two Identification (i):</label>
                                        <asp:TextBox ID="txtIdentification1" runat="server" CssClass="form-control" />
                                        <label class="form-label required">Two Identification (ii):</label>
                                        <asp:TextBox ID="txtIdentification2" runat="server" CssClass="form-control" />
                                        <span id="IdentificationError" style="display: none; color: red;">Please enter at least one identification mark.</span>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <!-- Row 15 -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label class="form-label required">Medium:</label>

                                        <asp:DropDownList ID="ddlMedium" runat="server" CssClass="form-control select2">
                                        </asp:DropDownList>
                                        <span id="MediumError" class="text-danger" style="display: none;">Please select a Medium.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <div class="form-group">
                                        <label for="dob" class="form-label">ApaarId :</label>
                                        <asp:TextBox ID="txtApaarId" runat="server" class="form-control" oninput="enforceMaxLength(this, 12)" TextMode="Number" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="showaadhardiv">

                            <div class="mb-3">
                                <div class="form-group">
                                    <label class="form-label">Do you have Aadhar?</label><br>
                                    <div class="form-check form-check-inline">
                                        <asp:HiddenField ID="hfAadharOption" runat="server" ClientIDMode="Static" />
                                        <input type="radio" name="aadhar_option" id="aadharYes" class="form-check-input" value="Yes">

                                        <label class="form-check-label" for="aadharYes">Yes</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input type="radio" name="aadhar_option" id="aadharNo" class="form-check-input" value="No">

                                        <label class="form-check-label" for="aadharNo">No</label>
                                    </div>
                                </div>
                            </div>

                            <!-- Div shown if Aadhar = Yes -->
                            <div id="aadharYesDiv">
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
                            <div id="aadharNoDiv">
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
                            </div>
                            <!-- Aadhar Instruction Block -->


                            <!-- Signature Section -->
                            <div class="row justify-content-end mb-3 d-none">
                                <div class="col-auto">
                                    <div class="signature-box">

                                        <asp:Image ID="signaturePreview" runat="server" CssClass="signature-img" AlternateText="Signature Preview" />
                                        <div class="mt-1">
                                            <small>Signature of Candidate</small><br>
                                            <small>अभ्यर्थी का हस्ताक्षर</small>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="col-12 text-center">
                            <asp:HiddenField ID="hfStudentId" runat="server" />
                            <asp:HiddenField ID="hfFaculty" runat="server" />

                            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary px-4 mt-2" Text="Update" OnClick="btnUpdate_Click" OnClientClick="return validateStudentDetails();" />
                            <asp:Button ID="btnAddStudentReg" runat="server" CssClass="btn btn-success" Text="Add Student" OnClick="btnAddStudentReg_Click" Visible="false" OnClientClick="return validateStudentDetails();" />

                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>

    <script>

        function validateStudentDetails() {
            debugger;

            // Radio buttons
            var rdoPrivate = document.getElementById('<%= rdoPrivate.ClientID %>');
            var isPrivate = rdoPrivate.checked;

            if (isPrivate) {
                var privateRequiredFields = [
                    { field: document.getElementById('<%= txtcollegeName.ClientID %>'), error: document.getElementById('txtcollegeNameErr') },
                    { field: document.getElementById('<%= txtcollegeCode.ClientID %>'), error: document.getElementById('txtcollegeCodeErr') },
                    { field: document.getElementById('<%= txtStudentName.ClientID %>'), error: document.getElementById('txtStudentNameErr') },
                    { field: document.getElementById('<%= txtmotherName.ClientID %>'), error: document.getElementById('txtmotherNameErr') },
                    { field: document.getElementById('<%= txtfatherName.ClientID %>'), error: document.getElementById('txtfatherNameErr') },
                    { field: document.getElementById('<%= ddlFaculty.ClientID %>'), error: document.getElementById('ddlFacultyErr') },
                    { field: document.getElementById('<%= txtDOB.ClientID %>'), error: document.getElementById('txtDOBErr') },
                      { field: document.getElementById('<%= ddlMatrixBoard.ClientID %>'), error: document.getElementById('ddlMatrixBoardErr') },
                    { field: document.getElementById('<%= txtboardRollCode.ClientID %>'), error: document.getElementById('txtboardRollCodeErr') },
                    { field: document.getElementById('<%= txtrollNumber.ClientID %>'), error: document.getElementById('txtrollNumberErr') },
                ];

                for (var j = 0; j < privateRequiredFields.length; j++) {
                    var pItem = privateRequiredFields[j];
                    var pValue = pItem.field.value ? pItem.field.value.trim() : '';

                    if (pValue === "" || pValue === "0") {
                        pItem.error.style.display = "inline";
                        pItem.field.classList.add("is-invalid");
                        pItem.field.focus();
                        return false;
                    } else {
                        pItem.error.style.display = "none";
                        pItem.field.classList.remove("is-invalid");
                    }
                }
            }

            var alwaysRequiredFields = [
                { field: document.getElementById('<%= txtsubDivision.ClientID %>'), error: document.getElementById('txtsubDivisionErr') },
                { field: document.getElementById('<%= txtdistrict.ClientID %>'), error: document.getElementById('txtdistrictErr') },
                ////////{ field: document.getElementById('<%= ddlMatrixBoard.ClientID %>'), error: document.getElementById('ddlMatrixBoardErr') },
               <%-- { field: document.getElementById('<%= txtboardRollCode.ClientID %>'), error: document.getElementById('txtboardRollCodeErr') },
                { field: document.getElementById('<%= txtrollNumber.ClientID %>'), error: document.getElementById('txtrollNumberErr') },--%>
                { field: document.getElementById('<%= txtpassingYear.ClientID %>'), error: document.getElementById('txtpassingYearErr') },
                { field: document.getElementById('<%= ddlGender.ClientID %>'), error: document.getElementById('ddlGenderError') },
                { field: document.getElementById('<%= ddlCasteCategory.ClientID %>'), error: document.getElementById('ddlCasteCategoryError') },
                { field: document.getElementById('<%= txtDOB.ClientID %>'), error: document.getElementById('txtDOBErr') }
            ];

            for (var i = 0; i < alwaysRequiredFields.length; i++) {
                var item = alwaysRequiredFields[i];
                var value = item.field.value ? item.field.value.trim() : '';

                if (value === "" || value === "0") {
                    item.error.style.display = "inline";
                    item.field.classList.add("is-invalid");
                    item.field.focus();
                    return false;
                } else {
                    item.error.style.display = "none";
                    item.field.classList.remove("is-invalid");
                }
            }

            // ✅ Additional DOB Max Date Check
            var dobField = document.getElementById('<%= txtDOB.ClientID %>');
            var dobValue = dobField.value.trim();
            var maxDOB = new Date("2015-12-31");

            if (dobValue) {
                var selectedDOB = new Date(dobValue);
                if (selectedDOB > maxDOB) {
                    document.getElementById('txtDOBErr').style.display = "inline";
                    document.getElementById('txtDOBErr').textContent = "DOB must be before 31-Dec-2015.";
                    dobField.classList.add("is-invalid");
                    dobField.focus();
                    return false;
                } else {
                    document.getElementById('txtDOBErr').style.display = "none";
                    dobField.classList.remove("is-invalid");
                }
            }

            var Nationality = document.getElementById('<%= ddlNationality.ClientID %>');
            var errorSpan = document.getElementById('NationalityError');
            if (Nationality.value === "0" || Nationality.value === "") {
                errorSpan.style.display = "inline";
                Nationality.focus();
                return false;
            } else {
                errorSpan.style.display = "none";
                Nationality.classList.remove("is-invalid");
            }

            var Religion = document.getElementById('<%= ddlReligion.ClientID %>');
            var ReligionError = document.getElementById('ReligionError');
            if (Religion.value === "0" || Religion.value === "") {
                ReligionError.style.display = "inline";
                Religion.focus();
                return false;
            } else {
                ReligionError.style.display = "none";
                Religion.classList.remove("is-invalid");
            }


            var mobileField = document.getElementById('<%= txtMobile.ClientID %>');
            var mobile = mobileField.value.trim();
            var mobileErrorSpan = document.getElementById('txtMobileError');

            // ✅ Regex: Starts with 6-9 and has exactly 10 digits
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
            }

            else {
                mobileErrorSpan.style.display = "none";
                mobileField.classList.remove("is-invalid");
            }

            var parentnoField = document.getElementById('<%= txtuniqueid.ClientID %>');
            var parentno = parentnoField.value.trim();
            var parentnoErrorSpan = document.getElementById('txtparentnoError');

            //var parentnoPattern = /^[6-9]\d{9}$/;
            //if (!parentnoPattern.test(parentno) && parentno != "") {
            //    parentnoErrorSpan.style.display = "inline";
            //    parentnoErrorSpan.textContent = "Please enter a valid 10-digit Parent Number starting with 6, 7, 8, or 9.";
            //    parentnoField.classList.add("is-invalid");
            //    parentnoField.focus();
            //    return false;
            //} else {
            //    parentnoErrorSpan.style.display = "none";
            //    parentnoField.classList.remove("is-invalid");
            //}

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

            var MaritalStatus = document.getElementById('<%= ddlMaritalStatus.ClientID %>');
            var MaritalError = document.getElementById('MaritalError');
            if (MaritalStatus.value === "0" || MaritalStatus.value === "") {
                MaritalError.style.display = "inline";
                MaritalStatus.focus();
                return false;
            } else {
                MaritalError.style.display = "none";
                MaritalStatus.classList.remove("is-invalid");
            }

            var identification1 = document.getElementById('<%= txtIdentification1.ClientID %>').value.trim();
            var identification2 = document.getElementById('<%= txtIdentification2.ClientID %>').value.trim();
            var identificationError = document.getElementById('IdentificationError');

            if (identification1 === "" && identification2 === "") {
                identificationError.style.display = "inline";
                document.getElementById('<%= txtIdentification1.ClientID %>').classList.add("is-invalid");
                document.getElementById('<%= txtIdentification2.ClientID %>').classList.add("is-invalid");
                return false;
            } else {
                identificationError.style.display = "none";
                document.getElementById('<%= txtIdentification1.ClientID %>').classList.remove("is-invalid");
                document.getElementById('<%= txtIdentification2.ClientID %>').classList.remove("is-invalid");
            }

            var Medium = document.getElementById('<%= ddlMedium.ClientID %>');
            var MediumError = document.getElementById('MediumError');
            if (Medium.value === "0" || Medium.value === "") {
                MediumError.style.display = "inline";
                Medium.focus();
                return false;
            } else {
                MediumError.style.display = "none";
                Medium.classList.remove("is-invalid");
            }

            // Aadhar validation
            var aadharYes = document.getElementById('aadharYes');
            var aadharNo = document.getElementById('aadharNo');
            var aadharPattern = /^[0-9]{12}$/;
            var AadharNumber = document.getElementById('<%= txtAadharNumber.ClientID %>');
            var txtAadharNumberError = document.getElementById('txtAadharNumberError');

            if (!aadharYes.checked && !aadharNo.checked) {
                alert("Please select whether you have Aadhar.");
                aadharYes.focus();
                return false;
            }

            if (aadharYes.checked) {
                var aadharValue = AadharNumber.value.trim();

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
                    txtAadharNumberError.textContent = "";
                    AadharNumber.classList.remove("is-invalid");
                }
            }
        }
        window.onload = function () {
            const yesRadio = document.getElementById('aadharYes');
            const noRadio = document.getElementById('aadharNo');
            const aadharNumberField = document.getElementById('<%= txtAadharNumber.ClientID %>');
            const hfAadharOption = document.getElementById('hfAadharOption');

            // Pre-select based on Aadhar value
            if (aadharNumberField && aadharNumberField.value.trim() !== "") {
                yesRadio.checked = true;
                hfAadharOption.value = "Yes";
            } else {
                // noRadio.checked = true;
                yesRadio.checked = true;
                hfAadharOption.value = "No";
            }

            toggleAadharDivs();

            // Add event listeners
            yesRadio.addEventListener('change', function () {
                hfAadharOption.value = "Yes";
                toggleAadharDivs();
            });
            noRadio.addEventListener('change', function () {
                hfAadharOption.value = "No";
                toggleAadharDivs();
            });
        };

        function toggleAadharDivs() {
            const yesRadio = document.getElementById('aadharYes');
            const noRadio = document.getElementById('aadharNo');
            const yesDiv = document.getElementById('aadharYesDiv');
            const noDiv = document.getElementById('aadharNoDiv');
            const aadharNumberField = document.getElementById('<%= txtAadharNumber.ClientID %>');

            if (yesRadio.checked) {
                yesDiv.style.display = 'block';
                noDiv.style.display = 'none';
            } else if (noRadio.checked) {
                yesDiv.style.display = 'none';
                noDiv.style.display = 'block';
                //if (aadharNumberField) {
                //    aadharNumberField.value = '';
                //}
            }
        }

        function enforceMaxLength(el, maxLength) {
            if (el.value.length > maxLength) {
                el.value = el.value.slice(0, maxLength);
            }
        }
    </script>
</asp:Content>

