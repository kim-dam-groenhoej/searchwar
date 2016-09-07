<%@ Page Title="404" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="404.aspx.cs" Inherits="error404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DataContentPlaceHolder" Runat="Server">

[<asp:HyperLink ID="HyperUrlError" runat="server"></asp:HyperLink>]
<asp:Label ID="LblErrorMessage" runat="server"> findes ikke på serveren.</asp:Label>
<br /><br />
<asp:Label ID="LblHelper" runat="server">Mente du?</asp:Label>
<br />
    <asp:GridView ID="GridViewHelper" GridLines="None" ShowHeader="false" runat="server">
        <Columns>
           <asp:TemplateField>
               <ItemTemplate>
               
                   <asp:HyperLink ID="HyperHelper" NavigateUrl='<%# Eval("Url") %>' runat="server"><%# Eval("Title") %></asp:HyperLink><br />
               
               </ItemTemplate>
           </asp:TemplateField>
        </Columns>
    <EmptyDataTemplate>
        Ingen resultater fundet.
    </EmptyDataTemplate>
    </asp:GridView>
<br />
<asp:Label ID="LblContact" runat="server">Kontakt</asp:Label>
<asp:HyperLink ID="HyperContact" runat="server" NavigateUrl='~/Contact.aspx'>Kontakt</asp:HyperLink>
<asp:Label ID="LblContactText" runat="server"> venligst en administrator, hvis du mener denne side burde eksisterer.</asp:Label>

</asp:Content>


