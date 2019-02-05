$(function () {

    'use strict';

	/* ==============================================
	** Navbar
	 ==============================================*/

    /* Fixed navbar -------------------------------*/
    if ($('.fixed-navbar').length) {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 0) {
                $('.fixed-navbar .top-navbar').addClass("fly");
                $('.fixed-navbar .content-container').css('margin-top', '54px');
            } else {
                $('.fixed-navbar .top-navbar').removeClass("fly");
                $('.fixed-navbar .content-container').css('margin-top', '0');
            }
        });
    }

    /* Fixed tabs -------------------------------*/
    if ($('.fixed-tabs').length) {
        var fn = $('.front-navigation').outerHeight(true),
            ft = $('.fixed-tabs .top-navbar.with-tabs .tabs-wrapper').outerHeight(true);

        $(window).scroll(function () {
            if ($(this).scrollTop() > fn) {
                $('.fixed-tabs .top-navbar.with-tabs .tabs-wrapper').addClass("fly");
                $('.fixed-tabs .content-container').css('margin-top', ft + 'px');
            } else {
                $('.fixed-tabs .top-navbar.with-tabs .tabs-wrapper').removeClass("fly");
                $('.fixed-tabs .content-container').css('margin-top', '0');
            }
        });
    }

    /* Show/hide search form ----------------------*/
    $('.show-search').on('click', function () {
        $('.searchbar').addClass('show');
        $('.front-navigation').addClass('hide');
        $('.searchbar input[type="text"]').focus();
    });
    $('.search-close').on('click', function () {
        $('.searchbar').removeClass('show');
        $('.front-navigation').removeClass('hide');
    });


	/* ==============================================
	** Sidebar
	 ==============================================*/

    $('.sidenav').sidenav();

    $('#main-menu li.has-sub > a').on('click', function () {
        $(this).removeAttr('href');
        var element = $(this).parent('li');
        if (element.hasClass('open')) {
            element.removeClass('open');
            element.find('li').removeClass('open');
            element.find('ul').slideUp();
        } else {
            element.addClass('open');
            element.children('ul').slideDown();
            element.siblings('li').children('ul').slideUp();
            element.siblings('li').removeClass('open');
            element.siblings('li').find('li').removeClass('open');
            element.siblings('li').find('ul').slideUp();
        }
    });

    $('#main-menu > ul > li.has-sub > a').append('<span class="arrow-icon ti-angle-right"></span>');
    $('#main-menu > ul > li.has-sub > ul > li.has-sub > a').append('<span class="fa fa-angle-double-right"></span>');
    /*------------------------------------------------------*/

	/* ==============================================
	** Slick Slider
	 ==============================================*/

    /* Main slider (index.html) ----------------------------*/
    $('.home-slider.flat').slick({
        dots: true,
        arrows: false,
        fade: true,
        autoplay: true,
        autoplaySpeed: 5000,
        speed: 2000,
    });

    /* News slider (index2.html) ---------------------------*/
    $('.home-slider.news').slick({
        dots: false,
        arrows: true,
        fade: false,
        autoplay: true,
        speed: 800,
        cssEase: 'linear',
        prevArrow: '<span class="prev-arr"><span class="ti-angle-left"></span></span>',
        nextArrow: '<span class="next-arr"><span class="ti-angle-right"></span></span>',
    });

    /* Shop slider (index3.html) ---------------------------*/
    $('.home-slider.shop').slick({
        dots: true,
        arrows: false,
        fade: false,
        autoplay: true,
        autoplaySpeed: 5000,
        speed: 800,
        cssEase: 'linear',
    });

    /* Blog gallery -------------------------------*/
    $('.thumb-gallery').slick({
        dots: true,
        arrows: true,
        autoplay: true,
        speed: 500,
        autoplaySpeed: 4000,
        cssEase: 'linear',
        prevArrow: '<span class="prev-arr"><span class="ti-angle-left"></span></span>',
        nextArrow: '<span class="next-arr"><span class="ti-angle-right"></span></span>',
    });
    /*------------------------------------------------------*/

    // Product (thumb) slider
    $('.home-featured-products').slick({
        infinite: false,
        speed: 500,
        cssEase: 'linear',
        slidesToShow: 2,
        slidesToScroll: 1,
        prevArrow: '<span class="prev-arr"><span class="ti-angle-left"></span></span>',
        nextArrow: '<span class="next-arr"><span class="ti-angle-right"></span></span>',
    });

	/* ==============================================
	** Material Components
	 ==============================================*/

    /* Accordion ----------------------------------*/
    $('.collapsible').collapsible();
    $('.collapsible.expandable').collapsible({
        accordion: false
    });

    /* Modal --------------------------------------*/
    $('.modal').modal();

    /* Dropdown -----------------------------------*/
    $('.dropdown-trigger').dropdown();

    /* Form select --------------------------------*/
    $('select').formSelect();

    /* Date & time pickers ------------------------*/
    $('.datepicker').datepicker();
    $('.timepicker').timepicker();

    /* Chips --------------------------------------*/
    $('.chips').chips();
    $('.chips-initial').chips({
        data: [{
            tag: 'Apple',
        }, {
            tag: 'Microsoft',
        }, {
            tag: 'Google',
        }],
    });
    $('.chips-placeholder').chips({
        placeholder: 'Enter a tag',
        secondaryPlaceholder: '+Tag',
    });
    $('.chips-autocomplete').chips({
        autocompleteOptions: {
            data: {
                'Apple': null,
                'Microsoft': null,
                'Google': null
            },
            limit: Infinity,
            minLength: 1
        }
    });

    /* Tabs ---------------------------------------*/
    $('ul.tabs').tabs();

    $('.fixed-action-btn').floatingActionButton({
        direction: 'left',
        hoverEnabled: false
    });

	/* ==============================================
	** CounterUp Number (4 cards at the top)
	 ==============================================*/
    $('.count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
                duration: 3000,
                easing: 'swing',
                step: function (now) {
                    $(this).text(Math.ceil(now));
                    $(this).text($(this).text().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
            });
    });
    /* ------------------------------------------------ */


	/* ==============================================
	** Portfolio mixItUp
	 ==============================================*/
    $('#mix-wrapper').mixItUp({
        load: {
            sort: 'order:asc'
        },
        selectors: {
            target: '.mix-target',
            filter: '.filter-btn',
            sort: '.sort-btn'
        },
        animation: {
            animateChangeLayout: true,
            animateResizeTargets: true
        },
        layout: {
            containerClass: 'portfolio-grid'
        },
        callbacks: {
            onMixEnd: function (state) {
            }
        }
    });

    $('.port-list').on('click', function () {
        $('.port-list').addClass('active');
        $('.port-grid').removeClass('active');
        $('#mix-wrapper').mixItUp('changeLayout', {
            containerClass: 'portfolio-list'
        });
        return false;
    });
    $('.port-grid').on('click', function () {
        $('.port-list').removeClass('active');
        $('.port-grid').addClass('active');
        $('#mix-wrapper').mixItUp('changeLayout', {
            containerClass: 'portfolio-grid'
        });
        return false;
    });

	/* ==============================================
	** Swipebox
	 ==============================================*/
    $(document).swipebox({ selector: '.swipebox' });

	/* ==============================================
	** Scroll top
	 ==============================================*/
    var winScroll = $(window).scrollTop();
    if (winScroll > 1) {
        $('#to-top').css({ bottom: "0" });
    } else {
        $('#to-top').css({ bottom: "0" });
    }
    $(window).on("scroll", function () {
        winScroll = $(window).scrollTop();

        if (winScroll > 1) {
            $('#to-top').css({ opacity: 1, bottom: "30px" });
        } else {
            $('#to-top').css({ opacity: 0, bottom: "-30px" });
        }
    });
    $('#to-top').on("click", function () {
        $('html, body').animate({ scrollTop: '0px' }, 800);
        return false;
    });
    /*------------------------------------------------------*/

    $(".location-search").click(function (e) {
        e.preventDefault();

        getLocation();
    });

    //function that gets the location and returns it
    function getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(useLocation, errorLocation);
        } else {
            alert("Tarayýcýnýzýn konum desteði bulunmuyor.");
        }
    }

    function errorLocation(error) {
        var constant;
        var message = "";
        switch (error.code) {
            case error.PERMISSION_DENIED:
                constant = "PERMISSION_DENIED";
                message = "Konum eriþimine izin vermeniz gerekmektedir.";
                break;
            case error.POSITION_UNAVAILABLE:
                constant = "POSITION_UNAVAILABLE";
                message = "Konum kullanýlamýyor.";
                break;
            case error.TIMEOUT:
                constant = "TIMEOUT";
                message = "Konum bulma zaman aþýmýna uðradý.";
                break;
            default:
                constant = "Unrecognized error";
                break;
        }

        if (!message)
            message = error.message;

        alert("Error code: " + error.code + "\nConstant: " + constant + "\nMessage: " + message);
    }

    function useLocation(position) {
        window.location = "/home/search" +
            '?lon=' +
            position.coords.longitude +
            '&lat=' +
            position.coords.latitude;
    };

    //function that retrieves the position
    function showPosition(position) {
        var location = {
            longitude: position.coords.longitude,
            latitude: position.coords.latitude
        }
        console.log(location);
    }


});