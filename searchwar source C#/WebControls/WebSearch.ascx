<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="WebSearch.ascx.cs" Inherits="WebControls_WebSearchl" %>

        <div id="DivSearchClanwar" class="ui-dialog-content ui-widget-content">

            <asp:Table ID="TblBoxSearch" CssClass="TblBoxsTheme ui-corner-all" Width="100%" runat="server">
                <asp:TableRow>
                    <asp:TableCell style="width: 38%; ">
                        
                    </asp:TableCell>
                    <asp:TableCell style="width: 62%;">
                        <h1 id="H1SearchInsert" class="searchHead" runat="server" meta:resourcekey="H1SearchInsert"></h1>
                    </asp:TableCell>
                </asp:TableRow>
               <asp:TableRow style="display: none !important;;">
                    <asp:TableHeaderCell CssClass="TblHeadRow" style="width: 38%; display: none !important;">
                
                        <asp:Label ID="LblLastSearch" runat="server" meta:resourcekey="LblLastSearchResource1"></asp:Label>
                
                    </asp:TableHeaderCell>
                    <asp:TableCell CssClass="TblCellRow" style="width: 62%; display: none !important;">
                
                        <asp:DropDownList ID="DdlLastSearch" Enabled="false" runat="server" meta:resourcekey="DDLLastSearchResource1">
                          <asp:ListItem Text="not working"></asp:ListItem>
                        </asp:DropDownList>
                
                    </asp:TableCell>
               </asp:TableRow>
           
               <asp:TableRow CssClass="TrAdvanced">
                    <asp:TableCell ColumnSpan="2">
                    
                        <h2 ID="H2HeadClan" runat="server" meta:resourcekey="H2HeadClanResource1"></h2>
                    
                    </asp:TableCell>
                </asp:TableRow>
            
            
                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblClanName" CssClass="LblHelp" runat="server" meta:resourcekey="LblClanNameResource1"></asp:Label>

               
                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                       <div class="iconWrapper">
                            <span class="icon icon-clanname"></span>
                       </div>
                            <asp:TextBox ID="TxtClanName" minlength="2" CssClass="required" runat="server" meta:resourcekey="TxtClanNameResource1"></asp:TextBox>

                   </asp:TableCell>
                </asp:TableRow>



                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblClanSkill" CssClass="LblHelp" runat="server" meta:resourcekey="LblClanSkillResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                       <div class="iconWrapper">
                            <span class="icon ui-icon ui-icon-star"></span>
                       </div>
                       <asp:DropDownList ID="DdlClanSkill" meta:resourcekey="DDLClanSkillResource1" runat="server">
                       </asp:DropDownList>
                       
                    
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblClanContinent" CssClass="LblHelp" runat="server" meta:resourcekey="LblClanContinentResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                       <div class="iconWrapper">
                            <span class="icon icon-continent"></span>
                       </div>
                         <asp:DropDownList ID="DdlClanContinent" onchange="ChangeCountries(this)" meta:resourcekey="DDLClanContinentResource1" runat="server">
                         </asp:DropDownList>

                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblClanCountry" CssClass="LblHelp" runat="server" meta:resourcekey="LblClanCountryResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell ID="TcClanCountry" CssClass="TblCellRow">
                       <div class="iconWrapper">
                            <span class="icon ui-icon ui-icon-flag"></span>
                       </div>
                           <asp:DropDownList ID="DdlClanCountry" CssClass="required" meta:resourcekey="DDLClanCountryResource1" 
                            runat="server">
                           </asp:DropDownList>
                           <asp:HiddenField ID="HfClanCountry" EnableViewState="false" runat="server" />

                       
                   </asp:TableCell>
                </asp:TableRow>
            
            
                <asp:TableRow CssClass="TrAdvanced">
                    <asp:TableCell ColumnSpan="2">
                    
                        <h2 ID="H2HeadSearch" runat="server" meta:resourcekey="H2HeadSearchResource1"></h2>
                    
                    </asp:TableCell>
                </asp:TableRow>
            
                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblSearchTime" CssClass="LblHelp" runat="server" meta:resourcekey="LblSearchTimeResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                    
                    <div style="width: 359px;">
                        <div style="width: 150px; display: block; float: left;  z-index: 55; position: relative;">
                                   <asp:TextBox ID="txtFromTime" minlength="4" runat="server" CssClass="time required" style="float: left;"></asp:TextBox><span id="amorpm" clientidmode="Static" runat="server"></span>
                               <div class="iconWrapper">
                                    <span class="icon icon-time bugfix"></span>
                               </div>
                        </div>

                        <div style="width: 209px; display: block; float: left; position: relative;">
                               <div class="iconWrapper">
                                    <span class="icon ui-icon ui-icon-calendar"></span>
                               </div>
                                   <asp:TextBox ID="txtFromDate" minlength="4" CssClass="fromdate required" runat="server" style="float: left;"></asp:TextBox>
                        </div>
                        <div style="clear:both; font-size:0;"></div>
                    </div>
                       
                   </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblGame" CssClass="LblHelp" runat="server" meta:resourcekey="LblGameResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
               
                       <div class="iconWrapper">
                            <span class="icon icon-game"></span>
                       </div>
                           <asp:DropDownList ID="DdlSearchGame" CssClass="required" meta:resourcekey="DDLGameResource1" runat="server"></asp:DropDownList>
   
                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblGameType" CssClass="LblHelp" runat="server" meta:resourcekey="LblGameTypeResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell ID="TcClanGameType" CssClass="TblCellRow">
               
                       <div class="iconWrapper">
                            <span class="icon icon-gametype"></span>
                       </div>
                           <asp:DropDownList ID="DdlSearchGameType" onchange="OnChangeDropdownbox(this)" meta:resourcekey="DDLGameTypeResource1" runat="server">
                           </asp:DropDownList>
                           <asp:HiddenField ID="HfSearchGameType" EnableViewState="false" runat="server" />


                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblMap" CssClass="LblHelp" runat="server" meta:resourcekey="LblMapResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                       <div class="iconWrapper">
                            <span class="icon ui-icon ui-icon-plus"></span>
                       </div>

                       <asp:TextBox ID="TxtMap" runat="server" meta:resourcekey="TxtMapResource1"></asp:TextBox>

                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblSearchSkill" CssClass="LblHelp" runat="server" meta:resourcekey="LblSearchSkillResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
               
                       <div class="iconWrapper">
                            <span class="icon ui-icon ui-icon-star"></span>
                       </div>

                           <asp:DropDownList ID="DdlSearchSkill" meta:resourcekey="DDLSearchSkillResource1" runat="server">
                           </asp:DropDownList>


                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblSearchContinent" CssClass="LblHelp" runat="server" meta:resourcekey="LblSearchContinentResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
               
                       <div class="iconWrapper">
                            <span class="icon icon-continent"></span>
                       </div>
                           <asp:DropDownList ID="DdlSearchContinent" onchange="ChangeCountries(this)" meta:resourcekey="DDLSearchContinentResource1" runat="server">
                           </asp:DropDownList>
 
                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="TrAdvanced">
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblSearchCountry" CssClass="LblHelp" runat="server" meta:resourcekey="LblSearchCountryResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell ID="TcSearchCountry" CssClass="TblCellRow">
               
                       <div class="iconWrapper">
                            <span class="icon ui-icon ui-icon-flag"></span>
                       </div>
                           <asp:DropDownList ID="DdlSearchCountry" AutoPostBack="true" meta:resourcekey="DDLSearchCountryResource1" runat="server">
                           </asp:DropDownList>
                           <asp:HiddenField ID="HfSearchCountry" EnableViewState="false" runat="server" />


                       
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                   <asp:TableHeaderCell CssClass="TblHeadRow">
               
                       <asp:Label ID="LblXvsX" CssClass="LblHelp" runat="server" meta:resourcekey="LblXvsXResource1"></asp:Label>

                   </asp:TableHeaderCell>
                   <asp:TableCell CssClass="TblCellRow">
                    
                       <div class="iconWrapper">
                            <span class="icon icon-versus"></span>
                       </div>
                           <asp:DropDownList ID="DdlSearchXvsX" CssClass="required" onchange="ChangevsX(this)" meta:resourcekey="DDLXvsResource1" runat="server">
                           </asp:DropDownList>
                   

                       
                   </asp:TableCell>
                </asp:TableRow>



                <asp:TableRow>
                    <asp:TableHeaderCell>

                    </asp:TableHeaderCell>
                    <asp:TableCell CssClass="TblCellRow">
                
                        <asp:Button ID="BtnSearchWar" runat="server" meta:resourcekey="BtnSearchWarResource1" OnClick="BtnSearchWar_OnClick" />
                        <asp:HyperLink ID="HyperAdvancedSearch" CssClass="hyperAdv" NavigateUrl='javascript:' runat="server" meta:resourcekey="HyperAdvancedSearchResource1"></asp:HyperLink>
              

                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

        </div>

        <div id="showSearchSendingData" title="Sending data">
            <br />
            <center>Please wait... Sending data</center>
        </div>
