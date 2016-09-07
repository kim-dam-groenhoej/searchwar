
//<![CDATA[ 
          var PnlViewClanChats = aspxMasterControlIds.PnlViewClanChats;
          var HyperClanChat = aspxMasterControlIds.HyperClanChat;

          $(document).ready(function () {

              // show main content
              $(aspxMasterControlIds.PnlJavscriptIsEnable).css("display", "block");

              $(aspxMasterControlIds.Aspnetform + " input:submit").button();
              $(aspxMasterControlIds.Aspnetform + " input:button").button();

              $(HyperClanChat).click(function (event) {

                  var height = 165;
                  if ($(PnlViewClanChats).css("height") == "0px") {
                      $(PnlViewClanChats).animate({
                          height: '+=' + height
                      }, 400);
                  } else {
                      $(PnlViewClanChats).animate({
                          height: '-=' + height
                      }, 400);
                  }

              });


          });  
         
    //]]> 