
//<![CDATA[
$(document).ready(function() {
    jQuery.fn.center = function() {
        $(this[0]).css("position", "absolute");
        $(this[0]).css("top", ($(window).height() - $(this[0]).height()) / 2 + $(window).scrollTop() + "px");
        $(this[0]).css("left", ($(window).width() - $(this[0]).width()) / 2 + $(window).scrollLeft() + "px");
        return $(this[0]);
    };
});
    //]]> 