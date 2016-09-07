<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="unknown.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DataContentPlaceHolder" Runat="Server">

<asp:Label ID="LblError" runat="server">Fejl</asp:Label>
<asp:Label ID="LblErrorMessage" runat="server"> En ukendt fejl opstod. Prøv venligst igen.</asp:Label>

<asp:Label ID="LblContact" runat="server">Kontakt</asp:Label>
<asp:HyperLink ID="HyperContact" runat="server" NavigateUrl='~/Contact.aspx'>Kontakt</asp:HyperLink>
<asp:Label ID="LblContactText" runat="server"> venligst en administrator, hvis du stadig har problemer.</asp:Label>

</asp:Content>

