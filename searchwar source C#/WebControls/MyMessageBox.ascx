<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyMessageBox.ascx.cs"
    Inherits="MyMessageBox" %>

<div id="containter" class="container" runat="server" visible="false">
    <asp:Panel ID="MessageBox" runat="server">
        <asp:HyperLink runat="server" id="CloseButton" style="display: block; float: right; padding: 10px; cursor:pointer; height: 10px; width: 10px;">
            <asp:Image runat="server" ImageUrl="~/images/messagebox/close.png" AlternateText="Click here to close this message" style="height: 10px; width: 10px;" />
        </asp:HyperLink>
        <p>
          <asp:Literal ID="litMessage" runat="server"></asp:Literal>
        </p>
            
        
    </asp:Panel>
</div>