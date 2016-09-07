<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register src="~/WebControls/WebNewestSearchs.ascx" tagname="WebNewestSearchs" tagprefix="uc1" %>


<%@ Register src="~/WebControls/WebSearch.ascx" tagname="WebSearchl" tagprefix="uc2" %>


<%@ Register src="WebControls/Weblogin.ascx" tagname="Weblogin" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="h" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cj" Runat="Server">
    
    <script type="text/javascript">
    
      var aspxControlIds = {
        ddlXvsX: "<%= "#" + Wsc.FindControl("DdlSearchXvsX").ClientID %>",
        GetGameTypeControl: "<%= "#" + Wsc.FindControl("DdlSearchGameType").ClientID %>",
        ddlSearchGame: "<%= "#" + Wsc.FindControl("DdlSearchGame").ClientID %>",
        DdlClanSkill: "<%= "#" + Wsc.FindControl("DdlClanSkill").ClientID %>",
        DdlSearchSkill:  "<%= "#" + Wsc.FindControl("DdlSearchSkill").ClientID %>",
        hfClanCountry: "<%= "#" + Wsc.FindControl("HfClanCountry").ClientID %>",
        hfSearchCountry: "<%= "#" + Wsc.FindControl("HfSearchCountry").ClientID %>",
        hfSearchGameType: "<%= "#" + Wsc.FindControl("HfSearchGameType").ClientID %>",
        ClanCountry: "<%= "#" + Wsc.FindControl("DdlClanCountry").ClientID %>",
        SearchCountry: "<%= "#" + Wsc.FindControl("DdlSearchCountry").ClientID %>",
        PnlNewestBlog: "<%= "#" + PnlNewestBlog.ClientID %>",
        PnlInfoContent: "<%= "#" + PnlInfoContent.ClientID %>",
        PnlSearchContent: "<%= "#" + PnlSearchContent.ClientID %>",
        DdlClanContinent: "<%= "#" + Wsc.FindControl("DdlClanContinent").ClientID %>",
        DdlSearchContinent: "<%= "#" + Wsc.FindControl("DdlSearchContinent").ClientID %>",
        txtFromTime: "<%= "#" + Wsc.FindControl("txtFromTime").ClientID %>",
        txtFromDate: "<%= "#" + Wsc.FindControl("txtFromDate").ClientID %>",
        txtClanName: "<%= "#" + Wsc.FindControl("TxtClanName").ClientID %>",
        BtnSearchWar: "<%= "#" + Wsc.FindControl("BtnSearchWar").ClientID %>",
        DivSearchClanwar: "#DivSearchClanwar",
        HyperAdvancedSearch: "<%= "#" + Wsc.FindControl("HyperAdvancedSearch").ClientID %>"
      };

      var aspxValues = {
        CurretLangId: "<%= new SearchWar.LangSystem.LangaugeSystem().CurrentLangId %>",
        Choose: "<%= GetLocalResourceObject("ListItemChooseResource1").ToString() %>",
        MainUrl: "<%= ResolveUrl("~/") %>",
        HyperSearchSimpleLink: "<%= GetLocalResourceObject("HyperSearchSimpleLinkResource1.Text").ToString() %>",
        HyperAdvancedSearch: "<%= GetLocalResourceObject("HyperAdvancedSearchResource1.Text").ToString() %>",
        ImgLoading: "<%= ResolveUrl("~/images/search.gif.ashx").ChangeToImageHost() %>",
        timePickerHourType: <%= _timePickerHourType.ToString().ToLower() %>,
        timePickerCloseText: 'Klik for at luk'
      };
      


      $(document).ready(function() {


        $(aspxControlIds.PnlSearchContent).find("input:text").addClass("ui-widget ui-state-default ui-corner-all");
		
        $(aspxControlIds.ClanCountry).combobox();
        $(aspxControlIds.DdlClanSkill).combobox();
        $(aspxControlIds.DdlClanContinent).combobox({
           selected: function(event, ui) {
              ChangeCountries($(aspxControlIds.DdlClanContinent));
           }
        });
        $(aspxControlIds.SearchCountry).combobox();
        $(aspxControlIds.ddlXvsX).combobox();
        $(aspxControlIds.DdlSearchSkill).combobox();
        $(aspxControlIds.DdlSearchContinent).combobox({
           selected: function(event, ui) {
              ChangeCountries($(aspxControlIds.DdlSearchContinent));
           }
        });
        $(aspxControlIds.ddlSearchGame).combobox({
           selected: function(event, ui) {
              ChangeGameTypes(this);
           }
        });
        $(aspxControlIds.GetGameTypeControl).combobox();

		$(aspxControlIds.txtFromDate).datepicker({
			showWeek: true,
            dateFormat: 'dd/mm/yy',
            altFormat: 'dd/mm/yy',
            minDate: new Date(),
			firstDay: 1
		});

        $("#showSearchSendingData").dialog({
            autoOpen: false,
            modal: true
        });

                            $.validator.addMethod(
                                "dateCheck",
                                function(value, element) {
                                    var splitDateStr = value.split('/');

                                    var fromdate = new Date(splitDateStr[2], parseFloat(splitDateStr[1]) - 1, splitDateStr[0], 23, 59, 59);
                                    var dateNow = new Date();

                                    if (fromdate <= dateNow) {
                                        return false;
                                    }

                                    return true;
                                },
                                "Please enter a date higher"
                            );

                            $.validator.addMethod(
                                "timeCheck",
                                function(value, element) {
                                    var splitDateStr = $(aspxControlIds.txtFromDate).val().split('/');
                                    var splitTimeStr = value.split(':');

                                    if ($("#amorpm").html().toLowerCase() == "pm" && $("#amorpm").is(":visible")) {

                                        splitTimeStr[0] = parseInt(splitTimeStr[0]) + 12;

                                    }

                                    var fromDateTime = new Date(splitDateStr[2], parseFloat(splitDateStr[1]) - 1, splitDateStr[0], splitTimeStr[0], splitTimeStr[1], 59);
                                    var dateNow = new Date();

                                    if (fromDateTime < dateNow) {
                                        return false;
                                    }

                                    return true;
                                },
                                "Please enter a time higher"
                            );
						
							$("#Aspnetform").validate({
                                onkeyup: false,
                                onclick: false,
                                onsubmit: false,
                                focusInvalid: false,
                                onfocusout: false,
                                focusCleanup: false,
                                errorClass: "errorjquery",
                                validClass: "successjquery",
                                ignore: ".ignore",
								invalidHandler: function(form, validator) {
									
								},
                                errorPlacement: function(error,element) {

                                },
                                  highlight: function(element, errorClass, validClass) {
                                     $(element).parent().find("." + errorClass).remove();
                                     $(element).parent().find("." + validClass).remove();
                                     $(element).parent().find(".unknownjquery").remove();
                                     
                                     if ($(element).is("select")) {
                                        $(element).parent().find("input").css("background", "#ddd");
                                     }
                                     $(element).css("background", "#ddd");
                                     var ee = $("<label>").insertAfter(element);
                                     ee.attr("title", $(element).attr("title"));
                                     $(ee).addClass(errorClass);
                                     $(ee).addClass("ui-icon ui-icon-alert");
                                  },
                                  unhighlight: function(element, errorClass, validClass) {
                                     $(element).parent().find("." + errorClass).remove();
                                     $(element).parent().find("." + validClass).remove();
                                     $(element).parent().find(".unknownjquery").remove();

                                     if ($(element).is("select")) {
                                        $(element).parent().find("input").css("background", "#fff");
                                     }
                                     $(element).css("background", "#fff");
                                     var ee = $("<label>").insertAfter(element);
                                     ee.attr("title", $(element).attr("title"));
                                     $(ee).addClass(validClass);
                                     $(ee).addClass("ui-icon ui-icon-check");
                                  },
							  rules: {

                              }});

                              

                            $(aspxControlIds.txtFromDate).rules("add", { 
                                dateCheck:true
                            });

                           $(aspxControlIds.txtFromTime).rules("add", { 
                                timeCheck:true
                            });

                              $(".uiBtn").button();




                              $("#DivSearchClanwar").find(":input[type='text']").each(function (i, item) {

                                   var ee = $("<label>").insertAfter(item);
                                   $(ee).addClass("unknownjquery");

                              });
                              

                              var firstFocus = false;
                              $(aspxControlIds.txtClanName).focus(function() {
                                
                                if (firstFocus == false) {
                                    
                                    firstFocus = true;
                                    $(this).val("");
                                }

                              });



                        // search click event
                        $(aspxControlIds.BtnSearchWar).click(function(event) {   
                            var isValid = true;
                            
                            $("#DivSearchClanwar").find(":input[type='text']").each(function (i, item) {
                              if (!$(item).valid()) {
                                isValid = false;
                              }
                            });
                            
                            $("#DivSearchClanwar").find("select").each(function (i, item) {

                              if (!$(item).valid()) {
                                isValid = false;
                              }
                            });
                            
                            if (isValid == false) {
                                
                                event.preventDefault();
                            }
                        });


                        $('#other').click(function() {
                          $('#target').submit();
                        });


					});

                    function ValidateSearch() {       
                        var isValid = true;
                        isValid = ValidateSearchAll();

                        if (isValid == true) {
                            $("#showSearchSendingData").dialog("open");
                        }

                        // is not valid
                        return isValid;
                    }


    </script>
    
    <asp:Literal ID="LJavascriptHolder" runat="server" Mode="PassThrough"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" Runat="Server">

      <div id="PnlSearchContent" class="PnlSearchContent" runat="server">
        

            <uc2:WebSearchl ID="Wsc" runat="server" />
        
            <br />
        
          <div id="PnlNewestBlog" runat="server" class="PnlMarginFix">
          
              <asp:HyperLink ID="HyperBlog" href='gohere.aspx' CssClass="HyperBlog" runat="server">
          
                <h4 ID="H4BlogTitle" runat="server"></h4>
                <asp:Label ID="LblBlogDateAdded" runat="server" ForeColor="#999"></asp:Label> - 
                <asp:Label ID="LblBlogText" runat="server" CssClass="LblBloxText"></asp:Label>
            
                
              </asp:HyperLink>
              <br />
          
          </div>
      </div>
   
      <div id="PnlInfoContent" runat="server" class="PnlInfoContent">
        <div class="PnlMarginFix">
    
          <h3 ID="H3NewestMatchSearch" runat="server" meta:resourcekey="H3NewMatchSearchResource1"></h3>
          <uc1:WebNewestSearchs ID="Wnms" runat="server" />
      
        </div>
    
        	<h3   ID="H3AboutSearchWar" runat="server" meta:resourcekey="H3AboutSearchWarResource1">Om Searchwar.net</h3>
	        <div>
		            <asp:Label ID="LblAboutSearchWar" runat="server" meta:resourcekey="LblAboutSearchWar"></asp:Label>
	        </div>   

        	<h3   ID="H3Signup" runat="server" meta:resourcekey="H3Signup">Opret profil</h3>
	        <div>
		            <asp:Label ID="LblSignupDesc" runat="server" meta:resourcekey="LblSignupDesc"></asp:Label>
	        </div>   

      </div> 


</asp:Content>

