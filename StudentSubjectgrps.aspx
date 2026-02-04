<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StudentSubjectgrps.aspx.cs" Inherits="StudentSubjectgrps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-collapse: collapse;
            width: 100%;
        }

        th, td {
            padding: 4px 8px;
            vertical-align: middle;
        }

        ul {
            list-style: none;
        }

            ul li input[type=checkbox],
            ul li input[type=radio],
            table tr td input[type=checkbox],
            table tr td input[type=radio] {
                margin-right: 10px;
            }

        select.no-arrow {
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            background-image: none !important;
            background-repeat: no-repeat;
            background-position: right center;
            padding-right: 10px;
        }


            select.no-arrow::-ms-expand {
                display: none;
            }

        .borderline {
            border: 1px solid #00000075;
            padding: 10px;
        }

        input[type=checkbox] {
            transform: scale(1.5);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row">
            <div class="col-12">

                <div class="card">

                    <div class="card-body">
                        <div class="row">
                            <div class="col-12 text-center">
                                <div class="section-title">Compulsory Subject Group (Total 200 Marks)</div>
                                <p>(Select (&#10003;) one subject from each group - each subject: 100 Marks)</p>
                            </div>
                            <div class="form-group col-md-12">
                                <label for="faculty">Faculty</label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control select2  no-arrow" Enabled="false">
                                </asp:DropDownList>
                            </div>
                            <div class="borderline col-md-6">

                                <p><strong>Compulsory Subject Group-1 (100 Marks)</strong></p>
                                <asp:Repeater ID="rptCompulsorySubjects" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:CheckBox ID="chkSubject" runat="server" CssClass="compGroup1"
                                                Text='<%# Eval("SubjectPaperName") + " - " + Eval("SubjectPaperCode") + "" %>'
                                                Value='<%# Eval("SubjectPaperCode") %>' onclick="validateCompulsoryGroups()" />
                                            <input type="hidden" class="compGroup1Value" value='<%# Eval("SubjectPaperCode") %>' />
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="col-md-6 borderline">
                                <p><strong>Compulsory Subject Group-2 (100 Marks)</strong></p>
                                <asp:Repeater ID="rptCompulsorySubjects2" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:CheckBox ID="chkSubject" runat="server" CssClass="compGroup2"
                                                Text='<%# Eval("SubjectPaperName") + " - " + Eval("SubjectPaperCode") + "" %>'
                                                Value='<%# Eval("SubjectPaperCode") %>' onclick="validateCompulsoryGroups()" />
                                            <input type="hidden" class="compGroup2Value" value='<%# Eval("SubjectPaperCode") %>' />
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-body">

                        <div id="ElectiveSection" runat="server">
                            <div class="q33-box borderline">
                                <div class="section-title">Elective Subject Group (Total 300 Marks)</div>
                                <p>(Select (✓) any three subjects - each 100 Marks)</p>
                                <table>
                                    <asp:Repeater ID="rptElectiveSubjects" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# !string.IsNullOrEmpty(Eval("Name1").ToString()) ? "<input type='checkbox' ID=\"chkSubject_" + Eval("Code1") + "\" name='chkElectiveSubjects' class='electiveSubject electiveSubject1' " +
                                            "value=\"" + Eval("Code1") + "\" " + "data-name=\"" + Eval("Name1") + "\" " + GetCheckedAttr(Eval("Code1")) + " onclick='validateCompulsoryGroups()' /> " + HttpUtility.HtmlEncode(Eval("Name1")) + " - " + HttpUtility.HtmlEncode(Eval("Code1")) : "" %>
                                                </td>
                                                <td>
                                                    <%# !string.IsNullOrEmpty(Eval("Name2").ToString()) ? "<input type='checkbox' ID=\"chkGroup2_" + Eval("Code2") + "\" name='chkGroup2Subjects' class='electiveSubject electiveSubject2' " +
                                            "value=\"" + Eval("Code2") + "\" " + "data-name=\"" + Eval("Name2") + "\" " +GetCheckedAttr(Eval("Code2")) + " onclick='validateCompulsoryGroups()' /> " + HttpUtility.HtmlEncode(Eval("Name2")) + " - " + HttpUtility.HtmlEncode(Eval("Code2")): ""%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>

                        <div id="VocElectiveSection" runat="server">
                            <div class="q33-box borderline">
                                <div class="section-title">Elective Subject Group (Total 300 Marks)</div>
                                <p>(Select (✓) any three subjects - each 100 Marks)</p>
                                <table>
                                    <asp:Repeater ID="rptVocElectiveSubjects" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# !string.IsNullOrEmpty(Eval("Name1").ToString()) ? 
                    "<input type='checkbox' " +
                    "ID=\"chkVocSubject_" + Eval("Code1") + "\" " +
                    "name='chkVocElectiveSubjects' " +
                    "class='electiveSubject electiveVoc1' " +
                    "value=\"" + Eval("Code1") + "\" " +
                    "data-name=\"" + HttpUtility.HtmlEncode(Eval("Name1")).ToLower() + "\" " +
                    (Eval("Code1").ToString() == "402" ? "checked='checked'" : GetCheckedAttr(Eval("Code1"))) + " " +
                    "onclick='syncVocElectiveSelection(this)' /> " +
                    HttpUtility.HtmlEncode(Eval("Name1")) + " - " + HttpUtility.HtmlEncode(Eval("Code1")) 
                    : "" %>
                                                </td>
                                                <td>
                                                    <%# !string.IsNullOrEmpty(Eval("Name2").ToString()) ? 
                    "<input type='checkbox' " +
                    "ID=\"chkVocSubject_" + Eval("Code2") + "\" " +
                    "name='chkVocElectiveSubjects' " +
                    "class='electiveSubject electiveVoc2' " +
                    "value=\"" + Eval("Code2") + "\" " +
                    "data-name=\"" + HttpUtility.HtmlEncode(Eval("Name2")).ToLower() + "\" " +
                    GetCheckedAttr(Eval("Code2")) + " " +
                    "onclick='syncVocElectiveSelection(this)' /> " +
                    HttpUtility.HtmlEncode(Eval("Name2")) + " - " + HttpUtility.HtmlEncode(Eval("Code2")) 
                    : "" %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>



                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header"></div>
                    <div class="card-body">
                        <div class="q33-box borderline">
                            <div class="section-title">Additional Subject Group (100 Marks)</div>
                            <ol>
                                <li>The student who desires to keep additional subject must select (✓) any one subject from the following group that is NOT selected in compulsory or elective groups.</li>
                                <li>The student who does not want to keep additional subject should not select any subject under this group.</li>
                            </ol>
                            <table>
                                <asp:HiddenField ID="hfAdditionalSubjects" runat="server" />
                                <asp:Repeater ID="rptAdditionalSubjects" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox
                                                    ID="chkAdditional1"
                                                    runat="server"
                                                    CssClass="additionalSubject"
                                                    Text='<%# Eval("Name1") + " - " + Eval("Code1") + "" %>'
                                                    ToolTip='<%# Eval("Name1") %>'
                                                    Enabled='<%# !string.IsNullOrEmpty(Eval("Name1").ToString()) %>'
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("Code1").ToString()) %>'
                                                    onclick="validateCompulsoryGroups()" />
                                                <asp:HiddenField ID="hfCode1" runat="server" Value='<%# Eval("Code1") %>' />
                                                <asp:HiddenField ID="hfPaperId1" runat="server" Value='<%# Eval("PaperId1") %>' />
                                            </td>

                                            <td>
                                                <asp:CheckBox ID="chkAdditional2" runat="server"
                                                    CssClass="additionalSubject"
                                                    Text='<%# Eval("Name2") + " - " + Eval("Code2") + "" %>'
                                                    ToolTip='<%# Eval("Name2") %>'
                                                    Enabled='<%# !string.IsNullOrEmpty(Eval("Name2").ToString()) %>'
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("Code2").ToString()) %>'
                                                    onclick="validateCompulsoryGroups()" />
                                                <asp:HiddenField ID="hfCode2" runat="server" Value='<%# Eval("Code2") %>' />
                                                <asp:HiddenField ID="hfPaperId2" runat="server" Value='<%# Eval("PaperId2") %>' />
                                            </td>

                                            <td>
                                                <asp:CheckBox ID="chkAdditional3" runat="server"
                                                    CssClass="additionalSubject"
                                                    Text='<%# Eval("Name3") + " - " + Eval("Code3") + "" %>'
                                                    ToolTip='<%# Eval("Name3") %>'
                                                    Enabled='<%# !string.IsNullOrEmpty(Eval("Name3").ToString()) %>'
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("Code3").ToString()) %>'
                                                    onclick="validateCompulsoryGroups()" />
                                                <asp:HiddenField ID="hfCode3" runat="server" Value='<%# Eval("Code3") %>' />
                                                <asp:HiddenField ID="hfPaperId3" runat="server" Value='<%# Eval("PaperId3") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>

                            <div class="note">
                                <b>Note:</b> Computer Science, Yoga & Phy. Edu. and Multimedia & Web.Tech. cannot be interchanged/swapped with any other subject.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">

                <div class="card">

                    <div class="card-body">
                        <div class="subject-group" id="divVocationalSubjects" runat="server">
                            <p runat="server" id="div_vocational" visible="false"><strong>Vocational Additional Subjects</strong></p>
                            <div class="borderline" id="Borderline_vocational" runat="server" visible="false">
                                <asp:Repeater ID="rptVocationalAdditionalSubjects" runat="server">

                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <li>
                                            <asp:CheckBox ID="chkSubject" runat="server"
                                                Text='<%# Eval("SubjectPaperName") + " - " + Eval("SubjectPaperCode") + "" %>'
                                                Value='<%# Eval("SubjectPaperCode") %>'
                                                CssClass="VocationalSubjects"
                                                ToolTip='<%# Eval("SubjectPaperName") %>'
                                                onclick="validateCompulsoryGroups()" />

                                            <input type="hidden" class="vocationalValue" value='<%# Eval("SubjectPaperCode") %>' />
                                        </li>

                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="text-center">


                            <asp:HiddenField ID="hfSubjectGroupId" runat="server" />
                            <asp:HiddenField ID="hfVocationalSubjectCount" runat="server" />
                            <asp:HiddenField ID="hfElectiveSubjects" runat="server" />
                            <asp:HiddenField ID="hfStudentPaperAppliedId" runat="server" />
                            <asp:HiddenField ID="hfSubjectPaperId" runat="server" />

                            <%--<asp:Button ID="Button1" runat="server" Text="Submit Selected Subjects"  CssClass="btn btn-primary text-center" OnClientClick="return validateSubjectSelection();"/>--%>
                            <asp:Button ID="btnSubmitSubjects" runat="server" Text="Update Subjects" CssClass="btn btn-primary text-center  mt-2" OnClick="btnSubmitSubjects_Click" OnClientClick="return validateSubjectSelection();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.7.3/dist/sweetalert2.all.min.js"></script>

    <script type="text/javascript">

        function syncVocElectiveSelection(clickedCheckbox) {
            // Force Code 402 to stay checked
            if (clickedCheckbox.value === "402" && !clickedCheckbox.checked) {
                clickedCheckbox.checked = true;
                clickedCheckbox.setAttribute('checked', 'checked');
                return;
            }

            const subjectName = clickedCheckbox.dataset.name.trim().toLowerCase();
            if (!subjectName) return;

            const isChecked = clickedCheckbox.checked;
            clickedCheckbox.setAttribute('checked', isChecked ? 'checked' : '');
            document.querySelectorAll('input[type="checkbox"][data-name="' + subjectName + '"]').forEach(cb => {
                if (cb !== clickedCheckbox) {
                    cb.checked = isChecked;
                    cb.setAttribute('checked', isChecked ? 'checked' : '');
                }
            });
        }


        function getElectiveSubjectValues() {
            const values = [];
            document.querySelectorAll('.electiveSubject:checked').forEach(cb => {
                values.push(cb.value.trim());
            });

            //document.querySelectorAll('.electiveSubject:checked').forEach(cb => {
            //    values.push(cb.value.trim());
            //});
            return values;
        }

        function storeElectiveSelections() {
            const selectedElectives = getElectiveSubjectValues();
            document.getElementById('<%= hfElectiveSubjects.ClientID %>').value = selectedElectives.join(',');
        }

        function restoreElectiveSelections() {
            const stored = document.getElementById('<%= hfElectiveSubjects.ClientID %>').value;
            if (!stored) return;

            const selected = stored.split(',');
            selected.forEach(code => {
                const cb = document.querySelector('.electiveSubject[value="' + code.trim() + '"]');
                if (cb) cb.checked = true;
            });
        }

        function validateSubjectSelection() {
            storeElectiveSelections();

            let group1SubjectsDetails = getCheckedGroupDetails(".compGroup1", ".compGroup1Value");
            let group2SubjectsDetails = getCheckedGroupDetails(".compGroup2", ".compGroup2Value");
            let electiveSubjectsDetails = getElectiveSubjectDetails();
            let additionalSubjects = getAdditionalSubjectValues();
            let vocationalSubjects = getVocationalSubjectValues();

            if (group1SubjectsDetails.length !== 1) {
                swal({
                    title: "Selection Required",
                    text: "Please select exactly one subject from Compulsory Group 1.",
                    icon: "warning",
                    button: "OK"
                });
                return false;
            }

            if (group2SubjectsDetails.length !== 1) {
                swal({
                    title: "Selection Required",
                    text: "Please select exactly one subject from Compulsory Group 2.",
                    icon: "warning",
                    button: "OK"
                });
                return false;
            }

            if (electiveSubjectsDetails.length !== 3) {
                swal({
                    title: "Selection Required",
                    text: "Please select exactly three subjects from Elective Subject Group.",
                    icon: "warning",
                    button: "OK"
                });
                return false;
            }
            if (additionalSubjects.length === 1) {

                const allMainSubjectsWithDetails = [
                    ...group1SubjectsDetails,
                    ...group2SubjectsDetails,
                    ...electiveSubjectsDetails
                ];

                const selectedAdditional = additionalSubjects[0]; // { code, name }

                const isDuplicate = allMainSubjectsWithDetails.some(mainSubject => {
                    return (
                        mainSubject.name?.trim().toLowerCase() === selectedAdditional.name?.trim().toLowerCase() ||
                        mainSubject.code?.trim().toLowerCase() === selectedAdditional.code?.trim().toLowerCase()
                    );
                });


                if (isDuplicate) {
                    swal({
                        title: "Invalid Selection",
                        text: "The subject '" + selectedAdditional.name.toUpperCase() + "' selected in the Additional Group must not be the same as any subject selected in the Compulsory or Elective Groups.",
                        icon: "error",
                        button: "OK"
                    });

                    document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
                        const parentSpan = cb.closest('.additionalSubject');
                        const title = parentSpan?.title?.trim().toLowerCase();
                        const codeMatch = parentSpan?.innerText?.match(/\(([^)]+)\)/);
                        const code = codeMatch && codeMatch[1] ? codeMatch[1].trim().toLowerCase() : '';

                        if (
                            title === selectedAdditional.name?.trim().toLowerCase() ||
                            code === selectedAdditional.code?.trim().toLowerCase()
                        ) {
                            cb.checked = false; // uncheck the checkbox
                        }
                    });

                    return false;
                }
            }
            const vocationalCountLimit = 25;
            const currentVocationalStudentsCount = parseInt(document.getElementById('<%= hfVocationalSubjectCount.ClientID %>').value);

            if (vocationalSubjects.length > 0 && currentVocationalStudentsCount >= vocationalCountLimit) {
                swal({
                    title: "Limit Reached",
                    text: "Only " + vocationalCountLimit + " students can apply for vocational subjects in this faculty and college. You cannot select any vocational subjects.",
                    icon: "error",
                    button: "OK"
                });
                // Uncheck all vocational subjects
                document.querySelectorAll('.VocationalSubjects input[type="checkbox"]').forEach(cb => {
                    cb.checked = false;
                });
                // Uncheck all additional subjects as well
                document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
                    debugger
                    cb.checked = false;
                });
                return false; // Prevent form submission if the vocational limit is exceeded on client side with a new selection
            }

            return true;


        }

        function getCheckedGroupValues(checkboxClass, valueClass) {
            const values = [];
            document.querySelectorAll(checkboxClass + " input[type='checkbox']").forEach(cb => {
                if (cb.checked) {
                    const hidden = cb.closest("li").querySelector(valueClass);
                    if (hidden) values.push(hidden.value.trim().toLowerCase());
                }
            });
            return values;
        }

        function getCheckedGroupDetails(checkboxClass, valueClass) {
            const details = [];
            document.querySelectorAll(checkboxClass + " input[type='checkbox']").forEach(cb => {
                if (cb.checked) {

                    const label = cb.parentElement.innerText.split('-')[0].trim().toLowerCase();
                    const hidden = cb.closest("li").querySelector(valueClass);
                    if (hidden) {
                        details.push({
                            code: hidden.value.trim().toLowerCase(),
                            name: label
                        });
                    }
                }
            });
            return details;
        }

        function getElectiveSubjectDetails() {
            const details = [];
            document.querySelectorAll('.electiveSubject:checked').forEach(cb => {
                const label = cb.dataset.name?.toLowerCase() ?? '';
                details.push({
                    code: cb.value.trim().toLowerCase(),
                    name: label
                });
            });
            return details;
        }
        function getAdditionalSubjectValues() {
            debugger
            const values = [];

            document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
                if (cb.checked) {

                    const parentSpan = cb.closest('.additionalSubject');
                    const label = parentSpan.querySelector('label')?.textContent?.trim() || '';
                    const title = parentSpan?.title?.trim() || '';

                    let code = '', name = title;

                    const parts = label.split(' - ');
                    if (parts.length === 2) {
                        name = parts[0].trim();
                        code = parts[1].trim();
                    }
                    if (!code) {
                        debugger
                        const hidden = td.querySelector('input[type="hidden"]');
                        if (hidden) code = hidden.value.trim();
                    }
                    if (name && code) {
                        values.push({ code, name });
                    }
                }
            });

            return values;
        }


        function getVocationalSubjectValues() {
            const values = [];
            document.querySelectorAll(".VocationalSubjects input[type='checkbox']").forEach(cb => {
                if (cb.checked) {
                    const hidden = cb.closest("li").querySelector(".vocationalValue");
                    if (hidden) {
                        values.push(hidden.value.trim().toLowerCase());
                    }
                }
            });
            return values;
        }

        function validateCompulsoryGroups() {
            const restrictedSubjects = ['english', 'hindi'];
            let group1Selected = [];
            let group2Selected = [];

            document.querySelectorAll('.compGroup1 input[type="checkbox"]').forEach(cb => {
                if (cb.checked) {
                    const subjectName = cb.parentElement.innerText.split('-')[0].trim().toLowerCase();
                    group1Selected.push({ name: subjectName, checkbox: cb });
                }
            });

            document.querySelectorAll('.compGroup2 input[type="checkbox"]').forEach(cb => {
                if (cb.checked) {
                    const subjectName = cb.parentElement.innerText.split('-')[0].trim().toLowerCase();
                    group2Selected.push({ name: subjectName, checkbox: cb });
                }
            });

            document.querySelectorAll('.compGroup1 input[type="checkbox"]').forEach(cb => {
                cb.addEventListener('change', function () {
                    if (this.checked) {
                        document.querySelectorAll('.compGroup1 input[type="checkbox"]').forEach(otherCb => {
                            if (otherCb !== this) otherCb.checked = false;
                        });
                    }
                });
            });

            document.querySelectorAll('.compGroup2 input[type="checkbox"]').forEach(cb => {
                cb.addEventListener('change', function () {
                    if (this.checked) {
                        document.querySelectorAll('.compGroup2 input[type="checkbox"]').forEach(otherCb => {
                            if (otherCb !== this) otherCb.checked = false;
                        });
                    }
                });
            });

            for (let subject of restrictedSubjects) {
                let inGroup1 = group1Selected.find(s => s.name === subject);
                let inGroup2 = group2Selected.find(s => s.name === subject);

                if (inGroup1 && inGroup2) {
                    swal({
                        title: "Invalid Selection",
                        text: "Subject '" + subject.charAt(0).toUpperCase() + subject.slice(1) + "' cannot be selected in both Compulsory Groups.",
                        icon: "error",
                        button: "OK"
                    });
                    inGroup2.checkbox.checked = false;
                    return false;
                }
            }

            //let electiveCheckboxes = document.querySelectorAll('input.electiveSubject');
            //electiveCheckboxes.forEach(cb => {
            //    cb.addEventListener('change', function () {
            //        let checkedCount = 0;
            //        electiveCheckboxes.forEach(eCb => {
            //            if (eCb.checked) checkedCount++;
            //        });

            //        if (checkedCount > 3) {
            //            swal({
            //                title: "Limit Exceeded",
            //                text: "You can select a maximum of three elective subjects.",
            //                icon: "warning",
            //                button: "OK"
            //            });
            //            this.checked = false;
            //        }
            //    });
            //});
            let electiveCheckboxes = document.querySelectorAll('input.electiveSubject');
            electiveCheckboxes.forEach(cb => {
                cb.addEventListener('change', function () {
                    let checkedBoxes = Array.from(document.querySelectorAll('input.electiveSubject:checked'));
                    if (checkedBoxes.length > 3) {
                        swal({
                            title: "Limit Exceeded",
                            text: "You can select a maximum of three elective subjects.",
                            icon: "warning",
                            button: "OK"
                        });

                        // Uncheck all beyond the first three
                        checkedBoxes.slice(3).forEach(extra => {
                            extra.checked = false;
                            extra.removeAttribute('checked');
                        });
                    }
                });
            });




            let additionalCheckboxes = document.querySelectorAll('.additionalSubject input[type="checkbox"]');
            additionalCheckboxes.forEach(cb => {
                cb.addEventListener('change', function () {
                    if (this.checked) {
                        additionalCheckboxes.forEach(otherCb => {
                            if (otherCb !== this) otherCb.checked = false;
                        });
                    }
                });
            });

            let vocationalCheckboxes = document.querySelectorAll('.VocationalSubjects input[type="checkbox"]');
            vocationalCheckboxes.forEach(cb => {
                cb.addEventListener('change', function () {
                    const vocationalCountLimit = 2;
                    const currentVocationalStudentsCount = parseInt(document.getElementById('<%= hfVocationalSubjectCount.ClientID %>').value);

                    if (currentVocationalStudentsCount >= vocationalCountLimit) {
                        if (this.checked) {
                            swal({
                                title: "Limit Reached",
                                text: "Only " + vocationalCountLimit + " students can apply for vocational subjects in this faculty and college. You cannot select any vocational subjects.",
                                icon: "error",
                                button: "OK"
                            });
                            this.checked = false;
                        }

                        vocationalCheckboxes.forEach(otherCb => {
                            otherCb.checked = false;
                        });
                    } else {
                        if (this.checked) {
                            vocationalCheckboxes.forEach(otherCb => {
                                if (otherCb !== this) otherCb.checked = false;
                            });
                        }
                    }
                });
            });

            return true;
        }

        document.addEventListener('DOMContentLoaded', function () {
            validateCompulsoryGroups();
            restoreElectiveSelections();

            const form = document.forms[0];
            if (form) {
                form.addEventListener('submit', function () {
                    storeElectiveSelections();
                });
            }
        });

       <%-- function storeAdditionalSelections() {
            debugger
            const selected = [];
            document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
                if (cb.checked) {
                    selected.push(cb.value.trim());
                }
            });
            document.getElementById('<%= hfAdditionalSubjects.ClientID %>').value = selected.join(',');
    }--%>
        function storeAdditionalSelections() {
            const selected = [];
            const checkboxes = document.querySelectorAll('input.additionalSubject[type="checkbox"]');

            checkboxes.forEach(cb => {
                if (cb.checked) {
                    selected.push(cb.value.trim());
                }
            });

            console.log('Selected:', selected);

            document.getElementById('<%= hfAdditionalSubjects.ClientID %>').value = selected.join(',');
        }


        function restoreAdditionalSelections() {
            const stored = document.getElementById('<%= hfAdditionalSubjects.ClientID %>').value;
            if (!stored) return;

            const selectedCodes = stored.split(',').map(s => s.trim().toLowerCase());
            document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
                const code = cb.value.trim().toLowerCase();
                if (selectedCodes.includes(code)) {
                    cb.checked = true;
                }
            });
        }

        document.addEventListener('DOMContentLoaded', function () {
            restoreAdditionalSelections();

            const form = document.forms[0];
            if (form) {
                form.addEventListener('submit', function () {
                    storeAdditionalSelections();
                });
            }
        });
    </script>


</asp:Content>
