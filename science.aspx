<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="science.aspx.cs" Inherits="science" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-group mt-4 text-center">
            <asp:Button ID="btnDownload" runat="server" class="btn btn-warning mr-2" Text="Download Registration Form (PDF)" OnClick="btnDownload_Click" />
    </div>
</asp:Content>