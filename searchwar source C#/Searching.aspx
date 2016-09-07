<%@ Page Title="" Language="C#" EnableSessionState="False" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Searching.aspx.cs" Inherits="Searching" %>

<%@ Register src="WebControls/MyMessageBox.ascx" tagname="MyMessageBox" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cj" Runat="Server">
    <script type="text/javascript">

      var aspxValues = {
        CurretLangId: "<%= new SearchWar.LangSystem.LangaugeSystem().CurrentLangId %>",
        MainUrl: "<%= ResolveUrl("~/") %>",
        ImgWarningUrl: "<%= ResolveUrl("~/images/messagebox/warning.png.ashx").ChangeToImageHost() %>",
        ImgErrorUrl: "<%= ResolveUrl("~/images/messagebox/error.png.ashx").ChangeToImageHost() %>"
      };
      
      var aspxControls = {
        PnlFoundMatchs: "#" + "<%= PnlFoundMatchs.ClientID %>",
        PnlMatchStyle: "#" + "<%= PnlMatchStyle.ClientID %>",
        HyperMatch: "#" + "<%= HyperMatch.ClientID %>",
        PnlLoading: "#" + "<%= PnlLoading.ClientID %>",
        LblSearching: "#" + "<%= LblSearching.ClientID %>",
        H2CountResults: "#" + "<%= H2CountResults.ClientID %>",
        ImgLoading: "#" + "<%= ImgLoading.ClientID %>"
      };
    
         
        function parseXml(xml)
        {
          if (typeof xml == "string")
          {
          
              var startxml = xml;

              if(window.ActiveXObject && window.GetObject)
              {
                  xml = new ActiveXObject("Microsoft.XMLDOM");
                  xml.async = false;
                  xml.loadXML(startxml);
              }

	          return xml;
          } else {
            return xml;
          }

        }
      
      var PnlFoundMatchs = aspxControls.PnlFoundMatchs;
      var PnlMatchStyle = aspxControls.PnlMatchStyle;
      var HyperMatch = aspxControls.HyperMatch;
      var PnlLoading = aspxControls.PnlLoading;
      var CurrentLangId = aspxValues.CurretLangId;
      var LblSearching = aspxControls.LblSearching;
      var H2CountResults = aspxControls.H2CountResults;
      var ImgLoading = aspxControls.ImgLoading;
      var CountResults = 1;
      var MsgToUsers = "";
      var CreateMatchUrl = "error.aspx";
      var maxNumberOfResults = 11;
      var itemIds = new Array();
      var listOfSearchWarIds = new Array();
      
      
      var CreateMatchUrl = "<%= xmlSearchUrlaction %>";

      function Searching() {

          $.ajax({
            type: "GET",
            url: CreateMatchUrl,
            cache: false,
            contentType: "application/rss+xml; charset=utf-8",
            timeout: 300000,
            async: true,
            crossDomain: true,
            global: false,
            dataType: (jQuery.browser.msie) ? "text" : "xml",           
            error: function(XMLHttpRequest, textStatus, errorThrown) {

                  console.log(textStatus + ":" + errorThrown);
                 
                 setTimeout('Searching()', 10000);
            },
            success: function(xmlData) {
//                  
//                  CreateMatchUrl = "<%= xmlSearchUrl %>";

//                  if (jQuery.browser.msie) {
//                      xmlData = parseXml(xmlData);
//                  }


//                  if (xmlData != null) {
//                      
//                      xmlData = $(xmlData).find("search");

//                      var number = 1;
//                      var itemData = "";
//                      
//                      var resultStatusBool = xmlData.find("status").attr("bool");
//                      var resultStatus = xmlData.find("status").text();
//                      
//                      itemIds = new Array(maxNumberOfResults);
//                      
//                      if (resultStatusBool.toLowerCase() == "false") {
//                          
//                          if (resultStatus == "NoResults") {
//                              MsgToUsers = "<%= GetLocalResourceObject("LblSearchingNoResult.Text") %>";
//                          }
//                          
//                          if (resultStatus == "FromDateIsSmallerThanDateNow") {
//                              $(ImgLoading).attr("src", aspxValues.ImgWarningUrl);
//                              MsgToUsers = "<%= GetLocalResourceObject("LblSearchingErrorFromDateIsSmallerThanDateNow.Text") %>";
//                              $(PnlLoading).show('highlight');
//                              
//                              // clear
//                              $(PnlFoundMatchs).html("");
//                          }
//                          
//                          if (resultStatus == "FromDateIsHigherThanToDate") {
//                              $(ImgLoading).attr("src", aspxValues.ImgWarningUrl);
//                              MsgToUsers = "<%= GetLocalResourceObject("LblSearchingErrorFromDateIsHigherThanToDate.Text") %>";
//                              $(PnlLoading).show('highlight');
//                              
//                              // clear
//                              $(PnlFoundMatchs).html("");
//                          }
//                          
//                          if (resultStatus == "FromTimeIsSmallerThanTimeNow") {
//                              $(ImgLoading).attr("src", aspxValues.ImgWarningUrl);
//                              MsgToUsers = "<%= GetLocalResourceObject("LblSearchingErrorFromTimeIsSmallerThanTimeNow.Text") %>";
//                              $(PnlLoading).show('highlight');
//                              
//                              // clear
//                              $(PnlFoundMatchs).html("");                   
//                          }
//                          
//                          if (resultStatus == "FromTimeIsHigherThanToTime") {
//                              $(ImgLoading).attr("src", aspxValues.ImgWarningUrl);
//                              MsgToUsers = "<%= GetLocalResourceObject("LblSearchingErrorFromTimeIsHigherThanToTime.Text") %>";
//                              $(PnlLoading).show('highlight');
//                              
//                              // clear
//                              $(PnlFoundMatchs).html("");
//                          }
//                          
//                      } else {                    
//                        setTimeout('Searching()', 100);
//                      }
//                      
//                      if (CountResults >= maxNumberOfResults) {
//                        
//                        if (resultStatusBool.toLowerCase() == "true") {
//                          $(PnlLoading).hide('slow');
//                          
//                          setTimeout(function() {
//                            Searching(3);
//                          }, 3000);
//                          
//                          // bug fix for not using action 1!
//                          resultStatusBool = "true";
//                          resultStatus = "UpdatingUserActivity";
//                        }

//                      } else {
//                        
//                        if (resultStatusBool.toLowerCase() == "true") {
//                            
//                            MsgToUsers = "<%= GetLocalResourceObject("LblSearchingMore.Text") %>";
//                            
//                            $(xmlData).find("is i").each(function()
//                            {
//                              
//                              var d = $(this);
//                              getSearchMatchId = d.attr("id");
//                
//                              // dont add if its in array
//                              if (($.inArray(getSearchMatchId, listOfSearchWarIds) > -1) == false) {
//                                  // insert in array of matchids
//                                  listOfSearchWarIds.push(getSearchMatchId);
//                                  
//                                  
//                                  getClanName = d.find("cn").text();
//                                  getClanContinent = d.find("ct").text();
//                                  getClanCountry = d.find("cy").text();
//                                  getClanTLD = d.find("cy").attr("t");
//                                  getClanSkill = d.find("cs").text();
//                                  getSearchContinent = d.find("st").text();
//                                  getSearchCountry = d.find("sy").text();
//                                  getSearchSkill = d.find("ss").text();
//                                  getSearchGame = d.find("g").text();
//                                  getSearchGameMode = d.find("gt").text();
//                                  getSearchFromDate = d.find("fd").text();
//                                  getSearchxvs = d.find("x").text();
//                                  getSearchvsx = d.find("y").text();
//                                  getSearchMap = d.find("m").text();
//                                  
//	                                var maine = $($(PnlMatchStyle).clone());
//            	                    
//    	                            var e = maine.find(HyperMatch);
//	                                e.attr("id", e.attr("id") + CountResults);
//            	                    
//	                                if (getClanName != "") {
//	                                    e.find("[id*=H3Match]").text(CountResults + ": " + getClanName);
//	                                } else {
//	                                    e.find("[id*=H3Match]").text(CountResults + ": " + e.find("[id*=H3Match]").text());
//	                                }
//	                                e.find("[id*=ImgTLD]").attr("src", aspxValues.MainUrl + "flags/" + getClanTLD.toLowerCase() + ".gif.ashx");
//	                                e.find("[id*=LblFromDate]").text(getSearchFromDate);
//	                                e.find("[id*=LblSearchPlayers]").text(getSearchxvs + " / " + getSearchvsx);
//	                                e.find("[id*=LblGameAndMode]").text(getSearchGame);
//	                                if (getSearchGameMode != "") {
//	                                    e.find("[id*=LblGameAndMode]").text(e.find("[id*=LblGameAndMode]").text() + getSearchGameMode);
//	                                }
//	                                if (getClanSkill != "") {
//	                                    e.find("[id*=LblClanSkill]").text(getClanSkill);
//	                                }
//	                                if (getClanCountry != "") {
//	                                    e.find("[id*=LblClanCountry]").text(getClanCountry);
//	                                }
//	                                if (getSearchMap != "") {
//	                                    e.find("[id*=LblMap]").text(getSearchMap);
//	                                }
//	                                if (getSearchSkill != "") {
//	                                    e.find("[id*=LblSearchSkill]").text(getSearchSkill);
//	                                }
//	                                if (getSearchCountry != "") {
//	                                    e.find("[id*=LblSearchCountry]").text(getSearchCountry);
//	                                }
//            	                    
//	                                // write html
//	                                itemData = itemData + maine.html();
//                                  
//                                  // update count number
//                                  UpdateCountResults();
//                                  
//                                  itemIds[number] = e.attr("id");
//                                  
//                                  number = number + 1;
//                              }
//                            });
//                            
//                            // write all html data
//                            var data = $(PnlFoundMatchs).append(itemData);
//                            
//                            var allMatchsElements = $(itemIds);
//                            
//                            // show all elements
//                            number = 1;
//                            $.each(allMatchsElements, function(i, o) {
//                              if (1 != CountResults) {
//                                if (o != null) {
//                                  
//                                  var em = $(PnlFoundMatchs + " #" + o);

//                                    // Set color on the first result!
//                                    if (em.attr("id").indexOf("1") != -1) {
//                                      em.find(".PnlMatchInfoBox").css("background-color", "lightgreen");
//                                      em.find("h2").css("color", "#000");
//                                      em.find(".LblFromDate").css("color", "yellow");
//                                      em.find(".LblGameAndMode").css("color", "#5e5e5e");
//                                      em.find(".LblNewestHead").css("color", "#000");
//                                        
//                                       setTimeout(function() {
//                                          em.show('highlight',{},500);
//                                        }, (400 * number));
//                                      
//                                    } else {   
//                                    
//                                        setTimeout(function() {
//                                          em.show('highlight',{},500);
//                                        }, (400 * number));
//                                        
//                                    }
//                                    
//                           
//                                    em.hover(function() {
//                                        var eh = $(this);
//                                        eh.find(".PnlMatchInfoBox").css("background-color", "lightgreen");
//                                        eh.find("h2").css("color", "#000");
//                                        eh.find(".LblFromDate").css("color", "yellow");
//                                        eh.find(".LblGameAndMode").css("color", "#5e5e5e");
//                                        eh.find(".LblNewestHead").css("color", "#000");
//                                    },
//                                       function() {
//                                           var eh = $(this);
//                                           eh.find(".PnlMatchInfoBox").css("background-color", "#ddd");
//                                           eh.find(".LblFromDate").css("color", "#4c6aaa");
//                                           eh.find("h2").css("color", "#4c6aaa");
//                                           eh.find(".LblGameAndMode").css("color", "#999");
//                                           eh.find(".LblNewestHead").css("color", "#616");
//                                    });
//                                    
//                                    number = number + 1;
//                    	          }
//                    	        }
//                      	      
//                            });
//                            
//                        }
//                      }
//                      
//                  } else {
//                          $(ImgLoading).attr("src", aspxValues.ImgErrorUrl);
//                          $(PnlLoading).show('highlight');
//                          MsgToUsers = "<%= GetLocalResourceObject("LblSearchingError.Text") %>";
//                          
//                          // clear
//                          $(PnlFoundMatchs).html("");
//                  }
//                  
//                  // update loading
//                  $(LblSearching).text(MsgToUsers);

                }
                
          }); 
          
          
      }
      
      function UpdateCountResults() {
        CountResults = CountResults + 1;
        
        $(H2CountResults).text((CountResults - 1) + " " + "<%= GetLocalResourceObject("H2CountResults.InnerText") %>");
      }
      
      function OpenDefaultUrl() {
      
        window.location = "<%= ResolveUrl("~/") %>";

      }
      
      $(window).load(function(){

        $(PnlLoading).show();
        Searching();
        
      });
      
      
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" Runat="Server">

