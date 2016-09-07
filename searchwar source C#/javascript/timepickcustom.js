

      var choosehoverbg = "#ddd";
      $(window).load(function () {

          jQuery.fn.timepicker = function (getoptions) {
              var defaults = {
                  tooltipOnChoose: "Click to close",
                  hourType: true
              };

              var options = $.extend(defaults, getoptions);


              var oldee = $(this[0]);
              var ee = $(this[0]).clone();
              var divaround = $("<span id='DivchooseTime" + ee.attr("id") + "'></span>");
              divaround.css("position", "relative");
              divaround.css("display", "block");
              divaround.css("height", "auto");
              divaround.css("width", "auto");
              divaround.html(ee);
              if (!$("#amorpm")) {
                  divaround.append("<span id='amorpm'></span>");
              }
              oldee.replaceWith(divaround);

              // add css
              ee.addClass("ui-helper-reset ui-corner-all ui-autocomplete-input ui-widget ui-widget-content divaround");


              var chooseid = $("<div id='chooseTime" + ee.attr("id") + "'></div>");
              chooseid.insertAfter(ee);
              chooseid.addClass("chooseid");

              // create div for all of it
              var divChooseAmOrPm = $("<div></div>");
              divChooseAmOrPm.attr("id", "ChooseAmPm");
              divChooseAmOrPm.addClass("divChooseAmOrPm");
              divChooseAmOrPm.appendTo(chooseid);

              // create timers content
              var divChooseHour = $("<div id='ChooseHour'></div>");
              divChooseHour.css("display", "none");
              divChooseHour.css("padding", "5px 0 5px 0");
              divChooseHour.css("overflow", "hidden");
              divChooseHour.css("position", "relative");
              divChooseHour.css("z-index", "3000");

              var divChooseHour2 = $("<div id='ChooseHour2'></div>");
              divChooseHour2.css("display", "none");
              divChooseHour2.css("padding", "5px 0 5px 0");
              divChooseHour2.css("overflow", "hidden");
              divChooseHour2.css("position", "relative");
              divChooseHour2.css("z-index", "3000");

              // create timers content
              var divChooseMinute = $("<div id='ChooseMinute'></div>");
              divChooseMinute.css("display", "none");
              divChooseMinute.css("padding", "0 0 5px 0");
              divChooseMinute.css("overflow", "hidden");
              divChooseMinute.css("position", "relative");
              divChooseMinute.css("z-index", "3000");


              divChooseHour.appendTo(chooseid);
              divChooseHour2.appendTo(chooseid);

              divChooseMinute.appendTo(chooseid);

              // create am
              var createAm = $("<a id='chooseaAm'><span class='ui-button-text'>am</span></a>");
              createAm.addClass("ui-button-text-only ui-button ui-widget ui-state-default ui-corner-all DivChoose");
              createAm.attr("nummargin", "0");
              showhourtime(divChooseHour, false, ee, chooseid, options.tooltipOnChoose, options.hourType);
              createAm.hover(function () {
                  divChooseMinute.hide();
                  divChooseHour2.hide();
                  $(divChooseHour2).parent().find(".DivChoose").removeClass("ui-state-hover");
                  $(divChooseMinute).parent().find(".DivChoose").removeClass("ui-state-hover");

                  // reset bgs
                  $(this).parent().find(".DivChoose").css("background", "#fff").removeClass("ui-state-hover"); ;

                  // hover change bg
                  $(this).addClass("ui-state-hover"); ;

                  // set am
                  $("#amorpm").html("am");

                  divChooseHour.css("margin-left", ($(this).attr("nummargin") * 33).toString() + "px");
                  divChooseHour.show();
              });

              // create pm
              var createPm = $("<a id='chooseaPm'><span class='ui-button-text'>pm</span></a>");
              createPm.addClass("ui-button-text-only ui-button ui-widget ui-state-default ui-corner-all DivChoose");
              createPm.attr("nummargin", "1");
              showhourtime(divChooseHour2, true, ee, chooseid, options.tooltipOnChoose, options.hourType);
              createPm.hover(function () {
                  divChooseMinute.hide();
                  $(divChooseHour).parent().find(".DivChoose").removeClass("ui-state-hover");
                  $(divChooseMinute).parent().find(".DivChoose").removeClass("ui-state-hover");
                  divChooseHour.hide();

                  // reset bgs
                  $(this).parent().find(".DivChoose").removeClass("ui-state-hover"); ;

                  // hover change bg
                  $(this).addClass("ui-state-hover"); ;

                  // set pm
                  $("#amorpm").html("pm");

                  divChooseHour2.css("margin-left", ($(this).attr("nummargin") * 33).toString() + "px");
                  divChooseHour2.show();
              });

              createAm.appendTo(divChooseAmOrPm);
              createPm.appendTo(divChooseAmOrPm);

              divChooseAmOrPm.find(".DivChoose").show();

              showminutetime(divChooseMinute, ee, chooseid, options.tooltipOnChoose);

              ee.hover(function () {

                  ee.focus();
                  chooseid.show();

              });

              divaround.mouseleave(function () {

                  divChooseMinute.hide();
                  divChooseHour.hide();
                  divChooseHour2.hide();


                  $(divChooseHour).parent().find(".DivChoose").removeClass("ui-state-hover");
                  $(divChooseHour2).parent().find(".DivChoose").removeClass("ui-state-hover");
                  $(divChooseMinute).parent().find(".DivChoose").removeClass("ui-state-hover");

                  chooseid.hide();
              });

              return ee;
          };

      });


      function showhourtime(createElementsTo, getamOrPm, getee, chooseID, tooltipOnChoose, hourType) {
            var divChooseMinute = chooseID.find("#ChooseMinute");

            var number = 0;
            var number2 = 12;
            if (getamOrPm == true) {
                if (Boolean(hourType) == true) {
                    number = 13;
                    number2 = 23;
                } else {
                    number = 1;
                    number2 = 11;
                }
            }

            var createTimeE;
            var nummargin = 0;
            for (i=number;i<=number2;i++)
            {
                var writeNumber = i.toString();
                if (writeNumber.length == 1) {
                    writeNumber = "0" + writeNumber;
                }

                createTimeE = $("<a href='javascript:slidedownonclick(\"#" + chooseID.attr("id") + "\")' title='" + tooltipOnChoose + "' id='choosea" + i.toString() + "'><span class='ui-button-text'>" + writeNumber + "</span></a>");
                createTimeE.addClass("ui-button-text-only ui-button ui-widget ui-state-default ui-corner-all DivChoose");
                createTimeE.attr("nummargin", (nummargin + (getamOrPm == true ? 1 : 0)).toString());
                createTimeE.hover(function() {
                    
                    getee.val($(this).find("span").html() + ":00");

                    // reset bgs
                    $(this).parent().find(".DivChoose").removeClass("ui-state-hover");

                    divChooseMinute.css("margin-left", ($(this).attr("nummargin") * 33).toString() + "px");

                    // hover change bg
                    $(this).addClass("ui-state-hover");

                    divChooseMinute.show();
                });

                createTimeE.appendTo(createElementsTo);
                nummargin = nummargin + 1;
            }


      }

      function showminutetime(createElementsTo2, getee2, chooseID, tooltipOnChoose) {

            var minnumber = 0;
            var minnumber2 = 50;

            var createTimeE2;
            for (ii=minnumber;ii<=minnumber2;ii = ii + 10)
            {
                var writeNumber2 = ii.toString();
                if (writeNumber2.length == 1) {
                    writeNumber2 = "0" + writeNumber2;
                }

                createTimeE2 = $("<a href='javascript:slidedownonclick(\"#" + chooseID.attr("id") + "\")' title='" + tooltipOnChoose + "' id='choosea" + ii.toString() + "'><span class='ui-button-text'>" + writeNumber2 + "</span></a>");
                createTimeE2.addClass("ui-button-text-only ui-button ui-widget ui-state-default ui-corner-all DivChoose");
                createTimeE2.hover(function() {
                    
                    getee2.val(getee2.val().split(':')[0] + ":" + $(this).find("span").html());

                    // reset bgs
                    $(this).parent().find(".DivChoose").removeClass("ui-state-hover");

                    // hover change bg
                    $(this).addClass("ui-state-hover");

                });

                createTimeE2.appendTo(createElementsTo2);

            }

      }

      function slidedownonclick(ee) {
          $(ee).slideUp('fast');
      }
         