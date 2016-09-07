<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebNewestSearchs.ascx.cs" Inherits="WebControls_WebNewestSearchs" %>
  <asp:Repeater ID="RNewestMatchSearchs" runat="server" 
    onitemdatabound="BoundData">
    <ItemTemplate>
      
      
      <%-- Style/structure of match box (START) --%>
      <asp:HyperLink ID="HyperMatch" runat="server" CssClass="HyperMatch" NavigateUrl='gohere.aspx'>
        
        <asp:Panel ID="PnlHeadDate" CssClass="PnlHeadDate" runat="server">
              <asp:Label ID="LblFromDate" CssClass="LblFromDate" runat="server"></asp:Label>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlMatchResult" CssClass="PnlMatchInfoBox" runat="server">
          
            <asp:Panel ID="PnlMatchClanDetails" CssClass="PnlMatchDetails" runat="server">
              
              <asp:Label ID="LblHeadClanName" meta:resourcekey="LblHeadClanName" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblClanName" runat="server" meta:resourcekey="LblNoInfo"></asp:Label><br />
              <asp:Label ID="LblHeadClanCountry" meta:resourcekey="LblHeadClanCountry" CssClass="LblNewestHead" runat="server"></asp:Label>         <asp:Image ID="imgTLD" CssClass="imgTLD" ImageUrl="~/flags/eg.gif" runat="server" style="margin-right: 5px;" /> <asp:Label ID="LblClanCountry" meta:resourcekey="LblNoInfo" runat="server"></asp:Label><br />
              <asp:Label ID="LblHeadSearchPlayers" meta:resourcekey="LblHeadSearchPlayers" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblSearchPlayers" runat="server" meta:resourcekey="LblNoInfo"></asp:Label>
              
            </asp:Panel>
          
        </asp:Panel>
        <asp:Label ID="LblGameAndMode" CssClass="LblGameAndMode" runat="server" meta:resourcekey="LblNoInfo"></asp:Label>
        
      </asp:HyperLink>
      <%-- Style/structure of match box (END) --%>
 
    </ItemTemplate>
  </asp:Repeater>
  
  <script type="text/javascript">

        function OnMouseHover(element) {
        
          if ($(element).find("h4").css("text-decoration") == "underline") {
            $(element).find("h4").css("text-decoration", "none");
            $(element).find(".PnlNewestInfoBox").css("border", "1px solid #999").css("background-color", "#DDDDDD");
            $(element).find(".LblNewestHead").css("color", "#616");
          } else {
            $(element).find("h4").css("text-decoration", "underline");
            $(element).find(".PnlNewestInfoBox").css("border", "1px solid #000").css("background-color", "lightgreen");
            $(element).find(".LblNewestHead").css("color", "#000");
          }

        }
         
  </script>
  
  <asp:Label ID="LblNullNewestSearchs" runat="server" Visible="false" meta:resourcekey="LblNullNewestSearchs" style="display: block; margin-bottom: 10px;"></asp:Label>