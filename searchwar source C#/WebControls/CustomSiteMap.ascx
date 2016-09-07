<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomSiteMap.ascx.cs" ClassName="CustomSiteMap" Inherits="CustomSiteMap" %>

<ul id="ULBlindHelper" runat="server" style="padding: 0px; margin: 0px;">
  <asp:Repeater ID="RSiteMap" runat="server" 
      onitemdatabound="CheckCurrentPath">
      <ItemTemplate>
              <li id="LiNode" runat="server" class="LiCusteomSiteMapNode">
                <asp:HyperLink ID="HyperNodePath" runat="server"><%# Eval("SiteMapNodeTitle") %></asp:HyperLink>
              </li>
      </ItemTemplate>
  </asp:Repeater>
</ul>
