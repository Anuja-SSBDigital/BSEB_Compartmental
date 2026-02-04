<%@ Page Language="C#"   MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExamStudentSubjectgrps.aspx.cs" Inherits="ExamStudentSubjectgrps" %>

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
                            <div class="borderline col-md-6" ID="comp1title" runat="server">

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
                           <div class="col-md-6 borderline" ID="comp1title2" runat="server">
                                <p ><strong>Compulsory Subject Group-2 (100 Marks)</strong></p>
                                <asp:Repeater ID="rptCompulsorySubjects2" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:CheckBox ID="chkSubject" runat="server" CssClass="compGroup2"
                                                Text='<%# Eval("SubjectPaperName") + " - " + Eval("SubjectPaperCode") %>'
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
        <div class="card" id="ElectiveCard" runat="server">
            <div class="card-body">

                <div id="ElectiveSection" runat="server">
                    <div class="q33-box borderline"  ID="Elective1title" runat="server">
                        <div class="section-title">Elective Subject Group (Total 300 Marks)</div>
                        <p ID="Elective1title2" runat="server">(Select (✓) any three subjects - each 100 Marks)</p>
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
                    <div class="q33-box borderline" ID="Elective1titleVoc" runat="server">
                        <div class="section-title">Elective Subject Group (Total 300 Marks)</div>
                        <p ID="Elective1titleVoc2" runat="server">(Select (✓) any three subjects - each 100 Marks)</p>
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
        <div class="row"  id="divAdditionalSubjects" runat="server">
            <div class="col-12">
                <div class="card">
                    <div class="card-header"></div>
                    <div class="card-body">
                        <div class="q33-box borderline" ID="Addi1title" runat="server">
                            <div class="section-title" >Additional Subject Group (100 Marks)</div>
                            <ol ID="Add2title" runat="server">
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
                                                    <%--<asp:HiddenField ID="hfPaperId1" runat="server" Value='<%# Eval("PaperId1") %>' />--%>
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
                                                      <%--<asp:HiddenField ID="hfPaperId2" runat="server" Value='<%# Eval("PaperId2") %>' />--%>
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
                                                     <%--<asp:HiddenField ID="hfPaperId3" runat="server" Value='<%# Eval("PaperId3") %>' />--%>
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
                                            Text='<%# Eval("SubjectPaperName") + " - " + Eval("SubjectPaperCode") %>'
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
                               <asp:HiddenField runat="server" ID="hnd_extype"/>
                               <asp:HiddenField runat="server" ID="hnd_ExamCorrectionForm"/>
                               <asp:HiddenField runat="server" ID="hnd_StudentExamRegForm"/>
                               <asp:HiddenField runat="server" ID="hnd_FacultyId"/>
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
   

     //function syncVocElectiveSelection(clickedCheckbox) {
     //    // Force Code 402 to stay checked
     //    if (clickedCheckbox.value === "402" && !clickedCheckbox.checked) {
     //        clickedCheckbox.checked = true;
     //        clickedCheckbox.setAttribute('checked', 'checked');
     //        return;
     //    }

     //    const subjectName = clickedCheckbox.dataset.name.trim().toLowerCase();
     //    if (!subjectName) return;

     //    const isChecked = clickedCheckbox.checked;
     //    clickedCheckbox.setAttribute('checked', isChecked ? 'checked' : '');
     //    document.querySelectorAll('input[type="checkbox"][data-name="' + subjectName + '"]').forEach(cb => {
     //        if (cb !== clickedCheckbox) {
     //            cb.checked = isChecked;
     //            cb.setAttribute('checked', isChecked ? 'checked' : '');
     //        }
     //    });
        
     //}
     function syncVocElectiveSelection(clickedCheckbox) {
         debugger

         const ExamTypeId = parseInt(document.getElementById('<%= hnd_extype.ClientID %>').value);
         const ExamCorrectionForm = document.getElementById('<%= hnd_ExamCorrectionForm.ClientID %>').value.trim();

         const subjectName = clickedCheckbox.dataset.name.trim().toLowerCase();

         // ⭐ CASE 1: FULL LOCK
         // ExamTypeId = 3 AND ExamCorrectionForm = "ExamCorrectionForm"
         if (ExamTypeId === 3 && ExamCorrectionForm === "ExamCorrectionForm") {
             clickedCheckbox.checked = clickedCheckbox.defaultChecked; // revert to original
             return false;  // stop — lock all Voc elective checkboxes
         }

         // ⭐ CASE 2: Normal Selection Sync ONLY IF:
         // ExamTypeId = 3 AND ExamCorrectionForm != "ExamCorrectionForm"
         if (ExamTypeId === 3 && ExamCorrectionForm !== "ExamCorrectionForm") {

             const isChecked = clickedCheckbox.checked;
             clickedCheckbox.setAttribute('checked', isChecked ? 'checked' : '');

             // Sync other checkboxes of same subject
             document.querySelectorAll('input[type="checkbox"][data-name="' + subjectName + '"]').forEach(cb => {
                 if (cb !== clickedCheckbox) {
                     cb.checked = isChecked;
                     cb.setAttribute('checked', isChecked ? 'checked' : '');
                 }
             });

             return; // stop here, don’t go further
         }

         // ⭐ CASE 3: OTHER EXAM TYPES → Original logic

         // Force Code 402 to stay checked
         if (clickedCheckbox.value === "402" && !clickedCheckbox.checked) {
             clickedCheckbox.checked = true;
             clickedCheckbox.setAttribute('checked', 'checked');
             return;
         }

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
         debugger
         const ExamTypeId = parseInt(document.getElementById('<%= hnd_extype.ClientID %>').value);
         const ExamCorrectionForm = document.getElementById('<%= hnd_ExamCorrectionForm.ClientID %>').value;

         if (ExamTypeId == 3) 
         {
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
                 const selectedAdditional = additionalSubjects[0];

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
                             cb.checked = false;
                         }
                     });
                     return false;
                 }
             }
             return true;
         }
         else if (ExamTypeId == 1 && ExamCorrectionForm == 'ExamCorrectionForm')
         {
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
                 const selectedAdditional = additionalSubjects[0];

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
                             cb.checked = false;
                         }
                     });
                     return false;
                 }
             }
             return true;
         }
         else if (ExamTypeId === 4) {
             const allCheckboxes = document.querySelectorAll(
                 '.compGroup1 input[type="checkbox"], ' +
                 '.compGroup2 input[type="checkbox"], ' +
                 'input.electiveSubject, ' +
                 //'input.electiveVoc1, ' +
                 //'input.electiveVoc2, ' +
                 '.additionalSubject input[type="checkbox"], ' +
                 '.VocationalSubjects input[type="checkbox"]'
             );

             const anySelected = Array.from(allCheckboxes).some(cb => cb.checked);

             if (!anySelected) {
                 swal({
                     title: "Selection Required",
                     text: "Please select at least one subject from any of the groups.",
                     icon: "warning",
                     button: "OK"
                 });
                 return false;
             }
             return true;
         }
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
         const ExamTypeId = parseInt(document.getElementById('<%= hnd_extype.ClientID %>').value);
        if (ExamTypeId != 4) {
            const restrictedSubjects = ['english', 'hindi'];

            // Attach change event listeners to handle the single-selection logic and duplicate check
            document.querySelectorAll('.compGroup1 input[type="checkbox"]').forEach(cb => {
                cb.addEventListener('change', function () {
                    if (this.checked) {
                        const selectedSubjectName = this.parentElement.innerText.split('-')[0].trim().toLowerCase();

                        // Check for duplicate in Group 2
                        let group2CheckedSubject = document.querySelector('.compGroup2 input[type="checkbox"]:checked');
                        let group2SubjectName = group2CheckedSubject ? group2CheckedSubject.parentElement.innerText.split('-')[0].trim().toLowerCase() : '';

                        if (restrictedSubjects.includes(selectedSubjectName) && selectedSubjectName === group2SubjectName) {
                            swal({
                                title: "Invalid Selection",
                                text: `Subject '${selectedSubjectName.charAt(0).toUpperCase() + selectedSubjectName.slice(1)}' cannot be selected in both Compulsory Groups.`,
                                icon: "error",
                                button: "OK"
                            });
                            this.checked = false; // Uncheck the newly selected subject
                            return;
                        }

                        // Uncheck other subjects in Group 1
                        document.querySelectorAll('.compGroup1 input[type="checkbox"]').forEach(otherCb => {
                            if (otherCb !== this) otherCb.checked = false;
                        });
                    }
                });
            });

            document.querySelectorAll('.compGroup2 input[type="checkbox"]').forEach(cb => {
                cb.addEventListener('change', function () {
                    if (this.checked) {
                        const selectedSubjectName = this.parentElement.innerText.split('-')[0].trim().toLowerCase();

                        // Check for duplicate in Group 1
                        let group1CheckedSubject = document.querySelector('.compGroup1 input[type="checkbox"]:checked');
                        let group1SubjectName = group1CheckedSubject ? group1CheckedSubject.parentElement.innerText.split('-')[0].trim().toLowerCase() : '';

                        if (restrictedSubjects.includes(selectedSubjectName) && selectedSubjectName === group1SubjectName) {
                            swal({
                                title: "Invalid Selection",
                                text: `Subject '${selectedSubjectName.charAt(0).toUpperCase() + selectedSubjectName.slice(1)}' cannot be selected in both Compulsory Groups.`,
                                icon: "error",
                                button: "OK"
                            });
                            this.checked = false; // Uncheck the newly selected subject
                            return;
                        }

                        // Uncheck other subjects in Group 2
                        document.querySelectorAll('.compGroup2 input[type="checkbox"]').forEach(otherCb => {
                            if (otherCb !== this) otherCb.checked = false;
                        });
                    }
                });
            });

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
                            //this.checked = false; // Uncheck the one that exceeded the limit
                            //this.removeAttribute('checked');
                        }
                    });
                });

            let additionalCheckboxes = document.querySelectorAll('.additionalSubject input[type="checkbox"]');
            additionalCheckboxes.forEach(cb => {
                let wasChecked = cb.checked;
                cb.addEventListener('mousedown', function () {
                    wasChecked = cb.checked;
                });
                cb.addEventListener('change', function (e) {
                    if (wasChecked && !cb.checked && (ExamTypeId === 2 || ExamTypeId === 6)) {
                        cb.checked = true;
                        return;
                    }
                    if (cb.checked) {
                        additionalCheckboxes.forEach(otherCb => {
                            if (otherCb !== cb) otherCb.checked = false;
                        });
                    }
                });
            });

            let vocationalCheckboxes = document.querySelectorAll('.VocationalSubjects input[type="checkbox"]');
            vocationalCheckboxes.forEach(cb => {
                let wasChecked = cb.checked;
                cb.addEventListener('mousedown', function () {
                    wasChecked = cb.checked;
                });
                cb.addEventListener('change', function (e) {
                    const vocationalCountLimit = 2;
                    const currentVocationalStudentsCount = parseInt(document.getElementById('<%= hfVocationalSubjectCount.ClientID %>').value);

                    if (wasChecked && !cb.checked && (ExamTypeId === 2 || ExamTypeId === 6)) {
                        cb.checked = true;
                        return;
                    }

                    if (currentVocationalStudentsCount >= vocationalCountLimit) {
                        if (cb.checked) {
                            swal({
                                title: "Limit Reached",
                                text: "Only " + vocationalCountLimit + " students can apply for vocational subjects in this faculty and college.",
                                icon: "error",
                                button: "OK"
                            });
                            cb.checked = false;
                        }
                    } else {
                        if (cb.checked) {
                            vocationalCheckboxes.forEach(otherCb => {
                                if (otherCb !== cb) otherCb.checked = false;
                            });
                        }
                    }
                });
            });
            return true;
        }
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
        const selected = [];
        const checkboxes = document.querySelectorAll('input.additionalSubject[type="checkbox"]');

        checkboxes.forEach(cb => {
            if (cb.checked) {
                selected.push(cb.value.trim());
            }
        });
        document.getElementById('<%= hfAdditionalSubjects.ClientID %>').value = selected.join(',');
    }--%>
     function storeAdditionalSelections() {
         const hf = document.getElementById('<%= hfAdditionalSubjects.ClientID %>');
         if (!hf) return;   // <-- prevents null crash

         const selected = [];
         document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
             if (cb.checked) selected.push(cb.value.trim());
         });

         hf.value = selected.join(',');
     }
     function restoreAdditionalSelections() {
         const hf = document.getElementById('<%= hfAdditionalSubjects.ClientID %>');
         if (!hf) return;   // <-- prevents error

         const stored = hf.value;
         if (!stored) return;

         const selectedCodes = stored.split(',').map(s => s.trim().toLowerCase());
         document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
             const code = cb.value.trim().toLowerCase();
             if (selectedCodes.includes(code)) {
                 cb.checked = true;
             }
         });
     }

   <%--  function restoreAdditionalSelections() {
         const stored = document.getElementById('<%= hfAdditionalSubjects.ClientID %>').value;
     if (!stored) return;

     const selectedCodes = stored.split(',').map(s => s.trim().toLowerCase());
     document.querySelectorAll('.additionalSubject input[type="checkbox"]').forEach(cb => {
         const code = cb.value.trim().toLowerCase();
         if (selectedCodes.includes(code)) {
             cb.checked = true;
         }
     });
 }--%>

    document.addEventListener('DOMContentLoaded', function () {
        restoreAdditionalSelections();
        const form = document.forms[0];
        if (form) {
            form.addEventListener('submit', function () {
                storeAdditionalSelections();
            });
        }
    });

    document.addEventListener('DOMContentLoaded', function () {
        const ExamTypeId = parseInt(document.getElementById('<%= hnd_extype.ClientID %>').value);
        const isLocked = '<%= ViewState["IsLocked"] %>' === 'True';
        const allCheckboxes = document.querySelectorAll(
            '.compGroup1 input[type="checkbox"], ' +
            '.compGroup2 input[type="checkbox"], ' +
            'input.electiveSubject, ' +
            'input.electiveVoc1, ' +
            'input.electiveVoc2, ' +
            '.additionalSubject input[type="checkbox"], ' +
            '.VocationalSubjects input[type="checkbox"]'
        );
        allCheckboxes.forEach(cb => {
            cb.dataset.initialChecked = cb.checked ? 'true' : 'false';
        });

        allCheckboxes.forEach(cb => {
            cb.addEventListener('click', function (e) {
                const wasChecked = cb.dataset.initialChecked === 'true';
                const ExamCorrectionForm = parseInt(document.getElementById('<%= hnd_ExamCorrectionForm.ClientID %>').value);
                const StudentExamRegForm = parseInt(document.getElementById('<%= hnd_StudentExamRegForm.ClientID %>').value);
                const FacultyId = parseInt(document.getElementById('<%= hnd_FacultyId.ClientID %>').value);

                if (ExamTypeId === 1 && ExamCorrectionForm == "ExamCorrectionForm") {
                    return;  // allow free checking
                }
                if (ExamTypeId === 3 && isLocked) {
                    e.preventDefault();
                    return false;
                }
                //if (FacultyId === 4) {
                //    e.preventDefault();
                //    return false;
                //}
                if ([2, 5, 6].includes(ExamTypeId)) {
                    e.preventDefault();
                    return false;
                }
               
            });
        });
    });
 </script>


</asp:Content>

