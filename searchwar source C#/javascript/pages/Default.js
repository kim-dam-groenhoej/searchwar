
//<![CDATA[ 
         var CurrentLangId = aspxValues.CurretLangId;
         var LoadingId = "Loading";
         var LoadingTag = $("<img id='"+LoadingId+"' class='loading' src='"+aspxValues.ImgLoading+"' />");
         LoadingId = "#" + LoadingId;
         
         var txtSearchGame = aspxControlIds.txtSearchGame;
         var txtSearchGameType = aspxControlIds.txtSearchGameType;
         
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
         
         function ChangevsX(element) {
            
              var ddlvsX = aspxControlIds.ddlvsX;
         
              $(ddlvsX).val($(element).val()).attr("selected", "selected");
         
         }
         
         /*
                     //#region
             Reading GameTypes after selecting Continent - start
         */
         function ChangeGameTypes(element) {
              var value = $("OPTION:selected", element).val();
              var text = $("OPTION:selected", element).text();
              var GetGameTypeControl = aspxControlIds.GetGameTypeControl;
              var GetTc = $("#" + $(GetGameTypeControl).parent().attr("id"));
              
              if (value == "" || value == null) {
                
                  $(GetGameTypeControl + ">option").remove();
                  $(GetGameTypeControl).prepend("<option value='' selected='selected'>" + aspxValues.Choose + "</option>");

                  // speciel combobox fix
                  $(GetGameTypeControl).attr("disabled", "disabled");
                  $("#" + $(GetGameTypeControl).attr("id") + "input").attr("disabled", "disabled");
                  $("#" + $(GetGameTypeControl).attr("id") + "input").val("");
                  $("#" + $(GetGameTypeControl).attr("id") + "btn").attr("disabled", "disabled");
                  $("#" + $(GetGameTypeControl).attr("id") + "btn").addClass("ui-button-disabled ui-state-disabled");

                  IsClearedGameType = false;
                
              } else {
                GetTc.prepend(LoadingTag);
              
              $(function() {
              
                   $.ajax({
                        type: "GET",
	                      url: aspxValues.MainUrl + "xml.ashx",
	                      cache: false,
	                      contentType: "application/rss+xml; charset=utf-8",
	                      timeout: 3000,
	                      async: true, 
	                      data: "xml=gt&gi=" + value,
	                      dataType: (jQuery.browser.msie) ? "text" : "xml",
	                      
	                      error: function(XMLHttpRequest, textStatus, errorThrown) {
          	                 $(GetGameTypeControl + ">option").remove();
                             $(GetGameTypeControl).prepend("<option value='' selected='selected'>" + aspxValues.Choose + "</option>");
                             
                             // speciel combobox fix
                             $(GetGameTypeControl).attr("disabled", "disabled");
                             $("#" + $(GetGameTypeControl).attr("id") + "input").attr("disabled", "disabled");
                             $("#" + $(GetGameTypeControl).attr("id") + "input").val("");
                             $("#" + $(GetGameTypeControl).attr("id") + "btn").attr("disabled", "disabled");
                             $("#" + $(GetGameTypeControl).attr("id") + "btn").addClass("ui-button-disabled ui-state-disabled");
                             
                              IsClearedGameType = false;
                             
          	                 GetTc.find(LoadingId).remove();
                        },

	                      success: function(xmlData) {
    	                             
                            xmlData = parseXml(xmlData);
                            
                            if (xmlData != null) {
                                
                                /*
                                   Clear ClanCountry
                               */
                                $(GetGameTypeControl + ">option").remove();
                                
                                
                                var theGameType = null;
                                var theId = null;
                                var theitem = null;
	                              $("gts gt", xmlData).each(function(g)
	                              {
	                                theitem = $(this);
		                              theGameType = theitem.find("gn").text();
		                              theId = theitem.attr("id");
          		                    
		                              $(GetGameTypeControl).append("<option value='"+theId+"'>"+theGameType+"</option>");
                              
	                              });

	                              $(GetGameTypeControl).prepend("<option value='' selected='selected'>" + aspxValues.Choose + "</option>");

                                  // speciel combobox fix
                                $("#" + $(GetGameTypeControl).attr("id") + "input").removeAttr("disabled");
                                $("#" + $(GetGameTypeControl).attr("id") + "input").val("");
                                $("#" + $(GetGameTypeControl).attr("id") + "btn").removeClass("ui-state-disabled");
                                $("#" + $(GetGameTypeControl).attr("id") + "btn").removeClass("ui-button-disabled");
                                $("#" + $(GetGameTypeControl).attr("id") + "btn").removeAttr("disabled");
                                $("#" + $(GetGameTypeControl).attr("id") + "btn").hover(function() {
                                    $(this).addClass("ui-state-hover");
                                });
                                $("#" + $(GetGameTypeControl).attr("id") + "btn").mouseleave(function() {
                                    $(this).removeClass("ui-state-hover");
                                });
                                

                                GetTc.find(LoadingId).remove();

          	                } else {
          	                    GetTc.find(LoadingId).remove();
          	                }
    	                  }
	                });
	            });
	            } 
              
         }
         /*
             Reading GameTypes after selecting Continent - END
             //#endregion
         */
         
         /*
                     //#region
             search games - start
         */
         var games = null;
         var gametypes = null;
         var ddlSearchGame = aspxControlIds.ddlSearchGame;
         var GetGameTypeControl = aspxControlIds.GetGameTypeControl;
         var lastgamevalue = null;
         function SearchDDL(element) {

              var SearchControl = null;
              
              if ($(element).attr("id") == $(txtSearchGame).attr("id")) {
                  SearchControl = ddlSearchGame;
              } else {
                  SearchControl = GetGameTypeControl;
              }     
              
              var searchGame = $(element).val();
              
              if (searchGame != null || searchGame != "") {
                  if ((SearchControl == ddlSearchGame ? games : gametypes) == null) {
                      
                      if (SearchControl == ddlSearchGame) {
                        games = new Array();
                      } else {
                        gametypes = new Array();
                      }
                    
                    // Save all data
                    $(SearchControl).find("option").each(function(i){
                      
                      if (SearchControl == ddlSearchGame) {
                        games[i] = new Array($(this).val(), $(this).text());
                      } else {
                        gametypes[i] = new Array($(this).val(), $(this).text());
                      }
                      
                    });
                  }
                  
                   
                   $(SearchControl).find("option").remove();
                   
                   $.each(
                     (SearchControl == ddlSearchGame ? games : gametypes),
                     function(intIndex, objValue) {
                        
                        if (objValue[1].replace("-", " ").replace(":", "").replace(" ", "").toLowerCase().indexOf(searchGame.replace("-", " ").replace(":", "").replace(" ", "").toLowerCase()) != -1) {
                            $(SearchControl).append($("<option value='" + objValue[0] + "' selected='selected'>" + objValue[1] + "</option>"));
                        }
                        
                     });
                     
                   if ($(SearchControl).find("option").size() == 0) {
                       $(SearchControl).append($("<option value='' selected='selected'>No results" + "</option>"));
                   }
                   

               }
               
                /*
                     Update gametypes by txtsearchgame
                     (new objects is selected in dropdownbox)
                */
                if ($(element).attr("id") == $(txtSearchGame).attr("id") && lastgamevalue != $(SearchControl).val()) {
                      lastgamevalue = $(SearchControl).val();
                      ChangeGameTypes($(SearchControl));
                }
             
         }
         
         var IsClearedGame = false;
         var IsClearedGameType = false;
         $(window).load(function () {


             $(aspxControlIds.txtFromTime).timepicker({ hourType: Boolean(aspxValues.timePickerHourType),
                 tooltipOnChoose: aspxValues.timePickerCloseText
             });

             $(".HyperMatch").hover(function () {
                 var eh = $(this);
                 eh.find(".PnlMatchInfoBox").css("background-color", "lightgreen");
                 eh.find(".LblFromDate").css("color", "yellow");
                 eh.find(".LblFromTo").css("color", "#000");
                 eh.find(".LblToDate").css("color", "yellow");
                 eh.find(".LblGameAndMode").css("color", "#5e5e5e");
                 eh.find(".LblNewestHead").css("color", "#000");
             },
               function () {
                   var eh = $(this);
                   eh.find(".PnlMatchInfoBox").css("background-color", "#ddd");
                   eh.find(".LblFromDate").css("color", "#4c6aaa");
                   eh.find(".LblFromTo").css("color", "#999");
                   eh.find(".LblToDate").css("color", "#4c6aaa");
                   eh.find(".LblGameAndMode").css("color", "#999");
                   eh.find(".LblNewestHead").css("color", "#616");
               });

         });

         function clearBox(element) {
              if (($(element).attr("id") == $(txtSearchGame).attr("id") ? IsClearedGame : IsClearedGameType) == false) {
                  
                  $(element).attr("value","");
                  
                  if ($(element).attr("id") == $(txtSearchGame).attr("id")) {
                    IsClearedGame = true;
                  } else {
                    IsClearedGameType = true;
                  }
                  
              }
         }
         
         function InsertDefaultText(element) {
              if (($(element).attr("id") == $(txtSearchGame).attr("id") ? IsClearedGame : IsClearedGameType) == true) {
                  
                  if ($(element).val() == "") {
                    $(element).attr("value", ($(element).attr("id") == $(txtSearchGame).attr("id") ? txtSearchGame_Text : txtSearchGameType_Text));
                    
                    if ($(element).attr("id") == $(txtSearchGame).attr("id")) {
                      IsClearedGame = false;
                    } else {
                      IsClearedGameType = false;
                    }
                  }
              }
         }
         /*
                     //#endregion
             search gamest - end
         */

         
         /*
                     //#region
             Add value to hiddenfields - start
         */
         var ddlClanCountry = aspxControlIds.ddlClanCountry;
         var ddlSearchCountry = aspxControlIds.ddlSearchCountry;
         var GetGameTypeControl = aspxControlIds.GetGameTypeControl;
         var hfClanCountry = aspxControlIds.hfClanCountry;
         var hfSearchCountry = aspxControlIds.hfSearchCountry;
         var hfSearchGameType = aspxControlIds.hfSearchGameType;
         function OnChangeDropdownbox(element) {
         
            var hiddenControl = null;
            if ($(element).attr("id") == $(ddlClanCountry).attr("id")) {
                hiddenControl = hfClanCountry;
            } else if ($(element).attr("id") == $(ddlSearchCountry).attr("id")) {
                hiddenControl = hfSearchCountry;
            } else if ($(element).attr("id") == $(GetGameTypeControl).attr("id")) {
                hiddenControl = hfSearchGameType;
            } else {
                hiddenControl = null;
            }
            
            if (hiddenControl != null) {
                
                $(hiddenControl).val($(element).find('option:selected').text());
                
            }

         
         }
          /*
                     //#endregion
             Add value to hiddenfieldst - end
         */
         
         
         /*
                     //#region
             Reading Countries after selecting Continent - start
         */
         function ChangeCountries(element) 
         {
              
              var value = $("OPTION:selected", element).val();
              var text = $("OPTION:selected", element).text();
              var ClanCountry = aspxControlIds.ClanCountry;
              var SearchCountry = aspxControlIds.SearchCountry;
              var DdlClanContinent = aspxControlIds.DdlClanContinent;
              var DdlSearchContinent = aspxControlIds.DdlSearchContinent;
              var GetCountryControlToChange = null;

              if ($(element).attr("id") == $(DdlClanContinent).attr("id")) {
                GetCountryControlToChange = ClanCountry;
              }

            if ($(element).attr("id") == $(DdlSearchContinent).attr("id")) {
                GetCountryControlToChange = SearchCountry;
              }
              var GetTc = $("#" + $(GetCountryControlToChange).parent().attr("id"));
              
              if (value == "" || value == null) {
                $(function() {
                  $(GetCountryControlToChange + ">option").remove();
                  $(GetCountryControlToChange).prepend("<option value='' selected='selected'>" + aspxValues.Choose + "</option>");
                  $(GetCountryControlToChange).attr("disabled","disabled");
                });
              } else {
              
              $(function() {
                  
                  GetTc.prepend(LoadingTag);
                  
                  $.ajax({
                    type: "GET",
	                  url: aspxValues.MainUrl + "xml.ashx",
	                  cache: false,
	                  contentType: "application/rss+xml; charset=utf-8",
	                  timeout: 3000,
	                  async: true, 
	                  data: "xml=c&ci=" + value + "&li=" + CurrentLangId,
	                  dataType: (jQuery.browser.msie) ? "text" : "xml",
	                  
                    error: function(XMLHttpRequest, textStatus, errorThrown) {


                        // speciel combobox fix
                        $(GetCountryControlToChange).attr("disabled", "disabled");
                        $("#" + $(GetCountryControlToChange).attr("id") + "input").attr("disabled", "disabled");
                        $("#" + $(GetCountryControlToChange).attr("id") + "input").val("");
                        $("#" + $(GetCountryControlToChange).attr("id") + "btn").attr("disabled", "disabled");
                        $("#" + $(GetCountryControlToChange).attr("id") + "btn").addClass("ui-button-disabled ui-state-disabled");
                             

                         GetTc.find(LoadingId).remove();
                    },
	                  
	                  success: function(xmlData) {
                            
                            xmlData = parseXml(xmlData);
                            
                            if (xmlData != null) {
                            
                           /*
                               Clear ClanCountry
                           */
                            
                            $(GetCountryControlToChange + ">option").remove();
                            
                            var theItem = null;
                            var theId = null;
	                          $(xmlData).find("c").each(function()
	                          {
		                          theCountry = $(this).text();
		                          theId = $(this).attr("i");
      		                    
		                          $(GetCountryControlToChange).append(
                                $("<option></option>").val(theId).html(theCountry)
                              );
                          
	                          });

	                          $(GetCountryControlToChange).prepend("<option value='' selected='selected'>" + aspxValues.Choose + "</option>");
	                          
                              // speciel combobox fix
	                          $("#" + $(GetCountryControlToChange).attr("id") + "input").removeAttr("disabled");
	                          $("#" + $(GetCountryControlToChange).attr("id") + "input").val("");
	                          $("#" + $(GetCountryControlToChange).attr("id") + "btn").removeClass("ui-state-disabled");
	                          $("#" + $(GetCountryControlToChange).attr("id") + "btn").removeClass("ui-button-disabled");
	                          $("#" + $(GetCountryControlToChange).attr("id") + "btn").removeAttr("disabled");
	                          $("#" + $(GetCountryControlToChange).attr("id") + "btn").hover(function () {
	                              $(this).addClass("ui-state-hover");
	                          });
	                          $("#" + $(GetCountryControlToChange).attr("id") + "btn").mouseleave(function () {
	                              $(this).removeClass("ui-state-hover");
	                          });

                            GetTc.find(LoadingId).remove();
   
                        } else {
      	                    GetTc.find(LoadingId).remove();
      	                }
                            
	                  }
                  }); 
              
              });
              }
         }
         
         /*
             Reading Countries after selecting Continent - END
             //#endregion
         */
         
         $(window).load(function() {
             
             /*
                         //#region
             Advanced Search On/Off - start
             */
             var PnlNewestBlog = aspxControlIds.PnlNewestBlog;
             var PnlInfoContent = aspxControlIds.PnlInfoContent;
             var PnlSearchContent = aspxControlIds.PnlSearchContent;
             var HyperAdvancedSearch = aspxControlIds.HyperAdvancedSearch;
             var AdvancedIsShow = false;
             var GetTrAdvanced = $(PnlSearchContent + " .TrAdvanced");
             var GetTrAdvancedChildrenth = GetTrAdvanced.children("th");
             var GetTrAdvancedChildrentd = GetTrAdvanced.children("td");
             var TypeOfBrower = jQuery.browser.msie;
             
             if (TypeOfBrower) { 
                GetTrAdvancedChildrentd.hide();
                GetTrAdvancedChildrenth.hide();
             }
             GetTrAdvanced.hide();
             
         
             $(HyperAdvancedSearch).click(function(event) {
              
               var HyperSearch = $(this);
              
               if (AdvancedIsShow == false) {
               
                   HyperSearch.text(aspxValues.HyperSearchSimpleLink);
                   AdvancedIsShow = true;
                   
                   if (TypeOfBrower) { 
                      GetTrAdvanced.css("display", "block");
                      GetTrAdvancedChildrentd.fadeIn("fast", function() {
                        $(this).css("display", "block");
                      });
                      GetTrAdvancedChildrenth.fadeIn("fast", function() {
                        $(this).css("display", "block");
                      });
                      
                      
                   } else {
                      GetTrAdvanced.fadeIn("fast", function() {
                        $(this).css("display", "table-row");
                      });
                   }
                   
                   $(PnlNewestBlog).fadeOut("fast", function() {
                      $(this).remove();
                      $(PnlInfoContent).append($(this).css("margin-top", "10px").fadeIn("slow"));
                   }); 
                     
                   
               } else {
               
                   HyperSearch.text(aspxValues.HyperAdvancedSearch);
                   AdvancedIsShow = false;
                   
                   if (TypeOfBrower) { 
                      GetTrAdvancedChildrentd.hide();
                      GetTrAdvancedChildrenth.hide();
                      GetTrAdvanced.hide();
                   } else {
                      GetTrAdvanced.fadeOut('fast');
                   }
                   
                   $(PnlNewestBlog).fadeOut('fast', function() {
                      $(this).remove();
                      $(PnlSearchContent).append($(this).css('margin-top', '0px').fadeIn('slow'));
                   });
                                      
               }
                   
             });
             /*
             Advanced Search On/Off - END
             //#endregion
             */
             
                          
             
         });
         
    //]]> 