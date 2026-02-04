<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DummyCorrectionDetails.aspx.cs" Inherits="DummyCorrectionDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h4>Exam Report</h4>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlDummyCorrectionDetailstype" runat="server" AutoPostBack="false" CssClass="form-control select2">
                                    <asp:ListItem Value="CORRECTION">Correction Details</asp:ListItem>
                                    <asp:ListItem Value="DOWNLOAD">Dummy Download Details</asp:ListItem>
                                    <asp:ListItem Value="PRACTICALADMITCARD">Practical Admit Card</asp:ListItem>
                                      <asp:ListItem Value="THEORYADMITCARD">Theory Admit Card</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4">
                        <asp:Button ID="ViewCorrectiondata" runat="server" Text="VIEW RECORD" CssClass="btn btn-primary mr-2" OnClick="ViewCorrectiondata_Click" />
                    </div>

                    <asp:Repeater ID="rptCorrection" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>Student Name Change</th>
                                        <th>Father Name Change</th>
                                        <th>Mother Name Change</th>
                                        <th>DOB Change</th>
                                        <th>Gender Change</th>
                                        <th>Caste Change</th>
                                        <th>Religion Change</th>
                                        <th>Faculty Change</th>
                                        <th>Exam Type Change</th>
                                        <th>Total Changes</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("STUDENTNAMECHANGE") %></td>
                                <td><%# Eval("FATHERNAMECHANGE") %></td>
                                <td><%# Eval("MOTHERNAMECHANGE") %></td>
                                <td><%# Eval("DOBCHANGE") %></td>
                                <td><%# Eval("GENDERCHANGE") %></td>
                                <td><%# Eval("CASTECHANGE") %></td>
                                <td><%# Eval("RELIGIONCHANGE") %></td>
                                <td><%# Eval("FACULTYCHANGE") %></td>
                                <td><%# Eval("EXAMTYPECHANGE") %></td>
                                <td><%# Eval("TOTALCHANGES") %></td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </tbody>
        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptDownload" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>College Dummy Download</th>
                                        <th>Student Dummy Download</th>
                                        <th>Total Dummy Download</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("CollegeDummyDownload") %></td>
                                <td><%# Eval("StudentDummyDownload") %></td>
                                <td><%# Eval("TotalDummyDownload") %></td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </tbody>
        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptPracticalAdmitCard" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>PracticalAdmit Card Download</th>
                                       
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                               <td style="text-align:center; font-weight:bold;">
    <%# Eval("PracticalAdmitCardDownload") %>
</td>

                               
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                            </table>
                            </FooterTemplate>
                    </asp:Repeater>

                                        <asp:Repeater ID="rptTheoryAdmitCard" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>TheoryAdmit Card Download</th>
                                       
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                               <td style="text-align:center; font-weight:bold;">
    <%# Eval("TheoryAdmitCardDownload") %>
</td>

                               
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
                            </table>
                            </FooterTemplate>
                    </asp:Repeater>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

