<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="CreateUser" %>

<%@ Register src="WebControls/CreateUser.ascx" tagname="CreateUser" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cj" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cp" Runat="Server">

    <div id="PnlSearchContent" class="PnlSearchContent" runat="server">
    <uc1:CreateUser ID="CreateUser1" runat="server" />
</div>
</asp:Content>

