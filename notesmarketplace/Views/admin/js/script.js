/* =================================
            Preloader
===================================*/

$(window).on('load', function () {
    //makes sure that whole site is loaded
    $('#status').fadeOut();
    $('#preloader').delay(350).fadeOut('slow');
});

/* =================================
            Navigation
===================================*/

$(function () {

    $(window).scroll(function () {

        if ($(window).scrollTop() > 50) {
            //show white nav
            $("#hm-nav nav").addClass("white-nav-top");

            // Show dark logo
            $("#hm-nav .navbar-brand img").attr("src", "img/home/logo.png")

        } else {
            // hide white nav
            $("#hm-nav nav").removeClass("white-nav-top");

            // Show normal logo
            $("#hm-nav .navbar-brand img").attr("src", "img/home/top-logo.png")
        }
    });

});

/* =================================
            Navigation
===================================*/

$(".toggle-password").click(function () {

    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

/* =================================
            Searched Books
===================================*/

$(function () {
    $("#searched-books").owlCarousel({
        items: 9,
        autoplay: false,
        nav: true,
        dots: true
    });
});

/*============================================
                Mobile Menu
============================================*/

$(function () {

    // Show mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });

    // Show mobile nav
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });
});