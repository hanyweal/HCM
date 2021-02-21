
$.fn.center = function () {
    $(this).css({
        position: "absolute",
        left: ($(window).width() - $(this).outerWidth()) / 2,
        top: ($(window).height() - $(this).outerHeight()) / 2
    });
};
