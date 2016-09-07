<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Weblogin.ascx.cs" Inherits="WebControls_Weblogin" %>
<asp:Login ID="UserLogin" meta:resourcekey="UserLogin" RenderOuterTable="false" 
    runat="server" onloggingin="Login">
    <LayoutTemplate>

    <div id="userLogin">

            <table id="TblLogin" style="width: 100%;">
            <tr>
                <td style="width: 30%">
                         <asp:Label ID="UserNameLabel" runat="server" meta:resourcekey="UserNameLabel" AssociatedControlID="UserName"></asp:Label>
                </td>
                <td style="width: 70%">

                                 <asp:TextBox ID="UserName"  ValidationGroup="ctl00$Login1" ClientIDMode="Static" CssClass="ui-widget ui-state-default ui-corner-all" runat="server" style="width: 90%;"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td>
                         <asp:Label ID="PasswordLabel" runat="server" meta:resourcekey="PasswordLabel" AssociatedControlID="Password"></asp:Label>
                </td>
                <td>          
                                <asp:TextBox ID="Password"  ValidationGroup="ctl00$Login1" runat="server"  ClientIDMode="Static"  CssClass="ui-widget ui-state-default ui-corner-all" TextMode="Password" style="width: 90%;"></asp:TextBox>
                </td>
            </tr>
            </table>

            
             <asp:CheckBox ID="RememberMe" runat="server" meta:resourcekey="RememberMe" />
             <div class="errors">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
             </div>
        
        <div style="overflow:hidden;">
            <asp:LinkButton ID="LoginButton" runat="server" CssClass="uiBtn loginBtn"  ValidationGroup="ctl00$Login1" meta:resourcekey="LoginButton" CommandName="Login" Text="Log In"  style="float:left;"></asp:LinkButton>
            <asp:HyperLink ID="HyperCreateUser" runat="server" CssClass="uiBtn" meta:resourcekey="HyperCreateUser" style="float:right;"></asp:HyperLink>
        </div>
        <asp:HyperLink ID="HyperForgotPassword" runat="server" meta:resourcekey="HyperForgotPassword" style="margin: 6px 0 0 0; float: right;"></asp:HyperLink>
    </div>
    </LayoutTemplate>
</asp:Login>
