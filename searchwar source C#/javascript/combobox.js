
	(function ($) {
	    var itsvalid = false;
	    $.widget("ui.combobox", {
	        // default options
	        options: {
	            width: "",
	            SpecielInBottom: "no",
	            ShowallIiems: "Show all items"
	        },
	        _create: function () {
	            var self = this,
                    SpecielInBottom = this.options.SpecielInBottom,
					select = $(this.element).css("display", "none"),
                    select2 = this.element,
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";


	            var input = $("<input id='" + select2.attr("id") + "input' />").css("float", "left").prop("disabled", $(select2).prop("disabled"))
					.insertAfter(select)
					.val(value).addClass("ddlspeciel").removeClass("ui-corner-all").css("width", this.options.width)
					.autocomplete({
					    delay: 2,
					    minLength: 0,
					    source: function (request, response) {

					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));

					        if (SpecielInBottom == "yes") {
					            var topheight = $(input.autocomplete("widget")).css("top").replace("px", "");
					            input.autocomplete("widget").css("top", (topheight - 147.5).toString() + "px").css("overflow-y", "scroll").css("height", "200px").css("z-index", "11");
					        }

					    },
					    select: function (event, ui) {

					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });

					        $("#" + select.attr("id") + "input").attr("value", $("#" + select.attr("id") + " option:selected").text());


					        $(select).change();

					    },
					    change: function (event, ui) {
					        if (!ui.item) {
					            itsvalid = false;
					            select.find("option").each(function () {
					                if ($.trim($(input).val()).toLowerCase() == $.trim($(this).text()).toLowerCase()) {
					                    $(this).selected = true;
					                    itsvalid = true;
					                }
					            });

					            if (itsvalid == false) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					            } else {
					                $(select).change();
					            }


					        }

					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left").attr("autocomplete", "off");

	            input.data("autocomplete")._renderItem = function (ul, item) {
	                $(ul).attr("id", select2.attr("id") + "showdropdown");

	                return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul)
	            };


	            $("<button id='" + select2.attr("id") + "btn' type='button' onclick='return false;'></button>").prop("disabled", $(select2).prop("disabled"))
					.attr("tabIndex", -1)
					.attr("title", this.options.ShowallIiems)
					.insertAfter(input)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon dllspecielbtn").css("margin-top", "2px").css("height", "20px").css("float", "left").css("display", "block")
					.click(function () {

					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return false;
					    }

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();

					    return false;
					}).mouseleave(function () {
					    $(this).removeClass("ui-state-focus");
					});



	        }
	    });
	})(jQuery);

         