<asp:Panel ID="PnlSearchResults" CssClass="PnlSearchResults" runat="server">

  <h4 id="H2CountResults" runat="server" meta:resourcekey="H2CountResults" style="margin-top: 2px; cursor: default; color: #999; float: right;"></h4>
  <h1 id="H1Results" runat="server" meta:resourcekey="H1Results"></h1>
  <%--
  <uc1:MyMessageBox ID="MmbDataError" runat="server" />--%>
  
  <asp:Panel ID="PnlFoundMatchs" runat="server">
  
  </asp:Panel>
  
  <asp:Panel ID="PnlMatchStyle" runat="server" style="display: none;">
        

      <%-- Style/structure of match box (START) --%>
      <asp:HyperLink ID="HyperMatch" runat="server" CssClass="HyperMatch" NavigateUrl='gohere.aspx' style="display: none;">
        <asp:Image ID="ImgTLD" CssClass="imgTLD" ImageUrl="~/flags/eg.gif" runat="server" style="margin-right: 5px;" /> <h2 id="H3Match" meta:resourcekey="H3NoInfo" runat="server" style="display: inline;"></h2>
        
        <asp:Panel ID="PnlHeadDate" CssClass="PnlHeadDate" runat="server">
              <asp:Label ID="LblFromDate" CssClass="LblFromDate" runat="server"></asp:Label>
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlMatchResult" CssClass="PnlMatchInfoBox" runat="server">
          
            <asp:Panel ID="PnlMatchClanDetails" CssClass="PnlMatchDetails" runat="server">
              
              <asp:Label ID="LblHeadSearchPlayers" meta:resourcekey="LblHeadSearchPlayers" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblSearchPlayers" runat="server" meta:resourcekey="LblNoInfo"></asp:Label><br />
              <asp:Label ID="LblHeadClanSkill" meta:resourcekey="LblHeadClanSkill" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblClanSkill" runat="server" meta:resourcekey="LblNoInfo"></asp:Label><br />
              <asp:Label ID="LblHeadClanCountry" meta:resourcekey="LblHeadClanCountry" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblClanCountry" meta:resourcekey="LblNoInfo" runat="server"></asp:Label>
              
            </asp:Panel>
            
            <asp:Panel ID="PnlMatchClanSearch" CssClass="PnlMatchDetails" runat="server">
              
              <asp:Label ID="LblHeadMap" meta:resourcekey="LblHeadMap" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblMap" runat="server" meta:resourcekey="LblNoInfo"></asp:Label><br />
              <asp:Label ID="LblHeadSearchSkill" meta:resourcekey="LblHeadSearchSkill" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblSearchSkill" runat="server" meta:resourcekey="LblNoInfo"></asp:Label><br />
              <asp:Label ID="LblHeadSearchCountry" meta:resourcekey="LblHeadSearchCountry" CssClass="LblNewestHead" runat="server"></asp:Label> <asp:Label ID="LblSearchCountry" runat="server" meta:resourcekey="LblNoInfo"></asp:Label>
              
            </asp:Panel>
          
        </asp:Panel>
        <asp:Label ID="LblGameAndMode" CssClass="LblGameAndMode" runat="server" meta:resourcekey="LblNoInfo"></asp:Label>
        
      </asp:HyperLink>
      <%-- Style/structure of match box (END) --%>
    
  </asp:Panel>
  
  <asp:Panel ID="PnlLoading" runat="server" style="position: relative; display: block; width: 100%; overflow: hidden;">
    <asp:Image ID="ImgLoading" runat="server" ImageUrl='~/images/search.gif.ashx' style="float: left; margin: 4px;" />
    <asp:Label ID="LblSearching" Font-Bold="true" meta:resourcekey="LblSearching" runat="server" style="float: left; margin: 16px 0 10px 10px;"></asp:Label>
    
    <input type="button" enableviewstate="false" id="BtnNewSearch" runat="server" meta:resourcekey="BtnNewSearch" onclick='OpenDefaultUrl();' style="float: right; margin: 8px 10px 0 10px; width: auto; height: auto; padding: 3px;" />

  </asp:Panel>
  
</asp:Panel>



<asp:Panel ID="PnlMessageContent" CssClass="PnlMessageContent" runat="server">
  
  <h3 id="H3Messages" runat="server" meta:resourcekey="H3Messages"></h3>
  
</asp:Panel>

</asp:Content>

